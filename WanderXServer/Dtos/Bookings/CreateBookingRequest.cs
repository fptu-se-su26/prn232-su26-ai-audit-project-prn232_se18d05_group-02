using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Bookings;

public class CreateBookingRequest
{
    public Guid? UserId { get; set; }

    [EmailAddress]
    public string? UserEmail { get; set; }

    [StringLength(32)]
    public string? TourCode { get; set; }

    public Guid? DepartureScheduleId { get; set; }

    [StringLength(200)]
    public string? TourName { get; set; }

    [StringLength(200)]
    public string? Destination { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    public DateTime? DepartureDate { get; set; }

    [Range(1, 200)]
    public int GuestCount { get; set; }

    [Range(0.01, 999999999999)]
    public decimal TotalAmount { get; set; }

    public List<BookingPassengerRequest> Passengers { get; set; } = new();
}
