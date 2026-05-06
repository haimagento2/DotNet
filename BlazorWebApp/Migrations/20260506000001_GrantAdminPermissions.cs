using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class GrantAdminPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Grant admin permission to some license members
            // Update members with IDs 1, 3, 5 to admin permission (if they exist)
            migrationBuilder.Sql(@"
                UPDATE LicenseMembers
                SET Permission = 2
                WHERE Id IN (1, 3, 5) AND Permission != 1
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert admin permissions back to member
            migrationBuilder.Sql(@"
                UPDATE LicenseMembers
                SET Permission = 3
                WHERE Id IN (1, 3, 5) AND Permission = 2
            ");
        }
    }
}