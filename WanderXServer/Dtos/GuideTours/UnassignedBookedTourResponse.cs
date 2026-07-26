namespace WanderXServer.Dtos.GuideTours;

public class UnassignedBookedTourResponse
{
    public string TourCode { get; set; } = string.Empty;

    public string TourName { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public DateTime DepartureDate { get; set; }

    public DateTime EndDate { get; set; }

    public int GuestCount { get; set; }

    public int BookingCount { get; set; }

    public string DefaultMeetingPoint { get; set; } = string.Empty;

    public string DefaultSummary { get; set; } = string.Empty;
}
