using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "FirstName", "LastName", "Email", "Phone", "Company", "City", "CreatedAt" },
                values: new object[,]
                {
                    { 1, "John", "Anderson", "john.anderson@example.com", "+1-555-0101", "TechCorp", "New York", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Sarah", "Bennett", "sarah.bennett@example.com", "+1-555-0102", "InnovateLabs", "San Francisco", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Michael", "Chen", "michael.chen@example.com", "+1-555-0103", "DataSystems", "Los Angeles", new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Emily", "Davis", "emily.davis@example.com", "+1-555-0104", "CloudNext", "Chicago", new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "David", "Edwards", "david.edwards@example.com", "+1-555-0105", "FastTrack", "Houston", new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Jessica", "Garcia", "jessica.garcia@example.com", "+1-555-0106", "SecureNet", "Phoenix", new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Robert", "Harris", "robert.harris@example.com", "+1-555-0107", "SmartBuild", "Philadelphia", new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Amanda", "Johnson", "amanda.johnson@example.com", "+1-555-0108", "VisionCore", "San Antonio", new DateTime(2025, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Christopher", "King", "christopher.king@example.com", "+1-555-0109", "FutureAI", "San Diego", new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Victoria", "Lee", "victoria.lee@example.com", "+1-555-0110", "PowerTech", "Dallas", new DateTime(2025, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Nathan", "Martinez", "nathan.martinez@example.com", "+1-555-0111", "QuantumEdge", "Austin", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Olivia", "Nelson", "olivia.nelson@example.com", "+1-555-0112", "BrightPath", "Seattle", new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Ethan", "Parker", "ethan.parker@example.com", "+1-555-0113", "NexaGroup", "Denver", new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "Sophia", "Quinn", "sophia.quinn@example.com", "+1-555-0114", "AlphaLogic", "Boston", new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "Liam", "Rivera", "liam.rivera@example.com", "+1-555-0115", "CoreWave", "Miami", new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "Isabella", "Scott", "isabella.scott@example.com", "+1-555-0116", "PinnacleIO", "Atlanta", new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "Mason", "Turner", "mason.turner@example.com", "+1-555-0117", "SkyLink", "Portland", new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, "Ava", "Underwood", "ava.underwood@example.com", "+1-555-0118", "GridForce", "Minneapolis", new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, "Logan", "Vasquez", "logan.vasquez@example.com", "+1-555-0119", "NovaSpark", "Las Vegas", new DateTime(2025, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, "Mia", "Walsh", "mia.walsh@example.com", "+1-555-0120", "ZenithTech", "Nashville", new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
