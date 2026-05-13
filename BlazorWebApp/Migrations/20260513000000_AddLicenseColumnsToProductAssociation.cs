using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWebApp.Migrations
{
    public partial class AddLicenseColumnsToProductAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "ProgramProductAssociated",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "period",
                table: "ProgramProductAssociated",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "linking_license",
                table: "ProgramProductAssociated",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "community_only",
                table: "ProgramProductAssociated",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "ProgramProductAssociated");

            migrationBuilder.DropColumn(
                name: "period",
                table: "ProgramProductAssociated");

            migrationBuilder.DropColumn(
                name: "linking_license",
                table: "ProgramProductAssociated");

            migrationBuilder.DropColumn(
                name: "community_only",
                table: "ProgramProductAssociated");
        }
    }
}
