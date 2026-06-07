using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WanderXServer.BusinessObject;

[Index(nameof(UserId), IsUnique = true)]
public class GuideProfile
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

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

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
}
