using WanderXServer.Dtos.Users;

namespace WanderXServer.Dtos.Bookings;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string BookingCode { get; set; } = string.Empty;
    public string? TourCode { get; set; }
    public Guid? DepartureScheduleId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public DateTime DepartureDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? StatusUpdatedAt { get; set; }
    public string? StatusUpdatedBy { get; set; }
    public string? CancellationStatus { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CancellationRequestedAt { get; set; }
    public DateTime? CancellationReviewedAt { get; set; }
    public string? CancellationReviewedBy { get; set; }
    public string? CancellationReviewNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? PaymentOption { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? RemainingAmount { get; set; }
    public DateTime? PaymentUpdatedAt { get; set; }
    public string? PaymentUpdatedBy { get; set; }
    public string? PaymentNote { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RequestFailedAt { get; set; }
    public DateTime? PaymentFailedAt { get; set; }
    public DateTime? ConfirmationFailedAt { get; set; }
    public DateTime? CompletionFailedAt { get; set; }
    public List<BookingPassengerDto> Passengers { get; set; } = new();
}
