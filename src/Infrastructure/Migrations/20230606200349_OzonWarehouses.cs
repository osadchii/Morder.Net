using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class OzonWarehouses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OzonWarehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OzonId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OzonWarehouseId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OzonWarehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OzonWarehouses_Marketplace_OzonId",
                        column: x => x.OzonId,
                        principalSchema: "dbo",
                        principalTable: "Marketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OzonWarehouseBlacklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OzonWarehouseId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OzonWarehouseBlacklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OzonWarehouseBlacklists_OzonWarehouses_OzonWarehouseId",
                        column: x => x.OzonWarehouseId,
                        principalTable: "OzonWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OzonWarehouseBlacklists_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OzonWarehouseBlacklists_OzonWarehouseId_ProductId",
                table: "OzonWarehouseBlacklists",
                columns: new[] { "OzonWarehouseId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OzonWarehouseBlacklists_ProductId",
                table: "OzonWarehouseBlacklists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OzonWarehouses_OzonId_OzonWarehouseId",
                table: "OzonWarehouses",
                columns: new[] { "OzonId", "OzonWarehouseId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OzonWarehouseBlacklists");

            migrationBuilder.DropTable(
                name: "OzonWarehouses");
        }
    }
}
