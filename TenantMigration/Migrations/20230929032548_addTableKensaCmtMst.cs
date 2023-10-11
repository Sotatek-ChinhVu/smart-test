using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addTableKensaCmtMst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KENSA_CMT_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", maxLength: 2, nullable: false),
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(3)", maxLength: 3, nullable: false),
                    CMTSEQNO = table.Column<int>(name: "CMT_SEQ_NO", type: "integer", maxLength: 9, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CENTERCD = table.Column<string>(name: "CENTER_CD", type: "character varying(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_KENSA_CMT_MST", x => new { x.HPID, x.CMTCD, x.CMTSEQNO });
                });

            migrationBuilder.CreateIndex(
                name: "KENSA_CMT_MST_SKEY1",
                table: "KENSA_CMT_MST",
                columns: new[] { "HP_ID", "CMT_CD", "CMT_SEQ_NO", "IS_DELETED" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KENSA_CMT_MST");
        }
    }
}
