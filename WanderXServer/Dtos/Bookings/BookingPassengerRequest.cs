using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Bookings;

public class BookingPassengerRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string TicketType { get; set; } = "Adult";
}
