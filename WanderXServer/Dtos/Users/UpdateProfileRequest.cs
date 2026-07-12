using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Users;

public class UpdateProfileRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 120 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(240)]
    public string? Address { get; set; }

    public string? CurrentPassword { get; set; }
    
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string? NewPassword { get; set; }
}
