using WanderXServer.Dtos.Auth;

namespace WanderXServer.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<MessageResponse> ForgotPasswordAsync(ForgotPasswordRequest request);

    Task<MessageResponse> VerifyPhoneAsync(VerifyPhoneRequest request);

    Task<AuthResponse> ResendVerificationCodeAsync(ForgotPasswordRequest request);
}
