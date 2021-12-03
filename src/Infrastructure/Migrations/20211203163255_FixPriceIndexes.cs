using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class FixPriceIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Price_ProductId_PriceTypeId",
                schema: "dbo",
                table: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Price_ProductId_PriceTypeId",
                schema: "dbo",
                table: "Price",
                columns: new[] { "ProductId", "PriceTypeId" });
        }
    }
}
