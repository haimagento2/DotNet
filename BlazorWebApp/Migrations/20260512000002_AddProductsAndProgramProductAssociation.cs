using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorWebApp.Migrations
{
    public partial class AddProductsAndProgramProductAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SKU = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: false),
                    EnableSubscriptionLinking = table.Column<bool>(type: "INTEGER", nullable: false),
                    LicenseOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommunityOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    SendEmailInstruction = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramProductAssociated",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramProductAssociated", x => new { x.ProgramId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProgramProductAssociated_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramProductAssociated_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramProductAssociated_ProductId",
                table: "ProgramProductAssociated",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramProductAssociated");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
