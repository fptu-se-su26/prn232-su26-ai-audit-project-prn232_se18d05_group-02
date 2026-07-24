using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class TourScheduleApiClient
{
    private readonly HttpClient _httpClient;

    public TourScheduleApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TourScheduleResponse>> GetSchedulesAsync(int? tourId = null, DateTime? filterDate = null, int? month = null, int? year = null)
    {
        var query = new List<string>();

        if (tourId is int selectedTourId && selectedTourId > 0)
        {
            query.Add($"tourId={selectedTourId}");
        }

        if (filterDate is DateTime date)
        {
            query.Add($"filterDate={Uri.EscapeDataString(date.ToString("yyyy-MM-dd"))}");
        }

        if (month is int selectedMonth && selectedMonth > 0)
        {
            query.Add($"month={selectedMonth}");
        }

        if (year is int selectedYear && selectedYear > 0)
        {
            query.Add($"year={selectedYear}");
        }

        var path = query.Count == 0
            ? "api/tour-schedules"
            : $"api/tour-schedules?{string.Join("&", query)}";

        return await GetAsync<List<TourScheduleResponse>>(path) ?? new List<TourScheduleResponse>();
    }

    public async Task<List<TourScheduleTourOptionResponse>> GetTourOptionsAsync()
    {
        return await GetAsync<List<TourScheduleTourOptionResponse>>("api/tour-schedules/tour-options")
            ?? new List<TourScheduleTourOptionResponse>();
    }

    public Task<TourScheduleResponse?> CreateAsync(CreateTourScheduleRequest request)
    {
        return SendAsync<CreateTourScheduleRequest, TourScheduleResponse>(HttpMethod.Post, "api/tour-schedules", request);
    }

    public Task<TourScheduleResponse?> UpdateAsync(int id, UpdateTourScheduleRequest request)
    {
        return SendAsync<UpdateTourScheduleRequest, TourScheduleResponse>(HttpMethod.Put, $"api/tour-schedules/{id}", request);
    }

    public async Task DeleteAsync(int id)
    {
        using var response = await _httpClient.DeleteAsync($"api/tour-schedules/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ReadErrorAsync(response));
        }
    }

    public async Task MoveUpAsync(int id)
    {
        await PostCommandAsync($"api/tour-schedules/{id}/move-up");
    }

    public async Task MoveDownAsync(int id)
    {
        await PostCommandAsync($"api/tour-schedules/{id}/move-down");
    }

    private async Task PostCommandAsync(string path)
    {
        using var response = await _httpClient.PostAsync(path, null);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ReadErrorAsync(response));
        }
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
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return "Tour schedule API was not found. Please restart WanderXServer so the new /api/tour-schedules endpoint is loaded.";
        }

        var detail = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(detail))
        {
            return "The tour schedule request could not be completed.";
        }

        try
        {
            using var document = JsonDocument.Parse(detail);

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The tour schedule request could not be completed.";
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }
}
