using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniBill.Migrations
{
    /// <inheritdoc />
    public partial class Add_ShortUnitName_Prop_in_Unit_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UnitName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShortUnitName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortUnitName",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "UnitName",
                table: "Units",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
