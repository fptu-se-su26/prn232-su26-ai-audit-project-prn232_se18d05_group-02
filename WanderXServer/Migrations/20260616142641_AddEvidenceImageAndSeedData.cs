using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WanderXServer.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenceImageAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsEmailConfirmed", "IsPhoneConfirmed", "LastLoginAt", "NormalizedEmail", "PasswordHash", "PhoneNumber", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("17dfb04d-c76b-2ca8-53dd-d046876722bb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "kenji.guide@wanderx.com", "Kenji Tanaka", true, true, null, "KENJI.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAEPzlRpac0CX5r90IrhUOzSRR7Adt0xogl/pVSoJLvjMpoUPzXGWw6pJ5xQOeo/8r/A==", "+81312345678", 2, null },
                    { new Guid("1ce80d60-b399-5bed-4433-56519702ab4b"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "diego.guide@wanderx.com", "Diego Silva", true, true, null, "DIEGO.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAEHfkEZ1J6390rf+n3fqRFHWuAGcwltDK6BZT4hb9BwxglvHTQ1ogFZKJIKTq7lrhxg==", "+5511987654321", 2, null },
                    { new Guid("291d83be-f690-836b-0bc7-9e09068249d5"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "zara.guide@wanderx.com", "Zara Nkosi", true, true, null, "ZARA.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAELY+pD+0iJsZ63c6Txr0KKFqA191DYxZDBNMmN6TYvIhmJD0Y2PYhfpdXbUBocaCKw==", "+27111234567", 2, null },
                    { new Guid("50294718-b00a-5ade-e743-dd51703db16f"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "linh.guide@wanderx.com", "Linh Pham", true, true, null, "LINH.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAEBcoo4OE8IlBDsRItsRN6hJH++5+zYDITnuIlH/7DNnFOsdZSe5Exep3KAt5tBf6pQ==", "+84901234567", 2, null },
                    { new Guid("7e47509a-5539-f246-0ed1-f4c44503381e"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "aisha.guide@wanderx.com", "Aisha Rahman", true, true, null, "AISHA.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAEGafICFfZnZ9nadQ7wGBep92Fh4XgkxjMFkHNgRxezEdzfbz+/aZKQFk/bC23bWDcA==", "+971501234567", 2, null },
                    { new Guid("85c94e16-4303-95d4-be76-141669bbba55"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ad@ad.123", "WanderX Admin", true, true, null, "AD@AD.123", "AQAAAAIAAYagAAAAEI1VqXHBOtlcn12zpeyO7lIagmatjR2Y0Rrre/+ZoF5EVdMOSHwcX8Tk0rmDgh18oQ==", "+10000000000", 4, null },
                    { new Guid("95fac87b-89ea-2fb1-3728-cc3ae139d5c1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "elena.guide@wanderx.com", "Elena Rodriguez", true, true, null, "ELENA.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAEARtUt3MoUz/mYa4kjNObxVMNvEzO9R95l6q06r3kZ/v/5ZIc4tgbwlvSxTjiZYFug==", "+34123456789", 2, null },
                    { new Guid("d8f1c642-e5df-297c-4c2b-46474f46b966"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "marcus.guide@wanderx.com", "Marcus Vane", true, true, null, "MARCUS.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAECRCGG5A+4TkOQRpJlu2yfBXkFKs4kWlWx7EJcF10w54wyzlwkfFs3k1jEAhKlvXJg==", "+442012345678", 2, null },
                    { new Guid("f20871e7-c4a8-9a20-5868-413a053dbc7d"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mira.guide@wanderx.com", "Mira Novak", true, true, null, "MIRA.GUIDE@WANDERX.COM", "AQAAAAIAAYagAAAAENGgH8ftl0hqtOs6aPca/bFtNaNY9VxYwE1SXur00ah2gOqds2cgn19iVowSJcaQYQ==", "+385911234567", 2, null }
                });

            migrationBuilder.InsertData(
                table: "GuideProfiles",
                columns: new[] { "Id", "Bio", "CompletedTours", "CreatedAt", "ExpertiseArea", "Languages", "Region", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("0fb26a69-2fb8-1da1-55a3-45e6198d3da8"), "Sailing host and coastal culture guide for small-group Adriatic itineraries.", 91, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Island Sailing", "English, German, Italian", "Adriatic Coast", "Unavailable", null, new Guid("f20871e7-c4a8-9a20-5868-413a053dbc7d") },
                    { new Guid("371d8c63-9c8c-9c42-9baa-454e86047fc9"), "Desert logistics guide focused on private family itineraries and soft-adventure routes.", 78, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Desert Expeditions", "English, French, Thai", "UAE and Oman", "Active", null, new Guid("7e47509a-5539-f246-0ed1-f4c44503381e") },
                    { new Guid("7aa42825-68f1-e8cb-d48f-59fc348793b0"), "Wildlife interpreter for conservation-first safari experiences.", 47, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Safari Wildlife", "English, French", "Southern Africa", "Active", null, new Guid("291d83be-f690-836b-0bc7-9e09068249d5") },
                    { new Guid("7d5aa862-058c-0a75-79b8-3b82115eddc2"), "Eco-guide for rainforest conservation trips, river routes, and birding experiences.", 39, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rainforest Ecology", "Spanish, English, Italian", "Amazon Basin", "Active", null, new Guid("1ce80d60-b399-5bed-4433-56519702ab4b") },
                    { new Guid("a6512362-9ce5-25b4-190c-ba15b81d6db3"), "Museum-trained historian for heritage and architecture tours.", 154, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "European History", "English, German", "Western Europe", "Unavailable", null, new Guid("d8f1c642-e5df-297c-4c2b-46474f46b966") },
                    { new Guid("b8675ef8-50e0-79b1-4a5f-4bbffdaef105"), "Central Vietnam specialist for royal heritage, craft villages, and coastal food trails.", 63, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Heritage Walks", "Vietnamese, English, Korean", "Hue, Da Nang, Hoi An", "On Tour", null, new Guid("50294718-b00a-5ade-e743-dd51703db16f") },
                    { new Guid("c12f1645-3d28-c152-5960-6c1e15958131"), "Food historian specializing in private markets and seasonal dining.", 112, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Culinary Arts", "Japanese, English", "Kyoto and Kansai", "On Tour", null, new Guid("17dfb04d-c76b-2ca8-53dd-d046876722bb") },
                    { new Guid("f6b68305-41c7-ecf1-b56f-85775e02b7f7"), "Certified trekking leader for alpine and cultural routes.", 86, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-Altitude Trekking", "Spanish, English, French", "Europe Alps", "On Tour", null, new Guid("95fac87b-89ea-2fb1-3728-cc3ae139d5c1") }
                });

            migrationBuilder.InsertData(
                table: "GuideTourAssignments",
                columns: new[] { "Id", "CreatedAt", "DeclineReason", "DeclinedAt", "Destination", "EndDate", "EvidenceImage", "FinishedAt", "GuideProfileId", "ItinerarySummary", "MeetingPoint", "Region", "StartDate", "Status", "TourCode", "TourName", "TravelerCount", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("03590cb6-1594-8533-dbfa-0792e4d667e3"), new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Cape Town, South Africa", new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==", new DateTime(2026, 5, 25, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("7aa42825-68f1-e8cb-d48f-59fc348793b0"), "Penguin colony visit, marine ecology briefing, and coastal picnic coordination.", "V&A Waterfront Clock Tower", "Southern Africa", new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Finished", "WX-CPT-088", "Cape Town Coastal Wildlife", 6, new DateTime(2026, 5, 25, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("08ff049e-b97b-7a33-f88d-6e6f3bb0fb85"), new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Serengeti, Tanzania", new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("7aa42825-68f1-e8cb-d48f-59fc348793b0"), "Wildlife interpretation, conservation briefing, and daily field logistics for safari guests.", "Arusha Coffee Lodge reception", "Southern Africa", new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-SAF-718", "Serengeti Conservation Safari", 12, null },
                    { new Guid("10899a07-b207-8825-76b9-63e7dcf0562d"), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Family emergency; requested reassignment before confirmation.", new DateTime(2026, 6, 4, 10, 0, 0, 0, DateTimeKind.Utc), "Abu Dhabi, UAE", new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("371d8c63-9c8c-9c42-9baa-454e86047fc9"), "Cultural etiquette briefing and mosque architecture tour.", "Grand Mosque visitor center", "UAE and Oman", new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Declined", "WX-AUH-090", "Abu Dhabi Grand Mosque Etiquette", 11, new DateTime(2026, 6, 4, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11a5eaaa-9b23-40a1-adf0-38c8b62ed32d"), new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Paris, France", new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("a6512362-9ce5-25b4-190c-ba15b81d6db3"), "Private Belle Epoque architecture route with atelier visit and evening performance transfer.", "Le Meurice lobby", "Western Europe", new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-PAR-510", "Paris Belle Epoque", 6, null },
                    { new Guid("19d7922e-1f39-7acc-d726-82b057ce6df8"), new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Venice, Italy", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("f6b68305-41c7-ecf1-b56f-85775e02b7f7"), "Private cultural walking tour with artisan workshop and evening cicchetti tasting.", "Hotel Danieli lobby", "Northern Italy", new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-VEN-214", "Venice Hidden Canals", 6, null },
                    { new Guid("26af094f-eb8b-837c-5ff2-36296adf47ea"), new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Kyoto, Japan", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("c12f1645-3d28-c152-5960-6c1e15958131"), "Market orientation, tea ceremony coordination, and private kaiseki dining support.", "Nishiki Market west gate", "Kyoto and Kansai", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-KYO-330", "Kyoto Culinary Immersion", 10, null },
                    { new Guid("2756a29c-7607-ed22-fda9-318b100098a5"), new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Visa processing conflict with another cross-border assignment.", new DateTime(2026, 7, 3, 10, 0, 0, 0, DateTimeKind.Utc), "Livingstone, Zambia", new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("7aa42825-68f1-e8cb-d48f-59fc348793b0"), "River trail guiding, waterfall history, and sunset boat support.", "Royal Livingstone Hotel veranda", "Southern Africa", new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Declined", "WX-VIC-012", "Victoria Falls River Walk", 10, new DateTime(2026, 7, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("28287815-5d53-79f3-ef65-dc4049d0d165"), new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Dubrovnik, Croatia", new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==", new DateTime(2026, 6, 11, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("0fb26a69-2fb8-1da1-55a3-45e6198d3da8"), "Early-access wall walk, filming-location context, and breakfast terrace transfer.", "Pile Gate outer bridge", "Adriatic Coast", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Finished", "WX-DBV-733", "Dubrovnik Walls at Sunrise", 4, new DateTime(2026, 6, 11, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2836fd79-806d-9a63-ecf7-02d8dcd5da82"), new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Rome, Italy", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==", new DateTime(2026, 6, 1, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("f6b68305-41c7-ecf1-b56f-85775e02b7f7"), "After-hours private landmarks route with gallery access, local host coordination, and supper transfer.", "Piazza Navona fountain", "Central Italy", new DateTime(2026, 5, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Finished", "WX-ROM-078", "Rome After Hours", 4, new DateTime(2026, 6, 1, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2fa60f03-1a79-8a76-2b4f-0dae8853a701"), new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Kruger National Park, South Africa", new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("7aa42825-68f1-e8cb-d48f-59fc348793b0"), "Morning game drives, track-reading sessions, and family-friendly conservation storytelling.", "Skukuza Airport arrivals", "Southern Africa", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-KRU-635", "Kruger Big Five Field Notes", 8, null },
                    { new Guid("3355672a-fa40-0b58-208d-ac4a10788969"), new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Boat captain schedule changed; guide requested operations review.", new DateTime(2026, 6, 23, 10, 0, 0, 0, DateTimeKind.Utc), "Hvar, Croatia", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("0fb26a69-2fb8-1da1-55a3-45e6198d3da8"), "Sailing day with vineyard visit and island dinner booking.", "Hvar harbor customs pier", "Adriatic Coast", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Declined", "WX-HVR-520", "Hvar Wine and Sail", 6, new DateTime(2026, 6, 23, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("3823c9c1-f34e-8b72-e025-ff01c6edcd13"), new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Muscat, Oman", new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("371d8c63-9c8c-9c42-9baa-454e86047fc9"), "Coastal forts, souq interpretation, and frankincense workshop coordination.", "Al Alam Palace parking court", "UAE and Oman", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-MCT-407", "Muscat Forts and Frankincense", 6, null },
                    { new Guid("434cec88-3548-881f-0908-654c115c9c60"), new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Dubai, UAE", new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("371d8c63-9c8c-9c42-9baa-454e86047fc9"), "Contemporary design route, private desert camp handoff, and family-friendly pacing.", "Museum of the Future entrance", "UAE and Oman", new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-DXB-618", "Dubai Design and Desert", 7, null },
                    { new Guid("46a779a8-d13d-1481-486f-f1714e8d7dc5"), new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Nara, Japan", new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("c12f1645-3d28-c152-5960-6c1e15958131"), "Temple etiquette support, tea master coordination, and cultural storytelling across private shrine visits.", "Kintetsu Nara Station east gate", "Kansai", new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-NAR-504", "Nara Temples and Tea", 7, null },
                    { new Guid("679949fb-cd9d-effb-d009-8cee79678322"), new DateTime(2026, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Osaka, Japan", new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==", new DateTime(2026, 6, 9, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("c12f1645-3d28-c152-5960-6c1e15958131"), "Hands-on takoyaki workshop, local bar crawl routing, and dietary preference support.", "Namba Station exit 14", "Kansai", new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Finished", "WX-OSA-119", "Osaka Street Food Lab", 12, new DateTime(2026, 6, 9, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("6a9c12e1-7de2-c2dd-6b42-9f51fd57af6e"), new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Da Nang, Vietnam", new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==", new DateTime(2026, 6, 3, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("b8675ef8-50e0-79b1-4a5f-4bbffdaef105"), "Wellness-focused coastal route with spa transfers, seafood tasting, and sunrise photo stops.", "InterContinental Sun Peninsula lobby", "Central Vietnam", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Finished", "WX-DNG-144", "Da Nang Coastal Wellness", 4, new DateTime(2026, 6, 3, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("6c700894-792b-e7b3-a034-d56ef3d05d26"), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Zermatt, Switzerland", new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("f6b68305-41c7-ecf1-b56f-85775e02b7f7"), "Lead a premium alpine photography route, coordinate sunrise viewpoints, and manage safety pacing.", "Zermatt Station, north entrance", "Europe Alps", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-ALP-102", "Swiss Alps Photography Trek", 8, null },
                    { new Guid("810a2896-c7b3-b391-b934-390e0ac90d8b"), new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Rio de Janeiro, Brazil", new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0MDAiIGhlaWdodD0iMzAwIiB2aWV3Qm94PSIwIDAgNDAwIDMwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2Y0ZjZmOCIvPjxjaXJjbGUgY3g9IjIwMCIgY3k9IjEyMCIgcj0iNDUiIGZpbGw9IiNlOGY1ZTkiLz48cGF0aCBkPSJNMTg1LDEyMCBMMTk1LDEzMCBMMjE1LDExMCIgc3Ryb2tlPSIjMmU3ZDMyIiBzdHJva2Utd2lkdGg9IjYiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIgZmlsbD0ibm9uZSIvPjx0ZXh0IHg9IjIwMCIgeT0iMjAwIiBmb250LWZhbWlseT0ic2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZm9udC1zdHlsZT0ibm9ybWFsIiBmb250LXdlaWdodD0iYm9sZCIgZmlsbD0iIzJjM2U1MCIgdGV4dC1hbmNob3I9Im1pZGRsZSI+VG91ciBFdmlkZW5jZSBQcm9vZjwvdGV4dD48dGV4dCB4PSIyMDAiIHk9IjIyNSIgZm9udC1mYW1pbHk9InNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTUiIGZpbGw9IiM3ZjhjOGQiIHRleHQtYW5jaG9yPSJtaWRkbGUiPldhbmRlclggVmVyaWZpZWQgRmluaXNoPC90ZXh0Pjwvc3ZnPg==", new DateTime(2026, 5, 27, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("7d5aa862-058c-0a75-79b8-3b82115eddc2"), "Atlantic forest nature walk, viewpoint timing, and local lunch coordination.", "Copacabana Palace entrance", "Brazil Coast", new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Finished", "WX-RIO-203", "Rio Atlantic Forest Day", 5, new DateTime(2026, 5, 27, 18, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("9550475c-e646-90fb-5550-500b9773f62b"), new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Split, Croatia", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("0fb26a69-2fb8-1da1-55a3-45e6198d3da8"), "Island-hopping route, marina coordination, swim-stop safety, and coastal storytelling.", "Split marina gate B", "Adriatic Coast", new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-ADR-884", "Croatian Islands by Sail", 8, null },
                    { new Guid("aa6a3c1e-ceb3-4263-a3cc-0888bebfb0b9"), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hue, Vietnam", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("b8675ef8-50e0-79b1-4a5f-4bbffdaef105"), "Imperial Citadel interpretation, dragon boat logistics, and royal cuisine experience coordination.", "Azerai La Residence lobby", "Central Vietnam", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-HUE-226", "Hue Imperial Heritage", 9, null },
                    { new Guid("b8b6e387-e407-e999-6721-6f9ba108e64d"), new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Andorra la Vella, Andorra", new DateTime(2026, 7, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("f6b68305-41c7-ecf1-b56f-85775e02b7f7"), "Four-day mountain traverse with vehicle support, wellness stops, and daily route briefings.", "Grand Plaza Hotel reception", "Europe Alps", new DateTime(2026, 7, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-PYR-441", "Pyrenees Luxury Traverse", 9, null },
                    { new Guid("baf5d3d5-ee11-72d3-ca31-c786b76a3197"), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Tokyo, Japan", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("c12f1645-3d28-c152-5960-6c1e15958131"), "Architecture, fashion ateliers, and private design studio visits for a small creative group.", "Aoyama Grand Hotel lobby", "Kanto", new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-TOK-204", "Tokyo Design Weekend", 5, null },
                    { new Guid("cbcc468d-f978-02b4-ecbe-f76a0d5d17da"), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hoi An, Vietnam", new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("b8675ef8-50e0-79b1-4a5f-4bbffdaef105"), "Old town craft route, lantern workshop translation, and riverside dinner support.", "Japanese Covered Bridge", "Central Vietnam", new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-HAN-315", "Hoi An Lantern Makers", 5, null },
                    { new Guid("d4662d14-7966-cab3-bfc8-f325bc15514b"), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Cusco, Peru", new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("7d5aa862-058c-0a75-79b8-3b82115eddc2"), "Cloud forest ecology, lodge orientation, and daily trail difficulty checks.", "Cusco airport domestic arrivals", "Amazon Basin", new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", "WX-PER-662", "Peruvian Cloud Forest", 8, null },
                    { new Guid("da8528f1-132b-0514-32cb-f2eef3cdbb27"), new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Manaus, Brazil", new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("7d5aa862-058c-0a75-79b8-3b82115eddc2"), "Dawn birding route, canopy safety briefing, and lodge-to-river coordination.", "Manaus river port pier 3", "Amazon Basin", new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Assigned", "WX-AMZ-552", "Amazon Dawn Birding", 6, null },
                    { new Guid("ee381434-dcf6-d2a8-4786-35274ffb2330"), new DateTime(2026, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Guide unavailable for medical appointment.", new DateTime(2026, 6, 8, 10, 0, 0, 0, DateTimeKind.Utc), "London, United Kingdom", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("a6512362-9ce5-25b4-190c-ba15b81d6db3"), "Curated museum access with architectural context and private dining transfer.", "British Museum main entrance", "Western Europe", new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Declined", "WX-LON-901", "London Museum Privileges", 5, new DateTime(2026, 6, 8, 10, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("03590cb6-1594-8533-dbfa-0792e4d667e3"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("08ff049e-b97b-7a33-f88d-6e6f3bb0fb85"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("10899a07-b207-8825-76b9-63e7dcf0562d"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("11a5eaaa-9b23-40a1-adf0-38c8b62ed32d"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("19d7922e-1f39-7acc-d726-82b057ce6df8"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("26af094f-eb8b-837c-5ff2-36296adf47ea"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("2756a29c-7607-ed22-fda9-318b100098a5"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("28287815-5d53-79f3-ef65-dc4049d0d165"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("2836fd79-806d-9a63-ecf7-02d8dcd5da82"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("2fa60f03-1a79-8a76-2b4f-0dae8853a701"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("3355672a-fa40-0b58-208d-ac4a10788969"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("3823c9c1-f34e-8b72-e025-ff01c6edcd13"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("434cec88-3548-881f-0908-654c115c9c60"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("46a779a8-d13d-1481-486f-f1714e8d7dc5"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("679949fb-cd9d-effb-d009-8cee79678322"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("6a9c12e1-7de2-c2dd-6b42-9f51fd57af6e"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("6c700894-792b-e7b3-a034-d56ef3d05d26"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("810a2896-c7b3-b391-b934-390e0ac90d8b"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("9550475c-e646-90fb-5550-500b9773f62b"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("aa6a3c1e-ceb3-4263-a3cc-0888bebfb0b9"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("b8b6e387-e407-e999-6721-6f9ba108e64d"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("baf5d3d5-ee11-72d3-ca31-c786b76a3197"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("cbcc468d-f978-02b4-ecbe-f76a0d5d17da"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("d4662d14-7966-cab3-bfc8-f325bc15514b"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("da8528f1-132b-0514-32cb-f2eef3cdbb27"));

            migrationBuilder.DeleteData(
                table: "GuideTourAssignments",
                keyColumn: "Id",
                keyValue: new Guid("ee381434-dcf6-d2a8-4786-35274ffb2330"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("85c94e16-4303-95d4-be76-141669bbba55"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("0fb26a69-2fb8-1da1-55a3-45e6198d3da8"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("371d8c63-9c8c-9c42-9baa-454e86047fc9"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("7aa42825-68f1-e8cb-d48f-59fc348793b0"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("7d5aa862-058c-0a75-79b8-3b82115eddc2"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("a6512362-9ce5-25b4-190c-ba15b81d6db3"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("b8675ef8-50e0-79b1-4a5f-4bbffdaef105"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("c12f1645-3d28-c152-5960-6c1e15958131"));

            migrationBuilder.DeleteData(
                table: "GuideProfiles",
                keyColumn: "Id",
                keyValue: new Guid("f6b68305-41c7-ecf1-b56f-85775e02b7f7"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("17dfb04d-c76b-2ca8-53dd-d046876722bb"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1ce80d60-b399-5bed-4433-56519702ab4b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("291d83be-f690-836b-0bc7-9e09068249d5"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("50294718-b00a-5ade-e743-dd51703db16f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e47509a-5539-f246-0ed1-f4c44503381e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("95fac87b-89ea-2fb1-3728-cc3ae139d5c1"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d8f1c642-e5df-297c-4c2b-46474f46b966"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f20871e7-c4a8-9a20-5868-413a053dbc7d"));
        }
    }
}
