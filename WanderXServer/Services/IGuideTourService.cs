using WanderXServer.Dtos.GuideTours;

namespace WanderXServer.Services;

public interface IGuideTourService
{
    Task<IReadOnlyList<GuideTourAssignmentResponse>> GetAllAsync(string? guideEmail, string? search);

    Task<IReadOnlyList<GuideTourAssignmentResponse>> GetScheduleAsync(string guideEmail);

    Task<GuideTourAssignmentResponse> GetByIdAsync(Guid id);

    Task<GuideTourAssignmentResponse> CreateAsync(CreateGuideTourAssignmentRequest request);

    Task<GuideTourAssignmentResponse> ChangeGuideAsync(Guid id, ChangeGuideAssignmentRequest request);

    Task<GuideTourAssignmentResponse> DeclineAsync(Guid id, DeclineTourRequest request);

    Task<GuideTourAssignmentResponse> FinishAsync(Guid id);
}
