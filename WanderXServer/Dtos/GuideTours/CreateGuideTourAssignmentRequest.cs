using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.GuideTours;

public class CreateGuideTourAssignmentRequest
{
    [Required(ErrorMessage = "Guide profile is required.")]
    public Guid GuideProfileId { get; set; }

    [Required(ErrorMessage = "Tour code is required.")]
    [StringLength(32, ErrorMessage = "Tour code must be 32 characters or fewer.")]
    public string TourCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(160, ErrorMessage = "Tour name must be 160 characters or fewer.")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Destination is required.")]
    [StringLength(160, ErrorMessage = "Destination must be 160 characters or fewer.")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "Region is required.")]
    [StringLength(160, ErrorMessage = "Region must be 160 characters or fewer.")]
    public string Region { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; } = DateTime.Today;

    [Range(1, 1000, ErrorMessage = "Traveler count must be between 1 and 1000.")]
    public int TravelerCount { get; set; } = 1;

    [Required(ErrorMessage = "Meeting point is required.")]
    [StringLength(240, ErrorMessage = "Meeting point must be 240 characters or fewer.")]
    public string MeetingPoint { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    [StringLength(32, ErrorMessage = "Status must be 32 characters or fewer.")]
    public string Status { get; set; } = "Assigned";

    [StringLength(1000, ErrorMessage = "Itinerary summary must be 1000 characters or fewer.")]
    public string ItinerarySummary { get; set; } = string.Empty;
}
