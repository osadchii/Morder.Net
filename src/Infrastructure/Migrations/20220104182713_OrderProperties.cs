using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class OrderProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedTimeLimit",
                schema: "dbo",
                table: "Order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerAddress",
                schema: "dbo",
                table: "Order",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PackingTimeLimit",
                schema: "dbo",
                table: "Order",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingTimeLimit",
                schema: "dbo",
                table: "Order",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedTimeLimit",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerAddress",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PackingTimeLimit",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingTimeLimit",
                schema: "dbo",
                table: "Order");
        }
    }
}
