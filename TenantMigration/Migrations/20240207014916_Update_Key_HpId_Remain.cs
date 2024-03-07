using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKeyHpIdRemain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_ingredients",
                table: "m38_ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kacode_yousiki_mst",
                table: "kacode_yousiki_mst");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_ingredients",
                table: "m38_ingredients",
                columns: new[] { "hp_id", "seibun_cd", "serial_num", "sbt" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kacode_yousiki_mst",
                table: "kacode_yousiki_mst",
                columns: new[] { "hp_id", "yousiki_ka_cd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_m38_ingredients",
                table: "m38_ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kacode_yousiki_mst",
                table: "kacode_yousiki_mst");

            migrationBuilder.AddPrimaryKey(
                name: "PK_m38_ingredients",
                table: "m38_ingredients",
                columns: new[] { "seibun_cd", "serial_num", "sbt" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_kacode_yousiki_mst",
                table: "kacode_yousiki_mst",
                column: "yousiki_ka_cd");
        }
    }
}
