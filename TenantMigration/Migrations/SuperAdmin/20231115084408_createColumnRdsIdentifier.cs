using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class createColumnRdsIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RDS_IDENTIFIER",
                table: "TENANT",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RDS_IDENTIFIER",
                table: "TENANT");
        }
    }
}
