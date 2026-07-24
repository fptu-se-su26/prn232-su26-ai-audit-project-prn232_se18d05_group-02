using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Bookings;

public class UpdateBookingStatusRequest
{
    [Required]
    [StringLength(32)]
    public string Status { get; set; } = string.Empty;

    [StringLength(120)]
    public string? UpdatedBy { get; set; }
}
