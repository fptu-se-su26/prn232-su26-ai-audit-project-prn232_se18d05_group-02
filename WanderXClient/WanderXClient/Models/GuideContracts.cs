using System.ComponentModel.DataAnnotations;

namespace WanderXClient.Models;

public sealed class GuideResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public List<string> Languages { get; set; } = new();

    public string LanguagesText { get; set; } = string.Empty;

    public string ExpertiseArea { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public int CompletedTours { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public sealed class CreateGuideRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 120 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temporary password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Languages are required.")]
    public string Languages { get; set; } = string.Empty;

    [Required(ErrorMessage = "Expertise area is required.")]
    public string ExpertiseArea { get; set; } = string.Empty;

    [Required(ErrorMessage = "Region is required.")]
    public string Region { get; set; } = string.Empty;

    [Range(0, 10000, ErrorMessage = "Completed tours must be 0 or higher.")]
    public int CompletedTours { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = "Active";

    [StringLength(600, ErrorMessage = "Bio must be 600 characters or fewer.")]
    public string Bio { get; set; } = string.Empty;
}

public sealed class UpdateGuideRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 120 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Languages are required.")]
    public string Languages { get; set; } = string.Empty;

    [Required(ErrorMessage = "Expertise area is required.")]
    public string ExpertiseArea { get; set; } = string.Empty;

    [Required(ErrorMessage = "Region is required.")]
    public string Region { get; set; } = string.Empty;

    [Range(0, 10000, ErrorMessage = "Completed tours must be 0 or higher.")]
    public int CompletedTours { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = "Active";

    [StringLength(600, ErrorMessage = "Bio must be 600 characters or fewer.")]
    public string Bio { get; set; } = string.Empty;
}

public sealed class GuideSelfUpdateRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 120 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Languages are required.")]
    public string Languages { get; set; } = string.Empty;

    [Required(ErrorMessage = "Expertise area is required.")]
    public string ExpertiseArea { get; set; } = string.Empty;

    [Required(ErrorMessage = "Region is required.")]
    public string Region { get; set; } = string.Empty;

    [Range(0, 10000, ErrorMessage = "Completed tours must be 0 or higher.")]
    public int CompletedTours { get; set; }

    [StringLength(600, ErrorMessage = "Bio must be 600 characters or fewer.")]
    public string Bio { get; set; } = string.Empty;
}
