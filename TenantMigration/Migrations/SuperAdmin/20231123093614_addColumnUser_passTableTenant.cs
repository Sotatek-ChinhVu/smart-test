using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class addColumnUserpassTableTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PASSWORD_CONNECT",
                table: "TENANT",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "USER_CONNECT",
                table: "TENANT",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PASSWORD_CONNECT",
                table: "TENANT");

            migrationBuilder.DropColumn(
                name: "USER_CONNECT",
                table: "TENANT");
        }
    }
}
