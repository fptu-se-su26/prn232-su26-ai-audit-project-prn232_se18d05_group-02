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
    public DbSet<AccountAuditLog> AccountAuditLogs => Set<AccountAuditLog>();
    public DbSet<PhoneVerification> PhoneVerifications => Set<PhoneVerification>();

    public DbSet<GuideProfile> GuideProfiles => Set<GuideProfile>();

    public DbSet<GuideTourAssignment> GuideTourAssignments => Set<GuideTourAssignment>();

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingPassenger> BookingPassengers => Set<BookingPassenger>();
    public DbSet<UserSpecialRequest> UserSpecialRequests => Set<UserSpecialRequest>();
    public DbSet<TourReview> TourReviews => Set<TourReview>();
    public DbSet<TravelStyleQuizResult> TravelStyleQuizResults => Set<TravelStyleQuizResult>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizOption> QuizOptions => Set<QuizOption>();
    public DbSet<Tour> Tours => Set<Tour>();

    public DbSet<TourSchedule> TourSchedules => Set<TourSchedule>();

    public DbSet<TourSeasonPrice> TourSeasonPrices => Set<TourSeasonPrice>();

    public DbSet<TourPromotion> TourPromotions => Set<TourPromotion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        // 1. Seed Admin
        var adminEmail = "ad@ad.123";
        var adminUser = new ApplicationUser
        {
            Id = CreateGuidFromEmail(adminEmail, "User"),
            FullName = "WanderX Admin",
            Email = adminEmail,
            NormalizedEmail = NormalizeEmail(adminEmail),
            PhoneNumber = "+10000000000",
            Role = UserRole.Admin,
            IsEmailConfirmed = true,
            IsPhoneConfirmed = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456");
        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        // 2. Seed Guides
        var usersToSeed = new List<ApplicationUser>();
        var profilesToSeed = new List<GuideProfile>();

        foreach (var guide in DevelopmentGuides)
        {
            var userId = CreateGuidFromEmail(guide.Email, "User");
            var profileId = CreateGuidFromEmail(guide.Email, "Profile");

            var user = new ApplicationUser
            {
                Id = userId,
                FullName = guide.FullName,
                Email = guide.Email,
                NormalizedEmail = NormalizeEmail(guide.Email),
                PhoneNumber = guide.PhoneNumber,
                Role = UserRole.Guide,
                IsEmailConfirmed = true,
                IsPhoneConfirmed = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Guide@123");
            usersToSeed.Add(user);

            var profile = new GuideProfile
            {
                Id = profileId,
                UserId = userId,
                Languages = guide.Languages,
                ExpertiseArea = guide.ExpertiseArea,
                Region = guide.Region,
                CompletedTours = guide.CompletedTours,
                Status = guide.Status,
                Bio = guide.Bio,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            profilesToSeed.Add(profile);
        }

        modelBuilder.Entity<ApplicationUser>().HasData(usersToSeed.ToArray());
        modelBuilder.Entity<GuideProfile>().HasData(profilesToSeed.ToArray());

        // 3. Seed Guide Tour Assignments
        var assignmentsToSeed = new List<GuideTourAssignment>();
        var today = new DateTime(2026, 6, 16, 0, 0, 0, DateTimeKind.Utc);

        foreach (var assignment in DevelopmentAssignments)
        {
            var profileId = CreateGuidFromEmail(assignment.GuideEmail, "Profile");
            var assignmentId = CreateGuidFromEmail(assignment.TourCode, "Assignment");

            var startDate = today.AddDays(assignment.StartOffsetDays);
            var endDate = today.AddDays(assignment.EndOffsetDays);
            var declinedAt = assignment.Status.Equals("Declined", StringComparison.OrdinalIgnoreCase)
                ? today.AddDays(assignment.EndOffsetDays).AddHours(10)
                : (DateTime?)null;
            var finishedAt = assignment.Status.Equals("Finished", StringComparison.OrdinalIgnoreCase)
                ? today.AddDays(assignment.EndOffsetDays).AddHours(18)
                : (DateTime?)null;

            var dbAssignment = new GuideTourAssignment
            {
                Id = assignmentId,
                GuideProfileId = profileId,
                TourCode = assignment.TourCode,
                TourName = assignment.TourName,
                Destination = assignment.Destination,
                Region = assignment.Region,
                StartDate = startDate,
                EndDate = endDate,
                TravelerCount = assignment.TravelerCount,
                MeetingPoint = assignment.MeetingPoint,
                Status = assignment.Status,
                ItinerarySummary = assignment.ItinerarySummary,
                DeclineReason = assignment.DeclineReason,
                DeclinedAt = declinedAt,
                FinishedAt = finishedAt,
                EvidenceImage = assignment.Status.Equals("Finished", StringComparison.OrdinalIgnoreCase)
                    ? SampleEvidenceImageBase64
                    : null,
                CreatedAt = today.AddDays(assignment.StartOffsetDays - 14),
                UpdatedAt = declinedAt ?? finishedAt
            };
            assignmentsToSeed.Add(dbAssignment);
        }

        modelBuilder.Entity<GuideTourAssignment>().HasData(assignmentsToSeed.ToArray());

        var toursToSeed = DevelopmentTours
            .Select(tour => new Tour
            {
                Id = CreateGuidFromEmail(tour.Code, "Tour"),
                Code = tour.Code,
                Name = tour.Name,
                Destination = tour.Destination,
                Region = tour.Region,
                ScheduleTourId = tour.ScheduleTourId,
                DurationDays = tour.DurationDays,
                Price = tour.Price,
                Capacity = tour.Capacity,
                Status = tour.Status,
                ImageUrl = tour.ImageUrl,
                Description = tour.Description,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            })
            .ToArray();

        modelBuilder.Entity<Tour>().HasData(toursToSeed);

        modelBuilder.Entity<Tour>()
            .HasAlternateKey(tour => tour.ScheduleTourId);

        modelBuilder.Entity<Tour>()
            .Property(tour => tour.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<TourSchedule>()
            .HasOne(schedule => schedule.Tour)
            .WithMany(tour => tour.Schedules)
            .HasForeignKey(schedule => schedule.TourId)
            .HasPrincipalKey(tour => tour.ScheduleTourId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TourSeasonPrice>()
            .Property(item => item.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<TourSeasonPrice>()
            .HasOne(item => item.Tour)
            .WithMany(tour => tour.SeasonPrices)
            .HasForeignKey(item => item.TourId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TourPromotion>()
            .Property(item => item.DiscountValue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<TourPromotion>()
            .HasOne(item => item.Tour)
            .WithMany(tour => tour.Promotions)
            .HasForeignKey(item => item.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static Guid CreateGuidFromEmail(string key, string type)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key + "_" + type));
        return new Guid(hash);
    }

    public void SeedDevelopmentData()
    {
        Database.EnsureCreated();
        EnsureUserAddressColumn();
        EnsureGuideTourAssignmentsTable();
        EnsureBookingsTable();
        EnsureUserSpecialRequestsTable();
        EnsureTourReviewsTable();
        EnsureTravelStyleQuizResultsTable();
        EnsureQuizQuestionsTable();
        EnsureQuizOptionsTable();
        EnsureApplicationSchema();
        EnsureTourPricingStorage();
        EnsureAccountSecuritySchema();

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        SeedAdmin(passwordHasher);
        SeedGuides(passwordHasher);

        SaveChanges();
        SeedGuideTourAssignments();
        SeedBookings();
        SeedQuizData();
        SeedTours();
        SaveChanges();
        SeedSchedulesForPaidAssignedBookings();
        SaveChanges();

        // Update existing Finished assignments that don't have an evidence image
        var finishedToursWithNoEvidence = GuideTourAssignments
            .Where(item => item.Status == "Finished" && string.IsNullOrEmpty(item.EvidenceImage))
            .ToList();

        if (finishedToursWithNoEvidence.Count > 0)
        {
            foreach (var tour in finishedToursWithNoEvidence)
            {
                tour.EvidenceImage = SampleEvidenceImageBase64;
            }
            SaveChanges();
        }
    }

    private void EnsureUserAddressColumn()
    {
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Users', 'Address') IS NULL
BEGIN
    ALTER TABLE [Users] ADD [Address] nvarchar(240) NULL;
END
""");
    }

    private void EnsureAccountSecuritySchema()
    {
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Users', 'AccountStatus') IS NULL ALTER TABLE [Users] ADD [AccountStatus] nvarchar(32) NOT NULL CONSTRAINT [DF_Users_AccountStatus] DEFAULT 'Active';
IF COL_LENGTH('Users', 'LockoutEnd') IS NULL ALTER TABLE [Users] ADD [LockoutEnd] datetime2 NULL;
IF COL_LENGTH('Users', 'LockReason') IS NULL ALTER TABLE [Users] ADD [LockReason] nvarchar(500) NULL;
IF COL_LENGTH('Users', 'TokenVersion') IS NULL ALTER TABLE [Users] ADD [TokenVersion] int NOT NULL CONSTRAINT [DF_Users_TokenVersion] DEFAULT 0;
IF OBJECT_ID(N'[AccountAuditLogs]', N'U') IS NULL
BEGIN
 CREATE TABLE [AccountAuditLogs]([Id] uniqueidentifier NOT NULL PRIMARY KEY,[ActorUserId] uniqueidentifier NOT NULL,[TargetUserId] uniqueidentifier NOT NULL,[Action] nvarchar(40) NOT NULL,[OldValue] nvarchar(500) NULL,[NewValue] nvarchar(500) NULL,[Reason] nvarchar(500) NOT NULL,[CreatedAt] datetime2 NOT NULL);
 CREATE INDEX [IX_AccountAuditLogs_TargetUserId_CreatedAt] ON [AccountAuditLogs]([TargetUserId],[CreatedAt]);
END;
IF OBJECT_ID(N'[PhoneVerifications]', N'U') IS NULL
BEGIN
 CREATE TABLE [PhoneVerifications]([Id] uniqueidentifier NOT NULL PRIMARY KEY,[UserId] uniqueidentifier NOT NULL,[PhoneNumber] nvarchar(20) NOT NULL,[OtpHash] nvarchar(128) NOT NULL,[ExpiresAt] datetime2 NOT NULL,[AttemptCount] int NOT NULL,[MaxAttempts] int NOT NULL,[SentCount] int NOT NULL,[LastSentAt] datetime2 NOT NULL,[Status] nvarchar(20) NOT NULL,[ProviderMessageId] nvarchar(120) NULL,[CreatedAt] datetime2 NOT NULL,[VerifiedAt] datetime2 NULL);
 CREATE INDEX [IX_PhoneVerifications_UserId_PhoneNumber_CreatedAt] ON [PhoneVerifications]([UserId],[PhoneNumber],[CreatedAt]);
END;
""");
    }
    public void EnsureApplicationSchema()
    {
        EnsureGuideTourAssignmentsTable();
        EnsureTourStorage();
    }

    public void EnsureTourStorage()
    {
        EnsureToursTable();
        EnsureTourSchedulesTable();
    }

    public void EnsureTourPricingStorage()
    {
        EnsureTourStorage();
        EnsureTourSeasonPricesTable();
        EnsureTourPromotionsTable();
    }

    public void EnsureBookingStorage()
    {
        EnsureBookingsTable();
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
        [EvidenceImage] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_GuideTourAssignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuideTourAssignments_GuideProfiles_GuideProfileId] FOREIGN KEY ([GuideProfileId]) REFERENCES [GuideProfiles] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_GuideTourAssignments_GuideProfileId] ON [GuideTourAssignments] ([GuideProfileId]);
END
""");

        Database.ExecuteSqlRaw("""
IF COL_LENGTH('GuideTourAssignments', 'EvidenceImage') IS NULL
BEGIN
    ALTER TABLE [GuideTourAssignments] ADD [EvidenceImage] nvarchar(max) NULL;
END
""");
    }
    // TV3
    private void EnsureBookingsTable()
    {
        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[Bookings]', N'U') IS NULL
BEGIN
    CREATE TABLE [Bookings] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [BookingCode] nvarchar(32) NOT NULL,
        [TourCode] nvarchar(32) NULL,
        [DepartureScheduleId] uniqueidentifier NULL,
        [TourName] nvarchar(200) NOT NULL,
        [Destination] nvarchar(200) NOT NULL,
        [ThumbnailUrl] nvarchar(500) NULL,
        [DepartureDate] datetime2 NOT NULL,
        [GuestCount] int NOT NULL,
        [TotalAmount] decimal(18, 2) NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [PaidAt] datetime2 NULL,
        [ConfirmedAt] datetime2 NULL,
        [CompletedAt] datetime2 NULL,
        CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Bookings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_Bookings_UserId] ON [Bookings] ([UserId]);
END
""");

        // Add columns if the table already existed without them
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'TourCode') IS NULL
    ALTER TABLE [Bookings] ADD [TourCode] nvarchar(32) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'DepartureScheduleId') IS NULL
    ALTER TABLE [Bookings] ADD [DepartureScheduleId] uniqueidentifier NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'StatusUpdatedAt') IS NULL
    ALTER TABLE [Bookings] ADD [StatusUpdatedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'StatusUpdatedBy') IS NULL
    ALTER TABLE [Bookings] ADD [StatusUpdatedBy] nvarchar(120) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CancellationStatus') IS NULL
    ALTER TABLE [Bookings] ADD [CancellationStatus] nvarchar(32) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CancellationReason') IS NULL
    ALTER TABLE [Bookings] ADD [CancellationReason] nvarchar(1000) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CancellationRequestedAt') IS NULL
    ALTER TABLE [Bookings] ADD [CancellationRequestedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CancellationReviewedAt') IS NULL
    ALTER TABLE [Bookings] ADD [CancellationReviewedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CancellationReviewedBy') IS NULL
    ALTER TABLE [Bookings] ADD [CancellationReviewedBy] nvarchar(120) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CancellationReviewNote') IS NULL
    ALTER TABLE [Bookings] ADD [CancellationReviewNote] nvarchar(1000) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentOption') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentOption] nvarchar(32) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentMethod') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentMethod] nvarchar(64) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentReference') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentReference] nvarchar(120) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaidAmount') IS NULL
    ALTER TABLE [Bookings] ADD [PaidAmount] decimal(18, 2) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'RemainingAmount') IS NULL
    ALTER TABLE [Bookings] ADD [RemainingAmount] decimal(18, 2) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentUpdatedAt') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentUpdatedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentUpdatedBy') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentUpdatedBy] nvarchar(120) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentNote') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentNote] nvarchar(1000) NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaidAt') IS NULL
    ALTER TABLE [Bookings] ADD [PaidAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'ConfirmedAt') IS NULL
    ALTER TABLE [Bookings] ADD [ConfirmedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CompletedAt') IS NULL
    ALTER TABLE [Bookings] ADD [CompletedAt] datetime2 NULL;
""");

        // Add new approval and failure columns
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'ApprovedAt') IS NULL
    ALTER TABLE [Bookings] ADD [ApprovedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'RequestFailedAt') IS NULL
    ALTER TABLE [Bookings] ADD [RequestFailedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'PaymentFailedAt') IS NULL
    ALTER TABLE [Bookings] ADD [PaymentFailedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'ConfirmationFailedAt') IS NULL
    ALTER TABLE [Bookings] ADD [ConfirmationFailedAt] datetime2 NULL;
""");
        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Bookings', 'CompletionFailedAt') IS NULL
    ALTER TABLE [Bookings] ADD [CompletionFailedAt] datetime2 NULL;
