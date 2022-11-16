using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostgreDataContext.Migrations
{
    public partial class AddRsvkrtByomei : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RSVKRT_BYOMEI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<int>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<int>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "bigint", nullable: false),
                    ID = table.Column<int>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD1 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD2 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD3 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD4 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD5 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD6 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD7 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD8 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD9 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD10 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD11 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD12 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD13 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD14 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD15 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD16 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD17 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD18 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD19 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD20 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKU_CD21 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    SYUBYO_KBN = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SIKKAN_KBN = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    NANBYO_CD = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    HOSOKU_CMT = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IS_NODSP_KARTE = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValue: "now()"),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("RSVKRT_BYOMEI");
        }
    }
}
