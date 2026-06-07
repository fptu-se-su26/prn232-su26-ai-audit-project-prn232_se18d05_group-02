namespace WanderXServer.Services;

public sealed class EmailOptions
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 25;

    public bool EnableSsl { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string FromName { get; set; } = "WanderX";

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Host) && !string.IsNullOrWhiteSpace(FromEmail);
}
