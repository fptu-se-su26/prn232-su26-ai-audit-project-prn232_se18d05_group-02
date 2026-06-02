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

    public async Task<GuideTourAssignmentResponse> FinishAsync(Guid id)
    {
        var assignment = await FindAssignmentAsync(id);

        if (!string.Equals(assignment.Status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Tour status can only be updated from Confirmed to Finished.");
        }

        assignment.Status = FinishedStatus;
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

    private static GuideTourAssignmentResponse ToResponse(GuideTourAssignment assignment)
    {
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
            IsCurrentBusyTour = IsCurrentBusyTour(assignment),
            CanDecline = CanDecline(assignment.Status),
            CanFinish = string.Equals(assignment.Status, ConfirmedStatus, StringComparison.OrdinalIgnoreCase)
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
}
