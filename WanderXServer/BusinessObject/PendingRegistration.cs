using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(NormalizedEmail), nameof(CreatedAt))]
public class PendingRegistration
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string NormalizedEmail { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(32)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{6}$")]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ConsumedAt { get; set; }
}
