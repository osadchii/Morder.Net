using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddMarketplaceCategoryProductSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketplaceCategorySetting",
                schema: "dbo",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    MarketplaceId = table.Column<int>(type: "integer", nullable: false),
                    Blocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceCategorySetting", x => new { x.CategoryId, x.MarketplaceId });
                    table.ForeignKey(
                        name: "FK_MarketplaceCategorySetting_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarketplaceCategorySetting_Marketplace_MarketplaceId",
                        column: x => x.MarketplaceId,
                        principalSchema: "dbo",
                        principalTable: "Marketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceProductSetting",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    MarketplaceId = table.Column<int>(type: "integer", nullable: false),
                    NullifyStock = table.Column<bool>(type: "boolean", nullable: false),
                    IgnoreRestrictions = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceProductSetting", x => new { x.ProductId, x.MarketplaceId });
                    table.ForeignKey(
                        name: "FK_MarketplaceProductSetting_Marketplace_MarketplaceId",
                        column: x => x.MarketplaceId,
                        principalSchema: "dbo",
                        principalTable: "Marketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarketplaceProductSetting_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceCategorySetting_MarketplaceId",
                schema: "dbo",
                table: "MarketplaceCategorySetting",
                column: "MarketplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                column: "MarketplaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketplaceCategorySetting",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MarketplaceProductSetting",
                schema: "dbo");
        }
    }
}
