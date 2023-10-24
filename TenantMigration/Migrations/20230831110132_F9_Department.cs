using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class F9Department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YOUSIKI_KA_CD",
                table: "KA_MST",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "KACODE_RECE_YOUSIKI",
                columns: table => new
                {
                    RECEKACD = table.Column<string>(name: "RECE_KA_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    YOUSIKIKACD = table.Column<string>(name: "YOUSIKI_KA_CD", type: "character varying(3)", maxLength: 3, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KACODE_RECE_YOUSIKI", x => new { x.RECEKACD, x.YOUSIKIKACD });
                });

            migrationBuilder.CreateTable(
                name: "KACODE_YOUSIKI_MST",
                columns: table => new
                {
                    YOUSIKIKACD = table.Column<string>(name: "YOUSIKI_KA_CD", type: "character varying(3)", maxLength: 3, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    KANAME = table.Column<string>(name: "KA_NAME", type: "character varying(40)", maxLength: 40, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KACODE_YOUSIKI_MST", x => x.YOUSIKIKACD);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KACODE_RECE_YOUSIKI");

            migrationBuilder.DropTable(
                name: "KACODE_YOUSIKI_MST");

            migrationBuilder.DropColumn(
                name: "YOUSIKI_KA_CD",
                table: "KA_MST");
        }
    }
}
