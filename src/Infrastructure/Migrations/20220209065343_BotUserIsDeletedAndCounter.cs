using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class BotUserIsDeletedAndCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "BotUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BotUserUsageCounter",
                schema: "dbo",
                columns: table => new
                {
                    BotUserId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    LastUse = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUserUsageCounter", x => x.BotUserId);
                    table.ForeignKey(
                        name: "FK_BotUserUsageCounter_BotUser_BotUserId",
                        column: x => x.BotUserId,
                        principalSchema: "dbo",
                        principalTable: "BotUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotUserUsageCounter",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "BotUser");
        }
    }
}
