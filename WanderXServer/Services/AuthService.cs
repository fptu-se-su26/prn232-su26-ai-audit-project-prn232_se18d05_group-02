using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    private readonly IEmailSender _emailSender;
    private readonly EmailOptions _emailOptions;

    public AuthService(WanderXDbContext dbContext, IConfiguration configuration, IEmailSender emailSender, IOptions<EmailOptions> emailOptions)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _emailSender = emailSender;
        _emailOptions = emailOptions.Value;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var exists = await _dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail);

        if (exists)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var pending = new PendingRegistration
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            PhoneNumber = request.PhoneNumber.Trim(),
            Code = GenerateVerificationCode(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        var passwordUser = new ApplicationUser
        {
            Id = pending.Id,
            Email = pending.Email,
            NormalizedEmail = pending.NormalizedEmail
        };
        pending.PasswordHash = _passwordHasher.HashPassword(passwordUser, request.Password);

        await _dbContext.PendingRegistrations
            .Where(item => item.NormalizedEmail == normalizedEmail && item.ConsumedAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(item => item.ConsumedAt, DateTime.UtcNow));

        _dbContext.PendingRegistrations.Add(pending);
        await _dbContext.SaveChangesAsync();

        if (!_emailOptions.IsConfigured)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Email service is not configured. Please configure SMTP before registering accounts.");
        }

        try
        {
            await SendRegistrationVerificationEmailAsync(pending.Email, pending.Code);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Could not send the verification OTP. The account was not created.");
        }

        await transaction.CommitAsync();

        return CreatePendingRegistrationResponse(
            pending,
            "Registration OTP sent to your email. Enter the code to create your account.");
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

        if (!user.IsEmailConfirmed || !user.IsPhoneConfirmed)
        {
            throw new InvalidOperationException("ACCOUNT_NOT_VERIFIED");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return CreateAuthResponse(user, "Signed in successfully.");
    }

    public async Task<AuthResponse> GoogleLoginAsync(string email, string fullName)
    {
        var normalizedEmail = NormalizeEmail(email);
        var user = await _dbContext.Users.FirstOrDefaultAsync(item => item.NormalizedEmail == normalizedEmail);
        if (user is null)
        {
            user = new ApplicationUser
            {
                FullName = string.IsNullOrWhiteSpace(fullName) ? email.Split('@')[0] : fullName.Trim(),
                Email = email.Trim(),
                NormalizedEmail = normalizedEmail,
                PhoneNumber = string.Empty,
                Role = UserRole.Customer,
                IsEmailConfirmed = true,
                IsPhoneConfirmed = true,
                AccountStatus = "Active"
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, Convert.ToHexString(RandomNumberGenerator.GetBytes(32)));
            _dbContext.Users.Add(user);
        }
        if (user.AccountStatus == "Inactive" || (user.AccountStatus == "Locked" && (!user.LockoutEnd.HasValue || user.LockoutEnd > DateTime.UtcNow)))
            throw new InvalidOperationException("ACCOUNT_LOCKED");
        user.IsEmailConfirmed = true;
        user.LastLoginAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return CreateAuthResponse(user, "Signed in with Google successfully.");
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
        var userExists = await _dbContext.Users.AnyAsync(item => item.NormalizedEmail == normalizedEmail);

        if (userExists)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        var pending = await _dbContext.PendingRegistrations
            .Where(item =>
                item.NormalizedEmail == normalizedEmail &&
                item.ConsumedAt == null)
            .OrderByDescending(item => item.CreatedAt)
            .FirstOrDefaultAsync();

        if (pending is null)
        {
            throw new InvalidOperationException("Verification code was not found. Please request a new code.");
        }

        if (pending.ExpiresAt < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Verification code has expired. Please request a new code.");
        }

        if (pending.Code != request.Code.Trim())
        {
            throw new InvalidOperationException("Verification code is invalid.");
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = new ApplicationUser
        {
            FullName = pending.FullName,
            Email = pending.Email,
            NormalizedEmail = pending.NormalizedEmail,
            PhoneNumber = pending.PhoneNumber,
            PasswordHash = pending.PasswordHash,
            Role = UserRole.Customer,
            IsEmailConfirmed = true,
            IsPhoneConfirmed = true,
            AccountStatus = "Active"
        };

        pending.ConsumedAt = DateTime.UtcNow;
        _dbContext.Users.Add(user);

        await _dbContext.PendingRegistrations
            .Where(item =>
                item.NormalizedEmail == normalizedEmail &&
                item.ConsumedAt == null &&
                item.Id != pending.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(item => item.ConsumedAt, DateTime.UtcNow));

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return new MessageResponse { Message = "Account created successfully. You can sign in now." };
    }
    public async Task<AuthResponse> ResendVerificationCodeAsync(ForgotPasswordRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var userExists = await _dbContext.Users.AnyAsync(item => item.NormalizedEmail == normalizedEmail);
        if (userExists)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var existingPending = await _dbContext.PendingRegistrations
            .Where(item => item.NormalizedEmail == normalizedEmail && item.ConsumedAt == null)
            .OrderByDescending(item => item.CreatedAt)
            .FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("Registration request was not found. Please register again.");

        existingPending.Code = GenerateVerificationCode();
        existingPending.ExpiresAt = DateTime.UtcNow.AddMinutes(10);
        existingPending.CreatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        if (!_emailOptions.IsConfigured)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Email service is not configured. Please configure SMTP before resending OTP.");
        }

        try
        {
            await SendRegistrationVerificationEmailAsync(existingPending.Email, existingPending.Code);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Could not send the verification OTP. Please try again.");
        }

        await transaction.CommitAsync();

        return CreatePendingRegistrationResponse(
            existingPending,
            "Verification code sent to your email.");
    }
    public static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }

    private static string GenerateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
    }

    private Task SendRegistrationVerificationEmailAsync(string email, string code)
    {
        return _emailSender.SendAsync(
            email,
            "Your WanderX registration OTP",
            $"Your WanderX registration OTP is {code}. It expires in 10 minutes.");
    }

    private static AuthResponse CreatePendingRegistrationResponse(PendingRegistration pending, string message, string? verificationCode = null)
    {
        return new AuthResponse
        {
            Message = message,
            Token = string.Empty,
            Email = pending.Email,
            FullName = pending.FullName,
            Role = UserRole.Customer.ToString(),
            VerificationCode = verificationCode
        };
    }

    private AuthResponse CreateAuthResponse(ApplicationUser user, string message, string? verificationCode = null, bool issueToken = true)
    {
        if (!issueToken)
        {
            return new AuthResponse
            {
                Message = message,
                Token = string.Empty,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                VerificationCode = verificationCode
            };
        }

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
