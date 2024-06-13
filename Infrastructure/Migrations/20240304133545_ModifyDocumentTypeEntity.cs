using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDocumentTypeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentOneTypeId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentOneUrl",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DocumentThreeTypeId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentThreeUrl",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DocumentTwoTypeId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentTwoUrl",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentOneTypeId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DocumentOneUrl",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DocumentThreeTypeId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DocumentThreeUrl",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DocumentTwoTypeId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DocumentTwoUrl",
                table: "Employees");
        }
    }
}
