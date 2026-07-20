using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Users;

namespace WanderXServer.Services;

public class UserService : IUserService
{
    private readonly WanderXDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<UserService> _logger;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

    public UserService(WanderXDbContext dbContext, IEmailSender emailSender, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
        _logger = logger;
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
                TourCode = b.TourCode,
                DepartureScheduleId = b.DepartureScheduleId,
                TourName = b.TourName,
                Destination = b.Destination,
                ThumbnailUrl = b.ThumbnailUrl,
                DepartureDate = b.DepartureDate,
                GuestCount = b.GuestCount,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                CancellationStatus = b.CancellationStatus,
                CancellationReason = b.CancellationReason,
                CancellationRequestedAt = b.CancellationRequestedAt,
                CancellationReviewedAt = b.CancellationReviewedAt,
                CancellationReviewedBy = b.CancellationReviewedBy,
                CancellationReviewNote = b.CancellationReviewNote,
                CreatedAt = b.CreatedAt,
                PaidAt = b.PaidAt,
                ConfirmedAt = b.ConfirmedAt,
                CompletedAt = b.CompletedAt,
                ApprovedAt = b.ApprovedAt,
                RequestFailedAt = b.RequestFailedAt,
                PaymentFailedAt = b.PaymentFailedAt,
                ConfirmationFailedAt = b.ConfirmationFailedAt,
                CompletionFailedAt = b.CompletionFailedAt,
                PaymentOption = b.PaymentOption,
                PaymentMethod = b.PaymentMethod,
                PaymentReference = b.PaymentReference,
                PaidAmount = b.PaidAmount,
                RemainingAmount = b.RemainingAmount,
                PaymentUpdatedAt = b.PaymentUpdatedAt,
                PaymentUpdatedBy = b.PaymentUpdatedBy,
                PaymentNote = b.PaymentNote
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
            TourCode = b.TourCode,
            DepartureScheduleId = b.DepartureScheduleId,
            TourName = b.TourName,
            Destination = b.Destination,
            ThumbnailUrl = b.ThumbnailUrl,
            DepartureDate = b.DepartureDate,
            GuestCount = b.GuestCount,
            TotalAmount = b.TotalAmount,
            Status = b.Status,
            CancellationStatus = b.CancellationStatus,
            CancellationReason = b.CancellationReason,
            CancellationRequestedAt = b.CancellationRequestedAt,
            CancellationReviewedAt = b.CancellationReviewedAt,
            CancellationReviewedBy = b.CancellationReviewedBy,
            CancellationReviewNote = b.CancellationReviewNote,
            CreatedAt = b.CreatedAt,
            PaidAt = b.PaidAt,
            ConfirmedAt = b.ConfirmedAt,
            CompletedAt = b.CompletedAt,
            ApprovedAt = b.ApprovedAt,
            RequestFailedAt = b.RequestFailedAt,
            PaymentFailedAt = b.PaymentFailedAt,
            ConfirmationFailedAt = b.ConfirmationFailedAt,
            CompletionFailedAt = b.CompletionFailedAt,
            PaymentOption = b.PaymentOption,
            PaymentMethod = b.PaymentMethod,
            PaymentReference = b.PaymentReference,
            PaidAmount = b.PaidAmount,
            RemainingAmount = b.RemainingAmount,
            PaymentUpdatedAt = b.PaymentUpdatedAt,
            PaymentUpdatedBy = b.PaymentUpdatedBy,
            PaymentNote = b.PaymentNote,
            Passengers = b.Passengers.Select(p => new BookingPassengerDto
            {
                Id = p.Id,
                FullName = p.FullName,
                PhoneNumber = p.PhoneNumber,
                TicketType = p.TicketType
            }).ToList()
        };
    }

    public async Task<BookingSummaryResponse?> CancelBookingAsync(Guid id, CreateCancellationRequest request)
    {
        var booking = await _dbContext.Bookings
            .Include(bk => bk.User)
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

        if (booking.Status == "Finished")
        {
            throw new InvalidOperationException("Finished bookings cannot be cancelled.");
        }

        if (string.Equals(booking.CancellationStatus, "Pending", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("A cancellation request is already pending review.");
        }

        var requestedAt = DateTime.UtcNow;
        booking.CancellationStatus = "Pending";
        booking.CancellationReason = string.IsNullOrWhiteSpace(request.Reason)
            ? null
            : request.Reason.Trim();
        booking.CancellationRequestedAt = requestedAt;
        booking.CancellationReviewedAt = null;
        booking.CancellationReviewedBy = null;
        booking.CancellationReviewNote = null;
        booking.UpdatedAt = requestedAt;

        await _dbContext.SaveChangesAsync();
        await SendCancellationRequestReceivedEmailAsync(booking);

        return new BookingSummaryResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            TourCode = booking.TourCode,
            DepartureScheduleId = booking.DepartureScheduleId,
            TourName = booking.TourName,
            Destination = booking.Destination,
            ThumbnailUrl = booking.ThumbnailUrl,
            DepartureDate = booking.DepartureDate,
            GuestCount = booking.GuestCount,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            CancellationStatus = booking.CancellationStatus,
            CancellationReason = booking.CancellationReason,
            CancellationRequestedAt = booking.CancellationRequestedAt,
            CancellationReviewedAt = booking.CancellationReviewedAt,
            CancellationReviewedBy = booking.CancellationReviewedBy,
            CancellationReviewNote = booking.CancellationReviewNote,
            CreatedAt = booking.CreatedAt,
            PaidAt = booking.PaidAt,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            ApprovedAt = booking.ApprovedAt,
            RequestFailedAt = booking.RequestFailedAt,
            PaymentFailedAt = booking.PaymentFailedAt,
            ConfirmationFailedAt = booking.ConfirmationFailedAt,
            CompletionFailedAt = booking.CompletionFailedAt,
            PaymentOption = booking.PaymentOption,
            PaymentMethod = booking.PaymentMethod,
            PaymentReference = booking.PaymentReference,
            PaidAmount = booking.PaidAmount,
            RemainingAmount = booking.RemainingAmount,
            PaymentUpdatedAt = booking.PaymentUpdatedAt,
            PaymentUpdatedBy = booking.PaymentUpdatedBy,
            PaymentNote = booking.PaymentNote,
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

    private async Task SendCancellationRequestReceivedEmailAsync(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.User.Email))
        {
            return;
        }

        var subject = $"WanderX cancellation request received - {booking.BookingCode}";
        var reason = string.IsNullOrWhiteSpace(booking.CancellationReason)
            ? "No reason provided."
            : booking.CancellationReason;

        var body = $"""
Hello {booking.User.FullName},

We have received your cancellation request for booking {booking.BookingCode}.

Tour: {booking.TourName}
Destination: {booking.Destination}
Departure date: {booking.DepartureDate:dd/MM/yyyy}
Reason: {reason}

Your request is now pending staff review. We will notify you after it has been approved or rejected.

WanderX Team
""";

        try
        {
            await _emailSender.SendAsync(booking.User.Email, subject, body);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Cancellation request received email failed. BookingCode={BookingCode}; To={ToEmail}",
                booking.BookingCode,
                booking.User.Email);
        }
    }

}
// end TV3
