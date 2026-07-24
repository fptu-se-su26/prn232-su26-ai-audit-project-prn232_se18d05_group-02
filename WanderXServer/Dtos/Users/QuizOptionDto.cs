using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Users;

public class QuizOptionDto
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    [Required]
    [StringLength(10)]
    public string OptionKey { get; set; } = string.Empty; // "A", "B", "C", "D"

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty; // "Adventure", "Cultural", "Relaxation", "Luxury"

    public int Score { get; set; } = 3;
}
