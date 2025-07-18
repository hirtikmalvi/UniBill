using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniBill.Migrations
{
    /// <inheritdoc />
    public partial class Update_Business_And_BusinessAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "BusinessAddresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "BusinessAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
