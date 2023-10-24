using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddTableKensaResultLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KENSA_RESULT_LOG",
                columns: table => new
                {
                    OPID = table.Column<int>(name: "OP_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    IMPDATE = table.Column<int>(name: "IMP_DATE", type: "integer", nullable: false),
                    KEKAFILE = table.Column<string>(name: "KEKA_FILE", type: "text", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_RESULT_LOG", x => x.OPID);
                });

            migrationBuilder.CreateIndex(
                name: "RAIIN_INF_IDX04",
                table: "RAIIN_INF",
                columns: new[] { "HP_ID", "RAIIN_NO", "IS_DELETED", "STATUS" });

            migrationBuilder.CreateIndex(
                name: "PT_INF_IDX02",
                table: "PT_INF",
                columns: new[] { "HP_ID", "PT_ID", "IS_DELETE" });

            migrationBuilder.CreateIndex(
                name: "PT_HOKEN_INF_IDX01",
                table: "PT_HOKEN_INF",
                columns: new[] { "HP_ID", "PT_ID", "HOKEN_ID", "HOKEN_KBN", "HOUBETU" });

            migrationBuilder.CreateIndex(
                name: "KENSA_RESULT_LOG_IDX01",
                table: "KENSA_RESULT_LOG",
                columns: new[] { "HP_ID", "IMP_DATE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KENSA_RESULT_LOG");

            migrationBuilder.DropIndex(
                name: "RAIIN_INF_IDX04",
                table: "RAIIN_INF");

            migrationBuilder.DropIndex(
                name: "PT_INF_IDX02",
                table: "PT_INF");

            migrationBuilder.DropIndex(
                name: "PT_HOKEN_INF_IDX01",
                table: "PT_HOKEN_INF");
        }
    }
}
