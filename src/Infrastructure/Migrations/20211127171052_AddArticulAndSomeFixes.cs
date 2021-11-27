using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddArticulAndSomeFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Product",
                newName: "DeletionMark");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Category",
                newName: "DeletionMark");

            migrationBuilder.AddColumn<bool>(
                name: "DeletionMark",
                schema: "dbo",
                table: "Warehouse",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Articul",
                schema: "dbo",
                table: "Product",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DeletionMark",
                schema: "dbo",
                table: "PriceType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Articul",
                schema: "dbo",
                table: "Product",
                column: "Articul",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_Articul",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeletionMark",
                schema: "dbo",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Articul",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeletionMark",
                schema: "dbo",
                table: "PriceType");

            migrationBuilder.RenameColumn(
                name: "DeletionMark",
                schema: "dbo",
                table: "Product",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "DeletionMark",
                schema: "dbo",
                table: "Category",
                newName: "IsDeleted");
        }
    }
}
