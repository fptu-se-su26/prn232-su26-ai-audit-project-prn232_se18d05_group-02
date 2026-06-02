using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.BusinessObject.Enums;

namespace WanderXServer.DataAccessLayer;

public class WanderXDbContext : DbContext
{
    public WanderXDbContext(DbContextOptions<WanderXDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    public DbSet<AuthVerificationCode> VerificationCodes => Set<AuthVerificationCode>();

    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    public DbSet<GuideProfile> GuideProfiles => Set<GuideProfile>();

    public DbSet<GuideTourAssignment> GuideTourAssignments => Set<GuideTourAssignment>();

    public void SeedDevelopmentData()
    {
        Database.EnsureCreated();
        EnsureGuideTourAssignmentsTable();

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        if (!Users.Any())
        {
            var admin = new ApplicationUser
            {
                FullName = "WanderX Admin",
                Email = "ad@ad.123",
                NormalizedEmail = NormalizeEmail("ad@ad.123"),
                PhoneNumber = "+10000000000",
                Role = UserRole.Admin,
                IsEmailConfirmed = true,
                IsPhoneConfirmed = true
            };

            admin.PasswordHash = passwordHasher.HashPassword(admin, "123456");
            Users.Add(admin);
        }

        if (!GuideProfiles.Any())
        {
            AddGuide(
                passwordHasher,
                "Elena Rodriguez",
                "elena.guide@wanderx.com",
                "+34123456789",
                "Spanish, English, French",
                "High-Altitude Trekking",
                "Europe Alps",
                86,
                "Active",
                "Certified trekking leader for alpine and cultural routes.");
            AddGuide(
                passwordHasher,
                "Kenji Tanaka",
                "kenji.guide@wanderx.com",
                "+81312345678",
                "Japanese, English",
                "Culinary Arts",
                "Kyoto and Kansai",
                112,
                "On Tour",
                "Food historian specializing in private markets and seasonal dining.");
            AddGuide(
                passwordHasher,
                "Zara Nkosi",
                "zara.guide@wanderx.com",
                "+27111234567",
                "Zulu, English, French",
                "Safari Wildlife",
                "Southern Africa",
                47,
                "Active",
                "Wildlife interpreter for conservation-first safari experiences.");
            AddGuide(
                passwordHasher,
                "Marcus Vane",
                "marcus.guide@wanderx.com",
                "+442012345678",
                "English, German",
                "European History",
                "Western Europe",
                154,
                "Unavailable",
                "Museum-trained historian for heritage and architecture tours.");
        }

        SaveChanges();

        if (!GuideTourAssignments.Any())
        {
            SeedGuideTourAssignments();
        }
    }

    private void EnsureGuideTourAssignmentsTable()
    {
        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[GuideTourAssignments]', N'U') IS NULL
BEGIN
    CREATE TABLE [GuideTourAssignments] (
        [Id] uniqueidentifier NOT NULL,
        [GuideProfileId] uniqueidentifier NOT NULL,
        [TourCode] nvarchar(32) NOT NULL,
        [TourName] nvarchar(160) NOT NULL,
        [Destination] nvarchar(160) NOT NULL,
        [Region] nvarchar(160) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [TravelerCount] int NOT NULL,
        [MeetingPoint] nvarchar(240) NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        [ItinerarySummary] nvarchar(1000) NOT NULL,
        [DeclineReason] nvarchar(600) NULL,
        [DeclinedAt] datetime2 NULL,
        [FinishedAt] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_GuideTourAssignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuideTourAssignments_GuideProfiles_GuideProfileId] FOREIGN KEY ([GuideProfileId]) REFERENCES [GuideProfiles] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_GuideTourAssignments_GuideProfileId] ON [GuideTourAssignments] ([GuideProfileId]);
END
""");
    }

    private void SeedGuideTourAssignments()
    {
        var today = DateTime.Today;
        AddAssignment(
            "elena.guide@wanderx.com",
            "WX-ALP-102",
            "Swiss Alps Photography Trek",
            "Zermatt, Switzerland",
            "Europe Alps",
            today.AddDays(-1),
            today.AddDays(3),
            8,
            "Zermatt Station, north entrance",
            "Confirmed",
            "Lead a premium alpine photography route, coordinate sunrise viewpoints, and manage safety pacing.");
        AddAssignment(
            "elena.guide@wanderx.com",
            "WX-VEN-214",
            "Venice Hidden Canals",
            "Venice, Italy",
            "Northern Italy",
            today.AddDays(12),
            today.AddDays(15),
            6,
            "Hotel Danieli lobby",
            "Assigned",
            "Private cultural walking tour with artisan workshop and evening cicchetti tasting.");
        AddAssignment(
            "kenji.guide@wanderx.com",
            "WX-KYO-330",
            "Kyoto Culinary Immersion",
            "Kyoto, Japan",
            "Kyoto and Kansai",
            today.AddDays(5),
            today.AddDays(9),
            10,
            "Nishiki Market west gate",
            "Confirmed",
            "Market orientation, tea ceremony coordination, and private kaiseki dining support.");
        AddAssignment(
            "zara.guide@wanderx.com",
            "WX-SAF-718",
            "Serengeti Conservation Safari",
            "Serengeti, Tanzania",
            "Southern Africa",
            today.AddDays(20),
            today.AddDays(27),
            12,
            "Arusha Coffee Lodge reception",
            "Assigned",
            "Wildlife interpretation, conservation briefing, and daily field logistics for safari guests.");

        SaveChanges();
    }

    private void AddAssignment(
        string guideEmail,
        string tourCode,
        string tourName,
        string destination,
        string region,
        DateTime startDate,
        DateTime endDate,
        int travelerCount,
        string meetingPoint,
        string status,
        string itinerarySummary)
    {
        var normalizedEmail = NormalizeEmail(guideEmail);
        var guide = GuideProfiles.FirstOrDefault(item => item.User.NormalizedEmail == normalizedEmail);

        if (guide is null)
        {
            return;
        }

        GuideTourAssignments.Add(new GuideTourAssignment
        {
            GuideProfileId = guide.Id,
            TourCode = tourCode,
            TourName = tourName,
            Destination = destination,
            Region = region,
            StartDate = startDate,
            EndDate = endDate,
            TravelerCount = travelerCount,
            MeetingPoint = meetingPoint,
            Status = status,
            ItinerarySummary = itinerarySummary
        });
    }

    private void AddGuide(
        PasswordHasher<ApplicationUser> passwordHasher,
        string fullName,
        string email,
        string phoneNumber,
        string languages,
        string expertiseArea,
        string region,
        int completedTours,
        string status,
        string bio)
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            Email = email,
            NormalizedEmail = NormalizeEmail(email),
            PhoneNumber = phoneNumber,
            Role = UserRole.Guide,
            IsEmailConfirmed = true,
            IsPhoneConfirmed = true
        };
        user.PasswordHash = passwordHasher.HashPassword(user, "Guide@123");

        GuideProfiles.Add(new GuideProfile
        {
            User = user,
            Languages = languages,
            ExpertiseArea = expertiseArea,
            Region = region,
            CompletedTours = completedTours,
            Status = status,
            Bio = bio
        });
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }
}
