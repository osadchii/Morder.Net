using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddExternalid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Warehouse",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                schema: "dbo",
                table: "Warehouse",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                schema: "dbo",
                table: "Product",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "PriceType",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Category",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                schema: "dbo",
                table: "Category",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "dbo",
                table: "Category");
        }
    }
}
