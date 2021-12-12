using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class BotUsersState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentState",
                schema: "dbo",
                table: "BotUser",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentStateKey",
                schema: "dbo",
                table: "BotUser",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                schema: "dbo",
                table: "BotUser");

            migrationBuilder.DropColumn(
                name: "CurrentStateKey",
                schema: "dbo",
                table: "BotUser");
        }
    }
}
