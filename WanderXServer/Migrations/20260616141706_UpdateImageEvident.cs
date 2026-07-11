using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WanderXServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageEvident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuideTourAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuideProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TourName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TravelerCount = table.Column<int>(type: "int", nullable: false),
                    MeetingPoint = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ItinerarySummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DeclineReason = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    DeclinedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EvidenceImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideTourAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideTourAssignments_GuideProfiles_GuideProfileId",
                        column: x => x.GuideProfileId,
                        principalTable: "GuideProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuideTourAssignments_GuideProfileId",
                table: "GuideTourAssignments",
                column: "GuideProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuideTourAssignments");
        }
    }
}
