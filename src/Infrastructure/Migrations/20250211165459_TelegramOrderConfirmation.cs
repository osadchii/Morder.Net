using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TelegramOrderConfirmation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmedByBotUserId",
                schema: "dbo",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmsOrders",
                schema: "dbo",
                table: "BotUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ConfirmedByBotUserId",
                schema: "dbo",
                table: "Order",
                column: "ConfirmedByBotUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_BotUser_ConfirmedByBotUserId",
                schema: "dbo",
                table: "Order",
                column: "ConfirmedByBotUserId",
                principalSchema: "dbo",
                principalTable: "BotUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_BotUser_ConfirmedByBotUserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ConfirmedByBotUserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ConfirmedByBotUserId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ConfirmsOrders",
                schema: "dbo",
                table: "BotUser");
        }
    }
}
