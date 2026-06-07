using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WanderXServer.BusinessObject;

public class PasswordResetToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    [Required]
    [StringLength(128, MinimumLength = 32)]
    public string Token { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime ExpiresAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? UsedAt { get; set; }

    [NotMapped]
    public bool IsUsed => UsedAt.HasValue;
}