""");

        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[BookingPassengers]', N'U') IS NULL
BEGIN
    CREATE TABLE [BookingPassengers] (
        [Id] uniqueidentifier NOT NULL,
        [BookingId] uniqueidentifier NOT NULL,
        [FullName] nvarchar(100) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [TicketType] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_BookingPassengers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BookingPassengers_Bookings_BookingId] FOREIGN KEY ([BookingId]) REFERENCES [Bookings] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_BookingPassengers_BookingId] ON [BookingPassengers] ([BookingId]);
END
""");
    }

    private void EnsureToursTable()
    {
        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[Tours]', N'U') IS NULL
BEGIN
    CREATE TABLE [Tours] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(32) NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        [Destination] nvarchar(160) NOT NULL,
        [Region] nvarchar(160) NOT NULL,
        [ScheduleTourId] int NOT NULL,
        [DurationDays] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Capacity] int NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        [LockReason] nvarchar(32) NULL,
        [ImageUrl] nvarchar(500) NOT NULL,
        [Description] nvarchar(1200) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Tours] PRIMARY KEY ([Id])
    );

    CREATE UNIQUE INDEX [IX_Tours_Code] ON [Tours] ([Code]);
END
""");

        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Tours', 'LockReason') IS NULL
BEGIN
    ALTER TABLE [Tours] ADD [LockReason] nvarchar(32) NULL;
END
""");

        Database.ExecuteSqlRaw("""
IF COL_LENGTH('Tours', 'ScheduleTourId') IS NULL
BEGIN
    ALTER TABLE [Tours] ADD [ScheduleTourId] int NULL;
END
""");

        Database.ExecuteSqlRaw("""
;WITH NumberedTours AS (
    SELECT [Id], ROW_NUMBER() OVER (ORDER BY [CreatedAt], [Code]) AS RowNumber
    FROM [Tours]
    WHERE [ScheduleTourId] IS NULL
)
UPDATE Tours
SET [ScheduleTourId] = NumberedTours.RowNumber
FROM [Tours]
INNER JOIN NumberedTours ON Tours.[Id] = NumberedTours.[Id]
WHERE Tours.[ScheduleTourId] IS NULL;
""");

        Database.ExecuteSqlRaw("""
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID('Tours')
        AND name = 'ScheduleTourId'
        AND is_nullable = 1
)
BEGIN
    ALTER TABLE [Tours] ALTER COLUMN [ScheduleTourId] int NOT NULL;
END
""");

        Database.ExecuteSqlRaw("""
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'AK_Tours_ScheduleTourId' AND object_id = OBJECT_ID('Tours'))
BEGIN
    CREATE UNIQUE INDEX [AK_Tours_ScheduleTourId] ON [Tours] ([ScheduleTourId]);
END
""");
    }

    private void EnsureTourSchedulesTable()
    {
        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[TourSchedules]', N'U') IS NULL
BEGIN
    CREATE TABLE [TourSchedules] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [TourId] int NOT NULL,
        [DayNumber] int NOT NULL,
        [ScheduleDate] datetime2 NOT NULL,
        [Title] nvarchar(160) NOT NULL,
        [Description] nvarchar(1200) NOT NULL,
        [Location] nvarchar(160) NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [SortOrder] int NOT NULL,
        CONSTRAINT [PK_TourSchedules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TourSchedules_Tours_TourId] FOREIGN KEY ([TourId]) REFERENCES [Tours] ([ScheduleTourId]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_TourSchedules_TourId_ScheduleDate_DayNumber_SortOrder] ON [TourSchedules] ([TourId], [ScheduleDate], [DayNumber], [SortOrder]);
END
""");
    }

    private void EnsureTourSeasonPricesTable()
    {
        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[TourSeasonPrices]', N'U') IS NULL
BEGIN
    CREATE TABLE [TourSeasonPrices] (
        [Id] uniqueidentifier NOT NULL,
        [TourId] uniqueidentifier NOT NULL,
        [SeasonName] nvarchar(120) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_TourSeasonPrices] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TourSeasonPrices_Tours_TourId] FOREIGN KEY ([TourId]) REFERENCES [Tours] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_TourSeasonPrices_TourId_StartDate_EndDate] ON [TourSeasonPrices] ([TourId], [StartDate], [EndDate]);
END
""");
    }

    private void EnsureTourPromotionsTable()
    {
        Database.ExecuteSqlRaw("""
IF OBJECT_ID(N'[TourPromotions]', N'U') IS NULL
BEGIN
    CREATE TABLE [TourPromotions] (
        [Id] uniqueidentifier NOT NULL,
        [TourId] uniqueidentifier NOT NULL,
        [Name] nvarchar(120) NOT NULL,
        [DiscountType] nvarchar(16) NOT NULL,
        [DiscountValue] decimal(18,2) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_TourPromotions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TourPromotions_Tours_TourId] FOREIGN KEY ([TourId]) REFERENCES [Tours] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_TourPromotions_TourId_StartDate_EndDate] ON [TourPromotions] ([TourId], [StartDate], [EndDate]);
END
""");
    }

    private void EnsureUserSpecialRequestsTable()
    {
        Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[UserSpecialRequests]', N'U') IS NULL
BEGIN
    CREATE TABLE [UserSpecialRequests] (
        [Id] uniqueidentifier NOT NULL,
        [BookingId] uniqueidentifier NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [ReviewedAt] datetime2 NULL,
        [AdminResponse] nvarchar(1000) NULL,
        CONSTRAINT [PK_UserSpecialRequests] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserSpecialRequests_Bookings_BookingId] FOREIGN KEY ([BookingId]) REFERENCES [Bookings] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_UserSpecialRequests_BookingId] ON [UserSpecialRequests] ([BookingId]);
