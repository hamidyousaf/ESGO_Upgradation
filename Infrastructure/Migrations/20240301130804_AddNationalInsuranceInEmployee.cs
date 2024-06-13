using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalInsuranceInEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalInsuranceRejectionReason",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "NationalInsuranceStatus",
                table: "Employees",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "NationalInsuranceUrl",
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
                name: "NationalInsuranceRejectionReason",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalInsuranceStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalInsuranceUrl",
                table: "Employees");
        }
    }
}
