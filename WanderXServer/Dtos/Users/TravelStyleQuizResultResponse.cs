namespace WanderXServer.Dtos.Users;

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
