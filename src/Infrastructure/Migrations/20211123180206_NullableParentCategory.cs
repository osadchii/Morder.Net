using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NullableParentCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentId",
                schema: "dbo",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                schema: "dbo",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                schema: "dbo",
                table: "Product",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "dbo",
                table: "Category",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentId",
                schema: "dbo",
                table: "Category",
                column: "ParentId",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                schema: "dbo",
                table: "Product",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentId",
                schema: "dbo",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                schema: "dbo",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                schema: "dbo",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "dbo",
                table: "Category",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentId",
                schema: "dbo",
                table: "Category",
                column: "ParentId",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                schema: "dbo",
                table: "Product",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