END
");
    }

    private void EnsureTourReviewsTable()
    {
        Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[TourReviews]', N'U') IS NULL
BEGIN
    CREATE TABLE [TourReviews] (
        [Id] uniqueidentifier NOT NULL,
        [BookingId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Rating] int NOT NULL,
        [Comment] nvarchar(2000) NULL,
        [TravelPhotos] nvarchar(max) NULL,
        [Status] nvarchar(32) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [ModeratedAt] datetime2 NULL,
        [ModerationReason] nvarchar(1000) NULL,
        [ModeratedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_TourReviews] PRIMARY KEY ([Id])
    );

    CREATE INDEX [IX_TourReviews_BookingId] ON [TourReviews] ([BookingId]);
    CREATE INDEX [IX_TourReviews_UserId] ON [TourReviews] ([UserId]);
END
");
    }

    private void EnsureTravelStyleQuizResultsTable()
    {
        Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[TravelStyleQuizResults]', N'U') IS NULL
BEGIN
    CREATE TABLE [TravelStyleQuizResults] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [AnswersJson] nvarchar(max) NOT NULL,
        [AdventureScore] int NOT NULL,
        [CulturalScore] int NOT NULL,
        [RelaxationScore] int NOT NULL,
        [LuxuryScore] int NOT NULL,
        [DominantStyle] nvarchar(50) NOT NULL,
        [CompletedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_TravelStyleQuizResults] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TravelStyleQuizResults_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_TravelStyleQuizResults_UserId] ON [TravelStyleQuizResults] ([UserId]);
