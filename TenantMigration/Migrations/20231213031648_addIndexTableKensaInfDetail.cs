using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addIndexTableKensaInfDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "KENSA_INF_DETAIL_PT_ID_IDX",
                table: "KENSA_INF_DETAIL",
                columns: new[] { "PT_ID", "IS_DELETED", "KENSA_ITEM_CD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "KENSA_INF_DETAIL_PT_ID_IDX",
                table: "KENSA_INF_DETAIL");
        }
    }
}
