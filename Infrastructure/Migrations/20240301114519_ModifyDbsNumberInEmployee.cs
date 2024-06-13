using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDbsNumberInEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DbsNumebrStatus",
                table: "Employees",
                newName: "DbsNumberStatus");

            migrationBuilder.RenameColumn(
                name: "DbsNumebrRejectionReason",
                table: "Employees",
                newName: "DbsNumberRejectionReason");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "JobBookings",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldMaxLength: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DbsNumberStatus",
                table: "Employees",
                newName: "DbsNumebrStatus");

            migrationBuilder.RenameColumn(
                name: "DbsNumberRejectionReason",
                table: "Employees",
                newName: "DbsNumebrRejectionReason");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "JobBookings",
                type: "tinyint",
                maxLength: 0,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);
        }
    }
}
