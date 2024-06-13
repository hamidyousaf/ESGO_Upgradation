using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobMinutesPerDay",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "EmployerCategoryId",
                table: "Jobs",
                newName: "EmployeeCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeCategoryId",
                table: "Jobs",
                newName: "EmployerCategoryId");

            migrationBuilder.AddColumn<decimal>(
                name: "JobMinutesPerDay",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
