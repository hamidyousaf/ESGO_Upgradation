using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAceessNIInEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AceessNIExpiryDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AceessNIRejectionReason",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "AceessNIStatus",
                table: "Employees",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "AceessNIUrl",
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
                name: "AceessNIExpiryDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AceessNIRejectionReason",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AceessNIStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AceessNIUrl",
                table: "Employees");
        }
    }
}
