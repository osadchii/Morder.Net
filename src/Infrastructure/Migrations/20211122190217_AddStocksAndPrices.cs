using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddStocksAndPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Price",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    PriceTypeId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => new { x.ProductId, x.PriceTypeId });
                    table.ForeignKey(
                        name: "FK_Price_PriceType_PriceTypeId",
                        column: x => x.PriceTypeId,
                        principalSchema: "dbo",
                        principalTable: "PriceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Price_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => new { x.ProductId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_Stock_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stock_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Price_PriceTypeId",
                schema: "dbo",
                table: "Price",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Price_ProductId_PriceTypeId",
                schema: "dbo",
                table: "Price",
                columns: new[] { "ProductId", "PriceTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductId_WarehouseId",
                schema: "dbo",
                table: "Stock",
                columns: new[] { "ProductId", "WarehouseId" });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_WarehouseId",
                schema: "dbo",
                table: "Stock",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Price",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PriceType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Warehouse",
                schema: "dbo");
        }
    }
}
