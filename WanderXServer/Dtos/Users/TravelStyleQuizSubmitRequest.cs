namespace WanderXServer.Dtos.Users;

public class TravelStyleQuizSubmitRequest
{
    public List<QuizAnswerDto> Answers { get; set; } = new();
}
