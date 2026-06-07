using Microsoft.AspNetCore.Mvc;
using WanderXServer.Dtos.Auth;
using WanderXServer.Services;

namespace WanderXServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            return Ok(await _authService.RegisterAsync(request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Registration failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            return Ok(await _authService.LoginAsync(request));
        }
        catch (InvalidOperationException exception)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Login failed",
                Detail = exception.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<MessageResponse>> ForgotPassword(ForgotPasswordRequest request)
    {
        return Ok(await _authService.ForgotPasswordAsync(request));
    }

    [HttpPost("verify-phone")]
    public async Task<ActionResult<MessageResponse>> VerifyPhone(VerifyPhoneRequest request)
    {
        try
        {
            return Ok(await _authService.VerifyPhoneAsync(request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Verification failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("resend-verification-code")]
    public async Task<ActionResult<AuthResponse>> ResendVerificationCode(ForgotPasswordRequest request)
    {
        try
        {
            return Ok(await _authService.ResendVerificationCodeAsync(request));
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Verification code failed",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}
