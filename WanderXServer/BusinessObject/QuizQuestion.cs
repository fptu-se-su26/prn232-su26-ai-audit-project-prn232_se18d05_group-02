using System.ComponentModel.DataAnnotations;

namespace WanderXServer.BusinessObject;

public class QuizQuestion
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public int DisplayOrder { get; set; }

    public ICollection<QuizOption> Options { get; set; } = new List<QuizOption>();
}
