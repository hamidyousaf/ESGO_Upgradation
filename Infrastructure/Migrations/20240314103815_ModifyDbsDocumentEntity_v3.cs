using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDbsDocumentEntity_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments",
                column: "DocumentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_DbsDocuments_DocumentTypeId",
                table: "DbsDocuments",
                column: "DocumentTypeId",
                unique: true);
        }
    }
}
