namespace WanderXClient.Models;

public sealed class UserSpecialRequestResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? AdminResponse { get; set; }
}

public sealed class CreateUserSpecialRequestRequest
{
    public Guid BookingId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public sealed class UpdateUserSpecialRequestRequest
{
    public string Status { get; set; } = string.Empty;
    public string? AdminResponse { get; set; }
}

// TV3 - Admin view model
public sealed class BookingWithRequestCountResponse
{
    public Guid BookingId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureDate { get; set; }
    public string BookingStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public int TotalRequests { get; set; }
    public int PendingRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int RejectedRequests { get; set; }
}
// end TV3

