using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Bookings;

public class UpdatePaymentRequest
{
    [Required]
    [StringLength(32)]
    public string PaymentOption { get; set; } = string.Empty;

    [Required]
    [StringLength(64)]
    public string PaymentMethod { get; set; } = string.Empty;

    [StringLength(120)]
    public string? PaymentReference { get; set; }

    [StringLength(120)]
    public string? UpdatedBy { get; set; }

    [StringLength(1000)]
    public string? PaymentNote { get; set; }
}
