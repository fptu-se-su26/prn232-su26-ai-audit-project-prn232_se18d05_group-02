using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Tours;

namespace WanderXServer.Services;

public class TourService : ITourService
{
    private const string PublishedStatus = "Published";
    private const string LockedStatus = "Locked";
    private const string FullLockReason = "Full";
    private const string DepartedLockReason = "Departed";
    private const string ManualLockReason = "Manual";
    private static readonly string[] AllowedStatuses = { "Draft", PublishedStatus, "Archived", "Hidden", LockedStatus };
    private static readonly string[] CapacityHoldingBookingStatuses = { "Paid", "Confirmed", "Finished" };
    private static readonly string[] DepartureBlockingAssignmentStatuses = { "Assigned", "Confirmed" };

    private readonly WanderXDbContext _dbContext;

    public TourService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TourResponse>> GetAllAsync(string? search, string? status, string? destination, DateTime? departureDate)
    {
        _dbContext.EnsureTourStorage();
        _dbContext.EnsureTourPricingStorage();

        var tours = _dbContext.Tours.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            tours = tours.Where(item =>
                item.Code.Contains(term) ||
                item.Name.Contains(term) ||
                item.Destination.Contains(term) ||
                item.Region.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status) && !status.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            tours = tours.Where(item => item.Status == status.Trim());
        }

        if (!string.IsNullOrWhiteSpace(destination))
        {
            var destinationTerm = destination.Trim();
            tours = tours.Where(item => item.Destination.Contains(destinationTerm));
        }

        if (departureDate.HasValue)
        {
            var date = departureDate.Value.Date;
            tours = tours.Where(tour =>
                _dbContext.Bookings.Any(booking =>
                    booking.TourCode == tour.Code &&
                    booking.DepartureDate.Date == date));
        }

        var result = await tours
            .OrderBy(item => item.Name)
            .ThenBy(item => item.Code)
            .ToListAsync();

        foreach (var tour in result)
        {
            await RefreshTourAvailabilityAsync(tour.Code);
        }

        result = await tours
            .OrderBy(item => item.Name)
            .ThenBy(item => item.Code)
            .ToListAsync();

