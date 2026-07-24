using System.ComponentModel.DataAnnotations;

namespace WanderXClient.Models;

public sealed class BookingResponse
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

public sealed class BookingPassengerRequest
{
    [Required(ErrorMessage = "Passenger name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string TicketType { get; set; } = "Adult";
}

public sealed class CreateBookingRequest
{
    public Guid? UserId { get; set; }

    [StringLength(256)]
    public string? UserEmail { get; set; }

    [StringLength(32)]
    public string? TourCode { get; set; }

    public Guid? DepartureScheduleId { get; set; }

    [StringLength(200)]
    public string? TourName { get; set; }

    [StringLength(200)]
    public string? Destination { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    public DateTime? DepartureDate { get; set; }

    [Range(1, 200)]
    public int GuestCount { get; set; }

    [Range(0.01, 999999999999)]
    public decimal TotalAmount { get; set; }

    public List<BookingPassengerRequest> Passengers { get; set; } = new();
}

public sealed class UpdateBookingRequest
{
    [StringLength(32)]
    public string? TourCode { get; set; }

    public Guid? DepartureScheduleId { get; set; }

    [StringLength(200)]
    public string? TourName { get; set; }

    [StringLength(200)]
    public string? Destination { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    public DateTime? DepartureDate { get; set; }

    [Range(1, 200)]
    public int GuestCount { get; set; }

    [Range(0.01, 999999999999)]
    public decimal TotalAmount { get; set; }

    public List<BookingPassengerRequest> Passengers { get; set; } = new();
}

public sealed class UpdateBookingStatusRequest
{
    [Required]
    [StringLength(32)]
    public string Status { get; set; } = string.Empty;

    [StringLength(120)]
    public string? UpdatedBy { get; set; }
}

public sealed class ReviewCancellationRequest
{
    [Required]
    [StringLength(32)]
    public string Status { get; set; } = "Approved";

    [StringLength(120)]
    public string? ReviewedBy { get; set; }

    [StringLength(1000)]
    public string? ReviewNote { get; set; }
}

public sealed class UpdatePaymentRequest
{
    [Required]
    [StringLength(32)]
    public string PaymentOption { get; set; } = "Full";

    [Required]
    [StringLength(64)]
    public string PaymentMethod { get; set; } = "Bank Transfer";

    [StringLength(120)]
    public string? PaymentReference { get; set; }

    [StringLength(120)]
    public string? UpdatedBy { get; set; }

    [StringLength(1000)]
    public string? PaymentNote { get; set; }
}
