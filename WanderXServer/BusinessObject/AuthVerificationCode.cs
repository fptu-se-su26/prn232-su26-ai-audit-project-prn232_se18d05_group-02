using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WanderXServer.BusinessObject.Enums;

namespace WanderXServer.BusinessObject;

public class AuthVerificationCode
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    [Required]
    [RegularExpression(@"^\d{6}$")]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public VerificationPurpose Purpose { get; set; } = VerificationPurpose.PhoneVerification;

    [DataType(DataType.DateTime)]
    public DateTime ExpiresAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? ConsumedAt { get; set; }

    [NotMapped]
    public bool IsConsumed => ConsumedAt.HasValue;
}
