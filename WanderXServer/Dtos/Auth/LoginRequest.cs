using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Auth;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
