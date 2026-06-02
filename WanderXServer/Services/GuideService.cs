using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.BusinessObject.Enums;
using WanderXServer.DataAccessLayer;
using WanderXServer.Dtos.Guides;

namespace WanderXServer.Services;

public class GuideService : IGuideService
{
    private readonly WanderXDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<GuideService> _logger;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

    public GuideService(WanderXDbContext dbContext, IEmailSender emailSender, ILogger<GuideService> logger)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<IReadOnlyList<GuideResponse>> GetAllAsync(string? search)
    {
        var profiles = await _dbContext.GuideProfiles
            .Include(profile => profile.User)
            .OrderBy(profile => profile.User.FullName)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            profiles = profiles
                .Where(profile =>
                    Contains(profile.User.FullName, term) ||
                    Contains(profile.User.Email, term) ||
                    Contains(profile.Languages, term) ||
                    Contains(profile.ExpertiseArea, term) ||
                    Contains(profile.Region, term))
                .ToList();
        }

        return profiles.Select(ToResponse).ToList();
    }

    public async Task<GuideResponse> GetByIdAsync(Guid id)
    {
        var profile = await FindProfileByIdAsync(id);
        return ToResponse(profile);
    }

    public async Task<GuideResponse> GetByEmailAsync(string email)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        var profile = await _dbContext.GuideProfiles
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.User.NormalizedEmail == normalizedEmail);

        if (profile is null)
        {
            throw new KeyNotFoundException("Guide profile was not found.");
        }

        return ToResponse(profile);
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        var normalizedEmail = AuthService.NormalizeEmail(email);
        return _dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail);
    }

    public async Task<GuideResponse> CreateAsync(CreateGuideRequest request)
    {
        var normalizedEmail = AuthService.NormalizeEmail(request.Email);
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
            Role = UserRole.Guide,
            IsEmailConfirmed = true,
            IsPhoneConfirmed = true
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        var profile = new GuideProfile
        {
            User = user,
            Languages = NormalizeCsv(request.Languages),
            ExpertiseArea = request.ExpertiseArea.Trim(),
            Region = request.Region.Trim(),
            CompletedTours = request.CompletedTours,
            Status = NormalizeStatus(request.Status),
            Bio = request.Bio.Trim()
        };

        _dbContext.GuideProfiles.Add(profile);
        await _dbContext.SaveChangesAsync();

        try
        {
            await SendGuideAccountEmailAsync(user, request.Password);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Guide account was created but notification email failed. GuideEmail={Email}", user.Email);
        }

        return ToResponse(profile);
    }

    public async Task<GuideResponse> UpdateAsync(Guid id, UpdateGuideRequest request)
    {
        var profile = await FindProfileByIdAsync(id);
        var normalizedEmail = AuthService.NormalizeEmail(request.Email);
        var emailTaken = await _dbContext.Users
            .AnyAsync(user => user.Id != profile.UserId && user.NormalizedEmail == normalizedEmail);

        if (emailTaken)
        {
            throw new InvalidOperationException("An account with this email already exists.");
        }

        profile.User.FullName = request.FullName.Trim();
        profile.User.Email = request.Email.Trim();
        profile.User.NormalizedEmail = normalizedEmail;
        profile.User.PhoneNumber = request.PhoneNumber.Trim();
        profile.User.Role = UserRole.Guide;
        profile.User.UpdatedAt = DateTime.UtcNow;
        ApplyProfileUpdate(profile, request.Languages, request.ExpertiseArea, request.Region, request.CompletedTours, request.Bio);
        profile.Status = NormalizeStatus(request.Status);

        await _dbContext.SaveChangesAsync();
        return ToResponse(profile);
    }

    public async Task<GuideResponse> SelfUpdateAsync(GuideSelfUpdateRequest request)
    {
        var normalizedEmail = AuthService.NormalizeEmail(request.Email);
        var profile = await _dbContext.GuideProfiles
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.User.NormalizedEmail == normalizedEmail);

        if (profile is null)
        {
            throw new KeyNotFoundException("Guide profile was not found.");
        }

        profile.User.FullName = request.FullName.Trim();
        profile.User.PhoneNumber = request.PhoneNumber.Trim();
        profile.User.UpdatedAt = DateTime.UtcNow;
        ApplyProfileUpdate(profile, request.Languages, request.ExpertiseArea, request.Region, request.CompletedTours, request.Bio);

        await _dbContext.SaveChangesAsync();
        return ToResponse(profile);
    }

    private async Task<GuideProfile> FindProfileByIdAsync(Guid id)
    {
        var profile = await _dbContext.GuideProfiles
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (profile is null)
        {
            throw new KeyNotFoundException("Guide profile was not found.");
        }

        return profile;
    }

    private static void ApplyProfileUpdate(
        GuideProfile profile,
        string languages,
        string expertiseArea,
        string region,
        int completedTours,
        string bio)
    {
        profile.Languages = NormalizeCsv(languages);
        profile.ExpertiseArea = expertiseArea.Trim();
        profile.Region = region.Trim();
        profile.CompletedTours = completedTours;
        profile.Bio = bio.Trim();
        profile.UpdatedAt = DateTime.UtcNow;
    }

    private static GuideResponse ToResponse(GuideProfile profile)
    {
        return new GuideResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FullName = profile.User.FullName,
            Email = profile.User.Email,
            PhoneNumber = profile.User.PhoneNumber,
            Languages = SplitCsv(profile.Languages),
            LanguagesText = profile.Languages,
            ExpertiseArea = profile.ExpertiseArea,
            Region = profile.Region,
            CompletedTours = profile.CompletedTours,
            Status = profile.Status,
            Bio = profile.Bio,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt
        };
    }

    private static bool Contains(string value, string term)
    {
        return value.Contains(term, StringComparison.OrdinalIgnoreCase);
    }

    private Task SendGuideAccountEmailAsync(ApplicationUser user, string temporaryPassword)
    {
        var subject = "Your WanderX guide account has been created";
        var body = $"""
Hello {user.FullName},

Your WanderX guide account has been created.

Login email: {user.Email}
Temporary password: {temporaryPassword}

Please sign in and update your guide profile information.

WanderX Team
""";

        return _emailSender.SendAsync(user.Email, subject, body);
    }

    private static string NormalizeCsv(string value)
    {
        return string.Join(", ", SplitCsv(value));
    }

    private static IReadOnlyList<string> SplitCsv(string value)
    {
        return value
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string NormalizeStatus(string value)
    {
        var status = value.Trim();
        return string.IsNullOrWhiteSpace(status) ? "Active" : status;
    }
}
