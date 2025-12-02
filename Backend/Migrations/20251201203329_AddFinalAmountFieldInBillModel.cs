using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniBill.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalAmountFieldInBillModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Bills");
        }
    }
}
