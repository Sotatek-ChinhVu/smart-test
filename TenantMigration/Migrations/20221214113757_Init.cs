using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNTING_FORM_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    FORMNO = table.Column<int>(name: "FORM_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FORMNAME = table.Column<string>(name: "FORM_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    FORMTYPE = table.Column<int>(name: "FORM_TYPE", type: "integer", nullable: false),
                    PRINTSORT = table.Column<int>(name: "PRINT_SORT", type: "integer", nullable: false),
                    MISEISANKBN = table.Column<int>(name: "MISEISAN_KBN", type: "integer", nullable: false),
                    SAIKBN = table.Column<int>(name: "SAI_KBN", type: "integer", nullable: false),
                    MISYUKBN = table.Column<int>(name: "MISYU_KBN", type: "integer", nullable: false),
                    SEIKYUKBN = table.Column<int>(name: "SEIKYU_KBN", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    FORM = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BASE = table.Column<int>(type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ACCOUNTING_FORM_MST", x => new { x.HPID, x.FORMNO });
                });

            migrationBuilder.CreateTable(
                name: "APPROVAL_INF",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: true),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: true),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_INF", x => new { x.ID, x.HPID, x.RAIINNO });
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_TRAIL_LOG",
                columns: table => new
                {
                    LOGID = table.Column<long>(name: "LOG_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LOGDATE = table.Column<DateTime>(name: "LOG_DATE", type: "timestamp with time zone", nullable: false),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    EVENTCD = table.Column<string>(name: "EVENT_CD", type: "character varying(11)", maxLength: 11, nullable: true),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDAY = table.Column<int>(name: "SIN_DAY", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_TRAIL_LOG", x => x.LOGID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_TRAIL_LOG_DETAIL",
                columns: table => new
                {
                    LOGID = table.Column<long>(name: "LOG_ID", type: "bigint", nullable: false),
                    HOSOKU = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_TRAIL_LOG_DETAIL", x => x.LOGID);
                });

            migrationBuilder.CreateTable(
                name: "AUTO_SANTEI_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTO_SANTEI_MST", x => new { x.ID, x.HPID, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "BACKUP_REQ",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OUTPUTTYPE = table.Column<int>(name: "OUTPUT_TYPE", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    FROMDATE = table.Column<int>(name: "FROM_DATE", type: "integer", nullable: false),
                    TODATE = table.Column<int>(name: "TO_DATE", type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BACKUP_REQ", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_BYOMEI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    BUIID = table.Column<int>(name: "BUI_ID", type: "integer", nullable: false),
                    BYOMEIBUI = table.Column<string>(name: "BYOMEI_BUI", type: "character varying(100)", maxLength: 100, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_BYOMEI_MST", x => new { x.HPID, x.BUIID, x.BYOMEIBUI });
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_ITEM_BYOMEI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    BYOMEIBUI = table.Column<string>(name: "BYOMEI_BUI", type: "character varying(100)", maxLength: 100, nullable: false),
                    LRKBN = table.Column<int>(name: "LR_KBN", type: "integer", nullable: false),
                    BOTHKBN = table.Column<int>(name: "BOTH_KBN", type: "integer", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_ITEM_BYOMEI_MST", x => new { x.HPID, x.ITEMCD, x.BYOMEIBUI });
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_ITEM_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_ITEM_MST", x => new { x.HPID, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    BUIID = table.Column<int>(name: "BUI_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ODRBUI = table.Column<string>(name: "ODR_BUI", type: "character varying(100)", maxLength: 100, nullable: true),
                    LRKBN = table.Column<int>(name: "LR_KBN", type: "integer", nullable: false),
                    MUSTLRKBN = table.Column<int>(name: "MUST_LR_KBN", type: "integer", nullable: false),
                    BOTHKBN = table.Column<int>(name: "BOTH_KBN", type: "integer", nullable: false),
                    KOUI30 = table.Column<int>(name: "KOUI_30", type: "integer", nullable: false),
                    KOUI40 = table.Column<int>(name: "KOUI_40", type: "integer", nullable: false),
                    KOUI50 = table.Column<int>(name: "KOUI_50", type: "integer", nullable: false),
                    KOUI60 = table.Column<int>(name: "KOUI_60", type: "integer", nullable: false),
                    KOUI70 = table.Column<int>(name: "KOUI_70", type: "integer", nullable: false),
                    KOUI80 = table.Column<int>(name: "KOUI_80", type: "integer", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_MST", x => new { x.HPID, x.BUIID });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SBYOMEI = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME1 = table.Column<string>(name: "KANA_NAME1", type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME2 = table.Column<string>(name: "KANA_NAME2", type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME3 = table.Column<string>(name: "KANA_NAME3", type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME4 = table.Column<string>(name: "KANA_NAME4", type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME5 = table.Column<string>(name: "KANA_NAME5", type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME6 = table.Column<string>(name: "KANA_NAME6", type: "character varying(200)", maxLength: 200, nullable: true),
                    KANANAME7 = table.Column<string>(name: "KANA_NAME7", type: "character varying(200)", maxLength: 200, nullable: true),
                    IKOCD = table.Column<string>(name: "IKO_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    SIKKANCD = table.Column<int>(name: "SIKKAN_CD", type: "integer", nullable: false),
                    TANDOKUKINSI = table.Column<int>(name: "TANDOKU_KINSI", type: "integer", nullable: false),
                    HOKENGAI = table.Column<int>(name: "HOKEN_GAI", type: "integer", nullable: false),
                    BYOMEIKANRI = table.Column<string>(name: "BYOMEI_KANRI", type: "character varying(8)", maxLength: 8, nullable: true),
                    SAITAKUKBN = table.Column<string>(name: "SAITAKU_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    KOUKANCD = table.Column<string>(name: "KOUKAN_CD", type: "character varying(4)", maxLength: 4, nullable: true),
                    SYUSAIDATE = table.Column<int>(name: "SYUSAI_DATE", type: "integer", nullable: false),
                    UPDDATE = table.Column<int>(name: "UPD_DATE", type: "integer", nullable: false),
                    DELDATE = table.Column<int>(name: "DEL_DATE", type: "integer", nullable: false),
                    NANBYOCD = table.Column<int>(name: "NANBYO_CD", type: "integer", nullable: false),
                    ICD101 = table.Column<string>(name: "ICD10_1", type: "character varying(5)", maxLength: 5, nullable: true),
                    ICD102 = table.Column<string>(name: "ICD10_2", type: "character varying(5)", maxLength: 5, nullable: true),
                    ICD1012013 = table.Column<string>(name: "ICD10_1_2013", type: "character varying(5)", maxLength: 5, nullable: true),
                    ICD1022013 = table.Column<string>(name: "ICD10_2_2013", type: "character varying(5)", maxLength: 5, nullable: true),
                    ISADOPTED = table.Column<int>(name: "IS_ADOPTED", type: "integer", nullable: false),
                    SYUSYOKUKBN = table.Column<string>(name: "SYUSYOKU_KBN", type: "character varying(8)", maxLength: 8, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BYOMEI_MST", x => new { x.HPID, x.BYOMEICD });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_MST_AFTERCARE",
                columns: table => new
                {
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BYOMEI_MST_AFTERCARE", x => new { x.BYOMEICD, x.BYOMEI, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_SET_GENERATION_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_BYOMEI_SET_GENERATION_MST", x => new { x.HPID, x.GENERATIONID });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_SET_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LEVEL1 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL2 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL3 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL4 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL5 = table.Column<int>(type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    SETNAME = table.Column<string>(name: "SET_NAME", type: "character varying(60)", maxLength: 60, nullable: true),
                    ISTITLE = table.Column<int>(name: "IS_TITLE", type: "integer", nullable: false),
                    SELECTTYPE = table.Column<int>(name: "SELECT_TYPE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_BYOMEI_SET_MST", x => new { x.HPID, x.GENERATIONID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "CALC_LOG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    LOGSBT = table.Column<int>(name: "LOG_SBT", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DELITEMCD = table.Column<string>(name: "DEL_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DELSBT = table.Column<int>(name: "DEL_SBT", type: "integer", nullable: false),
                    ISWARNING = table.Column<int>(name: "IS_WARNING", type: "integer", nullable: false),
                    TERMCNT = table.Column<int>(name: "TERM_CNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALC_LOG", x => new { x.HPID, x.PTID, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "CALC_STATUS",
                columns: table => new
                {
                    CALCID = table.Column<long>(name: "CALC_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEIKYUUP = table.Column<int>(name: "SEIKYU_UP", type: "integer", nullable: false),
                    CALCMODE = table.Column<int>(name: "CALC_MODE", type: "integer", nullable: false),
                    CLEARRECECHK = table.Column<int>(name: "CLEAR_RECE_CHK", type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALC_STATUS", x => x.CALCID);
                });

            migrationBuilder.CreateTable(
                name: "CMT_CHECK_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_CMT_CHECK_MST", x => new { x.HPID, x.ITEMCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "CMT_KBN_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMT_KBN_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CONTAINER_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    CONTAINERCD = table.Column<long>(name: "CONTAINER_CD", type: "bigint", nullable: false),
                    CONTAINERNAME = table.Column<string>(name: "CONTAINER_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTAINER_MST", x => new { x.HPID, x.CONTAINERCD });
                });

            migrationBuilder.CreateTable(
                name: "CONVERSION_ITEM_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SOURCEITEMCD = table.Column<string>(name: "SOURCE_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    DESTITEMCD = table.Column<string>(name: "DEST_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONVERSION_ITEM_INF", x => new { x.HPID, x.SOURCEITEMCD, x.DESTITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "DEF_HOKEN_NO",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    DIGIT1 = table.Column<string>(name: "DIGIT_1", type: "character varying(1)", maxLength: 1, nullable: false),
                    DIGIT2 = table.Column<string>(name: "DIGIT_2", type: "character varying(1)", maxLength: 1, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DIGIT3 = table.Column<string>(name: "DIGIT_3", type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT4 = table.Column<string>(name: "DIGIT_4", type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT5 = table.Column<string>(name: "DIGIT_5", type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT6 = table.Column<string>(name: "DIGIT_6", type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT7 = table.Column<string>(name: "DIGIT_7", type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT8 = table.Column<string>(name: "DIGIT_8", type: "character varying(1)", maxLength: 1, nullable: true),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DEF_HOKEN_NO", x => new { x.HPID, x.DIGIT1, x.DIGIT2, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_CUSTOM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD1 = table.Column<string>(name: "ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ITEMCD2 = table.Column<string>(name: "ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    HAIHANKBN = table.Column<int>(name: "HAIHAN_KBN", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TERMCNT = table.Column<int>(name: "TERM_CNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_CUSTOM", x => new { x.ID, x.HPID, x.ITEMCD1, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_DAY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD1 = table.Column<string>(name: "ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ITEMCD2 = table.Column<string>(name: "ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    HAIHANKBN = table.Column<int>(name: "HAIHAN_KBN", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_DAY", x => new { x.ID, x.HPID, x.ITEMCD1, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_KARTE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD1 = table.Column<string>(name: "ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ITEMCD2 = table.Column<string>(name: "ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    HAIHANKBN = table.Column<int>(name: "HAIHAN_KBN", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_KARTE", x => new { x.ID, x.HPID, x.ITEMCD1, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_MONTH",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD1 = table.Column<string>(name: "ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ITEMCD2 = table.Column<string>(name: "ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    HAIHANKBN = table.Column<int>(name: "HAIHAN_KBN", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    INCAFTER = table.Column<int>(name: "INC_AFTER", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_MONTH", x => new { x.ID, x.HPID, x.ITEMCD1, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_WEEK",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD1 = table.Column<string>(name: "ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ITEMCD2 = table.Column<string>(name: "ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    HAIHANKBN = table.Column<int>(name: "HAIHAN_KBN", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    INCAFTER = table.Column<int>(name: "INC_AFTER", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_WEEK", x => new { x.ID, x.HPID, x.ITEMCD1, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HOJYO",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    HOUKATUTERM1 = table.Column<int>(name: "HOUKATU_TERM1", type: "integer", nullable: false),
                    HOUKATUGRPNO1 = table.Column<string>(name: "HOUKATU_GRP_NO1", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOUKATUTERM2 = table.Column<int>(name: "HOUKATU_TERM2", type: "integer", nullable: false),
                    HOUKATUGRPNO2 = table.Column<string>(name: "HOUKATU_GRP_NO2", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOUKATUTERM3 = table.Column<int>(name: "HOUKATU_TERM3", type: "integer", nullable: false),
                    HOUKATUGRPNO3 = table.Column<string>(name: "HOUKATU_GRP_NO3", type: "character varying(7)", maxLength: 7, nullable: true),
                    HAIHANDAY = table.Column<int>(name: "HAIHAN_DAY", type: "integer", nullable: false),
                    HAIHANMONTH = table.Column<int>(name: "HAIHAN_MONTH", type: "integer", nullable: false),
                    HAIHANKARTE = table.Column<int>(name: "HAIHAN_KARTE", type: "integer", nullable: false),
                    HAIHANWEEK = table.Column<int>(name: "HAIHAN_WEEK", type: "integer", nullable: false),
                    NYUINID = table.Column<int>(name: "NYUIN_ID", type: "integer", nullable: false),
                    SANTEIKAISU = table.Column<int>(name: "SANTEI_KAISU", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HOJYO", x => new { x.HPID, x.ITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HOUKATU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    HOUKATUTERM = table.Column<int>(name: "HOUKATU_TERM", type: "integer", nullable: false),
                    HOUKATUGRPNO = table.Column<string>(name: "HOUKATU_GRP_NO", type: "character varying(7)", maxLength: 7, nullable: true),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HOUKATU", x => new { x.STARTDATE, x.HPID, x.ITEMCD, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HOUKATU_GRP",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    HOUKATUGRPNO = table.Column<string>(name: "HOUKATU_GRP_NO", type: "character varying(7)", maxLength: 7, nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HOUKATU_GRP", x => new { x.HPID, x.HOUKATUGRPNO, x.ITEMCD, x.SEQNO, x.USERSETTING, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_SANTEI_KAISU",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    UNITCD = table.Column<int>(name: "UNIT_CD", type: "integer", nullable: false),
                    MAXCOUNT = table.Column<int>(name: "MAX_COUNT", type: "integer", nullable: false),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    TERMCOUNT = table.Column<int>(name: "TERM_COUNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    ITEMGRPCD = table.Column<int>(name: "ITEM_GRP_CD", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_SANTEI_KAISU", x => new { x.HPID, x.ID, x.ITEMCD, x.SEQNO, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "DOC_CATEGORY_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORYNAME = table.Column<string>(name: "CATEGORY_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DOC_CATEGORY_MST", x => new { x.HPID, x.CATEGORYCD });
                });

            migrationBuilder.CreateTable(
                name: "DOC_COMMENT",
                columns: table => new
                {
                    CATEGORYID = table.Column<int>(name: "CATEGORY_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORYNAME = table.Column<string>(name: "CATEGORY_NAME", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    REPLACEWORD = table.Column<string>(name: "REPLACE_WORD", type: "text", nullable: true),
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
                    table.PrimaryKey("PK_DOC_COMMENT", x => x.CATEGORYID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_COMMENT_DETAIL",
                columns: table => new
                {
                    CATEGORYID = table.Column<int>(name: "CATEGORY_ID", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DOC_COMMENT_DETAIL", x => new { x.CATEGORYID, x.EDANO });
                });

            migrationBuilder.CreateTable(
                name: "DOC_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(300)", maxLength: 300, nullable: true),
                    DSPFILENAME = table.Column<string>(name: "DSP_FILE_NAME", type: "character varying(300)", maxLength: 300, nullable: true),
                    ISLOCKED = table.Column<int>(name: "IS_LOCKED", type: "integer", nullable: false),
                    LOCKDATE = table.Column<DateTime>(name: "LOCK_DATE", type: "timestamp with time zone", nullable: true),
                    LOCKID = table.Column<int>(name: "LOCK_ID", type: "integer", nullable: false),
                    LOCKMACHINE = table.Column<string>(name: "LOCK_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
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
                    table.PrimaryKey("PK_DOC_INF", x => new { x.HPID, x.PTID, x.SINDATE, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "DOSAGE_MST",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ONCEMIN = table.Column<double>(name: "ONCE_MIN", type: "double precision", nullable: false),
                    ONCEMAX = table.Column<double>(name: "ONCE_MAX", type: "double precision", nullable: false),
                    ONCELIMIT = table.Column<double>(name: "ONCE_LIMIT", type: "double precision", nullable: false),
                    ONCEUNIT = table.Column<int>(name: "ONCE_UNIT", type: "integer", nullable: false),
                    DAYMIN = table.Column<double>(name: "DAY_MIN", type: "double precision", nullable: false),
                    DAYMAX = table.Column<double>(name: "DAY_MAX", type: "double precision", nullable: false),
                    DAYLIMIT = table.Column<double>(name: "DAY_LIMIT", type: "double precision", nullable: false),
                    DAYUNIT = table.Column<int>(name: "DAY_UNIT", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DOSAGE_MST", x => new { x.ID, x.HPID, x.ITEMCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "DRUG_DAY_LIMIT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    LIMITDAY = table.Column<int>(name: "LIMIT_DAY", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_DRUG_DAY_LIMIT", x => new { x.ID, x.HPID, x.ITEMCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "DRUG_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    INFKBN = table.Column<int>(name: "INF_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DRUGINF = table.Column<string>(name: "DRUG_INF", type: "character varying(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_DRUG_INF", x => new { x.INFKBN, x.HPID, x.ITEMCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "DRUG_UNIT_CONV",
                columns: table => new
                {
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CNVVAL = table.Column<double>(name: "CNV_VAL", type: "double precision", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DRUG_UNIT_CONV", x => new { x.ITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "EVENT_MST",
                columns: table => new
                {
                    EVENTCD = table.Column<string>(name: "EVENT_CD", type: "character varying(11)", maxLength: 11, nullable: false),
                    EVENTNAME = table.Column<string>(name: "EVENT_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    AUDITTRAILING = table.Column<int>(name: "AUDIT_TRAILING", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_MST", x => x.EVENTCD);
                });

            migrationBuilder.CreateTable(
                name: "EXCEPT_HOKENSYA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    HOKENSYANO = table.Column<string>(name: "HOKENSYA_NO", type: "character varying(8)", maxLength: 8, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXCEPT_HOKENSYA", x => new { x.ID, x.HPID, x.PREFNO, x.HOKENNO, x.HOKENEDANO, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "FILING_AUTO_IMP",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    IMPPATH = table.Column<string>(name: "IMP_PATH", type: "character varying(300)", maxLength: 300, nullable: true),
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
                    table.PrimaryKey("PK_FILING_AUTO_IMP", x => new { x.SEQNO, x.HPID });
                });

            migrationBuilder.CreateTable(
                name: "FILING_CATEGORY_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORYNAME = table.Column<string>(name: "CATEGORY_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    DSPKANZOK = table.Column<int>(name: "DSP_KANZOK", type: "integer", nullable: false),
                    ISFILEDELETED = table.Column<int>(name: "IS_FILE_DELETED", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_FILING_CATEGORY_MST", x => new { x.CATEGORYCD, x.HPID });
                });

            migrationBuilder.CreateTable(
                name: "FILING_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    FILEID = table.Column<int>(name: "FILE_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    GETDATE = table.Column<int>(name: "GET_DATE", type: "integer", nullable: false),
                    FILENO = table.Column<int>(name: "FILE_NO", type: "integer", nullable: false),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(300)", maxLength: 300, nullable: true),
                    DSPFILENAME = table.Column<string>(name: "DSP_FILE_NAME", type: "character varying(1024)", maxLength: 1024, nullable: true),
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
                    table.PrimaryKey("PK_FILING_INF", x => new { x.HPID, x.FILEID, x.PTID, x.GETDATE, x.FILENO });
                });

            migrationBuilder.CreateTable(
                name: "FUNCTION_MST",
                columns: table => new
                {
                    FUNCTIONCD = table.Column<string>(name: "FUNCTION_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    FUNCTIONNAME = table.Column<string>(name: "FUNCTION_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCTION_MST", x => x.FUNCTIONCD);
                });

            migrationBuilder.CreateTable(
                name: "GC_STD_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    STDKBN = table.Column<int>(name: "STD_KBN", type: "integer", nullable: false),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    POINT = table.Column<double>(type: "double precision", nullable: false),
                    SDM25 = table.Column<double>(name: "SD_M25", type: "double precision", nullable: false),
                    SDM20 = table.Column<double>(name: "SD_M20", type: "double precision", nullable: false),
                    SDM10 = table.Column<double>(name: "SD_M10", type: "double precision", nullable: false),
                    SDAVG = table.Column<double>(name: "SD_AVG", type: "double precision", nullable: false),
                    SDP10 = table.Column<double>(name: "SD_P10", type: "double precision", nullable: false),
                    SDP20 = table.Column<double>(name: "SD_P20", type: "double precision", nullable: false),
                    SDP25 = table.Column<double>(name: "SD_P25", type: "double precision", nullable: false),
                    PER03 = table.Column<double>(name: "PER_03", type: "double precision", nullable: false),
                    PER10 = table.Column<double>(name: "PER_10", type: "double precision", nullable: false),
                    PER25 = table.Column<double>(name: "PER_25", type: "double precision", nullable: false),
                    PER50 = table.Column<double>(name: "PER_50", type: "double precision", nullable: false),
                    PER75 = table.Column<double>(name: "PER_75", type: "double precision", nullable: false),
                    PER90 = table.Column<double>(name: "PER_90", type: "double precision", nullable: false),
                    PER97 = table.Column<double>(name: "PER_97", type: "double precision", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GC_STD_MST", x => new { x.HPID, x.STDKBN, x.SEX, x.POINT });
                });

            migrationBuilder.CreateTable(
                name: "HOKEN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    HOKENSBTKBN = table.Column<int>(name: "HOKEN_SBT_KBN", type: "integer", nullable: false),
                    HOKENKOHIKBN = table.Column<int>(name: "HOKEN_KOHI_KBN", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENNAME = table.Column<string>(name: "HOKEN_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSNAME = table.Column<string>(name: "HOKEN_SNAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    HOKENNAMECD = table.Column<string>(name: "HOKEN_NAME_CD", type: "character varying(5)", maxLength: 5, nullable: true),
                    CHECKDIGIT = table.Column<int>(name: "CHECK_DIGIT", type: "integer", nullable: false),
                    JYUKYUCHECKDIGIT = table.Column<int>(name: "JYUKYU_CHECK_DIGIT", type: "integer", nullable: false),
                    ISFUTANSYANOCHECK = table.Column<int>(name: "IS_FUTANSYA_NO_CHECK", type: "integer", nullable: false),
                    ISJYUKYUSYANOCHECK = table.Column<int>(name: "IS_JYUKYUSYA_NO_CHECK", type: "integer", nullable: false),
                    ISTOKUSYUNOCHECK = table.Column<int>(name: "IS_TOKUSYU_NO_CHECK", type: "integer", nullable: false),
                    ISLIMITLIST = table.Column<int>(name: "IS_LIMIT_LIST", type: "integer", nullable: false),
                    ISLIMITLISTSUM = table.Column<int>(name: "IS_LIMIT_LIST_SUM", type: "integer", nullable: false),
                    ISOTHERPREFVALID = table.Column<int>(name: "IS_OTHER_PREF_VALID", type: "integer", nullable: false),
                    AGESTART = table.Column<int>(name: "AGE_START", type: "integer", nullable: false),
                    AGEEND = table.Column<int>(name: "AGE_END", type: "integer", nullable: false),
                    ENTEN = table.Column<int>(name: "EN_TEN", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    RECESPKBN = table.Column<int>(name: "RECE_SP_KBN", type: "integer", nullable: false),
                    RECESEIKYUKBN = table.Column<int>(name: "RECE_SEIKYU_KBN", type: "integer", nullable: false),
                    RECEFUTANROUND = table.Column<int>(name: "RECE_FUTAN_ROUND", type: "integer", nullable: false),
                    RECEKISAI = table.Column<int>(name: "RECE_KISAI", type: "integer", nullable: false),
                    RECEKISAI2 = table.Column<int>(name: "RECE_KISAI2", type: "integer", nullable: false),
                    RECEZEROKISAI = table.Column<int>(name: "RECE_ZERO_KISAI", type: "integer", nullable: false),
                    RECEFUTANHIDE = table.Column<int>(name: "RECE_FUTAN_HIDE", type: "integer", nullable: false),
                    RECEFUTANKBN = table.Column<int>(name: "RECE_FUTAN_KBN", type: "integer", nullable: false),
                    RECETENKISAI = table.Column<int>(name: "RECE_TEN_KISAI", type: "integer", nullable: false),
                    KOGAKUTOTALKBN = table.Column<int>(name: "KOGAKU_TOTAL_KBN", type: "integer", nullable: false),
                    KOGAKUTOTALALL = table.Column<int>(name: "KOGAKU_TOTAL_ALL", type: "integer", nullable: false),
                    CALCSPKBN = table.Column<int>(name: "CALC_SP_KBN", type: "integer", nullable: false),
                    KOGAKUTOTALEXCFUTAN = table.Column<int>(name: "KOGAKU_TOTAL_EXC_FUTAN", type: "integer", nullable: false),
                    KOGAKUTEKIYO = table.Column<int>(name: "KOGAKU_TEKIYO", type: "integer", nullable: false),
                    FUTANYUSEN = table.Column<int>(name: "FUTAN_YUSEN", type: "integer", nullable: false),
                    LIMITKBN = table.Column<int>(name: "LIMIT_KBN", type: "integer", nullable: false),
                    COUNTKBN = table.Column<int>(name: "COUNT_KBN", type: "integer", nullable: false),
                    FUTANKBN = table.Column<int>(name: "FUTAN_KBN", type: "integer", nullable: false),
                    FUTANRATE = table.Column<int>(name: "FUTAN_RATE", type: "integer", nullable: false),
                    KAIFUTANGAKU = table.Column<int>(name: "KAI_FUTANGAKU", type: "integer", nullable: false),
                    KAILIMITFUTAN = table.Column<int>(name: "KAI_LIMIT_FUTAN", type: "integer", nullable: false),
                    DAYLIMITFUTAN = table.Column<int>(name: "DAY_LIMIT_FUTAN", type: "integer", nullable: false),
                    DAYLIMITCOUNT = table.Column<int>(name: "DAY_LIMIT_COUNT", type: "integer", nullable: false),
                    MONTHLIMITFUTAN = table.Column<int>(name: "MONTH_LIMIT_FUTAN", type: "integer", nullable: false),
                    MONTHSPLIMIT = table.Column<int>(name: "MONTH_SP_LIMIT", type: "integer", nullable: false),
                    MONTHLIMITCOUNT = table.Column<int>(name: "MONTH_LIMIT_COUNT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    RECEKISAIKOKHO = table.Column<int>(name: "RECE_KISAI_KOKHO", type: "integer", nullable: false),
                    KOGAKUHAIRYOKBN = table.Column<int>(name: "KOGAKU_HAIRYO_KBN", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOKEN_MST", x => new { x.HPID, x.PREFNO, x.HOKENNO, x.HOKENEDANO, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "HOKENSYA_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    HOKENSYANO = table.Column<string>(name: "HOKENSYA_NO", type: "character varying(8)", maxLength: 8, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOUBETUKBN = table.Column<string>(name: "HOUBETU_KBN", type: "character varying(2)", maxLength: 2, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ISKIGONA = table.Column<int>(name: "IS_KIGO_NA", type: "integer", nullable: false),
                    RATEHONNIN = table.Column<int>(name: "RATE_HONNIN", type: "integer", nullable: false),
                    RATEKAZOKU = table.Column<int>(name: "RATE_KAZOKU", type: "integer", nullable: false),
                    POSTCODE = table.Column<string>(name: "POST_CODE", type: "character varying(7)", maxLength: 7, nullable: true),
                    ADDRESS1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ADDRESS2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TEL1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    DELETEDATE = table.Column<int>(name: "DELETE_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_HOKENSYA_MST", x => new { x.HPID, x.HOKENSYANO });
                });

            migrationBuilder.CreateTable(
                name: "HOLIDAY_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOLIDAYKBN = table.Column<int>(name: "HOLIDAY_KBN", type: "integer", nullable: false),
                    KYUSINKBN = table.Column<int>(name: "KYUSIN_KBN", type: "integer", nullable: false),
                    HOLIDAYNAME = table.Column<string>(name: "HOLIDAY_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_HOLIDAY_MST", x => new { x.HPID, x.SINDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "HP_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    HPCD = table.Column<string>(name: "HP_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    ROUSAIHPCD = table.Column<string>(name: "ROUSAI_HP_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    HPNAME = table.Column<string>(name: "HP_NAME", type: "character varying(80)", maxLength: 80, nullable: true),
                    RECEHPNAME = table.Column<string>(name: "RECE_HP_NAME", type: "character varying(80)", maxLength: 80, nullable: true),
                    KAISETUNAME = table.Column<string>(name: "KAISETU_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    POSTCD = table.Column<string>(name: "POST_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    FAXNO = table.Column<string>(name: "FAX_NO", type: "character varying(15)", maxLength: 15, nullable: true),
                    OTHERCONTACTS = table.Column<string>(name: "OTHER_CONTACTS", type: "character varying(100)", maxLength: 100, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_INF", x => new { x.HPID, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "IPN_KASAN_EXCLUDE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    IPNNAMECD = table.Column<string>(name: "IPN_NAME_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_KASAN_EXCLUDE", x => new { x.HPID, x.STARTDATE, x.IPNNAMECD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "IPN_KASAN_EXCLUDE_ITEM",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_KASAN_EXCLUDE_ITEM", x => new { x.HPID, x.STARTDATE, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "IPN_KASAN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    IPNNAMECD = table.Column<string>(name: "IPN_NAME_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    KASAN1 = table.Column<int>(type: "integer", nullable: false),
                    KASAN2 = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_IPN_KASAN_MST", x => new { x.HPID, x.STARTDATE, x.IPNNAMECD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "IPN_MIN_YAKKA_MST",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    IPNNAMECD = table.Column<string>(name: "IPN_NAME_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    YAKKA = table.Column<double>(type: "double precision", nullable: false),
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
                    table.PrimaryKey("PK_IPN_MIN_YAKKA_MST", x => new { x.HPID, x.ID, x.IPNNAMECD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "IPN_NAME_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    IPNNAMECD = table.Column<string>(name: "IPN_NAME_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    IPNNAME = table.Column<string>(name: "IPN_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_IPN_NAME_MST", x => new { x.HPID, x.IPNNAMECD, x.STARTDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "ITEM_GRP_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPSBT = table.Column<long>(name: "GRP_SBT", type: "bigint", nullable: false),
                    ITEMGRPCD = table.Column<long>(name: "ITEM_GRP_CD", type: "bigint", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_GRP_MST", x => new { x.HPID, x.GRPSBT, x.ITEMGRPCD, x.SEQNO, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "JIHI_SBT_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    JIHISBT = table.Column<int>(name: "JIHI_SBT", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    ISYOBO = table.Column<int>(name: "IS_YOBO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JIHI_SBT_MST", x => new { x.HPID, x.JIHISBT });
                });

            migrationBuilder.CreateTable(
                name: "JOB_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    JOBCD = table.Column<int>(name: "JOB_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JOBNAME = table.Column<string>(name: "JOB_NAME", type: "character varying(10)", maxLength: 10, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOB_MST", x => new { x.JOBCD, x.HPID });
                });

            migrationBuilder.CreateTable(
                name: "KA_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    RECEKACD = table.Column<string>(name: "RECE_KA_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    KASNAME = table.Column<string>(name: "KA_SNAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    KANAME = table.Column<string>(name: "KA_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_KA_MST", x => new { x.ID, x.HPID });
                });

            migrationBuilder.CreateTable(
                name: "KACODE_MST",
                columns: table => new
                {
                    RECEKACD = table.Column<string>(name: "RECE_KA_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    KANAME = table.Column<string>(name: "KA_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KACODE_MST", x => x.RECEKACD);
                });

            migrationBuilder.CreateTable(
                name: "KAIKEI_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    ADJUSTPID = table.Column<int>(name: "ADJUST_PID", type: "integer", nullable: false),
                    OYARAIINNO = table.Column<long>(name: "OYA_RAIIN_NO", type: "bigint", nullable: false),
                    ADJUSTKID = table.Column<int>(name: "ADJUST_KID", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOKENSBTCD = table.Column<int>(name: "HOKEN_SBT_CD", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    KOHI1ID = table.Column<int>(name: "KOHI1_ID", type: "integer", nullable: false),
                    KOHI2ID = table.Column<int>(name: "KOHI2_ID", type: "integer", nullable: false),
                    KOHI3ID = table.Column<int>(name: "KOHI3_ID", type: "integer", nullable: false),
                    KOHI4ID = table.Column<int>(name: "KOHI4_ID", type: "integer", nullable: false),
                    ROUSAIID = table.Column<int>(name: "ROUSAI_ID", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1HOUBETU = table.Column<string>(name: "KOHI1_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI2HOUBETU = table.Column<string>(name: "KOHI2_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI3HOUBETU = table.Column<string>(name: "KOHI3_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI4HOUBETU = table.Column<string>(name: "KOHI4_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1PRIORITY = table.Column<string>(name: "KOHI1_PRIORITY", type: "character varying(8)", maxLength: 8, nullable: true),
                    KOHI2PRIORITY = table.Column<string>(name: "KOHI2_PRIORITY", type: "character varying(8)", maxLength: 8, nullable: true),
                    KOHI3PRIORITY = table.Column<string>(name: "KOHI3_PRIORITY", type: "character varying(8)", maxLength: 8, nullable: true),
                    KOHI4PRIORITY = table.Column<string>(name: "KOHI4_PRIORITY", type: "character varying(8)", maxLength: 8, nullable: true),
                    HONKEKBN = table.Column<int>(name: "HONKE_KBN", type: "integer", nullable: false),
                    KOGAKUKBN = table.Column<int>(name: "KOGAKU_KBN", type: "integer", nullable: false),
                    KOGAKUTEKIYOKBN = table.Column<int>(name: "KOGAKU_TEKIYO_KBN", type: "integer", nullable: false),
                    ISTOKUREI = table.Column<int>(name: "IS_TOKUREI", type: "integer", nullable: false),
                    ISTASUKAI = table.Column<int>(name: "IS_TASUKAI", type: "integer", nullable: false),
                    KOGAKUTOTALKBN = table.Column<int>(name: "KOGAKU_TOTAL_KBN", type: "integer", nullable: false),
                    ISCHOKI = table.Column<int>(name: "IS_CHOKI", type: "integer", nullable: false),
                    KOGAKULIMIT = table.Column<int>(name: "KOGAKU_LIMIT", type: "integer", nullable: false),
                    TOTALKOGAKULIMIT = table.Column<int>(name: "TOTAL_KOGAKU_LIMIT", type: "integer", nullable: false),
                    GENMENKBN = table.Column<int>(name: "GENMEN_KBN", type: "integer", nullable: false),
                    ENTEN = table.Column<int>(name: "EN_TEN", type: "integer", nullable: false),
                    HOKENRATE = table.Column<int>(name: "HOKEN_RATE", type: "integer", nullable: false),
                    PTRATE = table.Column<int>(name: "PT_RATE", type: "integer", nullable: false),
                    KOHI1LIMIT = table.Column<int>(name: "KOHI1_LIMIT", type: "integer", nullable: false),
                    KOHI1OTHERFUTAN = table.Column<int>(name: "KOHI1_OTHER_FUTAN", type: "integer", nullable: false),
                    KOHI2LIMIT = table.Column<int>(name: "KOHI2_LIMIT", type: "integer", nullable: false),
                    KOHI2OTHERFUTAN = table.Column<int>(name: "KOHI2_OTHER_FUTAN", type: "integer", nullable: false),
                    KOHI3LIMIT = table.Column<int>(name: "KOHI3_LIMIT", type: "integer", nullable: false),
                    KOHI3OTHERFUTAN = table.Column<int>(name: "KOHI3_OTHER_FUTAN", type: "integer", nullable: false),
                    KOHI4LIMIT = table.Column<int>(name: "KOHI4_LIMIT", type: "integer", nullable: false),
                    KOHI4OTHERFUTAN = table.Column<int>(name: "KOHI4_OTHER_FUTAN", type: "integer", nullable: false),
                    TENSU = table.Column<int>(type: "integer", nullable: false),
                    TOTALIRYOHI = table.Column<int>(name: "TOTAL_IRYOHI", type: "integer", nullable: false),
                    HOKENFUTAN = table.Column<int>(name: "HOKEN_FUTAN", type: "integer", nullable: false),
                    KOGAKUFUTAN = table.Column<int>(name: "KOGAKU_FUTAN", type: "integer", nullable: false),
                    KOHI1FUTAN = table.Column<int>(name: "KOHI1_FUTAN", type: "integer", nullable: false),
                    KOHI2FUTAN = table.Column<int>(name: "KOHI2_FUTAN", type: "integer", nullable: false),
                    KOHI3FUTAN = table.Column<int>(name: "KOHI3_FUTAN", type: "integer", nullable: false),
                    KOHI4FUTAN = table.Column<int>(name: "KOHI4_FUTAN", type: "integer", nullable: false),
                    ICHIBUFUTAN = table.Column<int>(name: "ICHIBU_FUTAN", type: "integer", nullable: false),
                    GENMENGAKU = table.Column<int>(name: "GENMEN_GAKU", type: "integer", nullable: false),
                    HOKENFUTAN10EN = table.Column<int>(name: "HOKEN_FUTAN_10EN", type: "integer", nullable: false),
                    KOGAKUFUTAN10EN = table.Column<int>(name: "KOGAKU_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI1FUTAN10EN = table.Column<int>(name: "KOHI1_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI2FUTAN10EN = table.Column<int>(name: "KOHI2_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI3FUTAN10EN = table.Column<int>(name: "KOHI3_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI4FUTAN10EN = table.Column<int>(name: "KOHI4_FUTAN_10EN", type: "integer", nullable: false),
                    ICHIBUFUTAN10EN = table.Column<int>(name: "ICHIBU_FUTAN_10EN", type: "integer", nullable: false),
                    GENMENGAKU10EN = table.Column<int>(name: "GENMEN_GAKU_10EN", type: "integer", nullable: false),
                    ADJUSTROUND = table.Column<int>(name: "ADJUST_ROUND", type: "integer", nullable: false),
                    PTFUTAN = table.Column<int>(name: "PT_FUTAN", type: "integer", nullable: false),
                    KOGAKUOVERKBN = table.Column<int>(name: "KOGAKU_OVER_KBN", type: "integer", nullable: false),
                    RECESBT = table.Column<string>(name: "RECE_SBT", type: "character varying(4)", maxLength: 4, nullable: true),
                    JITUNISU = table.Column<int>(type: "integer", nullable: false),
                    ROUSAIIFUTAN = table.Column<int>(name: "ROUSAI_I_FUTAN", type: "integer", nullable: false),
                    ROUSAIROFUTAN = table.Column<int>(name: "ROUSAI_RO_FUTAN", type: "integer", nullable: false),
                    JIBAIITENSU = table.Column<int>(name: "JIBAI_I_TENSU", type: "integer", nullable: false),
                    JIBAIROTENSU = table.Column<int>(name: "JIBAI_RO_TENSU", type: "integer", nullable: false),
                    JIBAIHAFUTAN = table.Column<int>(name: "JIBAI_HA_FUTAN", type: "integer", nullable: false),
                    JIBAINIFUTAN = table.Column<int>(name: "JIBAI_NI_FUTAN", type: "integer", nullable: false),
                    JIBAIHOSINDAN = table.Column<int>(name: "JIBAI_HO_SINDAN", type: "integer", nullable: false),
                    JIBAIHOSINDANCOUNT = table.Column<int>(name: "JIBAI_HO_SINDAN_COUNT", type: "integer", nullable: false),
                    JIBAIHEMEISAI = table.Column<int>(name: "JIBAI_HE_MEISAI", type: "integer", nullable: false),
                    JIBAIHEMEISAICOUNT = table.Column<int>(name: "JIBAI_HE_MEISAI_COUNT", type: "integer", nullable: false),
                    JIBAIAFUTAN = table.Column<int>(name: "JIBAI_A_FUTAN", type: "integer", nullable: false),
                    JIBAIBFUTAN = table.Column<int>(name: "JIBAI_B_FUTAN", type: "integer", nullable: false),
                    JIBAICFUTAN = table.Column<int>(name: "JIBAI_C_FUTAN", type: "integer", nullable: false),
                    JIBAIDFUTAN = table.Column<int>(name: "JIBAI_D_FUTAN", type: "integer", nullable: false),
                    JIBAIKENPOTENSU = table.Column<int>(name: "JIBAI_KENPO_TENSU", type: "integer", nullable: false),
                    JIBAIKENPOFUTAN = table.Column<int>(name: "JIBAI_KENPO_FUTAN", type: "integer", nullable: false),
                    JIHIFUTAN = table.Column<int>(name: "JIHI_FUTAN", type: "integer", nullable: false),
                    JIHITAX = table.Column<int>(name: "JIHI_TAX", type: "integer", nullable: false),
                    JIHIOUTTAX = table.Column<int>(name: "JIHI_OUTTAX", type: "integer", nullable: false),
                    JIHIFUTANTAXFREE = table.Column<int>(name: "JIHI_FUTAN_TAXFREE", type: "integer", nullable: false),
                    JIHIFUTANTAXNR = table.Column<int>(name: "JIHI_FUTAN_TAX_NR", type: "integer", nullable: false),
                    JIHIFUTANTAXGEN = table.Column<int>(name: "JIHI_FUTAN_TAX_GEN", type: "integer", nullable: false),
                    JIHIFUTANOUTTAXNR = table.Column<int>(name: "JIHI_FUTAN_OUTTAX_NR", type: "integer", nullable: false),
                    JIHIFUTANOUTTAXGEN = table.Column<int>(name: "JIHI_FUTAN_OUTTAX_GEN", type: "integer", nullable: false),
                    JIHITAXNR = table.Column<int>(name: "JIHI_TAX_NR", type: "integer", nullable: false),
                    JIHITAXGEN = table.Column<int>(name: "JIHI_TAX_GEN", type: "integer", nullable: false),
                    JIHIOUTTAXNR = table.Column<int>(name: "JIHI_OUTTAX_NR", type: "integer", nullable: false),
                    JIHIOUTTAXGEN = table.Column<int>(name: "JIHI_OUTTAX_GEN", type: "integer", nullable: false),
                    TOTALPTFUTAN = table.Column<int>(name: "TOTAL_PT_FUTAN", type: "integer", nullable: false),
                    SORTKEY = table.Column<string>(name: "SORT_KEY", type: "character varying(61)", maxLength: 61, nullable: true),
                    ISNINPU = table.Column<int>(name: "IS_NINPU", type: "integer", nullable: false),
                    ISZAIISO = table.Column<int>(name: "IS_ZAIISO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KAIKEI_DETAIL", x => new { x.HPID, x.PTID, x.SINDATE, x.RAIINNO, x.HOKENPID, x.ADJUSTPID });
                });

            migrationBuilder.CreateTable(
                name: "KAIKEI_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    KOHI1ID = table.Column<int>(name: "KOHI1_ID", type: "integer", nullable: false),
                    KOHI2ID = table.Column<int>(name: "KOHI2_ID", type: "integer", nullable: false),
                    KOHI3ID = table.Column<int>(name: "KOHI3_ID", type: "integer", nullable: false),
                    KOHI4ID = table.Column<int>(name: "KOHI4_ID", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOKENSBTCD = table.Column<int>(name: "HOKEN_SBT_CD", type: "integer", nullable: false),
                    RECESBT = table.Column<string>(name: "RECE_SBT", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1HOUBETU = table.Column<string>(name: "KOHI1_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI2HOUBETU = table.Column<string>(name: "KOHI2_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI3HOUBETU = table.Column<string>(name: "KOHI3_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI4HOUBETU = table.Column<string>(name: "KOHI4_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    HONKEKBN = table.Column<int>(name: "HONKE_KBN", type: "integer", nullable: false),
                    HOKENRATE = table.Column<int>(name: "HOKEN_RATE", type: "integer", nullable: false),
                    PTRATE = table.Column<int>(name: "PT_RATE", type: "integer", nullable: false),
                    DISPRATE = table.Column<int>(name: "DISP_RATE", type: "integer", nullable: false),
                    TENSU = table.Column<int>(type: "integer", nullable: false),
                    TOTALIRYOHI = table.Column<int>(name: "TOTAL_IRYOHI", type: "integer", nullable: false),
                    PTFUTAN = table.Column<int>(name: "PT_FUTAN", type: "integer", nullable: false),
                    JIHIFUTAN = table.Column<int>(name: "JIHI_FUTAN", type: "integer", nullable: false),
                    JIHITAX = table.Column<int>(name: "JIHI_TAX", type: "integer", nullable: false),
                    JIHIOUTTAX = table.Column<int>(name: "JIHI_OUTTAX", type: "integer", nullable: false),
                    JIHIFUTANTAXFREE = table.Column<int>(name: "JIHI_FUTAN_TAXFREE", type: "integer", nullable: false),
                    JIHIFUTANTAXNR = table.Column<int>(name: "JIHI_FUTAN_TAX_NR", type: "integer", nullable: false),
                    JIHIFUTANTAXGEN = table.Column<int>(name: "JIHI_FUTAN_TAX_GEN", type: "integer", nullable: false),
                    JIHIFUTANOUTTAXNR = table.Column<int>(name: "JIHI_FUTAN_OUTTAX_NR", type: "integer", nullable: false),
                    JIHIFUTANOUTTAXGEN = table.Column<int>(name: "JIHI_FUTAN_OUTTAX_GEN", type: "integer", nullable: false),
                    JIHITAXNR = table.Column<int>(name: "JIHI_TAX_NR", type: "integer", nullable: false),
                    JIHITAXGEN = table.Column<int>(name: "JIHI_TAX_GEN", type: "integer", nullable: false),
                    JIHIOUTTAXNR = table.Column<int>(name: "JIHI_OUTTAX_NR", type: "integer", nullable: false),
                    JIHIOUTTAXGEN = table.Column<int>(name: "JIHI_OUTTAX_GEN", type: "integer", nullable: false),
                    ADJUSTFUTAN = table.Column<int>(name: "ADJUST_FUTAN", type: "integer", nullable: false),
                    ADJUSTROUND = table.Column<int>(name: "ADJUST_ROUND", type: "integer", nullable: false),
                    TOTALPTFUTAN = table.Column<int>(name: "TOTAL_PT_FUTAN", type: "integer", nullable: false),
                    ADJUSTFUTANVAL = table.Column<int>(name: "ADJUST_FUTAN_VAL", type: "integer", nullable: false),
                    ADJUSTFUTANRANGE = table.Column<int>(name: "ADJUST_FUTAN_RANGE", type: "integer", nullable: false),
                    ADJUSTRATEVAL = table.Column<int>(name: "ADJUST_RATE_VAL", type: "integer", nullable: false),
                    ADJUSTRATERANGE = table.Column<int>(name: "ADJUST_RATE_RANGE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KAIKEI_INF", x => new { x.HPID, x.PTID, x.SINDATE, x.RAIINNO, x.HOKENID });
                });

            migrationBuilder.CreateTable(
                name: "KANTOKU_MST",
                columns: table => new
                {
                    ROUDOUCD = table.Column<string>(name: "ROUDOU_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    KANTOKUCD = table.Column<string>(name: "KANTOKU_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    KANTOKUNAME = table.Column<string>(name: "KANTOKU_NAME", type: "character varying(60)", maxLength: 60, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KANTOKU_MST", x => new { x.ROUDOUCD, x.KANTOKUCD });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_FILTER_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    FILTERID = table.Column<long>(name: "FILTER_ID", type: "bigint", nullable: false),
                    FILTERITEMCD = table.Column<int>(name: "FILTER_ITEM_CD", type: "integer", nullable: false),
                    FILTEREDANO = table.Column<int>(name: "FILTER_EDA_NO", type: "integer", nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_FILTER_DETAIL", x => new { x.HPID, x.USERID, x.FILTERID, x.FILTERITEMCD, x.FILTEREDANO });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_FILTER_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    FILTERID = table.Column<long>(name: "FILTER_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILTERNAME = table.Column<string>(name: "FILTER_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    AUTOAPPLY = table.Column<int>(name: "AUTO_APPLY", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_KARTE_FILTER_MST", x => new { x.HPID, x.USERID, x.FILTERID });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_IMG_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    POSITION = table.Column<long>(type: "bigint", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    MESSAGE = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_IMG_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KARTE_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RICHTEXT = table.Column<byte[]>(name: "RICH_TEXT", type: "bytea", nullable: true),
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
                    table.PrimaryKey("PK_KARTE_INF", x => new { x.HPID, x.RAIINNO, x.SEQNO, x.KARTEKBN });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_KBN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    KBNNAME = table.Column<string>(name: "KBN_NAME", type: "character varying(10)", maxLength: 10, nullable: true),
                    KBNSHORTNAME = table.Column<string>(name: "KBN_SHORT_NAME", type: "character varying(1)", maxLength: 1, nullable: true),
                    CANIMG = table.Column<int>(name: "CAN_IMG", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_KARTE_KBN_MST", x => new { x.HPID, x.KARTEKBN });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_CENTER_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    CENTERCD = table.Column<string>(name: "CENTER_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CENTERNAME = table.Column<string>(name: "CENTER_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    PRIMARYKBN = table.Column<int>(name: "PRIMARY_KBN", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_CENTER_MST", x => new { x.HPID, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    IRAICD = table.Column<long>(name: "IRAI_CD", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAIDATE = table.Column<int>(name: "IRAI_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    TOSEKIKBN = table.Column<int>(name: "TOSEKI_KBN", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    RESULTCHECK = table.Column<int>(name: "RESULT_CHECK", type: "integer", nullable: false),
                    CENTERCD = table.Column<string>(name: "CENTER_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    NYUBI = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    YOKETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    BILIRUBIN = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
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
                    table.PrimaryKey("PK_KENSA_INF", x => new { x.HPID, x.PTID, x.IRAICD });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_INF_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    IRAICD = table.Column<long>(name: "IRAI_CD", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAIDATE = table.Column<int>(name: "IRAI_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    RESULTVAL = table.Column<string>(name: "RESULT_VAL", type: "character varying(10)", maxLength: 10, nullable: true),
                    RESULTTYPE = table.Column<string>(name: "RESULT_TYPE", type: "character varying(1)", maxLength: 1, nullable: true),
                    ABNORMALKBN = table.Column<string>(name: "ABNORMAL_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CMTCD1 = table.Column<string>(name: "CMT_CD1", type: "character varying(3)", maxLength: 3, nullable: true),
                    CMTCD2 = table.Column<string>(name: "CMT_CD2", type: "character varying(3)", maxLength: 3, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_INF_DETAIL", x => new { x.HPID, x.PTID, x.IRAICD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_IRAI_LOG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    CENTERCD = table.Column<string>(name: "CENTER_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    IRAIDATE = table.Column<int>(name: "IRAI_DATE", type: "integer", nullable: false),
                    FROMDATE = table.Column<int>(name: "FROM_DATE", type: "integer", nullable: false),
                    TODATE = table.Column<int>(name: "TO_DATE", type: "integer", nullable: false),
                    IRAIFILE = table.Column<string>(name: "IRAI_FILE", type: "text", nullable: true),
                    IRAILIST = table.Column<byte[]>(name: "IRAI_LIST", type: "bytea", nullable: true),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_IRAI_LOG", x => new { x.HPID, x.CENTERCD, x.CREATEDATE });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    KENSAITEMSEQNO = table.Column<int>(name: "KENSA_ITEM_SEQ_NO", type: "integer", nullable: false),
                    CENTERCD = table.Column<string>(name: "CENTER_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    KENSANAME = table.Column<string>(name: "KENSA_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    KENSAKANA = table.Column<string>(name: "KENSA_KANA", type: "character varying(20)", maxLength: 20, nullable: true),
                    UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MATERIALCD = table.Column<int>(name: "MATERIAL_CD", type: "integer", nullable: false),
                    CONTAINERCD = table.Column<int>(name: "CONTAINER_CD", type: "integer", nullable: false),
                    MALESTD = table.Column<string>(name: "MALE_STD", type: "character varying(60)", maxLength: 60, nullable: true),
                    MALESTDLOW = table.Column<string>(name: "MALE_STD_LOW", type: "character varying(60)", maxLength: 60, nullable: true),
                    MALESTDHIGH = table.Column<string>(name: "MALE_STD_HIGH", type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALESTD = table.Column<string>(name: "FEMALE_STD", type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALESTDLOW = table.Column<string>(name: "FEMALE_STD_LOW", type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALESTDHIGH = table.Column<string>(name: "FEMALE_STD_HIGH", type: "character varying(60)", maxLength: 60, nullable: true),
                    FORMULA = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DIGIT = table.Column<int>(type: "integer", nullable: false),
                    OYAITEMCD = table.Column<string>(name: "OYA_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    OYAITEMSEQNO = table.Column<int>(name: "OYA_ITEM_SEQ_NO", type: "integer", nullable: false),
                    SORTNO = table.Column<long>(name: "SORT_NO", type: "bigint", nullable: false),
                    CENTERITEMCD1 = table.Column<string>(name: "CENTER_ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: true),
                    CENTERITEMCD2 = table.Column<string>(name: "CENTER_ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_MST", x => new { x.HPID, x.KENSAITEMCD, x.KENSAITEMSEQNO });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_STD_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    MALESTD = table.Column<string>(name: "MALE_STD", type: "character varying(60)", maxLength: 60, nullable: true),
                    MALESTDLOW = table.Column<string>(name: "MALE_STD_LOW", type: "character varying(60)", maxLength: 60, nullable: true),
                    MALESTDHIGH = table.Column<string>(name: "MALE_STD_HIGH", type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALESTD = table.Column<string>(name: "FEMALE_STD", type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALESTDLOW = table.Column<string>(name: "FEMALE_STD_LOW", type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALESTDHIGH = table.Column<string>(name: "FEMALE_STD_HIGH", type: "character varying(60)", maxLength: 60, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_STD_MST", x => new { x.HPID, x.KENSAITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "KINKI_MST",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ACD = table.Column<string>(name: "A_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    BCD = table.Column<string>(name: "B_CD", type: "character varying(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_KINKI_MST", x => new { x.HPID, x.ID, x.ACD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "KOGAKU_LIMIT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    AGEKBN = table.Column<int>(name: "AGE_KBN", type: "integer", nullable: false),
                    KOGAKUKBN = table.Column<int>(name: "KOGAKU_KBN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    INCOMEKBN = table.Column<string>(name: "INCOME_KBN", type: "character varying(20)", maxLength: 20, nullable: true),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    BASELIMIT = table.Column<int>(name: "BASE_LIMIT", type: "integer", nullable: false),
                    ADJUSTLIMIT = table.Column<int>(name: "ADJUST_LIMIT", type: "integer", nullable: false),
                    TASULIMIT = table.Column<int>(name: "TASU_LIMIT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOGAKU_LIMIT", x => new { x.HPID, x.AGEKBN, x.KOGAKUKBN, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "KOHI_PRIORITY",
                columns: table => new
                {
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    PRIORITYNO = table.Column<string>(name: "PRIORITY_NO", type: "character varying(5)", maxLength: 5, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOHI_PRIORITY", x => new { x.PREFNO, x.HOUBETU, x.PRIORITYNO });
                });

            migrationBuilder.CreateTable(
                name: "KOUI_HOUKATU_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    HOUKATUTERM = table.Column<int>(name: "HOUKATU_TERM", type: "integer", nullable: false),
                    KOUIFROM = table.Column<int>(name: "KOUI_FROM", type: "integer", nullable: false),
                    KOUITO = table.Column<int>(name: "KOUI_TO", type: "integer", nullable: false),
                    IGNORESANTEIKBN = table.Column<int>(name: "IGNORE_SANTEI_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOUI_HOUKATU_MST", x => new { x.HPID, x.ITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "KOUI_KBN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    KOUIKBNID = table.Column<int>(name: "KOUI_KBN_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    KOUIKBN1 = table.Column<int>(name: "KOUI_KBN1", type: "integer", nullable: false),
                    KOUIKBN2 = table.Column<int>(name: "KOUI_KBN2", type: "integer", nullable: false),
                    KOUIGRPNAME = table.Column<string>(name: "KOUI_GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    KOUINAME = table.Column<string>(name: "KOUI_NAME", type: "character varying(20)", maxLength: 20, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOUI_KBN_MST", x => new { x.HPID, x.KOUIKBNID });
                });

            migrationBuilder.CreateTable(
                name: "LIMIT_CNT_LIST_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    KOHIID = table.Column<int>(name: "KOHI_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SORTKEY = table.Column<string>(name: "SORT_KEY", type: "character varying(61)", maxLength: 61, nullable: true),
                    OYARAIINNO = table.Column<long>(name: "OYA_RAIIN_NO", type: "bigint", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_LIMIT_CNT_LIST_INF", x => new { x.HPID, x.PTID, x.KOHIID, x.SINDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "LIMIT_LIST_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    KOHIID = table.Column<int>(name: "KOHI_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SORTKEY = table.Column<string>(name: "SORT_KEY", type: "character varying(61)", maxLength: 61, nullable: true),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    FUTANGAKU = table.Column<int>(name: "FUTAN_GAKU", type: "integer", nullable: false),
                    TOTALGAKU = table.Column<int>(name: "TOTAL_GAKU", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_LIMIT_LIST_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LIST_SET_GENERATION_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_LIST_SET_GENERATION_MST", x => new { x.HPID, x.GENERATIONID });
                });

            migrationBuilder.CreateTable(
                name: "LIST_SET_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false),
                    SETID = table.Column<int>(name: "SET_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SETKBN = table.Column<int>(name: "SET_KBN", type: "integer", nullable: false),
                    LEVEL1 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL2 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL3 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL4 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL5 = table.Column<int>(type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SETNAME = table.Column<string>(name: "SET_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    ISTITLE = table.Column<int>(name: "IS_TITLE", type: "integer", nullable: false),
                    SELECTTYPE = table.Column<int>(name: "SELECT_TYPE", type: "integer", nullable: false),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNITSBT = table.Column<int>(name: "UNIT_SBT", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true),
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
                    table.PrimaryKey("PK_LIST_SET_MST", x => new { x.HPID, x.GENERATIONID, x.SETID });
                });

            migrationBuilder.CreateTable(
                name: "LOCK_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    FUNCTIONCD = table.Column<string>(name: "FUNCTION_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    SINDATE = table.Column<long>(name: "SIN_DATE", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    OYARAIINNO = table.Column<long>(name: "OYA_RAIIN_NO", type: "bigint", nullable: false),
                    MACHINE = table.Column<string>(type: "text", nullable: true),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    LOCKDATE = table.Column<DateTime>(name: "LOCK_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCK_INF", x => new { x.HPID, x.PTID, x.FUNCTIONCD, x.SINDATE, x.RAIINNO, x.OYARAIINNO });
                });

            migrationBuilder.CreateTable(
                name: "LOCK_MST",
                columns: table => new
                {
                    FUNCTIONCDA = table.Column<string>(name: "FUNCTION_CD_A", type: "character varying(8)", maxLength: 8, nullable: false),
                    FUNCTIONCDB = table.Column<string>(name: "FUNCTION_CD_B", type: "character varying(8)", maxLength: 8, nullable: false),
                    LOCKRANGE = table.Column<int>(name: "LOCK_RANGE", type: "integer", nullable: false),
                    LOCKLEVEL = table.Column<int>(name: "LOCK_LEVEL", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCK_MST", x => new { x.FUNCTIONCDA, x.FUNCTIONCDB });
                });

            migrationBuilder.CreateTable(
                name: "M01_KIJYO_CMT",
                columns: table => new
                {
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(6)", maxLength: 6, nullable: false),
                    CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M01_KIJYO_CMT", x => x.CMTCD);
                });

            migrationBuilder.CreateTable(
                name: "M01_KINKI",
                columns: table => new
                {
                    ACD = table.Column<string>(name: "A_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    BCD = table.Column<string>(name: "B_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(6)", maxLength: 6, nullable: false),
                    SAYOKIJYOCD = table.Column<string>(name: "SAYOKIJYO_CD", type: "character varying(6)", maxLength: 6, nullable: false),
                    KYODOCD = table.Column<string>(name: "KYODO_CD", type: "character varying(3)", maxLength: 3, nullable: true),
                    KYODO = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    DATAKBN = table.Column<string>(name: "DATA_KBN", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M01_KINKI", x => new { x.ACD, x.BCD, x.CMTCD, x.SAYOKIJYOCD });
                });

            migrationBuilder.CreateTable(
                name: "M01_KINKI_CMT",
                columns: table => new
                {
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(6)", maxLength: 6, nullable: false),
                    CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M01_KINKI_CMT", x => x.CMTCD);
                });

            migrationBuilder.CreateTable(
                name: "M10_DAY_LIMIT",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    LIMITDAY = table.Column<int>(name: "LIMIT_DAY", type: "integer", nullable: false),
                    STDATE = table.Column<string>(name: "ST_DATE", type: "character varying(8)", maxLength: 8, nullable: true),
                    EDDATE = table.Column<string>(name: "ED_DATE", type: "character varying(8)", maxLength: 8, nullable: true),
                    CMT = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M10_DAY_LIMIT", x => new { x.YJCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M12_FOOD_ALRGY",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    FOODKBN = table.Column<string>(name: "FOOD_KBN", type: "character varying(2)", maxLength: 2, nullable: false),
                    TENPULEVEL = table.Column<string>(name: "TENPU_LEVEL", type: "character varying(2)", maxLength: 2, nullable: false),
                    KIKINCD = table.Column<string>(name: "KIKIN_CD", type: "character varying(9)", maxLength: 9, nullable: true),
                    ATTENTIONCMT = table.Column<string>(name: "ATTENTION_CMT", type: "character varying(500)", maxLength: 500, nullable: true),
                    WORKINGMECHANISM = table.Column<string>(name: "WORKING_MECHANISM", type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M12_FOOD_ALRGY", x => new { x.YJCD, x.FOODKBN, x.TENPULEVEL });
                });

            migrationBuilder.CreateTable(
                name: "M12_FOOD_ALRGY_KBN",
                columns: table => new
                {
                    FOODKBN = table.Column<string>(name: "FOOD_KBN", type: "character varying(2)", maxLength: 2, nullable: false),
                    FOODNAME = table.Column<string>(name: "FOOD_NAME", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M12_FOOD_ALRGY_KBN", x => x.FOODKBN);
                });

            migrationBuilder.CreateTable(
                name: "M14_AGE_CHECK",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    ATTENTIONCMTCD = table.Column<string>(name: "ATTENTION_CMT_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    WORKINGMECHANISM = table.Column<string>(name: "WORKING_MECHANISM", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TENPULEVEL = table.Column<string>(name: "TENPU_LEVEL", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKBN = table.Column<string>(name: "AGE_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    WEIGHTKBN = table.Column<string>(name: "WEIGHT_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    SEXKBN = table.Column<string>(name: "SEX_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    AGEMIN = table.Column<double>(name: "AGE_MIN", type: "double precision", nullable: false),
                    AGEMAX = table.Column<double>(name: "AGE_MAX", type: "double precision", nullable: false),
                    WEIGHTMIN = table.Column<double>(name: "WEIGHT_MIN", type: "double precision", nullable: false),
                    WEIGHTMAX = table.Column<double>(name: "WEIGHT_MAX", type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M14_AGE_CHECK", x => new { x.YJCD, x.ATTENTIONCMTCD });
                });

            migrationBuilder.CreateTable(
                name: "M14_CMT_CODE",
                columns: table => new
                {
                    ATTENTIONCMTCD = table.Column<string>(name: "ATTENTION_CMT_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    ATTENTIONCMT = table.Column<string>(name: "ATTENTION_CMT", type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M14_CMT_CODE", x => x.ATTENTIONCMTCD);
                });

            migrationBuilder.CreateTable(
                name: "M28_DRUG_MST",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    KOSEISYOCD = table.Column<string>(name: "KOSEISYO_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    KIKINCD = table.Column<string>(name: "KIKIN_CD", type: "character varying(9)", maxLength: 9, nullable: true),
                    DRUGNAME = table.Column<string>(name: "DRUG_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    DRUGKANA1 = table.Column<string>(name: "DRUG_KANA1", type: "character varying(100)", maxLength: 100, nullable: true),
                    DRUGKANA2 = table.Column<string>(name: "DRUG_KANA2", type: "character varying(100)", maxLength: 100, nullable: true),
                    IPNNAME = table.Column<string>(name: "IPN_NAME", type: "character varying(400)", maxLength: 400, nullable: true),
                    IPNKANA = table.Column<string>(name: "IPN_KANA", type: "character varying(100)", maxLength: 100, nullable: true),
                    YAKKAVAL = table.Column<int>(name: "YAKKA_VAL", type: "integer", nullable: false),
                    YAKKAUNIT = table.Column<string>(name: "YAKKA_UNIT", type: "character varying(20)", maxLength: 20, nullable: true),
                    SEIBUNRIKIKA = table.Column<double>(name: "SEIBUN_RIKIKA", type: "double precision", nullable: false),
                    SEIBUNRIKIKAUNIT = table.Column<string>(name: "SEIBUN_RIKIKA_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    YORYOJYURYO = table.Column<double>(name: "YORYO_JYURYO", type: "double precision", nullable: false),
                    YORYOJYURYOUNIT = table.Column<string>(name: "YORYO_JYURYO_UNIT", type: "character varying(20)", maxLength: 20, nullable: true),
                    SEIRIKIYORYORATE = table.Column<double>(name: "SEIRIKI_YORYO_RATE", type: "double precision", nullable: false),
                    SEIRIKIYORYOUNIT = table.Column<string>(name: "SEIRIKI_YORYO_UNIT", type: "character varying(40)", maxLength: 40, nullable: true),
                    MAKERCD = table.Column<string>(name: "MAKER_CD", type: "character varying(4)", maxLength: 4, nullable: true),
                    MAKERNAME = table.Column<string>(name: "MAKER_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    DRUGKBNCD = table.Column<string>(name: "DRUG_KBN_CD", type: "character varying(1)", maxLength: 1, nullable: true),
                    DRUGKBN = table.Column<string>(name: "DRUG_KBN", type: "character varying(10)", maxLength: 10, nullable: true),
                    FORMKBNCD = table.Column<string>(name: "FORM_KBN_CD", type: "character varying(3)", maxLength: 3, nullable: true),
                    FORMKBN = table.Column<string>(name: "FORM_KBN", type: "character varying(100)", maxLength: 100, nullable: true),
                    DOKUYAKUFLG = table.Column<string>(name: "DOKUYAKU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    GEKIYAKUFLG = table.Column<string>(name: "GEKIYAKU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    MAYAKUFLG = table.Column<string>(name: "MAYAKU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    KOSEISINYAKUFLG = table.Column<string>(name: "KOSEISINYAKU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    KAKUSEIZAIFLG = table.Column<string>(name: "KAKUSEIZAI_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    KAKUSEIZAIGENRYOFLG = table.Column<string>(name: "KAKUSEIZAI_GENRYO_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    SEIBUTUFLG = table.Column<string>(name: "SEIBUTU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    SPSEIBUTUFLG = table.Column<string>(name: "SP_SEIBUTU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    KOHATUFLG = table.Column<string>(name: "KOHATU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    YAKKA = table.Column<double>(type: "double precision", nullable: false),
                    KIKAKUUNIT = table.Column<string>(name: "KIKAKU_UNIT", type: "character varying(100)", maxLength: 100, nullable: true),
                    YAKKARATEFORMURA = table.Column<string>(name: "YAKKA_RATE_FORMURA", type: "character varying(30)", maxLength: 30, nullable: true),
                    YAKKARATEUNIT = table.Column<string>(name: "YAKKA_RATE_UNIT", type: "character varying(40)", maxLength: 40, nullable: true),
                    YAKKASYUSAIDATE = table.Column<string>(name: "YAKKA_SYUSAI_DATE", type: "character varying(8)", maxLength: 8, nullable: true),
                    KEIKASOTIDATE = table.Column<string>(name: "KEIKASOTI_DATE", type: "character varying(8)", maxLength: 8, nullable: true),
                    MAINDRUGCD = table.Column<string>(name: "MAIN_DRUG_CD", type: "character varying(8)", maxLength: 8, nullable: true),
                    MAINDRUGNAME = table.Column<string>(name: "MAIN_DRUG_NAME", type: "character varying(400)", maxLength: 400, nullable: true),
                    MAINDRUGKANA = table.Column<string>(name: "MAIN_DRUG_KANA", type: "character varying(400)", maxLength: 400, nullable: true),
                    KEYSEIBUN = table.Column<string>(name: "KEY_SEIBUN", type: "character varying(200)", maxLength: 200, nullable: true),
                    HAIGOFLG = table.Column<string>(name: "HAIGO_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    MAINDRUGNAMEFLG = table.Column<string>(name: "MAIN_DRUG_NAME_FLG", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M28_DRUG_MST", x => x.YJCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_AR_CODE",
                columns: table => new
                {
                    FUKUSAYOCD = table.Column<string>(name: "FUKUSAYO_CD", type: "text", nullable: false),
                    FUKUSAYOCMT = table.Column<string>(name: "FUKUSAYO_CMT", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_AR_CODE", x => x.FUKUSAYOCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_AR_DISCON",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "text", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    FUKUSAYOCD = table.Column<string>(name: "FUKUSAYO_CD", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_AR_DISCON", x => new { x.YJCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M34_AR_DISCON_CODE",
                columns: table => new
                {
                    FUKUSAYOCD = table.Column<string>(name: "FUKUSAYO_CD", type: "character varying(6)", maxLength: 6, nullable: false),
                    FUKUSAYOCMT = table.Column<string>(name: "FUKUSAYO_CMT", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_AR_DISCON_CODE", x => x.FUKUSAYOCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_DRUG_INFO_MAIN",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "text", nullable: false),
                    FORMCD = table.Column<string>(name: "FORM_CD", type: "text", nullable: true),
                    COLOR = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MARK = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    KONOCD = table.Column<string>(name: "KONO_CD", type: "text", nullable: true),
                    FUKUSAYOCD = table.Column<string>(name: "FUKUSAYO_CD", type: "text", nullable: true),
                    FUKUSAYOINITCD = table.Column<string>(name: "FUKUSAYO_INIT_CD", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_DRUG_INFO_MAIN", x => x.YJCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_FORM_CODE",
                columns: table => new
                {
                    FORMCD = table.Column<string>(name: "FORM_CD", type: "character varying(4)", maxLength: 4, nullable: false),
                    FORM = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_FORM_CODE", x => x.FORMCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_INDICATION_CODE",
                columns: table => new
                {
                    KONOCD = table.Column<string>(name: "KONO_CD", type: "text", nullable: false),
                    KONODETAILCMT = table.Column<string>(name: "KONO_DETAIL_CMT", type: "character varying(200)", maxLength: 200, nullable: true),
                    KONOSIMPLECMT = table.Column<string>(name: "KONO_SIMPLE_CMT", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_INDICATION_CODE", x => x.KONOCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_INTERACTION_PAT",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "text", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    INTERACTIONPATCD = table.Column<string>(name: "INTERACTION_PAT_CD", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_INTERACTION_PAT", x => new { x.YJCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M34_INTERACTION_PAT_CODE",
                columns: table => new
                {
                    INTERACTIONPATCD = table.Column<string>(name: "INTERACTION_PAT_CD", type: "text", nullable: false),
                    INTERACTIONPATCMT = table.Column<string>(name: "INTERACTION_PAT_CMT", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_INTERACTION_PAT_CODE", x => x.INTERACTIONPATCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_PRECAUTION_CODE",
                columns: table => new
                {
                    PRECAUTIONCD = table.Column<string>(name: "PRECAUTION_CD", type: "text", nullable: false),
                    EXTENDCD = table.Column<string>(name: "EXTEND_CD", type: "text", nullable: false),
                    PRECAUTIONCMT = table.Column<string>(name: "PRECAUTION_CMT", type: "character varying(200)", maxLength: 200, nullable: true),
                    PROPERTYCD = table.Column<int>(name: "PROPERTY_CD", type: "integer", nullable: false),
                    AGEMAX = table.Column<int>(name: "AGE_MAX", type: "integer", nullable: false),
                    AGEMIN = table.Column<int>(name: "AGE_MIN", type: "integer", nullable: false),
                    SEXCD = table.Column<string>(name: "SEX_CD", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_PRECAUTION_CODE", x => new { x.PRECAUTIONCD, x.EXTENDCD });
                });

            migrationBuilder.CreateTable(
                name: "M34_PRECAUTIONS",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "text", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    PRECAUTIONCD = table.Column<string>(name: "PRECAUTION_CD", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_PRECAUTIONS", x => new { x.YJCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M34_PROPERTY_CODE",
                columns: table => new
                {
                    PROPERTYCD = table.Column<int>(name: "PROPERTY_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PROPERTY = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_PROPERTY_CODE", x => x.PROPERTYCD);
                });

            migrationBuilder.CreateTable(
                name: "M34_SAR_SYMPTOM_CODE",
                columns: table => new
                {
                    FUKUSAYOINITCD = table.Column<string>(name: "FUKUSAYO_INIT_CD", type: "character varying(6)", maxLength: 6, nullable: false),
                    FUKUSAYOINITCMT = table.Column<string>(name: "FUKUSAYO_INIT_CMT", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_SAR_SYMPTOM_CODE", x => x.FUKUSAYOINITCD);
                });

            migrationBuilder.CreateTable(
                name: "M38_CLASS_CODE",
                columns: table => new
                {
                    CLASSCD = table.Column<string>(name: "CLASS_CD", type: "text", nullable: false),
                    CLASSNAME = table.Column<string>(name: "CLASS_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    MAJORDIVCD = table.Column<string>(name: "MAJOR_DIV_CD", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_CLASS_CODE", x => x.CLASSCD);
                });

            migrationBuilder.CreateTable(
                name: "M38_ING_CODE",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    SEIBUN = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_ING_CODE", x => x.SEIBUNCD);
                });

            migrationBuilder.CreateTable(
                name: "M38_INGREDIENTS",
                columns: table => new
                {
                    SERIALNUM = table.Column<int>(name: "SERIAL_NUM", type: "integer", nullable: false),
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    SBT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_INGREDIENTS", x => new { x.SEIBUNCD, x.SERIALNUM, x.SBT });
                });

            migrationBuilder.CreateTable(
                name: "M38_MAJOR_DIV_CODE",
                columns: table => new
                {
                    MAJORDIVCD = table.Column<string>(name: "MAJOR_DIV_CD", type: "text", nullable: false),
                    MAJORDIVNAME = table.Column<string>(name: "MAJOR_DIV_NAME", type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_MAJOR_DIV_CODE", x => x.MAJORDIVCD);
                });

            migrationBuilder.CreateTable(
                name: "M38_OTC_FORM_CODE",
                columns: table => new
                {
                    FORMCD = table.Column<string>(name: "FORM_CD", type: "text", nullable: false),
                    FORM = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_OTC_FORM_CODE", x => x.FORMCD);
                });

            migrationBuilder.CreateTable(
                name: "M38_OTC_MAIN",
                columns: table => new
                {
                    SERIALNUM = table.Column<int>(name: "SERIAL_NUM", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OTCCD = table.Column<string>(name: "OTC_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    TRADENAME = table.Column<string>(name: "TRADE_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    TRADEKANA = table.Column<string>(name: "TRADE_KANA", type: "character varying(400)", maxLength: 400, nullable: true),
                    CLASSCD = table.Column<string>(name: "CLASS_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    COMPANYCD = table.Column<string>(name: "COMPANY_CD", type: "character varying(4)", maxLength: 4, nullable: true),
                    TRADECD = table.Column<string>(name: "TRADE_CD", type: "character varying(3)", maxLength: 3, nullable: true),
                    DRUGFORMCD = table.Column<string>(name: "DRUG_FORM_CD", type: "character varying(6)", maxLength: 6, nullable: true),
                    YOHOCD = table.Column<string>(name: "YOHO_CD", type: "character varying(6)", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_OTC_MAIN", x => x.SERIALNUM);
                });

            migrationBuilder.CreateTable(
                name: "M38_OTC_MAKER_CODE",
                columns: table => new
                {
                    MAKERCD = table.Column<string>(name: "MAKER_CD", type: "text", nullable: false),
                    MAKERNAME = table.Column<string>(name: "MAKER_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    MAKERKANA = table.Column<string>(name: "MAKER_KANA", type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_OTC_MAKER_CODE", x => x.MAKERCD);
                });

            migrationBuilder.CreateTable(
                name: "M41_SUPPLE_INDEXCODE",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    INDEXCD = table.Column<string>(name: "INDEX_CD", type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M41_SUPPLE_INDEXCODE", x => new { x.SEIBUNCD, x.INDEXCD });
                });

            migrationBuilder.CreateTable(
                name: "M41_SUPPLE_INDEXDEF",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    INDEXWORD = table.Column<string>(name: "INDEX_WORD", type: "character varying(200)", maxLength: 200, nullable: true),
                    TOKUHOFLG = table.Column<string>(name: "TOKUHO_FLG", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M41_SUPPLE_INDEXDEF", x => x.SEIBUNCD);
                });

            migrationBuilder.CreateTable(
                name: "M41_SUPPLE_INGRE",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    SEIBUN = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M41_SUPPLE_INGRE", x => x.SEIBUNCD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRA_CMT",
                columns: table => new
                {
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    CMT = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRA_CMT", x => x.CMTCD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DIS_BC",
                columns: table => new
                {
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOTAICLASSCD = table.Column<string>(name: "BYOTAI_CLASS_CD", type: "character varying(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DIS_BC", x => new { x.BYOTAICD, x.BYOTAICLASSCD });
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DIS_CLASS",
                columns: table => new
                {
                    BYOTAICLASSCD = table.Column<string>(name: "BYOTAI_CLASS_CD", type: "character varying(4)", maxLength: 4, nullable: false),
                    BYOTAI = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DIS_CLASS", x => x.BYOTAICLASSCD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DIS_CON",
                columns: table => new
                {
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    STANDARDBYOTAI = table.Column<string>(name: "STANDARD_BYOTAI", type: "character varying(400)", maxLength: 400, nullable: true),
                    BYOTAIKBN = table.Column<int>(name: "BYOTAI_KBN", type: "integer", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    ICD10 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    RECECD = table.Column<string>(name: "RECE_CD", type: "character varying(33)", maxLength: 33, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DIS_CON", x => x.BYOTAICD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DRUG_MAIN_EX",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    TENPULEVEL = table.Column<int>(name: "TENPU_LEVEL", type: "integer", nullable: false),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    STAGE = table.Column<int>(type: "integer", nullable: false),
                    KIOCD = table.Column<string>(name: "KIO_CD", type: "character varying(1)", maxLength: 1, nullable: true),
                    FAMILYCD = table.Column<string>(name: "FAMILY_CD", type: "character varying(1)", maxLength: 1, nullable: true),
                    KIJYOCD = table.Column<string>(name: "KIJYO_CD", type: "character varying(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DRUG_MAIN_EX", x => new { x.YJCD, x.TENPULEVEL, x.BYOTAICD, x.CMTCD });
                });

            migrationBuilder.CreateTable(
                name: "M46_DOSAGE_DOSAGE",
                columns: table => new
                {
                    DOEICD = table.Column<string>(name: "DOEI_CD", type: "text", nullable: false),
                    DOEISEQNO = table.Column<int>(name: "DOEI_SEQ_NO", type: "integer", nullable: false),
                    KONOKOKACD = table.Column<string>(name: "KONOKOKA_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    KENSAPCD = table.Column<string>(name: "KENSA_PCD", type: "character varying(7)", maxLength: 7, nullable: true),
                    AGEOVER = table.Column<double>(name: "AGE_OVER", type: "double precision", nullable: false),
                    AGEUNDER = table.Column<double>(name: "AGE_UNDER", type: "double precision", nullable: false),
                    AGECD = table.Column<string>(name: "AGE_CD", type: "character varying(1)", maxLength: 1, nullable: true),
                    WEIGHTOVER = table.Column<double>(name: "WEIGHT_OVER", type: "double precision", nullable: false),
                    WEIGHTUNDER = table.Column<double>(name: "WEIGHT_UNDER", type: "double precision", nullable: false),
                    BODYOVER = table.Column<double>(name: "BODY_OVER", type: "double precision", nullable: false),
                    BODYUNDER = table.Column<double>(name: "BODY_UNDER", type: "double precision", nullable: false),
                    DRUGROUTE = table.Column<string>(name: "DRUG_ROUTE", type: "character varying(40)", maxLength: 40, nullable: true),
                    USEFLG = table.Column<string>(name: "USE_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    DRUGCONDITION = table.Column<string>(name: "DRUG_CONDITION", type: "character varying(400)", maxLength: 400, nullable: true),
                    KONOKOKA = table.Column<string>(type: "text", nullable: true),
                    USAGEDOSAGE = table.Column<string>(name: "USAGE_DOSAGE", type: "text", nullable: true),
                    FILENAMECD = table.Column<string>(name: "FILENAME_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    DRUGSYUGI = table.Column<string>(name: "DRUG_SYUGI", type: "text", nullable: true),
                    TEKIOBUI = table.Column<string>(name: "TEKIO_BUI", type: "character varying(300)", maxLength: 300, nullable: true),
                    YOUKAIKISYAKU = table.Column<string>(name: "YOUKAI_KISYAKU", type: "character varying(1500)", maxLength: 1500, nullable: true),
                    KISYAKUEKI = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    YOUKAIEKI = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HAITAFLG = table.Column<string>(name: "HAITA_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    NGKISYAKUEKI = table.Column<string>(name: "NG_KISYAKUEKI", type: "character varying(500)", maxLength: 500, nullable: true),
                    NGYOUKAIEKI = table.Column<string>(name: "NG_YOUKAIEKI", type: "character varying(500)", maxLength: 500, nullable: true),
                    COMBIDRUG = table.Column<string>(name: "COMBI_DRUG", type: "character varying(200)", maxLength: 200, nullable: true),
                    DRUGLINKCD = table.Column<int>(name: "DRUG_LINK_CD", type: "integer", nullable: false),
                    DRUGORDER = table.Column<int>(name: "DRUG_ORDER", type: "integer", nullable: false),
                    SINGLEDRUGFLG = table.Column<string>(name: "SINGLE_DRUG_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    KYUGENCD = table.Column<string>(name: "KYUGEN_CD", type: "character varying(1)", maxLength: 1, nullable: true),
                    DOSAGECHECKFLG = table.Column<string>(name: "DOSAGE_CHECK_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    ONCEMIN = table.Column<double>(name: "ONCE_MIN", type: "double precision", nullable: false),
                    ONCEMAX = table.Column<double>(name: "ONCE_MAX", type: "double precision", nullable: false),
                    ONCEUNIT = table.Column<string>(name: "ONCE_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    ONCELIMIT = table.Column<double>(name: "ONCE_LIMIT", type: "double precision", nullable: false),
                    ONCELIMITUNIT = table.Column<string>(name: "ONCE_LIMIT_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    DAYMINCNT = table.Column<double>(name: "DAY_MIN_CNT", type: "double precision", nullable: false),
                    DAYMAXCNT = table.Column<double>(name: "DAY_MAX_CNT", type: "double precision", nullable: false),
                    DAYMIN = table.Column<double>(name: "DAY_MIN", type: "double precision", nullable: false),
                    DAYMAX = table.Column<double>(name: "DAY_MAX", type: "double precision", nullable: false),
                    DAYUNIT = table.Column<string>(name: "DAY_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    DAYLIMIT = table.Column<double>(name: "DAY_LIMIT", type: "double precision", nullable: false),
                    DAYLIMITUNIT = table.Column<string>(name: "DAY_LIMIT_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    RISE = table.Column<int>(type: "integer", nullable: false),
                    MORNING = table.Column<int>(type: "integer", nullable: false),
                    DAYTIME = table.Column<int>(type: "integer", nullable: false),
                    NIGHT = table.Column<int>(type: "integer", nullable: false),
                    SLEEP = table.Column<int>(type: "integer", nullable: false),
                    BEFOREMEAL = table.Column<int>(name: "BEFORE_MEAL", type: "integer", nullable: false),
                    JUSTBEFOREMEAL = table.Column<int>(name: "JUST_BEFORE_MEAL", type: "integer", nullable: false),
                    AFTERMEAL = table.Column<int>(name: "AFTER_MEAL", type: "integer", nullable: false),
                    JUSTAFTERMEAL = table.Column<int>(name: "JUST_AFTER_MEAL", type: "integer", nullable: false),
                    BETWEENMEAL = table.Column<int>(name: "BETWEEN_MEAL", type: "integer", nullable: false),
                    ELSETIME = table.Column<int>(name: "ELSE_TIME", type: "integer", nullable: false),
                    DOSAGELIMITTERM = table.Column<int>(name: "DOSAGE_LIMIT_TERM", type: "integer", nullable: false),
                    DOSAGELIMITUNIT = table.Column<string>(name: "DOSAGE_LIMIT_UNIT", type: "character varying(1)", maxLength: 1, nullable: true),
                    UNITTERMLIMIT = table.Column<double>(name: "UNITTERM_LIMIT", type: "double precision", nullable: false),
                    UNITTERMUNIT = table.Column<string>(name: "UNITTERM_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    DOSAGEADDFLG = table.Column<string>(name: "DOSAGE_ADD_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    INCDECFLG = table.Column<string>(name: "INC_DEC_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    DECFLG = table.Column<string>(name: "DEC_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    INCDECINTERVAL = table.Column<int>(name: "INC_DEC_INTERVAL", type: "integer", nullable: false),
                    INCDECINTERVALUNIT = table.Column<string>(name: "INC_DEC_INTERVAL_UNIT", type: "character varying(1)", maxLength: 1, nullable: true),
                    DECLIMIT = table.Column<double>(name: "DEC_LIMIT", type: "double precision", nullable: false),
                    INCLIMIT = table.Column<double>(name: "INC_LIMIT", type: "double precision", nullable: false),
                    INCDECLIMITUNIT = table.Column<string>(name: "INC_DEC_LIMIT_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    TIMEDEPEND = table.Column<string>(name: "TIME_DEPEND", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    JUDGETERM = table.Column<int>(name: "JUDGE_TERM", type: "integer", nullable: false),
                    JUDGETERMUNIT = table.Column<string>(name: "JUDGE_TERM_UNIT", type: "character varying(1)", maxLength: 1, nullable: true),
                    EXTENDFLG = table.Column<string>(name: "EXTEND_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    ADDTERM = table.Column<int>(name: "ADD_TERM", type: "integer", nullable: false),
                    ADDTERMUNIT = table.Column<string>(name: "ADD_TERM_UNIT", type: "character varying(1)", maxLength: 1, nullable: true),
                    INTERVALWARNINGFLG = table.Column<string>(name: "INTERVAL_WARNING_FLG", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M46_DOSAGE_DOSAGE", x => new { x.DOEICD, x.DOEISEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M46_DOSAGE_DRUG",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    DOEICD = table.Column<string>(name: "DOEI_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    DRUGKBN = table.Column<string>(name: "DRUG_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    KIKAKUUNIT = table.Column<string>(name: "KIKAKU_UNIT", type: "character varying(100)", maxLength: 100, nullable: true),
                    YAKKAUNIT = table.Column<string>(name: "YAKKA_UNIT", type: "character varying(20)", maxLength: 20, nullable: true),
                    RIKIKARATE = table.Column<decimal>(name: "RIKIKA_RATE", type: "numeric", nullable: false),
                    RIKIKAUNIT = table.Column<string>(name: "RIKIKA_UNIT", type: "character varying(30)", maxLength: 30, nullable: true),
                    YOUKAIEKICD = table.Column<string>(name: "YOUKAIEKI_CD", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M46_DOSAGE_DRUG", x => new { x.DOEICD, x.YJCD });
                });

            migrationBuilder.CreateTable(
                name: "M56_ALRGY_DERIVATIVES",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    DRVALRGYCD = table.Column<string>(name: "DRVALRGY_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_ALRGY_DERIVATIVES", x => new { x.SEIBUNCD, x.DRVALRGYCD, x.YJCD });
                });

            migrationBuilder.CreateTable(
                name: "M56_ANALOGUE_CD",
                columns: table => new
                {
                    ANALOGUECD = table.Column<string>(name: "ANALOGUE_CD", type: "character varying(9)", maxLength: 9, nullable: false),
                    ANALOGUENAME = table.Column<string>(name: "ANALOGUE_NAME", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_ANALOGUE_CD", x => x.ANALOGUECD);
                });

            migrationBuilder.CreateTable(
                name: "M56_DRUG_CLASS",
                columns: table => new
                {
                    CLASSCD = table.Column<string>(name: "CLASS_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    CLASSNAME = table.Column<string>(name: "CLASS_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    CLASSDUPLICATION = table.Column<string>(name: "CLASS_DUPLICATION", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_DRUG_CLASS", x => x.CLASSCD);
                });

            migrationBuilder.CreateTable(
                name: "M56_DRVALRGY_CODE",
                columns: table => new
                {
                    DRVALRGYCD = table.Column<string>(name: "DRVALRGY_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    DRVALRGYNAME = table.Column<string>(name: "DRVALRGY_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    DRVALRGYGRP = table.Column<string>(name: "DRVALRGY_GRP", type: "character varying(4)", maxLength: 4, nullable: true),
                    RANKNO = table.Column<int>(name: "RANK_NO", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_DRVALRGY_CODE", x => x.DRVALRGYCD);
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_ANALOGUE",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(9)", maxLength: 9, nullable: false),
                    SEQNO = table.Column<string>(name: "SEQ_NO", type: "character varying(2)", maxLength: 2, nullable: false),
                    ANALOGUECD = table.Column<string>(name: "ANALOGUE_CD", type: "character varying(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_ANALOGUE", x => new { x.SEIBUNCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_ED_INGREDIENTS",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    SEQNO = table.Column<string>(name: "SEQ_NO", type: "character varying(3)", maxLength: 3, nullable: false),
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(9)", maxLength: 9, nullable: true),
                    SEIBUNINDEXCD = table.Column<string>(name: "SEIBUN_INDEX_CD", type: "character varying(3)", maxLength: 3, nullable: true),
                    SBT = table.Column<int>(type: "integer", nullable: false),
                    PRODRUGCHECK = table.Column<string>(name: "PRODRUG_CHECK", type: "character varying(1)", maxLength: 1, nullable: true),
                    ANALOGUECHECK = table.Column<string>(name: "ANALOGUE_CHECK", type: "character varying(1)", maxLength: 1, nullable: true),
                    YOKAIEKICHECK = table.Column<string>(name: "YOKAIEKI_CHECK", type: "character varying(1)", maxLength: 1, nullable: true),
                    TENKABUTUCHECK = table.Column<string>(name: "TENKABUTU_CHECK", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_ED_INGREDIENTS", x => new { x.YJCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_ING_CODE",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(9)", maxLength: 9, nullable: false),
                    SEIBUNINDEXCD = table.Column<string>(name: "SEIBUN_INDEX_CD", type: "character varying(3)", maxLength: 3, nullable: false),
                    SEIBUNNAME = table.Column<string>(name: "SEIBUN_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    YOHOCD = table.Column<string>(name: "YOHO_CD", type: "character varying(6)", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_ING_CODE", x => new { x.SEIBUNCD, x.SEIBUNINDEXCD });
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_INGRDT_MAIN",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    DRUGKBN = table.Column<string>(name: "DRUG_KBN", type: "character varying(2)", maxLength: 2, nullable: true),
                    YOHOCD = table.Column<string>(name: "YOHO_CD", type: "character varying(6)", maxLength: 6, nullable: true),
                    HAIGOUFLG = table.Column<string>(name: "HAIGOU_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    YUEKIFLG = table.Column<string>(name: "YUEKI_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    KANPOFLG = table.Column<string>(name: "KANPO_FLG", type: "character varying(1)", maxLength: 1, nullable: true),
                    ZENSINSAYOFLG = table.Column<string>(name: "ZENSINSAYO_FLG", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_INGRDT_MAIN", x => x.YJCD);
                });

            migrationBuilder.CreateTable(
                name: "M56_PRODRUG_CD",
                columns: table => new
                {
                    SEIBUNCD = table.Column<string>(name: "SEIBUN_CD", type: "character varying(9)", maxLength: 9, nullable: false),
                    SEQNO = table.Column<string>(name: "SEQ_NO", type: "character varying(2)", maxLength: 2, nullable: false),
                    KASSEITAICD = table.Column<string>(name: "KASSEITAI_CD", type: "character varying(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_PRODRUG_CD", x => new { x.SEQNO, x.SEIBUNCD });
                });

            migrationBuilder.CreateTable(
                name: "M56_USAGE_CODE",
                columns: table => new
                {
                    YOHOCD = table.Column<string>(name: "YOHO_CD", type: "text", nullable: false),
                    YOHO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_USAGE_CODE", x => x.YOHOCD);
                });

            migrationBuilder.CreateTable(
                name: "M56_YJ_DRUG_CLASS",
                columns: table => new
                {
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    CLASSCD = table.Column<string>(name: "CLASS_CD", type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_YJ_DRUG_CLASS", x => new { x.YJCD, x.CLASSCD });
                });

            migrationBuilder.CreateTable(
                name: "MALL_MESSAGE_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RECEIVENO = table.Column<int>(name: "RECEIVE_NO", type: "integer", nullable: false),
                    SENDNO = table.Column<int>(name: "SEND_NO", type: "integer", nullable: false),
                    MESSAGE = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MALL_MESSAGE_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MALL_RENKEI_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SINSATUNO = table.Column<int>(name: "SINSATU_NO", type: "integer", nullable: false),
                    KAIKEINO = table.Column<int>(name: "KAIKEI_NO", type: "integer", nullable: false),
                    RECEIVENO = table.Column<int>(name: "RECEIVE_NO", type: "integer", nullable: false),
                    SENDNO = table.Column<int>(name: "SEND_NO", type: "integer", nullable: false),
                    SENDFLG = table.Column<int>(name: "SEND_FLG", type: "integer", nullable: false),
                    CLINICCD = table.Column<int>(name: "CLINIC_CD", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MALL_RENKEI_INF", x => new { x.HPID, x.RAIINNO });
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MATERIALCD = table.Column<long>(name: "MATERIAL_CD", type: "bigint", nullable: false),
                    MATERIALNAME = table.Column<string>(name: "MATERIAL_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_MST", x => new { x.HPID, x.MATERIALCD });
                });

            migrationBuilder.CreateTable(
                name: "MONSHIN_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RTEXT = table.Column<string>(type: "text", nullable: true),
                    GETKBN = table.Column<int>(name: "GET_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_MONSHIN_INF", x => new { x.HPID, x.PTID, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "ODR_DATE_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_DATE_DETAIL", x => new { x.HPID, x.GRPID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "ODR_DATE_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    GRPNAME = table.Column<string>(name: "GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_ODR_DATE_INF", x => new { x.HPID, x.GRPID });
                });

            migrationBuilder.CreateTable(
                name: "ODR_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    ODRKOUIKBN = table.Column<int>(name: "ODR_KOUI_KBN", type: "integer", nullable: false),
                    RPNAME = table.Column<string>(name: "RP_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    SYOHOSBT = table.Column<int>(name: "SYOHO_SBT", type: "integer", nullable: false),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
                    TOSEKIKBN = table.Column<int>(name: "TOSEKI_KBN", type: "integer", nullable: false),
                    DAYSCNT = table.Column<int>(name: "DAYS_CNT", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ODR_INF", x => new { x.HPID, x.RAIINNO, x.RPNO, x.RPEDANO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "ODR_INF_CMT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    FONTCOLOR = table.Column<int>(name: "FONT_COLOR", type: "integer", nullable: false),
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(32)", maxLength: 32, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_INF_CMT", x => new { x.HPID, x.RAIINNO, x.RPNO, x.RPEDANO, x.ROWNO, x.EDANO });
                });

            migrationBuilder.CreateTable(
                name: "ODR_INF_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    UNITSBT = table.Column<int>(name: "UNIT_SBT", type: "integer", nullable: false),
                    TERMVAL = table.Column<double>(name: "TERM_VAL", type: "double precision", nullable: false),
                    KOHATUKBN = table.Column<int>(name: "KOHATU_KBN", type: "integer", nullable: false),
                    SYOHOKBN = table.Column<int>(name: "SYOHO_KBN", type: "integer", nullable: false),
                    SYOHOLIMITKBN = table.Column<int>(name: "SYOHO_LIMIT_KBN", type: "integer", nullable: false),
                    DRUGKBN = table.Column<int>(name: "DRUG_KBN", type: "integer", nullable: false),
                    YOHOKBN = table.Column<int>(name: "YOHO_KBN", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "text", nullable: true),
                    KOKUJI2 = table.Column<string>(type: "text", nullable: true),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    IPNCD = table.Column<string>(name: "IPN_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    IPNNAME = table.Column<string>(name: "IPN_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    JISSIKBN = table.Column<int>(name: "JISSI_KBN", type: "integer", nullable: false),
                    JISSIDATE = table.Column<DateTime>(name: "JISSI_DATE", type: "timestamp with time zone", nullable: true),
                    JISSIID = table.Column<int>(name: "JISSI_ID", type: "integer", nullable: false),
                    JISSIMACHINE = table.Column<string>(name: "JISSI_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    REQCD = table.Column<string>(name: "REQ_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true),
                    FONTCOLOR = table.Column<string>(name: "FONT_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    COMMENTNEWLINE = table.Column<int>(name: "COMMENT_NEWLINE", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_INF_DETAIL", x => new { x.HPID, x.RAIINNO, x.RPNO, x.RPEDANO, x.ROWNO });
                });

            migrationBuilder.CreateTable(
                name: "ONLINE_CONFIRMATION",
                columns: table => new
                {
                    RECEPTIONNO = table.Column<string>(name: "RECEPTION_NO", type: "text", nullable: false),
                    RECEPTIONDATETIME = table.Column<DateTime>(name: "RECEPTION_DATETIME", type: "timestamp with time zone", nullable: false),
                    YOYAKUDATE = table.Column<int>(name: "YOYAKU_DATE", type: "integer", nullable: false),
                    SEGMENTOFRESULT = table.Column<string>(name: "SEGMENT_OF_RESULT", type: "character varying(1)", maxLength: 1, nullable: true),
                    ERRORMESSAGE = table.Column<string>(name: "ERROR_MESSAGE", type: "character varying(60)", maxLength: 60, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONLINE_CONFIRMATION", x => x.RECEPTIONNO);
                });

            migrationBuilder.CreateTable(
                name: "ONLINE_CONFIRMATION_HISTORY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ONLINECONFIRMATIONDATE = table.Column<DateTime>(name: "ONLINE_CONFIRMATION_DATE", type: "timestamp with time zone", nullable: false),
                    CONFIRMATIONTYPE = table.Column<int>(name: "CONFIRMATION_TYPE", type: "integer", nullable: false),
                    CONFIRMATIONRESULT = table.Column<string>(name: "CONFIRMATION_RESULT", type: "text", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONLINE_CONFIRMATION_HISTORY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ONLINE_CONSENT",
                columns: table => new
                {
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    CONSKBN = table.Column<int>(name: "CONS_KBN", type: "integer", nullable: false),
                    CONSDATE = table.Column<DateTime>(name: "CONS_DATE", type: "timestamp with time zone", nullable: false),
                    LIMITDATE = table.Column<DateTime>(name: "LIMIT_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONLINE_CONSENT", x => new { x.PTID, x.CONSKBN });
                });

            migrationBuilder.CreateTable(
                name: "PATH_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    GRPEDANO = table.Column<int>(name: "GRP_EDA_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    PATH = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CHARCD = table.Column<int>(name: "CHAR_CD", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATH_CONF", x => new { x.HPID, x.GRPCD, x.GRPEDANO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PAYMENT_METHOD_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PAYMENTMETHODCD = table.Column<int>(name: "PAYMENT_METHOD_CD", type: "integer", nullable: false),
                    PAYNAME = table.Column<string>(name: "PAY_NAME", type: "character varying(60)", maxLength: 60, nullable: true),
                    PAYSNAME = table.Column<string>(name: "PAY_SNAME", type: "character varying(1)", maxLength: 1, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PAYMENT_METHOD_MST", x => new { x.HPID, x.PAYMENTMETHODCD });
                });

            migrationBuilder.CreateTable(
                name: "PERMISSION_MST",
                columns: table => new
                {
                    FUNCTIONCD = table.Column<string>(name: "FUNCTION_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    PERMISSION = table.Column<int>(type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSION_MST", x => new { x.FUNCTIONCD, x.PERMISSION });
                });

            migrationBuilder.CreateTable(
                name: "PHYSICAL_AVERAGE",
                columns: table => new
                {
                    JISSIYEAR = table.Column<int>(name: "JISSI_YEAR", type: "integer", nullable: false),
                    AGEYEAR = table.Column<int>(name: "AGE_YEAR", type: "integer", nullable: false),
                    AGEMONTH = table.Column<int>(name: "AGE_MONTH", type: "integer", nullable: false),
                    AGEDAY = table.Column<int>(name: "AGE_DAY", type: "integer", nullable: false),
                    MALEHEIGHT = table.Column<double>(name: "MALE_HEIGHT", type: "double precision", nullable: false),
                    MALEWEIGHT = table.Column<double>(name: "MALE_WEIGHT", type: "double precision", nullable: false),
                    MALECHEST = table.Column<double>(name: "MALE_CHEST", type: "double precision", nullable: false),
                    MALEHEAD = table.Column<double>(name: "MALE_HEAD", type: "double precision", nullable: false),
                    FEMALEHEIGHT = table.Column<double>(name: "FEMALE_HEIGHT", type: "double precision", nullable: false),
                    FEMALEWEIGHT = table.Column<double>(name: "FEMALE_WEIGHT", type: "double precision", nullable: false),
                    FEMALECHEST = table.Column<double>(name: "FEMALE_CHEST", type: "double precision", nullable: false),
                    FEMALEHEAD = table.Column<double>(name: "FEMALE_HEAD", type: "double precision", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHYSICAL_AVERAGE", x => new { x.JISSIYEAR, x.AGEYEAR, x.AGEMONTH, x.AGEDAY });
                });

            migrationBuilder.CreateTable(
                name: "PI_IMAGE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    IMAGETYPE = table.Column<int>(name: "IMAGE_TYPE", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(30)", maxLength: 30, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_IMAGE", x => new { x.HPID, x.IMAGETYPE, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "PI_INF",
                columns: table => new
                {
                    PIID = table.Column<string>(name: "PI_ID", type: "character varying(6)", maxLength: 6, nullable: false),
                    WDATE = table.Column<int>(name: "W_DATE", type: "integer", nullable: false),
                    TITLE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    RDATE = table.Column<int>(name: "R_DATE", type: "integer", nullable: false),
                    REVISION = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RTYPE = table.Column<string>(name: "R_TYPE", type: "character varying(20)", maxLength: 20, nullable: true),
                    RREASON = table.Column<string>(name: "R_REASON", type: "character varying(200)", maxLength: 200, nullable: true),
                    SCCJNO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    THERAPEUTICCLASSIFICATION = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PREPARATIONNAME = table.Column<string>(name: "PREPARATION_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    HIGHLIGHT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FEATURE = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RELATEDMATTER = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    COMMONNAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GENERICNAME = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_INF", x => x.PIID);
                });

            migrationBuilder.CreateTable(
                name: "PI_INF_DETAIL",
                columns: table => new
                {
                    PIID = table.Column<string>(name: "PI_ID", type: "character varying(6)", maxLength: 6, nullable: false),
                    BRANCH = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    JPN = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    LEVEL = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_INF_DETAIL", x => new { x.PIID, x.BRANCH, x.JPN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PI_PRODUCT_INF",
                columns: table => new
                {
                    PIID = table.Column<string>(name: "PI_ID", type: "text", nullable: false),
                    BRANCH = table.Column<string>(type: "text", nullable: false),
                    JPN = table.Column<string>(type: "text", nullable: false),
                    PIIDFULL = table.Column<string>(name: "PI_ID_FULL", type: "text", nullable: false),
                    PRODUCTNAME = table.Column<string>(name: "PRODUCT_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    UNIT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MAKER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    VENDER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    MARKETER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OTHER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    YJCD = table.Column<string>(name: "YJ_CD", type: "text", nullable: true),
                    HOTCD = table.Column<string>(name: "HOT_CD", type: "text", nullable: true),
                    SOSYONAME = table.Column<string>(name: "SOSYO_NAME", type: "character varying(80)", maxLength: 80, nullable: true),
                    GENERICNAME = table.Column<string>(name: "GENERIC_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    GENERICENGNAME = table.Column<string>(name: "GENERIC_ENG_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    GENERALNO = table.Column<string>(name: "GENERAL_NO", type: "character varying(50)", maxLength: 50, nullable: true),
                    VERDATE = table.Column<string>(name: "VER_DATE", type: "text", nullable: true),
                    YAKKAREG = table.Column<string>(name: "YAKKA_REG", type: "text", nullable: true),
                    YAKKADEL = table.Column<string>(name: "YAKKA_DEL", type: "text", nullable: true),
                    ISSTOPED = table.Column<string>(name: "IS_STOPED", type: "text", nullable: true),
                    STOPDATE = table.Column<string>(name: "STOP_DATE", type: "text", nullable: true),
                    PISTATE = table.Column<string>(name: "PI_STATE", type: "text", nullable: true),
                    PISBT = table.Column<string>(name: "PI_SBT", type: "text", nullable: true),
                    BIKOPIUNIT = table.Column<string>(name: "BIKO_PI_UNIT", type: "character varying(512)", maxLength: 512, nullable: true),
                    BIKOPIBRANCH = table.Column<string>(name: "BIKO_PI_BRANCH", type: "character varying(256)", maxLength: 256, nullable: true),
                    UPDDATEIMG = table.Column<DateTime>(name: "UPD_DATE_IMG", type: "timestamp with time zone", nullable: true),
                    UPDDATEPI = table.Column<DateTime>(name: "UPD_DATE_PI", type: "timestamp with time zone", nullable: true),
                    UPDDATEPRODUCT = table.Column<DateTime>(name: "UPD_DATE_PRODUCT", type: "timestamp with time zone", nullable: true),
                    UPDDATEXML = table.Column<DateTime>(name: "UPD_DATE_XML", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_PRODUCT_INF", x => new { x.PIIDFULL, x.PIID, x.BRANCH, x.JPN });
                });

            migrationBuilder.CreateTable(
                name: "POST_CODE_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    POSTCD = table.Column<string>(name: "POST_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    PREFKANA = table.Column<string>(name: "PREF_KANA", type: "character varying(60)", maxLength: 60, nullable: true),
                    CITYKANA = table.Column<string>(name: "CITY_KANA", type: "character varying(60)", maxLength: 60, nullable: true),
                    POSTALTERMKANA = table.Column<string>(name: "POSTAL_TERM_KANA", type: "character varying(150)", maxLength: 150, nullable: true),
                    PREFNAME = table.Column<string>(name: "PREF_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    CITYNAME = table.Column<string>(name: "CITY_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    BANTI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_POST_CODE_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRIORITY_HAIHAN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    HAIHANGRP = table.Column<long>(name: "HAIHAN_GRP", type: "bigint", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    USERSETTING = table.Column<int>(name: "USER_SETTING", type: "integer", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    ITEMCD1 = table.Column<string>(name: "ITEM_CD1", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD2 = table.Column<string>(name: "ITEM_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD3 = table.Column<string>(name: "ITEM_CD3", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD4 = table.Column<string>(name: "ITEM_CD4", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD5 = table.Column<string>(name: "ITEM_CD5", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD6 = table.Column<string>(name: "ITEM_CD6", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD7 = table.Column<string>(name: "ITEM_CD7", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD8 = table.Column<string>(name: "ITEM_CD8", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMCD9 = table.Column<string>(name: "ITEM_CD9", type: "character varying(10)", maxLength: 10, nullable: true),
                    SPJYOKEN = table.Column<int>(name: "SP_JYOKEN", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TERMCNT = table.Column<int>(name: "TERM_CNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false),
                    TARGETKBN = table.Column<int>(name: "TARGET_KBN", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRIORITY_HAIHAN_MST", x => new { x.HPID, x.HAIHANGRP, x.STARTDATE, x.USERSETTING });
                });

            migrationBuilder.CreateTable(
                name: "PT_ALRGY_DRUG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DRUGNAME = table.Column<string>(name: "DRUG_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_ALRGY_DRUG", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_ALRGY_ELSE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ALRGYNAME = table.Column<string>(name: "ALRGY_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_ALRGY_ELSE", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_ALRGY_FOOD",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ALRGYKBN = table.Column<string>(name: "ALRGY_KBN", type: "text", nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_ALRGY_FOOD", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_BYOMEI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    SYUSYOKUCD1 = table.Column<string>(name: "SYUSYOKU_CD1", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD2 = table.Column<string>(name: "SYUSYOKU_CD2", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD3 = table.Column<string>(name: "SYUSYOKU_CD3", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD4 = table.Column<string>(name: "SYUSYOKU_CD4", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD5 = table.Column<string>(name: "SYUSYOKU_CD5", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD6 = table.Column<string>(name: "SYUSYOKU_CD6", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD7 = table.Column<string>(name: "SYUSYOKU_CD7", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD8 = table.Column<string>(name: "SYUSYOKU_CD8", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD9 = table.Column<string>(name: "SYUSYOKU_CD9", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD10 = table.Column<string>(name: "SYUSYOKU_CD10", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD11 = table.Column<string>(name: "SYUSYOKU_CD11", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD12 = table.Column<string>(name: "SYUSYOKU_CD12", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD13 = table.Column<string>(name: "SYUSYOKU_CD13", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD14 = table.Column<string>(name: "SYUSYOKU_CD14", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD15 = table.Column<string>(name: "SYUSYOKU_CD15", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD16 = table.Column<string>(name: "SYUSYOKU_CD16", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD17 = table.Column<string>(name: "SYUSYOKU_CD17", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD18 = table.Column<string>(name: "SYUSYOKU_CD18", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD19 = table.Column<string>(name: "SYUSYOKU_CD19", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD20 = table.Column<string>(name: "SYUSYOKU_CD20", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD21 = table.Column<string>(name: "SYUSYOKU_CD21", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    TENKIKBN = table.Column<int>(name: "TENKI_KBN", type: "integer", nullable: false),
                    TENKIDATE = table.Column<int>(name: "TENKI_DATE", type: "integer", nullable: false),
                    SYUBYOKBN = table.Column<int>(name: "SYUBYO_KBN", type: "integer", nullable: false),
                    SIKKANKBN = table.Column<int>(name: "SIKKAN_KBN", type: "integer", nullable: false),
                    NANBYOCD = table.Column<int>(name: "NANBYO_CD", type: "integer", nullable: false),
                    HOSOKUCMT = table.Column<string>(name: "HOSOKU_CMT", type: "character varying(80)", maxLength: 80, nullable: true),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    TOGETUBYOMEI = table.Column<int>(name: "TOGETU_BYOMEI", type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPKARTE = table.Column<int>(name: "IS_NODSP_KARTE", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    ISIMPORTANT = table.Column<int>(name: "IS_IMPORTANT", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_BYOMEI", x => new { x.HPID, x.PTID, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "PT_CMT_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PT_CMT_INF", x => new { x.ID, x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_FAMILY",
                columns: table => new
                {
                    FAMILYID = table.Column<long>(name: "FAMILY_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZOKUGARACD = table.Column<string>(name: "ZOKUGARA_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    PARENTID = table.Column<int>(name: "PARENT_ID", type: "integer", nullable: false),
                    FAMILYPTID = table.Column<long>(name: "FAMILY_PT_ID", type: "bigint", nullable: false),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    ISDEAD = table.Column<int>(name: "IS_DEAD", type: "integer", nullable: false),
                    ISSEPARATED = table.Column<int>(name: "IS_SEPARATED", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
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
                    table.PrimaryKey("PK_PT_FAMILY", x => x.PTID);
                });

            migrationBuilder.CreateTable(
                name: "PT_FAMILY_REKI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    FAMILYID = table.Column<long>(name: "FAMILY_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_FAMILY_REKI", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_GRP_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRPCODE = table.Column<string>(name: "GRP_CODE", type: "character varying(4)", maxLength: 4, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_GRP_INF", x => new { x.HPID, x.GRPID, x.GRPCODE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_GRP_ITEM",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    GRPCODE = table.Column<string>(name: "GRP_CODE", type: "character varying(2)", maxLength: 2, nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRPCODENAME = table.Column<string>(name: "GRP_CODE_NAME", type: "character varying(30)", maxLength: 30, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_GRP_ITEM", x => new { x.HPID, x.GRPID, x.GRPCODE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_GRP_NAME_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    GRPNAME = table.Column<string>(name: "GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_PT_GRP_NAME_MST", x => new { x.HPID, x.GRPID });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_CHECK",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENGRP = table.Column<int>(name: "HOKEN_GRP", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CHECKDATE = table.Column<DateTime>(name: "CHECK_DATE", type: "timestamp with time zone", nullable: false),
                    CHECKID = table.Column<int>(name: "CHECK_ID", type: "integer", nullable: false),
                    CHECKMACHINE = table.Column<string>(name: "CHECK_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    CHECKCMT = table.Column<string>(name: "CHECK_CMT", type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_HOKEN_CHECK", x => new { x.HPID, x.PTID, x.HOKENGRP, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    EDANO = table.Column<string>(name: "EDA_NO", type: "character varying(2)", maxLength: 2, nullable: true),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    HOKENSYANO = table.Column<string>(name: "HOKENSYA_NO", type: "character varying(8)", maxLength: 8, nullable: true),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HONKEKBN = table.Column<int>(name: "HONKE_KBN", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENSYANAME = table.Column<string>(name: "HOKENSYA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSYAPOST = table.Column<string>(name: "HOKENSYA_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOKENSYAADDRESS = table.Column<string>(name: "HOKENSYA_ADDRESS", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSYATEL = table.Column<string>(name: "HOKENSYA_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    KEIZOKUKBN = table.Column<int>(name: "KEIZOKU_KBN", type: "integer", nullable: false),
                    SIKAKUDATE = table.Column<int>(name: "SIKAKU_DATE", type: "integer", nullable: false),
                    KOFUDATE = table.Column<int>(name: "KOFU_DATE", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOGAKUKBN = table.Column<int>(name: "KOGAKU_KBN", type: "integer", nullable: false),
                    KOGAKUTYPE = table.Column<int>(name: "KOGAKU_TYPE", type: "integer", nullable: false),
                    TOKUREIYM1 = table.Column<int>(name: "TOKUREI_YM1", type: "integer", nullable: false),
                    TOKUREIYM2 = table.Column<int>(name: "TOKUREI_YM2", type: "integer", nullable: false),
                    TASUKAIYM = table.Column<int>(name: "TASUKAI_YM", type: "integer", nullable: false),
                    SYOKUMUKBN = table.Column<int>(name: "SYOKUMU_KBN", type: "integer", nullable: false),
                    GENMENKBN = table.Column<int>(name: "GENMEN_KBN", type: "integer", nullable: false),
                    GENMENRATE = table.Column<int>(name: "GENMEN_RATE", type: "integer", nullable: false),
                    GENMENGAKU = table.Column<int>(name: "GENMEN_GAKU", type: "integer", nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIKOFUNO = table.Column<string>(name: "ROUSAI_KOFU_NO", type: "character varying(14)", maxLength: 14, nullable: true),
                    ROUSAISAIGAIKBN = table.Column<int>(name: "ROUSAI_SAIGAI_KBN", type: "integer", nullable: false),
                    ROUSAIJIGYOSYONAME = table.Column<string>(name: "ROUSAI_JIGYOSYO_NAME", type: "character varying(80)", maxLength: 80, nullable: true),
                    ROUSAIPREFNAME = table.Column<string>(name: "ROUSAI_PREF_NAME", type: "character varying(10)", maxLength: 10, nullable: true),
                    ROUSAICITYNAME = table.Column<string>(name: "ROUSAI_CITY_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    ROUSAISYOBYODATE = table.Column<int>(name: "ROUSAI_SYOBYO_DATE", type: "integer", nullable: false),
                    ROUSAISYOBYOCD = table.Column<string>(name: "ROUSAI_SYOBYO_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIROUDOUCD = table.Column<string>(name: "ROUSAI_ROUDOU_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIKANTOKUCD = table.Column<string>(name: "ROUSAI_KANTOKU_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIRECECOUNT = table.Column<int>(name: "ROUSAI_RECE_COUNT", type: "integer", nullable: false),
                    RYOYOSTARTDATE = table.Column<int>(name: "RYOYO_START_DATE", type: "integer", nullable: false),
                    RYOYOENDDATE = table.Column<int>(name: "RYOYO_END_DATE", type: "integer", nullable: false),
                    JIBAIHOKENNAME = table.Column<string>(name: "JIBAI_HOKEN_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    JIBAIHOKENTANTO = table.Column<string>(name: "JIBAI_HOKEN_TANTO", type: "character varying(40)", maxLength: 40, nullable: true),
                    JIBAIHOKENTEL = table.Column<string>(name: "JIBAI_HOKEN_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    JIBAIJYUSYOUDATE = table.Column<int>(name: "JIBAI_JYUSYOU_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_HOKEN_INF", x => new { x.HPID, x.PTID, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_PATTERN",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOKENSBTCD = table.Column<int>(name: "HOKEN_SBT_CD", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    KOHI1ID = table.Column<int>(name: "KOHI1_ID", type: "integer", nullable: false),
                    KOHI2ID = table.Column<int>(name: "KOHI2_ID", type: "integer", nullable: false),
                    KOHI3ID = table.Column<int>(name: "KOHI3_ID", type: "integer", nullable: false),
                    KOHI4ID = table.Column<int>(name: "KOHI4_ID", type: "integer", nullable: false),
                    HOKENMEMO = table.Column<string>(name: "HOKEN_MEMO", type: "character varying(400)", maxLength: 400, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_HOKEN_PATTERN", x => new { x.HPID, x.PTID, x.SEQNO, x.HOKENPID });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_SCAN",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENGRP = table.Column<int>(name: "HOKEN_GRP", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_HOKEN_SCAN", x => new { x.HPID, x.PTID, x.HOKENGRP, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTNUM = table.Column<long>(name: "PT_NUM", type: "bigint", nullable: false),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    ISDEAD = table.Column<int>(name: "IS_DEAD", type: "integer", nullable: false),
                    DEATHDATE = table.Column<int>(name: "DEATH_DATE", type: "integer", nullable: false),
                    HOMEPOST = table.Column<string>(name: "HOME_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOMEADDRESS1 = table.Column<string>(name: "HOME_ADDRESS1", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOMEADDRESS2 = table.Column<string>(name: "HOME_ADDRESS2", type: "character varying(100)", maxLength: 100, nullable: true),
                    TEL1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    TEL2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    MAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SETAINUSI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZOKUGARA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    JOB = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    RENRAKUNAME = table.Column<string>(name: "RENRAKU_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKUPOST = table.Column<string>(name: "RENRAKU_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    RENRAKUADDRESS1 = table.Column<string>(name: "RENRAKU_ADDRESS1", type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKUADDRESS2 = table.Column<string>(name: "RENRAKU_ADDRESS2", type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKUTEL = table.Column<string>(name: "RENRAKU_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    RENRAKUMEMO = table.Column<string>(name: "RENRAKU_MEMO", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICENAME = table.Column<string>(name: "OFFICE_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICEPOST = table.Column<string>(name: "OFFICE_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    OFFICEADDRESS1 = table.Column<string>(name: "OFFICE_ADDRESS1", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICEADDRESS2 = table.Column<string>(name: "OFFICE_ADDRESS2", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICETEL = table.Column<string>(name: "OFFICE_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    OFFICEMEMO = table.Column<string>(name: "OFFICE_MEMO", type: "character varying(100)", maxLength: 100, nullable: true),
                    ISRYOSYODETAIL = table.Column<int>(name: "IS_RYOSYO_DETAIL", type: "integer", nullable: false),
                    PRIMARYDOCTOR = table.Column<int>(name: "PRIMARY_DOCTOR", type: "integer", nullable: false),
                    ISTESTER = table.Column<int>(name: "IS_TESTER", type: "integer", nullable: false),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    MAINHOKENPID = table.Column<int>(name: "MAIN_HOKEN_PID", type: "integer", nullable: false),
                    REFERENCENO = table.Column<long>(name: "REFERENCE_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LIMITCONSFLG = table.Column<int>(name: "LIMIT_CONS_FLG", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_INF", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_INFECTION",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "text", nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_INFECTION", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_JIBAI_DOC",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    SINDANCOST = table.Column<int>(name: "SINDAN_COST", type: "integer", nullable: false),
                    SINDANNUM = table.Column<int>(name: "SINDAN_NUM", type: "integer", nullable: false),
                    MEISAICOST = table.Column<int>(name: "MEISAI_COST", type: "integer", nullable: false),
                    MEISAINUM = table.Column<int>(name: "MEISAI_NUM", type: "integer", nullable: false),
                    ELSECOST = table.Column<int>(name: "ELSE_COST", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_JIBAI_DOC", x => new { x.HPID, x.PTID, x.SINYM, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_JIBKAR",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    WEBID = table.Column<string>(name: "WEB_ID", type: "character varying(16)", maxLength: 16, nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ODRKAIJI = table.Column<int>(name: "ODR_KAIJI", type: "integer", nullable: false),
                    ODRUPDATEDATE = table.Column<DateTime>(name: "ODR_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    KARTEKAIJI = table.Column<int>(name: "KARTE_KAIJI", type: "integer", nullable: false),
                    KARTEUPDATEDATE = table.Column<DateTime>(name: "KARTE_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    KENSAKAIJI = table.Column<int>(name: "KENSA_KAIJI", type: "integer", nullable: false),
                    KENSAUPDATEDATE = table.Column<DateTime>(name: "KENSA_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    BYOMEIKAIJI = table.Column<int>(name: "BYOMEI_KAIJI", type: "integer", nullable: false),
                    BYOMEIUPDATEDATE = table.Column<DateTime>(name: "BYOMEI_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_JIBKAR", x => new { x.HPID, x.WEBID });
                });

            migrationBuilder.CreateTable(
                name: "PT_KIO_REKI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "text", nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_KIO_REKI", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_KOHI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    FUTANSYANO = table.Column<string>(name: "FUTANSYA_NO", type: "character varying(8)", maxLength: 8, nullable: true),
                    JYUKYUSYANO = table.Column<string>(name: "JYUKYUSYA_NO", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOKENSBTKBN = table.Column<int>(name: "HOKEN_SBT_KBN", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    TOKUSYUNO = table.Column<string>(name: "TOKUSYU_NO", type: "character varying(20)", maxLength: 20, nullable: true),
                    SIKAKUDATE = table.Column<int>(name: "SIKAKU_DATE", type: "integer", nullable: false),
                    KOFUDATE = table.Column<int>(name: "KOFU_DATE", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_KOHI", x => new { x.HPID, x.PTID, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_KYUSEI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_KYUSEI", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_LAST_VISIT_DATE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    LASTVISITDATE = table.Column<int>(name: "LAST_VISIT_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_LAST_VISIT_DATE", x => new { x.HPID, x.PTID });
                });

            migrationBuilder.CreateTable(
                name: "PT_MEMO",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PT_MEMO", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_OTC_DRUG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    SERIALNUM = table.Column<int>(name: "SERIAL_NUM", type: "integer", nullable: false),
                    TRADENAME = table.Column<string>(name: "TRADE_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_OTC_DRUG", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_OTHER_DRUG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DRUGNAME = table.Column<string>(name: "DRUG_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_OTHER_DRUG", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_PREGNANCY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    PERIODDATE = table.Column<int>(name: "PERIOD_DATE", type: "integer", nullable: false),
                    PERIODDUEDATE = table.Column<int>(name: "PERIOD_DUE_DATE", type: "integer", nullable: false),
                    OVULATIONDATE = table.Column<int>(name: "OVULATION_DATE", type: "integer", nullable: false),
                    OVULATIONDUEDATE = table.Column<int>(name: "OVULATION_DUE_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_PREGNANCY", x => new { x.ID, x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_ROUSAI_TENKI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    SINKEI = table.Column<int>(type: "integer", nullable: false),
                    TENKI = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_ROUSAI_TENKI", x => new { x.HPID, x.PTID, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_SANTEI_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBNNO = table.Column<int>(name: "KBN_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    KBNVAL = table.Column<int>(name: "KBN_VAL", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PT_SANTEI_CONF", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_SUPPLE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    INDEXCD = table.Column<string>(name: "INDEX_CD", type: "text", nullable: true),
                    INDEXWORD = table.Column<string>(name: "INDEX_WORD", type: "character varying(200)", maxLength: 200, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_PT_SUPPLE", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "PT_TAG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: true),
                    MEMODATA = table.Column<byte[]>(name: "MEMO_DATA", type: "bytea", nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    ISDSPUKETUKE = table.Column<int>(name: "IS_DSP_UKETUKE", type: "integer", nullable: false),
                    ISDSPKARTE = table.Column<int>(name: "IS_DSP_KARTE", type: "integer", nullable: false),
                    ISDSPKAIKEI = table.Column<int>(name: "IS_DSP_KAIKEI", type: "integer", nullable: false),
                    ISDSPRECE = table.Column<int>(name: "IS_DSP_RECE", type: "integer", nullable: false),
                    BACKGROUNDCOLOR = table.Column<string>(name: "BACKGROUND_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    TAGGRPCD = table.Column<int>(name: "TAG_GRP_CD", type: "integer", nullable: false),
                    ALPHABLENDVAL = table.Column<int>(name: "ALPHABLEND_VAL", type: "integer", nullable: false),
                    FONTSIZE = table.Column<int>(type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    WIDTH = table.Column<int>(type: "integer", nullable: false),
                    HEIGHT = table.Column<int>(type: "integer", nullable: false),
                    LEFT = table.Column<int>(type: "integer", nullable: false),
                    TOP = table.Column<int>(type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_TAG", x => new { x.HPID, x.PTID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_CMT_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_CMT_INF", x => new { x.HPID, x.RAIINNO, x.CMTKBN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_KBN",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    FILTERID = table.Column<int>(name: "FILTER_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_FILTER_KBN", x => new { x.HPID, x.FILTERID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_MST",
                columns: table => new
                {
                    FILTERID = table.Column<int>(name: "FILTER_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    FILTERNAME = table.Column<string>(name: "FILTER_NAME", type: "text", nullable: true),
                    SELECTKBN = table.Column<int>(name: "SELECT_KBN", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    SHORTCUT = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_FILTER_MST", x => x.FILTERID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_SORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    FILTERID = table.Column<int>(name: "FILTER_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PRIORITY = table.Column<int>(type: "integer", nullable: false),
                    COLUMNNAME = table.Column<string>(name: "COLUMN_NAME", type: "text", nullable: true),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SORTKBN = table.Column<int>(name: "SORT_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_FILTER_SORT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_STATE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    FILTERID = table.Column<int>(name: "FILTER_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_FILTER_STATE", x => new { x.HPID, x.FILTERID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    OYARAIINNO = table.Column<long>(name: "OYA_RAIIN_NO", type: "bigint", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    ISYOYAKU = table.Column<int>(name: "IS_YOYAKU", type: "integer", nullable: false),
                    YOYAKUTIME = table.Column<string>(name: "YOYAKU_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    YOYAKUID = table.Column<int>(name: "YOYAKU_ID", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    UKETUKETIME = table.Column<string>(name: "UKETUKE_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    UKETUKEID = table.Column<int>(name: "UKETUKE_ID", type: "integer", nullable: false),
                    UKETUKENO = table.Column<int>(name: "UKETUKE_NO", type: "integer", nullable: false),
                    SINSTARTTIME = table.Column<string>(name: "SIN_START_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    SINENDTIME = table.Column<string>(name: "SIN_END_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    KAIKEITIME = table.Column<string>(name: "KAIKEI_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    KAIKEIID = table.Column<int>(name: "KAIKEI_ID", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
                    SYOSAISINKBN = table.Column<int>(name: "SYOSAISIN_KBN", type: "integer", nullable: false),
                    JIKANKBN = table.Column<int>(name: "JIKAN_KBN", type: "integer", nullable: false),
                    CONFIRMATIONRESULT = table.Column<string>(name: "CONFIRMATION_RESULT", type: "character varying(120)", maxLength: 120, nullable: true),
                    CONFIRMATIONSTATE = table.Column<int>(name: "CONFIRMATION_STATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_INF", x => new { x.HPID, x.RAIINNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    KBNNAME = table.Column<string>(name: "KBN_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    COLORCD = table.Column<string>(name: "COLOR_CD", type: "character varying(8)", maxLength: 8, nullable: true),
                    ISCONFIRMED = table.Column<int>(name: "IS_CONFIRMED", type: "integer", nullable: false),
                    ISAUTO = table.Column<int>(name: "IS_AUTO", type: "integer", nullable: false),
                    ISAUTODELETE = table.Column<int>(name: "IS_AUTO_DELETE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_KBN_DETAIL", x => new { x.HPID, x.GRPID, x.KBNCD });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_INF", x => new { x.HPID, x.PTID, x.RAIINNO, x.GRPID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_ITEM",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ISEXCLUDE = table.Column<int>(name: "IS_EXCLUDE", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_ITEM", x => new { x.HPID, x.GRPID, x.KBNCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_KOUI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
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
                    table.PrimaryKey("PK_RAIIN_KBN_KOUI", x => new { x.HPID, x.GRPID, x.KBNCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    GRPNAME = table.Column<string>(name: "GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_RAIIN_KBN_MST", x => new { x.HPID, x.GRPID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_YOYAKU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YOYAKUCD = table.Column<int>(name: "YOYAKU_CD", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_KBN_YOYAKU", x => new { x.HPID, x.GRPID, x.KBNCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_CMT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
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
                    table.PrimaryKey("PK_RAIIN_LIST_CMT", x => new { x.HPID, x.RAIINNO, x.CMTKBN });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    KBNNAME = table.Column<string>(name: "KBN_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    COLORCD = table.Column<string>(name: "COLOR_CD", type: "character varying(8)", maxLength: 8, nullable: true),
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
                    table.PrimaryKey("PK_RAIIN_LIST_DETAIL", x => new { x.HPID, x.GRPID, x.KBNCD });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_DOC",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_LIST_DOC", x => new { x.HPID, x.GRPID, x.KBNCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_FILE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_LIST_FILE", x => new { x.HPID, x.GRPID, x.KBNCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    RAIINLISTKBN = table.Column<int>(name: "RAIIN_LIST_KBN", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_INF", x => new { x.HPID, x.PTID, x.SINDATE, x.RAIINNO, x.GRPID, x.RAIINLISTKBN });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_ITEM",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ISEXCLUDE = table.Column<int>(name: "IS_EXCLUDE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_LIST_ITEM", x => new { x.HPID, x.KBNCD, x.SEQNO, x.GRPID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_KOUI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_LIST_KOUI", x => new { x.HPID, x.KBNCD, x.SEQNO, x.GRPID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRPNAME = table.Column<string>(name: "GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_LIST_MST", x => new { x.HPID, x.GRPID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_TAG",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    TAGNO = table.Column<int>(name: "TAG_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RAIIN_LIST_TAG", x => new { x.HPID, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RECE_CHECK_CMT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ISPENDING = table.Column<int>(name: "IS_PENDING", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ISCHECKED = table.Column<int>(name: "IS_CHECKED", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RECE_CHECK_CMT", x => new { x.HPID, x.PTID, x.SINYM, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RECE_CHECK_ERR",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    ERRCD = table.Column<string>(name: "ERR_CD", type: "character varying(5)", maxLength: 5, nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    ACD = table.Column<string>(name: "A_CD", type: "character varying(100)", maxLength: 100, nullable: false),
                    BCD = table.Column<string>(name: "B_CD", type: "character varying(100)", maxLength: 100, nullable: false),
                    MESSAGE1 = table.Column<string>(name: "MESSAGE_1", type: "character varying(100)", maxLength: 100, nullable: true),
                    MESSAGE2 = table.Column<string>(name: "MESSAGE_2", type: "character varying(100)", maxLength: 100, nullable: true),
                    ISCHECKED = table.Column<int>(name: "IS_CHECKED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_CHECK_ERR", x => new { x.HPID, x.PTID, x.SINYM, x.HOKENID, x.ERRCD, x.SINDATE, x.ACD, x.BCD });
                });

            migrationBuilder.CreateTable(
                name: "RECE_CHECK_OPT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ERRCD = table.Column<string>(name: "ERR_CD", type: "character varying(5)", maxLength: 5, nullable: false),
                    CHECKOPT = table.Column<int>(name: "CHECK_OPT", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_CHECK_OPT", x => new { x.HPID, x.ERRCD });
                });

            migrationBuilder.CreateTable(
                name: "RECE_CMT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    CMTSBT = table.Column<int>(name: "CMT_SBT", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT = table.Column<string>(type: "text", nullable: true),
                    CMTDATA = table.Column<string>(name: "CMT_DATA", type: "character varying(38)", maxLength: 38, nullable: true),
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
                    table.PrimaryKey("PK_RECE_CMT", x => new { x.HPID, x.PTID, x.SINYM, x.HOKENID, x.CMTKBN, x.CMTSBT, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RECE_FUTAN_KBN",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    FUTANKBNCD = table.Column<string>(name: "FUTAN_KBN_CD", type: "character varying(1)", maxLength: 1, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_FUTAN_KBN", x => new { x.HPID, x.SEIKYUYM, x.PTID, x.SINYM, x.HOKENID, x.HOKENPID });
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    HOKENID2 = table.Column<int>(name: "HOKEN_ID2", type: "integer", nullable: false),
                    KOHI1ID = table.Column<int>(name: "KOHI1_ID", type: "integer", nullable: false),
                    KOHI2ID = table.Column<int>(name: "KOHI2_ID", type: "integer", nullable: false),
                    KOHI3ID = table.Column<int>(name: "KOHI3_ID", type: "integer", nullable: false),
                    KOHI4ID = table.Column<int>(name: "KOHI4_ID", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOKENSBTCD = table.Column<int>(name: "HOKEN_SBT_CD", type: "integer", nullable: false),
                    RECESBT = table.Column<string>(name: "RECE_SBT", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOKENSYANO = table.Column<string>(name: "HOKENSYA_NO", type: "character varying(8)", maxLength: 8, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1HOUBETU = table.Column<string>(name: "KOHI1_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI2HOUBETU = table.Column<string>(name: "KOHI2_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI3HOUBETU = table.Column<string>(name: "KOHI3_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI4HOUBETU = table.Column<string>(name: "KOHI4_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    HONKEKBN = table.Column<int>(name: "HONKE_KBN", type: "integer", nullable: false),
                    KOGAKUKBN = table.Column<int>(name: "KOGAKU_KBN", type: "integer", nullable: false),
                    KOGAKUTEKIYOKBN = table.Column<int>(name: "KOGAKU_TEKIYO_KBN", type: "integer", nullable: false),
                    ISTOKUREI = table.Column<int>(name: "IS_TOKUREI", type: "integer", nullable: false),
                    ISTASUKAI = table.Column<int>(name: "IS_TASUKAI", type: "integer", nullable: false),
                    KOGAKUKOHI1LIMIT = table.Column<int>(name: "KOGAKU_KOHI1_LIMIT", type: "integer", nullable: false),
                    KOGAKUKOHI2LIMIT = table.Column<int>(name: "KOGAKU_KOHI2_LIMIT", type: "integer", nullable: false),
                    KOGAKUKOHI3LIMIT = table.Column<int>(name: "KOGAKU_KOHI3_LIMIT", type: "integer", nullable: false),
                    KOGAKUKOHI4LIMIT = table.Column<int>(name: "KOGAKU_KOHI4_LIMIT", type: "integer", nullable: false),
                    TOTALKOGAKULIMIT = table.Column<int>(name: "TOTAL_KOGAKU_LIMIT", type: "integer", nullable: false),
                    GENMENKBN = table.Column<int>(name: "GENMEN_KBN", type: "integer", nullable: false),
                    HOKENRATE = table.Column<int>(name: "HOKEN_RATE", type: "integer", nullable: false),
                    PTRATE = table.Column<int>(name: "PT_RATE", type: "integer", nullable: false),
                    ENTEN = table.Column<int>(name: "EN_TEN", type: "integer", nullable: false),
                    KOHI1LIMIT = table.Column<int>(name: "KOHI1_LIMIT", type: "integer", nullable: false),
                    KOHI1OTHERFUTAN = table.Column<int>(name: "KOHI1_OTHER_FUTAN", type: "integer", nullable: false),
                    KOHI2LIMIT = table.Column<int>(name: "KOHI2_LIMIT", type: "integer", nullable: false),
                    KOHI2OTHERFUTAN = table.Column<int>(name: "KOHI2_OTHER_FUTAN", type: "integer", nullable: false),
                    KOHI3LIMIT = table.Column<int>(name: "KOHI3_LIMIT", type: "integer", nullable: false),
                    KOHI3OTHERFUTAN = table.Column<int>(name: "KOHI3_OTHER_FUTAN", type: "integer", nullable: false),
                    KOHI4LIMIT = table.Column<int>(name: "KOHI4_LIMIT", type: "integer", nullable: false),
                    KOHI4OTHERFUTAN = table.Column<int>(name: "KOHI4_OTHER_FUTAN", type: "integer", nullable: false),
                    TENSU = table.Column<int>(type: "integer", nullable: false),
                    TOTALIRYOHI = table.Column<int>(name: "TOTAL_IRYOHI", type: "integer", nullable: false),
                    HOKENFUTAN = table.Column<int>(name: "HOKEN_FUTAN", type: "integer", nullable: false),
                    KOGAKUFUTAN = table.Column<int>(name: "KOGAKU_FUTAN", type: "integer", nullable: false),
                    KOHI1FUTAN = table.Column<int>(name: "KOHI1_FUTAN", type: "integer", nullable: false),
                    KOHI2FUTAN = table.Column<int>(name: "KOHI2_FUTAN", type: "integer", nullable: false),
                    KOHI3FUTAN = table.Column<int>(name: "KOHI3_FUTAN", type: "integer", nullable: false),
                    KOHI4FUTAN = table.Column<int>(name: "KOHI4_FUTAN", type: "integer", nullable: false),
                    ICHIBUFUTAN = table.Column<int>(name: "ICHIBU_FUTAN", type: "integer", nullable: false),
                    GENMENGAKU = table.Column<int>(name: "GENMEN_GAKU", type: "integer", nullable: false),
                    HOKENFUTAN10EN = table.Column<int>(name: "HOKEN_FUTAN_10EN", type: "integer", nullable: false),
                    KOGAKUFUTAN10EN = table.Column<int>(name: "KOGAKU_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI1FUTAN10EN = table.Column<int>(name: "KOHI1_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI2FUTAN10EN = table.Column<int>(name: "KOHI2_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI3FUTAN10EN = table.Column<int>(name: "KOHI3_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI4FUTAN10EN = table.Column<int>(name: "KOHI4_FUTAN_10EN", type: "integer", nullable: false),
                    ICHIBUFUTAN10EN = table.Column<int>(name: "ICHIBU_FUTAN_10EN", type: "integer", nullable: false),
                    GENMENGAKU10EN = table.Column<int>(name: "GENMEN_GAKU_10EN", type: "integer", nullable: false),
                    PTFUTAN = table.Column<int>(name: "PT_FUTAN", type: "integer", nullable: false),
                    KOGAKUOVERKBN = table.Column<int>(name: "KOGAKU_OVER_KBN", type: "integer", nullable: false),
                    HOKENTENSU = table.Column<int>(name: "HOKEN_TENSU", type: "integer", nullable: false),
                    HOKENICHIBUFUTAN = table.Column<int>(name: "HOKEN_ICHIBU_FUTAN", type: "integer", nullable: false),
                    HOKENICHIBUFUTAN10EN = table.Column<int>(name: "HOKEN_ICHIBU_FUTAN_10EN", type: "integer", nullable: false),
                    KOHI1TENSU = table.Column<int>(name: "KOHI1_TENSU", type: "integer", nullable: false),
                    KOHI1ICHIBUSOTOGAKU = table.Column<int>(name: "KOHI1_ICHIBU_SOTOGAKU", type: "integer", nullable: false),
                    KOHI1ICHIBUSOTOGAKU10EN = table.Column<int>(name: "KOHI1_ICHIBU_SOTOGAKU_10EN", type: "integer", nullable: false),
                    KOHI1ICHIBUFUTAN = table.Column<int>(name: "KOHI1_ICHIBU_FUTAN", type: "integer", nullable: false),
                    KOHI2TENSU = table.Column<int>(name: "KOHI2_TENSU", type: "integer", nullable: false),
                    KOHI2ICHIBUSOTOGAKU = table.Column<int>(name: "KOHI2_ICHIBU_SOTOGAKU", type: "integer", nullable: false),
                    KOHI2ICHIBUSOTOGAKU10EN = table.Column<int>(name: "KOHI2_ICHIBU_SOTOGAKU_10EN", type: "integer", nullable: false),
                    KOHI2ICHIBUFUTAN = table.Column<int>(name: "KOHI2_ICHIBU_FUTAN", type: "integer", nullable: false),
                    KOHI3TENSU = table.Column<int>(name: "KOHI3_TENSU", type: "integer", nullable: false),
                    KOHI3ICHIBUSOTOGAKU = table.Column<int>(name: "KOHI3_ICHIBU_SOTOGAKU", type: "integer", nullable: false),
                    KOHI3ICHIBUSOTOGAKU10EN = table.Column<int>(name: "KOHI3_ICHIBU_SOTOGAKU_10EN", type: "integer", nullable: false),
                    KOHI3ICHIBUFUTAN = table.Column<int>(name: "KOHI3_ICHIBU_FUTAN", type: "integer", nullable: false),
                    KOHI4TENSU = table.Column<int>(name: "KOHI4_TENSU", type: "integer", nullable: false),
                    KOHI4ICHIBUSOTOGAKU = table.Column<int>(name: "KOHI4_ICHIBU_SOTOGAKU", type: "integer", nullable: false),
                    KOHI4ICHIBUSOTOGAKU10EN = table.Column<int>(name: "KOHI4_ICHIBU_SOTOGAKU_10EN", type: "integer", nullable: false),
                    KOHI4ICHIBUFUTAN = table.Column<int>(name: "KOHI4_ICHIBU_FUTAN", type: "integer", nullable: false),
                    TOTALICHIBUFUTAN = table.Column<int>(name: "TOTAL_ICHIBU_FUTAN", type: "integer", nullable: false),
                    TOTALICHIBUFUTAN10EN = table.Column<int>(name: "TOTAL_ICHIBU_FUTAN_10EN", type: "integer", nullable: false),
                    HOKENRECETENSU = table.Column<int>(name: "HOKEN_RECE_TENSU", type: "integer", nullable: true),
                    HOKENRECEFUTAN = table.Column<int>(name: "HOKEN_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECETENSU = table.Column<int>(name: "KOHI1_RECE_TENSU", type: "integer", nullable: true),
                    KOHI1RECEFUTAN = table.Column<int>(name: "KOHI1_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECEKYUFU = table.Column<int>(name: "KOHI1_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI2RECETENSU = table.Column<int>(name: "KOHI2_RECE_TENSU", type: "integer", nullable: true),
                    KOHI2RECEFUTAN = table.Column<int>(name: "KOHI2_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI2RECEKYUFU = table.Column<int>(name: "KOHI2_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI3RECETENSU = table.Column<int>(name: "KOHI3_RECE_TENSU", type: "integer", nullable: true),
                    KOHI3RECEFUTAN = table.Column<int>(name: "KOHI3_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI3RECEKYUFU = table.Column<int>(name: "KOHI3_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI4RECETENSU = table.Column<int>(name: "KOHI4_RECE_TENSU", type: "integer", nullable: true),
                    KOHI4RECEFUTAN = table.Column<int>(name: "KOHI4_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI4RECEKYUFU = table.Column<int>(name: "KOHI4_RECE_KYUFU", type: "integer", nullable: true),
                    HOKENNISSU = table.Column<int>(name: "HOKEN_NISSU", type: "integer", nullable: true),
                    KOHI1NISSU = table.Column<int>(name: "KOHI1_NISSU", type: "integer", nullable: true),
                    KOHI2NISSU = table.Column<int>(name: "KOHI2_NISSU", type: "integer", nullable: true),
                    KOHI3NISSU = table.Column<int>(name: "KOHI3_NISSU", type: "integer", nullable: true),
                    KOHI4NISSU = table.Column<int>(name: "KOHI4_NISSU", type: "integer", nullable: true),
                    KOHI1RECEKISAI = table.Column<int>(name: "KOHI1_RECE_KISAI", type: "integer", nullable: false),
                    KOHI2RECEKISAI = table.Column<int>(name: "KOHI2_RECE_KISAI", type: "integer", nullable: false),
                    KOHI3RECEKISAI = table.Column<int>(name: "KOHI3_RECE_KISAI", type: "integer", nullable: false),
                    KOHI4RECEKISAI = table.Column<int>(name: "KOHI4_RECE_KISAI", type: "integer", nullable: false),
                    KOHI1NAMECD = table.Column<string>(name: "KOHI1_NAME_CD", type: "character varying(5)", maxLength: 5, nullable: true),
                    KOHI2NAMECD = table.Column<string>(name: "KOHI2_NAME_CD", type: "character varying(5)", maxLength: 5, nullable: true),
                    KOHI3NAMECD = table.Column<string>(name: "KOHI3_NAME_CD", type: "character varying(5)", maxLength: 5, nullable: true),
                    KOHI4NAMECD = table.Column<string>(name: "KOHI4_NAME_CD", type: "character varying(5)", maxLength: 5, nullable: true),
                    SEIKYUKBN = table.Column<int>(name: "SEIKYU_KBN", type: "integer", nullable: false),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PTSTATUS = table.Column<string>(name: "PT_STATUS", type: "character varying(60)", maxLength: 60, nullable: true),
                    ROUSAIIFUTAN = table.Column<int>(name: "ROUSAI_I_FUTAN", type: "integer", nullable: false),
                    ROUSAIROFUTAN = table.Column<int>(name: "ROUSAI_RO_FUTAN", type: "integer", nullable: false),
                    JIBAIITENSU = table.Column<int>(name: "JIBAI_I_TENSU", type: "integer", nullable: false),
                    JIBAIROTENSU = table.Column<int>(name: "JIBAI_RO_TENSU", type: "integer", nullable: false),
                    JIBAIHAFUTAN = table.Column<int>(name: "JIBAI_HA_FUTAN", type: "integer", nullable: false),
                    JIBAINIFUTAN = table.Column<int>(name: "JIBAI_NI_FUTAN", type: "integer", nullable: false),
                    JIBAIHOSINDAN = table.Column<int>(name: "JIBAI_HO_SINDAN", type: "integer", nullable: false),
                    JIBAIHOSINDANCOUNT = table.Column<int>(name: "JIBAI_HO_SINDAN_COUNT", type: "integer", nullable: false),
                    JIBAIHEMEISAI = table.Column<int>(name: "JIBAI_HE_MEISAI", type: "integer", nullable: false),
                    JIBAIHEMEISAICOUNT = table.Column<int>(name: "JIBAI_HE_MEISAI_COUNT", type: "integer", nullable: false),
                    JIBAIAFUTAN = table.Column<int>(name: "JIBAI_A_FUTAN", type: "integer", nullable: false),
                    JIBAIBFUTAN = table.Column<int>(name: "JIBAI_B_FUTAN", type: "integer", nullable: false),
                    JIBAICFUTAN = table.Column<int>(name: "JIBAI_C_FUTAN", type: "integer", nullable: false),
                    JIBAIDFUTAN = table.Column<int>(name: "JIBAI_D_FUTAN", type: "integer", nullable: false),
                    JIBAIKENPOTENSU = table.Column<int>(name: "JIBAI_KENPO_TENSU", type: "integer", nullable: false),
                    JIBAIKENPOFUTAN = table.Column<int>(name: "JIBAI_KENPO_FUTAN", type: "integer", nullable: false),
                    SINKEI = table.Column<int>(type: "integer", nullable: false),
                    TENKI = table.Column<int>(type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    ISTESTER = table.Column<int>(name: "IS_TESTER", type: "integer", nullable: false),
                    ISZAIISO = table.Column<int>(name: "IS_ZAIISO", type: "integer", nullable: false),
                    CHOKIKBN = table.Column<int>(name: "CHOKI_KBN", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF", x => new { x.HPID, x.SEIKYUYM, x.PTID, x.SINYM, x.HOKENID });
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF_EDIT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RECESBT = table.Column<string>(name: "RECE_SBT", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1HOUBETU = table.Column<string>(name: "KOHI1_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI2HOUBETU = table.Column<string>(name: "KOHI2_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI3HOUBETU = table.Column<string>(name: "KOHI3_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI4HOUBETU = table.Column<string>(name: "KOHI4_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENRECETENSU = table.Column<int>(name: "HOKEN_RECE_TENSU", type: "integer", nullable: true),
                    HOKENRECEFUTAN = table.Column<int>(name: "HOKEN_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECETENSU = table.Column<int>(name: "KOHI1_RECE_TENSU", type: "integer", nullable: true),
                    KOHI1RECEFUTAN = table.Column<int>(name: "KOHI1_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECEKYUFU = table.Column<int>(name: "KOHI1_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI2RECETENSU = table.Column<int>(name: "KOHI2_RECE_TENSU", type: "integer", nullable: true),
                    KOHI2RECEFUTAN = table.Column<int>(name: "KOHI2_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI2RECEKYUFU = table.Column<int>(name: "KOHI2_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI3RECETENSU = table.Column<int>(name: "KOHI3_RECE_TENSU", type: "integer", nullable: true),
                    KOHI3RECEFUTAN = table.Column<int>(name: "KOHI3_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI3RECEKYUFU = table.Column<int>(name: "KOHI3_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI4RECETENSU = table.Column<int>(name: "KOHI4_RECE_TENSU", type: "integer", nullable: true),
                    KOHI4RECEFUTAN = table.Column<int>(name: "KOHI4_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI4RECEKYUFU = table.Column<int>(name: "KOHI4_RECE_KYUFU", type: "integer", nullable: true),
                    HOKENNISSU = table.Column<int>(name: "HOKEN_NISSU", type: "integer", nullable: true),
                    KOHI1NISSU = table.Column<int>(name: "KOHI1_NISSU", type: "integer", nullable: true),
                    KOHI2NISSU = table.Column<int>(name: "KOHI2_NISSU", type: "integer", nullable: true),
                    KOHI3NISSU = table.Column<int>(name: "KOHI3_NISSU", type: "integer", nullable: true),
                    KOHI4NISSU = table.Column<int>(name: "KOHI4_NISSU", type: "integer", nullable: true),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_RECE_INF_EDIT", x => new { x.HPID, x.SEIKYUYM, x.PTID, x.SINYM, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF_JD",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    KOHIID = table.Column<int>(name: "KOHI_ID", type: "integer", nullable: false),
                    FUTANSBTCD = table.Column<int>(name: "FUTAN_SBT_CD", type: "integer", nullable: false),
                    NISSU1 = table.Column<int>(type: "integer", nullable: false),
                    NISSU2 = table.Column<int>(type: "integer", nullable: false),
                    NISSU3 = table.Column<int>(type: "integer", nullable: false),
                    NISSU4 = table.Column<int>(type: "integer", nullable: false),
                    NISSU5 = table.Column<int>(type: "integer", nullable: false),
                    NISSU6 = table.Column<int>(type: "integer", nullable: false),
                    NISSU7 = table.Column<int>(type: "integer", nullable: false),
                    NISSU8 = table.Column<int>(type: "integer", nullable: false),
                    NISSU9 = table.Column<int>(type: "integer", nullable: false),
                    NISSU10 = table.Column<int>(type: "integer", nullable: false),
                    NISSU11 = table.Column<int>(type: "integer", nullable: false),
                    NISSU12 = table.Column<int>(type: "integer", nullable: false),
                    NISSU13 = table.Column<int>(type: "integer", nullable: false),
                    NISSU14 = table.Column<int>(type: "integer", nullable: false),
                    NISSU15 = table.Column<int>(type: "integer", nullable: false),
                    NISSU16 = table.Column<int>(type: "integer", nullable: false),
                    NISSU17 = table.Column<int>(type: "integer", nullable: false),
                    NISSU18 = table.Column<int>(type: "integer", nullable: false),
                    NISSU19 = table.Column<int>(type: "integer", nullable: false),
                    NISSU20 = table.Column<int>(type: "integer", nullable: false),
                    NISSU21 = table.Column<int>(type: "integer", nullable: false),
                    NISSU22 = table.Column<int>(type: "integer", nullable: false),
                    NISSU23 = table.Column<int>(type: "integer", nullable: false),
                    NISSU24 = table.Column<int>(type: "integer", nullable: false),
                    NISSU25 = table.Column<int>(type: "integer", nullable: false),
                    NISSU26 = table.Column<int>(type: "integer", nullable: false),
                    NISSU27 = table.Column<int>(type: "integer", nullable: false),
                    NISSU28 = table.Column<int>(type: "integer", nullable: false),
                    NISSU29 = table.Column<int>(type: "integer", nullable: false),
                    NISSU30 = table.Column<int>(type: "integer", nullable: false),
                    NISSU31 = table.Column<int>(type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF_JD", x => new { x.HPID, x.SEIKYUYM, x.PTID, x.SINYM, x.HOKENID, x.KOHIID });
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF_PRE_EDIT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    RECESBT = table.Column<string>(name: "RECE_SBT", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1HOUBETU = table.Column<string>(name: "KOHI1_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI2HOUBETU = table.Column<string>(name: "KOHI2_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI3HOUBETU = table.Column<string>(name: "KOHI3_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI4HOUBETU = table.Column<string>(name: "KOHI4_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENRECETENSU = table.Column<int>(name: "HOKEN_RECE_TENSU", type: "integer", nullable: true),
                    HOKENRECEFUTAN = table.Column<int>(name: "HOKEN_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECETENSU = table.Column<int>(name: "KOHI1_RECE_TENSU", type: "integer", nullable: true),
                    KOHI1RECEFUTAN = table.Column<int>(name: "KOHI1_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECEKYUFU = table.Column<int>(name: "KOHI1_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI2RECETENSU = table.Column<int>(name: "KOHI2_RECE_TENSU", type: "integer", nullable: true),
                    KOHI2RECEFUTAN = table.Column<int>(name: "KOHI2_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI2RECEKYUFU = table.Column<int>(name: "KOHI2_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI3RECETENSU = table.Column<int>(name: "KOHI3_RECE_TENSU", type: "integer", nullable: true),
                    KOHI3RECEFUTAN = table.Column<int>(name: "KOHI3_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI3RECEKYUFU = table.Column<int>(name: "KOHI3_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI4RECETENSU = table.Column<int>(name: "KOHI4_RECE_TENSU", type: "integer", nullable: true),
                    KOHI4RECEFUTAN = table.Column<int>(name: "KOHI4_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI4RECEKYUFU = table.Column<int>(name: "KOHI4_RECE_KYUFU", type: "integer", nullable: true),
                    HOKENNISSU = table.Column<int>(name: "HOKEN_NISSU", type: "integer", nullable: true),
                    KOHI1NISSU = table.Column<int>(name: "KOHI1_NISSU", type: "integer", nullable: true),
                    KOHI2NISSU = table.Column<int>(name: "KOHI2_NISSU", type: "integer", nullable: true),
                    KOHI3NISSU = table.Column<int>(name: "KOHI3_NISSU", type: "integer", nullable: true),
                    KOHI4NISSU = table.Column<int>(name: "KOHI4_NISSU", type: "integer", nullable: true),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF_PRE_EDIT", x => new { x.HPID, x.SEIKYUYM, x.PTID, x.SINYM, x.HOKENID });
                });

            migrationBuilder.CreateTable(
                name: "RECE_SEIKYU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    SEIKYUKBN = table.Column<int>(name: "SEIKYU_KBN", type: "integer", nullable: false),
                    PREHOKENID = table.Column<int>(name: "PRE_HOKEN_ID", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
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
                    table.PrimaryKey("PK_RECE_SEIKYU", x => new { x.HPID, x.SINYM, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RECE_STATUS",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    FUSENKBN = table.Column<int>(name: "FUSEN_KBN", type: "integer", nullable: false),
                    ISPAPERRECE = table.Column<int>(name: "IS_PAPER_RECE", type: "integer", nullable: false),
                    ISPRECHECKED = table.Column<int>(name: "IS_PRECHECKED", type: "integer", nullable: false),
                    OUTPUT = table.Column<int>(type: "integer", nullable: false),
                    STATUSKBN = table.Column<int>(name: "STATUS_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RECE_STATUS", x => new { x.HPID, x.PTID, x.SEIKYUYM, x.HOKENID, x.SINYM });
                });

            migrationBuilder.CreateTable(
                name: "RECEDEN_CMT_SELECT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMNO = table.Column<int>(name: "ITEM_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    COMMENTCD = table.Column<string>(name: "COMMENT_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    KBNNO = table.Column<string>(name: "KBN_NO", type: "character varying(64)", maxLength: 64, nullable: true),
                    PTSTATUS = table.Column<int>(name: "PT_STATUS", type: "integer", nullable: false),
                    CONDKBN = table.Column<int>(name: "COND_KBN", type: "integer", nullable: false),
                    NOTSANTEIKBN = table.Column<int>(name: "NOT_SANTEI_KBN", type: "integer", nullable: false),
                    NYUGAIKBN = table.Column<int>(name: "NYUGAI_KBN", type: "integer", nullable: false),
                    SANTEICNT = table.Column<int>(name: "SANTEI_CNT", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECEDEN_CMT_SELECT", x => new { x.HPID, x.ITEMNO, x.EDANO, x.ITEMCD, x.STARTDATE, x.COMMENTCD });
                });

            migrationBuilder.CreateTable(
                name: "RECEDEN_HEN_JIYUU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HENREIJIYUUCD = table.Column<string>(name: "HENREI_JIYUU_CD", type: "character varying(9)", maxLength: 9, nullable: true),
                    HENREIJIYUU = table.Column<string>(name: "HENREI_JIYUU", type: "text", nullable: true),
                    HOSOKU = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_RECEDEN_HEN_JIYUU", x => new { x.HPID, x.PTID, x.HOKENID, x.SINYM, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RECEDEN_RIREKI_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEARCHNO = table.Column<string>(name: "SEARCH_NO", type: "character varying(30)", maxLength: 30, nullable: true),
                    RIREKI = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_RECEDEN_RIREKI_INF", x => new { x.HPID, x.PTID, x.HOKENID, x.SINYM, x.SEQNO });
                });

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

            migrationBuilder.CreateTable(
                name: "RENKEI_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RENKEIID = table.Column<int>(name: "RENKEI_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PTNUMLENGTH = table.Column<int>(name: "PT_NUM_LENGTH", type: "integer", nullable: false),
                    TEMPLATEID = table.Column<int>(name: "TEMPLATE_ID", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_CONF", x => new { x.HPID, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RENKEIID = table.Column<int>(name: "RENKEI_ID", type: "integer", nullable: false),
                    RENKEINAME = table.Column<string>(name: "RENKEI_NAME", type: "character varying(255)", maxLength: 255, nullable: true),
                    RENKEISBT = table.Column<int>(name: "RENKEI_SBT", type: "integer", nullable: false),
                    FUNCTIONTYPE = table.Column<int>(name: "FUNCTION_TYPE", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_MST", x => new { x.HPID, x.RENKEIID });
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_PATH_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RENKEIID = table.Column<int>(name: "RENKEI_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PATH = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    CHARCD = table.Column<int>(name: "CHAR_CD", type: "integer", nullable: false),
                    WORKPATH = table.Column<string>(name: "WORK_PATH", type: "character varying(300)", maxLength: 300, nullable: true),
                    INTERVAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    USER = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PASSWORD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_PATH_CONF", x => new { x.HPID, x.EDANO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_REQ",
                columns: table => new
                {
                    REQID = table.Column<long>(name: "REQ_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    REQSBT = table.Column<int>(name: "REQ_SBT", type: "integer", nullable: false),
                    REQTYPE = table.Column<string>(name: "REQ_TYPE", type: "character varying(2)", maxLength: 2, nullable: true),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    ERRMST = table.Column<string>(name: "ERR_MST", type: "character varying(120)", maxLength: 120, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_REQ", x => x.REQID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_TEMPLATE_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TEMPLATEID = table.Column<int>(name: "TEMPLATE_ID", type: "integer", nullable: false),
                    TEMPLATENAME = table.Column<string>(name: "TEMPLATE_NAME", type: "character varying(255)", maxLength: 255, nullable: true),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FILE = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_TEMPLATE_MST", x => new { x.HPID, x.TEMPLATEID });
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_TIMING_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RENKEIID = table.Column<int>(name: "RENKEI_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    EVENTCD = table.Column<string>(name: "EVENT_CD", type: "character varying(11)", maxLength: 11, nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_TIMING_CONF", x => new { x.HPID, x.EVENTCD, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_TIMING_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RENKEIID = table.Column<int>(name: "RENKEI_ID", type: "integer", nullable: false),
                    EVENTCD = table.Column<string>(name: "EVENT_CD", type: "character varying(11)", maxLength: 11, nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_TIMING_MST", x => new { x.HPID, x.RENKEIID, x.EVENTCD });
                });

            migrationBuilder.CreateTable(
                name: "ROUDOU_MST",
                columns: table => new
                {
                    ROUDOUCD = table.Column<string>(name: "ROUDOU_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    ROUDOUNAME = table.Column<string>(name: "ROUDOU_NAME", type: "character varying(60)", maxLength: 60, nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUDOU_MST", x => x.ROUDOUCD);
                });

            migrationBuilder.CreateTable(
                name: "ROUSAI_GOSEI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GOSEIGRP = table.Column<int>(name: "GOSEI_GRP", type: "integer", nullable: false),
                    GOSEIITEMCD = table.Column<string>(name: "GOSEI_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SISIKBN = table.Column<int>(name: "SISI_KBN", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUSAI_GOSEI_MST", x => new { x.HPID, x.GOSEIGRP, x.GOSEIITEMCD, x.ITEMCD, x.SISIKBN, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "RSV_DAY_COMMENT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_RSV_DAY_COMMENT", x => new { x.HPID, x.SINDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_DAY_PTN",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    ENDTIME = table.Column<int>(name: "END_TIME", type: "integer", nullable: false),
                    MINUTES = table.Column<int>(type: "integer", nullable: false),
                    NUMBER = table.Column<int>(type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    ISHOLIDAY = table.Column<int>(name: "IS_HOLIDAY", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RSV_FRAME_DAY_PTN", x => new { x.HPID, x.RSVFRAMEID, x.SINDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_INF",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    ENDTIME = table.Column<int>(name: "END_TIME", type: "integer", nullable: false),
                    FRAMENO = table.Column<int>(name: "FRAME_NO", type: "integer", nullable: false),
                    ISHOLIDAY = table.Column<int>(name: "IS_HOLIDAY", type: "integer", nullable: false),
                    NUMBER = table.Column<long>(type: "bigint", nullable: false),
                    FRAMESBT = table.Column<int>(name: "FRAME_SBT", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_INF", x => new { x.HPID, x.RSVFRAMEID, x.SINDATE, x.STARTTIME, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RSVGRPID = table.Column<int>(name: "RSV_GRP_ID", type: "integer", nullable: false),
                    SORTKEY = table.Column<int>(name: "SORT_KEY", type: "integer", nullable: false),
                    RSVFRAMENAME = table.Column<string>(name: "RSV_FRAME_NAME", type: "character varying(60)", maxLength: 60, nullable: true),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    MAKERAIIN = table.Column<int>(name: "MAKE_RAIIN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RSV_FRAME_MST", x => new { x.HPID, x.RSVFRAMEID });
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_WEEK_PTN",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    WEEK = table.Column<int>(type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    ENDTIME = table.Column<int>(name: "END_TIME", type: "integer", nullable: false),
                    MINUTES = table.Column<int>(type: "integer", nullable: false),
                    NUMBER = table.Column<int>(type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    ISHOLIDAY = table.Column<int>(name: "IS_HOLIDAY", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RSV_FRAME_WEEK_PTN", x => new { x.ID, x.HPID, x.RSVFRAMEID, x.WEEK, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_WITH",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false),
                    WITHFRAMEID = table.Column<int>(name: "WITH_FRAME_ID", type: "integer", nullable: false),
                    SORTKEY = table.Column<int>(name: "SORT_KEY", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_WITH", x => new { x.ID, x.HPID, x.RSVFRAMEID });
                });

            migrationBuilder.CreateTable(
                name: "RSV_GRP_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVGRPID = table.Column<int>(name: "RSV_GRP_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTKEY = table.Column<int>(name: "SORT_KEY", type: "integer", nullable: false),
                    RSVGRPNAME = table.Column<string>(name: "RSV_GRP_NAME", type: "character varying(60)", maxLength: 60, nullable: false),
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
                    table.PrimaryKey("PK_RSV_GRP_MST", x => new { x.HPID, x.RSVGRPID });
                });

            migrationBuilder.CreateTable(
                name: "RSV_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVSBT = table.Column<int>(name: "RSV_SBT", type: "integer", nullable: false),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_INF", x => new { x.HPID, x.RSVFRAMEID, x.SINDATE, x.STARTTIME, x.RAIINNO });
                });

            migrationBuilder.CreateTable(
                name: "RSV_RENKEI_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    OTHERSEQNO = table.Column<long>(name: "OTHER_SEQ_NO", type: "bigint", nullable: false),
                    OTHERSEQNO2 = table.Column<long>(name: "OTHER_SEQ_NO2", type: "bigint", nullable: false),
                    OTHERPTID = table.Column<long>(name: "OTHER_PT_ID", type: "bigint", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_RENKEI_INF", x => new { x.HPID, x.RAIINNO });
                });

            migrationBuilder.CreateTable(
                name: "RSV_RENKEI_INF_TK",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SYSTEMKBN = table.Column<int>(name: "SYSTEM_KBN", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    OTHERSEQNO = table.Column<long>(name: "OTHER_SEQ_NO", type: "bigint", nullable: false),
                    OTHERSEQNO2 = table.Column<long>(name: "OTHER_SEQ_NO2", type: "bigint", nullable: false),
                    OTHERPTID = table.Column<long>(name: "OTHER_PT_ID", type: "bigint", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_RENKEI_INF_TK", x => new { x.HPID, x.RAIINNO, x.SYSTEMKBN });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_BYOMEI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD1 = table.Column<string>(name: "SYUSYOKU_CD1", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD2 = table.Column<string>(name: "SYUSYOKU_CD2", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD3 = table.Column<string>(name: "SYUSYOKU_CD3", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD4 = table.Column<string>(name: "SYUSYOKU_CD4", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD5 = table.Column<string>(name: "SYUSYOKU_CD5", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD6 = table.Column<string>(name: "SYUSYOKU_CD6", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD7 = table.Column<string>(name: "SYUSYOKU_CD7", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD8 = table.Column<string>(name: "SYUSYOKU_CD8", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD9 = table.Column<string>(name: "SYUSYOKU_CD9", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD10 = table.Column<string>(name: "SYUSYOKU_CD10", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD11 = table.Column<string>(name: "SYUSYOKU_CD11", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD12 = table.Column<string>(name: "SYUSYOKU_CD12", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD13 = table.Column<string>(name: "SYUSYOKU_CD13", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD14 = table.Column<string>(name: "SYUSYOKU_CD14", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD15 = table.Column<string>(name: "SYUSYOKU_CD15", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD16 = table.Column<string>(name: "SYUSYOKU_CD16", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD17 = table.Column<string>(name: "SYUSYOKU_CD17", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD18 = table.Column<string>(name: "SYUSYOKU_CD18", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD19 = table.Column<string>(name: "SYUSYOKU_CD19", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD20 = table.Column<string>(name: "SYUSYOKU_CD20", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD21 = table.Column<string>(name: "SYUSYOKU_CD21", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    SYUBYOKBN = table.Column<int>(name: "SYUBYO_KBN", type: "integer", nullable: false),
                    SIKKANKBN = table.Column<int>(name: "SIKKAN_KBN", type: "integer", nullable: false),
                    NANBYOCD = table.Column<int>(name: "NANBYO_CD", type: "integer", nullable: false),
                    HOSOKUCMT = table.Column<string>(name: "HOSOKU_CMT", type: "character varying(80)", maxLength: 80, nullable: true),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPKARTE = table.Column<int>(name: "IS_NODSP_KARTE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RSVKRT_BYOMEI", x => new { x.HPID, x.PTID, x.RSVKRTNO, x.SEQNO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_KARTE_IMG_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    POSITION = table.Column<long>(type: "bigint", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    MESSAGE = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_KARTE_IMG_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_KARTE_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    RSVDATE = table.Column<int>(name: "RSV_DATE", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RICHTEXT = table.Column<byte[]>(name: "RICH_TEXT", type: "bytea", nullable: false),
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
                    table.PrimaryKey("PK_RSVKRT_KARTE_INF", x => new { x.HPID, x.PTID, x.RSVKRTNO, x.KARTEKBN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RSVKRTKBN = table.Column<int>(name: "RSVKRT_KBN", type: "integer", nullable: false),
                    RSVDATE = table.Column<int>(name: "RSV_DATE", type: "integer", nullable: false),
                    RSVNAME = table.Column<string>(name: "RSV_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_RSVKRT_MST", x => new { x.HPID, x.PTID, x.RSVKRTNO });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_ODR_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RSVDATE = table.Column<int>(name: "RSV_DATE", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    ODRKOUIKBN = table.Column<int>(name: "ODR_KOUI_KBN", type: "integer", nullable: false),
                    RPNAME = table.Column<string>(name: "RP_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    SYOHOSBT = table.Column<int>(name: "SYOHO_SBT", type: "integer", nullable: false),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
                    TOSEKIKBN = table.Column<int>(name: "TOSEKI_KBN", type: "integer", nullable: false),
                    DAYSCNT = table.Column<int>(name: "DAYS_CNT", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_ODR_INF", x => new { x.HPID, x.PTID, x.RSVKRTNO, x.RPNO, x.RPEDANO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_ODR_INF_CMT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    RSVDATE = table.Column<int>(name: "RSV_DATE", type: "integer", nullable: false),
                    FONTCOLOR = table.Column<int>(name: "FONT_COLOR", type: "integer", nullable: false),
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(32)", maxLength: 32, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_ODR_INF_CMT", x => new { x.HPID, x.PTID, x.RSVKRTNO, x.RPEDANO, x.RPNO, x.ROWNO, x.EDANO });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_ODR_INF_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVKRTNO = table.Column<long>(name: "RSVKRT_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    RSVDATE = table.Column<int>(name: "RSV_DATE", type: "integer", nullable: false),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    UNITSBT = table.Column<int>(name: "UNIT_SBT", type: "integer", nullable: false),
                    TERMVAL = table.Column<double>(name: "TERM_VAL", type: "double precision", nullable: false),
                    KOHATUKBN = table.Column<int>(name: "KOHATU_KBN", type: "integer", nullable: false),
                    SYOHOKBN = table.Column<int>(name: "SYOHO_KBN", type: "integer", nullable: false),
                    SYOHOLIMITKBN = table.Column<int>(name: "SYOHO_LIMIT_KBN", type: "integer", nullable: false),
                    DRUGKBN = table.Column<int>(name: "DRUG_KBN", type: "integer", nullable: false),
                    YOHOKBN = table.Column<int>(name: "YOHO_KBN", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    IPNCD = table.Column<string>(name: "IPN_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    IPNNAME = table.Column<string>(name: "IPN_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true),
                    FONTCOLOR = table.Column<string>(name: "FONT_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    COMMENTNEWLINE = table.Column<int>(name: "COMMENT_NEWLINE", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_ODR_INF_DETAIL", x => new { x.HPID, x.PTID, x.RSVKRTNO, x.RPNO, x.RPEDANO, x.ROWNO });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_AUTO_ORDER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SANTEIGRPCD = table.Column<int>(name: "SANTEI_GRP_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    ADDTYPE = table.Column<int>(name: "ADD_TYPE", type: "integer", nullable: false),
                    ADDTARGET = table.Column<int>(name: "ADD_TARGET", type: "integer", nullable: false),
                    TERMCNT = table.Column<int>(name: "TERM_CNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false),
                    CNTTYPE = table.Column<int>(name: "CNT_TYPE", type: "integer", nullable: false),
                    MAXCNT = table.Column<long>(name: "MAX_CNT", type: "bigint", nullable: false),
                    SPCONDITION = table.Column<int>(name: "SP_CONDITION", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_AUTO_ORDER", x => new { x.ID, x.HPID, x.SANTEIGRPCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_AUTO_ORDER_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SANTEIGRPCD = table.Column<int>(name: "SANTEI_GRP_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_AUTO_ORDER_DETAIL", x => new { x.ID, x.HPID, x.SANTEIGRPCD, x.SEQNO, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_CNT_CHECK",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SANTEIGRPCD = table.Column<int>(name: "SANTEI_GRP_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    TERMCNT = table.Column<int>(name: "TERM_CNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false),
                    CNTTYPE = table.Column<int>(name: "CNT_TYPE", type: "integer", nullable: false),
                    MAXCNT = table.Column<long>(name: "MAX_CNT", type: "bigint", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(10)", maxLength: 10, nullable: true),
                    ERRKBN = table.Column<int>(name: "ERR_KBN", type: "integer", nullable: false),
                    TARGETCD = table.Column<string>(name: "TARGET_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SPCONDITION = table.Column<int>(name: "SP_CONDITION", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_CNT_CHECK", x => new { x.HPID, x.SANTEIGRPCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_GRP_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SANTEIGRPCD = table.Column<int>(name: "SANTEI_GRP_CD", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "text", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_GRP_DETAIL", x => new { x.HPID, x.SANTEIGRPCD, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_GRP_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SANTEIGRPCD = table.Column<int>(name: "SANTEI_GRP_CD", type: "integer", nullable: false),
                    SANTEIGRPNAME = table.Column<string>(name: "SANTEI_GRP_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SANTEI_GRP_MST", x => new { x.HPID, x.SANTEIGRPCD });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ALERTDAYS = table.Column<int>(name: "ALERT_DAYS", type: "integer", nullable: false),
                    ALERTTERM = table.Column<int>(name: "ALERT_TERM", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_INF_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    KISANSBT = table.Column<int>(name: "KISAN_SBT", type: "integer", nullable: false),
                    KISANDATE = table.Column<int>(name: "KISAN_DATE", type: "integer", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    HOSOKUCOMMENT = table.Column<string>(name: "HOSOKU_COMMENT", type: "character varying(80)", maxLength: 80, nullable: true),
                    COMMENT = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SANTEI_INF_DETAIL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SCHEMA_CMT_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    COMMENTCD = table.Column<int>(name: "COMMENT_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    COMMENT = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SCHEMA_CMT_MST", x => new { x.HPID, x.COMMENTCD });
                });

            migrationBuilder.CreateTable(
                name: "SEIKATUREKI_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEIKATUREKI_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SENTENCE_LIST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SENTENCECD = table.Column<int>(name: "SENTENCE_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SENTENCE = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    SETKBN = table.Column<int>(name: "SET_KBN", type: "integer", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    LEVEL1 = table.Column<long>(type: "bigint", nullable: false),
                    LEVEL2 = table.Column<long>(type: "bigint", nullable: false),
                    LEVEL3 = table.Column<long>(type: "bigint", nullable: false),
                    SELECTTYPE = table.Column<int>(name: "SELECT_TYPE", type: "integer", nullable: false),
                    NEWLINE = table.Column<int>(name: "NEW_LINE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENTENCE_LIST", x => new { x.HPID, x.SENTENCE });
                });

            migrationBuilder.CreateTable(
                name: "SESSION_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MACHINE = table.Column<string>(type: "text", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    LOGINDATE = table.Column<DateTime>(name: "LOGIN_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_INF", x => new { x.HPID, x.MACHINE });
                });

            migrationBuilder.CreateTable(
                name: "SET_BYOMEI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD1 = table.Column<string>(name: "SYUSYOKU_CD1", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD2 = table.Column<string>(name: "SYUSYOKU_CD2", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD3 = table.Column<string>(name: "SYUSYOKU_CD3", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD4 = table.Column<string>(name: "SYUSYOKU_CD4", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD5 = table.Column<string>(name: "SYUSYOKU_CD5", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD6 = table.Column<string>(name: "SYUSYOKU_CD6", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD7 = table.Column<string>(name: "SYUSYOKU_CD7", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD8 = table.Column<string>(name: "SYUSYOKU_CD8", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD9 = table.Column<string>(name: "SYUSYOKU_CD9", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD10 = table.Column<string>(name: "SYUSYOKU_CD10", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD11 = table.Column<string>(name: "SYUSYOKU_CD11", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD12 = table.Column<string>(name: "SYUSYOKU_CD12", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD13 = table.Column<string>(name: "SYUSYOKU_CD13", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD14 = table.Column<string>(name: "SYUSYOKU_CD14", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD15 = table.Column<string>(name: "SYUSYOKU_CD15", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD16 = table.Column<string>(name: "SYUSYOKU_CD16", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD17 = table.Column<string>(name: "SYUSYOKU_CD17", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD18 = table.Column<string>(name: "SYUSYOKU_CD18", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD19 = table.Column<string>(name: "SYUSYOKU_CD19", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD20 = table.Column<string>(name: "SYUSYOKU_CD20", type: "character varying(7)", maxLength: 7, nullable: true),
                    SYUSYOKUCD21 = table.Column<string>(name: "SYUSYOKU_CD21", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    SYUBYOKBN = table.Column<int>(name: "SYUBYO_KBN", type: "integer", nullable: false),
                    SIKKANKBN = table.Column<int>(name: "SIKKAN_KBN", type: "integer", nullable: false),
                    NANBYOCD = table.Column<int>(name: "NANBYO_CD", type: "integer", nullable: false),
                    HOSOKUCMT = table.Column<string>(name: "HOSOKU_CMT", type: "character varying(80)", maxLength: 80, nullable: true),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPKARTE = table.Column<int>(name: "IS_NODSP_KARTE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SET_BYOMEI", x => new { x.ID, x.HPID, x.SETCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SET_GENERATION_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SET_GENERATION_MST", x => new { x.HPID, x.GENERATIONID });
                });

            migrationBuilder.CreateTable(
                name: "SET_KARTE_IMG_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    POSITION = table.Column<long>(type: "bigint", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_KARTE_IMG_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SET_KARTE_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RICHTEXT = table.Column<byte[]>(name: "RICH_TEXT", type: "bytea", nullable: true),
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
                    table.PrimaryKey("PK_SET_KARTE_INF", x => new { x.HPID, x.SETCD, x.KARTEKBN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SET_KBN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETKBN = table.Column<int>(name: "SET_KBN", type: "integer", nullable: false),
                    SETKBNEDANO = table.Column<int>(name: "SET_KBN_EDA_NO", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false),
                    SETKBNNAME = table.Column<string>(name: "SET_KBN_NAME", type: "character varying(60)", maxLength: 60, nullable: true),
                    KACD = table.Column<int>(name: "KA_CD", type: "integer", nullable: false),
                    DOCCD = table.Column<int>(name: "DOC_CD", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SET_KBN_MST", x => new { x.HPID, x.SETKBN, x.SETKBNEDANO, x.GENERATIONID });
                });

            migrationBuilder.CreateTable(
                name: "SET_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SETKBN = table.Column<int>(name: "SET_KBN", type: "integer", nullable: false),
                    SETKBNEDANO = table.Column<int>(name: "SET_KBN_EDA_NO", type: "integer", nullable: false),
                    GENERATIONID = table.Column<int>(name: "GENERATION_ID", type: "integer", nullable: false),
                    LEVEL1 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL2 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL3 = table.Column<int>(type: "integer", nullable: false),
                    SETNAME = table.Column<string>(name: "SET_NAME", type: "character varying(60)", maxLength: 60, nullable: true),
                    WEIGHTKBN = table.Column<int>(name: "WEIGHT_KBN", type: "integer", nullable: false),
                    COLOR = table.Column<int>(type: "integer", nullable: false),
                    ISGROUP = table.Column<int>(name: "IS_GROUP", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SET_MST", x => new { x.HPID, x.SETCD });
                });

            migrationBuilder.CreateTable(
                name: "SET_ODR_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ODRKOUIKBN = table.Column<int>(name: "ODR_KOUI_KBN", type: "integer", nullable: false),
                    RPNAME = table.Column<string>(name: "RP_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    SYOHOSBT = table.Column<int>(name: "SYOHO_SBT", type: "integer", nullable: false),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
                    TOSEKIKBN = table.Column<int>(name: "TOSEKI_KBN", type: "integer", nullable: false),
                    DAYSCNT = table.Column<int>(name: "DAYS_CNT", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_ODR_INF", x => new { x.HPID, x.SETCD, x.RPNO, x.RPEDANO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "SET_ODR_INF_CMT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    FONTCOLOR = table.Column<int>(name: "FONT_COLOR", type: "integer", nullable: false),
                    CMTCD = table.Column<string>(name: "CMT_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(32)", maxLength: 32, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_ODR_INF_CMT", x => new { x.HPID, x.SETCD, x.RPNO, x.RPEDANO, x.ROWNO, x.EDANO });
                });

            migrationBuilder.CreateTable(
                name: "SET_ODR_INF_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SETCD = table.Column<int>(name: "SET_CD", type: "integer", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    UNITSBT = table.Column<int>(name: "UNIT_SBT", type: "integer", nullable: false),
                    ODRTERMVAL = table.Column<double>(name: "ODR_TERM_VAL", type: "double precision", nullable: false),
                    KOHATUKBN = table.Column<int>(name: "KOHATU_KBN", type: "integer", nullable: false),
                    SYOHOKBN = table.Column<int>(name: "SYOHO_KBN", type: "integer", nullable: false),
                    SYOHOLIMITKBN = table.Column<int>(name: "SYOHO_LIMIT_KBN", type: "integer", nullable: false),
                    DRUGKBN = table.Column<int>(name: "DRUG_KBN", type: "integer", nullable: false),
                    YOHOKBN = table.Column<int>(name: "YOHO_KBN", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    IPNCD = table.Column<string>(name: "IPN_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    IPNNAME = table.Column<string>(name: "IPN_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true),
                    FONTCOLOR = table.Column<string>(name: "FONT_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    COMMENTNEWLINE = table.Column<int>(name: "COMMENT_NEWLINE", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_ODR_INF_DETAIL", x => new { x.HPID, x.SETCD, x.RPNO, x.RPEDANO, x.ROWNO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_KOUI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SYUKEISAKI = table.Column<string>(name: "SYUKEI_SAKI", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOKATUKENSA = table.Column<int>(name: "HOKATU_KENSA", type: "integer", nullable: false),
                    TOTALTEN = table.Column<double>(name: "TOTAL_TEN", type: "double precision", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    ZEI = table.Column<double>(type: "double precision", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    TENCOUNT = table.Column<string>(name: "TEN_COUNT", type: "character varying(20)", maxLength: 20, nullable: true),
                    TENCOLCOUNT = table.Column<int>(name: "TEN_COL_COUNT", type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPPAPERRECE = table.Column<int>(name: "IS_NODSP_PAPER_RECE", type: "integer", nullable: false),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    ENTENKBN = table.Column<int>(name: "ENTEN_KBN", type: "integer", nullable: false),
                    CDKBN = table.Column<string>(name: "CD_KBN", type: "character varying(2)", maxLength: 2, nullable: true),
                    RECID = table.Column<string>(name: "REC_ID", type: "character varying(2)", maxLength: 2, nullable: true),
                    JIHISBT = table.Column<int>(name: "JIHI_SBT", type: "integer", nullable: false),
                    KAZEIKBN = table.Column<int>(name: "KAZEI_KBN", type: "integer", nullable: false),
                    DETAILDATA = table.Column<string>(name: "DETAIL_DATA", type: "text", nullable: true),
                    DAY1 = table.Column<int>(type: "integer", nullable: false),
                    DAY2 = table.Column<int>(type: "integer", nullable: false),
                    DAY3 = table.Column<int>(type: "integer", nullable: false),
                    DAY4 = table.Column<int>(type: "integer", nullable: false),
                    DAY5 = table.Column<int>(type: "integer", nullable: false),
                    DAY6 = table.Column<int>(type: "integer", nullable: false),
                    DAY7 = table.Column<int>(type: "integer", nullable: false),
                    DAY8 = table.Column<int>(type: "integer", nullable: false),
                    DAY9 = table.Column<int>(type: "integer", nullable: false),
                    DAY10 = table.Column<int>(type: "integer", nullable: false),
                    DAY11 = table.Column<int>(type: "integer", nullable: false),
                    DAY12 = table.Column<int>(type: "integer", nullable: false),
                    DAY13 = table.Column<int>(type: "integer", nullable: false),
                    DAY14 = table.Column<int>(type: "integer", nullable: false),
                    DAY15 = table.Column<int>(type: "integer", nullable: false),
                    DAY16 = table.Column<int>(type: "integer", nullable: false),
                    DAY17 = table.Column<int>(type: "integer", nullable: false),
                    DAY18 = table.Column<int>(type: "integer", nullable: false),
                    DAY19 = table.Column<int>(type: "integer", nullable: false),
                    DAY20 = table.Column<int>(type: "integer", nullable: false),
                    DAY21 = table.Column<int>(type: "integer", nullable: false),
                    DAY22 = table.Column<int>(type: "integer", nullable: false),
                    DAY23 = table.Column<int>(type: "integer", nullable: false),
                    DAY24 = table.Column<int>(type: "integer", nullable: false),
                    DAY25 = table.Column<int>(type: "integer", nullable: false),
                    DAY26 = table.Column<int>(type: "integer", nullable: false),
                    DAY27 = table.Column<int>(type: "integer", nullable: false),
                    DAY28 = table.Column<int>(type: "integer", nullable: false),
                    DAY29 = table.Column<int>(type: "integer", nullable: false),
                    DAY30 = table.Column<int>(type: "integer", nullable: false),
                    DAY31 = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SIN_KOUI", x => new { x.HPID, x.PTID, x.SINYM, x.RPNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_KOUI_COUNT",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SINDAY = table.Column<int>(name: "SIN_DAY", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_KOUI_COUNT", x => new { x.HPID, x.PTID, x.SINYM, x.SINDAY, x.RAIINNO, x.RPNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_KOUI_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    RECID = table.Column<string>(name: "REC_ID", type: "character varying(2)", maxLength: 2, nullable: true),
                    ITEMSBT = table.Column<int>(name: "ITEM_SBT", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ODRITEMCD = table.Column<string>(name: "ODR_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    SURYO2 = table.Column<double>(type: "double precision", nullable: false),
                    FMTKBN = table.Column<int>(name: "FMT_KBN", type: "integer", nullable: false),
                    UNITCD = table.Column<int>(name: "UNIT_CD", type: "integer", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    ZEI = table.Column<double>(type: "double precision", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPPAPERRECE = table.Column<int>(name: "IS_NODSP_PAPER_RECE", type: "integer", nullable: false),
                    ISNODSPRYOSYU = table.Column<int>(name: "IS_NODSP_RYOSYU", type: "integer", nullable: false),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT1 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMTCD1 = table.Column<string>(name: "CMT_CD1", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTOPT1 = table.Column<string>(name: "CMT_OPT1", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT2 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMTCD2 = table.Column<string>(name: "CMT_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTOPT2 = table.Column<string>(name: "CMT_OPT2", type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT3 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMTCD3 = table.Column<string>(name: "CMT_CD3", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTOPT3 = table.Column<string>(name: "CMT_OPT3", type: "character varying(240)", maxLength: 240, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_KOUI_DETAIL", x => new { x.HPID, x.PTID, x.SINYM, x.RPNO, x.SEQNO, x.ROWNO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_RP_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FIRSTDAY = table.Column<int>(name: "FIRST_DAY", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    SINID = table.Column<int>(name: "SIN_ID", type: "integer", nullable: false),
                    CDNO = table.Column<string>(name: "CD_NO", type: "character varying(15)", maxLength: 15, nullable: true),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
                    KOUIDATA = table.Column<string>(name: "KOUI_DATA", type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SIN_RP_INF", x => new { x.HPID, x.PTID, x.SINYM, x.RPNO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_RP_NO_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SINDAY = table.Column<int>(name: "SIN_DAY", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_RP_NO_INF", x => new { x.HPID, x.PTID, x.SINYM, x.SINDAY, x.RAIINNO, x.RPNO });
                });

            migrationBuilder.CreateTable(
                name: "SINGLE_DOSE_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINGLE_DOSE_MST", x => new { x.HPID, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "SINREKI_FILTER_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SINREKI_FILTER_MST", x => new { x.HPID, x.GRPCD });
                });

            migrationBuilder.CreateTable(
                name: "SINREKI_FILTER_MST_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SINREKI_FILTER_MST_DETAIL", x => new { x.HPID, x.GRPCD, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "SOKATU_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    STARTYM = table.Column<int>(name: "START_YM", type: "integer", nullable: false),
                    REPORTID = table.Column<int>(name: "REPORT_ID", type: "integer", nullable: false),
                    REPORTEDANO = table.Column<int>(name: "REPORT_EDA_NO", type: "integer", nullable: false),
                    ENDYM = table.Column<int>(name: "END_YM", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    REPORTNAME = table.Column<string>(name: "REPORT_NAME", type: "character varying(30)", maxLength: 30, nullable: true),
                    PRINTTYPE = table.Column<int>(name: "PRINT_TYPE", type: "integer", nullable: false),
                    PRINTNOTYPE = table.Column<int>(name: "PRINT_NO_TYPE", type: "integer", nullable: false),
                    DATAALL = table.Column<int>(name: "DATA_ALL", type: "integer", nullable: false),
                    DATADISK = table.Column<int>(name: "DATA_DISK", type: "integer", nullable: false),
                    DATAPAPER = table.Column<int>(name: "DATA_PAPER", type: "integer", nullable: false),
                    DATAKBN = table.Column<int>(name: "DATA_KBN", type: "integer", nullable: false),
                    DISKKIND = table.Column<string>(name: "DISK_KIND", type: "character varying(10)", maxLength: 10, nullable: true),
                    DISKCNT = table.Column<int>(name: "DISK_CNT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    ISSORT = table.Column<int>(name: "IS_SORT", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOKATU_MST", x => new { x.HPID, x.PREFNO, x.STARTYM, x.REPORTEDANO, x.REPORTID });
                });

            migrationBuilder.CreateTable(
                name: "STA_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MENUID = table.Column<int>(name: "MENU_ID", type: "integer", nullable: false),
                    CONFID = table.Column<int>(name: "CONF_ID", type: "integer", nullable: false),
                    VAL = table.Column<string>(type: "character varying(1200)", maxLength: 1200, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_CONF", x => new { x.HPID, x.MENUID, x.CONFID });
                });

            migrationBuilder.CreateTable(
                name: "STA_CSV",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    REPORTID = table.Column<int>(name: "REPORT_ID", type: "integer", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    CONFNAME = table.Column<string>(name: "CONF_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    DATASBT = table.Column<int>(name: "DATA_SBT", type: "integer", nullable: false),
                    COLUMNS = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SORTKBN = table.Column<int>(name: "SORT_KBN", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_CSV", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "STA_GRP",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    REPORTID = table.Column<int>(name: "REPORT_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_GRP", x => new { x.HPID, x.GRPID, x.REPORTID });
                });

            migrationBuilder.CreateTable(
                name: "STA_MENU",
                columns: table => new
                {
                    MENUID = table.Column<int>(name: "MENU_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    REPORTID = table.Column<int>(name: "REPORT_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    MENUNAME = table.Column<string>(name: "MENU_NAME", type: "character varying(130)", maxLength: 130, nullable: true),
                    ISPRINT = table.Column<int>(name: "IS_PRINT", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_STA_MENU", x => x.MENUID);
                });

            migrationBuilder.CreateTable(
                name: "STA_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    REPORTID = table.Column<int>(name: "REPORT_ID", type: "integer", nullable: false),
                    REPORTNAME = table.Column<string>(name: "REPORT_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_MST", x => new { x.HPID, x.REPORTID });
                });

            migrationBuilder.CreateTable(
                name: "SUMMARY_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RTEXT = table.Column<byte[]>(type: "bytea", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUMMARY_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYOBYO_KEIKA",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SINDAY = table.Column<int>(name: "SIN_DAY", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KEIKA = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SYOBYO_KEIKA", x => new { x.HPID, x.PTID, x.SINYM, x.SINDAY, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SYOUKI_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    SYOUKIKBN = table.Column<int>(name: "SYOUKI_KBN", type: "integer", nullable: false),
                    SYOUKI = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SYOUKI_INF", x => new { x.HPID, x.PTID, x.SINYM, x.HOKENID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SYOUKI_KBN_MST",
                columns: table => new
                {
                    SYOUKIKBN = table.Column<int>(name: "SYOUKI_KBN", type: "integer", nullable: false),
                    STARTYM = table.Column<int>(name: "START_YM", type: "integer", nullable: false),
                    ENDYM = table.Column<int>(name: "END_YM", type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYOUKI_KBN_MST", x => new { x.SYOUKIKBN, x.STARTYM });
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CHANGE_LOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "text", nullable: true),
                    VERSION = table.Column<string>(type: "text", nullable: true),
                    ISPG = table.Column<int>(name: "IS_PG", type: "integer", nullable: false),
                    ISDB = table.Column<int>(name: "IS_DB", type: "integer", nullable: false),
                    ISMASTER = table.Column<int>(name: "IS_MASTER", type: "integer", nullable: false),
                    ISRUN = table.Column<int>(name: "IS_RUN", type: "integer", nullable: false),
                    ISNOTE = table.Column<int>(name: "IS_NOTE", type: "integer", nullable: false),
                    ISDRUGPHOTO = table.Column<int>(name: "IS_DRUG_PHOTO", type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    ERRMESSAGE = table.Column<string>(name: "ERR_MESSAGE", type: "text", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CHANGE_LOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    GRPEDANO = table.Column<int>(name: "GRP_EDA_NO", type: "integer", nullable: false),
                    VAL = table.Column<double>(type: "double precision", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CONF", x => new { x.HPID, x.GRPCD, x.GRPEDANO });
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CONF_ITEM",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MENUID = table.Column<int>(name: "MENU_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAMMIN = table.Column<int>(name: "PARAM_MIN", type: "integer", nullable: false),
                    PARAMMAX = table.Column<int>(name: "PARAM_MAX", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CONF_ITEM", x => new { x.HPID, x.MENUID, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CONF_MENU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MENUID = table.Column<int>(name: "MENU_ID", type: "integer", nullable: false),
                    MENUGRP = table.Column<int>(name: "MENU_GRP", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    MENUNAME = table.Column<string>(name: "MENU_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    GRPEDANO = table.Column<int>(name: "GRP_EDA_NO", type: "integer", nullable: false),
                    PATHGRPCD = table.Column<int>(name: "PATH_GRP_CD", type: "integer", nullable: false),
                    ISPARAM = table.Column<int>(name: "IS_PARAM", type: "integer", nullable: false),
                    PARAMMASK = table.Column<int>(name: "PARAM_MASK", type: "integer", nullable: false),
                    PARAMTYPE = table.Column<int>(name: "PARAM_TYPE", type: "integer", nullable: false),
                    PARAMHINT = table.Column<string>(name: "PARAM_HINT", type: "character varying(100)", maxLength: 100, nullable: true),
                    VALMIN = table.Column<double>(name: "VAL_MIN", type: "double precision", nullable: false),
                    VALMAX = table.Column<double>(name: "VAL_MAX", type: "double precision", nullable: false),
                    PARAMMIN = table.Column<double>(name: "PARAM_MIN", type: "double precision", nullable: false),
                    PARAMMAX = table.Column<double>(name: "PARAM_MAX", type: "double precision", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    ISVISIBLE = table.Column<int>(name: "IS_VISIBLE", type: "integer", nullable: false),
                    MANAGERKBN = table.Column<int>(name: "MANAGER_KBN", type: "integer", nullable: false),
                    ISVALUE = table.Column<int>(name: "IS_VALUE", type: "integer", nullable: false),
                    PARAMMAXLENGTH = table.Column<int>(name: "PARAM_MAX_LENGTH", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CONF_MENU", x => new { x.HPID, x.MENUID });
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_GENERATION_CONF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    GRPEDANO = table.Column<int>(name: "GRP_EDA_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_GENERATION_CONF", x => new { x.HPID, x.GRPEDANO, x.GRPCD, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "SYUNO_NYUKIN",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ADJUSTFUTAN = table.Column<int>(name: "ADJUST_FUTAN", type: "integer", nullable: false),
                    NYUKINGAKU = table.Column<int>(name: "NYUKIN_GAKU", type: "integer", nullable: false),
                    PAYMENTMETHODCD = table.Column<int>(name: "PAYMENT_METHOD_CD", type: "integer", nullable: false),
                    NYUKINDATE = table.Column<int>(name: "NYUKIN_DATE", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    NYUKINCMT = table.Column<string>(name: "NYUKIN_CMT", type: "character varying(100)", maxLength: 100, nullable: true),
                    NYUKINJITENSU = table.Column<int>(name: "NYUKINJI_TENSU", type: "integer", nullable: false),
                    NYUKINJISEIKYU = table.Column<int>(name: "NYUKINJI_SEIKYU", type: "integer", nullable: false),
                    NYUKINJIDETAIL = table.Column<string>(name: "NYUKINJI_DETAIL", type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SYUNO_NYUKIN", x => new { x.HPID, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "SYUNO_SEIKYU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    NYUKINKBN = table.Column<int>(name: "NYUKIN_KBN", type: "integer", nullable: false),
                    SEIKYUTENSU = table.Column<int>(name: "SEIKYU_TENSU", type: "integer", nullable: false),
                    ADJUSTFUTAN = table.Column<int>(name: "ADJUST_FUTAN", type: "integer", nullable: false),
                    SEIKYUGAKU = table.Column<int>(name: "SEIKYU_GAKU", type: "integer", nullable: false),
                    SEIKYUDETAIL = table.Column<string>(name: "SEIKYU_DETAIL", type: "text", nullable: true),
                    NEWSEIKYUTENSU = table.Column<int>(name: "NEW_SEIKYU_TENSU", type: "integer", nullable: false),
                    NEWADJUSTFUTAN = table.Column<int>(name: "NEW_ADJUST_FUTAN", type: "integer", nullable: false),
                    NEWSEIKYUGAKU = table.Column<int>(name: "NEW_SEIKYU_GAKU", type: "integer", nullable: false),
                    NEWSEIKYUDETAIL = table.Column<string>(name: "NEW_SEIKYU_DETAIL", type: "text", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYUNO_SEIKYU", x => new { x.HPID, x.RAIINNO, x.PTID, x.SINDATE });
                });

            migrationBuilder.CreateTable(
                name: "TAG_GRP_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TAGGRPNO = table.Column<int>(name: "TAG_GRP_NO", type: "integer", nullable: false),
                    TAGGRPNAME = table.Column<string>(name: "TAG_GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    GRPCOLOR = table.Column<string>(name: "GRP_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_TAG_GRP_MST", x => new { x.HPID, x.TAGGRPNO });
                });

            migrationBuilder.CreateTable(
                name: "TEKIOU_BYOMEI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: false),
                    SYSTEMDATA = table.Column<int>(name: "SYSTEM_DATA", type: "integer", nullable: false),
                    STARTYM = table.Column<int>(name: "START_YM", type: "integer", nullable: false),
                    ENDYM = table.Column<int>(name: "END_YM", type: "integer", nullable: false),
                    ISINVALID = table.Column<int>(name: "IS_INVALID", type: "integer", nullable: false),
                    ISINVALIDTOKUSYO = table.Column<int>(name: "IS_INVALID_TOKUSYO", type: "integer", nullable: false),
                    EDITKBN = table.Column<int>(name: "EDIT_KBN", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEKIOU_BYOMEI_MST", x => new { x.HPID, x.ITEMCD, x.BYOMEICD, x.SYSTEMDATA });
                });

            migrationBuilder.CreateTable(
                name: "TEKIOU_BYOMEI_MST_EXCLUDED",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_TEKIOU_BYOMEI_MST_EXCLUDED", x => new { x.HPID, x.ITEMCD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TEMPLATECD = table.Column<int>(name: "TEMPLATE_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CONTROLID = table.Column<int>(name: "CONTROL_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    OYACONTROLID = table.Column<int>(name: "OYA_CONTROL_ID", type: "integer", nullable: true),
                    TITLE = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CONTROLTYPE = table.Column<int>(name: "CONTROL_TYPE", type: "integer", nullable: false),
                    MENUKBN = table.Column<int>(name: "MENU_KBN", type: "integer", nullable: false),
                    DEFAULTVAL = table.Column<string>(name: "DEFAULT_VAL", type: "character varying(200)", maxLength: 200, nullable: true),
                    UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    NEWLINE = table.Column<int>(name: "NEW_LINE", type: "integer", nullable: false),
                    KARTEKBN = table.Column<int>(name: "KARTE_KBN", type: "integer", nullable: false),
                    CONTROLWIDTH = table.Column<int>(name: "CONTROL_WIDTH", type: "integer", nullable: false),
                    TITLEWIDTH = table.Column<int>(name: "TITLE_WIDTH", type: "integer", nullable: false),
                    UNITWIDTH = table.Column<int>(name: "UNIT_WIDTH", type: "integer", nullable: false),
                    LEFTMARGIN = table.Column<int>(name: "LEFT_MARGIN", type: "integer", nullable: false),
                    WORDWRAP = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<double>(type: "double precision", nullable: true),
                    FORMULA = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DECIMAL = table.Column<int>(type: "integer", nullable: false),
                    IME = table.Column<int>(type: "integer", nullable: false),
                    COLCOUNT = table.Column<int>(name: "COL_COUNT", type: "integer", nullable: false),
                    RENKEICD = table.Column<string>(name: "RENKEI_CD", type: "character varying(20)", maxLength: 20, nullable: true),
                    BACKGROUNDCOLOR = table.Column<string>(name: "BACKGROUND_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    FONTCOLOR = table.Column<string>(name: "FONT_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    FONTBOLD = table.Column<int>(name: "FONT_BOLD", type: "integer", nullable: false),
                    FONTITALIC = table.Column<int>(name: "FONT_ITALIC", type: "integer", nullable: false),
                    FONTUNDERLINE = table.Column<int>(name: "FONT_UNDER_LINE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_DETAIL", x => new { x.HPID, x.TEMPLATECD, x.SEQNO, x.CONTROLID });
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_DSP_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TEMPLATECD = table.Column<int>(name: "TEMPLATE_CD", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DSPKBN = table.Column<int>(name: "DSP_KBN", type: "integer", nullable: false),
                    ISDSP = table.Column<int>(name: "IS_DSP", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_DSP_CONF", x => new { x.HPID, x.TEMPLATECD, x.SEQNO, x.DSPKBN });
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_MENU_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MENUKBN = table.Column<int>(name: "MENU_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    VAL = table.Column<double>(type: "double precision", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_MENU_DETAIL", x => new { x.HPID, x.MENUKBN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_MENU_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    MENUKBN = table.Column<int>(name: "MENU_KBN", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBNNAME = table.Column<string>(name: "KBN_NAME", type: "character varying(30)", maxLength: 30, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_MENU_MST", x => new { x.HPID, x.MENUKBN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TEMPLATECD = table.Column<int>(name: "TEMPLATE_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    INSERTIONDESTINATION = table.Column<int>(name: "INSERTION_DESTINATION", type: "integer", nullable: false),
                    TITLE = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_TEMPLATE_MST", x => new { x.HPID, x.TEMPLATECD, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "TEN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    MASTERSBT = table.Column<string>(name: "MASTER_SBT", type: "character varying(1)", maxLength: 1, nullable: true),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    KANANAME1 = table.Column<string>(name: "KANA_NAME1", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME2 = table.Column<string>(name: "KANA_NAME2", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME3 = table.Column<string>(name: "KANA_NAME3", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME4 = table.Column<string>(name: "KANA_NAME4", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME5 = table.Column<string>(name: "KANA_NAME5", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME6 = table.Column<string>(name: "KANA_NAME6", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME7 = table.Column<string>(name: "KANA_NAME7", type: "character varying(120)", maxLength: 120, nullable: true),
                    RYOSYUNAME = table.Column<string>(name: "RYOSYU_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    RECENAME = table.Column<string>(name: "RECE_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    TENID = table.Column<int>(name: "TEN_ID", type: "integer", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    RECEUNITCD = table.Column<string>(name: "RECE_UNIT_CD", type: "character varying(3)", maxLength: 3, nullable: true),
                    RECEUNITNAME = table.Column<string>(name: "RECE_UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    ODRUNITNAME = table.Column<string>(name: "ODR_UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    CNVUNITNAME = table.Column<string>(name: "CNV_UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    ODRTERMVAL = table.Column<double>(name: "ODR_TERM_VAL", type: "double precision", nullable: false),
                    CNVTERMVAL = table.Column<double>(name: "CNV_TERM_VAL", type: "double precision", nullable: false),
                    DEFAULTVAL = table.Column<double>(name: "DEFAULT_VAL", type: "double precision", nullable: false),
                    ISADOPTED = table.Column<int>(name: "IS_ADOPTED", type: "integer", nullable: false),
                    KOUKIKBN = table.Column<int>(name: "KOUKI_KBN", type: "integer", nullable: false),
                    HOKATUKENSA = table.Column<int>(name: "HOKATU_KENSA", type: "integer", nullable: false),
                    BYOMEIKBN = table.Column<int>(name: "BYOMEI_KBN", type: "integer", nullable: false),
                    IGAKUKANRI = table.Column<int>(type: "integer", nullable: false),
                    JITUDAYCOUNT = table.Column<int>(name: "JITUDAY_COUNT", type: "integer", nullable: false),
                    JITUDAY = table.Column<int>(type: "integer", nullable: false),
                    DAYCOUNT = table.Column<int>(name: "DAY_COUNT", type: "integer", nullable: false),
                    DRUGKANRENKBN = table.Column<int>(name: "DRUG_KANREN_KBN", type: "integer", nullable: false),
                    KIZAMIID = table.Column<int>(name: "KIZAMI_ID", type: "integer", nullable: false),
                    KIZAMIMIN = table.Column<int>(name: "KIZAMI_MIN", type: "integer", nullable: false),
                    KIZAMIMAX = table.Column<int>(name: "KIZAMI_MAX", type: "integer", nullable: false),
                    KIZAMIVAL = table.Column<int>(name: "KIZAMI_VAL", type: "integer", nullable: false),
                    KIZAMITEN = table.Column<double>(name: "KIZAMI_TEN", type: "double precision", nullable: false),
                    KIZAMIERR = table.Column<int>(name: "KIZAMI_ERR", type: "integer", nullable: false),
                    MAXCOUNT = table.Column<int>(name: "MAX_COUNT", type: "integer", nullable: false),
                    MAXCOUNTERR = table.Column<int>(name: "MAX_COUNT_ERR", type: "integer", nullable: false),
                    TYUCD = table.Column<string>(name: "TYU_CD", type: "character varying(4)", maxLength: 4, nullable: true),
                    TYUSEQ = table.Column<string>(name: "TYU_SEQ", type: "character varying(1)", maxLength: 1, nullable: true),
                    TUSOKUAGE = table.Column<int>(name: "TUSOKU_AGE", type: "integer", nullable: false),
                    MINAGE = table.Column<string>(name: "MIN_AGE", type: "character varying(2)", maxLength: 2, nullable: true),
                    MAXAGE = table.Column<string>(name: "MAX_AGE", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGECHECK = table.Column<int>(name: "AGE_CHECK", type: "integer", nullable: false),
                    TIMEKASANKBN = table.Column<int>(name: "TIME_KASAN_KBN", type: "integer", nullable: false),
                    FUTEKIKBN = table.Column<int>(name: "FUTEKI_KBN", type: "integer", nullable: false),
                    FUTEKISISETUKBN = table.Column<int>(name: "FUTEKI_SISETU_KBN", type: "integer", nullable: false),
                    SYOTINYUYOJIKBN = table.Column<int>(name: "SYOTI_NYUYOJI_KBN", type: "integer", nullable: false),
                    LOWWEIGHTKBN = table.Column<int>(name: "LOW_WEIGHT_KBN", type: "integer", nullable: false),
                    HANDANKBN = table.Column<int>(name: "HANDAN_KBN", type: "integer", nullable: false),
                    HANDANGRPKBN = table.Column<int>(name: "HANDAN_GRP_KBN", type: "integer", nullable: false),
                    TEIGENKBN = table.Column<int>(name: "TEIGEN_KBN", type: "integer", nullable: false),
                    SEKITUIKBN = table.Column<int>(name: "SEKITUI_KBN", type: "integer", nullable: false),
                    KEIBUKBN = table.Column<int>(name: "KEIBU_KBN", type: "integer", nullable: false),
                    AUTOHOUGOUKBN = table.Column<int>(name: "AUTO_HOUGOU_KBN", type: "integer", nullable: false),
                    GAIRAIKANRIKBN = table.Column<int>(name: "GAIRAI_KANRI_KBN", type: "integer", nullable: false),
                    TUSOKUTARGETKBN = table.Column<int>(name: "TUSOKU_TARGET_KBN", type: "integer", nullable: false),
                    HOKATUKBN = table.Column<int>(name: "HOKATU_KBN", type: "integer", nullable: false),
                    TYOONPANAISIKBN = table.Column<int>(name: "TYOONPA_NAISI_KBN", type: "integer", nullable: false),
                    AUTOFUNGOKBN = table.Column<int>(name: "AUTO_FUNGO_KBN", type: "integer", nullable: false),
                    TYOONPAGYOKOKBN = table.Column<int>(name: "TYOONPA_GYOKO_KBN", type: "integer", nullable: false),
                    GAZOKASAN = table.Column<int>(name: "GAZO_KASAN", type: "integer", nullable: false),
                    KANSATUKBN = table.Column<int>(name: "KANSATU_KBN", type: "integer", nullable: false),
                    MASUIKBN = table.Column<int>(name: "MASUI_KBN", type: "integer", nullable: false),
                    FUKUBIKUNAISIKASAN = table.Column<int>(name: "FUKUBIKU_NAISI_KASAN", type: "integer", nullable: false),
                    FUKUBIKUKOTUNANKASAN = table.Column<int>(name: "FUKUBIKU_KOTUNAN_KASAN", type: "integer", nullable: false),
                    MASUIKASAN = table.Column<int>(name: "MASUI_KASAN", type: "integer", nullable: false),
                    MONITERKASAN = table.Column<int>(name: "MONITER_KASAN", type: "integer", nullable: false),
                    TOKETUKASAN = table.Column<int>(name: "TOKETU_KASAN", type: "integer", nullable: false),
                    TENKBNNO = table.Column<string>(name: "TEN_KBN_NO", type: "character varying(30)", maxLength: 30, nullable: true),
                    SHORTSTAYOPE = table.Column<int>(name: "SHORTSTAY_OPE", type: "integer", nullable: false),
                    BUIKBN = table.Column<int>(name: "BUI_KBN", type: "integer", nullable: false),
                    SISETUCD1 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD2 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD3 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD4 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD5 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD6 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD7 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD8 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD9 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD10 = table.Column<int>(type: "integer", nullable: false),
                    AGEKASANMIN1 = table.Column<string>(name: "AGEKASAN_MIN1", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX1 = table.Column<string>(name: "AGEKASAN_MAX1", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD1 = table.Column<string>(name: "AGEKASAN_CD1", type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASANMIN2 = table.Column<string>(name: "AGEKASAN_MIN2", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX2 = table.Column<string>(name: "AGEKASAN_MAX2", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD2 = table.Column<string>(name: "AGEKASAN_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASANMIN3 = table.Column<string>(name: "AGEKASAN_MIN3", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX3 = table.Column<string>(name: "AGEKASAN_MAX3", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD3 = table.Column<string>(name: "AGEKASAN_CD3", type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASANMIN4 = table.Column<string>(name: "AGEKASAN_MIN4", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX4 = table.Column<string>(name: "AGEKASAN_MAX4", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD4 = table.Column<string>(name: "AGEKASAN_CD4", type: "character varying(10)", maxLength: 10, nullable: true),
                    KENSACMT = table.Column<int>(name: "KENSA_CMT", type: "integer", nullable: false),
                    MADOKUKBN = table.Column<int>(name: "MADOKU_KBN", type: "integer", nullable: false),
                    SINKEIKBN = table.Column<int>(name: "SINKEI_KBN", type: "integer", nullable: false),
                    SEIBUTUKBN = table.Column<int>(name: "SEIBUTU_KBN", type: "integer", nullable: false),
                    ZOUEIKBN = table.Column<int>(name: "ZOUEI_KBN", type: "integer", nullable: false),
                    DRUGKBN = table.Column<int>(name: "DRUG_KBN", type: "integer", nullable: false),
                    ZAIKBN = table.Column<int>(name: "ZAI_KBN", type: "integer", nullable: false),
                    CAPACITY = table.Column<int>(type: "integer", nullable: false),
                    KOHATUKBN = table.Column<int>(name: "KOHATU_KBN", type: "integer", nullable: false),
                    TOKUZAIAGEKBN = table.Column<int>(name: "TOKUZAI_AGE_KBN", type: "integer", nullable: false),
                    SANSOKBN = table.Column<int>(name: "SANSO_KBN", type: "integer", nullable: false),
                    TOKUZAISBT = table.Column<int>(name: "TOKUZAI_SBT", type: "integer", nullable: false),
                    MAXPRICE = table.Column<int>(name: "MAX_PRICE", type: "integer", nullable: false),
                    MAXTEN = table.Column<int>(name: "MAX_TEN", type: "integer", nullable: false),
                    SYUKEISAKI = table.Column<string>(name: "SYUKEI_SAKI", type: "character varying(3)", maxLength: 3, nullable: true),
                    CDKBN = table.Column<string>(name: "CD_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    CDSYO = table.Column<int>(name: "CD_SYO", type: "integer", nullable: false),
                    CDBU = table.Column<int>(name: "CD_BU", type: "integer", nullable: false),
                    CDKBNNO = table.Column<int>(name: "CD_KBNNO", type: "integer", nullable: false),
                    CDEDANO = table.Column<int>(name: "CD_EDANO", type: "integer", nullable: false),
                    CDKOUNO = table.Column<int>(name: "CD_KOUNO", type: "integer", nullable: false),
                    KOKUJIKBN = table.Column<string>(name: "KOKUJI_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJISYO = table.Column<int>(name: "KOKUJI_SYO", type: "integer", nullable: false),
                    KOKUJIBU = table.Column<int>(name: "KOKUJI_BU", type: "integer", nullable: false),
                    KOKUJIKBNNO = table.Column<int>(name: "KOKUJI_KBN_NO", type: "integer", nullable: false),
                    KOKUJIEDANO = table.Column<int>(name: "KOKUJI_EDA_NO", type: "integer", nullable: false),
                    KOKUJIKOUNO = table.Column<int>(name: "KOKUJI_KOU_NO", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOHYOJUN = table.Column<int>(name: "KOHYO_JUN", type: "integer", nullable: false),
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    YAKKACD = table.Column<string>(name: "YAKKA_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    SYUSAISBT = table.Column<int>(name: "SYUSAI_SBT", type: "integer", nullable: false),
                    SYOHINKANREN = table.Column<string>(name: "SYOHIN_KANREN", type: "character varying(9)", maxLength: 9, nullable: true),
                    UPDDATE = table.Column<int>(name: "UPD_DATE", type: "integer", nullable: false),
                    DELDATE = table.Column<int>(name: "DEL_DATE", type: "integer", nullable: false),
                    KEIKADATE = table.Column<int>(name: "KEIKA_DATE", type: "integer", nullable: false),
                    KOKUJIBETUNO = table.Column<int>(name: "KOKUJI_BETUNO", type: "integer", nullable: false),
                    KOKUJIKBNNO0 = table.Column<int>(name: "KOKUJI_KBNNO", type: "integer", nullable: false),
                    ROUSAIKBN = table.Column<int>(name: "ROUSAI_KBN", type: "integer", nullable: false),
                    SISIKBN = table.Column<int>(name: "SISI_KBN", type: "integer", nullable: false),
                    SHOTCNT = table.Column<int>(name: "SHOT_CNT", type: "integer", nullable: false),
                    ISNOSEARCH = table.Column<int>(name: "IS_NOSEARCH", type: "integer", nullable: false),
                    ISNODSPPAPERRECE = table.Column<int>(name: "IS_NODSP_PAPER_RECE", type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPRYOSYU = table.Column<int>(name: "IS_NODSP_RYOSYU", type: "integer", nullable: false),
                    ISNODSPKARTE = table.Column<int>(name: "IS_NODSP_KARTE", type: "integer", nullable: false),
                    JIHISBT = table.Column<int>(name: "JIHI_SBT", type: "integer", nullable: false),
                    KAZEIKBN = table.Column<int>(name: "KAZEI_KBN", type: "integer", nullable: false),
                    YOHOKBN = table.Column<int>(name: "YOHO_KBN", type: "integer", nullable: false),
                    IPNNAMECD = table.Column<string>(name: "IPN_NAME_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    FUKUYORISE = table.Column<int>(name: "FUKUYO_RISE", type: "integer", nullable: false),
                    FUKUYOMORNING = table.Column<int>(name: "FUKUYO_MORNING", type: "integer", nullable: false),
                    FUKUYODAYTIME = table.Column<int>(name: "FUKUYO_DAYTIME", type: "integer", nullable: false),
                    FUKUYONIGHT = table.Column<int>(name: "FUKUYO_NIGHT", type: "integer", nullable: false),
                    FUKUYOSLEEP = table.Column<int>(name: "FUKUYO_SLEEP", type: "integer", nullable: false),
                    ISNODSPYAKUTAI = table.Column<int>(name: "IS_NODSP_YAKUTAI", type: "integer", nullable: false),
                    ZAIKEIPOINT = table.Column<double>(name: "ZAIKEI_POINT", type: "double precision", nullable: false),
                    SURYOROUNDUPKBN = table.Column<int>(name: "SURYO_ROUNDUP_KBN", type: "integer", nullable: false),
                    KOUSEISINKBN = table.Column<int>(name: "KOUSEISIN_KBN", type: "integer", nullable: false),
                    CHUSYADRUGSBT = table.Column<int>(name: "CHUSYA_DRUG_SBT", type: "integer", nullable: false),
                    KENSAFUKUSUSANTEI = table.Column<int>(name: "KENSA_FUKUSU_SANTEI", type: "integer", nullable: false),
                    SANTEIITEMCD = table.Column<string>(name: "SANTEI_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SANTEIGAIKBN = table.Column<int>(name: "SANTEIGAI_KBN", type: "integer", nullable: false),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(20)", maxLength: 20, nullable: true),
                    KENSAITEMSEQNO = table.Column<int>(name: "KENSA_ITEM_SEQ_NO", type: "integer", nullable: false),
                    RENKEICD1 = table.Column<string>(name: "RENKEI_CD1", type: "character varying(20)", maxLength: 20, nullable: true),
                    RENKEICD2 = table.Column<string>(name: "RENKEI_CD2", type: "character varying(20)", maxLength: 20, nullable: true),
                    SAIKETUKBN = table.Column<int>(name: "SAIKETU_KBN", type: "integer", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    CMTCOL1 = table.Column<int>(name: "CMT_COL1", type: "integer", nullable: false),
                    CMTCOLKETA1 = table.Column<int>(name: "CMT_COL_KETA1", type: "integer", nullable: false),
                    CMTCOL2 = table.Column<int>(name: "CMT_COL2", type: "integer", nullable: false),
                    CMTCOLKETA2 = table.Column<int>(name: "CMT_COL_KETA2", type: "integer", nullable: false),
                    CMTCOL3 = table.Column<int>(name: "CMT_COL3", type: "integer", nullable: false),
                    CMTCOLKETA3 = table.Column<int>(name: "CMT_COL_KETA3", type: "integer", nullable: false),
                    CMTCOL4 = table.Column<int>(name: "CMT_COL4", type: "integer", nullable: false),
                    CMTCOLKETA4 = table.Column<int>(name: "CMT_COL_KETA4", type: "integer", nullable: false),
                    SELECTCMTID = table.Column<int>(name: "SELECT_CMT_ID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    CMTSBT = table.Column<int>(name: "CMT_SBT", type: "integer", nullable: false),
                    KENSALABEL = table.Column<int>(name: "KENSA_LABEL", type: "integer", nullable: false),
                    GAIRAIKANSEN = table.Column<int>(name: "GAIRAI_KANSEN", type: "integer", nullable: false),
                    JIBIAGEKASAN = table.Column<int>(name: "JIBI_AGE_KASAN", type: "integer", nullable: false),
                    JIBISYONIKOKIN = table.Column<int>(name: "JIBI_SYONIKOKIN", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEN_MST", x => new { x.HPID, x.ITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "TEN_MST_MOTHER",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    MASTERSBT = table.Column<string>(name: "MASTER_SBT", type: "character varying(1)", maxLength: 1, nullable: true),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    KANANAME1 = table.Column<string>(name: "KANA_NAME1", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME2 = table.Column<string>(name: "KANA_NAME2", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME3 = table.Column<string>(name: "KANA_NAME3", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME4 = table.Column<string>(name: "KANA_NAME4", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME5 = table.Column<string>(name: "KANA_NAME5", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME6 = table.Column<string>(name: "KANA_NAME6", type: "character varying(120)", maxLength: 120, nullable: true),
                    KANANAME7 = table.Column<string>(name: "KANA_NAME7", type: "character varying(120)", maxLength: 120, nullable: true),
                    RYOSYUNAME = table.Column<string>(name: "RYOSYU_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    RECENAME = table.Column<string>(name: "RECE_NAME", type: "character varying(240)", maxLength: 240, nullable: true),
                    TENID = table.Column<int>(name: "TEN_ID", type: "integer", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    RECEUNITCD = table.Column<string>(name: "RECE_UNIT_CD", type: "character varying(3)", maxLength: 3, nullable: true),
                    RECEUNITNAME = table.Column<string>(name: "RECE_UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    ODRUNITNAME = table.Column<string>(name: "ODR_UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    CNVUNITNAME = table.Column<string>(name: "CNV_UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    ODRTERMVAL = table.Column<double>(name: "ODR_TERM_VAL", type: "double precision", nullable: false),
                    CNVTERMVAL = table.Column<double>(name: "CNV_TERM_VAL", type: "double precision", nullable: false),
                    DEFAULTVAL = table.Column<double>(name: "DEFAULT_VAL", type: "double precision", nullable: false),
                    ISADOPTED = table.Column<int>(name: "IS_ADOPTED", type: "integer", nullable: false),
                    KOUKIKBN = table.Column<int>(name: "KOUKI_KBN", type: "integer", nullable: false),
                    HOKATUKENSA = table.Column<int>(name: "HOKATU_KENSA", type: "integer", nullable: false),
                    BYOMEIKBN = table.Column<int>(name: "BYOMEI_KBN", type: "integer", nullable: false),
                    IGAKUKANRI = table.Column<int>(type: "integer", nullable: false),
                    JITUDAYCOUNT = table.Column<int>(name: "JITUDAY_COUNT", type: "integer", nullable: false),
                    JITUDAY = table.Column<int>(type: "integer", nullable: false),
                    DAYCOUNT = table.Column<int>(name: "DAY_COUNT", type: "integer", nullable: false),
                    DRUGKANRENKBN = table.Column<int>(name: "DRUG_KANREN_KBN", type: "integer", nullable: false),
                    KIZAMIID = table.Column<int>(name: "KIZAMI_ID", type: "integer", nullable: false),
                    KIZAMIMIN = table.Column<int>(name: "KIZAMI_MIN", type: "integer", nullable: false),
                    KIZAMIMAX = table.Column<int>(name: "KIZAMI_MAX", type: "integer", nullable: false),
                    KIZAMIVAL = table.Column<int>(name: "KIZAMI_VAL", type: "integer", nullable: false),
                    KIZAMITEN = table.Column<double>(name: "KIZAMI_TEN", type: "double precision", nullable: false),
                    KIZAMIERR = table.Column<int>(name: "KIZAMI_ERR", type: "integer", nullable: false),
                    MAXCOUNT = table.Column<int>(name: "MAX_COUNT", type: "integer", nullable: false),
                    MAXCOUNTERR = table.Column<int>(name: "MAX_COUNT_ERR", type: "integer", nullable: false),
                    TYUCD = table.Column<string>(name: "TYU_CD", type: "character varying(4)", maxLength: 4, nullable: true),
                    TYUSEQ = table.Column<string>(name: "TYU_SEQ", type: "character varying(1)", maxLength: 1, nullable: true),
                    TUSOKUAGE = table.Column<int>(name: "TUSOKU_AGE", type: "integer", nullable: false),
                    MINAGE = table.Column<string>(name: "MIN_AGE", type: "character varying(2)", maxLength: 2, nullable: true),
                    MAXAGE = table.Column<string>(name: "MAX_AGE", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGECHECK = table.Column<int>(name: "AGE_CHECK", type: "integer", nullable: false),
                    TIMEKASANKBN = table.Column<int>(name: "TIME_KASAN_KBN", type: "integer", nullable: false),
                    FUTEKIKBN = table.Column<int>(name: "FUTEKI_KBN", type: "integer", nullable: false),
                    FUTEKISISETUKBN = table.Column<int>(name: "FUTEKI_SISETU_KBN", type: "integer", nullable: false),
                    SYOTINYUYOJIKBN = table.Column<int>(name: "SYOTI_NYUYOJI_KBN", type: "integer", nullable: false),
                    LOWWEIGHTKBN = table.Column<int>(name: "LOW_WEIGHT_KBN", type: "integer", nullable: false),
                    HANDANKBN = table.Column<int>(name: "HANDAN_KBN", type: "integer", nullable: false),
                    HANDANGRPKBN = table.Column<int>(name: "HANDAN_GRP_KBN", type: "integer", nullable: false),
                    TEIGENKBN = table.Column<int>(name: "TEIGEN_KBN", type: "integer", nullable: false),
                    SEKITUIKBN = table.Column<int>(name: "SEKITUI_KBN", type: "integer", nullable: false),
                    KEIBUKBN = table.Column<int>(name: "KEIBU_KBN", type: "integer", nullable: false),
                    AUTOHOUGOUKBN = table.Column<int>(name: "AUTO_HOUGOU_KBN", type: "integer", nullable: false),
                    GAIRAIKANRIKBN = table.Column<int>(name: "GAIRAI_KANRI_KBN", type: "integer", nullable: false),
                    TUSOKUTARGETKBN = table.Column<int>(name: "TUSOKU_TARGET_KBN", type: "integer", nullable: false),
                    HOKATUKBN = table.Column<int>(name: "HOKATU_KBN", type: "integer", nullable: false),
                    TYOONPANAISIKBN = table.Column<int>(name: "TYOONPA_NAISI_KBN", type: "integer", nullable: false),
                    AUTOFUNGOKBN = table.Column<int>(name: "AUTO_FUNGO_KBN", type: "integer", nullable: false),
                    TYOONPAGYOKOKBN = table.Column<int>(name: "TYOONPA_GYOKO_KBN", type: "integer", nullable: false),
                    GAZOKASAN = table.Column<int>(name: "GAZO_KASAN", type: "integer", nullable: false),
                    KANSATUKBN = table.Column<int>(name: "KANSATU_KBN", type: "integer", nullable: false),
                    MASUIKBN = table.Column<int>(name: "MASUI_KBN", type: "integer", nullable: false),
                    FUKUBIKUNAISIKASAN = table.Column<int>(name: "FUKUBIKU_NAISI_KASAN", type: "integer", nullable: false),
                    FUKUBIKUKOTUNANKASAN = table.Column<int>(name: "FUKUBIKU_KOTUNAN_KASAN", type: "integer", nullable: false),
                    MASUIKASAN = table.Column<int>(name: "MASUI_KASAN", type: "integer", nullable: false),
                    MONITERKASAN = table.Column<int>(name: "MONITER_KASAN", type: "integer", nullable: false),
                    TOKETUKASAN = table.Column<int>(name: "TOKETU_KASAN", type: "integer", nullable: false),
                    TENKBNNO = table.Column<string>(name: "TEN_KBN_NO", type: "character varying(30)", maxLength: 30, nullable: true),
                    SHORTSTAYOPE = table.Column<int>(name: "SHORTSTAY_OPE", type: "integer", nullable: false),
                    BUIKBN = table.Column<int>(name: "BUI_KBN", type: "integer", nullable: false),
                    SISETUCD1 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD2 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD3 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD4 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD5 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD6 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD7 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD8 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD9 = table.Column<int>(type: "integer", nullable: false),
                    SISETUCD10 = table.Column<int>(type: "integer", nullable: false),
                    AGEKASANMIN1 = table.Column<string>(name: "AGEKASAN_MIN1", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX1 = table.Column<string>(name: "AGEKASAN_MAX1", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD1 = table.Column<string>(name: "AGEKASAN_CD1", type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASANMIN2 = table.Column<string>(name: "AGEKASAN_MIN2", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX2 = table.Column<string>(name: "AGEKASAN_MAX2", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD2 = table.Column<string>(name: "AGEKASAN_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASANMIN3 = table.Column<string>(name: "AGEKASAN_MIN3", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX3 = table.Column<string>(name: "AGEKASAN_MAX3", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD3 = table.Column<string>(name: "AGEKASAN_CD3", type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASANMIN4 = table.Column<string>(name: "AGEKASAN_MIN4", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANMAX4 = table.Column<string>(name: "AGEKASAN_MAX4", type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASANCD4 = table.Column<string>(name: "AGEKASAN_CD4", type: "character varying(10)", maxLength: 10, nullable: true),
                    KENSACMT = table.Column<int>(name: "KENSA_CMT", type: "integer", nullable: false),
                    MADOKUKBN = table.Column<int>(name: "MADOKU_KBN", type: "integer", nullable: false),
                    SINKEIKBN = table.Column<int>(name: "SINKEI_KBN", type: "integer", nullable: false),
                    SEIBUTUKBN = table.Column<int>(name: "SEIBUTU_KBN", type: "integer", nullable: false),
                    ZOUEIKBN = table.Column<int>(name: "ZOUEI_KBN", type: "integer", nullable: false),
                    DRUGKBN = table.Column<int>(name: "DRUG_KBN", type: "integer", nullable: false),
                    ZAIKBN = table.Column<int>(name: "ZAI_KBN", type: "integer", nullable: false),
                    CAPACITY = table.Column<int>(type: "integer", nullable: false),
                    KOHATUKBN = table.Column<int>(name: "KOHATU_KBN", type: "integer", nullable: false),
                    TOKUZAIAGEKBN = table.Column<int>(name: "TOKUZAI_AGE_KBN", type: "integer", nullable: false),
                    SANSOKBN = table.Column<int>(name: "SANSO_KBN", type: "integer", nullable: false),
                    TOKUZAISBT = table.Column<int>(name: "TOKUZAI_SBT", type: "integer", nullable: false),
                    MAXPRICE = table.Column<int>(name: "MAX_PRICE", type: "integer", nullable: false),
                    MAXTEN = table.Column<int>(name: "MAX_TEN", type: "integer", nullable: false),
                    SYUKEISAKI = table.Column<string>(name: "SYUKEI_SAKI", type: "character varying(3)", maxLength: 3, nullable: true),
                    CDKBN = table.Column<string>(name: "CD_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    CDSYO = table.Column<int>(name: "CD_SYO", type: "integer", nullable: false),
                    CDBU = table.Column<int>(name: "CD_BU", type: "integer", nullable: false),
                    CDKBNNO = table.Column<int>(name: "CD_KBNNO", type: "integer", nullable: false),
                    CDEDANO = table.Column<int>(name: "CD_EDANO", type: "integer", nullable: false),
                    CDKOUNO = table.Column<int>(name: "CD_KOUNO", type: "integer", nullable: false),
                    KOKUJIKBN = table.Column<string>(name: "KOKUJI_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJISYO = table.Column<int>(name: "KOKUJI_SYO", type: "integer", nullable: false),
                    KOKUJIBU = table.Column<int>(name: "KOKUJI_BU", type: "integer", nullable: false),
                    KOKUJIKBNNO = table.Column<int>(name: "KOKUJI_KBN_NO", type: "integer", nullable: false),
                    KOKUJIEDANO = table.Column<int>(name: "KOKUJI_EDA_NO", type: "integer", nullable: false),
                    KOKUJIKOUNO = table.Column<int>(name: "KOKUJI_KOU_NO", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOHYOJUN = table.Column<int>(name: "KOHYO_JUN", type: "integer", nullable: false),
                    YJCD = table.Column<string>(name: "YJ_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    YAKKACD = table.Column<string>(name: "YAKKA_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    SYUSAISBT = table.Column<int>(name: "SYUSAI_SBT", type: "integer", nullable: false),
                    SYOHINKANREN = table.Column<string>(name: "SYOHIN_KANREN", type: "character varying(9)", maxLength: 9, nullable: true),
                    UPDDATE = table.Column<int>(name: "UPD_DATE", type: "integer", nullable: false),
                    DELDATE = table.Column<int>(name: "DEL_DATE", type: "integer", nullable: false),
                    KEIKADATE = table.Column<int>(name: "KEIKA_DATE", type: "integer", nullable: false),
                    ROUSAIKBN = table.Column<int>(name: "ROUSAI_KBN", type: "integer", nullable: false),
                    SISIKBN = table.Column<int>(name: "SISI_KBN", type: "integer", nullable: false),
                    SHOTCNT = table.Column<int>(name: "SHOT_CNT", type: "integer", nullable: false),
                    ISNOSEARCH = table.Column<int>(name: "IS_NOSEARCH", type: "integer", nullable: false),
                    ISNODSPPAPERRECE = table.Column<int>(name: "IS_NODSP_PAPER_RECE", type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPRYOSYU = table.Column<int>(name: "IS_NODSP_RYOSYU", type: "integer", nullable: false),
                    ISNODSPKARTE = table.Column<int>(name: "IS_NODSP_KARTE", type: "integer", nullable: false),
                    JIHISBT = table.Column<int>(name: "JIHI_SBT", type: "integer", nullable: false),
                    KAZEIKBN = table.Column<int>(name: "KAZEI_KBN", type: "integer", nullable: false),
                    YOHOKBN = table.Column<int>(name: "YOHO_KBN", type: "integer", nullable: false),
                    IPNNAMECD = table.Column<string>(name: "IPN_NAME_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    FUKUYORISE = table.Column<int>(name: "FUKUYO_RISE", type: "integer", nullable: false),
                    FUKUYOMORNING = table.Column<int>(name: "FUKUYO_MORNING", type: "integer", nullable: false),
                    FUKUYODAYTIME = table.Column<int>(name: "FUKUYO_DAYTIME", type: "integer", nullable: false),
                    FUKUYONIGHT = table.Column<int>(name: "FUKUYO_NIGHT", type: "integer", nullable: false),
                    FUKUYOSLEEP = table.Column<int>(name: "FUKUYO_SLEEP", type: "integer", nullable: false),
                    SURYOROUNDUPKBN = table.Column<int>(name: "SURYO_ROUNDUP_KBN", type: "integer", nullable: false),
                    KOUSEISINKBN = table.Column<int>(name: "KOUSEISIN_KBN", type: "integer", nullable: false),
                    CHUSYADRUGSBT = table.Column<int>(name: "CHUSYA_DRUG_SBT", type: "integer", nullable: false),
                    KENSAFUKUSUSANTEI = table.Column<int>(name: "KENSA_FUKUSU_SANTEI", type: "integer", nullable: false),
                    SANTEIITEMCD = table.Column<string>(name: "SANTEI_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SANTEIGAIKBN = table.Column<int>(name: "SANTEIGAI_KBN", type: "integer", nullable: false),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(20)", maxLength: 20, nullable: true),
                    KENSAITEMSEQNO = table.Column<int>(name: "KENSA_ITEM_SEQ_NO", type: "integer", nullable: false),
                    RENKEICD1 = table.Column<string>(name: "RENKEI_CD1", type: "character varying(20)", maxLength: 20, nullable: true),
                    RENKEICD2 = table.Column<string>(name: "RENKEI_CD2", type: "character varying(20)", maxLength: 20, nullable: true),
                    SAIKETUKBN = table.Column<int>(name: "SAIKETU_KBN", type: "integer", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    CMTCOL1 = table.Column<int>(name: "CMT_COL1", type: "integer", nullable: false),
                    CMTCOLKETA1 = table.Column<int>(name: "CMT_COL_KETA1", type: "integer", nullable: false),
                    CMTCOL2 = table.Column<int>(name: "CMT_COL2", type: "integer", nullable: false),
                    CMTCOLKETA2 = table.Column<int>(name: "CMT_COL_KETA2", type: "integer", nullable: false),
                    CMTCOL3 = table.Column<int>(name: "CMT_COL3", type: "integer", nullable: false),
                    CMTCOLKETA3 = table.Column<int>(name: "CMT_COL_KETA3", type: "integer", nullable: false),
                    CMTCOL4 = table.Column<int>(name: "CMT_COL4", type: "integer", nullable: false),
                    CMTCOLKETA4 = table.Column<int>(name: "CMT_COL_KETA4", type: "integer", nullable: false),
                    SELECTCMTID = table.Column<int>(name: "SELECT_CMT_ID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEN_MST_MOTHER", x => new { x.HPID, x.ITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "TIME_ZONE_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    YOUBIKBN = table.Column<int>(name: "YOUBI_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    ENDTIME = table.Column<int>(name: "END_TIME", type: "integer", nullable: false),
                    TIMEKBN = table.Column<int>(name: "TIME_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_TIME_ZONE_CONF", x => new { x.HPID, x.YOUBIKBN, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "TIME_ZONE_DAY_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    ENDTIME = table.Column<int>(name: "END_TIME", type: "integer", nullable: false),
                    TIMEKBN = table.Column<int>(name: "TIME_KBN", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIME_ZONE_DAY_INF", x => new { x.HPID, x.ID, x.SINDATE });
                });

            migrationBuilder.CreateTable(
                name: "TODO_GRP_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TODOGRPNO = table.Column<int>(name: "TODO_GRP_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TODOGRPNAME = table.Column<string>(name: "TODO_GRP_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    GRPCOLOR = table.Column<string>(name: "GRP_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_TODO_GRP_MST", x => new { x.HPID, x.TODOGRPNO });
                });

            migrationBuilder.CreateTable(
                name: "TODO_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TODONO = table.Column<int>(name: "TODO_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TODOEDANO = table.Column<int>(name: "TODO_EDA_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    TODOKBNNO = table.Column<int>(name: "TODO_KBN_NO", type: "integer", nullable: false),
                    TODOGRPNO = table.Column<int>(name: "TODO_GRP_NO", type: "integer", nullable: false),
                    TANTO = table.Column<int>(type: "integer", nullable: false),
                    TERM = table.Column<int>(type: "integer", nullable: false),
                    CMT1 = table.Column<string>(type: "text", nullable: true),
                    CMT2 = table.Column<string>(type: "text", nullable: true),
                    ISDONE = table.Column<int>(name: "IS_DONE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_TODO_INF", x => new { x.HPID, x.TODONO, x.TODOEDANO, x.PTID });
                });

            migrationBuilder.CreateTable(
                name: "TODO_KBN_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TODOKBNNO = table.Column<int>(name: "TODO_KBN_NO", type: "integer", nullable: false),
                    TODOKBNNAME = table.Column<string>(name: "TODO_KBN_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    ACTCD = table.Column<int>(name: "ACT_CD", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TODO_KBN_MST", x => new { x.HPID, x.TODOKBNNO });
                });

            migrationBuilder.CreateTable(
                name: "TOKKI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TOKKICD = table.Column<string>(name: "TOKKI_CD", type: "character varying(2)", maxLength: 2, nullable: false),
                    TOKKINAME = table.Column<string>(name: "TOKKI_NAME", type: "character varying(20)", maxLength: 20, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOKKI_MST", x => new { x.HPID, x.TOKKICD });
                });

            migrationBuilder.CreateTable(
                name: "UKETUKE_SBT_DAY_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UKETUKE_SBT_DAY_INF", x => new { x.HPID, x.SINDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "UKETUKE_SBT_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    KBNID = table.Column<int>(name: "KBN_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBNNAME = table.Column<string>(name: "KBN_NAME", type: "character varying(20)", maxLength: 20, nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UKETUKE_SBT_MST", x => new { x.HPID, x.KBNID });
                });

            migrationBuilder.CreateTable(
                name: "UNIT_MST",
                columns: table => new
                {
                    UNITCD = table.Column<int>(name: "UNIT_CD", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIT_MST", x => x.UNITCD);
                });

            migrationBuilder.CreateTable(
                name: "USER_CONF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    GRPCD = table.Column<int>(name: "GRP_CD", type: "integer", nullable: false),
                    GRPITEMCD = table.Column<int>(name: "GRP_ITEM_CD", type: "integer", nullable: false),
                    GRPITEMEDANO = table.Column<int>(name: "GRP_ITEM_EDA_NO", type: "integer", nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_CONF", x => new { x.HPID, x.USERID, x.GRPCD, x.GRPITEMCD, x.GRPITEMEDANO });
                });

            migrationBuilder.CreateTable(
                name: "USER_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    JOBCD = table.Column<int>(name: "JOB_CD", type: "integer", nullable: false),
                    MANAGERKBN = table.Column<int>(name: "MANAGER_KBN", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SNAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DRNAME = table.Column<string>(name: "DR_NAME", type: "character varying(40)", maxLength: 40, nullable: true),
                    LOGINID = table.Column<string>(name: "LOGIN_ID", type: "character varying(20)", maxLength: 20, nullable: false),
                    LOGINPASS = table.Column<string>(name: "LOGIN_PASS", type: "character varying(20)", maxLength: 20, nullable: false),
                    MAYAKULICENSENO = table.Column<string>(name: "MAYAKU_LICENSE_NO", type: "character varying(20)", maxLength: 20, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    RENKEICD1 = table.Column<string>(name: "RENKEI_CD1", type: "character varying(14)", maxLength: 14, nullable: true),
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
                    table.PrimaryKey("PK_USER_MST", x => new { x.ID, x.HPID });
                });

            migrationBuilder.CreateTable(
                name: "USER_PERMISSION",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    FUNCTIONCD = table.Column<string>(name: "FUNCTION_CD", type: "character varying(8)", maxLength: 8, nullable: false),
                    PERMISSION = table.Column<int>(type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_PERMISSION", x => new { x.HPID, x.USERID, x.FUNCTIONCD });
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_KOUI",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SYUKEISAKI = table.Column<string>(name: "SYUKEI_SAKI", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOKATUKENSA = table.Column<int>(name: "HOKATU_KENSA", type: "integer", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPPAPERRECE = table.Column<int>(name: "IS_NODSP_PAPER_RECE", type: "integer", nullable: false),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    CDKBN = table.Column<string>(name: "CD_KBN", type: "character varying(2)", maxLength: 2, nullable: true),
                    RECID = table.Column<string>(name: "REC_ID", type: "character varying(2)", maxLength: 2, nullable: true),
                    JIHISBT = table.Column<int>(name: "JIHI_SBT", type: "integer", nullable: false),
                    KAZEIKBN = table.Column<int>(name: "KAZEI_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_WRK_SIN_KOUI", x => new { x.HPID, x.RAIINNO, x.HOKENKBN, x.RPNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_KOUI_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RECID = table.Column<string>(name: "REC_ID", type: "character varying(2)", maxLength: 2, nullable: true),
                    ITEMSBT = table.Column<int>(name: "ITEM_SBT", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ODRITEMCD = table.Column<string>(name: "ODR_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    SURYO2 = table.Column<double>(type: "double precision", nullable: false),
                    FMTKBN = table.Column<int>(name: "FMT_KBN", type: "integer", nullable: false),
                    UNITCD = table.Column<int>(name: "UNIT_CD", type: "integer", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    TENID = table.Column<int>(name: "TEN_ID", type: "integer", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    CDKBN = table.Column<string>(name: "CD_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    CDKBNNO = table.Column<int>(name: "CD_KBNNO", type: "integer", nullable: false),
                    CDEDANO = table.Column<int>(name: "CD_EDANO", type: "integer", nullable: false),
                    CDKOUNO = table.Column<int>(name: "CD_KOUNO", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    TYUCD = table.Column<string>(name: "TYU_CD", type: "character varying(4)", maxLength: 4, nullable: true),
                    TYUSEQ = table.Column<string>(name: "TYU_SEQ", type: "character varying(1)", maxLength: 1, nullable: true),
                    TUSOKUAGE = table.Column<int>(name: "TUSOKU_AGE", type: "integer", nullable: false),
                    ITEMSEQNO = table.Column<int>(name: "ITEM_SEQ_NO", type: "integer", nullable: false),
                    ITEMEDANO = table.Column<int>(name: "ITEM_EDA_NO", type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    ISNODSPPAPERRECE = table.Column<int>(name: "IS_NODSP_PAPER_RECE", type: "integer", nullable: false),
                    ISNODSPRYOSYU = table.Column<int>(name: "IS_NODSP_RYOSYU", type: "integer", nullable: false),
                    ISAUTOADD = table.Column<int>(name: "IS_AUTO_ADD", type: "integer", nullable: false),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(160)", maxLength: 160, nullable: true),
                    CMT1 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMTCD1 = table.Column<string>(name: "CMT_CD1", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTOPT1 = table.Column<string>(name: "CMT_OPT1", type: "character varying(160)", maxLength: 160, nullable: true),
                    CMT2 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMTCD2 = table.Column<string>(name: "CMT_CD2", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTOPT2 = table.Column<string>(name: "CMT_OPT2", type: "character varying(160)", maxLength: 160, nullable: true),
                    CMT3 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMTCD3 = table.Column<string>(name: "CMT_CD3", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTOPT3 = table.Column<string>(name: "CMT_OPT3", type: "character varying(160)", maxLength: 160, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRK_SIN_KOUI_DETAIL", x => new { x.HPID, x.RAIINNO, x.HOKENKBN, x.RPNO, x.SEQNO, x.ROWNO });
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_KOUI_DETAIL_DEL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ROWNO = table.Column<int>(name: "ROW_NO", type: "integer", nullable: false),
                    ITEMSEQNO = table.Column<int>(name: "ITEM_SEQ_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DELITEMCD = table.Column<string>(name: "DEL_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SANTEIDATE = table.Column<int>(name: "SANTEI_DATE", type: "integer", nullable: false),
                    DELSBT = table.Column<int>(name: "DEL_SBT", type: "integer", nullable: false),
                    ISWARNING = table.Column<int>(name: "IS_WARNING", type: "integer", nullable: false),
                    TERMCNT = table.Column<int>(name: "TERM_CNT", type: "integer", nullable: false),
                    TERMSBT = table.Column<int>(name: "TERM_SBT", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRK_SIN_KOUI_DETAIL_DEL", x => new { x.HPID, x.RAIINNO, x.HOKENKBN, x.RPNO, x.SEQNO, x.ROWNO, x.ITEMSEQNO });
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_RP_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    RPNO = table.Column<int>(name: "RP_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    SINID = table.Column<int>(name: "SIN_ID", type: "integer", nullable: false),
                    CDNO = table.Column<string>(name: "CD_NO", type: "character varying(15)", maxLength: 15, nullable: true),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_WRK_SIN_RP_INF", x => new { x.HPID, x.RAIINNO, x.HOKENKBN, x.RPNO });
                });

            migrationBuilder.CreateTable(
                name: "YAKKA_SYUSAI_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    YAKKACD = table.Column<string>(name: "YAKKA_CD", type: "character varying(12)", maxLength: 12, nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    SEIBUN = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    HINMOKU = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    SYUSAIDATE = table.Column<int>(name: "SYUSAI_DATE", type: "integer", nullable: false),
                    KEIKA = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    JUNSENPATU = table.Column<int>(name: "JUN_SENPATU", type: "integer", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    YAKKA = table.Column<double>(type: "double precision", nullable: false),
                    ISNOTARGET = table.Column<int>(name: "IS_NOTARGET", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YAKKA_SYUSAI_MST", x => new { x.HPID, x.YAKKACD, x.ITEMCD, x.STARTDATE });
                });

            migrationBuilder.CreateTable(
                name: "YOHO_INF_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    YOHOSUFFIX = table.Column<string>(name: "YOHO_SUFFIX", type: "character varying(240)", maxLength: 240, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOHO_INF_MST", x => new { x.HPID, x.ITEMCD });
                });

            migrationBuilder.CreateTable(
                name: "YOHO_SET_MST",
                columns: table => new
                {
                    SETID = table.Column<int>(name: "SET_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_YOHO_SET_MST", x => x.SETID);
                });

            migrationBuilder.CreateTable(
                name: "YOYAKU_ODR_INF",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    YOYAKUKARTENO = table.Column<long>(name: "YOYAKU_KARTE_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    YOYAKUDATE = table.Column<int>(name: "YOYAKU_DATE", type: "integer", nullable: false),
                    ODRKOUIKBN = table.Column<int>(name: "ODR_KOUI_KBN", type: "integer", nullable: false),
                    RPNAME = table.Column<string>(name: "RP_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    SYOHOSBT = table.Column<int>(name: "SYOHO_SBT", type: "integer", nullable: false),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false),
                    TOSEKIKBN = table.Column<int>(name: "TOSEKI_KBN", type: "integer", nullable: false),
                    DAYSCNT = table.Column<int>(name: "DAYS_CNT", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_YOYAKU_ODR_INF", x => new { x.HPID, x.PTID, x.YOYAKUKARTENO, x.RPNO, x.RPEDANO });
                });

            migrationBuilder.CreateTable(
                name: "YOYAKU_ODR_INF_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    YOYAKUKARTENO = table.Column<long>(name: "YOYAKU_KARTE_NO", type: "bigint", nullable: false),
                    RPNO = table.Column<long>(name: "RP_NO", type: "bigint", nullable: false),
                    RPEDANO = table.Column<long>(name: "RP_EDA_NO", type: "bigint", nullable: false),
                    ROWNO = table.Column<long>(name: "ROW_NO", type: "bigint", nullable: false),
                    YOYAKUDATE = table.Column<int>(name: "YOYAKU_DATE", type: "integer", nullable: false),
                    SINKOUIKBN = table.Column<int>(name: "SIN_KOUI_KBN", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEMNAME = table.Column<string>(name: "ITEM_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNITNAME = table.Column<string>(name: "UNIT_NAME", type: "character varying(24)", maxLength: 24, nullable: true),
                    UNITSBT = table.Column<int>(name: "UNIT_SBT", type: "integer", nullable: false),
                    TERMVAL = table.Column<double>(name: "TERM_VAL", type: "double precision", nullable: false),
                    KOHATUKBN = table.Column<int>(name: "KOHATU_KBN", type: "integer", nullable: false),
                    SYOHOKBN = table.Column<int>(name: "SYOHO_KBN", type: "integer", nullable: false),
                    SYOHOLIMITKBN = table.Column<int>(name: "SYOHO_LIMIT_KBN", type: "integer", nullable: false),
                    DRUGKBN = table.Column<int>(name: "DRUG_KBN", type: "integer", nullable: false),
                    YOHOKBN = table.Column<int>(name: "YOHO_KBN", type: "integer", nullable: false),
                    KOKUJI1 = table.Column<int>(type: "integer", nullable: false),
                    ISNODSPRECE = table.Column<int>(name: "IS_NODSP_RECE", type: "integer", nullable: false),
                    IPNCD = table.Column<string>(name: "IPN_CD", type: "character varying(12)", maxLength: 12, nullable: true),
                    IPNNAME = table.Column<string>(name: "IPN_NAME", type: "character varying(120)", maxLength: 120, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMTNAME = table.Column<string>(name: "CMT_NAME", type: "character varying(32)", maxLength: 32, nullable: true),
                    CMTOPT = table.Column<string>(name: "CMT_OPT", type: "character varying(38)", maxLength: 38, nullable: true),
                    FONTCOLOR = table.Column<int>(name: "FONT_COLOR", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOYAKU_ODR_INF_DETAIL", x => new { x.HPID, x.PTID, x.YOYAKUKARTENO, x.RPNO, x.RPEDANO, x.ROWNO });
                });

            migrationBuilder.CreateTable(
                name: "YOYAKU_SBT_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    YOYAKUSBT = table.Column<int>(name: "YOYAKU_SBT", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SBTNAME = table.Column<string>(name: "SBT_NAME", type: "character varying(120)", maxLength: 120, nullable: false),
                    DEFAULTCMT = table.Column<string>(name: "DEFAULT_CMT", type: "character varying(120)", maxLength: 120, nullable: true),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_YOYAKU_SBT_MST", x => new { x.HPID, x.YOYAKUSBT });
                });

            migrationBuilder.CreateTable(
                name: "Z_DOC_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(300)", maxLength: 300, nullable: true),
                    DSPFILENAME = table.Column<string>(name: "DSP_FILE_NAME", type: "character varying(300)", maxLength: 300, nullable: true),
                    ISLOCKED = table.Column<int>(name: "IS_LOCKED", type: "integer", nullable: false),
                    LOCKDATE = table.Column<DateTime>(name: "LOCK_DATE", type: "timestamp with time zone", nullable: true),
                    LOCKID = table.Column<int>(name: "LOCK_ID", type: "integer", nullable: false),
                    LOCKMACHINE = table.Column<string>(name: "LOCK_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
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
                    table.PrimaryKey("PK_Z_DOC_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_FILING_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    GETDATE = table.Column<int>(name: "GET_DATE", type: "integer", nullable: false),
                    CATEGORYCD = table.Column<int>(name: "CATEGORY_CD", type: "integer", nullable: false),
                    FILENO = table.Column<int>(name: "FILE_NO", type: "integer", nullable: false),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(300)", maxLength: 300, nullable: true),
                    DSPFILENAME = table.Column<string>(name: "DSP_FILE_NAME", type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    FILEID = table.Column<int>(name: "FILE_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_FILING_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_KENSA_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    IRAICD = table.Column<long>(name: "IRAI_CD", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAIDATE = table.Column<int>(name: "IRAI_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    INOUTKBN = table.Column<int>(name: "INOUT_KBN", type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    TOSEKIKBN = table.Column<int>(name: "TOSEKI_KBN", type: "integer", nullable: false),
                    SIKYUKBN = table.Column<int>(name: "SIKYU_KBN", type: "integer", nullable: false),
                    RESULTCHECK = table.Column<int>(name: "RESULT_CHECK", type: "integer", nullable: false),
                    CENTERCD = table.Column<string>(name: "CENTER_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    NYUBI = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    YOKETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    BILIRUBIN = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
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
                    table.PrimaryKey("PK_Z_KENSA_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_KENSA_INF_DETAIL",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    IRAICD = table.Column<long>(name: "IRAI_CD", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAIDATE = table.Column<int>(name: "IRAI_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    KENSAITEMCD = table.Column<string>(name: "KENSA_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    RESULTVAL = table.Column<string>(name: "RESULT_VAL", type: "character varying(10)", maxLength: 10, nullable: true),
                    RESULTTYPE = table.Column<string>(name: "RESULT_TYPE", type: "character varying(1)", maxLength: 1, nullable: true),
                    ABNORMALKBN = table.Column<string>(name: "ABNORMAL_KBN", type: "character varying(1)", maxLength: 1, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CMTCD1 = table.Column<string>(name: "CMT_CD1", type: "character varying(3)", maxLength: 3, nullable: true),
                    CMTCD2 = table.Column<string>(name: "CMT_CD2", type: "character varying(3)", maxLength: 3, nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_KENSA_INF_DETAIL", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_LIMIT_CNT_LIST_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    KOHIID = table.Column<int>(name: "KOHI_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SORTKEY = table.Column<string>(name: "SORT_KEY", type: "character varying(61)", maxLength: 61, nullable: true),
                    OYARAIINNO = table.Column<long>(name: "OYA_RAIIN_NO", type: "bigint", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_Z_LIMIT_CNT_LIST_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_LIMIT_LIST_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    KOHIID = table.Column<int>(name: "KOHI_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SORTKEY = table.Column<string>(name: "SORT_KEY", type: "character varying(61)", maxLength: 61, nullable: true),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    FUTANGAKU = table.Column<int>(name: "FUTAN_GAKU", type: "integer", nullable: false),
                    TOTALGAKU = table.Column<int>(name: "TOTAL_GAKU", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_LIMIT_LIST_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_MONSHIN_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RTEXT = table.Column<string>(type: "text", nullable: true),
                    GETKBN = table.Column<int>(name: "GET_KBN", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_MONSHIN_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ALRGY_DRUG",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DRUGNAME = table.Column<string>(name: "DRUG_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_ALRGY_DRUG", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ALRGY_ELSE",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ALRGYNAME = table.Column<string>(name: "ALRGY_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_ALRGY_ELSE", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ALRGY_FOOD",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ALRGYKBN = table.Column<string>(name: "ALRGY_KBN", type: "text", nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_ALRGY_FOOD", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_CMT_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_CMT_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_FAMILY",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    FAMILYID = table.Column<long>(name: "FAMILY_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZOKUGARACD = table.Column<string>(name: "ZOKUGARA_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    PARENTID = table.Column<int>(name: "PARENT_ID", type: "integer", nullable: false),
                    FAMILYPTID = table.Column<long>(name: "FAMILY_PT_ID", type: "bigint", nullable: false),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    ISDEAD = table.Column<int>(name: "IS_DEAD", type: "integer", nullable: false),
                    ISSEPARATED = table.Column<int>(name: "IS_SEPARATED", type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_FAMILY", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_FAMILY_REKI",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    FAMILYID = table.Column<long>(name: "FAMILY_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_FAMILY_REKI", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_GRP_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    GRPCODE = table.Column<string>(name: "GRP_CODE", type: "character varying(4)", maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_GRP_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_CHECK",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENGRP = table.Column<int>(name: "HOKEN_GRP", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CHECKDATE = table.Column<DateTime>(name: "CHECK_DATE", type: "timestamp with time zone", nullable: false),
                    CHECKID = table.Column<int>(name: "CHECK_ID", type: "integer", nullable: false),
                    CHECKMACHINE = table.Column<string>(name: "CHECK_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    CHECKCMT = table.Column<string>(name: "CHECK_CMT", type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_HOKEN_CHECK", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    EDANO = table.Column<string>(name: "EDA_NO", type: "character varying(2)", maxLength: 2, nullable: true),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    HOKENSYANO = table.Column<string>(name: "HOKENSYA_NO", type: "character varying(8)", maxLength: 8, nullable: true),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HONKEKBN = table.Column<int>(name: "HONKE_KBN", type: "integer", nullable: false),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENSYANAME = table.Column<string>(name: "HOKENSYA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSYAPOST = table.Column<string>(name: "HOKENSYA_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOKENSYAADDRESS = table.Column<string>(name: "HOKENSYA_ADDRESS", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSYATEL = table.Column<string>(name: "HOKENSYA_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    KEIZOKUKBN = table.Column<int>(name: "KEIZOKU_KBN", type: "integer", nullable: false),
                    SIKAKUDATE = table.Column<int>(name: "SIKAKU_DATE", type: "integer", nullable: false),
                    KOFUDATE = table.Column<int>(name: "KOFU_DATE", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOGAKUKBN = table.Column<int>(name: "KOGAKU_KBN", type: "integer", nullable: false),
                    KOGAKUTYPE = table.Column<int>(name: "KOGAKU_TYPE", type: "integer", nullable: false),
                    TOKUREIYM1 = table.Column<int>(name: "TOKUREI_YM1", type: "integer", nullable: false),
                    TOKUREIYM2 = table.Column<int>(name: "TOKUREI_YM2", type: "integer", nullable: false),
                    TASUKAIYM = table.Column<int>(name: "TASUKAI_YM", type: "integer", nullable: false),
                    SYOKUMUKBN = table.Column<int>(name: "SYOKUMU_KBN", type: "integer", nullable: false),
                    GENMENKBN = table.Column<int>(name: "GENMEN_KBN", type: "integer", nullable: false),
                    GENMENRATE = table.Column<int>(name: "GENMEN_RATE", type: "integer", nullable: false),
                    GENMENGAKU = table.Column<int>(name: "GENMEN_GAKU", type: "integer", nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIKOFUNO = table.Column<string>(name: "ROUSAI_KOFU_NO", type: "character varying(14)", maxLength: 14, nullable: true),
                    ROUSAISAIGAIKBN = table.Column<int>(name: "ROUSAI_SAIGAI_KBN", type: "integer", nullable: false),
                    ROUSAIJIGYOSYONAME = table.Column<string>(name: "ROUSAI_JIGYOSYO_NAME", type: "character varying(80)", maxLength: 80, nullable: true),
                    ROUSAIPREFNAME = table.Column<string>(name: "ROUSAI_PREF_NAME", type: "character varying(10)", maxLength: 10, nullable: true),
                    ROUSAICITYNAME = table.Column<string>(name: "ROUSAI_CITY_NAME", type: "character varying(20)", maxLength: 20, nullable: true),
                    ROUSAISYOBYODATE = table.Column<int>(name: "ROUSAI_SYOBYO_DATE", type: "integer", nullable: false),
                    ROUSAISYOBYOCD = table.Column<string>(name: "ROUSAI_SYOBYO_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIROUDOUCD = table.Column<string>(name: "ROUSAI_ROUDOU_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIKANTOKUCD = table.Column<string>(name: "ROUSAI_KANTOKU_CD", type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAIRECECOUNT = table.Column<int>(name: "ROUSAI_RECE_COUNT", type: "integer", nullable: false),
                    JIBAIHOKENNAME = table.Column<string>(name: "JIBAI_HOKEN_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    JIBAIHOKENTANTO = table.Column<string>(name: "JIBAI_HOKEN_TANTO", type: "character varying(40)", maxLength: 40, nullable: true),
                    JIBAIHOKENTEL = table.Column<string>(name: "JIBAI_HOKEN_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    JIBAIJYUSYOUDATE = table.Column<int>(name: "JIBAI_JYUSYOU_DATE", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    RYOYOSTARTDATE = table.Column<int>(name: "RYOYO_START_DATE", type: "integer", nullable: false),
                    RYOYOENDDATE = table.Column<int>(name: "RYOYO_END_DATE", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_HOKEN_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_PATTERN",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKENKBN = table.Column<int>(name: "HOKEN_KBN", type: "integer", nullable: false),
                    HOKENSBTCD = table.Column<int>(name: "HOKEN_SBT_CD", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    KOHI1ID = table.Column<int>(name: "KOHI1_ID", type: "integer", nullable: false),
                    KOHI2ID = table.Column<int>(name: "KOHI2_ID", type: "integer", nullable: false),
                    KOHI3ID = table.Column<int>(name: "KOHI3_ID", type: "integer", nullable: false),
                    KOHI4ID = table.Column<int>(name: "KOHI4_ID", type: "integer", nullable: false),
                    HOKENMEMO = table.Column<string>(name: "HOKEN_MEMO", type: "character varying(400)", maxLength: 400, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_PT_HOKEN_PATTERN", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_SCAN",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENGRP = table.Column<int>(name: "HOKEN_GRP", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILENAME = table.Column<string>(name: "FILE_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_HOKEN_SCAN", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    PTNUM = table.Column<long>(name: "PT_NUM", type: "bigint", nullable: false),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    ISDEAD = table.Column<int>(name: "IS_DEAD", type: "integer", nullable: false),
                    DEATHDATE = table.Column<int>(name: "DEATH_DATE", type: "integer", nullable: false),
                    HOMEPOST = table.Column<string>(name: "HOME_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    HOMEADDRESS1 = table.Column<string>(name: "HOME_ADDRESS1", type: "character varying(100)", maxLength: 100, nullable: true),
                    HOMEADDRESS2 = table.Column<string>(name: "HOME_ADDRESS2", type: "character varying(100)", maxLength: 100, nullable: true),
                    TEL1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    TEL2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    MAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SETAINUSI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZOKUGARA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    JOB = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    RENRAKUNAME = table.Column<string>(name: "RENRAKU_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKUPOST = table.Column<string>(name: "RENRAKU_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    RENRAKUADDRESS1 = table.Column<string>(name: "RENRAKU_ADDRESS1", type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKUADDRESS2 = table.Column<string>(name: "RENRAKU_ADDRESS2", type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKUTEL = table.Column<string>(name: "RENRAKU_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    RENRAKUMEMO = table.Column<string>(name: "RENRAKU_MEMO", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICENAME = table.Column<string>(name: "OFFICE_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICEPOST = table.Column<string>(name: "OFFICE_POST", type: "character varying(7)", maxLength: 7, nullable: true),
                    OFFICEADDRESS1 = table.Column<string>(name: "OFFICE_ADDRESS1", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICEADDRESS2 = table.Column<string>(name: "OFFICE_ADDRESS2", type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICETEL = table.Column<string>(name: "OFFICE_TEL", type: "character varying(15)", maxLength: 15, nullable: true),
                    OFFICEMEMO = table.Column<string>(name: "OFFICE_MEMO", type: "character varying(100)", maxLength: 100, nullable: true),
                    ISRYOSYODETAIL = table.Column<int>(name: "IS_RYOSYO_DETAIL", type: "integer", nullable: false),
                    PRIMARYDOCTOR = table.Column<int>(name: "PRIMARY_DOCTOR", type: "integer", nullable: false),
                    ISTESTER = table.Column<int>(name: "IS_TESTER", type: "integer", nullable: false),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    MAINHOKENPID = table.Column<int>(name: "MAIN_HOKEN_PID", type: "integer", nullable: false),
                    REFERENCENO = table.Column<long>(name: "REFERENCE_NO", type: "bigint", nullable: false),
                    LIMITCONSFLG = table.Column<int>(name: "LIMIT_CONS_FLG", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_INFECTION",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "text", nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_INFECTION", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_JIBKAR",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    WEBID = table.Column<string>(name: "WEB_ID", type: "character varying(16)", maxLength: 16, nullable: true),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ODRKAIJI = table.Column<int>(name: "ODR_KAIJI", type: "integer", nullable: false),
                    ODRUPDATEDATE = table.Column<DateTime>(name: "ODR_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    KARTEKAIJI = table.Column<int>(name: "KARTE_KAIJI", type: "integer", nullable: false),
                    KARTEUPDATEDATE = table.Column<DateTime>(name: "KARTE_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    KENSAKAIJI = table.Column<int>(name: "KENSA_KAIJI", type: "integer", nullable: false),
                    KENSAUPDATEDATE = table.Column<DateTime>(name: "KENSA_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    BYOMEIKAIJI = table.Column<int>(name: "BYOMEI_KAIJI", type: "integer", nullable: false),
                    BYOMEIUPDATEDATE = table.Column<DateTime>(name: "BYOMEI_UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_JIBKAR", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_KIO_REKI",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    BYOMEICD = table.Column<string>(name: "BYOMEI_CD", type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAICD = table.Column<string>(name: "BYOTAI_CD", type: "text", nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_KIO_REKI", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_KOHI",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PREFNO = table.Column<int>(name: "PREF_NO", type: "integer", nullable: false),
                    HOKENNO = table.Column<int>(name: "HOKEN_NO", type: "integer", nullable: false),
                    HOKENEDANO = table.Column<int>(name: "HOKEN_EDA_NO", type: "integer", nullable: false),
                    FUTANSYANO = table.Column<string>(name: "FUTANSYA_NO", type: "character varying(8)", maxLength: 8, nullable: true),
                    JYUKYUSYANO = table.Column<string>(name: "JYUKYUSYA_NO", type: "character varying(7)", maxLength: 7, nullable: true),
                    TOKUSYUNO = table.Column<string>(name: "TOKUSYU_NO", type: "character varying(20)", maxLength: 20, nullable: true),
                    SIKAKUDATE = table.Column<int>(name: "SIKAKU_DATE", type: "integer", nullable: false),
                    KOFUDATE = table.Column<int>(name: "KOFU_DATE", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    HOKENSBTKBN = table.Column<int>(name: "HOKEN_SBT_KBN", type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_KOHI", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_KYUSEI",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KANANAME = table.Column<string>(name: "KANA_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_PT_KYUSEI", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_MEMO",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_MEMO", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_OTC_DRUG",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    SERIALNUM = table.Column<int>(name: "SERIAL_NUM", type: "integer", nullable: false),
                    TRADENAME = table.Column<string>(name: "TRADE_NAME", type: "character varying(200)", maxLength: 200, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_OTC_DRUG", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_OTHER_DRUG",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    DRUGNAME = table.Column<string>(name: "DRUG_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_OTHER_DRUG", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_PREGNANCY",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    PERIODDATE = table.Column<int>(name: "PERIOD_DATE", type: "integer", nullable: false),
                    PERIODDUEDATE = table.Column<int>(name: "PERIOD_DUE_DATE", type: "integer", nullable: false),
                    OVULATIONDATE = table.Column<int>(name: "OVULATION_DATE", type: "integer", nullable: false),
                    OVULATIONDUEDATE = table.Column<int>(name: "OVULATION_DUE_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_PT_PREGNANCY", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ROUSAI_TENKI",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    SINKEI = table.Column<int>(type: "integer", nullable: false),
                    TENKI = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_PT_ROUSAI_TENKI", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_SANTEI_CONF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    KBNNO = table.Column<int>(name: "KBN_NO", type: "integer", nullable: false),
                    EDANO = table.Column<int>(name: "EDA_NO", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBNVAL = table.Column<int>(name: "KBN_VAL", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_PT_SANTEI_CONF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_SUPPLE",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    INDEXCD = table.Column<string>(name: "INDEX_CD", type: "text", nullable: true),
                    INDEXWORD = table.Column<string>(name: "INDEX_WORD", type: "character varying(200)", maxLength: 200, nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Z_PT_SUPPLE", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_TAG",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: true),
                    MEMODATA = table.Column<byte[]>(name: "MEMO_DATA", type: "bytea", nullable: true),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    ISDSPUKETUKE = table.Column<int>(name: "IS_DSP_UKETUKE", type: "integer", nullable: false),
                    ISDSPKARTE = table.Column<int>(name: "IS_DSP_KARTE", type: "integer", nullable: false),
                    ISDSPKAIKEI = table.Column<int>(name: "IS_DSP_KAIKEI", type: "integer", nullable: false),
                    ISDSPRECE = table.Column<int>(name: "IS_DSP_RECE", type: "integer", nullable: false),
                    BACKGROUNDCOLOR = table.Column<string>(name: "BACKGROUND_COLOR", type: "character varying(8)", maxLength: 8, nullable: true),
                    TAGGRPCD = table.Column<int>(name: "TAG_GRP_CD", type: "integer", nullable: false),
                    ALPHABLENDVAL = table.Column<int>(name: "ALPHABLEND_VAL", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    FONTSIZE = table.Column<int>(type: "integer", nullable: false),
                    WIDTH = table.Column<int>(type: "integer", nullable: false),
                    HEIGHT = table.Column<int>(type: "integer", nullable: false),
                    LEFT = table.Column<int>(type: "integer", nullable: false),
                    TOP = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_TAG", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_CMT_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_CMT_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    OYARAIINNO = table.Column<long>(name: "OYA_RAIIN_NO", type: "bigint", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    ISYOYAKU = table.Column<int>(name: "IS_YOYAKU", type: "integer", nullable: false),
                    YOYAKUTIME = table.Column<string>(name: "YOYAKU_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    YOYAKUID = table.Column<int>(name: "YOYAKU_ID", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    UKETUKETIME = table.Column<string>(name: "UKETUKE_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    UKETUKEID = table.Column<int>(name: "UKETUKE_ID", type: "integer", nullable: false),
                    UKETUKENO = table.Column<int>(name: "UKETUKE_NO", type: "integer", nullable: false),
                    SINSTARTTIME = table.Column<string>(name: "SIN_START_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    SINENDTIME = table.Column<string>(name: "SIN_END_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    KAIKEITIME = table.Column<string>(name: "KAIKEI_TIME", type: "character varying(6)", maxLength: 6, nullable: true),
                    KAIKEIID = table.Column<int>(name: "KAIKEI_ID", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    HOKENPID = table.Column<int>(name: "HOKEN_PID", type: "integer", nullable: false),
                    SYOSAISINKBN = table.Column<int>(name: "SYOSAISIN_KBN", type: "integer", nullable: false),
                    JIKANKBN = table.Column<int>(name: "JIKAN_KBN", type: "integer", nullable: false),
                    CONFIRMATIONRESULT = table.Column<string>(name: "CONFIRMATION_RESULT", type: "character varying(120)", maxLength: 120, nullable: true),
                    CONFIRMATIONSTATE = table.Column<int>(name: "CONFIRMATION_STATE", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    SANTEIKBN = table.Column<int>(name: "SANTEI_KBN", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_KBN_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    GRPID = table.Column<int>(name: "GRP_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBNCD = table.Column<int>(name: "KBN_CD", type: "integer", nullable: false),
                    ISDELETE = table.Column<int>(name: "IS_DELETE", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_KBN_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_LIST_CMT",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_Z_RAIIN_LIST_CMT", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_LIST_TAG",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    TAGNO = table.Column<int>(name: "TAG_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_RAIIN_LIST_TAG", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_CHECK_CMT",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ISPENDING = table.Column<int>(name: "IS_PENDING", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ISCHECKED = table.Column<int>(name: "IS_CHECKED", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_RECE_CHECK_CMT", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_CMT",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    CMTKBN = table.Column<int>(name: "CMT_KBN", type: "integer", nullable: false),
                    CMTSBT = table.Column<int>(name: "CMT_SBT", type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT = table.Column<string>(type: "text", nullable: true),
                    CMTDATA = table.Column<string>(name: "CMT_DATA", type: "character varying(38)", maxLength: 38, nullable: true),
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
                    table.PrimaryKey("PK_Z_RECE_CMT", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_INF_EDIT",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RECESBT = table.Column<string>(name: "RECE_SBT", type: "character varying(4)", maxLength: 4, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI1HOUBETU = table.Column<string>(name: "KOHI1_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI2HOUBETU = table.Column<string>(name: "KOHI2_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI3HOUBETU = table.Column<string>(name: "KOHI3_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    KOHI4HOUBETU = table.Column<string>(name: "KOHI4_HOUBETU", type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENRECETENSU = table.Column<int>(name: "HOKEN_RECE_TENSU", type: "integer", nullable: true),
                    HOKENRECEFUTAN = table.Column<int>(name: "HOKEN_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECETENSU = table.Column<int>(name: "KOHI1_RECE_TENSU", type: "integer", nullable: true),
                    KOHI1RECEFUTAN = table.Column<int>(name: "KOHI1_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI1RECEKYUFU = table.Column<int>(name: "KOHI1_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI2RECETENSU = table.Column<int>(name: "KOHI2_RECE_TENSU", type: "integer", nullable: true),
                    KOHI2RECEFUTAN = table.Column<int>(name: "KOHI2_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI2RECEKYUFU = table.Column<int>(name: "KOHI2_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI3RECETENSU = table.Column<int>(name: "KOHI3_RECE_TENSU", type: "integer", nullable: true),
                    KOHI3RECEFUTAN = table.Column<int>(name: "KOHI3_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI3RECEKYUFU = table.Column<int>(name: "KOHI3_RECE_KYUFU", type: "integer", nullable: true),
                    KOHI4RECETENSU = table.Column<int>(name: "KOHI4_RECE_TENSU", type: "integer", nullable: true),
                    KOHI4RECEFUTAN = table.Column<int>(name: "KOHI4_RECE_FUTAN", type: "integer", nullable: true),
                    KOHI4RECEKYUFU = table.Column<int>(name: "KOHI4_RECE_KYUFU", type: "integer", nullable: true),
                    HOKENNISSU = table.Column<int>(name: "HOKEN_NISSU", type: "integer", nullable: true),
                    KOHI1NISSU = table.Column<int>(name: "KOHI1_NISSU", type: "integer", nullable: true),
                    KOHI2NISSU = table.Column<int>(name: "KOHI2_NISSU", type: "integer", nullable: true),
                    KOHI3NISSU = table.Column<int>(name: "KOHI3_NISSU", type: "integer", nullable: true),
                    KOHI4NISSU = table.Column<int>(name: "KOHI4_NISSU", type: "integer", nullable: true),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_Z_RECE_INF_EDIT", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_SEIKYU",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEIKYUYM = table.Column<int>(name: "SEIKYU_YM", type: "integer", nullable: false),
                    SEIKYUKBN = table.Column<int>(name: "SEIKYU_KBN", type: "integer", nullable: false),
                    PREHOKENID = table.Column<int>(name: "PRE_HOKEN_ID", type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
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
                    table.PrimaryKey("PK_Z_RECE_SEIKYU", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RSV_DAY_COMMENT",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Z_RSV_DAY_COMMENT", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RSV_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RSVFRAMEID = table.Column<int>(name: "RSV_FRAME_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    STARTTIME = table.Column<int>(name: "START_TIME", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RSVSBT = table.Column<int>(name: "RSV_SBT", type: "integer", nullable: false),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RSV_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SANTEI_INF_DETAIL",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: true),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    KISANSBT = table.Column<int>(name: "KISAN_SBT", type: "integer", nullable: false),
                    KISANDATE = table.Column<int>(name: "KISAN_DATE", type: "integer", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    HOSOKUCOMMENT = table.Column<string>(name: "HOSOKU_COMMENT", type: "character varying(80)", maxLength: 80, nullable: true),
                    COMMENT = table.Column<string>(type: "text", nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SANTEI_INF_DETAIL", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SEIKATUREKI_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SEIKATUREKI_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SUMMARY_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RTEXT = table.Column<byte[]>(type: "bytea", nullable: true),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SUMMARY_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SYOBYO_KEIKA",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    SINDAY = table.Column<int>(name: "SIN_DAY", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KEIKA = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Z_SYOBYO_KEIKA", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SYOUKI_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINYM = table.Column<int>(name: "SIN_YM", type: "integer", nullable: false),
                    HOKENID = table.Column<int>(name: "HOKEN_ID", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    SYOUKIKBN = table.Column<int>(name: "SYOUKI_KBN", type: "integer", nullable: false),
                    SYOUKI = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Z_SYOUKI_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SYUNO_NYUKIN",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    ADJUSTFUTAN = table.Column<int>(name: "ADJUST_FUTAN", type: "integer", nullable: false),
                    NYUKINGAKU = table.Column<int>(name: "NYUKIN_GAKU", type: "integer", nullable: false),
                    PAYMENTMETHODCD = table.Column<int>(name: "PAYMENT_METHOD_CD", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    NYUKINCMT = table.Column<string>(name: "NYUKIN_CMT", type: "character varying(100)", maxLength: 100, nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NYUKINDATE = table.Column<int>(name: "NYUKIN_DATE", type: "integer", nullable: false),
                    NYUKINJITENSU = table.Column<int>(name: "NYUKINJI_TENSU", type: "integer", nullable: false),
                    NYUKINJISEIKYU = table.Column<int>(name: "NYUKINJI_SEIKYU", type: "integer", nullable: false),
                    NYUKINJIDETAIL = table.Column<string>(name: "NYUKINJI_DETAIL", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SYUNO_NYUKIN", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_TODO_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    TODONO = table.Column<int>(name: "TODO_NO", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TODOEDANO = table.Column<int>(name: "TODO_EDA_NO", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    TODOKBNNO = table.Column<int>(name: "TODO_KBN_NO", type: "integer", nullable: false),
                    TODOGRPNO = table.Column<int>(name: "TODO_GRP_NO", type: "integer", nullable: false),
                    TANTO = table.Column<int>(type: "integer", nullable: false),
                    TERM = table.Column<int>(type: "integer", nullable: false),
                    CMT1 = table.Column<string>(type: "text", nullable: true),
                    CMT2 = table.Column<string>(type: "text", nullable: true),
                    ISDONE = table.Column<int>(name: "IS_DONE", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Z_TODO_INF", x => x.OPID);
                });

            migrationBuilder.CreateTable(
                name: "Z_UKETUKE_SBT_DAY_INF",
                columns: table => new
                {
                    OPID = table.Column<long>(name: "OP_ID", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OPTYPE = table.Column<string>(name: "OP_TYPE", type: "character varying(10)", maxLength: 10, nullable: true),
                    OPTIME = table.Column<DateTime>(name: "OP_TIME", type: "timestamp with time zone", nullable: false),
                    OPADDR = table.Column<string>(name: "OP_ADDR", type: "character varying(100)", maxLength: 100, nullable: true),
                    OPHOSTNAME = table.Column<string>(name: "OP_HOSTNAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<int>(name: "SEQ_NO", type: "integer", nullable: false),
                    UKETUKESBT = table.Column<int>(name: "UKETUKE_SBT", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_UKETUKE_SBT_DAY_INF", x => x.OPID);
                });

            migrationBuilder.CreateIndex(
                name: "CALC_STATUS_IDX01",
                table: "CALC_STATUS",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "STATUS", "CREATE_MACHINE" });

            migrationBuilder.CreateIndex(
                name: "CALC_STATUS_IDX02",
                table: "CALC_STATUS",
                columns: new[] { "HP_ID", "STATUS", "CREATE_MACHINE" });

            migrationBuilder.CreateIndex(
                name: "CMT_KBN_MST_IDX01",
                table: "CMT_KBN_MST",
                columns: new[] { "HP_ID", "ITEM_CD", "START_DATE" });

            migrationBuilder.CreateIndex(
                name: "CONVERSION_ITEM_INF_IDX01",
                table: "CONVERSION_ITEM_INF",
                columns: new[] { "HP_ID", "SOURCE_ITEM_CD" });

            migrationBuilder.CreateIndex(
                name: "DEF_HOKEN_NO_IDX01",
                table: "DEF_HOKEN_NO",
                columns: new[] { "HP_ID", "DIGIT_1", "DIGIT_2", "DIGIT_3", "DIGIT_4", "DIGIT_5", "DIGIT_6", "DIGIT_7", "DIGIT_8", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "DENSI_HAIHAN_CUSTOM_IDX03",
                table: "DENSI_HAIHAN_CUSTOM",
                columns: new[] { "HP_ID", "ITEM_CD1", "HAIHAN_KBN", "START_DATE", "TARGET_KBN", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "DENSI_HAIHAN_DAY_IDX03",
                table: "DENSI_HAIHAN_DAY",
                columns: new[] { "HP_ID", "ITEM_CD1", "HAIHAN_KBN", "START_DATE", "END_DATE", "TARGET_KBN", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "DENSI_HAIHAN_KARTE_IDX03",
                table: "DENSI_HAIHAN_KARTE",
                columns: new[] { "HP_ID", "ITEM_CD1", "HAIHAN_KBN", "START_DATE", "END_DATE", "TARGET_KBN", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "DENSI_HAIHAN_MONTH_IDX03",
                table: "DENSI_HAIHAN_MONTH",
                columns: new[] { "HP_ID", "ITEM_CD1", "HAIHAN_KBN", "START_DATE", "END_DATE", "TARGET_KBN", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "DENSI_HAIHAN_WEEK_IDX03",
                table: "DENSI_HAIHAN_WEEK",
                columns: new[] { "HP_ID", "ITEM_CD1", "HAIHAN_KBN", "START_DATE", "END_DATE", "TARGET_KBN", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "DENSI_HOUKATU_GRP_IDX02",
                table: "DENSI_HOUKATU_GRP",
                columns: new[] { "HP_ID", "ITEM_CD", "START_DATE", "END_DATE", "TARGET_KBN", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "DRUG_INF_UKEY01",
                table: "DRUG_INF",
                columns: new[] { "HP_ID", "ITEM_CD", "INF_KBN", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "FILING_INF_IDX01",
                table: "FILING_INF",
                columns: new[] { "PT_ID", "GET_DATE", "FILE_NO", "CATEGORY_CD" });

            migrationBuilder.CreateIndex(
                name: "FUNCTION_MST_PKEY",
                table: "FUNCTION_MST",
                column: "FUNCTION_CD");

            migrationBuilder.CreateIndex(
                name: "HOLIDAY_MST_UKEY01",
                table: "HOLIDAY_MST",
                columns: new[] { "HP_ID", "SIN_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "IPN_MIN_YAKKA_MST_IDX01",
                table: "IPN_MIN_YAKKA_MST",
                columns: new[] { "HP_ID", "IPN_NAME_CD", "START_DATE" });

            migrationBuilder.CreateIndex(
                name: "PT_KA_MST_IDX01",
                table: "KA_MST",
                column: "KA_ID");

            migrationBuilder.CreateIndex(
                name: "KARTE_INF_IDX01",
                table: "KARTE_INF",
                columns: new[] { "HP_ID", "PT_ID", "KARTE_KBN" });

            migrationBuilder.CreateIndex(
                name: "LIMIT_LIST_INF_IDX01",
                table: "LIMIT_LIST_INF",
                columns: new[] { "PT_ID", "KOHI_ID", "SIN_DATE", "SEQ_NO" });

            migrationBuilder.CreateIndex(
                name: "M12_FOOD_ALRGY_IDX01",
                table: "M12_FOOD_ALRGY",
                columns: new[] { "KIKIN_CD", "YJ_CD", "FOOD_KBN", "TENPU_LEVEL" });

            migrationBuilder.CreateIndex(
                name: "MALL_MESSAGE_INF_IDX01",
                table: "MALL_MESSAGE_INF",
                column: "SIN_DATE");

            migrationBuilder.CreateIndex(
                name: "ODR_DATE_DETAIL_IDX01",
                table: "ODR_DATE_DETAIL",
                columns: new[] { "HP_ID", "GRP_ID", "ITEM_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "ODR_DATE_INF_IDX01",
                table: "ODR_DATE_INF",
                columns: new[] { "HP_ID", "GRP_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "ODR_INF_IDX01",
                table: "ODR_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "ODR_INF_DETAIL_IDX01",
                table: "ODR_INF_DETAIL",
                columns: new[] { "HP_ID", "PT_ID", "RAIIN_NO", "ITEM_CD" });

            migrationBuilder.CreateIndex(
                name: "ODR_INF_DETAIL_IDX02",
                table: "ODR_INF_DETAIL",
                column: "ITEM_CD");

            migrationBuilder.CreateIndex(
                name: "ODR_INF_DETAIL_IDX03",
                table: "ODR_INF_DETAIL",
                columns: new[] { "SIN_DATE", "PT_ID", "RAIIN_NO" });

            migrationBuilder.CreateIndex(
                name: "ODR_INF_DETAIL_IDX04",
                table: "ODR_INF_DETAIL",
                columns: new[] { "PT_ID", "SIN_DATE", "ITEM_CD" });

            migrationBuilder.CreateIndex(
                name: "ONLINE_CONFIRMATION_HISTORY_IDX01",
                table: "ONLINE_CONFIRMATION_HISTORY",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "PT_PATH_CONF_IDX01",
                table: "PATH_CONF",
                columns: new[] { "HP_ID", "GRP_CD", "GRP_EDA_NO", "MACHINE", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "PT_PATH_CONF_PKEY",
                table: "PATH_CONF",
                columns: new[] { "HP_ID", "GRP_CD", "GRP_EDA_NO", "SEQ_NO" });

            migrationBuilder.CreateIndex(
                name: "PERMISSION_MST_PKEY",
                table: "PERMISSION_MST",
                columns: new[] { "FUNCTION_CD", "PERMISSION" });

            migrationBuilder.CreateIndex(
                name: "PT_POST_CODE_MST_IDX01",
                table: "POST_CODE_MST",
                columns: new[] { "HP_ID", "POST_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PTCMT_INF_IDX01",
                table: "PT_CMT_INF",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "PT_FAMILY_IDX01",
                table: "PT_FAMILY",
                columns: new[] { "FAMILY_ID", "PT_ID", "FAMILY_PT_ID" });

            migrationBuilder.CreateIndex(
                name: "PT_FAMILY_REKI_IDX01",
                table: "PT_FAMILY_REKI",
                columns: new[] { "ID", "PT_ID", "FAMILY_ID" });

            migrationBuilder.CreateIndex(
                name: "PT_GRP_INF_IDX01",
                table: "PT_GRP_INF",
                columns: new[] { "HP_ID", "PT_ID", "GRP_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_GRP_ITEM_IDX01",
                table: "PT_GRP_ITEM",
                columns: new[] { "HP_ID", "GRP_ID", "GRP_CODE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_GRP_NAME_IDX01",
                table: "PT_GRP_NAME_MST",
                columns: new[] { "HP_ID", "GRP_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_HOKEN_PATTERN_IDX01",
                table: "PT_HOKEN_PATTERN",
                columns: new[] { "HP_ID", "PT_ID", "START_DATE", "END_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_HOKEN_SCAN_IDX01",
                table: "PT_HOKEN_SCAN",
                columns: new[] { "HP_ID", "PT_ID", "HOKEN_GRP", "HOKEN_ID" });

            migrationBuilder.CreateIndex(
                name: "PT_HOKEN_SCAN_PKEY",
                table: "PT_HOKEN_SCAN",
                columns: new[] { "HP_ID", "PT_ID", "HOKEN_GRP", "HOKEN_ID", "SEQ_NO", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_INF_IDX01",
                table: "PT_INF",
                columns: new[] { "HP_ID", "PT_NUM" });

            migrationBuilder.CreateIndex(
                name: "PT_JIBKAR_IDX01",
                table: "PT_JIBKAR",
                columns: new[] { "HP_ID", "WEB_ID", "PT_ID" });

            migrationBuilder.CreateIndex(
                name: "PT_KYUSEI_IDX01",
                table: "PT_KYUSEI",
                columns: new[] { "HP_ID", "PT_ID", "END_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_MEMO_IDX01",
                table: "PT_MEMO",
                columns: new[] { "HP_ID", "PT_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PTPREGNANCY_IDX01",
                table: "PT_PREGNANCY",
                columns: new[] { "ID", "HP_ID" });

            migrationBuilder.CreateIndex(
                name: "PT_ROUSAI_TENKI_IDX01",
                table: "PT_ROUSAI_TENKI",
                columns: new[] { "HP_ID", "PT_ID", "HOKEN_ID", "END_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_CALC_CONF_IDX01",
                table: "PT_SANTEI_CONF",
                columns: new[] { "HP_ID", "PT_ID", "KBN_NO", "EDA_NO", "START_DATE", "END_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "PT_CALC_CONF_PKEY",
                table: "PT_SANTEI_CONF",
                columns: new[] { "HP_ID", "PT_ID", "KBN_NO", "EDA_NO", "SEQ_NO" });

            migrationBuilder.CreateIndex(
                name: "PT_TAG_IDX01",
                table: "PT_TAG",
                columns: new[] { "HP_ID", "PT_ID", "START_DATE", "END_DATE", "IS_DSP_UKETUKE", "IS_DSP_KARTE", "IS_DSP_KAIKEI", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_CMT_INF_IDX01",
                table: "RAIIN_CMT_INF",
                columns: new[] { "HP_ID", "RAIIN_NO", "CMT_KBN", "IS_DELETE" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_FILTER_MST_IDX01",
                table: "RAIIN_FILTER_MST",
                columns: new[] { "HP_ID", "FILTER_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "KARTE_INF_IDX011",
                table: "RAIIN_FILTER_SORT",
                columns: new[] { "ID", "HP_ID", "FILTER_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_INF_IDX01",
                table: "RAIIN_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "STATUS", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_INF_IDX02",
                table: "RAIIN_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "STATUS", "SYOSAISIN_KBN", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_KBN_DETAIL_IDX01",
                table: "RAIIN_KBN_DETAIL",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_KBN_INF_IDX01",
                table: "RAIIN_KBN_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "RAIIN_NO", "GRP_ID", "IS_DELETE" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_KBN_ITEM_IDX01",
                table: "RAIIN_KBN_ITEM",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_KBN_KOUI_IDX01",
                table: "RAIIN_KBN_KOUI",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_KBN_MST_IDX01",
                table: "RAIIN_KBN_MST",
                columns: new[] { "HP_ID", "GRP_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_KBN_YOYAKU_IDX01",
                table: "RAIIN_KBN_YOYAKU",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_CMT_UKEY01",
                table: "RAIIN_LIST_CMT",
                columns: new[] { "HP_ID", "RAIIN_NO", "CMT_KBN", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_DETAIL_IDX01",
                table: "RAIIN_LIST_DETAIL",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_FILE_IDX01",
                table: "RAIIN_LIST_FILE",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_ITEM_IDX01",
                table: "RAIIN_LIST_ITEM",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_KOUI_IDX01",
                table: "RAIIN_LIST_KOUI",
                columns: new[] { "HP_ID", "GRP_ID", "KBN_CD", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_MST_IDX01",
                table: "RAIIN_LIST_MST",
                columns: new[] { "HP_ID", "GRP_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_TAG_UKEY01",
                table: "RAIIN_LIST_TAG",
                columns: new[] { "HP_ID", "RAIIN_NO", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RECE_CMT_IDX01",
                table: "RECE_CMT",
                columns: new[] { "HP_ID", "PT_ID", "SIN_YM", "HOKEN_ID", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "RECEDEN_CMT_SELECT_IDX01",
                table: "RECEDEN_CMT_SELECT",
                columns: new[] { "HP_ID", "ITEM_CD", "START_DATE", "COMMENT_CD", "IS_INVALID" });

            migrationBuilder.CreateIndex(
                name: "PT_KYUSEI_IDX011",
                table: "Z_PT_KYUSEI",
                columns: new[] { "HP_ID", "PT_ID", "END_DATE", "IS_DELETED" });

            migrationBuilder.CreateIndex(
                name: "SYUNO_NYUKIN_IDX01",
                table: "Z_SYUNO_NYUKIN",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "RAIIN_NO", "IS_DELETED" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCOUNTING_FORM_MST");

            migrationBuilder.DropTable(
                name: "APPROVAL_INF");

            migrationBuilder.DropTable(
                name: "AUDIT_TRAIL_LOG");

            migrationBuilder.DropTable(
                name: "AUDIT_TRAIL_LOG_DETAIL");

            migrationBuilder.DropTable(
                name: "AUTO_SANTEI_MST");

            migrationBuilder.DropTable(
                name: "BACKUP_REQ");

            migrationBuilder.DropTable(
                name: "BUI_ODR_BYOMEI_MST");

            migrationBuilder.DropTable(
                name: "BUI_ODR_ITEM_BYOMEI_MST");

            migrationBuilder.DropTable(
                name: "BUI_ODR_ITEM_MST");

            migrationBuilder.DropTable(
                name: "BUI_ODR_MST");

            migrationBuilder.DropTable(
                name: "BYOMEI_MST");

            migrationBuilder.DropTable(
                name: "BYOMEI_MST_AFTERCARE");

            migrationBuilder.DropTable(
                name: "BYOMEI_SET_GENERATION_MST");

            migrationBuilder.DropTable(
                name: "BYOMEI_SET_MST");

            migrationBuilder.DropTable(
                name: "CALC_LOG");

            migrationBuilder.DropTable(
                name: "CALC_STATUS");

            migrationBuilder.DropTable(
                name: "CMT_CHECK_MST");

            migrationBuilder.DropTable(
                name: "CMT_KBN_MST");

            migrationBuilder.DropTable(
                name: "CONTAINER_MST");

            migrationBuilder.DropTable(
                name: "CONVERSION_ITEM_INF");

            migrationBuilder.DropTable(
                name: "DEF_HOKEN_NO");

            migrationBuilder.DropTable(
                name: "DENSI_HAIHAN_CUSTOM");

            migrationBuilder.DropTable(
                name: "DENSI_HAIHAN_DAY");

            migrationBuilder.DropTable(
                name: "DENSI_HAIHAN_KARTE");

            migrationBuilder.DropTable(
                name: "DENSI_HAIHAN_MONTH");

            migrationBuilder.DropTable(
                name: "DENSI_HAIHAN_WEEK");

            migrationBuilder.DropTable(
                name: "DENSI_HOJYO");

            migrationBuilder.DropTable(
                name: "DENSI_HOUKATU");

            migrationBuilder.DropTable(
                name: "DENSI_HOUKATU_GRP");

            migrationBuilder.DropTable(
                name: "DENSI_SANTEI_KAISU");

            migrationBuilder.DropTable(
                name: "DOC_CATEGORY_MST");

            migrationBuilder.DropTable(
                name: "DOC_COMMENT");

            migrationBuilder.DropTable(
                name: "DOC_COMMENT_DETAIL");

            migrationBuilder.DropTable(
                name: "DOC_INF");

            migrationBuilder.DropTable(
                name: "DOSAGE_MST");

            migrationBuilder.DropTable(
                name: "DRUG_DAY_LIMIT");

            migrationBuilder.DropTable(
                name: "DRUG_INF");

            migrationBuilder.DropTable(
                name: "DRUG_UNIT_CONV");

            migrationBuilder.DropTable(
                name: "EVENT_MST");

            migrationBuilder.DropTable(
                name: "EXCEPT_HOKENSYA");

            migrationBuilder.DropTable(
                name: "FILING_AUTO_IMP");

            migrationBuilder.DropTable(
                name: "FILING_CATEGORY_MST");

            migrationBuilder.DropTable(
                name: "FILING_INF");

            migrationBuilder.DropTable(
                name: "FUNCTION_MST");

            migrationBuilder.DropTable(
                name: "GC_STD_MST");

            migrationBuilder.DropTable(
                name: "HOKEN_MST");

            migrationBuilder.DropTable(
                name: "HOKENSYA_MST");

            migrationBuilder.DropTable(
                name: "HOLIDAY_MST");

            migrationBuilder.DropTable(
                name: "HP_INF");

            migrationBuilder.DropTable(
                name: "IPN_KASAN_EXCLUDE");

            migrationBuilder.DropTable(
                name: "IPN_KASAN_EXCLUDE_ITEM");

            migrationBuilder.DropTable(
                name: "IPN_KASAN_MST");

            migrationBuilder.DropTable(
                name: "IPN_MIN_YAKKA_MST");

            migrationBuilder.DropTable(
                name: "IPN_NAME_MST");

            migrationBuilder.DropTable(
                name: "ITEM_GRP_MST");

            migrationBuilder.DropTable(
                name: "JIHI_SBT_MST");

            migrationBuilder.DropTable(
                name: "JOB_MST");

            migrationBuilder.DropTable(
                name: "KA_MST");

            migrationBuilder.DropTable(
                name: "KACODE_MST");

            migrationBuilder.DropTable(
                name: "KAIKEI_DETAIL");

            migrationBuilder.DropTable(
                name: "KAIKEI_INF");

            migrationBuilder.DropTable(
                name: "KANTOKU_MST");

            migrationBuilder.DropTable(
                name: "KARTE_FILTER_DETAIL");

            migrationBuilder.DropTable(
                name: "KARTE_FILTER_MST");

            migrationBuilder.DropTable(
                name: "KARTE_IMG_INF");

            migrationBuilder.DropTable(
                name: "KARTE_INF");

            migrationBuilder.DropTable(
                name: "KARTE_KBN_MST");

            migrationBuilder.DropTable(
                name: "KENSA_CENTER_MST");

            migrationBuilder.DropTable(
                name: "KENSA_INF");

            migrationBuilder.DropTable(
                name: "KENSA_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "KENSA_IRAI_LOG");

            migrationBuilder.DropTable(
                name: "KENSA_MST");

            migrationBuilder.DropTable(
                name: "KENSA_STD_MST");

            migrationBuilder.DropTable(
                name: "KINKI_MST");

            migrationBuilder.DropTable(
                name: "KOGAKU_LIMIT");

            migrationBuilder.DropTable(
                name: "KOHI_PRIORITY");

            migrationBuilder.DropTable(
                name: "KOUI_HOUKATU_MST");

            migrationBuilder.DropTable(
                name: "KOUI_KBN_MST");

            migrationBuilder.DropTable(
                name: "LIMIT_CNT_LIST_INF");

            migrationBuilder.DropTable(
                name: "LIMIT_LIST_INF");

            migrationBuilder.DropTable(
                name: "LIST_SET_GENERATION_MST");

            migrationBuilder.DropTable(
                name: "LIST_SET_MST");

            migrationBuilder.DropTable(
                name: "LOCK_INF");

            migrationBuilder.DropTable(
                name: "LOCK_MST");

            migrationBuilder.DropTable(
                name: "M01_KIJYO_CMT");

            migrationBuilder.DropTable(
                name: "M01_KINKI");

            migrationBuilder.DropTable(
                name: "M01_KINKI_CMT");

            migrationBuilder.DropTable(
                name: "M10_DAY_LIMIT");

            migrationBuilder.DropTable(
                name: "M12_FOOD_ALRGY");

            migrationBuilder.DropTable(
                name: "M12_FOOD_ALRGY_KBN");

            migrationBuilder.DropTable(
                name: "M14_AGE_CHECK");

            migrationBuilder.DropTable(
                name: "M14_CMT_CODE");

            migrationBuilder.DropTable(
                name: "M28_DRUG_MST");

            migrationBuilder.DropTable(
                name: "M34_AR_CODE");

            migrationBuilder.DropTable(
                name: "M34_AR_DISCON");

            migrationBuilder.DropTable(
                name: "M34_AR_DISCON_CODE");

            migrationBuilder.DropTable(
                name: "M34_DRUG_INFO_MAIN");

            migrationBuilder.DropTable(
                name: "M34_FORM_CODE");

            migrationBuilder.DropTable(
                name: "M34_INDICATION_CODE");

            migrationBuilder.DropTable(
                name: "M34_INTERACTION_PAT");

            migrationBuilder.DropTable(
                name: "M34_INTERACTION_PAT_CODE");

            migrationBuilder.DropTable(
                name: "M34_PRECAUTION_CODE");

            migrationBuilder.DropTable(
                name: "M34_PRECAUTIONS");

            migrationBuilder.DropTable(
                name: "M34_PROPERTY_CODE");

            migrationBuilder.DropTable(
                name: "M34_SAR_SYMPTOM_CODE");

            migrationBuilder.DropTable(
                name: "M38_CLASS_CODE");

            migrationBuilder.DropTable(
                name: "M38_ING_CODE");

            migrationBuilder.DropTable(
                name: "M38_INGREDIENTS");

            migrationBuilder.DropTable(
                name: "M38_MAJOR_DIV_CODE");

            migrationBuilder.DropTable(
                name: "M38_OTC_FORM_CODE");

            migrationBuilder.DropTable(
                name: "M38_OTC_MAIN");

            migrationBuilder.DropTable(
                name: "M38_OTC_MAKER_CODE");

            migrationBuilder.DropTable(
                name: "M41_SUPPLE_INDEXCODE");

            migrationBuilder.DropTable(
                name: "M41_SUPPLE_INDEXDEF");

            migrationBuilder.DropTable(
                name: "M41_SUPPLE_INGRE");

            migrationBuilder.DropTable(
                name: "M42_CONTRA_CMT");

            migrationBuilder.DropTable(
                name: "M42_CONTRAINDI_DIS_BC");

            migrationBuilder.DropTable(
                name: "M42_CONTRAINDI_DIS_CLASS");

            migrationBuilder.DropTable(
                name: "M42_CONTRAINDI_DIS_CON");

            migrationBuilder.DropTable(
                name: "M42_CONTRAINDI_DRUG_MAIN_EX");

            migrationBuilder.DropTable(
                name: "M46_DOSAGE_DOSAGE");

            migrationBuilder.DropTable(
                name: "M46_DOSAGE_DRUG");

            migrationBuilder.DropTable(
                name: "M56_ALRGY_DERIVATIVES");

            migrationBuilder.DropTable(
                name: "M56_ANALOGUE_CD");

            migrationBuilder.DropTable(
                name: "M56_DRUG_CLASS");

            migrationBuilder.DropTable(
                name: "M56_DRVALRGY_CODE");

            migrationBuilder.DropTable(
                name: "M56_EX_ANALOGUE");

            migrationBuilder.DropTable(
                name: "M56_EX_ED_INGREDIENTS");

            migrationBuilder.DropTable(
                name: "M56_EX_ING_CODE");

            migrationBuilder.DropTable(
                name: "M56_EX_INGRDT_MAIN");

            migrationBuilder.DropTable(
                name: "M56_PRODRUG_CD");

            migrationBuilder.DropTable(
                name: "M56_USAGE_CODE");

            migrationBuilder.DropTable(
                name: "M56_YJ_DRUG_CLASS");

            migrationBuilder.DropTable(
                name: "MALL_MESSAGE_INF");

            migrationBuilder.DropTable(
                name: "MALL_RENKEI_INF");

            migrationBuilder.DropTable(
                name: "MATERIAL_MST");

            migrationBuilder.DropTable(
                name: "MONSHIN_INF");

            migrationBuilder.DropTable(
                name: "ODR_DATE_DETAIL");

            migrationBuilder.DropTable(
                name: "ODR_DATE_INF");

            migrationBuilder.DropTable(
                name: "ODR_INF");

            migrationBuilder.DropTable(
                name: "ODR_INF_CMT");

            migrationBuilder.DropTable(
                name: "ODR_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "ONLINE_CONFIRMATION");

            migrationBuilder.DropTable(
                name: "ONLINE_CONFIRMATION_HISTORY");

            migrationBuilder.DropTable(
                name: "ONLINE_CONSENT");

            migrationBuilder.DropTable(
                name: "PATH_CONF");

            migrationBuilder.DropTable(
                name: "PAYMENT_METHOD_MST");

            migrationBuilder.DropTable(
                name: "PERMISSION_MST");

            migrationBuilder.DropTable(
                name: "PHYSICAL_AVERAGE");

            migrationBuilder.DropTable(
                name: "PI_IMAGE");

            migrationBuilder.DropTable(
                name: "PI_INF");

            migrationBuilder.DropTable(
                name: "PI_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "PI_PRODUCT_INF");

            migrationBuilder.DropTable(
                name: "POST_CODE_MST");

            migrationBuilder.DropTable(
                name: "PRIORITY_HAIHAN_MST");

            migrationBuilder.DropTable(
                name: "PT_ALRGY_DRUG");

            migrationBuilder.DropTable(
                name: "PT_ALRGY_ELSE");

            migrationBuilder.DropTable(
                name: "PT_ALRGY_FOOD");

            migrationBuilder.DropTable(
                name: "PT_BYOMEI");

            migrationBuilder.DropTable(
                name: "PT_CMT_INF");

            migrationBuilder.DropTable(
                name: "PT_FAMILY");

            migrationBuilder.DropTable(
                name: "PT_FAMILY_REKI");

            migrationBuilder.DropTable(
                name: "PT_GRP_INF");

            migrationBuilder.DropTable(
                name: "PT_GRP_ITEM");

            migrationBuilder.DropTable(
                name: "PT_GRP_NAME_MST");

            migrationBuilder.DropTable(
                name: "PT_HOKEN_CHECK");

            migrationBuilder.DropTable(
                name: "PT_HOKEN_INF");

            migrationBuilder.DropTable(
                name: "PT_HOKEN_PATTERN");

            migrationBuilder.DropTable(
                name: "PT_HOKEN_SCAN");

            migrationBuilder.DropTable(
                name: "PT_INF");

            migrationBuilder.DropTable(
                name: "PT_INFECTION");

            migrationBuilder.DropTable(
                name: "PT_JIBAI_DOC");

            migrationBuilder.DropTable(
                name: "PT_JIBKAR");

            migrationBuilder.DropTable(
                name: "PT_KIO_REKI");

            migrationBuilder.DropTable(
                name: "PT_KOHI");

            migrationBuilder.DropTable(
                name: "PT_KYUSEI");

            migrationBuilder.DropTable(
                name: "PT_LAST_VISIT_DATE");

            migrationBuilder.DropTable(
                name: "PT_MEMO");

            migrationBuilder.DropTable(
                name: "PT_OTC_DRUG");

            migrationBuilder.DropTable(
                name: "PT_OTHER_DRUG");

            migrationBuilder.DropTable(
                name: "PT_PREGNANCY");

            migrationBuilder.DropTable(
                name: "PT_ROUSAI_TENKI");

            migrationBuilder.DropTable(
                name: "PT_SANTEI_CONF");

            migrationBuilder.DropTable(
                name: "PT_SUPPLE");

            migrationBuilder.DropTable(
                name: "PT_TAG");

            migrationBuilder.DropTable(
                name: "RAIIN_CMT_INF");

            migrationBuilder.DropTable(
                name: "RAIIN_FILTER_KBN");

            migrationBuilder.DropTable(
                name: "RAIIN_FILTER_MST");

            migrationBuilder.DropTable(
                name: "RAIIN_FILTER_SORT");

            migrationBuilder.DropTable(
                name: "RAIIN_FILTER_STATE");

            migrationBuilder.DropTable(
                name: "RAIIN_INF");

            migrationBuilder.DropTable(
                name: "RAIIN_KBN_DETAIL");

            migrationBuilder.DropTable(
                name: "RAIIN_KBN_INF");

            migrationBuilder.DropTable(
                name: "RAIIN_KBN_ITEM");

            migrationBuilder.DropTable(
                name: "RAIIN_KBN_KOUI");

            migrationBuilder.DropTable(
                name: "RAIIN_KBN_MST");

            migrationBuilder.DropTable(
                name: "RAIIN_KBN_YOYAKU");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_CMT");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_DETAIL");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_DOC");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_FILE");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_INF");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_ITEM");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_KOUI");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_MST");

            migrationBuilder.DropTable(
                name: "RAIIN_LIST_TAG");

            migrationBuilder.DropTable(
                name: "RECE_CHECK_CMT");

            migrationBuilder.DropTable(
                name: "RECE_CHECK_ERR");

            migrationBuilder.DropTable(
                name: "RECE_CHECK_OPT");

            migrationBuilder.DropTable(
                name: "RECE_CMT");

            migrationBuilder.DropTable(
                name: "RECE_FUTAN_KBN");

            migrationBuilder.DropTable(
                name: "RECE_INF");

            migrationBuilder.DropTable(
                name: "RECE_INF_EDIT");

            migrationBuilder.DropTable(
                name: "RECE_INF_JD");

            migrationBuilder.DropTable(
                name: "RECE_INF_PRE_EDIT");

            migrationBuilder.DropTable(
                name: "RECE_SEIKYU");

            migrationBuilder.DropTable(
                name: "RECE_STATUS");

            migrationBuilder.DropTable(
                name: "RECEDEN_CMT_SELECT");

            migrationBuilder.DropTable(
                name: "RECEDEN_HEN_JIYUU");

            migrationBuilder.DropTable(
                name: "RECEDEN_RIREKI_INF");

            migrationBuilder.DropTable(
                name: "RELEASENOTE_READ");

            migrationBuilder.DropTable(
                name: "RENKEI_CONF");

            migrationBuilder.DropTable(
                name: "RENKEI_MST");

            migrationBuilder.DropTable(
                name: "RENKEI_PATH_CONF");

            migrationBuilder.DropTable(
                name: "RENKEI_REQ");

            migrationBuilder.DropTable(
                name: "RENKEI_TEMPLATE_MST");

            migrationBuilder.DropTable(
                name: "RENKEI_TIMING_CONF");

            migrationBuilder.DropTable(
                name: "RENKEI_TIMING_MST");

            migrationBuilder.DropTable(
                name: "ROUDOU_MST");

            migrationBuilder.DropTable(
                name: "ROUSAI_GOSEI_MST");

            migrationBuilder.DropTable(
                name: "RSV_DAY_COMMENT");

            migrationBuilder.DropTable(
                name: "RSV_FRAME_DAY_PTN");

            migrationBuilder.DropTable(
                name: "RSV_FRAME_INF");

            migrationBuilder.DropTable(
                name: "RSV_FRAME_MST");

            migrationBuilder.DropTable(
                name: "RSV_FRAME_WEEK_PTN");

            migrationBuilder.DropTable(
                name: "RSV_FRAME_WITH");

            migrationBuilder.DropTable(
                name: "RSV_GRP_MST");

            migrationBuilder.DropTable(
                name: "RSV_INF");

            migrationBuilder.DropTable(
                name: "RSV_RENKEI_INF");

            migrationBuilder.DropTable(
                name: "RSV_RENKEI_INF_TK");

            migrationBuilder.DropTable(
                name: "RSVKRT_BYOMEI");

            migrationBuilder.DropTable(
                name: "RSVKRT_KARTE_IMG_INF");

            migrationBuilder.DropTable(
                name: "RSVKRT_KARTE_INF");

            migrationBuilder.DropTable(
                name: "RSVKRT_MST");

            migrationBuilder.DropTable(
                name: "RSVKRT_ODR_INF");

            migrationBuilder.DropTable(
                name: "RSVKRT_ODR_INF_CMT");

            migrationBuilder.DropTable(
                name: "RSVKRT_ODR_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "SANTEI_AUTO_ORDER");

            migrationBuilder.DropTable(
                name: "SANTEI_AUTO_ORDER_DETAIL");

            migrationBuilder.DropTable(
                name: "SANTEI_CNT_CHECK");

            migrationBuilder.DropTable(
                name: "SANTEI_GRP_DETAIL");

            migrationBuilder.DropTable(
                name: "SANTEI_GRP_MST");

            migrationBuilder.DropTable(
                name: "SANTEI_INF");

            migrationBuilder.DropTable(
                name: "SANTEI_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "SCHEMA_CMT_MST");

            migrationBuilder.DropTable(
                name: "SEIKATUREKI_INF");

            migrationBuilder.DropTable(
                name: "SENTENCE_LIST");

            migrationBuilder.DropTable(
                name: "SESSION_INF");

            migrationBuilder.DropTable(
                name: "SET_BYOMEI");

            migrationBuilder.DropTable(
                name: "SET_GENERATION_MST");

            migrationBuilder.DropTable(
                name: "SET_KARTE_IMG_INF");

            migrationBuilder.DropTable(
                name: "SET_KARTE_INF");

            migrationBuilder.DropTable(
                name: "SET_KBN_MST");

            migrationBuilder.DropTable(
                name: "SET_MST");

            migrationBuilder.DropTable(
                name: "SET_ODR_INF");

            migrationBuilder.DropTable(
                name: "SET_ODR_INF_CMT");

            migrationBuilder.DropTable(
                name: "SET_ODR_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "SIN_KOUI");

            migrationBuilder.DropTable(
                name: "SIN_KOUI_COUNT");

            migrationBuilder.DropTable(
                name: "SIN_KOUI_DETAIL");

            migrationBuilder.DropTable(
                name: "SIN_RP_INF");

            migrationBuilder.DropTable(
                name: "SIN_RP_NO_INF");

            migrationBuilder.DropTable(
                name: "SINGLE_DOSE_MST");

            migrationBuilder.DropTable(
                name: "SINREKI_FILTER_MST");

            migrationBuilder.DropTable(
                name: "SINREKI_FILTER_MST_DETAIL");

            migrationBuilder.DropTable(
                name: "SOKATU_MST");

            migrationBuilder.DropTable(
                name: "STA_CONF");

            migrationBuilder.DropTable(
                name: "STA_CSV");

            migrationBuilder.DropTable(
                name: "STA_GRP");

            migrationBuilder.DropTable(
                name: "STA_MENU");

            migrationBuilder.DropTable(
                name: "STA_MST");

            migrationBuilder.DropTable(
                name: "SUMMARY_INF");

            migrationBuilder.DropTable(
                name: "SYOBYO_KEIKA");

            migrationBuilder.DropTable(
                name: "SYOUKI_INF");

            migrationBuilder.DropTable(
                name: "SYOUKI_KBN_MST");

            migrationBuilder.DropTable(
                name: "SYSTEM_CHANGE_LOG");

            migrationBuilder.DropTable(
                name: "SYSTEM_CONF");

            migrationBuilder.DropTable(
                name: "SYSTEM_CONF_ITEM");

            migrationBuilder.DropTable(
                name: "SYSTEM_CONF_MENU");

            migrationBuilder.DropTable(
                name: "SYSTEM_GENERATION_CONF");

            migrationBuilder.DropTable(
                name: "SYUNO_NYUKIN");

            migrationBuilder.DropTable(
                name: "SYUNO_SEIKYU");

            migrationBuilder.DropTable(
                name: "TAG_GRP_MST");

            migrationBuilder.DropTable(
                name: "TEKIOU_BYOMEI_MST");

            migrationBuilder.DropTable(
                name: "TEKIOU_BYOMEI_MST_EXCLUDED");

            migrationBuilder.DropTable(
                name: "TEMPLATE_DETAIL");

            migrationBuilder.DropTable(
                name: "TEMPLATE_DSP_CONF");

            migrationBuilder.DropTable(
                name: "TEMPLATE_MENU_DETAIL");

            migrationBuilder.DropTable(
                name: "TEMPLATE_MENU_MST");

            migrationBuilder.DropTable(
                name: "TEMPLATE_MST");

            migrationBuilder.DropTable(
                name: "TEN_MST");

            migrationBuilder.DropTable(
                name: "TEN_MST_MOTHER");

            migrationBuilder.DropTable(
                name: "TIME_ZONE_CONF");

            migrationBuilder.DropTable(
                name: "TIME_ZONE_DAY_INF");

            migrationBuilder.DropTable(
                name: "TODO_GRP_MST");

            migrationBuilder.DropTable(
                name: "TODO_INF");

            migrationBuilder.DropTable(
                name: "TODO_KBN_MST");

            migrationBuilder.DropTable(
                name: "TOKKI_MST");

            migrationBuilder.DropTable(
                name: "UKETUKE_SBT_DAY_INF");

            migrationBuilder.DropTable(
                name: "UKETUKE_SBT_MST");

            migrationBuilder.DropTable(
                name: "UNIT_MST");

            migrationBuilder.DropTable(
                name: "USER_CONF");

            migrationBuilder.DropTable(
                name: "USER_MST");

            migrationBuilder.DropTable(
                name: "USER_PERMISSION");

            migrationBuilder.DropTable(
                name: "WRK_SIN_KOUI");

            migrationBuilder.DropTable(
                name: "WRK_SIN_KOUI_DETAIL");

            migrationBuilder.DropTable(
                name: "WRK_SIN_KOUI_DETAIL_DEL");

            migrationBuilder.DropTable(
                name: "WRK_SIN_RP_INF");

            migrationBuilder.DropTable(
                name: "YAKKA_SYUSAI_MST");

            migrationBuilder.DropTable(
                name: "YOHO_INF_MST");

            migrationBuilder.DropTable(
                name: "YOHO_SET_MST");

            migrationBuilder.DropTable(
                name: "YOYAKU_ODR_INF");

            migrationBuilder.DropTable(
                name: "YOYAKU_ODR_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "YOYAKU_SBT_MST");

            migrationBuilder.DropTable(
                name: "Z_DOC_INF");

            migrationBuilder.DropTable(
                name: "Z_FILING_INF");

            migrationBuilder.DropTable(
                name: "Z_KENSA_INF");

            migrationBuilder.DropTable(
                name: "Z_KENSA_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "Z_LIMIT_CNT_LIST_INF");

            migrationBuilder.DropTable(
                name: "Z_LIMIT_LIST_INF");

            migrationBuilder.DropTable(
                name: "Z_MONSHIN_INF");

            migrationBuilder.DropTable(
                name: "Z_PT_ALRGY_DRUG");

            migrationBuilder.DropTable(
                name: "Z_PT_ALRGY_ELSE");

            migrationBuilder.DropTable(
                name: "Z_PT_ALRGY_FOOD");

            migrationBuilder.DropTable(
                name: "Z_PT_CMT_INF");

            migrationBuilder.DropTable(
                name: "Z_PT_FAMILY");

            migrationBuilder.DropTable(
                name: "Z_PT_FAMILY_REKI");

            migrationBuilder.DropTable(
                name: "Z_PT_GRP_INF");

            migrationBuilder.DropTable(
                name: "Z_PT_HOKEN_CHECK");

            migrationBuilder.DropTable(
                name: "Z_PT_HOKEN_INF");

            migrationBuilder.DropTable(
                name: "Z_PT_HOKEN_PATTERN");

            migrationBuilder.DropTable(
                name: "Z_PT_HOKEN_SCAN");

            migrationBuilder.DropTable(
                name: "Z_PT_INF");

            migrationBuilder.DropTable(
                name: "Z_PT_INFECTION");

            migrationBuilder.DropTable(
                name: "Z_PT_JIBKAR");

            migrationBuilder.DropTable(
                name: "Z_PT_KIO_REKI");

            migrationBuilder.DropTable(
                name: "Z_PT_KOHI");

            migrationBuilder.DropTable(
                name: "Z_PT_KYUSEI");

            migrationBuilder.DropTable(
                name: "Z_PT_MEMO");

            migrationBuilder.DropTable(
                name: "Z_PT_OTC_DRUG");

            migrationBuilder.DropTable(
                name: "Z_PT_OTHER_DRUG");

            migrationBuilder.DropTable(
                name: "Z_PT_PREGNANCY");

            migrationBuilder.DropTable(
                name: "Z_PT_ROUSAI_TENKI");

            migrationBuilder.DropTable(
                name: "Z_PT_SANTEI_CONF");

            migrationBuilder.DropTable(
                name: "Z_PT_SUPPLE");

            migrationBuilder.DropTable(
                name: "Z_PT_TAG");

            migrationBuilder.DropTable(
                name: "Z_RAIIN_CMT_INF");

            migrationBuilder.DropTable(
                name: "Z_RAIIN_INF");

            migrationBuilder.DropTable(
                name: "Z_RAIIN_KBN_INF");

            migrationBuilder.DropTable(
                name: "Z_RAIIN_LIST_CMT");

            migrationBuilder.DropTable(
                name: "Z_RAIIN_LIST_TAG");

            migrationBuilder.DropTable(
                name: "Z_RECE_CHECK_CMT");

            migrationBuilder.DropTable(
                name: "Z_RECE_CMT");

            migrationBuilder.DropTable(
                name: "Z_RECE_INF_EDIT");

            migrationBuilder.DropTable(
                name: "Z_RECE_SEIKYU");

            migrationBuilder.DropTable(
                name: "Z_RSV_DAY_COMMENT");

            migrationBuilder.DropTable(
                name: "Z_RSV_INF");

            migrationBuilder.DropTable(
                name: "Z_SANTEI_INF_DETAIL");

            migrationBuilder.DropTable(
                name: "Z_SEIKATUREKI_INF");

            migrationBuilder.DropTable(
                name: "Z_SUMMARY_INF");

            migrationBuilder.DropTable(
                name: "Z_SYOBYO_KEIKA");

            migrationBuilder.DropTable(
                name: "Z_SYOUKI_INF");

            migrationBuilder.DropTable(
                name: "Z_SYUNO_NYUKIN");

            migrationBuilder.DropTable(
                name: "Z_TODO_INF");

            migrationBuilder.DropTable(
                name: "Z_UKETUKE_SBT_DAY_INF");
        }
    }
}
