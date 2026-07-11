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

    public DbSet<Tour> Tours => Set<Tour>();

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
        EnsureGuideTourAssignmentsTable();
        EnsureToursTable();

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        SeedAdmin(passwordHasher);
        SeedGuides(passwordHasher);

        SaveChanges();
        SeedGuideTourAssignments();
        SeedTours();
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
        [DurationDays] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Capacity] int NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        [ImageUrl] nvarchar(500) NOT NULL,
        [Description] nvarchar(1200) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Tours] PRIMARY KEY ([Id])
    );

    CREATE UNIQUE INDEX [IX_Tours_Code] ON [Tours] ([Code]);
END
""");
    }

    private void SeedGuideTourAssignments()
    {
        var today = DateTime.Today;

        foreach (var assignment in DevelopmentAssignments)
        {
            AddAssignment(today, assignment);
        }
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
}
