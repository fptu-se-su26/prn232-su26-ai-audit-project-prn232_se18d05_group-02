using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Auth;

public class VerifyPhoneRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{6}$")]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; } = string.Empty;
}
