using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDbsDocumentEntity_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_EmployeeId",
                table: "DbsDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments",
                column: "DocumentTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_EmployeeId",
                table: "DbsDocuments",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_EmployeeId",
                table: "DbsDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_EmployeeId",
                table: "DbsDocuments",
                column: "EmployeeId",
                unique: true);
        }
    }
}
