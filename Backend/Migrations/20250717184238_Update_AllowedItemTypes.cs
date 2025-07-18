using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniBill.Migrations
{
    /// <inheritdoc />
    public partial class Update_AllowedItemTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllowedItemTypes_BusinessTypes_ItemTypeId",
                table: "AllowedItemTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_AllowedItemTypes_ItemTypes_ItemTypeId1",
                table: "AllowedItemTypes");

            migrationBuilder.DropIndex(
                name: "IX_AllowedItemTypes_ItemTypeId1",
                table: "AllowedItemTypes");

            migrationBuilder.DropColumn(
                name: "ItemTypeId1",
                table: "AllowedItemTypes");

            migrationBuilder.AddForeignKey(
                name: "FK_AllowedItemTypes_BusinessTypes_BusinessTypeId",
                table: "AllowedItemTypes",
                column: "BusinessTypeId",
                principalTable: "BusinessTypes",
                principalColumn: "BusinessTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AllowedItemTypes_ItemTypes_ItemTypeId",
                table: "AllowedItemTypes",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "ItemTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllowedItemTypes_BusinessTypes_BusinessTypeId",
                table: "AllowedItemTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_AllowedItemTypes_ItemTypes_ItemTypeId",
                table: "AllowedItemTypes");

            migrationBuilder.AddColumn<int>(
                name: "ItemTypeId1",
                table: "AllowedItemTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AllowedItemTypes_ItemTypeId1",
                table: "AllowedItemTypes",
                column: "ItemTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AllowedItemTypes_BusinessTypes_ItemTypeId",
                table: "AllowedItemTypes",
                column: "ItemTypeId",
                principalTable: "BusinessTypes",
                principalColumn: "BusinessTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AllowedItemTypes_ItemTypes_ItemTypeId1",
                table: "AllowedItemTypes",
                column: "ItemTypeId1",
                principalTable: "ItemTypes",
                principalColumn: "ItemTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
