using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Tours;

namespace WanderXServer.Services;

public class TourService : ITourService
{
    private static readonly string[] AllowedStatuses = { "Draft", "Published", "Archived", "Hidden" };

    private readonly WanderXDbContext _dbContext;

    public TourService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TourResponse>> GetAllAsync(string? search, string? status)
    {
        _dbContext.EnsureTourStorage();

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

        var result = await tours
            .OrderBy(item => item.Name)
            .ThenBy(item => item.Code)
            .ToListAsync();

        return result.Select(ToResponse).ToList();
    }

    public async Task<TourResponse> GetByIdAsync(Guid id)
    {
        _dbContext.EnsureTourStorage();

        return ToResponse(await FindTourAsync(id));
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
        tour.ImageUrl = request.ImageUrl.Trim();
        tour.Description = request.Description.Trim();
        tour.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

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

    private static TourResponse ToResponse(Tour tour)
    {
        return new TourResponse
        {
            Id = tour.Id,
            Code = tour.Code,
            Name = tour.Name,
            Destination = tour.Destination,
            Region = tour.Region,
            DurationDays = tour.DurationDays,
            Price = tour.Price,
            Capacity = tour.Capacity,
            Status = tour.Status,
            ImageUrl = tour.ImageUrl,
            Description = tour.Description,
            CreatedAt = tour.CreatedAt,
            UpdatedAt = tour.UpdatedAt
        };
    }
}
