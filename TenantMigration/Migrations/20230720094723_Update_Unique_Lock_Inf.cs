using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniqueLockInf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LOCK_INF_HP_ID_PT_ID_USER_ID",
                table: "LOCK_INF");

            migrationBuilder.CreateIndex(
                name: "IX_LOCK_INF_HP_ID_PT_ID_USER_ID",
                table: "LOCK_INF",
                columns: new[] { "HP_ID", "PT_ID", "USER_ID" },
                unique: true,
                filter: "\"FUNCTION_CD\" IN ('02000000', '03000000')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LOCK_INF_HP_ID_PT_ID_USER_ID",
                table: "LOCK_INF");

            migrationBuilder.CreateIndex(
                name: "IX_LOCK_INF_HP_ID_PT_ID_USER_ID",
                table: "LOCK_INF",
                columns: new[] { "HP_ID", "PT_ID", "USER_ID" },
                unique: true,
                filter: $"\"FUNCTION_CD\" = '02000000'");
        }
    }
}
