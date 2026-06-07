using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Guides;

public class UpdateGuideRequest
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
    [StringLength(240)]
    public string Languages { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string ExpertiseArea { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Region { get; set; } = string.Empty;

    [Range(0, 10000)]
    public int CompletedTours { get; set; }

    [Required]
    [StringLength(32)]
    public string Status { get; set; } = "Active";

    [StringLength(600)]
    public string Bio { get; set; } = string.Empty;
}
