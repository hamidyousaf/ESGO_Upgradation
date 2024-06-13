using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAceessNIInEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AceessNIUrl",
                table: "Employees",
                newName: "AccessNIUrl");

            migrationBuilder.RenameColumn(
                name: "AceessNIStatus",
                table: "Employees",
                newName: "AccessNIStatus");

            migrationBuilder.RenameColumn(
                name: "AceessNIRejectionReason",
                table: "Employees",
                newName: "AccessNIRejectionReason");

            migrationBuilder.RenameColumn(
                name: "AceessNIExpiryDate",
                table: "Employees",
                newName: "AccessNIExpiryDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessNIUrl",
                table: "Employees",
                newName: "AceessNIUrl");

            migrationBuilder.RenameColumn(
                name: "AccessNIStatus",
                table: "Employees",
                newName: "AceessNIStatus");

            migrationBuilder.RenameColumn(
                name: "AccessNIRejectionReason",
                table: "Employees",
                newName: "AceessNIRejectionReason");

            migrationBuilder.RenameColumn(
                name: "AccessNIExpiryDate",
                table: "Employees",
                newName: "AceessNIExpiryDate");
        }
    }
}