END
");
    }

    private void EnsureQuizQuestionsTable()
    {
        Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[QuizQuestions]', N'U') IS NULL
BEGIN
    CREATE TABLE [QuizQuestions] (
        [Id] uniqueidentifier NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_QuizQuestions] PRIMARY KEY ([Id])
    );
END
");
    }

    private void EnsureQuizOptionsTable()
    {
        Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[QuizOptions]', N'U') IS NULL
BEGIN
    CREATE TABLE [QuizOptions] (
        [Id] uniqueidentifier NOT NULL,
        [QuestionId] uniqueidentifier NOT NULL,
        [OptionKey] nvarchar(10) NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [Category] nvarchar(50) NOT NULL,
        [Score] int NOT NULL,
        CONSTRAINT [PK_QuizOptions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizOptions_QuizQuestions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [QuizQuestions] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_QuizOptions_QuestionId] ON [QuizOptions] ([QuestionId]);
END
");
    }

    private void SeedQuizData()
    {
        if (QuizQuestions.Any()) return;

        var questions = new List<QuizQuestion>
        {
            new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Text = "Bạn thích dành kỳ nghỉ của mình ở đâu nhất?",
                DisplayOrder = 1,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionKey = "A", Text = "Những ngọn núi hiểm trở hoặc rừng rậm hoang dã để hòa mình vào thiên nhiên.", Category = "Adventure", Score = 3 },
                    new QuizOption { OptionKey = "B", Text = "Các thành phố cổ kính, viện bảo tàng và di tích lịch sử văn hóa.", Category = "Cultural", Score = 3 },
                    new QuizOption { OptionKey = "C", Text = "Một bãi biển yên bình, tĩnh lặng hoặc khu nghỉ dưỡng spa biệt lập.", Category = "Relaxation", Score = 3 },
                    new QuizOption { OptionKey = "D", Text = "Khách sạn 5 sao cao cấp, trung tâm mua sắm sầm uất và khu vui chơi hiện đại.", Category = "Luxury", Score = 3 }
                }
            },
            new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Text = "Hoạt động yêu thích của bạn trong suốt chuyến đi là gì?",
                DisplayOrder = 2,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionKey = "A", Text = "Leo núi trekking, chèo thuyền kayak vượt thác hoặc các trò chơi mạo hiểm.", Category = "Adventure", Score = 3 },
                    new QuizOption { OptionKey = "B", Text = "Tham gia lễ hội truyền thống bản địa, học làm đồ thủ công, hoặc tham quan di sản.", Category = "Cultural", Score = 3 },
                    new QuizOption { OptionKey = "C", Text = "Nằm đọc sách bên hồ bơi, tắm nắng trên bãi cát, hoặc trị liệu spa thư giãn.", Category = "Relaxation", Score = 3 },
                    new QuizOption { OptionKey = "D", Text = "Thưởng thức bữa tối fine-dining tại nhà hàng Michelin hoặc tận hưởng du thuyền sang trọng.", Category = "Luxury", Score = 3 }
                }
            },
            new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Text = "Bạn thường chuẩn bị hành lý của mình như thế nào?",
                DisplayOrder = 3,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionKey = "A", Text = "Một chiếc balo gọn nhẹ với các vật dụng sinh tồn và đồ dã ngoại tiện lợi.", Category = "Adventure", Score = 3 },
                    new QuizOption { OptionKey = "B", Text = "Mang theo máy ảnh chuyên nghiệp, sách hướng dẫn du lịch và sổ tay ghi chép.", Category = "Cultural", Score = 3 },
                    new QuizOption { OptionKey = "C", Text = "Trang phục thoải mái, kem chống nắng, đồ bơi và vài cuốn tiểu thuyết.", Category = "Relaxation", Score = 3 },
                    new QuizOption { OptionKey = "D", Text = "Vali kéo sang trọng với trang phục thời thượng thiết kế riêng và nhiều phụ kiện.", Category = "Luxury", Score = 3 }
                }
            },
            new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Text = "Bạn thích đi du lịch cùng ai nhất để tận hưởng trọn vẹn chuyến đi?",
                DisplayOrder = 4,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionKey = "A", Text = "Đi du lịch một mình (solo) tự do hoặc nhóm bạn thân đam mê thử thách.", Category = "Adventure", Score = 3 },
                    new QuizOption { OptionKey = "B", Text = "Người có cùng niềm đam mê tìm hiểu lịch sử, văn hóa bản xứ sâu sắc.", Category = "Cultural", Score = 3 },
                    new QuizOption { OptionKey = "C", Text = "Gia đình thân yêu hoặc người đời để cùng nhau nghỉ ngơi hoàn toàn.", Category = "Relaxation", Score = 3 },
                    new QuizOption { OptionKey = "D", Text = "Một nhóm nhỏ cao cấp hoặc đối tác để cùng tận hưởng các dịch vụ VIP đẳng cấp.", Category = "Luxury", Score = 3 }
                }
            },
            new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Text = "Cách bạn lựa chọn ẩm thực và ăn uống khi đặt chân tới vùng đất mới?",
                DisplayOrder = 5,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionKey = "A", Text = "Ăn đồ đóng hộp mang theo tiện lợi hoặc thưởng thức bất cứ quán ăn ven đường nào.", Category = "Adventure", Score = 3 },
                    new QuizOption { OptionKey = "B", Text = "Thử các món ăn ẩm thực đường phố truyền thống và đặc sản độc lạ của người bản xứ.", Category = "Cultural", Score = 3 },
                    new QuizOption { OptionKey = "C", Text = "Gọi đồ ăn phục vụ tận phòng (room service) hoặc ăn buffet thoải mái tại resort.", Category = "Relaxation", Score = 3 },
                    new QuizOption { OptionKey = "D", Text = "Đặt bàn trước tại các nhà hàng nổi tiếng, sang trọng nhất và có tầm nhìn đẹp nhất vùng.", Category = "Luxury", Score = 3 }
                }
            }
        };

        QuizQuestions.AddRange(questions);
        SaveChanges();
    }
    // end TV3
    private void SeedGuideTourAssignments()
    {
        var today = DateTime.Today;

        foreach (var assignment in DevelopmentAssignments)
        {
            AddAssignment(today, assignment);
        }
    }
    // TV3
    private void SeedBookings()
    {
        var adminEmail = NormalizeEmail("ad@ad.123");
        var admin = Users.FirstOrDefault(u => u.NormalizedEmail == adminEmail);

        if (admin == null) return;

        var today = DateTime.UtcNow.Date;

        // Tour 1: Paid and confirmed, not completed yet.
        var tour1 = Bookings.FirstOrDefault(b => b.BookingCode == "WX987346");
        if (tour1 == null)
        {
            tour1 = new Booking
            {
                UserId = admin.Id,
                BookingCode = "WX987346",
                TourCode = "WX-HUE-226",
                TourName = "Hue Imperial Heritage",
                Destination = "Hue, Vietnam",
                ThumbnailUrl = "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&q=80&w=300",
                DepartureDate = today.AddDays(15),
                GuestCount = 2,
                TotalAmount = 99000000m,
                Status = "Paid",
                CreatedAt = today.AddDays(-5),
                PaidAt = today.AddDays(-5).AddHours(2),
                ConfirmedAt = today.AddDays(-4),
                CompletedAt = null
            };
            Bookings.Add(tour1);
        }
        else
        {
            tour1.TourCode = "WX-HUE-226";
            tour1.TourName = "Hue Imperial Heritage";
            tour1.Destination = "Hue, Vietnam";
            tour1.ThumbnailUrl = "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&q=80&w=300";
            tour1.DepartureDate = today.AddDays(15);
            tour1.GuestCount = 2;
            tour1.TotalAmount = 99000000m;
            tour1.Status = "Paid";
            tour1.PaidAt = today.AddDays(-5).AddHours(2);
            tour1.ConfirmedAt = today.AddDays(-4);
            tour1.CompletedAt = null;
        }

        if (!BookingPassengers.Any(passenger => passenger.BookingId == tour1.Id))
        {
            BookingPassengers.Add(new BookingPassenger { BookingId = tour1.Id, FullName = "WanderX Admin", PhoneNumber = "0901234567", TicketType = "Adult" });
            BookingPassengers.Add(new BookingPassenger { BookingId = tour1.Id, FullName = "Nguyễn Văn B", PhoneNumber = "0901234568", TicketType = "Adult" });
        }
        else
        {
            var passenger = BookingPassengers.FirstOrDefault(item => item.BookingId == tour1.Id && item.PhoneNumber == "0901234568");
            if (passenger is not null)
            {
                passenger.FullName = "Nguyễn Văn B";
            }
        }

        // Tour 2: Pending payment.
        var tour2 = Bookings.FirstOrDefault(b => b.BookingCode == "WX348612");
        if (tour2 == null)
        {
            tour2 = new Booking
            {
                UserId = admin.Id,
                BookingCode = "WX348612",
                TourCode = "WX-KYO-330",
                TourName = "Kyoto Culinary Immersion",
                Destination = "Kyoto, Japan",
                ThumbnailUrl = "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e?auto=format&fit=crop&q=80&w=300",
                DepartureDate = today.AddDays(60),
                GuestCount = 1,
                TotalAmount = 38900000m,
                Status = "Pending",
                CreatedAt = today.AddDays(-2),
                PaidAt = null,
                ConfirmedAt = null,
                CompletedAt = null
            };
            Bookings.Add(tour2);
        }
        else
        {
            tour2.TourCode = "WX-KYO-330";
            tour2.TourName = "Kyoto Culinary Immersion";
            tour2.Destination = "Kyoto, Japan";
            tour2.ThumbnailUrl = "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e?auto=format&fit=crop&q=80&w=300";
            tour2.DepartureDate = today.AddDays(60);
            tour2.GuestCount = 1;
            tour2.TotalAmount = 38900000m;
            tour2.Status = "Pending";
            tour2.PaidAt = null;
            tour2.ConfirmedAt = null;
            tour2.CompletedAt = null;
        }

        if (!BookingPassengers.Any(passenger => passenger.BookingId == tour2.Id))
        {
            BookingPassengers.Add(new BookingPassenger { BookingId = tour2.Id, FullName = "WanderX Admin", PhoneNumber = "0901234567", TicketType = "Adult" });
        }

        // Tour 3: Cancelled.
        var tour3 = Bookings.FirstOrDefault(b => b.BookingCode == "WX102874");
        if (tour3 == null)
        {
            tour3 = new Booking
            {
                UserId = admin.Id,
                BookingCode = "WX102874",
                TourCode = "WX-SAF-718",
                TourName = "Serengeti Conservation Safari",
                Destination = "Serengeti, Tanzania",
                ThumbnailUrl = "https://images.unsplash.com/photo-1516426122078-c23e76319801?auto=format&fit=crop&q=80&w=300",
                DepartureDate = today.AddDays(-30),
                GuestCount = 2,
                TotalAmount = 124000000m,
                Status = "Cancelled",
                CreatedAt = today.AddDays(-40),
                PaidAt = null,
                ConfirmedAt = null,
                CompletedAt = null
            };
            Bookings.Add(tour3);
        }
        else
        {
            tour3.TourCode = "WX-SAF-718";
            tour3.TourName = "Serengeti Conservation Safari";
            tour3.Destination = "Serengeti, Tanzania";
            tour3.ThumbnailUrl = "https://images.unsplash.com/photo-1516426122078-c23e76319801?auto=format&fit=crop&q=80&w=300";
            tour3.DepartureDate = today.AddDays(-30);
            tour3.GuestCount = 2;
            tour3.TotalAmount = 124000000m;
            tour3.Status = "Cancelled";
            tour3.PaidAt = null;
            tour3.ConfirmedAt = null;
            tour3.CompletedAt = null;
        }

        if (!BookingPassengers.Any(passenger => passenger.BookingId == tour3.Id))
        {
            BookingPassengers.Add(new BookingPassenger { BookingId = tour3.Id, FullName = "WanderX Admin", PhoneNumber = "0901234567", TicketType = "Adult" });
            BookingPassengers.Add(new BookingPassenger { BookingId = tour3.Id, FullName = "Trần Thị C", PhoneNumber = "0901234569", TicketType = "Adult" });
        }
        else
        {
            var passenger = BookingPassengers.FirstOrDefault(item => item.BookingId == tour3.Id && item.PhoneNumber == "0901234569");
            if (passenger is not null)
            {
                passenger.FullName = "Trần Thị C";
            }
        }
    }
    // end TV3

    private void SeedSchedulesForPaidAssignedBookings()
    {
        var qualifiedBookings = Bookings
            .Where(booking =>
                !string.IsNullOrWhiteSpace(booking.TourCode) &&
                (booking.Status == "Paid" || booking.PaidAt != null))
            .ToList();

        foreach (var booking in qualifiedBookings)
        {
            var tour = Tours.FirstOrDefault(item => item.Code == booking.TourCode);
            if (tour is null)
            {
                continue;
            }

            var assignment = GuideTourAssignments
                .Include(item => item.GuideProfile)
                .ThenInclude(item => item.User)
                .FirstOrDefault(item =>
                    item.TourCode == booking.TourCode &&
                    (item.Status == "Assigned" || item.Status == "Confirmed"));

            if (assignment is null)
            {
                continue;
            }

            var alreadySeeded = TourSchedules.Any(schedule =>
                schedule.TourId == tour.ScheduleTourId &&
                schedule.Description.Contains(booking.BookingCode));

            if (alreadySeeded)
            {
                continue;
            }

            for (var day = 1; day <= tour.DurationDays; day++)
            {
                var scheduleDate = booking.DepartureDate.Date.AddDays(day - 1);
                var dayPlan = GetDefaultScheduleDay(day, tour.Destination);

                TourSchedules.Add(new TourSchedule
                {
                    TourId = tour.ScheduleTourId,
                    DayNumber = day,
                    ScheduleDate = scheduleDate,
                    Title = $"Day {day}: {dayPlan.Title}",
                    Description = $"{dayPlan.Description} Booking {booking.BookingCode} is paid and assigned to guide {assignment.GuideProfile.User.FullName}.",
                    Location = day == 1 ? assignment.MeetingPoint : tour.Destination,
                    StartTime = dayPlan.StartTime,
                    EndTime = dayPlan.EndTime,
                    SortOrder = day
                });
            }
        }
    }

    private static ScheduleDayTemplate GetDefaultScheduleDay(int day, string destination)
    {
        return day switch
        {
            1 => new ScheduleDayTemplate(
                "Arrival briefing and local orientation",
                $"Meet guests, confirm travel documents, and introduce the route around {destination}.",
                new TimeSpan(9, 0, 0),
                new TimeSpan(11, 30, 0)),
            2 => new ScheduleDayTemplate(
                "Main experience route",
                $"Guide the core sightseeing and cultural experience planned for {destination}.",
                new TimeSpan(8, 30, 0),
                new TimeSpan(16, 30, 0)),
            3 => new ScheduleDayTemplate(
                "Immersive activity and guest support",
                "Coordinate the booked activity, meal timing, transfer support, and guest requests.",
                new TimeSpan(9, 0, 0),
                new TimeSpan(17, 0, 0)),
            _ => new ScheduleDayTemplate(
                "Wrap-up and departure support",
                "Review the itinerary, support checkout or transfer, and close the guided tour.",
                new TimeSpan(8, 30, 0),
                new TimeSpan(12, 0, 0))
        };
    }

    private void SeedTours()
    {
        foreach (var tour in DevelopmentTours)
        {
            if (Tours.Any(item => item.Code == tour.Code))
            {
                continue;
            }

            Tours.Add(new Tour
            {
                Code = tour.Code,
                Name = tour.Name,
                Destination = tour.Destination,
                Region = tour.Region,
                ScheduleTourId = tour.ScheduleTourId,
                DurationDays = tour.DurationDays,
                Price = tour.Price,
                Capacity = tour.Capacity,
                Status = tour.Status,
                ImageUrl = tour.ImageUrl,
                Description = tour.Description
            });
        }
    }

    private void AddAssignment(DateTime today, DevelopmentAssignment assignment)
    {
        if (GuideTourAssignments.Any(item => item.TourCode == assignment.TourCode))
        {
            return;
        }

        var normalizedEmail = NormalizeEmail(assignment.GuideEmail);
        var guide = GuideProfiles.FirstOrDefault(item => item.User.NormalizedEmail == normalizedEmail);

        if (guide is null)
        {
            return;
        }

        var startDate = today.AddDays(assignment.StartOffsetDays);
        var endDate = today.AddDays(assignment.EndOffsetDays);
        var declinedAt = assignment.Status.Equals("Declined", StringComparison.OrdinalIgnoreCase)
            ? today.AddDays(assignment.EndOffsetDays).AddHours(10)
            : (DateTime?)null;
        var finishedAt = assignment.Status.Equals("Finished", StringComparison.OrdinalIgnoreCase)
            ? today.AddDays(assignment.EndOffsetDays).AddHours(18)
            : (DateTime?)null;

        GuideTourAssignments.Add(new GuideTourAssignment
        {
            GuideProfileId = guide.Id,
            TourCode = assignment.TourCode,
            TourName = assignment.TourName,
            Destination = assignment.Destination,
            Region = assignment.Region,
            StartDate = startDate,
            EndDate = endDate,
            TravelerCount = assignment.TravelerCount,
            MeetingPoint = assignment.MeetingPoint,
            Status = assignment.Status,
            ItinerarySummary = assignment.ItinerarySummary,
            DeclineReason = assignment.DeclineReason,
            DeclinedAt = declinedAt,
            FinishedAt = finishedAt,
            EvidenceImage = assignment.Status.Equals("Finished", StringComparison.OrdinalIgnoreCase)
                ? SampleEvidenceImageBase64
                : null,
            CreatedAt = today.AddDays(assignment.StartOffsetDays - 14),
            UpdatedAt = declinedAt ?? finishedAt
        });
    }

    private void SeedAdmin(PasswordHasher<ApplicationUser> passwordHasher)
    {
        const string adminEmail = "ad@ad.123";
        var normalizedEmail = NormalizeEmail(adminEmail);

        if (Users.Any(user => user.NormalizedEmail == normalizedEmail))
        {
            return;
        }

        var admin = new ApplicationUser
        {
            FullName = "WanderX Admin",
            Email = adminEmail,
            NormalizedEmail = normalizedEmail,
            PhoneNumber = "+10000000000",
            Role = UserRole.Admin,
            IsEmailConfirmed = true,
            IsPhoneConfirmed = true
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "123456");
        Users.Add(admin);
    }

    private void SeedGuides(PasswordHasher<ApplicationUser> passwordHasher)
    {
        foreach (var guide in DevelopmentGuides)
        {
            AddGuide(passwordHasher, guide);
        }
    }

    private void AddGuide(PasswordHasher<ApplicationUser> passwordHasher, DevelopmentGuide guide)
    {
        var normalizedEmail = NormalizeEmail(guide.Email);

        if (GuideProfiles.Any(profile => profile.User.NormalizedEmail == normalizedEmail))
        {
            return;
        }

        var user = Users.FirstOrDefault(item => item.NormalizedEmail == normalizedEmail);

        if (user is null)
        {
            user = new ApplicationUser
            {
                FullName = guide.FullName,
                Email = guide.Email,
                NormalizedEmail = normalizedEmail,
                PhoneNumber = guide.PhoneNumber,
                Role = UserRole.Guide,
                IsEmailConfirmed = true,
                IsPhoneConfirmed = true
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Guide@123");
        }
        else
        {
            user.FullName = guide.FullName;
            user.Email = guide.Email;
            user.NormalizedEmail = normalizedEmail;
            user.PhoneNumber = guide.PhoneNumber;
            user.Role = UserRole.Guide;
            user.IsEmailConfirmed = true;
            user.IsPhoneConfirmed = true;
            user.UpdatedAt = DateTime.UtcNow;
        }

        GuideProfiles.Add(new GuideProfile
        {
            User = user,
            Languages = guide.Languages,
            ExpertiseArea = guide.ExpertiseArea,
            Region = guide.Region,
            CompletedTours = guide.CompletedTours,
            Status = guide.Status,
            Bio = guide.Bio
        });
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }

    private static readonly DevelopmentGuide[] DevelopmentGuides =
    {
        new(
            "Elena Rodriguez",
            "elena.guide@wanderx.com",
            "+34123456789",
            "Spanish, English, French",
            "High-Altitude Trekking",
            "Europe Alps",
            86,
            "On Tour",
            "Certified trekking leader for alpine and cultural routes."),
        new(
            "Kenji Tanaka",
            "kenji.guide@wanderx.com",
            "+81312345678",
            "Japanese, English",
            "Culinary Arts",
            "Kyoto and Kansai",
            112,
            "On Tour",
            "Food historian specializing in private markets and seasonal dining."),
        new(
            "Zara Nkosi",
            "zara.guide@wanderx.com",
            "+27111234567",
            "English, French",
            "Safari Wildlife",
            "Southern Africa",
            47,
            "Active",
            "Wildlife interpreter for conservation-first safari experiences."),
        new(
            "Marcus Vane",
            "marcus.guide@wanderx.com",
            "+442012345678",
            "English, German",
            "European History",
            "Western Europe",
            154,
            "Unavailable",
            "Museum-trained historian for heritage and architecture tours."),
        new(
            "Linh Pham",
            "linh.guide@wanderx.com",
            "+84901234567",
            "Vietnamese, English, Korean",
            "Heritage Walks",
            "Hue, Da Nang, Hoi An",
            63,
            "On Tour",
            "Central Vietnam specialist for royal heritage, craft villages, and coastal food trails."),
        new(
            "Aisha Rahman",
            "aisha.guide@wanderx.com",
            "+971501234567",
            "English, French, Thai",
            "Desert Expeditions",
            "UAE and Oman",
            78,
            "Active",
            "Desert logistics guide focused on private family itineraries and soft-adventure routes."),
        new(
            "Diego Silva",
            "diego.guide@wanderx.com",
            "+5511987654321",
            "Spanish, English, Italian",
            "Rainforest Ecology",
            "Amazon Basin",
            39,
            "Active",
            "Eco-guide for rainforest conservation trips, river routes, and birding experiences."),
        new(
            "Mira Novak",
            "mira.guide@wanderx.com",
            "+385911234567",
            "English, German, Italian",
            "Island Sailing",
            "Adriatic Coast",
            91,
            "Unavailable",
            "Sailing host and coastal culture guide for small-group Adriatic itineraries.")
    };

    private static readonly DevelopmentAssignment[] DevelopmentAssignments =
    {
        new("elena.guide@wanderx.com", "WX-ALP-102", "Swiss Alps Photography Trek", "Zermatt, Switzerland", "Europe Alps", -1, 3, 8, "Zermatt Station, north entrance", "Confirmed", "Lead a premium alpine photography route, coordinate sunrise viewpoints, and manage safety pacing."),
        new("elena.guide@wanderx.com", "WX-VEN-214", "Venice Hidden Canals", "Venice, Italy", "Northern Italy", 12, 15, 6, "Hotel Danieli lobby", "Assigned", "Private cultural walking tour with artisan workshop and evening cicchetti tasting."),
        new("elena.guide@wanderx.com", "WX-ROM-078", "Rome After Hours", "Rome, Italy", "Central Italy", -18, -15, 4, "Piazza Navona fountain", "Finished", "After-hours private landmarks route with gallery access, local host coordination, and supper transfer."),
        new("elena.guide@wanderx.com", "WX-PYR-441", "Pyrenees Luxury Traverse", "Andorra la Vella, Andorra", "Europe Alps", 31, 35, 9, "Grand Plaza Hotel reception", "Confirmed", "Four-day mountain traverse with vehicle support, wellness stops, and daily route briefings."),

        new("kenji.guide@wanderx.com", "WX-NAR-504", "Nara Temples and Tea", "Nara, Japan", "Kansai", -2, 1, 7, "Kintetsu Nara Station east gate", "Confirmed", "Temple etiquette support, tea master coordination, and cultural storytelling across private shrine visits."),
        new("kenji.guide@wanderx.com", "WX-KYO-330", "Kyoto Culinary Immersion", "Kyoto, Japan", "Kyoto and Kansai", 5, 9, 10, "Nishiki Market west gate", "Assigned", "Market orientation, tea ceremony coordination, and private kaiseki dining support."),
        new("kenji.guide@wanderx.com", "WX-OSA-119", "Osaka Street Food Lab", "Osaka, Japan", "Kansai", -9, -7, 12, "Namba Station exit 14", "Finished", "Hands-on takoyaki workshop, local bar crawl routing, and dietary preference support."),
        new("kenji.guide@wanderx.com", "WX-TOK-204", "Tokyo Design Weekend", "Tokyo, Japan", "Kanto", 19, 22, 5, "Aoyama Grand Hotel lobby", "Assigned", "Architecture, fashion ateliers, and private design studio visits for a small creative group."),

        new("zara.guide@wanderx.com", "WX-SAF-718", "Serengeti Conservation Safari", "Serengeti, Tanzania", "Southern Africa", 20, 27, 12, "Arusha Coffee Lodge reception", "Assigned", "Wildlife interpretation, conservation briefing, and daily field logistics for safari guests."),
        new("zara.guide@wanderx.com", "WX-KRU-635", "Kruger Big Five Field Notes", "Kruger National Park, South Africa", "Southern Africa", 2, 6, 8, "Skukuza Airport arrivals", "Confirmed", "Morning game drives, track-reading sessions, and family-friendly conservation storytelling."),
        new("zara.guide@wanderx.com", "WX-CPT-088", "Cape Town Coastal Wildlife", "Cape Town, South Africa", "Southern Africa", -25, -22, 6, "V&A Waterfront Clock Tower", "Finished", "Penguin colony visit, marine ecology briefing, and coastal picnic coordination."),
        new("zara.guide@wanderx.com", "WX-VIC-012", "Victoria Falls River Walk", "Livingstone, Zambia", "Southern Africa", 14, 17, 10, "Royal Livingstone Hotel veranda", "Declined", "River trail guiding, waterfall history, and sunset boat support.", "Visa processing conflict with another cross-border assignment."),

        new("marcus.guide@wanderx.com", "WX-LON-901", "London Museum Privileges", "London, United Kingdom", "Western Europe", -10, -8, 5, "British Museum main entrance", "Declined", "Curated museum access with architectural context and private dining transfer.", "Guide unavailable for medical appointment."),
        new("marcus.guide@wanderx.com", "WX-PAR-510", "Paris Belle Epoque", "Paris, France", "Western Europe", 40, 44, 6, "Le Meurice lobby", "Assigned", "Private Belle Epoque architecture route with atelier visit and evening performance transfer."),

        new("linh.guide@wanderx.com", "WX-HUE-226", "Hue Imperial Heritage", "Hue, Vietnam", "Central Vietnam", -1, 2, 9, "Azerai La Residence lobby", "Confirmed", "Imperial Citadel interpretation, dragon boat logistics, and royal cuisine experience coordination."),
        new("linh.guide@wanderx.com", "WX-HAN-315", "Hoi An Lantern Makers", "Hoi An, Vietnam", "Central Vietnam", 8, 10, 5, "Japanese Covered Bridge", "Assigned", "Old town craft route, lantern workshop translation, and riverside dinner support."),
        new("linh.guide@wanderx.com", "WX-DNG-144", "Da Nang Coastal Wellness", "Da Nang, Vietnam", "Central Vietnam", -16, -13, 4, "InterContinental Sun Peninsula lobby", "Finished", "Wellness-focused coastal route with spa transfers, seafood tasting, and sunrise photo stops."),

        new("aisha.guide@wanderx.com", "WX-DXB-618", "Dubai Design and Desert", "Dubai, UAE", "UAE and Oman", 6, 8, 7, "Museum of the Future entrance", "Confirmed", "Contemporary design route, private desert camp handoff, and family-friendly pacing."),
        new("aisha.guide@wanderx.com", "WX-MCT-407", "Muscat Forts and Frankincense", "Muscat, Oman", "UAE and Oman", 23, 26, 6, "Al Alam Palace parking court", "Assigned", "Coastal forts, souq interpretation, and frankincense workshop coordination."),
        new("aisha.guide@wanderx.com", "WX-AUH-090", "Abu Dhabi Grand Mosque Etiquette", "Abu Dhabi, UAE", "UAE and Oman", -13, -12, 11, "Grand Mosque visitor center", "Declined", "Cultural etiquette briefing and mosque architecture tour.", "Family emergency; requested reassignment before confirmation."),

        new("diego.guide@wanderx.com", "WX-AMZ-552", "Amazon Dawn Birding", "Manaus, Brazil", "Amazon Basin", 11, 16, 6, "Manaus river port pier 3", "Assigned", "Dawn birding route, canopy safety briefing, and lodge-to-river coordination."),
        new("diego.guide@wanderx.com", "WX-PER-662", "Peruvian Cloud Forest", "Cusco, Peru", "Amazon Basin", 29, 34, 8, "Cusco airport domestic arrivals", "Confirmed", "Cloud forest ecology, lodge orientation, and daily trail difficulty checks."),
        new("diego.guide@wanderx.com", "WX-RIO-203", "Rio Atlantic Forest Day", "Rio de Janeiro, Brazil", "Brazil Coast", -21, -20, 5, "Copacabana Palace entrance", "Finished", "Atlantic forest nature walk, viewpoint timing, and local lunch coordination."),

        new("mira.guide@wanderx.com", "WX-ADR-884", "Croatian Islands by Sail", "Split, Croatia", "Adriatic Coast", 17, 24, 8, "Split marina gate B", "Assigned", "Island-hopping route, marina coordination, swim-stop safety, and coastal storytelling."),
        new("mira.guide@wanderx.com", "WX-DBV-733", "Dubrovnik Walls at Sunrise", "Dubrovnik, Croatia", "Adriatic Coast", -6, -5, 4, "Pile Gate outer bridge", "Finished", "Early-access wall walk, filming-location context, and breakfast terrace transfer."),
        new("mira.guide@wanderx.com", "WX-HVR-520", "Hvar Wine and Sail", "Hvar, Croatia", "Adriatic Coast", 4, 7, 6, "Hvar harbor customs pier", "Declined", "Sailing day with vineyard visit and island dinner booking.", "Boat captain schedule changed; guide requested operations review.")
    };

    private static readonly DevelopmentTour[] DevelopmentTours =
    {
        new(
            1,
            "WX-HUE-226",
            "Hue Imperial Heritage",
            "Hue, Vietnam",
            "Central Vietnam",
            4,
            1290,
            12,
            "Published",
            "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=80",
            "A private heritage route through Hue's imperial citadel, royal cuisine, dragon boat moments, and quiet garden houses."),
        new(
            2,
            "WX-KYO-330",
            "Kyoto Culinary Immersion",
            "Kyoto, Japan",
            "Kyoto and Kansai",
            5,
            2380,
            10,
            "Published",
            "https://images.unsplash.com/photo-1493976040374-85c8e12f0c0e?auto=format&fit=crop&w=1200&q=80",
            "Seasonal market walks, tea ceremony coordination, private kitchens, and a polished kaiseki dining story."),
        new(
            3,
            "WX-SAF-718",
            "Serengeti Conservation Safari",
            "Serengeti, Tanzania",
            "Southern Africa",
            8,
            5200,
            12,
            "Draft",
            "https://images.unsplash.com/photo-1516426122078-c23e76319801?auto=format&fit=crop&w=1200&q=80",
            "Conservation-first safari planning with daily field logistics, wildlife interpretation, and lodge handoffs."),
        new(
            4,
            "WX-ADR-884",
            "Croatian Islands by Sail",
            "Split, Croatia",
            "Adriatic Coast",
            8,
            3420,
            8,
            "Archived",
            "https://images.unsplash.com/photo-1555990538-c48dbe6465d7?auto=format&fit=crop&w=1200&q=80",
            "Island-hopping by sail with marina coordination, coastal culture, swim stops, and relaxed private hosting.")
    };

    private const string SampleEvidenceImageBase64 = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==";

    private sealed record DevelopmentGuide(
        string FullName,
        string Email,
        string PhoneNumber,
        string Languages,
        string ExpertiseArea,
        string Region,
        int CompletedTours,
        string Status,
        string Bio);

    private sealed record DevelopmentAssignment(
        string GuideEmail,
        string TourCode,
        string TourName,
        string Destination,
        string Region,
        int StartOffsetDays,
        int EndOffsetDays,
        int TravelerCount,
        string MeetingPoint,
        string Status,
        string ItinerarySummary,
        string? DeclineReason = null);

    private sealed record DevelopmentTour(
        int ScheduleTourId,
        string Code,
        string Name,
        string Destination,
        string Region,
        int DurationDays,
        decimal Price,
        int Capacity,
        string Status,
        string ImageUrl,
        string Description);

    private sealed record ScheduleDayTemplate(
        string Title,
        string Description,
        TimeSpan StartTime,
        TimeSpan EndTime);
}
