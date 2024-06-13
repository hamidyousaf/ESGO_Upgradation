using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommissionsInJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRateAfterLimitedCommission",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRateAfterPayrollCommission",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRateAfterSelfCommission",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LimitedCommission",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PayrollCommission",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SelfCommission",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRateAfterLimitedCommission",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "HourlyRateAfterPayrollCommission",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "HourlyRateAfterSelfCommission",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "LimitedCommission",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PayrollCommission",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SelfCommission",
                table: "Jobs");
        }
    }
}
