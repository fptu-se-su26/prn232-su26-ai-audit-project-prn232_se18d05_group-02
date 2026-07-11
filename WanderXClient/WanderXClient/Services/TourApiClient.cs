using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class TourApiClient
{
    private readonly HttpClient _httpClient;

    public TourApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TourResponse>> GetToursAsync(string? search = null, string? status = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query.Add($"search={Uri.EscapeDataString(search)}");
        }

        if (!string.IsNullOrWhiteSpace(status) && !status.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            query.Add($"status={Uri.EscapeDataString(status)}");
        }

        var path = query.Count == 0
            ? "api/tours"
            : $"api/tours?{string.Join("&", query)}";

        return await _httpClient.GetFromJsonAsync<List<TourResponse>>(path) ?? new List<TourResponse>();
    }

    public Task<TourResponse?> CreateTourAsync(CreateTourRequest request)
    {
        return SendAsync<CreateTourRequest, TourResponse>(HttpMethod.Post, "api/tours", request);
    }

    public Task<TourResponse?> UpdateTourAsync(Guid id, UpdateTourRequest request)
    {
        return SendAsync<UpdateTourRequest, TourResponse>(HttpMethod.Put, $"api/tours/{id}", request);
    }

    public async Task DeleteTourAsync(Guid id)
    {
        using var response = await _httpClient.DeleteAsync($"api/tours/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ReadErrorAsync(response));
        }
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
            return "The tour request could not be completed.";
        }

        try
        {
            using var document = JsonDocument.Parse(detail);

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The tour request could not be completed.";
            }

            if (document.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? "The tour request could not be completed.";
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }
}
