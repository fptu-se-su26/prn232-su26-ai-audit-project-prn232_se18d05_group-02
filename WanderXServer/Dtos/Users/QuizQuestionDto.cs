using System.ComponentModel.DataAnnotations;

namespace WanderXServer.Dtos.Users;

public class QuizQuestionDto
{
    public Guid Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public List<QuizOptionDto> Options { get; set; } = new();
}
