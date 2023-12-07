using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class removeUniqueAdminId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT",
                column: "ADMIN_ID",
                unique: true,
                filter: "\"IS_DELETED\" = 0");
        }
    }
}
