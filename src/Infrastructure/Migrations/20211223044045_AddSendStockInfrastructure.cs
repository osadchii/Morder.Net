using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddSendStockInfrastructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceChange_PriceType_PriceTypeId",
                schema: "dbo",
                table: "PriceChange");

            migrationBuilder.DropForeignKey(
                name: "FK_StockChange_Warehouse_WarehouseId",
                schema: "dbo",
                table: "StockChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockChange",
                schema: "dbo",
                table: "StockChange");

            migrationBuilder.DropIndex(
                name: "IX_StockChange_WarehouseId",
                schema: "dbo",
                table: "StockChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceChange",
                schema: "dbo",
                table: "PriceChange");

            migrationBuilder.DropIndex(
                name: "IX_PriceChange_PriceTypeId",
                schema: "dbo",
                table: "PriceChange");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                schema: "dbo",
                table: "StockChange");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                schema: "dbo",
                table: "PriceChange");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceSendLimit",
                schema: "dbo",
                table: "Marketplace",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockSendLimit",
                schema: "dbo",
                table: "Marketplace",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockChange",
                schema: "dbo",
                table: "StockChange",
                columns: new[] { "MarketplaceId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceChange",
                schema: "dbo",
                table: "PriceChange",
                columns: new[] { "MarketplaceId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StockChange",
                schema: "dbo",
                table: "StockChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceChange",
                schema: "dbo",
                table: "PriceChange");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting");

            migrationBuilder.DropColumn(
                name: "PriceSendLimit",
                schema: "dbo",
                table: "Marketplace");

            migrationBuilder.DropColumn(
                name: "StockSendLimit",
                schema: "dbo",
                table: "Marketplace");

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                schema: "dbo",
                table: "StockChange",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceTypeId",
                schema: "dbo",
                table: "PriceChange",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockChange",
                schema: "dbo",
                table: "StockChange",
                columns: new[] { "MarketplaceId", "WarehouseId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceChange",
                schema: "dbo",
                table: "PriceChange",
                columns: new[] { "MarketplaceId", "PriceTypeId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_StockChange_WarehouseId",
                schema: "dbo",
                table: "StockChange",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceChange_PriceTypeId",
                schema: "dbo",
                table: "PriceChange",
                column: "PriceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceChange_PriceType_PriceTypeId",
                schema: "dbo",
                table: "PriceChange",
                column: "PriceTypeId",
                principalSchema: "dbo",
                principalTable: "PriceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockChange_Warehouse_WarehouseId",
                schema: "dbo",
                table: "StockChange",
                column: "WarehouseId",
                principalSchema: "dbo",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
