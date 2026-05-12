using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramImageAndLockServicesFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LockedUrlKey",
                table: "Programs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PadlockDisplay",
                table: "Programs",
                type: "TEXT",
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AddColumn<bool>(
                name: "RedirectToLockedUrlKey",
                table: "Programs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProgramImage",
                table: "Programs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedUrlKey",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "PadlockDisplay",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "RedirectToLockedUrlKey",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProgramImage",
                table: "Programs");
        }
    }
}
