using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class AuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        return PostAsync<LoginRequest, AuthResponse>("api/auth/login", request);
    }

    public Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        return PostAsync<RegisterRequest, AuthResponse>("api/auth/register", request);
    }

    public Task<MessageResponse?> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return PostAsync<ForgotPasswordRequest, MessageResponse>("api/auth/forgot-password", request);
    }

    public Task<MessageResponse?> VerifyPhoneAsync(VerifyPhoneRequest request)
    {
        return PostAsync<VerifyPhoneRequest, MessageResponse>("api/auth/verify-phone", request);
    }

    public Task<AuthResponse?> ResendCodeAsync(ForgotPasswordRequest request)
    {
        return PostAsync<ForgotPasswordRequest, AuthResponse>("api/auth/resend-verification-code", request);
    }

    private async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync(path, request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        var detail = await ReadErrorDetailAsync(response);
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail)
            ? "The request could not be completed."
            : detail);
    }

    private static async Task<string> ReadErrorDetailAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            if (document.RootElement.TryGetProperty("errors", out var errors) &&
                errors.ValueKind == JsonValueKind.Object)
            {
                var messages = new List<string>();
                foreach (var error in errors.EnumerateObject())
                {
                    if (error.Value.ValueKind != JsonValueKind.Array)
                    {
                        continue;
                    }

                    foreach (var message in error.Value.EnumerateArray())
                    {
                        if (message.ValueKind == JsonValueKind.String &&
                            !string.IsNullOrWhiteSpace(message.GetString()))
                        {
                            messages.Add($"{FormatFieldName(error.Name)}: {message.GetString()}");
                        }
                    }
                }

                if (messages.Count > 0)
                {
                    return string.Join(" ", messages);
                }
            }

            if (document.RootElement.TryGetProperty("detail", out var detail) &&
                detail.ValueKind == JsonValueKind.String)
            {
                return detail.GetString() ?? string.Empty;
            }

            if (document.RootElement.TryGetProperty("title", out var title) &&
                title.ValueKind == JsonValueKind.String)
            {
                return title.GetString() ?? string.Empty;
            }
        }
        catch (JsonException)
        {
            return content;
        }

        return content;
    }

    private static string FormatFieldName(string fieldName)
    {
        var cleanName = fieldName.Split('.').LastOrDefault() ?? fieldName;
        return cleanName switch
        {
            nameof(RegisterRequest.FullName) => "Full name",
            nameof(RegisterRequest.Email) => "Email",
            nameof(RegisterRequest.PhoneNumber) => "Phone number",
            nameof(RegisterRequest.Password) => "Password",
            nameof(RegisterRequest.ConfirmPassword) => "Confirm password",
            nameof(RegisterRequest.AcceptTerms) => "Terms",
            _ => cleanName
        };
    }

    public async Task InitializeCsrfAsync()
    {
        try
        {
            await _httpClient.GetAsync("api/auth/csrf");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"[AuthApiClient] Failed to initialize CSRF token: {ex.Message}");
        }
    }
}
