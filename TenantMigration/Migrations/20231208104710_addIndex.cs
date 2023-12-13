using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ODR_INF_RAIIN_NO_IDX",
                table: "ODR_INF",
                columns: new[] { "RAIIN_NO", "ODR_KOUI_KBN", "INOUT_KBN", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "M34_PRECAUTION_CODE_AGE_MIN_IDX",
                table: "M34_PRECAUTION_CODE",
                columns: new[] { "AGE_MIN", "AGE_MAX", "SEX_CD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ODR_INF_RAIIN_NO_IDX",
                table: "ODR_INF");

            migrationBuilder.DropIndex(
                name: "M34_PRECAUTION_CODE_AGE_MIN_IDX",
                table: "M34_PRECAUTION_CODE");
        }
    }
}
