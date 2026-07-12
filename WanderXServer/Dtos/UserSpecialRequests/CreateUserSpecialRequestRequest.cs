using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.UserSpecialRequests;

public class CreateUserSpecialRequestRequest
{
    [Required]
    public Guid BookingId { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;
}
