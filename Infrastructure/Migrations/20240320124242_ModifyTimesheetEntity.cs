using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTimesheetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Timesheets",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)20,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Timesheets",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)6,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)20);
        }
    }
}
