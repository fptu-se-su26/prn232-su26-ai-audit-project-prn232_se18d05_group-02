namespace WanderXServer.Dtos.Guides;

public class GuideResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public IReadOnlyList<string> Languages { get; set; } = Array.Empty<string>();

    public string LanguagesText { get; set; } = string.Empty;

    public string ExpertiseArea { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public int CompletedTours { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
