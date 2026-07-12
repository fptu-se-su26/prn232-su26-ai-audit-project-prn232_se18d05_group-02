namespace WanderXServer.Dtos.Users;

public class BookingSummaryResponse
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public DateTime DepartureDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Tiến trình đơn hàng (null = chưa xử lý)
    public DateTime? PaidAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Approval và Failure states
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RequestFailedAt { get; set; }
    public DateTime? PaymentFailedAt { get; set; }
    public DateTime? ConfirmationFailedAt { get; set; }
    public DateTime? CompletionFailedAt { get; set; }

    public List<BookingPassengerDto> Passengers { get; set; } = new();
}
