using System.ComponentModel.DataAnnotations;

namespace WanderXClient.Models;

public sealed class GuideTourAssignmentResponse
{
    public Guid Id { get; set; }

    public Guid GuideProfileId { get; set; }

    public string GuideName { get; set; } = string.Empty;

    public string GuideEmail { get; set; } = string.Empty;

    public string TourCode { get; set; } = string.Empty;

    public string TourName { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int TravelerCount { get; set; }

    public string MeetingPoint { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string ItinerarySummary { get; set; } = string.Empty;

    public string? DeclineReason { get; set; }

    public DateTime? DeclinedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public bool IsCurrentBusyTour { get; set; }

    public bool CanDecline { get; set; }

    public bool CanFinish { get; set; }
}

public sealed class DeclineTourRequest
{
    [Required(ErrorMessage = "Decline reason is required.")]
    [StringLength(600, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 600 characters.")]
    public string Reason { get; set; } = string.Empty;
}

public sealed class CreateGuideTourAssignmentRequest
{
    [Required(ErrorMessage = "Guide is required.")]
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
    public string Status { get; set; } = "Assigned";

    [StringLength(1000, ErrorMessage = "Itinerary summary must be 1000 characters or fewer.")]
    public string ItinerarySummary { get; set; } = string.Empty;
}

public sealed class ChangeGuideAssignmentRequest
{
    [Required(ErrorMessage = "Guide is required.")]
    public Guid GuideProfileId { get; set; }
}
