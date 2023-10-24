using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class IndexTableFlowsheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_DETAIL_IDX02",
                table: "RAIIN_LIST_DETAIL",
                columns: new[] { "HP_ID", "IS_DELETED" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_DETAIL_IDX02",
                table: "RAIIN_LIST_DETAIL");
        }
    }
}
