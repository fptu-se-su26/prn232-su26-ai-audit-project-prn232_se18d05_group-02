using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WanderXServer.BusinessObject;

public class Booking
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(32)]
    public string BookingCode { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string TourName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Destination { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [Required]
    public DateTime DepartureDate { get; set; }

    [Required]
    public int GuestCount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(32)]
    public string Status { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Timestamps cho Tiến trình đơn hàng
    public DateTime? PaidAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    // Timestamps cho Approval và Failure states
    public DateTime? ApprovedAt { get; set; }

    public DateTime? RequestFailedAt { get; set; }

    public DateTime? PaymentFailedAt { get; set; }

    public DateTime? ConfirmationFailedAt { get; set; }

    public DateTime? CompletionFailedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    public ICollection<BookingPassenger> Passengers { get; set; } = new List<BookingPassenger>();
}
