using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class BookedTourApiClient
{
    private readonly HttpClient _httpClient;

    public BookedTourApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BookedTourResponse>> GetBookedToursAsync(
        string? search = null,
        string? status = null,
        string? destination = null,
        DateTime? departureDate = null)
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

        if (!string.IsNullOrWhiteSpace(destination))
        {
            query.Add($"destination={Uri.EscapeDataString(destination)}");
        }

        if (departureDate.HasValue)
        {
            query.Add($"departureDate={Uri.EscapeDataString(departureDate.Value.ToString("yyyy-MM-dd"))}");
        }

        var path = query.Count == 0
            ? "api/booked-tours"
            : $"api/booked-tours?{string.Join("&", query)}";

        using var response = await _httpClient.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<BookedTourResponse>>() ?? new List<BookedTourResponse>();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        var detail = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(detail))
        {
            return "The booked tour request could not be completed.";
        }

        try
        {
            using var document = JsonDocument.Parse(detail);

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The booked tour request could not be completed.";
            }

            if (document.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? "The booked tour request could not be completed.";
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }
}
