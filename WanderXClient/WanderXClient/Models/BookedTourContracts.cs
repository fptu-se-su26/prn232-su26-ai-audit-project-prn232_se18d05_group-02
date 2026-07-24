namespace WanderXClient.Models;

public sealed class BookedTourResponse
{
    public string TourCode { get; set; } = string.Empty;

    public string TourName { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public DateTime DepartureDate { get; set; }

    public string BookingStatus { get; set; } = string.Empty;

    public Guid? TourId { get; set; }

    public string TourStatus { get; set; } = string.Empty;

    public int BookingCount { get; set; }

    public int GuestCount { get; set; }

    public decimal TotalAmount { get; set; }

    public bool CanDeleteOrHide { get; set; }
}
