using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject.Enums;

namespace WanderXServer.BusinessObject;

[Index(nameof(NormalizedEmail), IsUnique = true)]
public class ApplicationUser
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(120, MinimumLength = 2)]
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
    [StringLength(512)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; } = UserRole.Customer;

    public bool IsEmailConfirmed { get; set; }

    public bool IsPhoneConfirmed { get; set; }

    [StringLength(240)]
    public string? Address { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? LastLoginAt { get; set; }

    public ICollection<AuthVerificationCode> VerificationCodes { get; set; } = new List<AuthVerificationCode>();

    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    public GuideProfile? GuideProfile { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
