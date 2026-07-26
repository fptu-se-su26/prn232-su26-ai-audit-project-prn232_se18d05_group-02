using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.GuideTours;

namespace WanderXServer.Services;

public class GuideTourService : IGuideTourService
{
    private const string AssignedStatus = "Assigned";
    private const string ConfirmedStatus = "Confirmed";
    private const string DeclinedStatus = "Declined";
    private const string FinishedStatus = "Finished";

    private readonly WanderXDbContext _dbContext;

    public GuideTourService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GuideTourAssignmentResponse>> GetAllAsync(string? guideEmail, string? search)
    {
        var assignments = _dbContext.GuideTourAssignments
            .Include(item => item.GuideProfile)
            .ThenInclude(item => item.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(guideEmail))
        {
            var normalizedEmail = AuthService.NormalizeEmail(guideEmail);
            assignments = assignments.Where(item => item.GuideProfile.User.NormalizedEmail == normalizedEmail);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            assignments = assignments.Where(item =>
                item.TourCode.Contains(term) ||
                item.TourName.Contains(term) ||
                item.Destination.Contains(term) ||
                item.Region.Contains(term) ||
                item.GuideProfile.User.FullName.Contains(term) ||
                item.GuideProfile.User.Email.Contains(term));
        }

        var result = await assignments
            .OrderBy(item => item.StartDate)
            .ThenBy(item => item.TourCode)
            .ToListAsync();

        await RefreshGuideAvailabilityForAssignmentsAsync(result);
        await _dbContext.SaveChangesAsync();

        return result.Select(ToResponse).ToList();
    }

    public async Task<IReadOnlyList<GuideTourAssignmentResponse>> GetScheduleAsync(string guideEmail)
    {
        var guide = await FindGuideByEmailAsync(guideEmail);
        await RefreshGuideAvailabilityAsync(guide);

        var assignments = await _dbContext.GuideTourAssignments
            .Include(item => item.GuideProfile)
            .ThenInclude(item => item.User)
            .Where(item => item.GuideProfileId == guide.Id)
            .OrderBy(item => item.StartDate)
            .ToListAsync();

        return assignments.Select(ToResponse).ToList();
    }

    public async Task<GuideTourAssignmentResponse> GetByIdAsync(Guid id)
    {
        var assignment = await FindAssignmentAsync(id);
        await RefreshGuideAvailabilityAsync(assignment.GuideProfile);
        return ToResponse(assignment);
    }

