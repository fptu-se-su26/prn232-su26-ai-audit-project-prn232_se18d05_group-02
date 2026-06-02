using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(GuideProfileId))]
public class GuideTourAssignment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid GuideProfileId { get; set; }

    public GuideProfile GuideProfile { get; set; } = null!;

    [Required]
    [StringLength(32)]
    public string TourCode { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string TourName { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Region { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Range(1, 1000)]
    public int TravelerCount { get; set; }

    [Required]
    [StringLength(240)]
    public string MeetingPoint { get; set; } = string.Empty;

    [Required]
    [StringLength(32)]
    public string Status { get; set; } = "Confirmed";

    [StringLength(1000)]
    public string ItinerarySummary { get; set; } = string.Empty;

    [StringLength(600)]
    public string? DeclineReason { get; set; }

    public DateTime? DeclinedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
