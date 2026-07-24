using System.Net.Http.Json;
using System.Text.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class TourPricingApiClient
{
    private readonly HttpClient _httpClient;

    public TourPricingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TourPricingResponse> GetPricingAsync(Guid tourId, DateTime? previewDate = null)
    {
        var path = previewDate.HasValue
            ? $"api/tour-pricing/{tourId}?previewDate={Uri.EscapeDataString(previewDate.Value.ToString("yyyy-MM-dd"))}"
            : $"api/tour-pricing/{tourId}";

        using var response = await _httpClient.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TourPricingResponse>() ?? new TourPricingResponse();
        }

        throw new InvalidOperationException(await ReadErrorAsync(response));
    }

    public Task<TourSeasonPriceResponse?> CreateSeasonPriceAsync(Guid tourId, UpsertTourSeasonPriceRequest request)
    {
        return SendAsync<UpsertTourSeasonPriceRequest, TourSeasonPriceResponse>(HttpMethod.Post, $"api/tour-pricing/{tourId}/season-prices", request);
    }

    public Task<TourSeasonPriceResponse?> UpdateSeasonPriceAsync(Guid id, UpsertTourSeasonPriceRequest request)
    {
        return SendAsync<UpsertTourSeasonPriceRequest, TourSeasonPriceResponse>(HttpMethod.Put, $"api/tour-pricing/season-prices/{id}", request);
    }

    public Task DeleteSeasonPriceAsync(Guid id)
    {
        return DeleteAsync($"api/tour-pricing/season-prices/{id}");
    }

    public Task<TourPromotionResponse?> CreatePromotionAsync(Guid tourId, UpsertTourPromotionRequest request)
    {
        return SendAsync<UpsertTourPromotionRequest, TourPromotionResponse>(HttpMethod.Post, $"api/tour-pricing/{tourId}/promotions", request);
    }

    public Task<TourPromotionResponse?> UpdatePromotionAsync(Guid id, UpsertTourPromotionRequest request)
    {
        return SendAsync<UpsertTourPromotionRequest, TourPromotionResponse>(HttpMethod.Put, $"api/tour-pricing/promotions/{id}", request);
    }

    public Task DeletePromotionAsync(Guid id)
    {
        return DeleteAsync($"api/tour-pricing/promotions/{id}");
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

    private async Task DeleteAsync(string path)
    {
        using var response = await _httpClient.DeleteAsync(path);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(await ReadErrorAsync(response));
        }
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        var detail = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(detail))
        {
            return "The pricing request could not be completed.";
        }

        try
        {
            using var document = JsonDocument.Parse(detail);

            if (document.RootElement.TryGetProperty("detail", out var problemDetail))
            {
                return problemDetail.GetString() ?? "The pricing request could not be completed.";
            }

            if (document.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? "The pricing request could not be completed.";
            }
        }
        catch (JsonException)
        {
            return detail;
        }

        return detail;
    }
}
