namespace WanderXServer.Dtos.BookedTours;

public sealed class BookedTourFilterRequest
{
    public string? Search { get; set; }

    public string? Status { get; set; }

    public string? Destination { get; set; }

    public DateTime? DepartureDate { get; set; }
}
