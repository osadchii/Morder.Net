using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NullableMarketplacePriceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marketplace_PriceType_PriceTypeId",
                schema: "dbo",
                table: "Marketplace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.AlterColumn<int>(
                name: "PriceTypeId",
                schema: "dbo",
                table: "Marketplace",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                schema: "dbo",
                table: "OrderItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "dbo",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marketplace_PriceType_PriceTypeId",
                schema: "dbo",
                table: "Marketplace",
                column: "PriceTypeId",
                principalSchema: "dbo",
                principalTable: "PriceType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marketplace_PriceType_PriceTypeId",
                schema: "dbo",
                table: "Marketplace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_OrderId",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.AlterColumn<int>(
                name: "PriceTypeId",
                schema: "dbo",
                table: "Marketplace",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                schema: "dbo",
                table: "OrderItem",
                columns: new[] { "OrderId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Marketplace_PriceType_PriceTypeId",
                schema: "dbo",
                table: "Marketplace",
                column: "PriceTypeId",
                principalSchema: "dbo",
                principalTable: "PriceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
