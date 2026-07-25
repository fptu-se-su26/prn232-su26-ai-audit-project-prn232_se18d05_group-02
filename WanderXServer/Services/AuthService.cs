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

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreateAuthResponse(user, "Account created. Sign in and verify the phone number from Account Security.");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail);

        if (user is null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        if (user.AccountStatus == "Inactive" || (user.AccountStatus == "Locked" && (!user.LockoutEnd.HasValue || user.LockoutEnd > DateTime.UtcNow)))
        {
            throw new InvalidOperationException("ACCOUNT_LOCKED");
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

    public Task<MessageResponse> VerifyPhoneAsync(VerifyPhoneRequest request)
    {
        throw new InvalidOperationException("LEGACY_PHONE_VERIFICATION_DISABLED");
    }
    public async Task<AuthResponse> ResendVerificationCodeAsync(ForgotPasswordRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail)
            ?? throw new InvalidOperationException("Account not found.");
        return CreateAuthResponse(user, "Use the authenticated /api/auth/phone/send-otp endpoint.");
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
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("token_version", user.TokenVersion.ToString())
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
