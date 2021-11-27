using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SetUniqueExternalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ExternalId3",
                schema: "dbo",
                table: "Warehouse");

            migrationBuilder.DropIndex(
                name: "ExternalId2",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType");

            migrationBuilder.DropIndex(
                name: "ExternalId1",
                schema: "dbo",
                table: "Category");

            migrationBuilder.CreateIndex(
                name: "ExternalId3",
                schema: "dbo",
                table: "Warehouse",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ExternalId2",
                schema: "dbo",
                table: "Product",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ExternalId1",
                schema: "dbo",
                table: "Category",
                column: "ExternalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ExternalId3",
                schema: "dbo",
                table: "Warehouse");

            migrationBuilder.DropIndex(
                name: "ExternalId2",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType");

            migrationBuilder.DropIndex(
                name: "ExternalId1",
                schema: "dbo",
                table: "Category");

            migrationBuilder.CreateIndex(
                name: "ExternalId3",
                schema: "dbo",
                table: "Warehouse",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "ExternalId2",
                schema: "dbo",
                table: "Product",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "ExternalId1",
                schema: "dbo",
                table: "Category",
                column: "ExternalId");
        }
    }
}
