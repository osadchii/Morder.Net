using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class OrderModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId",
                schema: "dbo",
                table: "MarketplaceProductSetting");

            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                schema: "dbo",
                table: "OrderItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                schema: "dbo",
                table: "OrderItem",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                schema: "dbo",
                table: "Order",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MarketplaceId",
                schema: "dbo",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                schema: "dbo",
                table: "Order",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Order",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrderBox",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<decimal>(type: "numeric", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBox_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderBox_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderChanges",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderChanges", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderChanges_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ExternalId",
                schema: "dbo",
                table: "Order",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_MarketplaceId",
                schema: "dbo",
                table: "Order",
                column: "MarketplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MarketplaceId_Number",
                schema: "dbo",
                table: "Order",
                columns: new[] { "MarketplaceId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId_ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                columns: new[] { "MarketplaceId", "ExternalId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderBox_OrderId",
                schema: "dbo",
                table: "OrderBox",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBox_ProductId",
                schema: "dbo",
                table: "OrderBox",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Marketplace_MarketplaceId",
                schema: "dbo",
                table: "Order",
                column: "MarketplaceId",
                principalSchema: "dbo",
                principalTable: "Marketplace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Marketplace_MarketplaceId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropTable(
                name: "OrderBox",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderChanges");

            migrationBuilder.DropIndex(
                name: "IX_Order_ExternalId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_MarketplaceId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_MarketplaceId_Number",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId_ExternalId",
                schema: "dbo",
                table: "MarketplaceProductSetting");

            migrationBuilder.DropColumn(
                name: "Canceled",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Customer",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "MarketplaceId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "dbo",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceProductSetting_MarketplaceId",
                schema: "dbo",
                table: "MarketplaceProductSetting",
                column: "MarketplaceId");
        }
    }
}
