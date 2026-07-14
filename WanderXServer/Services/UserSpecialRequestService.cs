using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.UserSpecialRequests;

namespace WanderXServer.Services;

public class UserSpecialRequestService
{
    private readonly WanderXDbContext _dbContext;

    public UserSpecialRequestService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<UserSpecialRequestResponse>> GetByBookingIdAsync(Guid bookingId)
    {
        var requests = await _dbContext.UserSpecialRequests
            .Include(r => r.Booking)
            .Where(r => r.BookingId == bookingId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return requests.Select(ToResponse).ToList();
    }

    public async Task<IReadOnlyList<UserSpecialRequestResponse>> GetByUserIdAsync(Guid userId)
    {
        var requests = await _dbContext.UserSpecialRequests
            .Include(r => r.Booking)
            .Where(r => r.Booking.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return requests.Select(ToResponse).ToList();
    }

    // TV3 - Admin: get all bookings that have special requests, with request counts
    public async Task<IReadOnlyList<BookingWithRequestCountResponse>> GetAllBookingsWithRequestCountsAsync()
    {
        var bookingsWithRequests = await _dbContext.UserSpecialRequests
            .Include(r => r.Booking)
                .ThenInclude(b => b.User)
            .GroupBy(r => r.BookingId)
            .Select(g => new BookingWithRequestCountResponse
            {
                BookingId = g.Key,
                BookingCode = g.First().Booking!.BookingCode,
                TourName = g.First().Booking!.TourName,
                Destination = g.First().Booking!.Destination,
                DepartureDate = g.First().Booking!.DepartureDate,
                BookingStatus = g.First().Booking!.Status,
                CreatedAt = g.First().Booking!.CreatedAt,
                CustomerName = g.First().Booking!.User!.FullName,
                CustomerEmail = g.First().Booking!.User!.Email,
                TotalRequests = g.Count(),
                PendingRequests = g.Count(r => r.Status == "Pending"),
                ApprovedRequests = g.Count(r => r.Status == "Approved"),
                RejectedRequests = g.Count(r => r.Status == "Rejected")
            })
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookingsWithRequests;
    }
    // end TV3


    public async Task<UserSpecialRequestResponse> GetByIdAsync(Guid id)
    {
        var request = await _dbContext.UserSpecialRequests
            .Include(r => r.Booking)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request is null)
        {
            throw new KeyNotFoundException("Special request was not found.");
        }

        return ToResponse(request);
    }

    public async Task<UserSpecialRequestResponse> CreateAsync(CreateUserSpecialRequestRequest request)
    {
        var booking = await _dbContext.Bookings.FindAsync(request.BookingId);
        if (booking is null)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        // Check if booking is in a state that allows special requests
        if (booking.PaidAt.HasValue)
        {
            throw new InvalidOperationException("Special requests can only be added before payment is completed.");
        }

        var specialRequest = new UserSpecialRequest
        {
            BookingId = request.BookingId,
            Description = request.Description.Trim(),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.UserSpecialRequests.Add(specialRequest);
        await _dbContext.SaveChangesAsync();

        specialRequest.Booking = booking;
        return ToResponse(specialRequest);
    }

    public async Task<UserSpecialRequestResponse> UpdateAsync(Guid id, UpdateUserSpecialRequestRequest request)
    {
        var specialRequest = await _dbContext.UserSpecialRequests.FindAsync(id);
        if (specialRequest is null)
        {
            throw new KeyNotFoundException("Special request was not found.");
        }

        var allowedStatuses = new[] { "Pending", "Approved", "Rejected", "Completed" };
        if (!allowedStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Invalid status.");
        }

        specialRequest.Status = request.Status.Trim();
        specialRequest.AdminResponse = request.AdminResponse?.Trim();
        specialRequest.UpdatedAt = DateTime.UtcNow;

        if (!string.Equals(specialRequest.Status, "Pending", StringComparison.OrdinalIgnoreCase))
        {
            specialRequest.ReviewedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        var updatedRequest = await _dbContext.UserSpecialRequests
            .Include(r => r.Booking)
            .FirstOrDefaultAsync(r => r.Id == id);

        return ToResponse(updatedRequest!);
    }

    public async Task DeleteAsync(Guid id)
    {
        var specialRequest = await _dbContext.UserSpecialRequests.FindAsync(id);
        if (specialRequest is null)
        {
            throw new KeyNotFoundException("Special request was not found.");
        }

        // Only allow deletion if status is Pending
        if (!string.Equals(specialRequest.Status, "Pending", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only pending requests can be deleted.");
        }

        _dbContext.UserSpecialRequests.Remove(specialRequest);
        await _dbContext.SaveChangesAsync();
    }

    private static UserSpecialRequestResponse ToResponse(UserSpecialRequest request)
    {
        return new UserSpecialRequestResponse
        {
            Id = request.Id,
            BookingId = request.BookingId,
            BookingCode = request.Booking?.BookingCode ?? string.Empty,
            TourName = request.Booking?.TourName ?? string.Empty,
            Description = request.Description,
            Status = request.Status,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            ReviewedAt = request.ReviewedAt,
            AdminResponse = request.AdminResponse
        };
    }
}
