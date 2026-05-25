using System.Net.Http.Json;
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

        var detail = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail)
            ? "The request could not be completed."
            : detail);
    }
}
