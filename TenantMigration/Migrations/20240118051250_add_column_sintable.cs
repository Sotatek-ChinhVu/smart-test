using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnsintable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ef_flg",
                table: "wrk_sin_rp_inf",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cd_edano",
                table: "wrk_sin_koui_detail_del",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "cd_kbn",
                table: "wrk_sin_koui_detail_del",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "cd_kbnno",
                table: "wrk_sin_koui_detail_del",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cd_kouno",
                table: "wrk_sin_koui_detail_del",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "kokuji1",
                table: "wrk_sin_koui_detail_del",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "kokuji2",
                table: "wrk_sin_koui_detail_del",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ef_flg",
                table: "wrk_sin_koui_detail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ipn_flg",
                table: "wrk_sin_koui_detail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ef_flg",
                table: "wrk_sin_koui",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ef_flg",
                table: "sin_rp_inf",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ef_flg",
                table: "sin_koui_detail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ef_ten",
                table: "sin_koui_detail",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ipn_flg",
                table: "sin_koui_detail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ef_flg",
                table: "sin_koui",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ef_ten",
                table: "sin_koui",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ef_ten_count",
                table: "sin_koui",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ef_total_ten",
                table: "sin_koui",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ef_flg",
                table: "wrk_sin_rp_inf");

            migrationBuilder.DropColumn(
                name: "cd_edano",
                table: "wrk_sin_koui_detail_del");

            migrationBuilder.DropColumn(
                name: "cd_kbn",
                table: "wrk_sin_koui_detail_del");

            migrationBuilder.DropColumn(
                name: "cd_kbnno",
                table: "wrk_sin_koui_detail_del");

            migrationBuilder.DropColumn(
                name: "cd_kouno",
                table: "wrk_sin_koui_detail_del");

            migrationBuilder.DropColumn(
                name: "kokuji1",
                table: "wrk_sin_koui_detail_del");

            migrationBuilder.DropColumn(
                name: "kokuji2",
                table: "wrk_sin_koui_detail_del");

            migrationBuilder.DropColumn(
                name: "ef_flg",
                table: "wrk_sin_koui_detail");

            migrationBuilder.DropColumn(
                name: "ipn_flg",
                table: "wrk_sin_koui_detail");

            migrationBuilder.DropColumn(
                name: "ef_flg",
                table: "wrk_sin_koui");

            migrationBuilder.DropColumn(
                name: "ef_flg",
                table: "sin_rp_inf");

            migrationBuilder.DropColumn(
                name: "ef_flg",
                table: "sin_koui_detail");

            migrationBuilder.DropColumn(
                name: "ef_ten",
                table: "sin_koui_detail");

            migrationBuilder.DropColumn(
                name: "ipn_flg",
                table: "sin_koui_detail");

            migrationBuilder.DropColumn(
                name: "ef_flg",
                table: "sin_koui");

            migrationBuilder.DropColumn(
                name: "ef_ten",
                table: "sin_koui");

            migrationBuilder.DropColumn(
                name: "ef_ten_count",
                table: "sin_koui");

            migrationBuilder.DropColumn(
                name: "ef_total_ten",
                table: "sin_koui");
        }
    }
}
