using WanderXServer.BusinessObject;
using WanderXServer.Dtos.Users;

namespace WanderXServer.Services;

public interface IUserService
{
    Task<UserProfileResponse> GetProfileAsync(string email);
    Task<UserProfileResponse> UpdateProfileAsync(string email, UpdateProfileRequest request);
    Task<IEnumerable<BookingSummaryResponse>> GetBookingsAsync(string email);
    Task<BookingSummaryResponse?> GetBookingByIdAsync(Guid id); // TV3
    Task<BookingSummaryResponse?> CancelBookingAsync(Guid id, CreateCancellationRequest request);
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
}
