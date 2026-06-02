using WanderXServer.Dtos.Guides;

namespace WanderXServer.Services;

public interface IGuideService
{
    Task<IReadOnlyList<GuideResponse>> GetAllAsync(string? search);

    Task<GuideResponse> GetByIdAsync(Guid id);

    Task<GuideResponse> GetByEmailAsync(string email);

    Task<bool> EmailExistsAsync(string email);

    Task<GuideResponse> CreateAsync(CreateGuideRequest request);

    Task<GuideResponse> UpdateAsync(Guid id, UpdateGuideRequest request);

    Task<GuideResponse> SelfUpdateAsync(GuideSelfUpdateRequest request);
}
