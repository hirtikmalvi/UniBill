using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UniBill.Migrations
{
    /// <inheritdoc />
    public partial class AddBillStatusAndPaymentMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Bills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Bills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentModeId",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentModes",
                columns: table => new
                {
                    PaymentModeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentModes", x => x.PaymentModeId);
                });

            migrationBuilder.InsertData(
                table: "BillStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Paid" },
                    { 3, "Partial" },
                    { 4, "Cancelled" },
                    { 5, "Refunded" }
                });

            migrationBuilder.InsertData(
                table: "PaymentModes",
                columns: new[] { "PaymentModeId", "ModeName" },
                values: new object[,]
                {
                    { 1, "CASH" },
                    { 2, "UPI" },
                    { 3, "CARD" },
                    { 4, "BANK_TRANSFER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_PaymentModeId",
                table: "Bills",
                column: "PaymentModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_StatusId",
                table: "Bills",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_BillStatuses_StatusId",
                table: "Bills",
                column: "StatusId",
                principalTable: "BillStatuses",
                principalColumn: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_PaymentModes_PaymentModeId",
                table: "Bills",
                column: "PaymentModeId",
                principalTable: "PaymentModes",
                principalColumn: "PaymentModeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_BillStatuses_StatusId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_PaymentModes_PaymentModeId",
                table: "Bills");

            migrationBuilder.DropTable(
                name: "BillStatuses");

            migrationBuilder.DropTable(
                name: "PaymentModes");

            migrationBuilder.DropIndex(
                name: "IX_Bills_PaymentModeId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_StatusId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PaymentModeId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Bills");
        }
    }
}
