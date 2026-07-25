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

    public Task<BookingSummaryResponse?> CancelBookingAsync(Guid id, CreateCancellationRequest request)
    {
        return PutAsync<CreateCancellationRequest, BookingSummaryResponse>($"api/users/bookings/{id}/cancel", request);
    }

    public Task<IEnumerable<BookingResponse>?> GetAllBookingsAsync(string? status = null, string? search = null)
    {
        var query = BuildQuery(("status", status), ("search", search));
        return GetAsync<IEnumerable<BookingResponse>>($"api/bookings{query}");
    }

    public Task<BookingResponse?> GetManagedBookingByIdAsync(Guid id)
    {
        return GetAsync<BookingResponse>($"api/bookings/{id}");
    }

    public Task<BookingResponse?> CreateBookingAsync(CreateBookingRequest request)
    {
        return PostAsync<CreateBookingRequest, BookingResponse>("api/bookings", request);
    }

    public Task<BookingResponse?> UpdateBookingAsync(Guid id, UpdateBookingRequest request)
    {
        return PutAsync<UpdateBookingRequest, BookingResponse>($"api/bookings/{id}", request);
    }

    public Task<BookingResponse?> UpdateBookingStatusAsync(Guid id, UpdateBookingStatusRequest request)
    {
        return PutAsync<UpdateBookingStatusRequest, BookingResponse>($"api/bookings/{id}/status", request);
    }

    public Task<IEnumerable<BookingResponse>?> GetCancellationRequestsAsync(string? status = null, string? search = null)
    {
        var query = BuildQuery(("status", status), ("search", search));
        return GetAsync<IEnumerable<BookingResponse>>($"api/bookings/cancellations{query}");
    }

    public Task<BookingResponse?> ReviewCancellationRequestAsync(Guid id, ReviewCancellationRequest request)
    {
        return PutAsync<ReviewCancellationRequest, BookingResponse>($"api/bookings/{id}/cancellation/review", request);
    }

    public Task<IEnumerable<BookingResponse>?> GetPaymentsAsync(string? paymentStatus = null, string? search = null)
    {
        var query = BuildQuery(("paymentStatus", paymentStatus), ("search", search));
        return GetAsync<IEnumerable<BookingResponse>>($"api/bookings/payments{query}");
    }

    public Task<BookingResponse?> UpdatePaymentAsync(Guid id, UpdatePaymentRequest request)
    {
        return PutAsync<UpdatePaymentRequest, BookingResponse>($"api/bookings/{id}/payment", request);
    }

    public Task<UserSpecialRequestResponse?> CreateSpecialRequestAsync(CreateUserSpecialRequestRequest request)
    {
        return PostAsync<CreateUserSpecialRequestRequest, UserSpecialRequestResponse>("api/users/special-requests", request);
    }

    public Task<IEnumerable<UserSpecialRequestResponse>?> GetUserSpecialRequestsAsync(string email)
    {
        return GetAsync<IEnumerable<UserSpecialRequestResponse>>($"api/users/special-requests?email={Uri.EscapeDataString(email)}");
    }

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

    public Task<IEnumerable<TourReviewResponse>?> GetReviewsByTourNameAsync(string tourName)
        => GetAsync<IEnumerable<TourReviewResponse>>($"api/tourreviews/tour/{Uri.EscapeDataString(tourName)}");

    public Task<double> GetAverageRatingAsync(string tourName)
        => GetAsync<double>($"api/tourreviews/average/{Uri.EscapeDataString(tourName)}");

    public async Task<TourReviewResponse?> GetReviewByBookingIdAsync(Guid bookingId)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync($"api/tourreviews/booking/{bookingId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TourReviewResponse>();
        return null;
    }

    public async Task<TourReviewResponse?> CreateReviewAsync(CreateTourReviewRequest request)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("You are not logged in.");

        await AddAuthorizationHeaderAsync();
        var url = $"api/tourreviews?email={Uri.EscapeDataString(session.Email)}";
        using var response = await _httpClient.PostAsJsonAsync(url, request);

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
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "Could not submit review." : body);
    }

    public async Task<TourReviewResponse?> UpdateReviewAsync(Guid id, UpdateTourReviewRequest request)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("You are not logged in.");

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
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "Could not update review." : body);
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("You are not logged in.");

        await AddAuthorizationHeaderAsync();
        var url = $"api/tourreviews/{id}?email={Uri.EscapeDataString(session.Email)}";
        using var response = await _httpClient.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(body) ? "Could not delete review." : body);
        }
    }

    public Task<IEnumerable<TourReviewResponse>?> GetAllReviewsAsync()
        => GetAsync<IEnumerable<TourReviewResponse>>("api/tourreviews");

    public async Task<TourReviewResponse?> ModerateReviewAsync(Guid id, ModerateReviewRequest request)
    {
        var session = await _sessionService.GetAsync();
        if (session == null || string.IsNullOrEmpty(session.Email))
            throw new InvalidOperationException("You are not logged in.");
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
        detail = ExtractErrorDetail(detail);
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
        detail = ExtractErrorDetail(detail);
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
        detail = ExtractErrorDetail(detail);
        throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail)
            ? "The request could not be completed."
            : detail);
    }

    // Travel Style Quiz APIs
    public Task<TravelStyleQuizResultResponse?> GetLatestQuizResultAsync()
    {
        return GetAsync<TravelStyleQuizResultResponse>("api/travelstylequiz/latest");
    }

    public Task<IEnumerable<TravelStyleQuizResultResponse>?> GetQuizHistoryAsync()
    {
        return GetAsync<IEnumerable<TravelStyleQuizResultResponse>>("api/travelstylequiz/history");
    }

    public Task<TravelStyleQuizResultResponse?> SubmitQuizResultAsync(TravelStyleQuizSubmitRequest request)
    {
        return PostAsync<TravelStyleQuizSubmitRequest, TravelStyleQuizResultResponse>("api/travelstylequiz", request);
    }

    // Admin: Quiz Questions CRUD APIs
    public Task<IEnumerable<QuizQuestionClientDto>?> GetQuizQuestionsAsync()
    {
        return GetAsync<IEnumerable<QuizQuestionClientDto>>("api/travelstylequiz/questions");
    }

    public Task<QuizQuestionClientDto?> CreateQuizQuestionAsync(QuizQuestionClientDto dto)
    {
        return PostAsync<QuizQuestionClientDto, QuizQuestionClientDto>("api/travelstylequiz/questions", dto);
    }

    public Task<QuizQuestionClientDto?> UpdateQuizQuestionAsync(Guid id, QuizQuestionClientDto dto)
    {
        return PutAsync<QuizQuestionClientDto, QuizQuestionClientDto>($"api/travelstylequiz/questions/{id}", dto);
    }

    public async Task DeleteQuizQuestionAsync(Guid id)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/travelstylequiz/questions/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var detail = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(detail)
                ? "The request could not be completed."
                : detail);
        }
    }

    public Task<AdminUserPage?> GetAdminUsersAsync(string keyword,string role,string status,bool? phoneVerified,int page=1)=>GetAsync<AdminUserPage>($"api/admin/users?Keyword={Uri.EscapeDataString(keyword)}&Role={Uri.EscapeDataString(role)}&Status={Uri.EscapeDataString(status)}&PhoneVerified={phoneVerified}&Page={page}&PageSize=10");
    public Task<AdminUserDetail?> GetAdminUserAsync(Guid id)=>GetAsync<AdminUserDetail>($"api/admin/users/{id}");
    public Task<AdminUserDetail?> ChangeUserRoleAsync(Guid id,ChangeRoleRequest r)=>PutAsync<ChangeRoleRequest,AdminUserDetail>($"api/admin/users/{id}/roles",r);
    public Task<AdminUserDetail?> LockUserAsync(Guid id,LockUserRequest r)=>PostAsync<LockUserRequest,AdminUserDetail>($"api/admin/users/{id}/lock",r);
    public Task<AdminUserDetail?> UnlockUserAsync(Guid id,UnlockUserRequest r)=>PostAsync<UnlockUserRequest,AdminUserDetail>($"api/admin/users/{id}/unlock",r);
    public Task<SendPhoneOtpResponse?> SendPhoneOtpAsync(SendPhoneOtpRequest r)=>PostAsync<SendPhoneOtpRequest,SendPhoneOtpResponse>("api/auth/phone/send-otp",r);
    public Task<object?> VerifyPhoneOtpAsync(VerifyPhoneOtpRequest r)=>PostAsync<VerifyPhoneOtpRequest,object>("api/auth/phone/verify-otp",r);
    public Task<DashboardSummary?> GetDashboardSummaryAsync(DashboardFilter filter) => GetAsync<DashboardSummary>($"api/admin/dashboard/summary?{BuildDashboardQuery(filter)}");
    public Task<List<BookingStatusPoint>?> GetDashboardBookingsAsync(DashboardFilter filter) => GetAsync<List<BookingStatusPoint>>($"api/admin/dashboard/bookings-by-status?{BuildDashboardQuery(filter)}");
    public Task<RevenueByTourPage?> GetDashboardRevenueAsync(DashboardFilter filter) => GetAsync<RevenueByTourPage>($"api/admin/dashboard/revenue-by-tour?{BuildDashboardQuery(filter)}");
    public Task<List<TopGuideItem>?> GetDashboardGuidesAsync(DashboardFilter filter) => GetAsync<List<TopGuideItem>>($"api/admin/dashboard/top-guides?{BuildDashboardQuery(filter)}");
    private static string BuildDashboardQuery(DashboardFilter f){var q=new List<string>{$"Page={f.Page}",$"PageSize={f.PageSize}"};if(f.FromDate.HasValue)q.Add($"FromDate={f.FromDate:yyyy-MM-dd}");if(f.ToDate.HasValue)q.Add($"ToDate={f.ToDate:yyyy-MM-dd}");if(!string.IsNullOrWhiteSpace(f.TourCode))q.Add($"TourCode={Uri.EscapeDataString(f.TourCode)}");if(!string.IsNullOrWhiteSpace(f.Destination))q.Add($"Destination={Uri.EscapeDataString(f.Destination)}");if(!string.IsNullOrWhiteSpace(f.Status))q.Add($"Status={Uri.EscapeDataString(f.Status)}");return string.Join("&",q);}
    public Task<RecommendationPageResponse?> GetRecommendationsAsync(RecommendationFilter filter, bool personalized)
    {
        var endpoint = personalized ? "api/recommendations/me" : "api/tours/search";
        return GetAsync<RecommendationPageResponse>($"{endpoint}?{BuildRecommendationQuery(filter)}");
    }

    public Task<RecommendedTourResponse?> GetRandomTourAsync(RecommendationFilter filter, IEnumerable<Guid> excludedTourIds)
    {
        var exclude = string.Join("&", excludedTourIds.Select(id => $"ExcludeTourIds={id}"));
        var query = BuildRecommendationQuery(filter);
        return GetAsync<RecommendedTourResponse>($"api/tours/random?{query}{(string.IsNullOrEmpty(exclude) ? "" : "&" + exclude)}");
    }

    private static string BuildRecommendationQuery(RecommendationFilter filter)
    {
        var values = new List<string>
        {
            $"Page={filter.Page}", $"PageSize={filter.PageSize}", $"Sort={Uri.EscapeDataString(filter.Sort)}", "AvailableOnly=true"
        };
        if (filter.BudgetMin.HasValue) values.Add($"BudgetMin={filter.BudgetMin.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        if (filter.BudgetMax.HasValue) values.Add($"BudgetMax={filter.BudgetMax.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        if (!string.IsNullOrWhiteSpace(filter.Destination)) values.Add($"Destination={Uri.EscapeDataString(filter.Destination.Trim())}");
        if (!string.IsNullOrWhiteSpace(filter.TourType)) values.Add($"TourType={Uri.EscapeDataString(filter.TourType.Trim())}");
        if (filter.StartDateFrom.HasValue) values.Add($"StartDateFrom={filter.StartDateFrom.Value:yyyy-MM-dd}");
        if (filter.StartDateTo.HasValue) values.Add($"StartDateTo={filter.StartDateTo.Value:yyyy-MM-dd}");
        if (filter.DurationMin.HasValue) values.Add($"DurationMin={filter.DurationMin.Value}");
        if (filter.DurationMax.HasValue) values.Add($"DurationMax={filter.DurationMax.Value}");
        if (filter.RatingMin.HasValue) values.Add($"RatingMin={filter.RatingMin.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
        return string.Join("&", values);
    }
    private async Task AddAuthorizationHeaderAsync()
    {
        var session = await _sessionService.GetAsync();
        if (session != null && !string.IsNullOrEmpty(session.Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);
        }
    }

    private static string BuildQuery(params (string Key, string? Value)[] values)
    {
        var parts = values
            .Where(item => !string.IsNullOrWhiteSpace(item.Value))
            .Select(item => $"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value!)}")
            .ToArray();

        return parts.Length == 0 ? string.Empty : $"?{string.Join("&", parts)}";
    }

    private static string ExtractErrorDetail(string detail)
    {
        if (string.IsNullOrWhiteSpace(detail))
        {
            return detail;
        }

        try
        {
            using var json = System.Text.Json.JsonDocument.Parse(detail);
            if (json.RootElement.TryGetProperty("detail", out var detailProperty))
            {
                return detailProperty.GetString() ?? detail;
            }

            if (json.RootElement.TryGetProperty("error", out var errorProperty))
            {
                return errorProperty.GetString() ?? detail;
            }
        }
        catch (System.Text.Json.JsonException)
        {
        }

        return detail;
    }
}
