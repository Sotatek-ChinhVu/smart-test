using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class AddMigrationEntityRELEASENOTEREAD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RELEASENOTE_READ",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    VERSION = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RELEASENOTE_READ", x => new { x.HPID, x.USERID, x.VERSION });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RELEASENOTE_READ");
        }
    }
}
