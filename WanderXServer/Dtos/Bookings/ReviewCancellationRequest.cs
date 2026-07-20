using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Bookings;

public class ReviewCancellationRequest
{
    [Required]
    [StringLength(32)]
    public string Status { get; set; } = string.Empty;

    [StringLength(120)]
    public string? ReviewedBy { get; set; }

    [StringLength(1000)]
    public string? ReviewNote { get; set; }
}
