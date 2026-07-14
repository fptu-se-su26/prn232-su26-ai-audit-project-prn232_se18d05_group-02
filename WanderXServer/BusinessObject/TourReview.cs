using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WanderXServer.BusinessObject;

[Table("TourReviews")]
public class TourReview
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid BookingId { get; set; }

    [ForeignKey(nameof(BookingId))]
    public Booking? Booking { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(2000)]
    public string? Comment { get; set; }

    public string? TravelPhotos { get; set; }

    [Required]
    [StringLength(32)]
    public string Status { get; set; } = "Visible";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ModeratedAt { get; set; }

    [StringLength(1000)]
    public string? ModerationReason { get; set; }

    public Guid? ModeratedBy { get; set; }
}
