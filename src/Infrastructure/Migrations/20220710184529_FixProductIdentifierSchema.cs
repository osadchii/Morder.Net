using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class FixProductIdentifierSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdentifiers_Marketplace_MarketplaceId",
                table: "ProductIdentifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdentifiers_Product_ProductId",
                table: "ProductIdentifiers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIdentifiers",
                table: "ProductIdentifiers");

            migrationBuilder.RenameTable(
                name: "ProductIdentifiers",
                newName: "ProductIdentifier",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIdentifiers_ProductId",
                schema: "dbo",
                table: "ProductIdentifier",
                newName: "IX_ProductIdentifier_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIdentifier",
                schema: "dbo",
                table: "ProductIdentifier",
                columns: new[] { "MarketplaceId", "ProductId", "Type" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdentifier_Marketplace_MarketplaceId",
                schema: "dbo",
                table: "ProductIdentifier",
                column: "MarketplaceId",
                principalSchema: "dbo",
                principalTable: "Marketplace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdentifier_Product_ProductId",
                schema: "dbo",
                table: "ProductIdentifier",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdentifier_Marketplace_MarketplaceId",
                schema: "dbo",
                table: "ProductIdentifier");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdentifier_Product_ProductId",
                schema: "dbo",
                table: "ProductIdentifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIdentifier",
                schema: "dbo",
                table: "ProductIdentifier");

            migrationBuilder.RenameTable(
                name: "ProductIdentifier",
                schema: "dbo",
                newName: "ProductIdentifiers");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIdentifier_ProductId",
                table: "ProductIdentifiers",
                newName: "IX_ProductIdentifiers_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIdentifiers",
                table: "ProductIdentifiers",
                columns: new[] { "MarketplaceId", "ProductId", "Type" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdentifiers_Marketplace_MarketplaceId",
                table: "ProductIdentifiers",
                column: "MarketplaceId",
                principalSchema: "dbo",
                principalTable: "Marketplace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdentifiers_Product_ProductId",
                table: "ProductIdentifiers",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
