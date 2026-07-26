using Microsoft.EntityFrameworkCore;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.BookedTours;

namespace WanderXServer.Services;

public sealed class BookedTourService : IBookedTourService
{
    private readonly WanderXDbContext _dbContext;

    public BookedTourService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<BookedTourResponse>> GetBookedToursAsync(BookedTourFilterRequest filter)
    {
        _dbContext.EnsureBookingStorage();
        _dbContext.EnsureTourStorage();

        var bookings = _dbContext.Bookings.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            bookings = bookings.Where(item =>
                item.BookingCode.Contains(term) ||
                item.TourName.Contains(term) ||
                item.Destination.Contains(term) ||
                (item.TourCode != null && item.TourCode.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status) &&
            !filter.Status.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            var status = filter.Status.Trim();
            bookings = bookings.Where(item => item.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(filter.Destination))
        {
            var destination = filter.Destination.Trim();
            bookings = bookings.Where(item => item.Destination.Contains(destination));
        }

        if (filter.DepartureDate.HasValue)
        {
            var date = filter.DepartureDate.Value.Date;
            bookings = bookings.Where(item => item.DepartureDate.Date == date);
        }

        var bookedRows = await bookings
            .GroupBy(item => new
            {
                TourCode = item.TourCode ?? string.Empty,
                item.TourName,
                item.Destination,
                DepartureDate = item.DepartureDate.Date,
                item.Status
            })
            .Select(group => new
            {
                group.Key.TourCode,
                group.Key.TourName,
                group.Key.Destination,
                group.Key.DepartureDate,
                BookingStatus = group.Key.Status,
                BookingCount = group.Count(),
                GuestCount = group.Sum(item => item.GuestCount),
                TotalAmount = group.Sum(item => item.TotalAmount)
            })
            .OrderBy(item => item.DepartureDate)
            .ThenBy(item => item.TourName)
            .ToListAsync();

        var tourCodes = bookedRows
            .Select(item => item.TourCode)
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var tours = await _dbContext.Tours
            .AsNoTracking()
            .Where(item => tourCodes.Contains(item.Code))
            .ToDictionaryAsync(item => item.Code, StringComparer.OrdinalIgnoreCase);

        return bookedRows.Select(item =>
        {
            tours.TryGetValue(item.TourCode, out var tour);

            return new BookedTourResponse
            {
                TourCode = item.TourCode,
                TourName = tour?.Name ?? item.TourName,
                Destination = tour?.Destination ?? item.Destination,
                DepartureDate = item.DepartureDate,
                BookingStatus = item.BookingStatus,
                TourId = tour?.Id,
                TourStatus = tour?.Status ?? "Unmapped",
                BookingCount = item.BookingCount,
                GuestCount = item.GuestCount,
                TotalAmount = item.TotalAmount,
                CanDeleteOrHide = false
            };
        }).ToList();
    }
}
