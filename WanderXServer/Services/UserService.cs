using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Users;

namespace WanderXServer.Services;

public class UserService : IUserService
{
    private readonly WanderXDbContext _dbContext;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

    public UserService(WanderXDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfileResponse> GetProfileAsync(string email)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        return new UserProfileResponse
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(string email, UpdateProfileRequest request)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        user.FullName = request.FullName.Trim();
        user.PhoneNumber = request.PhoneNumber.Trim();
        user.Address = request.Address?.Trim();
        
        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            if (string.IsNullOrEmpty(request.CurrentPassword))
            {
                throw new InvalidOperationException("Current password is required to change password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid current password.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return new UserProfileResponse
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }
// TV3
    public async Task<IEnumerable<BookingSummaryResponse>> GetBookingsAsync(string email)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var bookings = await _dbContext.Bookings
            .Where(b => b.UserId == user.Id)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BookingSummaryResponse
            {
                Id = b.Id,
                BookingCode = b.BookingCode,
                TourName = b.TourName,
                Destination = b.Destination,
                ThumbnailUrl = b.ThumbnailUrl,
                DepartureDate = b.DepartureDate,
                GuestCount = b.GuestCount,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                CreatedAt = b.CreatedAt,
                PaidAt = b.PaidAt,
                ConfirmedAt = b.ConfirmedAt,
                CompletedAt = b.CompletedAt,
                ApprovedAt = b.ApprovedAt,
                RequestFailedAt = b.RequestFailedAt,
                PaymentFailedAt = b.PaymentFailedAt,
                ConfirmationFailedAt = b.ConfirmationFailedAt,
                CompletionFailedAt = b.CompletionFailedAt
            })
            .ToListAsync();

        return bookings;
    }

    public async Task<BookingSummaryResponse?> GetBookingByIdAsync(Guid id)
    {
        var b = await _dbContext.Bookings
            .Include(bk => bk.Passengers)
            .FirstOrDefaultAsync(bk => bk.Id == id);
            
        if (b == null) return null;

        return new BookingSummaryResponse
        {
            Id = b.Id,
            BookingCode = b.BookingCode,
            TourName = b.TourName,
            Destination = b.Destination,
            ThumbnailUrl = b.ThumbnailUrl,
            DepartureDate = b.DepartureDate,
            GuestCount = b.GuestCount,
            TotalAmount = b.TotalAmount,
            Status = b.Status,
            CreatedAt = b.CreatedAt,
            PaidAt = b.PaidAt,
            ConfirmedAt = b.ConfirmedAt,
            CompletedAt = b.CompletedAt,
            ApprovedAt = b.ApprovedAt,
            RequestFailedAt = b.RequestFailedAt,
            PaymentFailedAt = b.PaymentFailedAt,
            ConfirmationFailedAt = b.ConfirmationFailedAt,
            CompletionFailedAt = b.CompletionFailedAt,
            Passengers = b.Passengers.Select(p => new BookingPassengerDto
            {
                Id = p.Id,
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                TicketType = p.TicketType
            }).ToList()
        };
    }

    public async Task<BookingSummaryResponse?> CancelBookingAsync(Guid id)
    {
        var booking = await _dbContext.Bookings
            .Include(bk => bk.Passengers)
            .FirstOrDefaultAsync(bk => bk.Id == id);

        if (booking == null)
        {
            return null;
        }

        if (booking.Status == "Cancelled")
        {
            throw new InvalidOperationException("Booking is already cancelled.");
        }

        booking.Status = "Cancelled";
        booking.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new BookingSummaryResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            TourName = booking.TourName,
            Destination = booking.Destination,
            ThumbnailUrl = booking.ThumbnailUrl,
            DepartureDate = booking.DepartureDate,
            GuestCount = booking.GuestCount,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt,
            PaidAt = booking.PaidAt,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            ApprovedAt = booking.ApprovedAt,
            RequestFailedAt = booking.RequestFailedAt,
            PaymentFailedAt = booking.PaymentFailedAt,
            ConfirmationFailedAt = booking.ConfirmationFailedAt,
            CompletionFailedAt = booking.CompletionFailedAt,
            Passengers = booking.Passengers.Select(p => new BookingPassengerDto
            {
                Id = p.Id,
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                TicketType = p.TicketType
            }).ToList()
        };
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
    }

}
// end TV3