namespace WanderXServer.Dtos.UserSpecialRequests;

public class UserSpecialRequestResponse
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
