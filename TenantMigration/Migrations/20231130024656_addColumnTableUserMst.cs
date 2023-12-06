using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addColumnTableUserMst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HPKI_ISSUER_DN",
                table: "USER_MST",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HPKI_SN",
                table: "USER_MST",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LOGIN_TYPE",
                table: "USER_MST",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HPKI_ISSUER_DN",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "HPKI_SN",
                table: "USER_MST");

            migrationBuilder.DropColumn(
                name: "LOGIN_TYPE",
                table: "USER_MST");
        }
    }
}
