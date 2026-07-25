using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Bookings;
using WanderXServer.Dtos.Users;

namespace WanderXServer.Services;

public class BookingService : IBookingService
{
    private const string PendingStatus = "Pending";
    private const string PaidStatus = "Paid";
    private const string ConfirmedStatus = "Confirmed";
    private const string FinishedStatus = "Finished";
    private const string CancelledStatus = "Cancelled";
    private const string CancellationPendingStatus = "Pending";
    private const string CancellationApprovedStatus = "Approved";
    private const string CancellationRejectedStatus = "Rejected";
    private const string FullPaymentOption = "Full";
    private const string DepositPaymentOption = "Deposit40";
    private const string BalancePaymentOption = "Balance";
    private static readonly HashSet<string> AllowedTicketTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Adult",
        "Child"
    };

    private readonly WanderXDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<BookingService> _logger;
    private readonly ITourService _tourService;

    public BookingService(WanderXDbContext dbContext, IEmailSender emailSender, ILogger<BookingService> logger, ITourService tourService)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
        _logger = logger;
        _tourService = tourService;
    }

    public async Task<IReadOnlyList<BookingResponse>> GetAllAsync(string? status, string? search)
    {
        var bookings = _dbContext.Bookings
            .Include(booking => booking.User)
            .Include(booking => booking.Passengers)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            var normalizedStatus = status.Trim();
            bookings = bookings.Where(booking => booking.Status == normalizedStatus);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            bookings = bookings.Where(booking =>
                booking.BookingCode.Contains(term) ||
                booking.TourName.Contains(term) ||
                booking.Destination.Contains(term) ||
                (booking.TourCode != null && booking.TourCode.Contains(term)) ||
                booking.User.FullName.Contains(term) ||
                booking.User.Email.Contains(term));
        }

        var result = await bookings
            .OrderByDescending(booking => booking.CreatedAt)
            .ToListAsync();

        return result.Select(ToResponse).ToList();
    }

    public async Task<BookingResponse> GetByIdAsync(Guid id)
    {
        var booking = await FindBookingAsync(id);
        return ToResponse(booking);
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var user = await FindUserAsync(request.UserId, request.UserEmail);
        var schedule = request.DepartureScheduleId.HasValue
            ? await FindScheduleAsync(request.DepartureScheduleId, request.TourCode)
            : null;

        var tourCode = ResolveTourCode(request.TourCode, schedule);
        var departureDate = ResolveDepartureDate(request.DepartureDate, schedule);
        await _tourService.EnsureTourCanAcceptBookingAsync(tourCode, request.GuestCount, departureDate);

        var booking = new Booking
        {
            UserId = user.Id,
            User = user,
            BookingCode = await GenerateBookingCodeAsync(),
            TourCode = tourCode,
            DepartureScheduleId = schedule?.Id ?? request.DepartureScheduleId,
            TourName = ResolveRequiredText(request.TourName, schedule?.TourName, "Tour name is required."),
            Destination = ResolveRequiredText(request.Destination, schedule?.Destination, "Destination is required."),
            ThumbnailUrl = request.ThumbnailUrl?.Trim(),
            DepartureDate = departureDate,
            GuestCount = request.GuestCount,
            TotalAmount = request.TotalAmount,
            Status = PendingStatus,
            ApprovedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        AddPassengerRows(booking, request.Passengers);

        _dbContext.Bookings.Add(booking);
        await _dbContext.SaveChangesAsync();
        await _tourService.RefreshTourAvailabilityAsync(booking.TourCode);

        return ToResponse(booking);
    }

    public async Task<BookingResponse> UpdateAsync(Guid id, UpdateBookingRequest request)
    {
        var booking = await _dbContext.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (booking is null)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        EnsureBookingCanBeEdited(booking);

        var schedule = request.DepartureScheduleId.HasValue
            ? await FindScheduleAsync(request.DepartureScheduleId, request.TourCode)
            : null;

        var tourCode = ResolveTourCode(request.TourCode, schedule);
        var departureScheduleId = schedule?.Id ?? request.DepartureScheduleId;
        var tourName = ResolveRequiredText(request.TourName, schedule?.TourName, "Tour name is required.");
        var destination = ResolveRequiredText(request.Destination, schedule?.Destination, "Destination is required.");
        var thumbnailUrl = request.ThumbnailUrl?.Trim();
        var departureDate = ResolveDepartureDate(request.DepartureDate, schedule);
        var updatedAt = DateTime.UtcNow;
        await _tourService.EnsureTourCanAcceptBookingAsync(tourCode, request.GuestCount, departureDate, id);

        var passengerRows = BuildPassengerRows(id, request.GuestCount, request.Passengers);

        var updatedRows = await _dbContext.Bookings
            .Where(item => item.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(item => item.TourCode, tourCode)
                .SetProperty(item => item.DepartureScheduleId, departureScheduleId)
                .SetProperty(item => item.TourName, tourName)
                .SetProperty(item => item.Destination, destination)
                .SetProperty(item => item.ThumbnailUrl, thumbnailUrl)
                .SetProperty(item => item.DepartureDate, departureDate)
                .SetProperty(item => item.GuestCount, request.GuestCount)
                .SetProperty(item => item.TotalAmount, request.TotalAmount)
                .SetProperty(item => item.UpdatedAt, updatedAt));

        if (updatedRows == 0)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        await _dbContext.BookingPassengers
            .Where(passenger => passenger.BookingId == id)
            .ExecuteDeleteAsync();

        _dbContext.BookingPassengers.AddRange(passengerRows);

        await _dbContext.SaveChangesAsync();
        await _tourService.RefreshTourAvailabilityAsync(booking.TourCode);
        await _tourService.RefreshTourAvailabilityAsync(tourCode);
        return ToResponse(await FindBookingAsync(id));
    }

    public async Task<BookingResponse> UpdateStatusAsync(Guid id, UpdateBookingStatusRequest request)
    {
        var booking = await _dbContext.Bookings
            .Include(item => item.User)
            .Include(item => item.Passengers)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (booking is null)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        var newStatus = NormalizeBookingStatus(request.Status);
        EnsureStatusTransitionAllowed(booking.Status, newStatus);
        var changedAt = DateTime.UtcNow;

        booking.Status = newStatus;
        booking.StatusUpdatedAt = changedAt;
        booking.StatusUpdatedBy = TrimToNull(request.UpdatedBy) ?? "Admin staff";
        booking.UpdatedAt = changedAt;

        if (newStatus == PendingStatus)
        {
            booking.PaidAt = null;
            booking.ConfirmedAt = null;
            booking.CompletedAt = null;
            booking.RequestFailedAt = null;
        }
        else if (newStatus == ConfirmedStatus)
        {
            booking.ConfirmedAt = changedAt;
            booking.ConfirmationFailedAt = null;
        }
        else if (newStatus == FinishedStatus)
        {
            booking.CompletedAt = changedAt;
            booking.CompletionFailedAt = null;
        }
        else if (newStatus == CancelledStatus)
        {
            booking.RequestFailedAt = changedAt;
        }

        await _dbContext.SaveChangesAsync();
        await _tourService.RefreshTourAvailabilityAsync(booking.TourCode);

        if (newStatus == CancelledStatus)
        {
            await SendCancellationEmailAsync(booking);
        }

        return ToResponse(booking);
    }

    public async Task<IReadOnlyList<BookingResponse>> GetCancellationRequestsAsync(string? status, string? search)
    {
        var requests = _dbContext.Bookings
            .Include(booking => booking.User)
            .Include(booking => booking.Passengers)
            .Where(booking => booking.CancellationRequestedAt != null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            var normalizedStatus = NormalizeCancellationStatus(status);
            requests = requests.Where(booking => booking.CancellationStatus == normalizedStatus);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            requests = requests.Where(booking =>
                booking.BookingCode.Contains(term) ||
                booking.TourName.Contains(term) ||
                booking.Destination.Contains(term) ||
                (booking.TourCode != null && booking.TourCode.Contains(term)) ||
                booking.User.FullName.Contains(term) ||
                booking.User.Email.Contains(term));
        }

        var result = await requests
            .OrderBy(booking => booking.CancellationStatus == CancellationPendingStatus ? 0 : 1)
            .ThenByDescending(booking => booking.CancellationRequestedAt)
            .ToListAsync();

        return result.Select(ToResponse).ToList();
    }

    public async Task<BookingResponse> ReviewCancellationRequestAsync(Guid id, ReviewCancellationRequest request)
    {
        var booking = await _dbContext.Bookings
            .Include(item => item.User)
            .Include(item => item.Passengers)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (booking is null)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        if (!string.Equals(booking.CancellationStatus, CancellationPendingStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only pending cancellation requests can be reviewed.");
        }

        var reviewStatus = NormalizeCancellationStatus(request.Status);
        if (reviewStatus is not CancellationApprovedStatus and not CancellationRejectedStatus)
        {
            throw new InvalidOperationException("Cancellation request status must be Approved or Rejected.");
        }

        var reviewedAt = DateTime.UtcNow;
        var reviewedBy = TrimToNull(request.ReviewedBy) ?? "Admin staff";

        booking.CancellationStatus = reviewStatus;
        booking.CancellationReviewedAt = reviewedAt;
        booking.CancellationReviewedBy = reviewedBy;
        booking.CancellationReviewNote = TrimToNull(request.ReviewNote);
        booking.UpdatedAt = reviewedAt;

        if (reviewStatus == CancellationApprovedStatus)
        {
            EnsureStatusTransitionAllowed(booking.Status, CancelledStatus);
            booking.Status = CancelledStatus;
            booking.StatusUpdatedAt = reviewedAt;
            booking.StatusUpdatedBy = reviewedBy;
            booking.RequestFailedAt = reviewedAt;
        }

        await _dbContext.SaveChangesAsync();
        await _tourService.RefreshTourAvailabilityAsync(booking.TourCode);

        if (reviewStatus == CancellationApprovedStatus)
        {
            await SendCancellationApprovedEmailAsync(booking);
        }
        else
        {
            await SendCancellationRejectedEmailAsync(booking);
        }

        return ToResponse(booking);
    }

    public async Task<IReadOnlyList<BookingResponse>> GetPaymentsAsync(string? paymentStatus, string? search)
    {
        var bookings = _dbContext.Bookings
            .Include(booking => booking.User)
            .Include(booking => booking.Passengers)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(paymentStatus))
        {
            var normalizedStatus = NormalizePaymentStatus(paymentStatus);
            bookings = normalizedStatus switch
            {
                "Unpaid" => bookings.Where(booking => booking.PaidAt == null && booking.PaymentFailedAt == null),
                "DepositPaid" => bookings.Where(booking => booking.PaidAt != null && booking.RemainingAmount != null && booking.RemainingAmount > 0),
                "Paid" => bookings.Where(booking => booking.PaidAt != null && (booking.RemainingAmount == null || booking.RemainingAmount <= 0)),
                "Failed" => bookings.Where(booking => booking.PaymentFailedAt != null),
                _ => bookings
            };
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            bookings = bookings.Where(booking =>
                booking.BookingCode.Contains(term) ||
                booking.TourName.Contains(term) ||
                booking.Destination.Contains(term) ||
                (booking.TourCode != null && booking.TourCode.Contains(term)) ||
                booking.User.FullName.Contains(term) ||
                booking.User.Email.Contains(term));
        }

        var result = await bookings
            .OrderBy(booking => booking.PaidAt == null ? 0 : 1)
            .ThenByDescending(booking => booking.PaymentUpdatedAt ?? booking.CreatedAt)
            .ToListAsync();

        return result.Select(ToResponse).ToList();
    }

    public async Task<BookingResponse> UpdatePaymentAsync(Guid id, UpdatePaymentRequest request)
    {
        var booking = await _dbContext.Bookings
            .Include(item => item.User)
            .Include(item => item.Passengers)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (booking is null)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        if (string.Equals(booking.Status, CancelledStatus, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(booking.Status, FinishedStatus, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Payments cannot be updated for cancelled or finished bookings.");
        }

        if (booking.PaidAt.HasValue && booking.RemainingAmount.HasValue && booking.RemainingAmount <= 0m)
        {
            throw new InvalidOperationException("Fully paid bookings cannot be updated.");
        }

        var paymentOption = NormalizePaymentOption(request.PaymentOption);
        var paymentMethod = NormalizePaymentMethod(request.PaymentMethod);
        var updatedAt = DateTime.UtcNow;

        var paidAmount = paymentOption switch
        {
            FullPaymentOption => booking.TotalAmount,
            DepositPaymentOption => decimal.Round(booking.TotalAmount * 0.4m, 2, MidpointRounding.AwayFromZero),
            BalancePaymentOption => booking.TotalAmount,
            _ => booking.TotalAmount
        };
        var remainingAmount = Math.Max(booking.TotalAmount - paidAmount, 0m);

        if (paymentOption == BalancePaymentOption && (booking.PaidAmount ?? 0m) <= 0m)
        {
            throw new InvalidOperationException("A deposit must be recorded before collecting the remaining balance.");
        }

        booking.PaymentOption = paymentOption == BalancePaymentOption
            ? booking.PaymentOption ?? DepositPaymentOption
            : paymentOption;
        booking.PaymentMethod = paymentMethod;
        booking.PaymentReference = TrimToNull(request.PaymentReference)
            ?? GeneratePaymentReference(paymentMethod, booking.BookingCode, updatedAt);
        booking.PaidAmount = paidAmount;
        booking.RemainingAmount = remainingAmount;
        booking.PaymentUpdatedAt = updatedAt;
        booking.PaymentUpdatedBy = TrimToNull(request.UpdatedBy) ?? "Admin staff";
        booking.PaymentNote = TrimToNull(request.PaymentNote);
        booking.PaidAt ??= updatedAt;
        booking.PaymentFailedAt = null;
        booking.UpdatedAt = updatedAt;

        if (string.Equals(booking.Status, PendingStatus, StringComparison.OrdinalIgnoreCase))
        {
            booking.Status = PaidStatus;
            booking.StatusUpdatedAt = updatedAt;
            booking.StatusUpdatedBy = booking.PaymentUpdatedBy;
        }

        await _dbContext.SaveChangesAsync();
        await _tourService.RefreshTourAvailabilityAsync(booking.TourCode);

        return ToResponse(booking);
    }

    private async Task<Booking> FindBookingAsync(Guid id)
    {
        var booking = await _dbContext.Bookings
            .Include(item => item.User)
            .Include(item => item.Passengers)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (booking is null)
        {
            throw new KeyNotFoundException("Booking was not found.");
        }

        return booking;
    }

    private async Task<ApplicationUser> FindUserAsync(Guid? userId, string? userEmail)
    {
        if (userId.HasValue)
        {
            var userById = await _dbContext.Users.FindAsync(userId.Value);
            if (userById is not null)
            {
                return userById;
            }
        }

        if (!string.IsNullOrWhiteSpace(userEmail))
        {
            var normalizedEmail = AuthService.NormalizeEmail(userEmail);
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail);
            if (userByEmail is not null)
            {
                return userByEmail;
            }
        }

        throw new KeyNotFoundException("Customer was not found.");
    }

    private async Task<GuideTourAssignment?> FindScheduleAsync(Guid? departureScheduleId, string? tourCode)
    {
        if (departureScheduleId.HasValue)
        {
            var schedule = await _dbContext.GuideTourAssignments.FindAsync(departureScheduleId.Value);
            if (schedule is null)
            {
                throw new KeyNotFoundException("Departure schedule was not found.");
            }

            return schedule;
        }

        if (!string.IsNullOrWhiteSpace(tourCode))
        {
            return await _dbContext.GuideTourAssignments
                .OrderBy(item => item.StartDate)
                .FirstOrDefaultAsync(item => item.TourCode == tourCode.Trim());
        }

        return null;
    }

    private static string? ResolveTourCode(string? requestedTourCode, GuideTourAssignment? schedule)
    {
        return schedule?.TourCode ?? TrimToNull(requestedTourCode);
    }

    private static string ResolveRequiredText(string? requestedValue, string? scheduleValue, string errorMessage)
    {
        var value = TrimToNull(scheduleValue) ?? TrimToNull(requestedValue);
        if (value is null)
        {
            throw new InvalidOperationException(errorMessage);
        }

        return value;
    }

    private static DateTime ResolveDepartureDate(DateTime? requestedDepartureDate, GuideTourAssignment? schedule)
    {
        var departureDate = schedule?.StartDate ?? requestedDepartureDate;
        if (!departureDate.HasValue)
        {
            throw new InvalidOperationException("Departure date is required.");
        }

        return departureDate.Value.Date;
    }

    private static void EnsureBookingCanBeEdited(Booking booking)
    {
        if (string.Equals(booking.Status, "Cancelled", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(booking.Status, "Finished", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Cancelled or finished bookings cannot be updated.");
        }
    }

    private static void AddPassengerRows(Booking booking, IReadOnlyCollection<BookingPassengerRequest> passengers)
    {
        foreach (var passenger in BuildPassengerRows(booking.Id, booking.GuestCount, passengers))
        {
            booking.Passengers.Add(passenger);
        }
    }

    private static List<BookingPassenger> BuildPassengerRows(
        Guid bookingId,
        int guestCount,
        IReadOnlyCollection<BookingPassengerRequest> passengers)
    {
        if (passengers.Count == 0)
        {
            throw new InvalidOperationException("At least one passenger is required.");
        }

        if (guestCount != passengers.Count)
        {
            throw new InvalidOperationException("Guest count must match the passenger list.");
        }

        var passengerRows = new List<BookingPassenger>();

        foreach (var passenger in passengers)
        {
            var ticketType = passenger.TicketType.Trim();
            if (!AllowedTicketTypes.Contains(ticketType))
            {
                throw new InvalidOperationException("Ticket type must be Adult or Child.");
            }

            passengerRows.Add(new BookingPassenger
            {
                BookingId = bookingId,
                FullName = passenger.FullName.Trim(),
                PhoneNumber = passenger.PhoneNumber.Trim(),
                TicketType = ticketType
            });
        }

        return passengerRows;
    }

    private async Task<string> GenerateBookingCodeAsync()
    {
        string bookingCode;

        do
        {
            bookingCode = $"WX{Random.Shared.Next(100000, 999999)}";
        }
        while (await _dbContext.Bookings.AnyAsync(booking => booking.BookingCode == bookingCode));

        return bookingCode;
    }

    private static string? TrimToNull(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string NormalizeBookingStatus(string status)
    {
        var value = TrimToNull(status);
        if (value is null)
        {
            throw new InvalidOperationException("Booking status must be Pending, Paid, Confirmed, Finished, or Cancelled.");
        }

        return value.ToLowerInvariant() switch
        {
            "confirm" or "confirmed" => ConfirmedStatus,
            "finish" or "finished" => FinishedStatus,
            "cancel" or "cancelled" => CancelledStatus,
            "paid" => PaidStatus,
            "pending" => PendingStatus,
            _ => throw new InvalidOperationException("Booking status must be Pending, Paid, Confirmed, Finished, or Cancelled.")
        };
    }

    private static string NormalizePaymentOption(string option)
    {
        var value = TrimToNull(option);
        if (value is null)
        {
            throw new InvalidOperationException("Payment option must be Full, Deposit40, or Balance.");
        }

        return value.ToLowerInvariant() switch
        {
            "full" or "fullpayment" or "full payment" => FullPaymentOption,
            "deposit" or "deposit40" or "deposit 40" or "deposit40percent" or "deposit 40%" => DepositPaymentOption,
            "balance" or "remaining" or "remainingbalance" or "remaining balance" => BalancePaymentOption,
            _ => throw new InvalidOperationException("Payment option must be Full, Deposit40, or Balance.")
        };
    }

    private static string NormalizePaymentMethod(string method)
    {
        var value = TrimToNull(method);
        if (value is null)
        {
            throw new InvalidOperationException("Payment method is required.");
        }

        return value.ToLowerInvariant() switch
        {
            "cash" => "Cash",
            "card" or "creditcard" or "credit card" => "Card",
            "bank" or "banktransfer" or "bank transfer" => "Bank Transfer",
            "ewallet" or "e-wallet" or "wallet" => "E-Wallet",
            _ => value
        };
    }

    private static string GeneratePaymentReference(string paymentMethod, string bookingCode, DateTime timestamp)
    {
        var prefix = paymentMethod.ToLowerInvariant() switch
        {
            "bank transfer" => "BT",
            "card" => "CARD",
            "cash" => "CASH",
            "e-wallet" => "EW",
            _ => "PAY"
        };

        return $"{prefix}-{bookingCode}-{timestamp:yyyyMMddHHmmss}";
    }

    private static string NormalizePaymentStatus(string status)
    {
        var value = TrimToNull(status);
        if (value is null)
        {
            throw new InvalidOperationException("Payment status must be Unpaid, DepositPaid, Paid, or Failed.");
        }

        return value.ToLowerInvariant() switch
        {
            "unpaid" => "Unpaid",
            "deposit" or "depositpaid" or "deposit paid" => "DepositPaid",
            "paid" => "Paid",
            "failed" => "Failed",
            _ => throw new InvalidOperationException("Payment status must be Unpaid, DepositPaid, Paid, or Failed.")
        };
    }

    private static string NormalizeCancellationStatus(string status)
    {
        var value = TrimToNull(status);
        if (value is null)
        {
            throw new InvalidOperationException("Cancellation request status must be Pending, Approved, or Rejected.");
        }

        return value.ToLowerInvariant() switch
        {
            "pending" => CancellationPendingStatus,
            "approve" or "approved" => CancellationApprovedStatus,
            "reject" or "rejected" => CancellationRejectedStatus,
            _ => throw new InvalidOperationException("Cancellation request status must be Pending, Approved, or Rejected.")
        };
    }

    private static void EnsureStatusTransitionAllowed(string currentStatus, string newStatus)
    {
        var current = NormalizeBookingStatus(currentStatus);
        if (newStatus == current)
        {
            return;
        }

        if (current is FinishedStatus or CancelledStatus)
        {
            throw new InvalidOperationException("Finished or cancelled bookings cannot change status.");
        }

        var isAllowed = current switch
        {
            PendingStatus => newStatus is CancelledStatus,
            PaidStatus => newStatus is ConfirmedStatus or CancelledStatus,
            ConfirmedStatus => newStatus is FinishedStatus or CancelledStatus,
            _ => false
        };

        if (current == PendingStatus && newStatus == ConfirmedStatus)
        {
            throw new InvalidOperationException("Payment must be completed before confirming this booking.");
        }

        if (!isAllowed)
        {
            throw new InvalidOperationException("Invalid booking status transition.");
        }
    }

    private async Task SendCancellationEmailAsync(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.User.Email))
        {
            return;
        }

        var subject = $"WanderX booking {booking.BookingCode} was cancelled";
        var body = $"""
Hello {booking.User.FullName},

Your WanderX booking {booking.BookingCode} for {booking.TourName} has been cancelled.
Departure date: {booking.DepartureDate:dd/MM/yyyy}

If you need support, please contact WanderX staff.
""";

        await SendEmailSafelyAsync(booking, subject, body, "booking cancellation");
    }

    private async Task SendCancellationApprovedEmailAsync(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.User.Email))
        {
            return;
        }

        var note = string.IsNullOrWhiteSpace(booking.CancellationReviewNote)
            ? "No staff note was added."
            : booking.CancellationReviewNote;

        var subject = $"WanderX cancellation request approved - {booking.BookingCode}";
        var body = $"""
Hello {booking.User.FullName},

Your cancellation request for booking {booking.BookingCode} has been approved.

Tour: {booking.TourName}
Destination: {booking.Destination}
Departure date: {booking.DepartureDate:dd/MM/yyyy}
Staff note: {note}

The booking status has been updated to Cancelled.

WanderX Team
""";

        await SendEmailSafelyAsync(booking, subject, body, "cancellation approval");
    }

    private async Task SendCancellationRejectedEmailAsync(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.User.Email))
        {
            return;
        }

        var note = string.IsNullOrWhiteSpace(booking.CancellationReviewNote)
            ? "No staff note was added."
            : booking.CancellationReviewNote;

        var subject = $"WanderX cancellation request rejected - {booking.BookingCode}";
        var body = $"""
Hello {booking.User.FullName},

Your cancellation request for booking {booking.BookingCode} was not approved.

Tour: {booking.TourName}
Destination: {booking.Destination}
Departure date: {booking.DepartureDate:dd/MM/yyyy}
Staff note: {note}

Your booking status remains {booking.Status}. If you need support, please contact WanderX staff.

WanderX Team
""";

        await SendEmailSafelyAsync(booking, subject, body, "cancellation rejection");
    }

    private async Task SendEmailSafelyAsync(Booking booking, string subject, string body, string emailType)
    {
        try
        {
            await _emailSender.SendAsync(booking.User.Email, subject, body);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Booking {EmailType} email failed. BookingCode={BookingCode}; To={ToEmail}",
                emailType,
                booking.BookingCode,
                booking.User.Email);
        }
    }

    private static BookingResponse ToResponse(Booking booking)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            CustomerName = booking.User.FullName,
            CustomerEmail = booking.User.Email,
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
            StatusUpdatedAt = booking.StatusUpdatedAt,
            StatusUpdatedBy = booking.StatusUpdatedBy,
            CancellationStatus = booking.CancellationStatus,
            CancellationReason = booking.CancellationReason,
            CancellationRequestedAt = booking.CancellationRequestedAt,
            CancellationReviewedAt = booking.CancellationReviewedAt,
            CancellationReviewedBy = booking.CancellationReviewedBy,
            CancellationReviewNote = booking.CancellationReviewNote,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt,
            PaymentOption = booking.PaymentOption,
            PaymentMethod = booking.PaymentMethod,
            PaymentReference = booking.PaymentReference,
            PaidAmount = booking.PaidAmount,
            RemainingAmount = booking.RemainingAmount,
            PaymentUpdatedAt = booking.PaymentUpdatedAt,
            PaymentUpdatedBy = booking.PaymentUpdatedBy,
            PaymentNote = booking.PaymentNote,
            PaidAt = booking.PaidAt,
            ConfirmedAt = booking.ConfirmedAt,
            CompletedAt = booking.CompletedAt,
            ApprovedAt = booking.ApprovedAt,
            RequestFailedAt = booking.RequestFailedAt,
            PaymentFailedAt = booking.PaymentFailedAt,
            ConfirmationFailedAt = booking.ConfirmationFailedAt,
            CompletionFailedAt = booking.CompletionFailedAt,
            Passengers = booking.Passengers.Select(passenger => new BookingPassengerDto
            {
                Id = passenger.Id,
                FullName = passenger.FullName,
                PhoneNumber = passenger.PhoneNumber,
                TicketType = passenger.TicketType
            }).ToList()
        };
    }
}
