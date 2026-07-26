namespace WanderXClient.Models;

public class TravelStyleQuizSubmitRequest
{
    public List<QuizAnswerDto> Answers { get; set; } = new();
}

public class QuizAnswerDto
{
    public Guid QuestionId { get; set; }
    public Guid OptionId { get; set; }
}
public class TravelStyleQuizResultResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string AnswersJson { get; set; } = string.Empty;
    public int AdventureScore { get; set; }
    public int CulturalScore { get; set; }
    public int RelaxationScore { get; set; }
    public int LuxuryScore { get; set; }
    public string DominantStyle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
}

// Admin CRUD contracts for quiz questions
public class QuizQuestionClientDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<QuizOptionClientDto> Options { get; set; } = new();
}

public class QuizOptionClientDto
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public string OptionKey { get; set; } = string.Empty; // "A", "B", "C", "D"
    public string Text { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // "Adventure", "Cultural", "Relaxation", "Luxury"
    public int Score { get; set; } = 3;
}
