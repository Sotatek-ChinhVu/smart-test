using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addTableSinrekiFilterMstKoui : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IS_EXCLUDE",
                table: "SINREKI_FILTER_MST_DETAIL",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_ITEM_CD",
                table: "KENSA_SET_DETAIL",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "SET_NAME",
                table: "KENSA_SET",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateTable(
                name: "SINREKI_FILTER_MST_KOUI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KOUIKBNID = table.Column<int>(name: "KOUI_KBN_ID", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINREKI_FILTER_MST_KOUI", x => new { x.HPID, x.GRPCD, x.SEQNO });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SINREKI_FILTER_MST_KOUI");

            migrationBuilder.DropColumn(
                name: "IS_EXCLUDE",
                table: "SINREKI_FILTER_MST_DETAIL");

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_ITEM_CD",
                table: "KENSA_SET_DETAIL",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SET_NAME",
                table: "KENSA_SET",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
