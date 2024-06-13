using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeIdInDbsDocumentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "DbsDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_EmployeeId",
                table: "DbsDocuments",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DbsDocuments_Employees_EmployeeId",
                table: "DbsDocuments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbsDocuments_Employees_EmployeeId",
                table: "DbsDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_EmployeeId",
                table: "DbsDocuments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "DbsDocuments");
        }
    }
}
