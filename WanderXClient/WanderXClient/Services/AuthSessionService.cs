using System.Text.Json;
using Microsoft.JSInterop;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class AuthSessionService
{
    private const string SessionStorageKey = "wanderx.auth.session";
    private const string LocalStorageKey = "wanderx.auth.local";
    private readonly IJSRuntime _jsRuntime;

    public event Action? OnAvatarChanged;

    public AuthSessionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void NotifyAvatarChanged()
    {
        OnAvatarChanged?.Invoke();
    }

    public async Task SaveAsync(AuthResponse response, bool rememberMe)
    {
        var session = new AuthSession
        {
            Token = response.Token,
            Email = response.Email,
            FullName = response.FullName,
            Role = response.Role
        };

        var json = JsonSerializer.Serialize(session);
        var targetStorage = rememberMe ? "localStorage" : "sessionStorage";
        var staleStorage = rememberMe ? "sessionStorage" : "localStorage";

        await _jsRuntime.InvokeVoidAsync($"{targetStorage}.setItem", rememberMe ? LocalStorageKey : SessionStorageKey, json);
        await _jsRuntime.InvokeVoidAsync($"{staleStorage}.removeItem", rememberMe ? SessionStorageKey : LocalStorageKey);
    }

    public async Task<AuthSession?> GetAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", SessionStorageKey);

        if (string.IsNullOrWhiteSpace(json))
        {
            json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", LocalStorageKey);
        }

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<AuthSession>(json);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", SessionStorageKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
    }
}

public sealed class AuthSession
{
    public string Token { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsGuide =>
        Role.Equals("Guide", StringComparison.OrdinalIgnoreCase) ||
        Role.Equals("Guider", StringComparison.OrdinalIgnoreCase);
}
