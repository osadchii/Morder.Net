using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddProductFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Articul",
                schema: "dbo",
                table: "Product",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                schema: "dbo",
                table: "Product",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                schema: "dbo",
                table: "Product",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                schema: "dbo",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                schema: "dbo",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vat",
                schema: "dbo",
                table: "Product",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                schema: "dbo",
                table: "Product",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                schema: "dbo",
                table: "Product",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                schema: "dbo",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                schema: "dbo",
                table: "Product",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Brand",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Height",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Length",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Vat",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Vendor",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "VendorCode",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Width",
                schema: "dbo",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "Articul",
                schema: "dbo",
                table: "Product",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
