using WanderXServer.Dtos.Tours;

namespace WanderXServer.Services;

public interface ITourService
{
    Task<IReadOnlyList<TourResponse>> GetAllAsync(string? search, string? status, string? destination, DateTime? departureDate);

    Task<TourResponse> GetByIdAsync(Guid id);

    Task<TourResponse> CreateAsync(CreateTourRequest request);

    Task<TourResponse> UpdateAsync(Guid id, UpdateTourRequest request);

    Task<TourResponse> HideAsync(Guid id);

    Task<TourResponse> LockAsync(Guid id);

    Task<TourResponse> UnlockAsync(Guid id);

    Task EnsureTourCanAcceptBookingAsync(string? tourCode, int requestedGuestCount, DateTime departureDate, Guid? excludedBookingId = null);

    Task RefreshTourAvailabilityAsync(string? tourCode);

    Task DeleteAsync(Guid id);
}
