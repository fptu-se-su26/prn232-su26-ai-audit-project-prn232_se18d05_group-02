namespace WanderXServer.Dtos.GuideTours;

public class GuideTourAssignmentResponse
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
