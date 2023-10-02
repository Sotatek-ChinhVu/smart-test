using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndexRAIINLISTINFIDX01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_INF_IDX01",
                table: "RAIIN_LIST_INF",
                columns: new[] { "GRP_ID", "KBN_CD", "RAIIN_LIST_KBN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_INF_IDX01",
                table: "RAIIN_LIST_INF");
        }
    }
}
