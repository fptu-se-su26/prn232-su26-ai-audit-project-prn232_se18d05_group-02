using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.UserSpecialRequests;

public class UpdateUserSpecialRequestRequest
{
    [Required]
    [StringLength(32)]
    public string Status { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? AdminResponse { get; set; }
}
