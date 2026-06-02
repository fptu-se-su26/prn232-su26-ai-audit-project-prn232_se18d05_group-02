using WanderXServer.Dtos.GuideTours;

namespace WanderXServer.Services;

public interface IGuideTourService
{
    Task<IReadOnlyList<GuideTourAssignmentResponse>> GetScheduleAsync(string guideEmail);

    Task<GuideTourAssignmentResponse> GetByIdAsync(Guid id);

    Task<GuideTourAssignmentResponse> DeclineAsync(Guid id, DeclineTourRequest request);

    Task<GuideTourAssignmentResponse> FinishAsync(Guid id);
}
