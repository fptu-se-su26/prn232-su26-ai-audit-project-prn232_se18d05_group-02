using WanderXServer.Dtos.Tours;

namespace WanderXServer.Services;

public interface ITourService
{
    Task<IReadOnlyList<TourResponse>> GetAllAsync(string? search, string? status);

    Task<TourResponse> GetByIdAsync(Guid id);

    Task<TourResponse> CreateAsync(CreateTourRequest request);

    Task<TourResponse> UpdateAsync(Guid id, UpdateTourRequest request);

    Task DeleteAsync(Guid id);
}
