using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WanderXServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSpecialRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TourName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuestCount = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestFailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentFailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmationFailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionFailedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPassengers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TicketType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPassengers_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSpecialRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminResponse = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpecialRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSpecialRequests_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("17dfb04d-c76b-2ca8-53dd-d046876722bb"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEIx8fgcdkvfrPjQGqd4m+e/zyRnqCDm1n9aGdbjgRC6VT4xEaIpmOzm+KidKGq38og==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1ce80d60-b399-5bed-4433-56519702ab4b"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEAVj3bI5WVtwap25zXS7gAhazmhJnfJePiQkXHejfPN0u3d8Oxpx/4txbyh9AHeHCA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("291d83be-f690-836b-0bc7-9e09068249d5"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEPOMnoXsNxOailOYY8xxRvy/jT4nYnCZDPasUpDpyjrJdpzVsqjVN3lOdxxnXZbtGA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("50294718-b00a-5ade-e743-dd51703db16f"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEM5tcmLUECAW8Fkb12tgwBK+DsxncZ9NSPUgcXb4ll5HMdHF6xGEiDUKKe3uXP9krg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e47509a-5539-f246-0ed1-f4c44503381e"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEFEkC10x7wbQNTZcVM/czNyzMuR7jkEkCYl8mKB7c9EiaH5mkg7bW8swiI6WU8L5hA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("85c94e16-4303-95d4-be76-141669bbba55"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAELzrmn2Fi3pxcdppGEVdb00CPy7ZOtDLYeXfrg88FWJWbMK/28PwuforR/4RIWlP7Q==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("95fac87b-89ea-2fb1-3728-cc3ae139d5c1"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEBKQjHHnzneogfJsVcsdM7iSYqz9tq855YRGV5TzSXyWjCxT7393xStGZ9ri/RvqAw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d8f1c642-e5df-297c-4c2b-46474f46b966"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAENyf110svtBdb2W9lUDHy7xla7kdzqB+t76HCYvQWEvuvLvVaKRGgS9IX7WQpTchDQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f20871e7-c4a8-9a20-5868-413a053dbc7d"),
                columns: new[] { "Address", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAECMEwTj4fkIsc+GLAPjUEqgcMY8Sr0PpEzhcU5WtGScwc51vFKJ5aJeT+EGB/e5G8w==" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengers_BookingId",
                table: "BookingPassengers",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecialRequests_BookingId",
                table: "UserSpecialRequests",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPassengers");

            migrationBuilder.DropTable(
                name: "UserSpecialRequests");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("17dfb04d-c76b-2ca8-53dd-d046876722bb"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPzlRpac0CX5r90IrhUOzSRR7Adt0xogl/pVSoJLvjMpoUPzXGWw6pJ5xQOeo/8r/A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1ce80d60-b399-5bed-4433-56519702ab4b"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHfkEZ1J6390rf+n3fqRFHWuAGcwltDK6BZT4hb9BwxglvHTQ1ogFZKJIKTq7lrhxg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("291d83be-f690-836b-0bc7-9e09068249d5"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELY+pD+0iJsZ63c6Txr0KKFqA191DYxZDBNMmN6TYvIhmJD0Y2PYhfpdXbUBocaCKw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("50294718-b00a-5ade-e743-dd51703db16f"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBcoo4OE8IlBDsRItsRN6hJH++5+zYDITnuIlH/7DNnFOsdZSe5Exep3KAt5tBf6pQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e47509a-5539-f246-0ed1-f4c44503381e"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGafICFfZnZ9nadQ7wGBep92Fh4XgkxjMFkHNgRxezEdzfbz+/aZKQFk/bC23bWDcA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("85c94e16-4303-95d4-be76-141669bbba55"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI1VqXHBOtlcn12zpeyO7lIagmatjR2Y0Rrre/+ZoF5EVdMOSHwcX8Tk0rmDgh18oQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("95fac87b-89ea-2fb1-3728-cc3ae139d5c1"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEARtUt3MoUz/mYa4kjNObxVMNvEzO9R95l6q06r3kZ/v/5ZIc4tgbwlvSxTjiZYFug==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d8f1c642-e5df-297c-4c2b-46474f46b966"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECRCGG5A+4TkOQRpJlu2yfBXkFKs4kWlWx7EJcF10w54wyzlwkfFs3k1jEAhKlvXJg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f20871e7-c4a8-9a20-5868-413a053dbc7d"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENGgH8ftl0hqtOs6aPca/bFtNaNY9VxYwE1SXur00ah2gOqds2cgn19iVowSJcaQYQ==");
        }
    }
}
