using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddStockTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockChange",
                schema: "dbo",
                columns: table => new
                {
                    MarketplaceId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockChange", x => new { x.MarketplaceId, x.WarehouseId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_StockChange_Marketplace_MarketplaceId",
                        column: x => x.MarketplaceId,
                        principalSchema: "dbo",
                        principalTable: "Marketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockChange_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockChange_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockChange_MarketplaceId",
                schema: "dbo",
                table: "StockChange",
                column: "MarketplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockChange_ProductId",
                schema: "dbo",
                table: "StockChange",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockChange_WarehouseId",
                schema: "dbo",
                table: "StockChange",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockChange",
                schema: "dbo");
        }
    }
}
