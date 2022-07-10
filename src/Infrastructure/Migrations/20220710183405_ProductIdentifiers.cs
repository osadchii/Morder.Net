using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ProductIdentifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId_ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting");

            migrationBuilder.CreateTable(
                name: "ProductIdentifiers",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    MarketplaceId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIdentifiers", x => new { x.MarketplaceId, x.ProductId, x.Type });
                    table.ForeignKey(
                        name: "FK_ProductIdentifiers_Marketplace_MarketplaceId",
                        column: x => x.MarketplaceId,
                        principalSchema: "dbo",
                        principalTable: "Marketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductIdentifiers_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                column: "MarketplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIdentifiers_ProductId",
                table: "ProductIdentifiers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductIdentifiers");

            migrationBuilder.DropIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId",
                schema: "dbo",
                table: "MarketplaceProductSetting");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId_ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                columns: new[] { "MarketplaceId", "ExternalId" });
        }
    }
}
