using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class IndexTableFlowsheet2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF");

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID", "RAIIN_NO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF");

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID" });
        }
    }
}
