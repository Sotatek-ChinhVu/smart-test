using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHashPasswordUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "HASH_PASSWORD",
                table: "USER_MST",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "SALT",
                table: "USER_MST",
                type: "bytea",
                maxLength: 14,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HASH_PASSWORD",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "SALT",
                table: "USER_MST");
        }
    }
}