    public async Task<GuideTourAssignmentResponse> CreateAsync(CreateGuideTourAssignmentRequest request)
    {
        ValidateAssignmentDates(request.StartDate, request.EndDate);
        ValidateAssignmentStatus(request.Status);

        var guide = await FindGuideByIdAsync(request.GuideProfileId);
        var tourCode = request.TourCode.Trim();
        var startDate = request.StartDate.Date;

        var activeAssignmentExists = await _dbContext.GuideTourAssignments.AnyAsync(item =>
            item.TourCode == tourCode &&
            item.StartDate.Date == startDate &&
            item.Status != DeclinedStatus);

        if (activeAssignmentExists)
        {
            throw new InvalidOperationException($"An active tour assignment for {tourCode} on {startDate:dd/MM/yyyy} already exists.");
        }

        await EnsureGuideIsAvailableAsync(
            guide.Id,
            request.StartDate.Date,
            request.EndDate.Date,
            request.Status);

        var assignment = new GuideTourAssignment
        {
            GuideProfileId = guide.Id,
            GuideProfile = guide,
            TourCode = tourCode,
            TourName = request.TourName.Trim(),
            Destination = request.Destination.Trim(),
            Region = request.Region.Trim(),
            StartDate = request.StartDate.Date,
            EndDate = request.EndDate.Date,
            TravelerCount = request.TravelerCount,
            MeetingPoint = request.MeetingPoint.Trim(),
            Status = request.Status.Trim(),
            ItinerarySummary = request.ItinerarySummary.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.GuideTourAssignments.Add(assignment);
        await _dbContext.SaveChangesAsync();
        await RefreshGuideAvailabilityAsync(guide);
        await _dbContext.SaveChangesAsync();

        return ToResponse(assignment);
    }

    public async Task<GuideTourAssignmentResponse> ChangeGuideAsync(Guid id, ChangeGuideAssignmentRequest request)
    {
        var assignment = await FindAssignmentAsync(id);
        var previousGuide = assignment.GuideProfile;
        var nextGuide = await FindGuideByIdAsync(request.GuideProfileId);

        if (assignment.GuideProfileId == nextGuide.Id)
        {
            return ToResponse(assignment);
        }

        await EnsureGuideIsAvailableAsync(
            nextGuide.Id,
            assignment.StartDate.Date,
            assignment.EndDate.Date,
            assignment.Status,
            assignment.Id);

        assignment.GuideProfileId = nextGuide.Id;
        assignment.GuideProfile = nextGuide;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        await RefreshGuideAvailabilityAsync(previousGuide);
        await RefreshGuideAvailabilityAsync(nextGuide);
        await _dbContext.SaveChangesAsync();

        return ToResponse(assignment);
    }

    public async Task<GuideTourAssignmentResponse> DeclineAsync(Guid id, DeclineTourRequest request)
    {
        var assignment = await FindAssignmentAsync(id);

        if (!CanDecline(assignment.Status))
        {
            throw new InvalidOperationException("Only assigned or confirmed tours can be declined.");
        }

        assignment.Status = DeclinedStatus;
        assignment.DeclineReason = request.Reason.Trim();
        assignment.DeclinedAt = DateTime.UtcNow;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        await RefreshGuideAvailabilityAsync(assignment.GuideProfile);
        await _dbContext.SaveChangesAsync();

        return ToResponse(assignment);
    }

    public async Task<GuideTourAssignmentResponse> ConfirmAsync(Guid id)
    {
        var assignment = await FindAssignmentAsync(id);

        if (!string.Equals(assignment.Status, AssignedStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only assigned tours can be confirmed.");
        }

        assignment.Status = ConfirmedStatus;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        await RefreshGuideAvailabilityAsync(assignment.GuideProfile);
        await _dbContext.SaveChangesAsync();

        return ToResponse(assignment);
    }

    public async Task<GuideTourAssignmentResponse> FinishAsync(Guid id, FinishTourRequest request)
    {
        var assignment = await FindAssignmentAsync(id);

        if (!string.Equals(assignment.Status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Tour status can only be updated from Confirmed to Finished.");
        }

        if (DateTime.Today < assignment.EndDate.Date)
        {
            throw new InvalidOperationException($"Tour cannot be marked finished until the last day of the tour ({assignment.EndDate:dd MMM yyyy}).");
        }

        assignment.Status = FinishedStatus;
        assignment.EvidenceImage = request.EvidenceImage;
        assignment.FinishedAt = DateTime.UtcNow;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        await RefreshGuideAvailabilityAsync(assignment.GuideProfile);
        await _dbContext.SaveChangesAsync();

        return ToResponse(assignment);
    }

    private async Task<GuideProfile> FindGuideByEmailAsync(string email)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        var guide = await _dbContext.GuideProfiles
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.User.NormalizedEmail == normalizedEmail);

        if (guide is null)
        {
            throw new KeyNotFoundException("Guide profile was not found.");
        }

        return guide;
    }

    private async Task<GuideProfile> FindGuideByIdAsync(Guid id)
    {
        var guide = await _dbContext.GuideProfiles
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (guide is null)
        {
            throw new KeyNotFoundException("Guide profile was not found.");
        }

        return guide;
    }

    private async Task<GuideTourAssignment> FindAssignmentAsync(Guid id)
    {
        var assignment = await _dbContext.GuideTourAssignments
            .Include(item => item.GuideProfile)
            .ThenInclude(item => item.User)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (assignment is null)
        {
            throw new KeyNotFoundException("Tour assignment was not found.");
        }

        return assignment;
    }

    private async Task RefreshGuideAvailabilityAsync(GuideProfile guide)
    {
        var today = DateTime.Today;
        var hasCurrentConfirmedTour = await _dbContext.GuideTourAssignments.AnyAsync(item =>
            item.GuideProfileId == guide.Id &&
            item.Status == ConfirmedStatus &&
            item.StartDate.Date <= today &&
            item.EndDate.Date >= today);

        guide.Status = hasCurrentConfirmedTour ? "On Tour" : "Active";
        guide.UpdatedAt = DateTime.UtcNow;
    }

    private async Task RefreshGuideAvailabilityForAssignmentsAsync(IEnumerable<GuideTourAssignment> assignments)
    {
        var guides = assignments
            .Select(item => item.GuideProfile)
            .DistinctBy(item => item.Id)
            .ToList();

        foreach (var guide in guides)
        {
            await RefreshGuideAvailabilityAsync(guide);
        }
    }

    private static void ValidateAssignmentDates(DateTime startDate, DateTime endDate)
    {
        if (endDate.Date < startDate.Date)
        {
            throw new InvalidOperationException("End date must be on or after start date.");
        }
    }

    private static void ValidateAssignmentStatus(string status)
    {
        var allowedStatuses = new[] { AssignedStatus, ConfirmedStatus, DeclinedStatus, FinishedStatus };

        if (!allowedStatuses.Contains(status.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Assignment status is not supported.");
        }
    }

    private async Task EnsureGuideIsAvailableAsync(
        Guid guideProfileId,
        DateTime startDate,
        DateTime endDate,
        string targetStatus,
        Guid? excludedAssignmentId = null)
    {
        if (!BlocksGuideAvailability(targetStatus))
        {
            return;
        }

        var conflictingAssignment = await _dbContext.GuideTourAssignments
            .AsNoTracking()
            .Where(item =>
                item.GuideProfileId == guideProfileId &&
                item.Id != excludedAssignmentId &&
                (item.Status == AssignedStatus || item.Status == ConfirmedStatus) &&
                item.StartDate.Date <= endDate.Date &&
                item.EndDate.Date >= startDate.Date)
            .OrderBy(item => item.StartDate)
            .FirstOrDefaultAsync();

        if (conflictingAssignment is null)
        {
            return;
        }

        throw new InvalidOperationException(
            $"This guide is already assigned to {conflictingAssignment.TourCode} ({conflictingAssignment.StartDate:dd MMM yyyy} - {conflictingAssignment.EndDate:dd MMM yyyy}).");
    }

    private static bool BlocksGuideAvailability(string status)
    {
        return string.Equals(status, AssignedStatus, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase);
    }

    private static GuideTourAssignmentResponse ToResponse(GuideTourAssignment assignment)
    {
        var today = DateTime.Today;
        var isConfirmed = string.Equals(assignment.Status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase);

        return new GuideTourAssignmentResponse
        {
            Id = assignment.Id,
            GuideProfileId = assignment.GuideProfileId,
            GuideName = assignment.GuideProfile.User.FullName,
            GuideEmail = assignment.GuideProfile.User.Email,
            TourCode = assignment.TourCode,
            TourName = assignment.TourName,
            Destination = assignment.Destination,
            Region = assignment.Region,
            StartDate = assignment.StartDate,
            EndDate = assignment.EndDate,
            TravelerCount = assignment.TravelerCount,
            MeetingPoint = assignment.MeetingPoint,
            Status = assignment.Status,
            ItinerarySummary = assignment.ItinerarySummary,
            DeclineReason = assignment.DeclineReason,
            DeclinedAt = assignment.DeclinedAt,
            FinishedAt = assignment.FinishedAt,
            EvidenceImage = assignment.EvidenceImage,
            IsCurrentBusyTour = IsCurrentBusyTour(assignment),
            CanConfirm = string.Equals(assignment.Status, AssignedStatus, StringComparison.OrdinalIgnoreCase),
            CanDecline = CanDecline(assignment.Status),
            CanFinish = isConfirmed && today >= assignment.EndDate.Date
        };
    }

    private static bool CanDecline(string status)
    {
        return string.Equals(status, AssignedStatus, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsCurrentBusyTour(GuideTourAssignment assignment)
    {
        var today = DateTime.Today;
        return string.Equals(assignment.Status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase) &&
            assignment.StartDate.Date <= today &&
            assignment.EndDate.Date >= today;
    }

    public async Task<IReadOnlyList<UnassignedBookedTourResponse>> GetUnassignedBookedToursAsync()
    {
        _dbContext.EnsureBookingStorage();
        _dbContext.EnsureTourStorage();

        var validStatuses = new[] { "Paid", "Confirmed", "Approved" };
        var activeBookings = await _dbContext.Bookings
            .AsNoTracking()
            .Where(item => item.TourCode != null && item.TourCode != "" && validStatuses.Contains(item.Status))
            .ToListAsync();

        if (activeBookings.Count == 0)
        {
            return Array.Empty<UnassignedBookedTourResponse>();
        }

        var bookedGroups = activeBookings
            .GroupBy(item => new
            {
                TourCode = item.TourCode!.Trim().ToUpperInvariant(),
                DepartureDate = item.DepartureDate.Date
            })
            .Select(group => new
            {
                group.Key.TourCode,
                group.Key.DepartureDate,
                TourName = group.First().TourName,
                Destination = group.First().Destination,
                GuestCount = group.Sum(item => item.GuestCount),
                BookingCount = group.Count()
            })
            .ToList();

        var assignedPairs = await _dbContext.GuideTourAssignments
            .AsNoTracking()
            .Where(item => item.Status != DeclinedStatus)
            .Select(item => new
            {
                TourCode = item.TourCode.Trim().ToUpperInvariant(),
                StartDate = item.StartDate.Date
            })
            .ToListAsync();

        var assignedSet = assignedPairs
            .Select(item => $"{item.TourCode}_{item.StartDate:yyyyMMdd}")
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var unassignedGroups = bookedGroups
            .Where(group => !assignedSet.Contains($"{group.TourCode}_{group.DepartureDate:yyyyMMdd}"))
            .OrderBy(group => group.DepartureDate)
            .ThenBy(group => group.TourCode)
            .ToList();

        if (unassignedGroups.Count == 0)
        {
            return Array.Empty<UnassignedBookedTourResponse>();
        }

        var tourCodes = unassignedGroups.Select(item => item.TourCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var tours = await _dbContext.Tours
            .AsNoTracking()
            .Where(item => tourCodes.Contains(item.Code))
            .ToDictionaryAsync(item => item.Code, StringComparer.OrdinalIgnoreCase);

        return unassignedGroups.Select(item =>
        {
            tours.TryGetValue(item.TourCode, out var tour);
            var durationDays = tour?.DurationDays ?? 1;
            var endDate = item.DepartureDate.AddDays(Math.Max(0, durationDays - 1));
            var region = tour?.Region ?? "Central Vietnam";
            var meetingPoint = $"Central Meeting Station, {item.Destination}";
            var summary = tour?.Description ?? $"Standard itinerary for {item.TourName}.";

            return new UnassignedBookedTourResponse
            {
                TourCode = item.TourCode,
                TourName = tour?.Name ?? item.TourName,
                Destination = tour?.Destination ?? item.Destination,
                Region = region,
                DepartureDate = item.DepartureDate,
                EndDate = endDate,
                GuestCount = item.GuestCount,
                BookingCount = item.BookingCount,
                DefaultMeetingPoint = meetingPoint,
                DefaultSummary = summary
            };
        }).ToList();
    }
}
