using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ExternalId3",
                schema: "dbo",
                table: "Warehouse",
                newName: "UNIQUE_INDEX_ExternalId4");

            migrationBuilder.RenameIndex(
                name: "ExternalId2",
                schema: "dbo",
                table: "Product",
                newName: "UNIQUE_INDEX_ExternalId3");

            migrationBuilder.RenameIndex(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType",
                newName: "UNIQUE_INDEX_ExternalId1");

            migrationBuilder.RenameIndex(
                name: "ExternalId1",
                schema: "dbo",
                table: "Category",
                newName: "UNIQUE_INDEX_ExternalId2");

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Count = table.Column<decimal>(type: "numeric", nullable: false),
                    Sum = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UNIQUE_INDEX_ExternalId",
                schema: "dbo",
                table: "Order",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "dbo");

            migrationBuilder.RenameIndex(
                name: "UNIQUE_INDEX_ExternalId4",
                schema: "dbo",
                table: "Warehouse",
                newName: "ExternalId3");

            migrationBuilder.RenameIndex(
                name: "UNIQUE_INDEX_ExternalId3",
                schema: "dbo",
                table: "Product",
                newName: "ExternalId2");

            migrationBuilder.RenameIndex(
                name: "UNIQUE_INDEX_ExternalId1",
                schema: "dbo",
                table: "PriceType",
                newName: "ExternalId");

            migrationBuilder.RenameIndex(
                name: "UNIQUE_INDEX_ExternalId2",
                schema: "dbo",
                table: "Category",
                newName: "ExternalId1");
        }
    }
}
