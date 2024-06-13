using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyJob_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedReason",
                table: "Jobs",
                newName: "CancellationReason");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Jobs",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "CancellationReason",
                table: "Jobs",
                newName: "DeletedReason");
        }
    }
}
