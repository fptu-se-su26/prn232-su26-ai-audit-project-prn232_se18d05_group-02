using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Auth;

public class RegisterRequest
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(32)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true")]
    public bool AcceptTerms { get; set; }
}
