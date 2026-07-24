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

        using var response = await _httpClient.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<TourResponse>>() ?? new List<TourResponse>();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
    }

    public async Task<TourResponse> GetTourAsync(Guid id)
    {
        using var response = await _httpClient.GetAsync($"api/tours/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TourResponse>() ?? new TourResponse();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
    }

    public Task<TourResponse?> CreateTourAsync(CreateTourRequest request)
    {
        return SendAsync<CreateTourRequest, TourResponse>(HttpMethod.Post, "api/tours", request);
    }

    public Task<TourResponse?> UpdateTourAsync(Guid id, UpdateTourRequest request)
    {
        return SendAsync<UpdateTourRequest, TourResponse>(HttpMethod.Put, $"api/tours/{id}", request);
    }

    public async Task<TourResponse?> HideTourAsync(Guid id)
    {
        using var response = await _httpClient.PatchAsync($"api/tours/{id}/hide", null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TourResponse>();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
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

            var titleText = document.RootElement.TryGetProperty("title", out var title)
                ? title.GetString()
                : null;

            if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable &&
                string.Equals(titleText, "Database unavailable", StringComparison.OrdinalIgnoreCase))
            {
                return "Database is unavailable. Please start SQL Server SQLEXPRESS, restart the server, and refresh this page.";
            }

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The tour request could not be completed.";
            }

            if (!string.IsNullOrWhiteSpace(titleText))
            {
                return titleText;
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }
}
