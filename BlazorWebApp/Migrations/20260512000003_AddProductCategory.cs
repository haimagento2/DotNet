using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWebApp.Migrations
{
    public partial class AddProductCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
