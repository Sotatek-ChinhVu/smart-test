using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "YAKKA_SYUSAI_MST_IDX01",
                table: "YAKKA_SYUSAI_MST",
                columns: new[] { "START_DATE", "END_DATE" });

            migrationBuilder.CreateIndex(
                name: "TEN_MST_IDX08",
                table: "TEN_MST",
                columns: new[] { "HP_ID", "ITEM_CD", "START_DATE", "END_DATE", "NAME", "KANA_NAME1", "KANA_NAME2", "KANA_NAME3", "KANA_NAME4", "KANA_NAME5", "KANA_NAME6", "KANA_NAME7", "IS_DELETED", "IS_ADOPTED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_INF_IDX03",
                table: "RAIIN_INF",
                columns: new[] { "IS_DELETED", "SIN_DATE", "PT_ID" });

            migrationBuilder.CreateIndex(
                name: "KENSA_MST_IDX01",
                table: "KENSA_MST",
                column: "KENSA_ITEM_CD");

            migrationBuilder.CreateIndex(
                name: "IPN_NAME_MST_IDX01",
                table: "IPN_NAME_MST",
                column: "IPN_NAME_CD");

            migrationBuilder.CreateIndex(
                name: "IPN_MIN_YAKKA_MST_IDX02",
                table: "IPN_MIN_YAKKA_MST",
                columns: new[] { "HP_ID", "START_DATE", "END_DATE", "IPN_NAME_CD" });

            migrationBuilder.CreateIndex(
                name: "IPN_KASAN_EXCLUDE_ITEM_IDX01",
                table: "IPN_KASAN_EXCLUDE_ITEM",
                columns: new[] { "HP_ID", "START_DATE", "END_DATE", "ITEM_CD" });

            migrationBuilder.CreateIndex(
                name: "IPN_KASAN_EXCLUDE_IDX01",
                table: "IPN_KASAN_EXCLUDE",
                columns: new[] { "HP_ID", "START_DATE", "END_DATE", "IPN_NAME_CD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "YAKKA_SYUSAI_MST_IDX01",
                table: "YAKKA_SYUSAI_MST");

            migrationBuilder.DropIndex(
                name: "TEN_MST_IDX08",
                table: "TEN_MST");

            migrationBuilder.DropIndex(
                name: "RAIIN_INF_IDX03",
                table: "RAIIN_INF");

            migrationBuilder.DropIndex(
                name: "KENSA_MST_IDX01",
                table: "KENSA_MST");

            migrationBuilder.DropIndex(
                name: "IPN_NAME_MST_IDX01",
                table: "IPN_NAME_MST");

            migrationBuilder.DropIndex(
                name: "IPN_MIN_YAKKA_MST_IDX02",
                table: "IPN_MIN_YAKKA_MST");

            migrationBuilder.DropIndex(
                name: "IPN_KASAN_EXCLUDE_ITEM_IDX01",
                table: "IPN_KASAN_EXCLUDE_ITEM");

            migrationBuilder.DropIndex(
                name: "IPN_KASAN_EXCLUDE_IDX01",
                table: "IPN_KASAN_EXCLUDE");
        }
    }
}
