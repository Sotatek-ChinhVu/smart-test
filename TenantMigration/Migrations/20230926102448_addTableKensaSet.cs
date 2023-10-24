using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addTableKensaSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KENSA_SET",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", maxLength: 2, nullable: false),
                    SETID = table.Column<int>(name: "SET_ID", type: "integer", maxLength: 9, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SETNAME = table.Column<string>(name: "SET_NAME", type: "character varying(30)", maxLength: 30, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", maxLength: 9, nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", maxLength: 1, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", maxLength: 8, nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", maxLength: 8, nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_SET", x => new { x.HPID, x.SETID });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_SET_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", maxLength: 2, nullable: false),
                    SETID = table.Column<int>(name: "SET_ID", type: "integer", maxLength: 9, nullable: false),
                    SETEDANO = table.Column<int>(name: "SET_EDA_NO", type: "integer", maxLength: 9, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    KENSAITEMSEQNO = table.Column<int>(name: "KENSA_ITEM_SEQ_NO", type: "integer", maxLength: 2, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", maxLength: 9, nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", maxLength: 1, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", maxLength: 8, nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", maxLength: 8, nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_SET_DETAIL", x => new { x.HPID, x.SETID, x.SETEDANO });
                });

            migrationBuilder.CreateIndex(
                name: "KENSA_SET_PKEY",
                table: "KENSA_SET",
                columns: new[] { "HP_ID", "SET_ID" });

            migrationBuilder.CreateIndex(
                name: "KENSA_SET_DETAIL_PKEY",
                table: "KENSA_SET_DETAIL",
                columns: new[] { "HP_ID", "SET_ID", "SET_EDA_NO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KENSA_SET");

            migrationBuilder.DropTable(
                name: "KENSA_SET_DETAIL");
        }
    }
}
