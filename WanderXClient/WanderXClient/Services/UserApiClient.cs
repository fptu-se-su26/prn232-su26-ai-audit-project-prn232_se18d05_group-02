using System.Net.Http.Headers;
using System.Net.Http.Json;
using WanderXClient.Models;

namespace WanderXClient.Services;

public sealed class UserApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthSessionService _sessionService;

    public UserApiClient(HttpClient httpClient, AuthSessionService sessionService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
    }

    public Task<UserProfileResponse?> GetProfileAsync(string email)
    {
        return GetAsync<UserProfileResponse>($"api/users/profile?email={Uri.EscapeDataString(email)}");
    }

    public Task<UserProfileResponse?> UpdateProfileAsync(string email, UpdateProfileRequest request)
    {
        return PutAsync<UpdateProfileRequest, UserProfileResponse>($"api/users/profile?email={Uri.EscapeDataString(email)}", request);
    }

    public Task<IEnumerable<BookingSummaryResponse>?> GetBookingsAsync(string email)
    {
        return GetAsync<IEnumerable<BookingSummaryResponse>>($"api/users/bookings?email={Uri.EscapeDataString(email)}");
    }

    public Task<BookingSummaryResponse?> GetBookingByIdAsync(Guid id)
    {
        return GetAsync<BookingSummaryResponse>($"api/users/bookings/{id}");
    }

    public Task<BookingSummaryResponse?> CancelBookingAsync(Guid id)
    {
        return PutAsync<object, BookingSummaryResponse>($"api/users/bookings/{id}/cancel", new { });
    }

    public Task<UserSpecialRequestResponse?> CreateSpecialRequestAsync(CreateUserSpecialRequestRequest request)
    {
        return PostAsync<CreateUserSpecialRequestRequest, UserSpecialRequestResponse>("api/users/special-requests", request);
    }

    public Task<IEnumerable<UserSpecialRequestResponse>?> GetUserSpecialRequestsAsync(string email)
    {
        return GetAsync<IEnumerable<UserSpecialRequestResponse>>($"api/users/special-requests?email={Uri.EscapeDataString(email)}");
    }

    // TV3 - Admin methods
    public Task<IEnumerable<BookingWithRequestCountResponse>?> GetAllBookingsWithRequestCountsAsync()
    {
        return GetAsync<IEnumerable<BookingWithRequestCountResponse>>("api/users/admin/bookings-with-requests");
    }

    public Task<IEnumerable<UserSpecialRequestResponse>?> GetSpecialRequestsByBookingAsync(Guid bookingId)
    {
        return GetAsync<IEnumerable<UserSpecialRequestResponse>>($"api/users/admin/bookings/{bookingId}/special-requests");
    }

    public Task<UserSpecialRequestResponse?> ReviewSpecialRequestAsync(Guid requestId, UpdateUserSpecialRequestRequest request)
    {
        return PutAsync<UpdateUserSpecialRequestRequest, UserSpecialRequestResponse>($"api/users/admin/special-requests/{requestId}/review", request);
    }
    // end TV3

    // Tour Reviews
    public Task<IEnumerable<TourReviewResponse>?> GetReviewsByTourNameAsync(string tourName)
        => GetAsync<IEnumerable<TourReviewResponse>>($"api/tourreviews/tour/{Uri.EscapeDataString(tourName)}");

    public Task<double> GetAverageRatingAsync(string tourName)
        => GetAsync<double>($"api/tourreviews/average/{Uri.EscapeDataString(tourName)}");

    public async Task<TourReviewResponse?> GetReviewByBookingIdAsync(Guid bookingId)
    {
        // GET api/tourreviews/booking/{bookingId} trả về 204 nếu chưa có review
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync($"api/tourreviews/booking/{bookingId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TourReviewResponse>();
        return null; // Bất kỳ lỗi nào cũng trả về null (không có review)
    }

    public async Task<TourReviewResponse?> CreateReviewAsync(CreateTourReviewRequest request)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("Bạn chưa đăng nhập.");

        await AddAuthorizationHeaderAsync();
        var url = $"api/tourreviews?email={Uri.EscapeDataString(session.Email)}";
        using var response = await _httpClient.PostAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TourReviewResponse>();

        var body = await response.Content.ReadAsStringAsync();
        // Thử parse JSON error
        try
        {
            var err = System.Text.Json.JsonDocument.Parse(body);
            if (err.RootElement.TryGetProperty("error", out var errProp))
                throw new InvalidOperationException(errProp.GetString() ?? body);
        }
        catch (System.Text.Json.JsonException) { }
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "Không thể gửi đánh giá." : body);
    }

    public async Task<TourReviewResponse?> UpdateReviewAsync(Guid id, UpdateTourReviewRequest request)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("Bạn chưa đăng nhập.");

        await AddAuthorizationHeaderAsync();
        var url = $"api/tourreviews/{id}?email={Uri.EscapeDataString(session.Email)}";
        using var response = await _httpClient.PutAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TourReviewResponse>();

        var body = await response.Content.ReadAsStringAsync();
        try
        {
            var err = System.Text.Json.JsonDocument.Parse(body);
            if (err.RootElement.TryGetProperty("error", out var errProp))
                throw new InvalidOperationException(errProp.GetString() ?? body);
        }
        catch (System.Text.Json.JsonException) { }
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "Không thể cập nhật đánh giá." : body);
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("Bạn chưa đăng nhập.");

        await AddAuthorizationHeaderAsync();
        var url = $"api/tourreviews/{id}?email={Uri.EscapeDataString(session.Email)}";
        using var response = await _httpClient.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "Không thể xóa đánh giá." : body);
        }
    }

    public Task<IEnumerable<TourReviewResponse>?> GetAllReviewsAsync()
        => GetAsync<IEnumerable<TourReviewResponse>>("api/tourreviews");

    public async Task<TourReviewResponse?> ModerateReviewAsync(Guid id, ModerateReviewRequest request)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("Bạn chưa đăng nhập.");
        return await PutAsync<ModerateReviewRequest, TourReviewResponse>(
            $"api/tourreviews/{id}/moderate?email={Uri.EscapeDataString(session.Email)}", request);
    }

    private async Task<TResponse?> GetAsync<TResponse>(string path)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync(path);

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return default;
        }

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        var detail = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail)
            ? "The request could not be completed."
            : detail);
    }

    private async Task<TResponse?> PutAsync<TRequest, TResponse>(string path, TRequest request)
    {
        await AddAuthorizationHeaderAsync();
        using var response = await _httpClient.PutAsJsonAsync(path, request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        var detail = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail)
            ? "The request could not be completed."
            : detail);
    }

    private async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest request)
    {
        await AddAuthorizationHeaderAsync();
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

    private async Task AddAuthorizationHeaderAsync()
    {
        var session = await _sessionService.GetAsync();
        if (session != null && !string.IsNullOrEmpty(session.Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);
        }
    }
}
