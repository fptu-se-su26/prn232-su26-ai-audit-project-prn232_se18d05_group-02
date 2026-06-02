using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class GuideTourApiClient
{
    private readonly HttpClient _httpClient;

    public GuideTourApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GuideTourAssignmentResponse>> GetScheduleAsync(string email)
    {
        return await GetAsync<List<GuideTourAssignmentResponse>>(
            $"api/guide-tours/schedule?email={Uri.EscapeDataString(email)}") ?? new List<GuideTourAssignmentResponse>();
    }

    public Task<GuideTourAssignmentResponse?> GetByIdAsync(Guid id)
    {
        return GetAsync<GuideTourAssignmentResponse>($"api/guide-tours/{id}");
    }

    public Task<GuideTourAssignmentResponse?> DeclineAsync(Guid id, DeclineTourRequest request)
    {
        return SendAsync<DeclineTourRequest, GuideTourAssignmentResponse>(
            HttpMethod.Post,
            $"api/guide-tours/{id}/decline",
            request);
    }

    public Task<GuideTourAssignmentResponse?> FinishAsync(Guid id)
    {
        return SendAsync<object, GuideTourAssignmentResponse>(
            HttpMethod.Put,
            $"api/guide-tours/{id}/finish",
            new { });
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
            return "The guide tour request could not be completed.";
        }

        try
        {
            using var document = JsonDocument.Parse(detail);

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The guide tour request could not be completed.";
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }
}
