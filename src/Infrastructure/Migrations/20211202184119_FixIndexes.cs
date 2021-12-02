using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class FixIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_ProductId_WarehouseId",
                schema: "dbo",
                table: "Stock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductId_WarehouseId",
                schema: "dbo",
                table: "Stock",
                columns: new[] { "ProductId", "WarehouseId" });
        }
    }
}