        var pricing = await GetPricingSnapshotsAsync(result);
        return result.Select(tour => ToResponse(tour, pricing.GetValueOrDefault(tour.Id))).ToList();
    }

    public async Task<TourResponse> GetByIdAsync(Guid id)
    {
        _dbContext.EnsureTourStorage();
        _dbContext.EnsureTourPricingStorage();

        var tour = await FindTourAsync(id);
        return ToResponse(tour, await GetPricingSnapshotAsync(tour));
    }

    public async Task<TourResponse> CreateAsync(CreateTourRequest request)
    {
        _dbContext.EnsureTourStorage();

        ValidateStatus(request.Status);
        var code = NormalizeCode(request.Code);
        ValidateTourCode(code);
        ValidateTourDetails(
            request.Name,
            request.Destination,
            request.Region,
            request.DurationDays,
            request.Price,
            request.Capacity,
            request.ImageUrl,
            request.Description);

        if (await _dbContext.Tours.AnyAsync(item => item.Code == code))
        {
            throw new InvalidOperationException("A tour with this code already exists.");
        }

        var tour = new Tour
        {
            Code = code,
            Name = request.Name.Trim(),
            Destination = request.Destination.Trim(),
            Region = request.Region.Trim(),
            ScheduleTourId = await GetNextScheduleTourIdAsync(),
            DurationDays = request.DurationDays,
            Price = request.Price,
            Capacity = request.Capacity,
            Status = request.Status.Trim(),
            LockReason = ResolveLockReason(request.Status, null),
            ImageUrl = request.ImageUrl.Trim(),
            Description = request.Description.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Tours.Add(tour);
        await _dbContext.SaveChangesAsync();

        return ToResponse(tour);
    }

    public async Task<TourResponse> UpdateAsync(Guid id, UpdateTourRequest request)
    {
        _dbContext.EnsureTourStorage();

        ValidateStatus(request.Status);
        var tour = await FindTourAsync(id);
        var code = NormalizeCode(request.Code);
        ValidateTourCode(code);
        ValidateTourDetails(
            request.Name,
            request.Destination,
            request.Region,
            request.DurationDays,
            request.Price,
            request.Capacity,
            request.ImageUrl,
            request.Description);

        if (await _dbContext.Tours.AnyAsync(item => item.Id != id && item.Code == code))
        {
            throw new InvalidOperationException("A tour with this code already exists.");
        }

        tour.Code = code;
        tour.Name = request.Name.Trim();
        tour.Destination = request.Destination.Trim();
        tour.Region = request.Region.Trim();
        tour.DurationDays = request.DurationDays;
        tour.Price = request.Price;
        tour.Capacity = request.Capacity;
        tour.Status = request.Status.Trim();
        tour.LockReason = ResolveLockReason(request.Status, tour.LockReason);
        tour.ImageUrl = request.ImageUrl.Trim();
        tour.Description = request.Description.Trim();
        tour.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        await RefreshTourAvailabilityAsync(tour.Code);

        return ToResponse(tour);
    }

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.EnsureTourStorage();

        var tour = await FindTourAsync(id);
        await EnsureTourHasNoBookingsAsync(tour);

        _dbContext.Tours.Remove(tour);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TourResponse> HideAsync(Guid id)
    {
        _dbContext.EnsureTourStorage();

        var tour = await FindTourAsync(id);
        await EnsureTourHasNoBookingsAsync(tour);

        tour.Status = "Hidden";
        tour.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return ToResponse(tour);
    }

    public async Task<TourResponse> LockAsync(Guid id)
    {
        _dbContext.EnsureTourStorage();

        var tour = await FindTourAsync(id);
        tour.Status = LockedStatus;
        tour.LockReason = ManualLockReason;
        tour.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return ToResponse(tour);
    }

    public async Task<TourResponse> UnlockAsync(Guid id)
    {
        _dbContext.EnsureTourStorage();

        var tour = await FindTourAsync(id);
        var lockReason = await GetAutomaticLockReasonAsync(tour);

        if (lockReason is not null)
        {
            throw new InvalidOperationException(lockReason == FullLockReason
                ? "This tour is full and cannot be unlocked."
                : "This tour has already departed and cannot be unlocked.");
        }

        tour.Status = PublishedStatus;
        tour.LockReason = null;
        tour.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return ToResponse(tour);
    }

    public async Task EnsureTourCanAcceptBookingAsync(string? tourCode, int requestedGuestCount, DateTime departureDate, Guid? excludedBookingId = null)
    {
        if (string.IsNullOrWhiteSpace(tourCode))
        {
            return;
        }

        _dbContext.EnsureTourStorage();
        var code = NormalizeCode(tourCode);
        var tour = await _dbContext.Tours.FirstOrDefaultAsync(item => item.Code == code);
        if (tour is null)
        {
            return;
        }

        await RefreshTourAvailabilityAsync(code);
        await _dbContext.Entry(tour).ReloadAsync();

        if (string.Equals(tour.Status, LockedStatus, StringComparison.OrdinalIgnoreCase))
        {
            var canRecheckCapacityForExistingBooking = excludedBookingId.HasValue &&
                string.Equals(tour.LockReason, FullLockReason, StringComparison.OrdinalIgnoreCase);

            if (!canRecheckCapacityForExistingBooking)
            {
                throw new InvalidOperationException(ToLockedBookingMessage(tour.LockReason));
            }
        }

        if (!string.Equals(tour.Status, PublishedStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("This tour is not open for booking.");
        }

        if (departureDate.Date <= DateTime.UtcNow.Date)
        {
            throw new InvalidOperationException("This tour has already reached its departure date.");
        }

        var bookedGuests = await GetBookedGuestCountAsync(tour.Code, excludedBookingId);
        if (bookedGuests + requestedGuestCount > tour.Capacity)
        {
            throw new InvalidOperationException($"Only {Math.Max(0, tour.Capacity - bookedGuests)} seat(s) are available for this tour.");
        }
    }

    public async Task RefreshTourAvailabilityAsync(string? tourCode)
    {
        if (string.IsNullOrWhiteSpace(tourCode))
        {
            return;
        }

        _dbContext.EnsureTourStorage();
        var code = NormalizeCode(tourCode);
        var tour = await _dbContext.Tours.FirstOrDefaultAsync(item => item.Code == code);
        if (tour is null)
        {
            return;
        }

        if (string.Equals(tour.LockReason, ManualLockReason, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var lockReason = await GetAutomaticLockReasonAsync(tour);
        if (lockReason is not null)
        {
            tour.Status = LockedStatus;
            tour.LockReason = lockReason;
            tour.UpdatedAt = DateTime.UtcNow;
        }
        else if (string.Equals(tour.Status, LockedStatus, StringComparison.OrdinalIgnoreCase))
        {
            tour.Status = PublishedStatus;
            tour.LockReason = null;
            tour.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task<Tour> FindTourAsync(Guid id)
    {
        var tour = await _dbContext.Tours.FirstOrDefaultAsync(item => item.Id == id);

        if (tour is null)
        {
            throw new KeyNotFoundException("Tour was not found.");
        }

        return tour;
    }

    private static void ValidateStatus(string status)
    {
        if (!AllowedStatuses.Contains(status.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Tour status is not supported.");
        }
    }

    private async Task EnsureTourHasNoBookingsAsync(Tour tour)
    {
        var hasBookings = await _dbContext.Bookings
            .AnyAsync(item => item.TourCode != null && item.TourCode == tour.Code);

        if (hasBookings)
        {
            throw new InvalidOperationException("This tour already has bookings. It cannot be deleted or hidden.");
        }
    }

    private async Task<string?> GetAutomaticLockReasonAsync(Tour tour)
    {
        var today = DateTime.UtcNow.Date;
        var departureDate = await GetEarliestDepartureDateAsync(tour.Code);
        if (departureDate.HasValue && departureDate.Value.Date <= today)
        {
            return DepartedLockReason;
        }

        var bookedGuests = await GetBookedGuestCountAsync(tour.Code);
        if (bookedGuests >= tour.Capacity)
        {
            return FullLockReason;
        }

        return null;
    }

    private async Task<DateTime?> GetEarliestDepartureDateAsync(string tourCode)
    {
        var bookingDeparture = await _dbContext.Bookings
            .AsNoTracking()
            .Where(booking =>
                booking.TourCode == tourCode &&
                CapacityHoldingBookingStatuses.Contains(booking.Status))
            .MinAsync(booking => (DateTime?)booking.DepartureDate);

        if (bookingDeparture.HasValue)
        {
            return bookingDeparture.Value.Date;
        }

        return await _dbContext.GuideTourAssignments
            .AsNoTracking()
            .Where(assignment =>
                assignment.TourCode == tourCode &&
                DepartureBlockingAssignmentStatuses.Contains(assignment.Status))
            .MinAsync(assignment => (DateTime?)assignment.StartDate);
    }

    private async Task<int> GetBookedGuestCountAsync(string tourCode, Guid? excludedBookingId = null)
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .Where(booking =>
                booking.TourCode == tourCode &&
                booking.Id != excludedBookingId &&
                CapacityHoldingBookingStatuses.Contains(booking.Status))
            .SumAsync(booking => (int?)booking.GuestCount) ?? 0;
    }

    private static string? ResolveLockReason(string status, string? currentLockReason)
    {
        return status.Trim().Equals(LockedStatus, StringComparison.OrdinalIgnoreCase)
            ? currentLockReason ?? ManualLockReason
            : null;
    }

    private static string ToLockedBookingMessage(string? lockReason)
    {
        return lockReason switch
        {
            FullLockReason => "This tour is full and cannot accept more bookings.",
            DepartedLockReason => "This tour has already reached its departure date.",
            ManualLockReason => "This tour is locked by admin staff.",
            _ => "This tour is locked and cannot accept bookings."
        };
    }

    private static void ValidateTourCode(string code)
    {
        if (!Regex.IsMatch(code, "^WX-[A-Z]{2,20}-\\d{3}$", RegexOptions.IgnoreCase))
        {
            throw new InvalidOperationException("Tour code must follow WX-LOCATION-001, for example WX-HUE-226.");
        }
    }

    private static void ValidateTourDetails(
        string name,
        string destination,
        string region,
        int durationDays,
        decimal price,
        int capacity,
        string imageUrl,
        string description)
    {
        if (name.Trim().Length < 4)
        {
            throw new InvalidOperationException("Tour name must be at least 4 characters.");
        }

        if (ContainsDigit(name))
        {
            throw new InvalidOperationException("Tour name cannot contain numbers.");
        }

        if (destination.Trim().Length < 2)
        {
            throw new InvalidOperationException("Destination must be at least 2 characters.");
        }

        if (ContainsDigit(destination))
        {
            throw new InvalidOperationException("Destination cannot contain numbers.");
        }

        if (region.Trim().Length < 2)
        {
            throw new InvalidOperationException("Region must be at least 2 characters.");
        }

        if (durationDays is < 1 or > 30)
        {
            throw new InvalidOperationException("Duration must be between 1 and 30 days.");
        }

        if (price <= 0)
        {
            throw new InvalidOperationException("Price must be greater than 0.");
        }

        if (capacity is < 1 or > 100)
        {
            throw new InvalidOperationException("Capacity must be between 1 and 100.");
        }

        if (!IsValidHttpUrl(imageUrl))
        {
            throw new InvalidOperationException("Tour image must be a valid http or https URL.");
        }

        if (description.Trim().Length < 20)
        {
            throw new InvalidOperationException("Description must be at least 20 characters.");
        }
    }

    private static bool IsValidHttpUrl(string value)
    {
        return Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private static bool ContainsDigit(string value)
    {
        return value.Any(char.IsDigit);
    }

    private static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }

    private async Task<int> GetNextScheduleTourIdAsync()
    {
        var maxId = await _dbContext.Tours.MaxAsync(tour => (int?)tour.ScheduleTourId) ?? 0;
        return maxId + 1;
    }

    private async Task<Dictionary<Guid, TourPricingSnapshot>> GetPricingSnapshotsAsync(IReadOnlyCollection<Tour> tours)
    {
        var tourIds = tours.Select(tour => tour.Id).ToList();
        var date = DateTime.UtcNow.Date;

        var seasonPrices = await _dbContext.TourSeasonPrices
            .AsNoTracking()
            .Where(item => tourIds.Contains(item.TourId) &&
                item.IsActive &&
                item.StartDate.Date <= date &&
                item.EndDate.Date >= date)
            .ToListAsync();

        var promotions = await _dbContext.TourPromotions
            .AsNoTracking()
            .Where(item => tourIds.Contains(item.TourId) &&
                item.IsActive &&
                item.StartDate.Date <= date &&
                item.EndDate.Date >= date)
            .ToListAsync();

        return tours.ToDictionary(
            tour => tour.Id,
            tour =>
            {
                var appliedSeason = seasonPrices
                    .Where(item => item.TourId == tour.Id)
                    .OrderByDescending(item => item.Price)
                    .FirstOrDefault();

                var priceBeforeDiscount = appliedSeason?.Price ?? tour.Price;
                var appliedPromotion = promotions
                    .Where(item => item.TourId == tour.Id)
                    .OrderByDescending(item => CalculateDiscount(priceBeforeDiscount, item))
                    .FirstOrDefault();

                return CreatePricingSnapshot(priceBeforeDiscount, appliedPromotion);
            });
    }

    private async Task<TourPricingSnapshot> GetPricingSnapshotAsync(Tour tour)
    {
        var date = DateTime.UtcNow.Date;
        var appliedSeason = await _dbContext.TourSeasonPrices
            .AsNoTracking()
            .Where(item => item.TourId == tour.Id &&
                item.IsActive &&
                item.StartDate.Date <= date &&
                item.EndDate.Date >= date)
            .OrderByDescending(item => item.Price)
            .FirstOrDefaultAsync();

        var priceBeforeDiscount = appliedSeason?.Price ?? tour.Price;
        var promotions = await _dbContext.TourPromotions
            .AsNoTracking()
            .Where(item => item.TourId == tour.Id &&
                item.IsActive &&
                item.StartDate.Date <= date &&
                item.EndDate.Date >= date)
            .ToListAsync();

        var appliedPromotion = promotions
            .OrderByDescending(item => CalculateDiscount(priceBeforeDiscount, item))
            .FirstOrDefault();

        return CreatePricingSnapshot(priceBeforeDiscount, appliedPromotion);
    }

    private static TourPricingSnapshot CreatePricingSnapshot(decimal priceBeforeDiscount, TourPromotion? promotion)
    {
        var discountAmount = promotion is null ? 0 : CalculateDiscount(priceBeforeDiscount, promotion);
        var effectivePrice = Math.Max(0, priceBeforeDiscount - discountAmount);
        var discountPercent = priceBeforeDiscount <= 0 || discountAmount <= 0
            ? 0
            : (int)Math.Round(discountAmount / priceBeforeDiscount * 100, MidpointRounding.AwayFromZero);

        return new TourPricingSnapshot(
            priceBeforeDiscount,
            effectivePrice,
            discountAmount,
            discountPercent,
            promotion?.Name);
    }

    private static decimal CalculateDiscount(decimal price, TourPromotion promotion)
    {
        var discount = promotion.DiscountType.Equals("Percent", StringComparison.OrdinalIgnoreCase)
            ? price * promotion.DiscountValue / 100
            : promotion.DiscountValue;

        return Math.Min(price, discount);
    }

    private static TourResponse ToResponse(Tour tour, TourPricingSnapshot? pricing = null)
    {
        var resolvedPricing = pricing ?? new TourPricingSnapshot(tour.Price, tour.Price, 0, 0, null);

        return new TourResponse
        {
            Id = tour.Id,
            Code = tour.Code,
            Name = tour.Name,
            Destination = tour.Destination,
            Region = tour.Region,
            DurationDays = tour.DurationDays,
            Price = tour.Price,
            OriginalPrice = resolvedPricing.OriginalPrice,
            EffectivePrice = resolvedPricing.EffectivePrice,
            DiscountAmount = resolvedPricing.DiscountAmount,
            DiscountPercent = resolvedPricing.DiscountPercent,
            AppliedPromotionName = resolvedPricing.AppliedPromotionName,
            Capacity = tour.Capacity,
            Status = tour.Status,
            LockReason = tour.LockReason,
            ImageUrl = tour.ImageUrl,
            Description = tour.Description,
            CreatedAt = tour.CreatedAt,
            UpdatedAt = tour.UpdatedAt
        };
    }

    private sealed record TourPricingSnapshot(
        decimal OriginalPrice,
        decimal EffectivePrice,
        decimal DiscountAmount,
        int DiscountPercent,
        string? AppliedPromotionName);
}
