using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class GuideApiClient
{
    private readonly HttpClient _httpClient;

    public GuideApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GuideResponse>> GetGuidesAsync(string? search = null)
    {
        var path = string.IsNullOrWhiteSpace(search)
            ? "api/guides"
            : $"api/guides?search={Uri.EscapeDataString(search)}";

        return await _httpClient.GetFromJsonAsync<List<GuideResponse>>(path) ?? new List<GuideResponse>();
    }

    public Task<GuideResponse?> GetGuideByEmailAsync(string email)
    {
        return GetAsync<GuideResponse>($"api/guides/by-email?email={Uri.EscapeDataString(email)}");
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var response = await GetAsync<EmailExistsResponse>($"api/guides/email-exists?email={Uri.EscapeDataString(email)}");
        return response?.Exists ?? false;
    }

    public Task<GuideResponse?> CreateGuideAsync(CreateGuideRequest request)
    {
        return SendAsync<CreateGuideRequest, GuideResponse>(HttpMethod.Post, "api/guides", request);
    }

    public Task<GuideResponse?> UpdateGuideAsync(Guid id, UpdateGuideRequest request)
    {
        return SendAsync<UpdateGuideRequest, GuideResponse>(HttpMethod.Put, $"api/guides/{id}", request);
    }

    public Task<GuideResponse?> SelfUpdateGuideAsync(GuideSelfUpdateRequest request)
    {
        return SendAsync<GuideSelfUpdateRequest, GuideResponse>(HttpMethod.Put, "api/guides/self-profile", request);
    }

    private async Task<TResponse?> GetAsync<TResponse>(string path)
    {
        using var response = await _httpClient.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
    }

    private async Task<TResponse?> SendAsync<TRequest, TResponse>(HttpMethod method, string path, TRequest request)
    {
        using var message = new HttpRequestMessage(method, path)
        {
            Content = JsonContent.Create(request)
        };
        using var response = await _httpClient.SendAsync(message);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        var detail = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(detail))
        {
            return "The guide request could not be completed.";
        }

        try
        {
            using var document = JsonDocument.Parse(detail);

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The guide request could not be completed.";
            }

            if (document.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? "The guide request could not be completed.";
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }

    private sealed class EmailExistsResponse
    {
        public bool Exists { get; set; }
    }
}
