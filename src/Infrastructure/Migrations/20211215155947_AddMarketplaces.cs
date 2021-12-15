using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddMarketplaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceTypeId",
                schema: "dbo",
                table: "Company",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Marketplace",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ProductTypes = table.Column<string>(type: "text", nullable: false),
                    MinimalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Settings = table.Column<string>(type: "text", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    NullifyStocks = table.Column<bool>(type: "boolean", nullable: false),
                    PriceTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketplace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marketplace_PriceType_PriceTypeId",
                        column: x => x.PriceTypeId,
                        principalSchema: "dbo",
                        principalTable: "PriceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marketplace_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_PriceTypeId",
                schema: "dbo",
                table: "Company",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Marketplace_PriceTypeId",
                schema: "dbo",
                table: "Marketplace",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Marketplace_WarehouseId",
                schema: "dbo",
                table: "Marketplace",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_PriceType_PriceTypeId",
                schema: "dbo",
                table: "Company",
                column: "PriceTypeId",
                principalSchema: "dbo",
                principalTable: "PriceType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_PriceType_PriceTypeId",
                schema: "dbo",
                table: "Company");

            migrationBuilder.DropTable(
                name: "Marketplace",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Company_PriceTypeId",
                schema: "dbo",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                schema: "dbo",
                table: "Company");
        }
    }
}
