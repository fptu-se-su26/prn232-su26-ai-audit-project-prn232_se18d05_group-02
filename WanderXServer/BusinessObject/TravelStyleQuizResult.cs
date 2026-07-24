using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WanderXServer.BusinessObject;

public class TravelStyleQuizResult
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string AnswersJson { get; set; } = string.Empty;

    [Required]
    public int AdventureScore { get; set; }

    [Required]
    public int CulturalScore { get; set; }

    [Required]
    public int RelaxationScore { get; set; }

    [Required]
    public int LuxuryScore { get; set; }

    [Required]
    [StringLength(50)]
    public string DominantStyle { get; set; } = string.Empty;

    [Required]
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
}
