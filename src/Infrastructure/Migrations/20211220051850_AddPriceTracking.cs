using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddPriceTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PriceChangesTracking",
                schema: "dbo",
                table: "Marketplace",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StockChangesTracking",
                schema: "dbo",
                table: "Marketplace",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PriceChange",
                schema: "dbo",
                columns: table => new
                {
                    MarketplaceId = table.Column<int>(type: "integer", nullable: false),
                    PriceTypeId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceChange", x => new { x.MarketplaceId, x.PriceTypeId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_PriceChange_Marketplace_MarketplaceId",
                        column: x => x.MarketplaceId,
                        principalSchema: "dbo",
                        principalTable: "Marketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceChange_PriceType_PriceTypeId",
                        column: x => x.PriceTypeId,
                        principalSchema: "dbo",
                        principalTable: "PriceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceChange_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceChange_MarketplaceId",
                schema: "dbo",
                table: "PriceChange",
                column: "MarketplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChange_PriceTypeId",
                schema: "dbo",
                table: "PriceChange",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChange_ProductId",
                schema: "dbo",
                table: "PriceChange",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceChange",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "PriceChangesTracking",
                schema: "dbo",
                table: "Marketplace");

            migrationBuilder.DropColumn(
                name: "StockChangesTracking",
                schema: "dbo",
                table: "Marketplace");
        }
    }
}
