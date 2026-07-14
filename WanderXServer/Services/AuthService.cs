using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WanderXServer.BusinessObject;
using WanderXServer.BusinessObject.Enums;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Auth;

namespace WanderXServer.Services;

public class AuthService : IAuthService
{
    private readonly WanderXDbContext _dbContext;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();
    private readonly IConfiguration _configuration;

    public AuthService(WanderXDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var exists = await _dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail);

        if (exists)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            PhoneNumber = request.PhoneNumber.Trim(),
            Role = UserRole.Customer
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        var verificationCode = CreateVerificationCode(user.Id);
        user.VerificationCodes.Add(verificationCode);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreateAuthResponse(user, "Account created. Verify the phone number to finish onboarding.", verificationCode.Code);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail);

        if (user is null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return CreateAuthResponse(user, "Signed in successfully.");
    }

    public async Task<MessageResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail);

        if (user is null)
        {
            return new MessageResponse { Message = "If the email exists, a reset link has been prepared." };
        }

        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(30)
        };

        _dbContext.PasswordResetTokens.Add(resetToken);
        await _dbContext.SaveChangesAsync();

        return new MessageResponse
        {
            Message = "Reset token generated. Wire this token into an email provider for production.",
            ResetToken = resetToken.Token
        };
    }

    public async Task<MessageResponse> VerifyPhoneAsync(VerifyPhoneRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users
            .Include(item => item.VerificationCodes)
            .FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail);

        if (user is null)
        {
            throw new InvalidOperationException("Account not found.");
        }

        var verificationCode = user.VerificationCodes
            .Where(item => item.Purpose == VerificationPurpose.PhoneVerification)
            .Where(item => item.ConsumedAt == null)
            .Where(item => item.ExpiresAt >= DateTime.UtcNow)
            .OrderByDescending(item => item.CreatedAt)
            .FirstOrDefault(item => item.Code == request.Code);

        if (verificationCode is null)
        {
            throw new InvalidOperationException("Invalid or expired verification code.");
        }

        verificationCode.ConsumedAt = DateTime.UtcNow;
        user.IsPhoneConfirmed = true;
        user.IsEmailConfirmed = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new MessageResponse { Message = "Account verified. You can sign in now." };
    }

    public async Task<AuthResponse> ResendVerificationCodeAsync(ForgotPasswordRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail);

        if (user is null)
        {
            throw new InvalidOperationException("Account not found.");
        }

        var verificationCode = CreateVerificationCode(user.Id);
        _dbContext.VerificationCodes.Add(verificationCode);
        await _dbContext.SaveChangesAsync();

        return CreateAuthResponse(user, "Verification code sent.", verificationCode.Code);
    }

    public static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }

    private static AuthVerificationCode CreateVerificationCode(Guid userId)
    {
        return new AuthVerificationCode
        {
            UserId = userId,
            Code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6"),
            Purpose = VerificationPurpose.PhoneVerification,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };
    }

    private AuthResponse CreateAuthResponse(ApplicationUser user, string message, string? verificationCode = null)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "WanderX";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "WanderXClient";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new AuthResponse
        {
            Message = message,
            Token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            VerificationCode = verificationCode
        };
    }
}
