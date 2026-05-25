namespace WanderXServer.Dtos.Auth;

public class MessageResponse
{
    public string Message { get; set; } = string.Empty;

    public string? ResetToken { get; set; }
}
