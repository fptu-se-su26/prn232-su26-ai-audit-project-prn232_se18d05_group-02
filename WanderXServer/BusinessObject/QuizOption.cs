using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WanderXServer.BusinessObject;

public class QuizOption
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid QuestionId { get; set; }

    [Required]
    [StringLength(10)]
    public string OptionKey { get; set; } = string.Empty; // "A", "B", "C", "D"

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty; // "Adventure", "Cultural", "Relaxation", "Luxury"

    [Required]
    public int Score { get; set; } = 3;

    [ForeignKey(nameof(QuestionId))]
    public QuizQuestion Question { get; set; } = null!;
}
