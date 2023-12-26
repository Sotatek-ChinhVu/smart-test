using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addIndexToTableKaikeiInf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "KAIKEI_INF_IDX01",
                table: "KAIKEI_INF",
                columns: new[] { "HP_ID", "RAIIN_NO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "KAIKEI_INF_IDX01",
                table: "KAIKEI_INF");
        }
    }
}
