using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostgreDataContext.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_YAKKA_SYUSAI_MST",
                table: "YAKKA_SYUSAI_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TEN_MST",
                table: "TEN_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SET_MST",
                table: "SET_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SET_KBN_MST",
                table: "SET_KBN_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SET_GENERATION_MST",
                table: "SET_GENERATION_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_TAG",
                table: "RAIIN_LIST_TAG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_MST",
                table: "RAIIN_LIST_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_DETAIL",
                table: "RAIIN_LIST_DETAIL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_CMT",
                table: "RAIIN_LIST_CMT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KENSA_MST",
                table: "KENSA_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KARTE_KBN_MST",
                table: "KARTE_KBN_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KARTE_FILTER_MST",
                table: "KARTE_FILTER_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KARTE_FILTER_DETAIL",
                table: "KARTE_FILTER_DETAIL");

            migrationBuilder.AlterColumn<string>(
                name: "UNIT_NAME",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "KEIKA",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "KBN",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "BIKO",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "YJ_CD",
                table: "TEN_MST",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "YAKKA_CD",
                table: "TEN_MST",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "TYU_SEQ",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "TYU_CD",
                table: "TEN_MST",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "TEN_KBN_NO",
                table: "TEN_MST",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SYUKEI_SAKI",
                table: "TEN_MST",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "SYOHIN_KANREN",
                table: "TEN_MST",
                type: "character varying(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "SANTEI_ITEM_CD",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "RYOSYU_NAME",
                table: "TEN_MST",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(240)",
                oldMaxLength: 240);

            migrationBuilder.AlterColumn<string>(
                name: "RENKEI_CD2",
                table: "TEN_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "RENKEI_CD1",
                table: "TEN_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "RECE_UNIT_NAME",
                table: "TEN_MST",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "RECE_UNIT_CD",
                table: "TEN_MST",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "ODR_UNIT_NAME",
                table: "TEN_MST",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "MIN_AGE",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "MAX_AGE",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "KOKUJI_KBN",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "KOKUJI2",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "KOKUJI1",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_ITEM_CD",
                table: "TEN_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME7",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME6",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME5",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME4",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME3",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME2",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME1",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "IPN_NAME_CD",
                table: "TEN_MST",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "CNV_UNIT_NAME",
                table: "TEN_MST",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "CD_KBN",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN4",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN3",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN2",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN1",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX4",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX3",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX2",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX1",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD4",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD3",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD2",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD1",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "TEN_MST",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SET_NAME",
                table: "SET_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "SET_MST",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SET_KBN_NAME",
                table: "SET_KBN_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "SET_KBN_MST",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "RAIIN_LIST_TAG",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "COLOR_CD",
                table: "RAIIN_LIST_DETAIL",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "TEXT",
                table: "RAIIN_LIST_CMT",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "UNIT",
                table: "KENSA_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "OYA_ITEM_CD",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "MALE_STD_LOW",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "MALE_STD_HIGH",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "MALE_STD",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_KANA",
                table: "KENSA_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "FORMULA",
                table: "KENSA_MST",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FEMALE_STD_LOW",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "FEMALE_STD_HIGH",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "FEMALE_STD",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "CENTER_ITEM_CD2",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CENTER_ITEM_CD1",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CENTER_CD",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "UPDATE_MACHINE",
                table: "KARTE_KBN_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CREATE_MACHINE",
                table: "KARTE_KBN_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PARAM",
                table: "KARTE_FILTER_DETAIL",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AddPrimaryKey(
                name: "PK_YAKKA_SYUSAI_MST",
                table: "YAKKA_SYUSAI_MST",
                columns: new[] { "HP_ID", "YAKKA_CD", "ITEM_CD", "START_DATE" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TEN_MST",
                table: "TEN_MST",
                columns: new[] { "HP_ID", "ITEM_CD", "START_DATE" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SET_MST",
                table: "SET_MST",
                columns: new[] { "HP_ID", "SET_CD" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SET_KBN_MST",
                table: "SET_KBN_MST",
                columns: new[] { "HP_ID", "SET_KBN", "SET_KBN_EDA_NO", "GENERATION_ID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SET_GENERATION_MST",
                table: "SET_GENERATION_MST",
                columns: new[] { "HP_ID", "GENERATION_ID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_TAG",
                table: "RAIIN_LIST_TAG",
                columns: new[] { "HP_ID", "RAIIN_NO", "SEQ_NO" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_MST",
                table: "RAIIN_LIST_MST",
                columns: new[] { "HP_ID", "GRP_ID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "RAIIN_NO", "GRP_ID", "RAIIN_LIST_KBN" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_DETAIL",
                table: "RAIIN_LIST_DETAIL",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_CMT",
                table: "RAIIN_LIST_CMT",
                columns: new[] { "HP_ID", "RAIIN_NO", "CMT_KBN" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_KENSA_MST",
                table: "KENSA_MST",
                columns: new[] { "HP_ID", "KENSA_ITEM_CD", "KENSA_ITEM_SEQ_NO" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_KARTE_KBN_MST",
                table: "KARTE_KBN_MST",
                columns: new[] { "HP_ID", "KARTE_KBN" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_KARTE_FILTER_MST",
                table: "KARTE_FILTER_MST",
                columns: new[] { "HP_ID", "USER_ID", "FILTER_ID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_KARTE_FILTER_DETAIL",
                table: "KARTE_FILTER_DETAIL",
                columns: new[] { "HP_ID", "USER_ID", "FILTER_ID", "FILTER_ITEM_CD", "FILTER_EDA_NO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_YAKKA_SYUSAI_MST",
                table: "YAKKA_SYUSAI_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TEN_MST",
                table: "TEN_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SET_MST",
                table: "SET_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SET_KBN_MST",
                table: "SET_KBN_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SET_GENERATION_MST",
                table: "SET_GENERATION_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_TAG",
                table: "RAIIN_LIST_TAG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_MST",
                table: "RAIIN_LIST_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_DETAIL",
                table: "RAIIN_LIST_DETAIL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_CMT",
                table: "RAIIN_LIST_CMT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KENSA_MST",
                table: "KENSA_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KARTE_KBN_MST",
                table: "KARTE_KBN_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KARTE_FILTER_MST",
                table: "KARTE_FILTER_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KARTE_FILTER_DETAIL",
                table: "KARTE_FILTER_DETAIL");

            migrationBuilder.AlterColumn<string>(
                name: "UNIT_NAME",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KEIKA",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KBN",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BIKO",
                table: "YAKKA_SYUSAI_MST",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YJ_CD",
                table: "TEN_MST",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YAKKA_CD",
                table: "TEN_MST",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TYU_SEQ",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TYU_CD",
                table: "TEN_MST",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TEN_KBN_NO",
                table: "TEN_MST",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SYUKEI_SAKI",
                table: "TEN_MST",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SYOHIN_KANREN",
                table: "TEN_MST",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SANTEI_ITEM_CD",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RYOSYU_NAME",
                table: "TEN_MST",
                type: "character varying(240)",
                maxLength: 240,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(240)",
                oldMaxLength: 240,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RENKEI_CD2",
                table: "TEN_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RENKEI_CD1",
                table: "TEN_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RECE_UNIT_NAME",
                table: "TEN_MST",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RECE_UNIT_CD",
                table: "TEN_MST",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ODR_UNIT_NAME",
                table: "TEN_MST",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MIN_AGE",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MAX_AGE",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KOKUJI_KBN",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KOKUJI2",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KOKUJI1",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_ITEM_CD",
                table: "TEN_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME7",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME6",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME5",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME4",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME3",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME2",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KANA_NAME1",
                table: "TEN_MST",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IPN_NAME_CD",
                table: "TEN_MST",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CNV_UNIT_NAME",
                table: "TEN_MST",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CD_KBN",
                table: "TEN_MST",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN4",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN3",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN2",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MIN1",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX4",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX3",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX2",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_MAX1",
                table: "TEN_MST",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD4",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD3",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD2",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AGEKASAN_CD1",
                table: "TEN_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "TEN_MST",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SET_NAME",
                table: "SET_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "SET_MST",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SET_KBN_NAME",
                table: "SET_KBN_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "SET_KBN_MST",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "RAIIN_LIST_TAG",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "COLOR_CD",
                table: "RAIIN_LIST_DETAIL",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TEXT",
                table: "RAIIN_LIST_CMT",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UNIT",
                table: "KENSA_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OYA_ITEM_CD",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MALE_STD_LOW",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MALE_STD_HIGH",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MALE_STD",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_KANA",
                table: "KENSA_MST",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FORMULA",
                table: "KENSA_MST",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FEMALE_STD_LOW",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FEMALE_STD_HIGH",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FEMALE_STD",
                table: "KENSA_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CENTER_ITEM_CD2",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CENTER_ITEM_CD1",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CENTER_CD",
                table: "KENSA_MST",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UPDATE_MACHINE",
                table: "KARTE_KBN_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "CREATE_MACHINE",
                table: "KARTE_KBN_MST",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "PARAM",
                table: "KARTE_FILTER_DETAIL",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_YAKKA_SYUSAI_MST",
                table: "YAKKA_SYUSAI_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TEN_MST",
                table: "TEN_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SET_MST",
                table: "SET_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SET_KBN_MST",
                table: "SET_KBN_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SET_GENERATION_MST",
                table: "SET_GENERATION_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_TAG",
                table: "RAIIN_LIST_TAG",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_MST",
                table: "RAIIN_LIST_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_DETAIL",
                table: "RAIIN_LIST_DETAIL",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_CMT",
                table: "RAIIN_LIST_CMT",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KENSA_MST",
                table: "KENSA_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KARTE_KBN_MST",
                table: "KARTE_KBN_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KARTE_FILTER_MST",
                table: "KARTE_FILTER_MST",
                column: "HP_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KARTE_FILTER_DETAIL",
                table: "KARTE_FILTER_DETAIL",
                column: "HP_ID");
        }
    }
}
