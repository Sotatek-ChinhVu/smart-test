using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKeyFillingInf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FILING_INF",
                table: "FILING_INF");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FILING_INF",
                table: "FILING_INF",
                columns: new[] { "HP_ID", "FILE_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FILING_INF",
                table: "FILING_INF");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FILING_INF",
                table: "FILING_INF",
                columns: new[] { "HP_ID", "FILE_ID", "PT_ID", "GET_DATE", "FILE_NO" });
        }
    }
}
