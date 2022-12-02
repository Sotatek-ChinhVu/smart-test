using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostgreDataContext.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNTING_FORM_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    FORM_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FORM_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FORM_TYPE = table.Column<int>(type: "integer", nullable: false),
                    PRINT_SORT = table.Column<int>(type: "integer", nullable: false),
                    MISEISAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    MISYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    FORM = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BASE = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCOUNTING_FORM_MST", x => new { x.HP_ID, x.FORM_NO });
                });

            migrationBuilder.CreateTable(
                name: "APPROVAL_INF",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_INF", x => new { x.ID, x.HP_ID, x.RAIIN_NO });
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_TRAIL_LOG",
                columns: table => new
                {
                    LOG_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LOG_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    EVENT_CD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DAY = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_TRAIL_LOG", x => x.LOG_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_TRAIL_LOG_DETAIL",
                columns: table => new
                {
                    LOG_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOSOKU = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_TRAIL_LOG_DETAIL", x => x.LOG_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUTO_SANTEI_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTO_SANTEI_MST", x => new { x.ID, x.HP_ID, x.ITEM_CD });
                });

            migrationBuilder.CreateTable(
                name: "BACKUP_REQ",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OUTPUT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    FROM_DATE = table.Column<int>(type: "integer", nullable: false),
                    TO_DATE = table.Column<int>(type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BACKUP_REQ", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_BYOMEI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    BUI_ID = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_BUI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_BYOMEI_MST", x => new { x.HP_ID, x.BUI_ID, x.BYOMEI_BUI });
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_ITEM_BYOMEI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BYOMEI_BUI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LR_KBN = table.Column<int>(type: "integer", nullable: false),
                    BOTH_KBN = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_ITEM_BYOMEI_MST", x => new { x.HP_ID, x.ITEM_CD, x.BYOMEI_BUI });
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_ITEM_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_ITEM_MST", x => new { x.HP_ID, x.ITEM_CD });
                });

            migrationBuilder.CreateTable(
                name: "BUI_ODR_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    BUI_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ODR_BUI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LR_KBN = table.Column<int>(type: "integer", nullable: false),
                    MUST_LR_KBN = table.Column<int>(type: "integer", nullable: false),
                    BOTH_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOUI_30 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_40 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_50 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_60 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_70 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_80 = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUI_ODR_MST", x => new { x.HP_ID, x.BUI_ID });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SBYOMEI = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME3 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME4 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME5 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME6 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KANA_NAME7 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IKO_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SIKKAN_CD = table.Column<int>(type: "integer", nullable: false),
                    TANDOKU_KINSI = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_GAI = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_KANRI = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    SAITAKU_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOUKAN_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    SYUSAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    UPD_DATE = table.Column<int>(type: "integer", nullable: false),
                    DEL_DATE = table.Column<int>(type: "integer", nullable: false),
                    NANBYO_CD = table.Column<int>(type: "integer", nullable: false),
                    ICD10_1 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    ICD10_2 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    ICD10_1_2013 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    ICD10_2_2013 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    IS_ADOPTED = table.Column<int>(type: "integer", nullable: false),
                    SYUSYOKU_KBN = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BYOMEI_MST", x => new { x.HP_ID, x.BYOMEI_CD });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_MST_AFTERCARE",
                columns: table => new
                {
                    BYOMEI_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BYOMEI_MST_AFTERCARE", x => new { x.BYOMEI_CD, x.BYOMEI, x.START_DATE });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_SET_GENERATION_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BYOMEI_SET_GENERATION_MST", x => new { x.HP_ID, x.GENERATION_ID });
                });

            migrationBuilder.CreateTable(
                name: "BYOMEI_SET_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LEVEL1 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL2 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL3 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL4 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL5 = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    SET_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IS_TITLE = table.Column<int>(type: "integer", nullable: false),
                    SELECT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BYOMEI_SET_MST", x => new { x.HP_ID, x.GENERATION_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "CALC_LOG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    LOG_SBT = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DEL_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DEL_SBT = table.Column<int>(type: "integer", nullable: false),
                    IS_WARNING = table.Column<int>(type: "integer", nullable: false),
                    TERM_CNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALC_LOG", x => new { x.HP_ID, x.PT_ID, x.RAIIN_NO, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "CALC_STATUS",
                columns: table => new
                {
                    CALC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_UP = table.Column<int>(type: "integer", nullable: false),
                    CALC_MODE = table.Column<int>(type: "integer", nullable: false),
                    CLEAR_RECE_CHK = table.Column<int>(type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALC_STATUS", x => x.CALC_ID);
                });

            migrationBuilder.CreateTable(
                name: "CMT_CHECK_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMT_CHECK_MST", x => new { x.HP_ID, x.ITEM_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "CMT_KBN_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMT_KBN_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "COLUMN_SETTING",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    TABLE_NAME = table.Column<string>(type: "text", nullable: false),
                    COLUMN_NAME = table.Column<string>(type: "text", nullable: false),
                    DISPLAY_ORDER = table.Column<int>(type: "integer", nullable: false),
                    IS_PINNED = table.Column<bool>(type: "boolean", nullable: false),
                    IS_HIDDEN = table.Column<bool>(type: "boolean", nullable: false),
                    WIDTH = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLUMN_SETTING", x => new { x.USER_ID, x.TABLE_NAME, x.COLUMN_NAME });
                });

            migrationBuilder.CreateTable(
                name: "CONTAINER_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    CONTAINER_CD = table.Column<long>(type: "bigint", nullable: false),
                    CONTAINER_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTAINER_MST", x => new { x.HP_ID, x.CONTAINER_CD });
                });

            migrationBuilder.CreateTable(
                name: "CONVERSION_ITEM_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SOURCE_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DEST_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONVERSION_ITEM_INF", x => new { x.HP_ID, x.SOURCE_ITEM_CD, x.DEST_ITEM_CD });
                });

            migrationBuilder.CreateTable(
                name: "DEF_HOKEN_NO",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    DIGIT_1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DIGIT_2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DIGIT_3 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT_4 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT_5 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT_6 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT_7 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    DIGIT_8 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEF_HOKEN_NO", x => new { x.HP_ID, x.DIGIT_1, x.DIGIT_2, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_CUSTOM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HAIHAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TERM_CNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_CUSTOM", x => new { x.ID, x.HP_ID, x.ITEM_CD1, x.SEQ_NO, x.USER_SETTING });
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_DAY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HAIHAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_DAY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_KARTE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HAIHAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_KARTE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_MONTH",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HAIHAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    INC_AFTER = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_MONTH", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HAIHAN_WEEK",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    HAIHAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    INC_AFTER = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HAIHAN_WEEK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HOJYO",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_TERM1 = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_GRP_NO1 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    HOUKATU_TERM2 = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_GRP_NO2 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    HOUKATU_TERM3 = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_GRP_NO3 = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    HAIHAN_DAY = table.Column<int>(type: "integer", nullable: false),
                    HAIHAN_MONTH = table.Column<int>(type: "integer", nullable: false),
                    HAIHAN_KARTE = table.Column<int>(type: "integer", nullable: false),
                    HAIHAN_WEEK = table.Column<int>(type: "integer", nullable: false),
                    NYUIN_ID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_KAISU = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HOJYO", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HOUKATU",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_TERM = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_GRP_NO = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HOUKATU", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_HOUKATU_GRP",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_GRP_NO = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_HOUKATU_GRP", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DENSI_SANTEI_KAISU",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    UNIT_CD = table.Column<int>(type: "integer", nullable: false),
                    MAX_COUNT = table.Column<int>(type: "integer", nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    TERM_COUNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DENSI_SANTEI_KAISU", x => new { x.HP_ID, x.ID, x.ITEM_CD, x.SEQ_NO, x.USER_SETTING });
                });

            migrationBuilder.CreateTable(
                name: "DOC_CATEGORY_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_CATEGORY_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_COMMENT",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_NAME = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    REPLACE_WORD = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_COMMENT", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_COMMENT_DETAIL",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_COMMENT_DETAIL", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "DOC_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DSP_FILE_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IS_LOCKED = table.Column<int>(type: "integer", nullable: false),
                    LOCK_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LOCK_ID = table.Column<int>(type: "integer", nullable: false),
                    LOCK_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOC_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DOSAGE_MST",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ONCE_MIN = table.Column<double>(type: "double precision", nullable: false),
                    ONCE_MAX = table.Column<double>(type: "double precision", nullable: false),
                    ONCE_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    ONCE_UNIT = table.Column<int>(type: "integer", nullable: false),
                    DAY_MIN = table.Column<double>(type: "double precision", nullable: false),
                    DAY_MAX = table.Column<double>(type: "double precision", nullable: false),
                    DAY_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    DAY_UNIT = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOSAGE_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DRUG_DAY_LIMIT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    LIMIT_DAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DRUG_DAY_LIMIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DRUG_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    INF_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DRUG_INF = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DRUG_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DRUG_UNIT_CONV",
                columns: table => new
                {
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CNV_VAL = table.Column<double>(type: "double precision", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DRUG_UNIT_CONV", x => x.ITEM_CD);
                });

            migrationBuilder.CreateTable(
                name: "EVENT_MST",
                columns: table => new
                {
                    EVENT_CD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    EVENT_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AUDIT_TRAILING = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT_MST", x => x.EVENT_CD);
                });

            migrationBuilder.CreateTable(
                name: "EXCEPT_HOKENSYA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOKENSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXCEPT_HOKENSYA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FILING_AUTO_IMP",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IMP_PATH = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILING_AUTO_IMP", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "FILING_CATEGORY_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    DSP_KANZOK = table.Column<int>(type: "integer", nullable: false),
                    IS_FILE_DELETED = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILING_CATEGORY_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "FILING_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILE_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    GET_DATE = table.Column<int>(type: "integer", nullable: false),
                    FILE_NO = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DSP_FILE_NAME = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILING_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "FUNCTION_MST",
                columns: table => new
                {
                    FUNCTION_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    FUNCTION_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUNCTION_MST", x => x.FUNCTION_CD);
                });

            migrationBuilder.CreateTable(
                name: "GC_STD_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    STD_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    POINT = table.Column<double>(type: "double precision", nullable: false),
                    SD_M25 = table.Column<double>(type: "double precision", nullable: false),
                    SD_M20 = table.Column<double>(type: "double precision", nullable: false),
                    SD_M10 = table.Column<double>(type: "double precision", nullable: false),
                    SD_AVG = table.Column<double>(type: "double precision", nullable: false),
                    SD_P10 = table.Column<double>(type: "double precision", nullable: false),
                    SD_P20 = table.Column<double>(type: "double precision", nullable: false),
                    SD_P25 = table.Column<double>(type: "double precision", nullable: false),
                    PER_03 = table.Column<double>(type: "double precision", nullable: false),
                    PER_10 = table.Column<double>(type: "double precision", nullable: false),
                    PER_25 = table.Column<double>(type: "double precision", nullable: false),
                    PER_50 = table.Column<double>(type: "double precision", nullable: false),
                    PER_75 = table.Column<double>(type: "double precision", nullable: false),
                    PER_90 = table.Column<double>(type: "double precision", nullable: false),
                    PER_97 = table.Column<double>(type: "double precision", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GC_STD_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "HOKEN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_SBT_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KOHI_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HOKEN_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HOKEN_SNAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    HOKEN_NAME_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CHECK_DIGIT = table.Column<int>(type: "integer", nullable: false),
                    JYUKYU_CHECK_DIGIT = table.Column<int>(type: "integer", nullable: false),
                    IS_FUTANSYA_NO_CHECK = table.Column<int>(type: "integer", nullable: false),
                    IS_JYUKYUSYA_NO_CHECK = table.Column<int>(type: "integer", nullable: false),
                    IS_TOKUSYU_NO_CHECK = table.Column<int>(type: "integer", nullable: false),
                    IS_LIMIT_LIST = table.Column<int>(type: "integer", nullable: false),
                    IS_LIMIT_LIST_SUM = table.Column<int>(type: "integer", nullable: false),
                    IS_OTHER_PREF_VALID = table.Column<int>(type: "integer", nullable: false),
                    AGE_START = table.Column<int>(type: "integer", nullable: false),
                    AGE_END = table.Column<int>(type: "integer", nullable: false),
                    EN_TEN = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    RECE_SP_KBN = table.Column<int>(type: "integer", nullable: false),
                    RECE_SEIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    RECE_FUTAN_ROUND = table.Column<int>(type: "integer", nullable: false),
                    RECE_KISAI = table.Column<int>(type: "integer", nullable: false),
                    RECE_KISAI2 = table.Column<int>(type: "integer", nullable: false),
                    RECE_ZERO_KISAI = table.Column<int>(type: "integer", nullable: false),
                    RECE_FUTAN_HIDE = table.Column<int>(type: "integer", nullable: false),
                    RECE_FUTAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    RECE_TEN_KISAI = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TOTAL_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TOTAL_ALL = table.Column<int>(type: "integer", nullable: false),
                    CALC_SP_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TOTAL_EXC_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TEKIYO = table.Column<int>(type: "integer", nullable: false),
                    FUTAN_YUSEN = table.Column<int>(type: "integer", nullable: false),
                    LIMIT_KBN = table.Column<int>(type: "integer", nullable: false),
                    COUNT_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUTAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUTAN_RATE = table.Column<int>(type: "integer", nullable: false),
                    KAI_FUTANGAKU = table.Column<int>(type: "integer", nullable: false),
                    KAI_LIMIT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    DAY_LIMIT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    DAY_LIMIT_COUNT = table.Column<int>(type: "integer", nullable: false),
                    MONTH_LIMIT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    MONTH_SP_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    MONTH_LIMIT_COUNT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    RECE_KISAI_KOKHO = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_HAIRYO_KBN = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOKEN_MST", x => new { x.HP_ID, x.PREF_NO, x.HOKEN_NO, x.HOKEN_EDA_NO, x.START_DATE });
                });

            migrationBuilder.CreateTable(
                name: "HOKENSYA_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKENSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HOUBETU_KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IS_KIGO_NA = table.Column<int>(type: "integer", nullable: false),
                    RATE_HONNIN = table.Column<int>(type: "integer", nullable: false),
                    RATE_KAZOKU = table.Column<int>(type: "integer", nullable: false),
                    POST_CODE = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    ADDRESS1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ADDRESS2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TEL1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    DELETE_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOKENSYA_MST", x => new { x.HP_ID, x.HOKENSYA_NO });
                });

            migrationBuilder.CreateTable(
                name: "HOLIDAY_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOLIDAY_KBN = table.Column<int>(type: "integer", nullable: false),
                    KYUSIN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOLIDAY_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOLIDAY_MST", x => new { x.HP_ID, x.SIN_DATE, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "HP_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    HP_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    ROUSAI_HP_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    HP_NAME = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    RECE_HP_NAME = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    KAISETU_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    POST_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    FAX_NO = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    OTHER_CONTACTS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "IPN_KASAN_EXCLUDE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_KASAN_EXCLUDE", x => new { x.HP_ID, x.START_DATE, x.IPN_NAME_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "IPN_KASAN_EXCLUDE_ITEM",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_KASAN_EXCLUDE_ITEM", x => new { x.HP_ID, x.START_DATE, x.ITEM_CD });
                });

            migrationBuilder.CreateTable(
                name: "IPN_KASAN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    KASAN1 = table.Column<int>(type: "integer", nullable: false),
                    KASAN2 = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_KASAN_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "IPN_MIN_YAKKA_MST",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    YAKKA = table.Column<double>(type: "double precision", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_MIN_YAKKA_MST", x => new { x.HP_ID, x.ID, x.IPN_NAME_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "IPN_NAME_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPN_NAME_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ITEM_GRP_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_SBT = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_GRP_CD = table.Column<long>(type: "bigint", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_GRP_MST", x => new { x.HP_ID, x.GRP_SBT, x.ITEM_GRP_CD, x.SEQ_NO, x.START_DATE });
                });

            migrationBuilder.CreateTable(
                name: "JIHI_SBT_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    JIHI_SBT = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    IS_YOBO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JIHI_SBT_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "JOB_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    JOB_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JOB_NAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOB_MST", x => new { x.JOB_CD, x.HP_ID });
                });

            migrationBuilder.CreateTable(
                name: "JSON_SETTING",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    KEY = table.Column<string>(type: "text", nullable: false),
                    VALUE = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JSON_SETTING", x => new { x.USER_ID, x.KEY });
                });

            migrationBuilder.CreateTable(
                name: "KA_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    RECE_KA_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    KA_SNAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    KA_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KA_MST", x => new { x.ID, x.HP_ID });
                });

            migrationBuilder.CreateTable(
                name: "KACODE_MST",
                columns: table => new
                {
                    RECE_KA_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    KA_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KACODE_MST", x => x.RECE_KA_CD);
                });

            migrationBuilder.CreateTable(
                name: "KAIKEI_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_PID = table.Column<int>(type: "integer", nullable: false),
                    OYA_RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    ADJUST_KID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_SBT_CD = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ID = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_ID = table.Column<int>(type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI2_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI3_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI4_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_PRIORITY = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    KOHI2_PRIORITY = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    KOHI3_PRIORITY = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    KOHI4_PRIORITY = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    HONKE_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TEKIYO_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_TOKUREI = table.Column<int>(type: "integer", nullable: false),
                    IS_TASUKAI = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TOTAL_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_CHOKI = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_KOGAKU_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    EN_TEN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_RATE = table.Column<int>(type: "integer", nullable: false),
                    PT_RATE = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    TENSU = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_IRYOHI = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    ICHIBU_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_GAKU_10EN = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_ROUND = table.Column<int>(type: "integer", nullable: false),
                    PT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_OVER_KBN = table.Column<int>(type: "integer", nullable: false),
                    RECE_SBT = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    JITUNISU = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_I_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_RO_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_I_TENSU = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_RO_TENSU = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HA_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_NI_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HO_SINDAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HO_SINDAN_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HE_MEISAI = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HE_MEISAI_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_A_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_B_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_C_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_D_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_KENPO_TENSU = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_KENPO_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_TAX = table.Column<int>(type: "integer", nullable: false),
                    JIHI_OUTTAX = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_TAXFREE = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_TAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_TAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_OUTTAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_OUTTAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_TAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_TAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_OUTTAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_OUTTAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_PT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<string>(type: "character varying(61)", maxLength: 61, nullable: false),
                    IS_NINPU = table.Column<int>(type: "integer", nullable: false),
                    IS_ZAIISO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KAIKEI_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "KAIKEI_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_SBT_CD = table.Column<int>(type: "integer", nullable: false),
                    RECE_SBT = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI2_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI3_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI4_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HONKE_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_RATE = table.Column<int>(type: "integer", nullable: false),
                    PT_RATE = table.Column<int>(type: "integer", nullable: false),
                    DISP_RATE = table.Column<int>(type: "integer", nullable: false),
                    TENSU = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_IRYOHI = table.Column<int>(type: "integer", nullable: false),
                    PT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_TAX = table.Column<int>(type: "integer", nullable: false),
                    JIHI_OUTTAX = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_TAXFREE = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_TAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_TAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_OUTTAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_FUTAN_OUTTAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_TAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_TAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    JIHI_OUTTAX_NR = table.Column<int>(type: "integer", nullable: false),
                    JIHI_OUTTAX_GEN = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_ROUND = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_PT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_FUTAN_VAL = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_FUTAN_RANGE = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_RATE_VAL = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_RATE_RANGE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KAIKEI_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "KANTOKU_MST",
                columns: table => new
                {
                    ROUDOU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    KANTOKU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    KANTOKU_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KANTOKU_MST", x => new { x.ROUDOU_CD, x.KANTOKU_CD });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_FILTER_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    FILTER_ID = table.Column<long>(type: "bigint", nullable: false),
                    FILTER_ITEM_CD = table.Column<int>(type: "integer", nullable: false),
                    FILTER_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_FILTER_DETAIL", x => new { x.HP_ID, x.USER_ID, x.FILTER_ID, x.FILTER_ITEM_CD, x.FILTER_EDA_NO });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_FILTER_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    FILTER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILTER_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    AUTO_APPLY = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_FILTER_MST", x => new { x.HP_ID, x.USER_ID, x.FILTER_ID });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_IMG_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    POSITION = table.Column<long>(type: "bigint", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MESSAGE = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_IMG_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KARTE_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RICH_TEXT = table.Column<byte[]>(type: "bytea", nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_INF", x => new { x.HP_ID, x.RAIIN_NO, x.SEQ_NO, x.KARTE_KBN });
                });

            migrationBuilder.CreateTable(
                name: "KARTE_KBN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    KBN_NAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    KBN_SHORT_NAME = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    CAN_IMG = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KARTE_KBN_MST", x => new { x.HP_ID, x.KARTE_KBN });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_CENTER_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    CENTER_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CENTER_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    PRIMARY_KBN = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_CENTER_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KENSA_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    IRAI_CD = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    TOSEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    RESULT_CHECK = table.Column<int>(type: "integer", nullable: false),
                    CENTER_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NYUBI = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    YOKETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    BILIRUBIN = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "KENSA_INF_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    IRAI_CD = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    KENSA_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    RESULT_VAL = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    RESULT_TYPE = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    ABNORMAL_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CMT_CD1 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    CMT_CD2 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_INF_DETAIL", x => new { x.HP_ID, x.PT_ID, x.IRAI_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_IRAI_LOG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    CENTER_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IRAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    FROM_DATE = table.Column<int>(type: "integer", nullable: false),
                    TO_DATE = table.Column<int>(type: "integer", nullable: false),
                    IRAI_FILE = table.Column<string>(type: "text", nullable: false),
                    IRAI_LIST = table.Column<byte[]>(type: "bytea", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_IRAI_LOG", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "KENSA_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    KENSA_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    KENSA_ITEM_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    CENTER_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    KENSA_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KENSA_KANA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MATERIAL_CD = table.Column<int>(type: "integer", nullable: false),
                    CONTAINER_CD = table.Column<int>(type: "integer", nullable: false),
                    MALE_STD = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    MALE_STD_LOW = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    MALE_STD_HIGH = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALE_STD = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALE_STD_LOW = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    FEMALE_STD_HIGH = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    FORMULA = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DIGIT = table.Column<int>(type: "integer", nullable: false),
                    OYA_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    OYA_ITEM_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<long>(type: "bigint", nullable: false),
                    CENTER_ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CENTER_ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_MST", x => new { x.HP_ID, x.KENSA_ITEM_CD, x.KENSA_ITEM_SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "KENSA_STD_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    KENSA_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    MALE_STD = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    MALE_STD_LOW = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    MALE_STD_HIGH = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    FEMALE_STD = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    FEMALE_STD_LOW = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    FEMALE_STD_HIGH = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KENSA_STD_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "KINKI_MST",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    A_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    B_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KINKI_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KOGAKU_LIMIT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    AGE_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    INCOME_KBN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    BASE_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    TASU_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOGAKU_LIMIT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "KOHI_PRIORITY",
                columns: table => new
                {
                    PREF_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    PRIORITY_NO = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOHI_PRIORITY", x => x.PREF_NO);
                });

            migrationBuilder.CreateTable(
                name: "KOUI_HOUKATU_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUKATU_TERM = table.Column<int>(type: "integer", nullable: false),
                    KOUI_FROM = table.Column<int>(type: "integer", nullable: false),
                    KOUI_TO = table.Column<int>(type: "integer", nullable: false),
                    IGNORE_SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOUI_HOUKATU_MST", x => new { x.HP_ID, x.ITEM_CD, x.START_DATE });
                });

            migrationBuilder.CreateTable(
                name: "KOUI_KBN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    KOUI_KBN_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    KOUI_KBN1 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_KBN2 = table.Column<int>(type: "integer", nullable: false),
                    KOUI_GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    KOUI_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOUI_KBN_MST", x => new { x.HP_ID, x.KOUI_KBN_ID });
                });

            migrationBuilder.CreateTable(
                name: "LIMIT_CNT_LIST_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KOHI_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<string>(type: "character varying(61)", maxLength: 61, nullable: false),
                    OYA_RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIMIT_CNT_LIST_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "LIMIT_LIST_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KOHI_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<string>(type: "character varying(61)", maxLength: 61, nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    FUTAN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_GAKU = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIMIT_LIST_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LIST_SET_GENERATION_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIST_SET_GENERATION_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "LIST_SET_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SET_KBN = table.Column<int>(type: "integer", nullable: false),
                    LEVEL1 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL2 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL3 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL4 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL5 = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SET_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    IS_TITLE = table.Column<int>(type: "integer", nullable: false),
                    SELECT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNIT_SBT = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIST_SET_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOCK_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    FUNCTION_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    SIN_DATE = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    OYA_RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    MACHINE = table.Column<string>(type: "text", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    LOCK_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCK_INF", x => new { x.HP_ID, x.PT_ID, x.FUNCTION_CD, x.SIN_DATE, x.RAIIN_NO, x.OYA_RAIIN_NO });
                });

            migrationBuilder.CreateTable(
                name: "LOCK_MST",
                columns: table => new
                {
                    FUNCTION_CD_A = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    FUNCTION_CD_B = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    LOCK_RANGE = table.Column<int>(type: "integer", nullable: false),
                    LOCK_LEVEL = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCK_MST", x => x.FUNCTION_CD_A);
                });

            migrationBuilder.CreateTable(
                name: "M01_KIJYO_CMT",
                columns: table => new
                {
                    CMT_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M01_KIJYO_CMT", x => x.CMT_CD);
                });

            migrationBuilder.CreateTable(
                name: "M01_KINKI",
                columns: table => new
                {
                    A_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    B_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CMT_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    SAYOKIJYO_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    KYODO_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KYODO = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    DATA_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M01_KINKI", x => x.A_CD);
                });

            migrationBuilder.CreateTable(
                name: "M01_KINKI_CMT",
                columns: table => new
                {
                    CMT_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M01_KINKI_CMT", x => x.CMT_CD);
                });

            migrationBuilder.CreateTable(
                name: "M10_DAY_LIMIT",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    LIMIT_DAY = table.Column<int>(type: "integer", nullable: false),
                    ST_DATE = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ED_DATE = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CMT = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M10_DAY_LIMIT", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M12_FOOD_ALRGY",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FOOD_KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    TENPU_LEVEL = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    KIKIN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    ATTENTION_CMT = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    WORKING_MECHANISM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M12_FOOD_ALRGY", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M12_FOOD_ALRGY_KBN",
                columns: table => new
                {
                    FOOD_KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    FOOD_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M12_FOOD_ALRGY_KBN", x => x.FOOD_KBN);
                });

            migrationBuilder.CreateTable(
                name: "M14_AGE_CHECK",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ATTENTION_CMT_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    WORKING_MECHANISM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TENPU_LEVEL = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGE_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    WEIGHT_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    SEX_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    AGE_MIN = table.Column<double>(type: "double precision", nullable: false),
                    AGE_MAX = table.Column<double>(type: "double precision", nullable: false),
                    WEIGHT_MIN = table.Column<double>(type: "double precision", nullable: false),
                    WEIGHT_MAX = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M14_AGE_CHECK", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M14_CMT_CODE",
                columns: table => new
                {
                    ATTENTION_CMT_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    ATTENTION_CMT = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M14_CMT_CODE", x => x.ATTENTION_CMT_CD);
                });

            migrationBuilder.CreateTable(
                name: "M28_DRUG_MST",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    KOSEISYO_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    KIKIN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    DRUG_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DRUG_KANA1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DRUG_KANA2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IPN_NAME = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    IPN_KANA = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    YAKKA_VAL = table.Column<int>(type: "integer", nullable: false),
                    YAKKA_UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SEIBUN_RIKIKA = table.Column<double>(type: "double precision", nullable: false),
                    SEIBUN_RIKIKA_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    YORYO_JYURYO = table.Column<double>(type: "double precision", nullable: false),
                    YORYO_JYURYO_UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SEIRIKI_YORYO_RATE = table.Column<double>(type: "double precision", nullable: false),
                    SEIRIKI_YORYO_UNIT = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    MAKER_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    MAKER_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    DRUG_KBN_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DRUG_KBN = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FORM_KBN_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    FORM_KBN = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DOKUYAKU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    GEKIYAKU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    MAYAKU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOSEISINYAKU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KAKUSEIZAI_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KAKUSEIZAI_GENRYO_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    SEIBUTU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    SP_SEIBUTU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOHATU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    YAKKA = table.Column<double>(type: "double precision", nullable: false),
                    KIKAKU_UNIT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    YAKKA_RATE_FORMURA = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    YAKKA_RATE_UNIT = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    YAKKA_SYUSAI_DATE = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    KEIKASOTI_DATE = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    MAIN_DRUG_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    MAIN_DRUG_NAME = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    MAIN_DRUG_KANA = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    KEY_SEIBUN = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HAIGO_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    MAIN_DRUG_NAME_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M28_DRUG_MST", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_AR_CODE",
                columns: table => new
                {
                    FUKUSAYO_CD = table.Column<string>(type: "text", nullable: false),
                    FUKUSAYO_CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_AR_CODE", x => x.FUKUSAYO_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_AR_DISCON",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "text", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    FUKUSAYO_CD = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_AR_DISCON", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_AR_DISCON_CODE",
                columns: table => new
                {
                    FUKUSAYO_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    FUKUSAYO_CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_AR_DISCON_CODE", x => x.FUKUSAYO_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_DRUG_INFO_MAIN",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "text", nullable: false),
                    FORM_CD = table.Column<string>(type: "text", nullable: false),
                    COLOR = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MARK = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    KONO_CD = table.Column<string>(type: "text", nullable: false),
                    FUKUSAYO_CD = table.Column<string>(type: "text", nullable: true),
                    FUKUSAYO_INIT_CD = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_DRUG_INFO_MAIN", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_FORM_CODE",
                columns: table => new
                {
                    FORM_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    FORM = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_FORM_CODE", x => x.FORM_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_INDICATION_CODE",
                columns: table => new
                {
                    KONO_CD = table.Column<string>(type: "text", nullable: false),
                    KONO_DETAIL_CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    KONO_SIMPLE_CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_INDICATION_CODE", x => x.KONO_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_INTERACTION_PAT",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "text", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    INTERACTION_PAT_CD = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_INTERACTION_PAT", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_INTERACTION_PAT_CODE",
                columns: table => new
                {
                    INTERACTION_PAT_CD = table.Column<string>(type: "text", nullable: false),
                    INTERACTION_PAT_CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_INTERACTION_PAT_CODE", x => x.INTERACTION_PAT_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_PRECAUTION_CODE",
                columns: table => new
                {
                    PRECAUTION_CD = table.Column<string>(type: "text", nullable: false),
                    EXTEND_CD = table.Column<string>(type: "text", nullable: false),
                    PRECAUTION_CMT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PROPERTY_CD = table.Column<int>(type: "integer", nullable: false),
                    AGE_MAX = table.Column<int>(type: "integer", nullable: false),
                    AGE_MIN = table.Column<int>(type: "integer", nullable: false),
                    SEX_CD = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_PRECAUTION_CODE", x => x.PRECAUTION_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_PRECAUTIONS",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "text", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    PRECAUTION_CD = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_PRECAUTIONS", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_PROPERTY_CODE",
                columns: table => new
                {
                    PROPERTY_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PROPERTY = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_PROPERTY_CODE", x => x.PROPERTY_CD);
                });

            migrationBuilder.CreateTable(
                name: "M34_SAR_SYMPTOM_CODE",
                columns: table => new
                {
                    FUKUSAYO_INIT_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    FUKUSAYO_INIT_CMT = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M34_SAR_SYMPTOM_CODE", x => x.FUKUSAYO_INIT_CD);
                });

            migrationBuilder.CreateTable(
                name: "M38_CLASS_CODE",
                columns: table => new
                {
                    CLASS_CD = table.Column<string>(type: "text", nullable: false),
                    CLASS_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MAJOR_DIV_CD = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_CLASS_CODE", x => x.CLASS_CD);
                });

            migrationBuilder.CreateTable(
                name: "M38_ING_CODE",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    SEIBUN = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_ING_CODE", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M38_INGREDIENTS",
                columns: table => new
                {
                    SERIAL_NUM = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEIBUN_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    SBT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_INGREDIENTS", x => x.SERIAL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "M38_MAJOR_DIV_CODE",
                columns: table => new
                {
                    MAJOR_DIV_CD = table.Column<string>(type: "text", nullable: false),
                    MAJOR_DIV_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_MAJOR_DIV_CODE", x => x.MAJOR_DIV_CD);
                });

            migrationBuilder.CreateTable(
                name: "M38_OTC_FORM_CODE",
                columns: table => new
                {
                    FORM_CD = table.Column<string>(type: "text", nullable: false),
                    FORM = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_OTC_FORM_CODE", x => x.FORM_CD);
                });

            migrationBuilder.CreateTable(
                name: "M38_OTC_MAIN",
                columns: table => new
                {
                    SERIAL_NUM = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OTC_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    TRADE_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TRADE_KANA = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    CLASS_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    COMPANY_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    TRADE_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    DRUG_FORM_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    YOHO_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_OTC_MAIN", x => x.SERIAL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "M38_OTC_MAKER_CODE",
                columns: table => new
                {
                    MAKER_CD = table.Column<string>(type: "text", nullable: false),
                    MAKER_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MAKER_KANA = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M38_OTC_MAKER_CODE", x => x.MAKER_CD);
                });

            migrationBuilder.CreateTable(
                name: "M41_SUPPLE_INDEXCODE",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    INDEX_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M41_SUPPLE_INDEXCODE", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M41_SUPPLE_INDEXDEF",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    INDEX_WORD = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TOKUHO_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M41_SUPPLE_INDEXDEF", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M41_SUPPLE_INGRE",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    SEIBUN = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M41_SUPPLE_INGRE", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRA_CMT",
                columns: table => new
                {
                    CMT_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    CMT = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRA_CMT", x => x.CMT_CD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DIS_BC",
                columns: table => new
                {
                    BYOTAI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOTAI_CLASS_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DIS_BC", x => x.BYOTAI_CD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DIS_CLASS",
                columns: table => new
                {
                    BYOTAI_CLASS_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    BYOTAI = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DIS_CLASS", x => x.BYOTAI_CLASS_CD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DIS_CON",
                columns: table => new
                {
                    BYOTAI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    STANDARD_BYOTAI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    BYOTAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    ICD10 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    RECE_CD = table.Column<string>(type: "character varying(33)", maxLength: 33, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DIS_CON", x => x.BYOTAI_CD);
                });

            migrationBuilder.CreateTable(
                name: "M42_CONTRAINDI_DRUG_MAIN_EX",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    TENPU_LEVEL = table.Column<int>(type: "integer", nullable: false),
                    BYOTAI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    CMT_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    STAGE = table.Column<int>(type: "integer", nullable: false),
                    KIO_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    FAMILY_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KIJYO_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M42_CONTRAINDI_DRUG_MAIN_EX", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M46_DOSAGE_DOSAGE",
                columns: table => new
                {
                    DOEI_CD = table.Column<string>(type: "text", nullable: false),
                    DOEI_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    KONOKOKA_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    KENSA_PCD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    AGE_OVER = table.Column<double>(type: "double precision", nullable: false),
                    AGE_UNDER = table.Column<double>(type: "double precision", nullable: false),
                    AGE_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    WEIGHT_OVER = table.Column<double>(type: "double precision", nullable: false),
                    WEIGHT_UNDER = table.Column<double>(type: "double precision", nullable: false),
                    BODY_OVER = table.Column<double>(type: "double precision", nullable: false),
                    BODY_UNDER = table.Column<double>(type: "double precision", nullable: false),
                    DRUG_ROUTE = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    USE_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DRUG_CONDITION = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    KONOKOKA = table.Column<string>(type: "text", nullable: false),
                    USAGE_DOSAGE = table.Column<string>(type: "text", nullable: false),
                    FILENAME_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    DRUG_SYUGI = table.Column<string>(type: "text", nullable: false),
                    TEKIO_BUI = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    YOUKAI_KISYAKU = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    KISYAKUEKI = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    YOUKAIEKI = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HAITA_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    NG_KISYAKUEKI = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    NG_YOUKAIEKI = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    COMBI_DRUG = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DRUG_LINK_CD = table.Column<int>(type: "integer", nullable: false),
                    DRUG_ORDER = table.Column<int>(type: "integer", nullable: false),
                    SINGLE_DRUG_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KYUGEN_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DOSAGE_CHECK_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ONCE_MIN = table.Column<double>(type: "double precision", nullable: false),
                    ONCE_MAX = table.Column<double>(type: "double precision", nullable: false),
                    ONCE_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ONCE_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    ONCE_LIMIT_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DAY_MIN_CNT = table.Column<double>(type: "double precision", nullable: false),
                    DAY_MAX_CNT = table.Column<double>(type: "double precision", nullable: false),
                    DAY_MIN = table.Column<double>(type: "double precision", nullable: false),
                    DAY_MAX = table.Column<double>(type: "double precision", nullable: false),
                    DAY_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DAY_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    DAY_LIMIT_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RISE = table.Column<int>(type: "integer", nullable: false),
                    MORNING = table.Column<int>(type: "integer", nullable: false),
                    DAYTIME = table.Column<int>(type: "integer", nullable: false),
                    NIGHT = table.Column<int>(type: "integer", nullable: false),
                    SLEEP = table.Column<int>(type: "integer", nullable: false),
                    BEFORE_MEAL = table.Column<int>(type: "integer", nullable: false),
                    JUST_BEFORE_MEAL = table.Column<int>(type: "integer", nullable: false),
                    AFTER_MEAL = table.Column<int>(type: "integer", nullable: false),
                    JUST_AFTER_MEAL = table.Column<int>(type: "integer", nullable: false),
                    BETWEEN_MEAL = table.Column<int>(type: "integer", nullable: false),
                    ELSE_TIME = table.Column<int>(type: "integer", nullable: false),
                    DOSAGE_LIMIT_TERM = table.Column<int>(type: "integer", nullable: false),
                    DOSAGE_LIMIT_UNIT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    UNITTERM_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    UNITTERM_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DOSAGE_ADD_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    INC_DEC_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DEC_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    INC_DEC_INTERVAL = table.Column<int>(type: "integer", nullable: false),
                    INC_DEC_INTERVAL_UNIT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DEC_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    INC_LIMIT = table.Column<double>(type: "double precision", nullable: false),
                    INC_DEC_LIMIT_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TIME_DEPEND = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    JUDGE_TERM = table.Column<int>(type: "integer", nullable: false),
                    JUDGE_TERM_UNIT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    EXTEND_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ADD_TERM = table.Column<int>(type: "integer", nullable: false),
                    ADD_TERM_UNIT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    INTERVAL_WARNING_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M46_DOSAGE_DOSAGE", x => x.DOEI_CD);
                });

            migrationBuilder.CreateTable(
                name: "M46_DOSAGE_DRUG",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DOEI_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    DRUG_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KIKAKU_UNIT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    YAKKA_UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RIKIKA_RATE = table.Column<decimal>(type: "numeric", nullable: false),
                    RIKIKA_UNIT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    YOUKAIEKI_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M46_DOSAGE_DRUG", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_ALRGY_DERIVATIVES",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DRVALRGY_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    SEIBUN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_ALRGY_DERIVATIVES", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_ANALOGUE_CD",
                columns: table => new
                {
                    ANALOGUE_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    ANALOGUE_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_ANALOGUE_CD", x => x.ANALOGUE_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_DRUG_CLASS",
                columns: table => new
                {
                    CLASS_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CLASS_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CLASS_DUPLICATION = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_DRUG_CLASS", x => x.CLASS_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_DRVALRGY_CODE",
                columns: table => new
                {
                    DRVALRGY_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    DRVALRGY_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DRVALRGY_GRP = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    RANK_NO = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_DRVALRGY_CODE", x => x.DRVALRGY_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_ANALOGUE   ",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    SEQ_NO = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ANALOGUE_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_ANALOGUE   ", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_ED_INGREDIENTS",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SEQ_NO = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    SEIBUN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    SEIBUN_INDEX_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    SBT = table.Column<int>(type: "integer", nullable: false),
                    PRODRUG_CHECK = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ANALOGUE_CHECK = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    YOKAIEKI_CHECK = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    TENKABUTU_CHECK = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_ED_INGREDIENTS", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_ING_CODE",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    SEIBUN_INDEX_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    SEIBUN_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    YOHO_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_ING_CODE", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_EX_INGRDT_MAIN",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DRUG_KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    YOHO_CD = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    HAIGOU_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    YUEKI_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KANPO_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ZENSINSAYO_FLG = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_EX_INGRDT_MAIN", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_PRODRUG_CD",
                columns: table => new
                {
                    SEIBUN_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    SEQ_NO = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    KASSEITAI_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_PRODRUG_CD", x => x.SEIBUN_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_USAGE_CODE",
                columns: table => new
                {
                    YOHO_CD = table.Column<string>(type: "text", nullable: false),
                    YOHO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_USAGE_CODE", x => x.YOHO_CD);
                });

            migrationBuilder.CreateTable(
                name: "M56_YJ_DRUG_CLASS",
                columns: table => new
                {
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CLASS_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M56_YJ_DRUG_CLASS", x => x.YJ_CD);
                });

            migrationBuilder.CreateTable(
                name: "MALL_MESSAGE_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RECEIVE_NO = table.Column<int>(type: "integer", nullable: false),
                    SEND_NO = table.Column<int>(type: "integer", nullable: false),
                    MESSAGE = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MALL_MESSAGE_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MALL_RENKEI_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SINSATU_NO = table.Column<int>(type: "integer", nullable: false),
                    KAIKEI_NO = table.Column<int>(type: "integer", nullable: false),
                    RECEIVE_NO = table.Column<int>(type: "integer", nullable: false),
                    SEND_NO = table.Column<int>(type: "integer", nullable: false),
                    SEND_FLG = table.Column<int>(type: "integer", nullable: false),
                    CLINIC_CD = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MALL_RENKEI_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    MATERIAL_CD = table.Column<long>(type: "bigint", nullable: false),
                    MATERIAL_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "MONSHIN_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: false),
                    RTEXT = table.Column<string>(type: "text", nullable: true),
                    GET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MONSHIN_INF", x => new { x.HP_ID, x.PT_ID, x.RAIIN_NO, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "ODR_DATE_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_DATE_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ODR_DATE_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_DATE_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ODR_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    ODR_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_SBT = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOSEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DAYS_CNT = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_INF", x => new { x.HP_ID, x.RAIIN_NO, x.RP_NO, x.RP_EDA_NO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "ODR_INF_CMT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    FONT_COLOR = table.Column<int>(type: "integer", nullable: false),
                    CMT_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_NAME = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_INF_CMT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ODR_INF_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEM_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    UNIT_SBT = table.Column<int>(type: "integer", nullable: false),
                    TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    KOHATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_LIMIT_KBN = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KBN = table.Column<int>(type: "integer", nullable: false),
                    YOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "text", nullable: true),
                    KOKUJI2 = table.Column<string>(type: "text", nullable: true),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IPN_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    IPN_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    JISSI_KBN = table.Column<int>(type: "integer", nullable: false),
                    JISSI_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JISSI_ID = table.Column<int>(type: "integer", nullable: false),
                    JISSI_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    REQ_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: true),
                    FONT_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    COMMENT_NEWLINE = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODR_INF_DETAIL", x => new { x.HP_ID, x.RAIIN_NO, x.RP_NO, x.RP_EDA_NO, x.ROW_NO });
                });

            migrationBuilder.CreateTable(
                name: "ONLINE_CONFIRMATION",
                columns: table => new
                {
                    RECEPTION_NO = table.Column<string>(type: "text", nullable: false),
                    RECEPTION_DATETIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    YOYAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEGMENT_OF_RESULT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ERROR_MESSAGE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONLINE_CONFIRMATION", x => x.RECEPTION_NO);
                });

            migrationBuilder.CreateTable(
                name: "ONLINE_CONFIRMATION_HISTORY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ONLINE_CONFIRMATION_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CONFIRMATION_TYPE = table.Column<int>(type: "integer", nullable: false),
                    CONFIRMATION_RESULT = table.Column<string>(type: "text", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONLINE_CONFIRMATION_HISTORY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ONLINE_CONSENT",
                columns: table => new
                {
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONS_KBN = table.Column<int>(type: "integer", nullable: false),
                    CONS_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LIMIT_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONLINE_CONSENT", x => new { x.PT_ID, x.CONS_KBN });
                });

            migrationBuilder.CreateTable(
                name: "PATH_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    GRP_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PATH = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CHAR_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATH_CONF", x => new { x.HP_ID, x.GRP_CD, x.GRP_EDA_NO, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PAYMENT_METHOD_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PAYMENT_METHOD_CD = table.Column<int>(type: "integer", nullable: false),
                    PAY_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    PAY_SNAME = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENT_METHOD_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PERMISSION_MST",
                columns: table => new
                {
                    FUNCTION_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    PERMISSION = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSION_MST", x => x.FUNCTION_CD);
                });

            migrationBuilder.CreateTable(
                name: "PHYSICAL_AVERAGE",
                columns: table => new
                {
                    JISSI_YEAR = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AGE_YEAR = table.Column<int>(type: "integer", nullable: false),
                    AGE_MONTH = table.Column<int>(type: "integer", nullable: false),
                    AGE_DAY = table.Column<int>(type: "integer", nullable: false),
                    MALE_HEIGHT = table.Column<double>(type: "double precision", nullable: false),
                    MALE_WEIGHT = table.Column<double>(type: "double precision", nullable: false),
                    MALE_CHEST = table.Column<double>(type: "double precision", nullable: false),
                    MALE_HEAD = table.Column<double>(type: "double precision", nullable: false),
                    FEMALE_HEIGHT = table.Column<double>(type: "double precision", nullable: false),
                    FEMALE_WEIGHT = table.Column<double>(type: "double precision", nullable: false),
                    FEMALE_CHEST = table.Column<double>(type: "double precision", nullable: false),
                    FEMALE_HEAD = table.Column<double>(type: "double precision", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHYSICAL_AVERAGE", x => x.JISSI_YEAR);
                });

            migrationBuilder.CreateTable(
                name: "PI_IMAGE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    IMAGE_TYPE = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_IMAGE", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PI_INF",
                columns: table => new
                {
                    PI_ID = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    W_DATE = table.Column<int>(type: "integer", nullable: false),
                    TITLE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    R_DATE = table.Column<int>(type: "integer", nullable: false),
                    REVISION = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    R_TYPE = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    R_REASON = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SCCJNO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    THERAPEUTICCLASSIFICATION = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PREPARATION_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HIGHLIGHT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FEATURE = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RELATEDMATTER = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    COMMONNAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GENERICNAME = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_INF", x => x.PI_ID);
                });

            migrationBuilder.CreateTable(
                name: "PI_INF_DETAIL",
                columns: table => new
                {
                    PI_ID = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    BRANCH = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    JPN = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    LEVEL = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_INF_DETAIL", x => new { x.PI_ID, x.BRANCH, x.JPN, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PI_PRODUCT_INF",
                columns: table => new
                {
                    PI_ID = table.Column<string>(type: "text", nullable: false),
                    BRANCH = table.Column<string>(type: "text", nullable: false),
                    JPN = table.Column<string>(type: "text", nullable: false),
                    PI_ID_FULL = table.Column<string>(type: "text", nullable: false),
                    PRODUCT_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    UNIT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MAKER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    VENDER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    MARKETER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OTHER = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    YJ_CD = table.Column<string>(type: "text", nullable: false),
                    HOT_CD = table.Column<string>(type: "text", nullable: true),
                    SOSYO_NAME = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    GENERIC_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    GENERIC_ENG_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    GENERAL_NO = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VER_DATE = table.Column<string>(type: "text", nullable: true),
                    YAKKA_REG = table.Column<string>(type: "text", nullable: true),
                    YAKKA_DEL = table.Column<string>(type: "text", nullable: true),
                    IS_STOPED = table.Column<string>(type: "text", nullable: true),
                    STOP_DATE = table.Column<string>(type: "text", nullable: true),
                    PI_STATE = table.Column<string>(type: "text", nullable: false),
                    PI_SBT = table.Column<string>(type: "text", nullable: false),
                    BIKO_PI_UNIT = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    BIKO_PI_BRANCH = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UPD_DATE_IMG = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPD_DATE_PI = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPD_DATE_PRODUCT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UPD_DATE_XML = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PI_PRODUCT_INF", x => new { x.PI_ID_FULL, x.PI_ID, x.BRANCH, x.JPN });
                });

            migrationBuilder.CreateTable(
                name: "POST_CODE_MST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    POST_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    PREF_KANA = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CITY_KANA = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    POSTAL_TERM_KANA = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PREF_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CITY_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    BANTI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POST_CODE_MST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRIORITY_HAIHAN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    HAIHAN_GRP = table.Column<long>(type: "bigint", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    USER_SETTING = table.Column<int>(type: "integer", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD6 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD7 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD8 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD9 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SP_JYOKEN = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TERM_CNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false),
                    TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRIORITY_HAIHAN_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_ALRGY_DRUG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DRUG_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_ALRGY_DRUG", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_ALRGY_ELSE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ALRGY_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_ALRGY_ELSE", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_ALRGY_FOOD",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ALRGY_KBN = table.Column<string>(type: "text", nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_ALRGY_FOOD", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_BYOMEI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
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
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    TENKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TENKI_DATE = table.Column<int>(type: "integer", nullable: false),
                    SYUBYO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKKAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    NANBYO_CD = table.Column<int>(type: "integer", nullable: false),
                    HOSOKU_CMT = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    TOGETU_BYOMEI = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    IS_IMPORTANT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_BYOMEI", x => new { x.HP_ID, x.PT_ID, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "PT_CMT_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_CMT_INF", x => new { x.ID, x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_FAMILY",
                columns: table => new
                {
                    FAMILY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZOKUGARA_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    PARENT_ID = table.Column<int>(type: "integer", nullable: false),
                    FAMILY_PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DEAD = table.Column<int>(type: "integer", nullable: false),
                    IS_SEPARATED = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_FAMILY", x => x.FAMILY_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_FAMILY_REKI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    FAMILY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_FAMILY_REKI", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_GRP_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRP_CODE = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_GRP_INF", x => new { x.HP_ID, x.GRP_ID, x.GRP_CODE, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_GRP_ITEM",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CODE = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRP_CODE_NAME = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_GRP_ITEM", x => new { x.HP_ID, x.GRP_ID, x.GRP_CODE, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_GRP_NAME_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_GRP_NAME_MST", x => new { x.HP_ID, x.GRP_ID });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_CHECK",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_GRP = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CHECK_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CHECK_ID = table.Column<int>(type: "integer", nullable: false),
                    CHECK_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    CHECK_CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_HOKEN_CHECK", x => new { x.HP_ID, x.PT_ID, x.HOKEN_GRP, x.HOKEN_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKENSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    HONKE_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    HOKENSYA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSYA_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    HOKENSYA_ADDRESS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HOKENSYA_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    KEIZOKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    KOFU_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TYPE = table.Column<int>(type: "integer", nullable: false),
                    TOKUREI_YM1 = table.Column<int>(type: "integer", nullable: false),
                    TOKUREI_YM2 = table.Column<int>(type: "integer", nullable: false),
                    TASUKAI_YM = table.Column<int>(type: "integer", nullable: false),
                    SYOKUMU_KBN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_RATE = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    TOKKI5 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAI_KOFU_NO = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    ROUSAI_SAIGAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_JIGYOSYO_NAME = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ROUSAI_PREF_NAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ROUSAI_CITY_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ROUSAI_SYOBYO_DATE = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_SYOBYO_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAI_ROUDOU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAI_KANTOKU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ROUSAI_RECE_COUNT = table.Column<int>(type: "integer", nullable: false),
                    RYOYO_START_DATE = table.Column<int>(type: "integer", nullable: false),
                    RYOYO_END_DATE = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HOKEN_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JIBAI_HOKEN_TANTO = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    JIBAI_HOKEN_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    JIBAI_JYUSYOU_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_HOKEN_INF", x => new { x.HP_ID, x.PT_ID, x.HOKEN_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_PATTERN",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_SBT_CD = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_MEMO = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_HOKEN_PATTERN", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO, x.HOKEN_PID });
                });

            migrationBuilder.CreateTable(
                name: "PT_HOKEN_SCAN",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_GRP = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_HOKEN_SCAN", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_NUM = table.Column<long>(type: "bigint", nullable: false),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DEAD = table.Column<int>(type: "integer", nullable: false),
                    DEATH_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOME_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    HOME_ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HOME_ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TEL1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    TEL2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    MAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SETAINUSI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZOKUGARA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    JOB = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    RENRAKU_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKU_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    RENRAKU_ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKU_ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RENRAKU_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    RENRAKU_MEMO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICE_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    OFFICE_ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICE_ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OFFICE_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    OFFICE_MEMO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_RYOSYO_DETAIL = table.Column<int>(type: "integer", nullable: false),
                    PRIMARY_DOCTOR = table.Column<int>(type: "integer", nullable: false),
                    IS_TESTER = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    MAIN_HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    REFERENCE_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LIMIT_CONS_FLG = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_INF", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_INFECTION",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAI_CD = table.Column<string>(type: "text", nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_INFECTION", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_JIBAI_DOC",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    SINDAN_COST = table.Column<int>(type: "integer", nullable: false),
                    SINDAN_NUM = table.Column<int>(type: "integer", nullable: false),
                    MEISAI_COST = table.Column<int>(type: "integer", nullable: false),
                    MEISAI_NUM = table.Column<int>(type: "integer", nullable: false),
                    ELSE_COST = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_JIBAI_DOC", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_JIBKAR",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WEB_ID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ODR_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    ODR_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KARTE_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    KARTE_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KENSA_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    KENSA_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BYOMEI_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_JIBKAR", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_KIO_REKI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    BYOTAI_CD = table.Column<string>(type: "text", nullable: true),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_KIO_REKI", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_KOHI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    FUTANSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    JYUKYUSYA_NO = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    HOKEN_SBT_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    TOKUSYU_NO = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SIKAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    KOFU_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_KOHI", x => new { x.HP_ID, x.PT_ID, x.HOKEN_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_KYUSEI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_KYUSEI", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_LAST_VISIT_DATE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LAST_VISIT_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_LAST_VISIT_DATE", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_MEMO",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_MEMO", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_OTC_DRUG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    SERIAL_NUM = table.Column<int>(type: "integer", nullable: false),
                    TRADE_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_OTC_DRUG", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_OTHER_DRUG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DRUG_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_OTHER_DRUG", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_PREGNANCY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    PERIOD_DATE = table.Column<int>(type: "integer", nullable: false),
                    PERIOD_DUE_DATE = table.Column<int>(type: "integer", nullable: false),
                    OVULATION_DATE = table.Column<int>(type: "integer", nullable: false),
                    OVULATION_DUE_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_PREGNANCY", x => new { x.ID, x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_ROUSAI_TENKI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    SINKEI = table.Column<int>(type: "integer", nullable: false),
                    TENKI = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_ROUSAI_TENKI", x => new { x.HP_ID, x.PT_ID, x.HOKEN_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_SANTEI_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    KBN_VAL = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_SANTEI_CONF", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_SUPPLE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    INDEX_CD = table.Column<string>(type: "text", nullable: true),
                    INDEX_WORD = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_SUPPLE", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "PT_TAG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: false),
                    MEMO_DATA = table.Column<byte[]>(type: "bytea", nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_UKETUKE = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_KAIKEI = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    BACKGROUND_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    TAG_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    ALPHABLEND_VAL = table.Column<int>(type: "integer", nullable: false),
                    FONTSIZE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    WIDTH = table.Column<int>(type: "integer", nullable: false),
                    HEIGHT = table.Column<int>(type: "integer", nullable: false),
                    LEFT = table.Column<int>(type: "integer", nullable: false),
                    TOP = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_TAG", x => new { x.HP_ID, x.PT_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_CMT_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_CMT_INF", x => new { x.HP_ID, x.RAIIN_NO, x.CMT_KBN, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_KBN",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    FILTER_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_FILTER_KBN", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_MST",
                columns: table => new
                {
                    FILTER_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    FILTER_NAME = table.Column<string>(type: "text", nullable: true),
                    SELECT_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    SHORTCUT = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_FILTER_MST", x => x.FILTER_ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_SORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    FILTER_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PRIORITY = table.Column<int>(type: "integer", nullable: false),
                    COLUMN_NAME = table.Column<string>(type: "text", nullable: true),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SORT_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_FILTER_SORT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_FILTER_STATE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    FILTER_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_FILTER_STATE", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    OYA_RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    IS_YOYAKU = table.Column<int>(type: "integer", nullable: false),
                    YOYAKU_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    YOYAKU_ID = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    UKETUKE_ID = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_NO = table.Column<int>(type: "integer", nullable: false),
                    SIN_START_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    SIN_END_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    KAIKEI_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    KAIKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    TANTO_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOSAISIN_KBN = table.Column<int>(type: "integer", nullable: false),
                    JIKAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    CONFIRMATION_RESULT = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CONFIRMATION_STATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_INF", x => new { x.HP_ID, x.RAIIN_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    KBN_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    COLOR_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    IS_CONFIRMED = table.Column<int>(type: "integer", nullable: false),
                    IS_AUTO = table.Column<int>(type: "integer", nullable: false),
                    IS_AUTO_DELETE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_DETAIL", x => new { x.HP_ID, x.GRP_ID, x.KBN_CD });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_INF", x => new { x.HP_ID, x.PT_ID, x.RAIIN_NO, x.GRP_ID, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_ITEM",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IS_EXCLUDE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_ITEM", x => new { x.HP_ID, x.GRP_ID, x.KBN_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_KOUI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KOUI_KBN_ID = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_KOUI", x => new { x.HP_ID, x.GRP_ID, x.KBN_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_MST", x => new { x.HP_ID, x.GRP_ID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_KBN_YOYAKU",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YOYAKU_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_KBN_YOYAKU", x => new { x.HP_ID, x.GRP_ID, x.KBN_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_CMT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_CMT", x => new { x.HP_ID, x.RAIIN_NO, x.CMT_KBN });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    KBN_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    COLOR_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_DETAIL", x => new { x.HP_ID, x.GRP_ID, x.KBN_CD });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_DOC",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_DOC", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_FILE",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_FILE", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_LIST_KBN = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_INF", x => new { x.HP_ID, x.PT_ID, x.SIN_DATE, x.RAIIN_NO, x.GRP_ID, x.RAIIN_LIST_KBN });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_ITEM",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IS_EXCLUDE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_ITEM", x => new { x.HP_ID, x.KBN_CD, x.SEQ_NO, x.GRP_ID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_KOUI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KOUI_KBN_ID = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_KOUI", x => new { x.HP_ID, x.KBN_CD, x.SEQ_NO, x.GRP_ID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_MST", x => new { x.HP_ID, x.GRP_ID });
                });

            migrationBuilder.CreateTable(
                name: "RAIIN_LIST_TAG",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    TAG_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAIIN_LIST_TAG", x => new { x.HP_ID, x.RAIIN_NO, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RECE_CHECK_CMT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_PENDING = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IS_CHECKED = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_CHECK_CMT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_CHECK_ERR",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    ERR_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    A_CD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    B_CD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MESSAGE_1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MESSAGE_2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_CHECKED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_CHECK_ERR", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_CHECK_OPT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ERR_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CHECK_OPT = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_CHECK_OPT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_CMT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_SBT = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT = table.Column<string>(type: "text", nullable: false),
                    CMT_DATA = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_CMT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_FUTAN_KBN",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    FUTAN_KBN_CD = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_FUTAN_KBN", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID2 = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_SBT_CD = table.Column<int>(type: "integer", nullable: false),
                    RECE_SBT = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOKENSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI2_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI3_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI4_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HONKE_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TEKIYO_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_TOKUREI = table.Column<int>(type: "integer", nullable: false),
                    IS_TASUKAI = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KOHI1_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KOHI2_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KOHI3_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KOHI4_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_KOGAKU_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_RATE = table.Column<int>(type: "integer", nullable: false),
                    PT_RATE = table.Column<int>(type: "integer", nullable: false),
                    EN_TEN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_LIMIT = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_OTHER_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    TENSU = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_IRYOHI = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    ICHIBU_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_GAKU_10EN = table.Column<int>(type: "integer", nullable: false),
                    PT_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_OVER_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_TENSU = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ICHIBU_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_TENSU = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ICHIBU_SOTOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ICHIBU_SOTOGAKU_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_TENSU = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ICHIBU_SOTOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ICHIBU_SOTOGAKU_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_TENSU = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ICHIBU_SOTOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ICHIBU_SOTOGAKU_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_TENSU = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ICHIBU_SOTOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ICHIBU_SOTOGAKU_10EN = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_ICHIBU_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_ICHIBU_FUTAN_10EN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_KISAI = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_RECE_KISAI = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_RECE_KISAI = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_RECE_KISAI = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_NAME_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    KOHI2_NAME_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    KOHI3_NAME_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    KOHI4_NAME_CD = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SEIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PT_STATUS = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ROUSAI_I_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_RO_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_I_TENSU = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_RO_TENSU = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HA_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_NI_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HO_SINDAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HO_SINDAN_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HE_MEISAI = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HE_MEISAI_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_A_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_B_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_C_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_D_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_KENPO_TENSU = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_KENPO_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    SINKEI = table.Column<int>(type: "integer", nullable: false),
                    TENKI = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    TANTO_ID = table.Column<int>(type: "integer", nullable: false),
                    IS_TESTER = table.Column<int>(type: "integer", nullable: false),
                    IS_ZAIISO = table.Column<int>(type: "integer", nullable: false),
                    CHOKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF_EDIT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RECE_SBT = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI2_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI3_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI4_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HOKEN_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_NISSU = table.Column<int>(type: "integer", nullable: true),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF_EDIT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF_JD",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI_ID = table.Column<int>(type: "integer", nullable: false),
                    FUTAN_SBT_CD = table.Column<int>(type: "integer", nullable: false),
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
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF_JD", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_INF_PRE_EDIT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    RECE_SBT = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI2_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI3_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI4_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HOKEN_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_NISSU = table.Column<int>(type: "integer", nullable: true),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_INF_PRE_EDIT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_SEIKYU",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    PRE_HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_SEIKYU", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECE_STATUS",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    FUSEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_PRECHECKED = table.Column<int>(type: "integer", nullable: false),
                    OUTPUT = table.Column<int>(type: "integer", nullable: false),
                    STATUS_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECE_STATUS", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECEDEN_CMT_SELECT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    COMMENT_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    KBN_NO = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PT_STATUS = table.Column<int>(type: "integer", nullable: false),
                    COND_KBN = table.Column<int>(type: "integer", nullable: false),
                    NOT_SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    NYUGAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_CNT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECEDEN_CMT_SELECT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECEDEN_HEN_JIYUU",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HENREI_JIYUU_CD = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    HENREI_JIYUU = table.Column<string>(type: "text", nullable: false),
                    HOSOKU = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECEDEN_HEN_JIYUU", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RECEDEN_RIREKI_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEARCH_NO = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RIREKI = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECEDEN_RIREKI_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RELEASENOTE_READ",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    VERSION = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RELEASENOTE_READ", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PT_NUM_LENGTH = table.Column<int>(type: "integer", nullable: false),
                    TEMPLATE_ID = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_CONF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RENKEI_SBT = table.Column<int>(type: "integer", nullable: false),
                    FUNCTION_TYPE = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_PATH_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PATH = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CHAR_CD = table.Column<int>(type: "integer", nullable: false),
                    WORK_PATH = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    INTERVAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    USER = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_PATH_CONF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_REQ",
                columns: table => new
                {
                    REQ_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    REQ_SBT = table.Column<int>(type: "integer", nullable: false),
                    REQ_TYPE = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    ERR_MST = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_REQ", x => x.REQ_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_TEMPLATE_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TEMPLATE_ID = table.Column<int>(type: "integer", nullable: false),
                    TEMPLATE_NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PARAM = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FILE = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_TEMPLATE_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_TIMING_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    EVENT_CD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_TIMING_CONF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RENKEI_TIMING_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    EVENT_CD = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RENKEI_TIMING_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ROUDOU_MST",
                columns: table => new
                {
                    ROUDOU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ROUDOU_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUDOU_MST", x => x.ROUDOU_CD);
                });

            migrationBuilder.CreateTable(
                name: "ROUSAI_GOSEI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GOSEI_GRP = table.Column<int>(type: "integer", nullable: false),
                    GOSEI_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SISI_KBN = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUSAI_GOSEI_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_DAY_COMMENT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_DAY_COMMENT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_DAY_PTN",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    END_TIME = table.Column<int>(type: "integer", nullable: false),
                    MINUTES = table.Column<int>(type: "integer", nullable: false),
                    NUMBER = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    IS_HOLIDAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_DAY_PTN", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_INF",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    END_TIME = table.Column<int>(type: "integer", nullable: false),
                    FRAME_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_HOLIDAY = table.Column<int>(type: "integer", nullable: false),
                    NUMBER = table.Column<long>(type: "bigint", nullable: false),
                    FRAME_SBT = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RSV_GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    TANTO_ID = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    MAKE_RAIIN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_MST", x => new { x.HP_ID, x.RSV_FRAME_ID });
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_WEEK_PTN",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    WEEK = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    END_TIME = table.Column<int>(type: "integer", nullable: false),
                    MINUTES = table.Column<int>(type: "integer", nullable: false),
                    NUMBER = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    IS_HOLIDAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_WEEK_PTN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_FRAME_WITH",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    WITH_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_FRAME_WITH", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_GRP_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_GRP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_KEY = table.Column<int>(type: "integer", nullable: false),
                    RSV_GRP_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_GRP_MST", x => new { x.HP_ID, x.RSV_GRP_ID });
                });

            migrationBuilder.CreateTable(
                name: "RSV_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSV_SBT = table.Column<int>(type: "integer", nullable: false),
                    TANTO_ID = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_INF", x => new { x.HP_ID, x.RSV_FRAME_ID, x.SIN_DATE, x.START_TIME, x.RAIIN_NO });
                });

            migrationBuilder.CreateTable(
                name: "RSV_RENKEI_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    OTHER_SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    OTHER_SEQ_NO2 = table.Column<long>(type: "bigint", nullable: false),
                    OTHER_PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_RENKEI_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSV_RENKEI_INF_TK",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    OTHER_SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    OTHER_SEQ_NO2 = table.Column<long>(type: "bigint", nullable: false),
                    OTHER_PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSV_RENKEI_INF_TK", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_BYOMEI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
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
                    SYUBYO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKKAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    NANBYO_CD = table.Column<int>(type: "integer", nullable: false),
                    HOSOKU_CMT = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_BYOMEI", x => new { x.HP_ID, x.PT_ID, x.RSVKRT_NO, x.SEQ_NO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_KARTE_IMG_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    POSITION = table.Column<long>(type: "bigint", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MESSAGE = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_KARTE_IMG_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_KARTE_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    RSV_DATE = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RICH_TEXT = table.Column<byte[]>(type: "bytea", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_KARTE_INF", x => new { x.HP_ID, x.PT_ID, x.RSVKRT_NO, x.KARTE_KBN, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RSVKRT_KBN = table.Column<int>(type: "integer", nullable: false),
                    RSV_DATE = table.Column<int>(type: "integer", nullable: false),
                    RSV_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_ODR_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RSV_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    ODR_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_SBT = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOSEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DAYS_CNT = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_ODR_INF", x => new { x.HP_ID, x.PT_ID, x.RSVKRT_NO, x.RP_NO, x.RP_EDA_NO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_ODR_INF_CMT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    RSV_DATE = table.Column<int>(type: "integer", nullable: false),
                    FONT_COLOR = table.Column<int>(type: "integer", nullable: false),
                    CMT_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_NAME = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_ODR_INF_CMT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RSVKRT_ODR_INF_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSVKRT_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    RSV_DATE = table.Column<int>(type: "integer", nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEM_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    UNIT_SBT = table.Column<int>(type: "integer", nullable: false),
                    TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    KOHATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_LIMIT_KBN = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KBN = table.Column<int>(type: "integer", nullable: false),
                    YOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IPN_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    IPN_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: true),
                    FONT_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    COMMENT_NEWLINE = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSVKRT_ODR_INF_DETAIL", x => new { x.HP_ID, x.PT_ID, x.RSVKRT_NO, x.RP_NO, x.RP_EDA_NO, x.ROW_NO });
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_AUTO_ORDER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    ADD_TYPE = table.Column<int>(type: "integer", nullable: false),
                    ADD_TARGET = table.Column<int>(type: "integer", nullable: false),
                    TERM_CNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false),
                    CNT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    MAX_CNT = table.Column<long>(type: "bigint", nullable: false),
                    SP_CONDITION = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_AUTO_ORDER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_AUTO_ORDER_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_AUTO_ORDER_DETAIL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_CNT_CHECK",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    TERM_CNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false),
                    CNT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    MAX_CNT = table.Column<long>(type: "bigint", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ERR_KBN = table.Column<int>(type: "integer", nullable: false),
                    TARGET_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SP_CONDITION = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_CNT_CHECK", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_GRP_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "text", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_GRP_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_GRP_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_GRP_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_GRP_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SANTEI_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ALERT_DAYS = table.Column<int>(type: "integer", nullable: false),
                    ALERT_TERM = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
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
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    KISAN_SBT = table.Column<int>(type: "integer", nullable: false),
                    KISAN_DATE = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    HOSOKU_COMMENT = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SANTEI_INF_DETAIL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SCHEMA_CMT_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    COMMENT_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    COMMENT = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHEMA_CMT_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SEIKATUREKI_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEIKATUREKI_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SENTENCE_LIST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SENTENCE_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SET_KBN = table.Column<int>(type: "integer", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    LEVEL1 = table.Column<long>(type: "bigint", nullable: false),
                    LEVEL2 = table.Column<long>(type: "bigint", nullable: false),
                    LEVEL3 = table.Column<long>(type: "bigint", nullable: false),
                    SENTENCE = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    SELECT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    NEW_LINE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENTENCE_LIST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SESSION_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    MACHINE = table.Column<string>(type: "text", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    LOGIN_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SET_BYOMEI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
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
                    SYUBYO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKKAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    NANBYO_CD = table.Column<int>(type: "integer", nullable: false),
                    HOSOKU_CMT = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_BYOMEI", x => new { x.ID, x.HP_ID, x.SET_CD, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "SET_GENERATION_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_GENERATION_MST", x => new { x.HP_ID, x.GENERATION_ID });
                });

            migrationBuilder.CreateTable(
                name: "SET_KARTE_IMG_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_CD = table.Column<int>(type: "integer", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    POSITION = table.Column<long>(type: "bigint", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_KARTE_IMG_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SET_KARTE_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_CD = table.Column<int>(type: "integer", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RICH_TEXT = table.Column<byte[]>(type: "bytea", nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_KARTE_INF", x => new { x.HP_ID, x.SET_CD, x.KARTE_KBN, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "SET_KBN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_KBN = table.Column<int>(type: "integer", nullable: false),
                    SET_KBN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_KBN_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    KA_CD = table.Column<int>(type: "integer", nullable: false),
                    DOC_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_KBN_MST", x => new { x.HP_ID, x.SET_KBN, x.SET_KBN_EDA_NO, x.GENERATION_ID });
                });

            migrationBuilder.CreateTable(
                name: "SET_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SET_KBN = table.Column<int>(type: "integer", nullable: false),
                    SET_KBN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    GENERATION_ID = table.Column<int>(type: "integer", nullable: false),
                    LEVEL1 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL2 = table.Column<int>(type: "integer", nullable: false),
                    LEVEL3 = table.Column<int>(type: "integer", nullable: false),
                    SET_NAME = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    WEIGHT_KBN = table.Column<int>(type: "integer", nullable: false),
                    COLOR = table.Column<int>(type: "integer", nullable: false),
                    IS_GROUP = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_MST", x => new { x.HP_ID, x.SET_CD });
                });

            migrationBuilder.CreateTable(
                name: "SET_ODR_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_CD = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ODR_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_SBT = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOSEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DAYS_CNT = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_ODR_INF", x => new { x.HP_ID, x.SET_CD, x.RP_NO, x.RP_EDA_NO, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "SET_ODR_INF_CMT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SET_CD = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    FONT_COLOR = table.Column<int>(type: "integer", nullable: false),
                    CMT_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_NAME = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_ODR_INF_CMT", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SET_ODR_INF_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SET_CD = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEM_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    UNIT_SBT = table.Column<int>(type: "integer", nullable: false),
                    ODR_TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    KOHATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_LIMIT_KBN = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KBN = table.Column<int>(type: "integer", nullable: false),
                    YOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IPN_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    IPN_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: true),
                    FONT_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    COMMENT_NEWLINE = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_ODR_INF_DETAIL", x => new { x.HP_ID, x.SET_CD, x.RP_NO, x.RP_EDA_NO, x.ROW_NO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_KOUI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SYUKEI_SAKI = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOKATU_KENSA = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_TEN = table.Column<double>(type: "double precision", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    ZEI = table.Column<double>(type: "double precision", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    TEN_COUNT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TEN_COL_COUNT = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    ENTEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    CD_KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    REC_ID = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    JIHI_SBT = table.Column<int>(type: "integer", nullable: false),
                    KAZEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DETAIL_DATA = table.Column<string>(type: "text", nullable: false),
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
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_KOUI", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SIN_KOUI_COUNT",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SIN_DAY = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_KOUI_COUNT", x => new { x.HP_ID, x.PT_ID, x.SIN_YM, x.SIN_DAY, x.RAIIN_NO, x.RP_NO, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_KOUI_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    REC_ID = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    ITEM_SBT = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ODR_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ITEM_NAME = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    SURYO2 = table.Column<double>(type: "double precision", nullable: false),
                    FMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    UNIT_CD = table.Column<int>(type: "integer", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    ZEI = table.Column<double>(type: "double precision", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RYOSYU = table.Column<int>(type: "integer", nullable: false),
                    CMT_OPT = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT1 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMT_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT_OPT1 = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT2 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMT_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT_OPT2 = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    CMT3 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CMT_CD3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CMT_OPT3 = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_KOUI_DETAIL", x => new { x.HP_ID, x.PT_ID, x.SIN_YM, x.RP_NO, x.SEQ_NO, x.ROW_NO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_RP_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FIRST_DAY = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIN_ID = table.Column<int>(type: "integer", nullable: false),
                    CD_NO = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOUI_DATA = table.Column<string>(type: "text", nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_RP_INF", x => new { x.HP_ID, x.PT_ID, x.SIN_YM, x.RP_NO });
                });

            migrationBuilder.CreateTable(
                name: "SIN_RP_NO_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SIN_DAY = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIN_RP_NO_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SINGLE_DOSE_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UNIT_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINGLE_DOSE_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SINREKI_FILTER_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINREKI_FILTER_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SINREKI_FILTER_MST_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINREKI_FILTER_MST_DETAIL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SOKATU_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    START_YM = table.Column<int>(type: "integer", nullable: false),
                    REPORT_ID = table.Column<int>(type: "integer", nullable: false),
                    REPORT_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    END_YM = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    REPORT_NAME = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PRINT_TYPE = table.Column<int>(type: "integer", nullable: false),
                    PRINT_NO_TYPE = table.Column<int>(type: "integer", nullable: false),
                    DATA_ALL = table.Column<int>(type: "integer", nullable: false),
                    DATA_DISK = table.Column<int>(type: "integer", nullable: false),
                    DATA_PAPER = table.Column<int>(type: "integer", nullable: false),
                    DATA_KBN = table.Column<int>(type: "integer", nullable: false),
                    DISK_KIND = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DISK_CNT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    IS_SORT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOKATU_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "STA_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    MENU_ID = table.Column<int>(type: "integer", nullable: false),
                    CONF_ID = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<string>(type: "character varying(1200)", maxLength: 1200, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_CONF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "STA_CSV",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    REPORT_ID = table.Column<int>(type: "integer", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    CONF_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DATA_SBT = table.Column<int>(type: "integer", nullable: false),
                    COLUMNS = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SORT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_CSV", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "STA_GRP",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    REPORT_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_GRP", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "STA_MENU",
                columns: table => new
                {
                    MENU_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    REPORT_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    MENU_NAME = table.Column<string>(type: "character varying(130)", maxLength: 130, nullable: false),
                    IS_PRINT = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_MENU", x => x.MENU_ID);
                });

            migrationBuilder.CreateTable(
                name: "STA_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    REPORT_ID = table.Column<int>(type: "integer", nullable: false),
                    REPORT_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STA_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SUMMARY_INF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: true),
                    RTEXT = table.Column<byte[]>(type: "bytea", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUMMARY_INF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYOBYO_KEIKA",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SIN_DAY = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KEIKA = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYOBYO_KEIKA", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SYOUKI_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    SYOUKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOUKI = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYOUKI_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SYOUKI_KBN_MST",
                columns: table => new
                {
                    SYOUKI_KBN = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_YM = table.Column<int>(type: "integer", nullable: false),
                    END_YM = table.Column<int>(type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYOUKI_KBN_MST", x => x.SYOUKI_KBN);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CHANGE_LOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILE_NAME = table.Column<string>(type: "text", nullable: false),
                    VERSION = table.Column<string>(type: "text", nullable: false),
                    IS_PG = table.Column<int>(type: "integer", nullable: false),
                    IS_DB = table.Column<int>(type: "integer", nullable: false),
                    IS_MASTER = table.Column<int>(type: "integer", nullable: false),
                    IS_RUN = table.Column<int>(type: "integer", nullable: false),
                    IS_NOTE = table.Column<int>(type: "integer", nullable: false),
                    IS_DRUG_PHOTO = table.Column<int>(type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    ERR_MESSAGE = table.Column<string>(type: "text", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CHANGE_LOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    GRP_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<double>(type: "double precision", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CONF", x => new { x.HP_ID, x.GRP_CD, x.GRP_EDA_NO });
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CONF_ITEM",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    MENU_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM_MIN = table.Column<int>(type: "integer", nullable: false),
                    PARAM_MAX = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CONF_ITEM", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_CONF_MENU",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    MENU_ID = table.Column<int>(type: "integer", nullable: false),
                    MENU_GRP = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    MENU_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    GRP_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    PATH_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_PARAM = table.Column<int>(type: "integer", nullable: false),
                    PARAM_MASK = table.Column<int>(type: "integer", nullable: false),
                    PARAM_TYPE = table.Column<int>(type: "integer", nullable: false),
                    PARAM_HINT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VAL_MIN = table.Column<double>(type: "double precision", nullable: false),
                    VAL_MAX = table.Column<double>(type: "double precision", nullable: false),
                    PARAM_MIN = table.Column<double>(type: "double precision", nullable: false),
                    PARAM_MAX = table.Column<double>(type: "double precision", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_VISIBLE = table.Column<int>(type: "integer", nullable: false),
                    MANAGER_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_VALUE = table.Column<int>(type: "integer", nullable: false),
                    PARAM_MAX_LENGTH = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_CONF_MENU", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_GENERATION_CONF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    GRP_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_GENERATION_CONF", x => new { x.HP_ID, x.GRP_EDA_NO, x.GRP_CD, x.ID });
                });

            migrationBuilder.CreateTable(
                name: "SYUNO_NYUKIN",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    NYUKIN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    PAYMENT_METHOD_CD = table.Column<int>(type: "integer", nullable: false),
                    NYUKIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    NYUKIN_CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NYUKINJI_TENSU = table.Column<int>(type: "integer", nullable: false),
                    NYUKINJI_SEIKYU = table.Column<int>(type: "integer", nullable: false),
                    NYUKINJI_DETAIL = table.Column<string>(type: "text", nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYUNO_NYUKIN", x => new { x.HP_ID, x.RAIIN_NO, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "SYUNO_SEIKYU",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    NYUKIN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_TENSU = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_GAKU = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_DETAIL = table.Column<string>(type: "text", nullable: true),
                    NEW_SEIKYU_TENSU = table.Column<int>(type: "integer", nullable: false),
                    NEW_ADJUST_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    NEW_SEIKYU_GAKU = table.Column<int>(type: "integer", nullable: false),
                    NEW_SEIKYU_DETAIL = table.Column<string>(type: "text", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYUNO_SEIKYU", x => new { x.HP_ID, x.RAIIN_NO, x.PT_ID, x.SIN_DATE });
                });

            migrationBuilder.CreateTable(
                name: "TAG_GRP_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TAG_GRP_NO = table.Column<int>(type: "integer", nullable: false),
                    TAG_GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GRP_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAG_GRP_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEKIOU_BYOMEI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    SYSTEM_DATA = table.Column<int>(type: "integer", nullable: false),
                    START_YM = table.Column<int>(type: "integer", nullable: false),
                    END_YM = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID = table.Column<int>(type: "integer", nullable: false),
                    IS_INVALID_TOKUSYO = table.Column<int>(type: "integer", nullable: false),
                    EDIT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEKIOU_BYOMEI_MST", x => new { x.HP_ID, x.ITEM_CD, x.BYOMEI_CD, x.SYSTEM_DATA });
                });

            migrationBuilder.CreateTable(
                name: "TEKIOU_BYOMEI_MST_EXCLUDED",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEKIOU_BYOMEI_MST_EXCLUDED", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TEMPLATE_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CONTROL_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    OYA_CONTROL_ID = table.Column<int>(type: "integer", nullable: true),
                    TITLE = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CONTROL_TYPE = table.Column<int>(type: "integer", nullable: false),
                    MENU_KBN = table.Column<int>(type: "integer", nullable: false),
                    DEFAULT_VAL = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UNIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NEW_LINE = table.Column<int>(type: "integer", nullable: false),
                    KARTE_KBN = table.Column<int>(type: "integer", nullable: false),
                    CONTROL_WIDTH = table.Column<int>(type: "integer", nullable: false),
                    TITLE_WIDTH = table.Column<int>(type: "integer", nullable: false),
                    UNIT_WIDTH = table.Column<int>(type: "integer", nullable: false),
                    LEFT_MARGIN = table.Column<int>(type: "integer", nullable: false),
                    WORDWRAP = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<double>(type: "double precision", nullable: true),
                    FORMULA = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DECIMAL = table.Column<int>(type: "integer", nullable: false),
                    IME = table.Column<int>(type: "integer", nullable: false),
                    COL_COUNT = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_CD = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BACKGROUND_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    FONT_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    FONT_BOLD = table.Column<int>(type: "integer", nullable: false),
                    FONT_ITALIC = table.Column<int>(type: "integer", nullable: false),
                    FONT_UNDER_LINE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_DSP_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TEMPLATE_CD = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DSP_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_DSP_CONF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_MENU_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MENU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEM_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<double>(type: "double precision", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_MENU_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_MENU_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MENU_KBN = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBN_NAME = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_MENU_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEMPLATE_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TEMPLATE_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    INSERTION_DESTINATION = table.Column<int>(type: "integer", nullable: false),
                    TITLE = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEMPLATE_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    MASTER_SBT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    KANA_NAME1 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KANA_NAME2 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KANA_NAME3 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KANA_NAME4 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KANA_NAME5 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KANA_NAME6 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    KANA_NAME7 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    RYOSYU_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    RECE_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    TEN_ID = table.Column<int>(type: "integer", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    RECE_UNIT_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    RECE_UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    ODR_UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    CNV_UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    ODR_TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    CNV_TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    DEFAULT_VAL = table.Column<double>(type: "double precision", nullable: false),
                    IS_ADOPTED = table.Column<int>(type: "integer", nullable: false),
                    KOUKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKATU_KENSA = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    IGAKUKANRI = table.Column<int>(type: "integer", nullable: false),
                    JITUDAY_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JITUDAY = table.Column<int>(type: "integer", nullable: false),
                    DAY_COUNT = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KANREN_KBN = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_ID = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_MIN = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_MAX = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_VAL = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_TEN = table.Column<double>(type: "double precision", nullable: false),
                    KIZAMI_ERR = table.Column<int>(type: "integer", nullable: false),
                    MAX_COUNT = table.Column<int>(type: "integer", nullable: false),
                    MAX_COUNT_ERR = table.Column<int>(type: "integer", nullable: false),
                    TYU_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    TYU_SEQ = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    TUSOKU_AGE = table.Column<int>(type: "integer", nullable: false),
                    MIN_AGE = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    MAX_AGE = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGE_CHECK = table.Column<int>(type: "integer", nullable: false),
                    TIME_KASAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUTEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUTEKI_SISETU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOTI_NYUYOJI_KBN = table.Column<int>(type: "integer", nullable: false),
                    LOW_WEIGHT_KBN = table.Column<int>(type: "integer", nullable: false),
                    HANDAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HANDAN_GRP_KBN = table.Column<int>(type: "integer", nullable: false),
                    TEIGEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEKITUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    KEIBU_KBN = table.Column<int>(type: "integer", nullable: false),
                    AUTO_HOUGOU_KBN = table.Column<int>(type: "integer", nullable: false),
                    GAIRAI_KANRI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TUSOKU_TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    TYOONPA_NAISI_KBN = table.Column<int>(type: "integer", nullable: false),
                    AUTO_FUNGO_KBN = table.Column<int>(type: "integer", nullable: false),
                    TYOONPA_GYOKO_KBN = table.Column<int>(type: "integer", nullable: false),
                    GAZO_KASAN = table.Column<int>(type: "integer", nullable: false),
                    KANSATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    MASUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUKUBIKU_NAISI_KASAN = table.Column<int>(type: "integer", nullable: false),
                    FUKUBIKU_KOTUNAN_KASAN = table.Column<int>(type: "integer", nullable: false),
                    MASUI_KASAN = table.Column<int>(type: "integer", nullable: false),
                    MONITER_KASAN = table.Column<int>(type: "integer", nullable: false),
                    TOKETU_KASAN = table.Column<int>(type: "integer", nullable: false),
                    TEN_KBN_NO = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    SHORTSTAY_OPE = table.Column<int>(type: "integer", nullable: false),
                    BUI_KBN = table.Column<int>(type: "integer", nullable: false),
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
                    AGEKASAN_MIN1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_MAX1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASAN_MIN2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_MAX2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASAN_MIN3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_MAX3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_CD3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    AGEKASAN_MIN4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_MAX4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AGEKASAN_CD4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    KENSA_CMT = table.Column<int>(type: "integer", nullable: false),
                    MADOKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SINKEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEIBUTU_KBN = table.Column<int>(type: "integer", nullable: false),
                    ZOUEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KBN = table.Column<int>(type: "integer", nullable: false),
                    ZAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    CAPACITY = table.Column<int>(type: "integer", nullable: false),
                    KOHATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOKUZAI_AGE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SANSO_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOKUZAI_SBT = table.Column<int>(type: "integer", nullable: false),
                    MAX_PRICE = table.Column<int>(type: "integer", nullable: false),
                    MAX_TEN = table.Column<int>(type: "integer", nullable: false),
                    SYUKEI_SAKI = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    CD_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    CD_SYO = table.Column<int>(type: "integer", nullable: false),
                    CD_BU = table.Column<int>(type: "integer", nullable: false),
                    CD_KBNNO = table.Column<int>(type: "integer", nullable: false),
                    CD_EDANO = table.Column<int>(type: "integer", nullable: false),
                    CD_KOUNO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI_SYO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_BU = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KOU_NO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    KOHYO_JUN = table.Column<int>(type: "integer", nullable: false),
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    YAKKA_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    SYUSAI_SBT = table.Column<int>(type: "integer", nullable: false),
                    SYOHIN_KANREN = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    UPD_DATE = table.Column<int>(type: "integer", nullable: false),
                    DEL_DATE = table.Column<int>(type: "integer", nullable: false),
                    KEIKA_DATE = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_BETUNO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KBNNO = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SISI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SHOT_CNT = table.Column<int>(type: "integer", nullable: false),
                    IS_NOSEARCH = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RYOSYU = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    JIHI_SBT = table.Column<int>(type: "integer", nullable: false),
                    KAZEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    YOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    FUKUYO_RISE = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_MORNING = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_DAYTIME = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_NIGHT = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_SLEEP = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_YAKUTAI = table.Column<int>(type: "integer", nullable: false),
                    ZAIKEI_POINT = table.Column<double>(type: "double precision", nullable: false),
                    SURYO_ROUNDUP_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOUSEISIN_KBN = table.Column<int>(type: "integer", nullable: false),
                    CHUSYA_DRUG_SBT = table.Column<int>(type: "integer", nullable: false),
                    KENSA_FUKUSU_SANTEI = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SANTEIGAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    KENSA_ITEM_CD = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    KENSA_ITEM_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_CD1 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RENKEI_CD2 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SAIKETU_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL1 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA1 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL2 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA2 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL3 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA3 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL4 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA4 = table.Column<int>(type: "integer", nullable: false),
                    SELECT_CMT_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    CMT_SBT = table.Column<int>(type: "integer", nullable: false),
                    KENSA_LABEL = table.Column<int>(type: "integer", nullable: false),
                    GAIRAI_KANSEN = table.Column<int>(type: "integer", nullable: false),
                    JIBI_AGE_KASAN = table.Column<int>(type: "integer", nullable: false),
                    JIBI_SYONIKOKIN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEN_MST", x => new { x.HP_ID, x.ITEM_CD, x.START_DATE });
                });

            migrationBuilder.CreateTable(
                name: "TEN_MST_MOTHER",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    MASTER_SBT = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    KANA_NAME1 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    KANA_NAME2 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    KANA_NAME3 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    KANA_NAME4 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    KANA_NAME5 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    KANA_NAME6 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    KANA_NAME7 = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    RYOSYU_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    RECE_NAME = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    TEN_ID = table.Column<int>(type: "integer", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    RECE_UNIT_CD = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    RECE_UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ODR_UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    CNV_UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ODR_TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    CNV_TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    DEFAULT_VAL = table.Column<double>(type: "double precision", nullable: false),
                    IS_ADOPTED = table.Column<int>(type: "integer", nullable: false),
                    KOUKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKATU_KENSA = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    IGAKUKANRI = table.Column<int>(type: "integer", nullable: false),
                    JITUDAY_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JITUDAY = table.Column<int>(type: "integer", nullable: false),
                    DAY_COUNT = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KANREN_KBN = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_ID = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_MIN = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_MAX = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_VAL = table.Column<int>(type: "integer", nullable: false),
                    KIZAMI_TEN = table.Column<double>(type: "double precision", nullable: false),
                    KIZAMI_ERR = table.Column<int>(type: "integer", nullable: false),
                    MAX_COUNT = table.Column<int>(type: "integer", nullable: false),
                    MAX_COUNT_ERR = table.Column<int>(type: "integer", nullable: false),
                    TYU_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    TYU_SEQ = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    TUSOKU_AGE = table.Column<int>(type: "integer", nullable: false),
                    MIN_AGE = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    MAX_AGE = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGE_CHECK = table.Column<int>(type: "integer", nullable: false),
                    TIME_KASAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUTEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUTEKI_SISETU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOTI_NYUYOJI_KBN = table.Column<int>(type: "integer", nullable: false),
                    LOW_WEIGHT_KBN = table.Column<int>(type: "integer", nullable: false),
                    HANDAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HANDAN_GRP_KBN = table.Column<int>(type: "integer", nullable: false),
                    TEIGEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEKITUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    KEIBU_KBN = table.Column<int>(type: "integer", nullable: false),
                    AUTO_HOUGOU_KBN = table.Column<int>(type: "integer", nullable: false),
                    GAIRAI_KANRI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TUSOKU_TARGET_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    TYOONPA_NAISI_KBN = table.Column<int>(type: "integer", nullable: false),
                    AUTO_FUNGO_KBN = table.Column<int>(type: "integer", nullable: false),
                    TYOONPA_GYOKO_KBN = table.Column<int>(type: "integer", nullable: false),
                    GAZO_KASAN = table.Column<int>(type: "integer", nullable: false),
                    KANSATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    MASUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    FUKUBIKU_NAISI_KASAN = table.Column<int>(type: "integer", nullable: false),
                    FUKUBIKU_KOTUNAN_KASAN = table.Column<int>(type: "integer", nullable: false),
                    MASUI_KASAN = table.Column<int>(type: "integer", nullable: false),
                    MONITER_KASAN = table.Column<int>(type: "integer", nullable: false),
                    TOKETU_KASAN = table.Column<int>(type: "integer", nullable: false),
                    TEN_KBN_NO = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SHORTSTAY_OPE = table.Column<int>(type: "integer", nullable: false),
                    BUI_KBN = table.Column<int>(type: "integer", nullable: false),
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
                    AGEKASAN_MIN1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_MAX1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AGEKASAN_MIN2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_MAX2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AGEKASAN_MIN3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_MAX3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_CD3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AGEKASAN_MIN4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_MAX4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    AGEKASAN_CD4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    KENSA_CMT = table.Column<int>(type: "integer", nullable: false),
                    MADOKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SINKEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEIBUTU_KBN = table.Column<int>(type: "integer", nullable: false),
                    ZOUEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KBN = table.Column<int>(type: "integer", nullable: false),
                    ZAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    CAPACITY = table.Column<int>(type: "integer", nullable: false),
                    KOHATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOKUZAI_AGE_KBN = table.Column<int>(type: "integer", nullable: false),
                    SANSO_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOKUZAI_SBT = table.Column<int>(type: "integer", nullable: false),
                    MAX_PRICE = table.Column<int>(type: "integer", nullable: false),
                    MAX_TEN = table.Column<int>(type: "integer", nullable: false),
                    SYUKEI_SAKI = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CD_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    CD_SYO = table.Column<int>(type: "integer", nullable: false),
                    CD_BU = table.Column<int>(type: "integer", nullable: false),
                    CD_KBNNO = table.Column<int>(type: "integer", nullable: false),
                    CD_EDANO = table.Column<int>(type: "integer", nullable: false),
                    CD_KOUNO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KOKUJI_SYO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_BU = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI_KOU_NO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KOHYO_JUN = table.Column<int>(type: "integer", nullable: false),
                    YJ_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    YAKKA_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SYUSAI_SBT = table.Column<int>(type: "integer", nullable: false),
                    SYOHIN_KANREN = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    UPD_DATE = table.Column<int>(type: "integer", nullable: false),
                    DEL_DATE = table.Column<int>(type: "integer", nullable: false),
                    KEIKA_DATE = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SISI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SHOT_CNT = table.Column<int>(type: "integer", nullable: false),
                    IS_NOSEARCH = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RYOSYU = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    JIHI_SBT = table.Column<int>(type: "integer", nullable: false),
                    KAZEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    YOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    IPN_NAME_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FUKUYO_RISE = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_MORNING = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_DAYTIME = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_NIGHT = table.Column<int>(type: "integer", nullable: false),
                    FUKUYO_SLEEP = table.Column<int>(type: "integer", nullable: false),
                    SURYO_ROUNDUP_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOUSEISIN_KBN = table.Column<int>(type: "integer", nullable: false),
                    CHUSYA_DRUG_SBT = table.Column<int>(type: "integer", nullable: false),
                    KENSA_FUKUSU_SANTEI = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SANTEIGAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    KENSA_ITEM_CD = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    KENSA_ITEM_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_CD1 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RENKEI_CD2 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SAIKETU_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL1 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA1 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL2 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA2 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL3 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA3 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL4 = table.Column<int>(type: "integer", nullable: false),
                    CMT_COL_KETA4 = table.Column<int>(type: "integer", nullable: false),
                    SELECT_CMT_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEN_MST_MOTHER", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TIME_ZONE_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    YOUBI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    END_TIME = table.Column<int>(type: "integer", nullable: false),
                    TIME_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIME_ZONE_CONF", x => new { x.HP_ID, x.YOUBI_KBN, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "TIME_ZONE_DAY_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    END_TIME = table.Column<int>(type: "integer", nullable: false),
                    TIME_KBN = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIME_ZONE_DAY_INF", x => new { x.HP_ID, x.ID, x.SIN_DATE });
                });

            migrationBuilder.CreateTable(
                name: "TODO_GRP_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TODO_GRP_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TODO_GRP_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    GRP_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TODO_GRP_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TODO_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TODO_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TODO_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    TODO_KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    TODO_GRP_NO = table.Column<int>(type: "integer", nullable: false),
                    TANTO = table.Column<int>(type: "integer", nullable: false),
                    TERM = table.Column<int>(type: "integer", nullable: false),
                    CMT1 = table.Column<string>(type: "text", nullable: false),
                    CMT2 = table.Column<string>(type: "text", nullable: false),
                    IS_DONE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TODO_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TODO_KBN_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TODO_KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    TODO_KBN_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ACT_CD = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TODO_KBN_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TOKKI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TOKKI_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    TOKKI_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOKKI_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "UKETUKE_SBT_DAY_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UKETUKE_SBT_DAY_INF", x => new { x.HP_ID, x.SIN_DATE, x.SEQ_NO });
                });

            migrationBuilder.CreateTable(
                name: "UKETUKE_SBT_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    KBN_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBN_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UKETUKE_SBT_MST", x => new { x.HP_ID, x.KBN_ID });
                });

            migrationBuilder.CreateTable(
                name: "UNIT_MST",
                columns: table => new
                {
                    UNIT_CD = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UNIT_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIT_MST", x => x.UNIT_CD);
                });

            migrationBuilder.CreateTable(
                name: "USER_CONF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    GRP_ITEM_CD = table.Column<int>(type: "integer", nullable: false),
                    GRP_ITEM_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    VAL = table.Column<int>(type: "integer", nullable: false),
                    PARAM = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_CONF", x => new { x.HP_ID, x.USER_ID, x.GRP_CD, x.GRP_ITEM_CD, x.GRP_ITEM_EDA_NO });
                });

            migrationBuilder.CreateTable(
                name: "USER_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    JOB_CD = table.Column<int>(type: "integer", nullable: false),
                    MANAGER_KBN = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    KANA_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SNAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DR_NAME = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    LOGIN_ID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LOGIN_PASS = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MAYAKU_LICENSE_NO = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    RENKEI_CD1 = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MST", x => new { x.ID, x.HP_ID });
                });

            migrationBuilder.CreateTable(
                name: "USER_PERMISSION",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    FUNCTION_CD = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    PERMISSION = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_PERMISSION", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_KOUI",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SYUKEI_SAKI = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOKATU_KENSA = table.Column<int>(type: "integer", nullable: false),
                    COUNT = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CD_KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    REC_ID = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    JIHI_SBT = table.Column<int>(type: "integer", nullable: false),
                    KAZEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRK_SIN_KOUI", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_KOUI_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    REC_ID = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ITEM_SBT = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ODR_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_NAME = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    SURYO2 = table.Column<double>(type: "double precision", nullable: false),
                    FMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    UNIT_CD = table.Column<int>(type: "integer", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TEN_ID = table.Column<int>(type: "integer", nullable: false),
                    TEN = table.Column<double>(type: "double precision", nullable: false),
                    CD_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    CD_KBNNO = table.Column<int>(type: "integer", nullable: false),
                    CD_EDANO = table.Column<int>(type: "integer", nullable: false),
                    CD_KOUNO = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    KOKUJI2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    TYU_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    TYU_SEQ = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    TUSOKU_AGE = table.Column<int>(type: "integer", nullable: false),
                    ITEM_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_PAPER_RECE = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RYOSYU = table.Column<int>(type: "integer", nullable: false),
                    IS_AUTO_ADD = table.Column<int>(type: "integer", nullable: false),
                    CMT_OPT = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CMT1 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CMT_CD1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_OPT1 = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CMT2 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CMT_CD2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_OPT2 = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    CMT3 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CMT_CD3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_OPT3 = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRK_SIN_KOUI_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_KOUI_DETAIL_DEL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ROW_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DEL_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SANTEI_DATE = table.Column<int>(type: "integer", nullable: false),
                    DEL_SBT = table.Column<int>(type: "integer", nullable: false),
                    IS_WARNING = table.Column<int>(type: "integer", nullable: false),
                    TERM_CNT = table.Column<int>(type: "integer", nullable: false),
                    TERM_SBT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRK_SIN_KOUI_DETAIL_DEL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "WRK_SIN_RP_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIN_ID = table.Column<int>(type: "integer", nullable: false),
                    CD_NO = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRK_SIN_RP_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "YAKKA_SYUSAI_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    YAKKA_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEIBUN = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    HINMOKU = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    KBN = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    SYUSAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    KEIKA = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BIKO = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    JUN_SENPATU = table.Column<int>(type: "integer", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    YAKKA = table.Column<double>(type: "double precision", nullable: false),
                    IS_NOTARGET = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YAKKA_SYUSAI_MST", x => new { x.HP_ID, x.YAKKA_CD, x.ITEM_CD, x.START_DATE });
                });

            migrationBuilder.CreateTable(
                name: "YOHO_INF_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    YOHO_SUFFIX = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOHO_INF_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "YOHO_SET_MST",
                columns: table => new
                {
                    SET_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOHO_SET_MST", x => x.SET_ID);
                });

            migrationBuilder.CreateTable(
                name: "YOYAKU_ODR_INF",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    YOYAKU_KARTE_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    YOYAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    ODR_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    RP_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_SBT = table.Column<int>(type: "integer", nullable: false),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false),
                    TOSEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    DAYS_CNT = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOYAKU_ODR_INF", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "YOYAKU_ODR_INF_DETAIL",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    YOYAKU_KARTE_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_NO = table.Column<long>(type: "bigint", nullable: false),
                    RP_EDA_NO = table.Column<long>(type: "bigint", nullable: false),
                    ROW_NO = table.Column<long>(type: "bigint", nullable: false),
                    YOYAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    SIN_KOUI_KBN = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ITEM_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    SURYO = table.Column<double>(type: "double precision", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    UNIT_SBT = table.Column<int>(type: "integer", nullable: false),
                    TERM_VAL = table.Column<double>(type: "double precision", nullable: false),
                    KOHATU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOHO_LIMIT_KBN = table.Column<int>(type: "integer", nullable: false),
                    DRUG_KBN = table.Column<int>(type: "integer", nullable: false),
                    YOHO_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOKUJI1 = table.Column<int>(type: "integer", nullable: false),
                    IS_NODSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    IPN_CD = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    IPN_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    BUNKATU = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT_NAME = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CMT_OPT = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false),
                    FONT_COLOR = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOYAKU_ODR_INF_DETAIL", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "YOYAKU_SBT_MST",
                columns: table => new
                {
                    HP_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YOYAKU_SBT = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SBT_NAME = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    DEFAULT_CMT = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOYAKU_SBT_MST", x => x.HP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_DOC_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DSP_FILE_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IS_LOCKED = table.Column<int>(type: "integer", nullable: false),
                    LOCK_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LOCK_ID = table.Column<int>(type: "integer", nullable: false),
                    LOCK_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_DOC_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_FILING_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    GET_DATE = table.Column<int>(type: "integer", nullable: false),
                    CATEGORY_CD = table.Column<int>(type: "integer", nullable: false),
                    FILE_NO = table.Column<int>(type: "integer", nullable: false),
                    FILE_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DSP_FILE_NAME = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    FILE_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_FILING_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_KENSA_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    IRAI_CD = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    INOUT_KBN = table.Column<int>(type: "integer", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    TOSEKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    RESULT_CHECK = table.Column<int>(type: "integer", nullable: false),
                    CENTER_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NYUBI = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    YOKETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    BILIRUBIN = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_KENSA_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_KENSA_INF_DETAIL",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    IRAI_CD = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IRAI_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    KENSA_ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RESULT_VAL = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RESULT_TYPE = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ABNORMAL_KBN = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CMT_CD1 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CMT_CD2 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_KENSA_INF_DETAIL", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_LIMIT_CNT_LIST_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KOHI_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<string>(type: "character varying(61)", maxLength: 61, nullable: false),
                    OYA_RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_LIMIT_CNT_LIST_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_LIMIT_LIST_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KOHI_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SORT_KEY = table.Column<string>(type: "character varying(61)", maxLength: 61, nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    FUTAN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    TOTAL_GAKU = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_LIMIT_LIST_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_MONSHIN_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "text", nullable: false),
                    RTEXT = table.Column<string>(type: "text", nullable: false),
                    GET_KBN = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_MONSHIN_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ALRGY_DRUG",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DRUG_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_ALRGY_DRUG", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ALRGY_ELSE",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ALRGY_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_ALRGY_ELSE", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ALRGY_FOOD",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ALRGY_KBN = table.Column<string>(type: "text", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_ALRGY_FOOD", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_CMT_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_CMT_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_FAMILY",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FAMILY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZOKUGARA_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    PARENT_ID = table.Column<int>(type: "integer", nullable: false),
                    FAMILY_PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DEAD = table.Column<int>(type: "integer", nullable: false),
                    IS_SEPARATED = table.Column<int>(type: "integer", nullable: false),
                    BIKO = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_FAMILY", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_FAMILY_REKI",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    FAMILY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOTAI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_FAMILY_REKI", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_GRP_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    GRP_CODE = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_GRP_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_CHECK",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_GRP = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CHECK_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CHECK_ID = table.Column<int>(type: "integer", nullable: false),
                    CHECK_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CHECK_CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_HOKEN_CHECK", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKENSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    HONKE_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HOKENSYA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HOKENSYA_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    HOKENSYA_ADDRESS = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HOKENSYA_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    KEIZOKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    SIKAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    KOFU_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_KBN = table.Column<int>(type: "integer", nullable: false),
                    KOGAKU_TYPE = table.Column<int>(type: "integer", nullable: false),
                    TOKUREI_YM1 = table.Column<int>(type: "integer", nullable: false),
                    TOKUREI_YM2 = table.Column<int>(type: "integer", nullable: false),
                    TASUKAI_YM = table.Column<int>(type: "integer", nullable: false),
                    SYOKUMU_KBN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_RATE = table.Column<int>(type: "integer", nullable: false),
                    GENMEN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    TOKKI2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    TOKKI3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    TOKKI4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    TOKKI5 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ROUSAI_KOFU_NO = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    ROUSAI_SAIGAI_KBN = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_JIGYOSYO_NAME = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    ROUSAI_PREF_NAME = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ROUSAI_CITY_NAME = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ROUSAI_SYOBYO_DATE = table.Column<int>(type: "integer", nullable: false),
                    ROUSAI_SYOBYO_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ROUSAI_ROUDOU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ROUSAI_KANTOKU_CD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ROUSAI_RECE_COUNT = table.Column<int>(type: "integer", nullable: false),
                    JIBAI_HOKEN_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    JIBAI_HOKEN_TANTO = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    JIBAI_HOKEN_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    JIBAI_JYUSYOU_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    RYOYO_START_DATE = table.Column<int>(type: "integer", nullable: false),
                    RYOYO_END_DATE = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_HOKEN_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_PATTERN",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOKEN_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_SBT_CD = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI1_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI2_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI3_ID = table.Column<int>(type: "integer", nullable: false),
                    KOHI4_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_MEMO = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_HOKEN_PATTERN", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_HOKEN_SCAN",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_GRP = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FILE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_HOKEN_SCAN", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_NUM = table.Column<long>(type: "bigint", nullable: false),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SEX = table.Column<int>(type: "integer", nullable: false),
                    BIRTHDAY = table.Column<int>(type: "integer", nullable: false),
                    IS_DEAD = table.Column<int>(type: "integer", nullable: false),
                    DEATH_DATE = table.Column<int>(type: "integer", nullable: false),
                    HOME_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    HOME_ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HOME_ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TEL1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    TEL2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    MAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SETAINUSI = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ZOKUGARA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    JOB = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    RENRAKU_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RENRAKU_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    RENRAKU_ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RENRAKU_ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RENRAKU_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    RENRAKU_MEMO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OFFICE_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OFFICE_POST = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    OFFICE_ADDRESS1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OFFICE_ADDRESS2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OFFICE_TEL = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    OFFICE_MEMO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_RYOSYO_DETAIL = table.Column<int>(type: "integer", nullable: false),
                    PRIMARY_DOCTOR = table.Column<int>(type: "integer", nullable: false),
                    IS_TESTER = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    MAIN_HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    REFERENCE_NO = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_CONS_FLG = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_INFECTION",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOTAI_CD = table.Column<string>(type: "text", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_INFECTION", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_JIBKAR",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    WEB_ID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ODR_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    ODR_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KARTE_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    KARTE_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KENSA_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    KENSA_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BYOMEI_KAIJI = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_JIBKAR", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_KIO_REKI",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI_CD = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    BYOTAI_CD = table.Column<string>(type: "text", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_KIO_REKI", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_KOHI",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PREF_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_NO = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    FUTANSYA_NO = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    JYUKYUSYA_NO = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    TOKUSYU_NO = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SIKAKU_DATE = table.Column<int>(type: "integer", nullable: false),
                    KOFU_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    RATE = table.Column<int>(type: "integer", nullable: false),
                    GENDOGAKU = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    HOKEN_SBT_KBN = table.Column<int>(type: "integer", nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_KOHI", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_KYUSEI",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KANA_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_KYUSEI", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_MEMO",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_MEMO", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_OTC_DRUG",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    SERIAL_NUM = table.Column<int>(type: "integer", nullable: false),
                    TRADE_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_OTC_DRUG", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_OTHER_DRUG",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DRUG_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_OTHER_DRUG", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_PREGNANCY",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    PERIOD_DATE = table.Column<int>(type: "integer", nullable: false),
                    PERIOD_DUE_DATE = table.Column<int>(type: "integer", nullable: false),
                    OVULATION_DATE = table.Column<int>(type: "integer", nullable: false),
                    OVULATION_DUE_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_PREGNANCY", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_ROUSAI_TENKI",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    SINKEI = table.Column<int>(type: "integer", nullable: false),
                    TENKI = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_ROUSAI_TENKI", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_SANTEI_CONF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBN_VAL = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_SANTEI_CONF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_SUPPLE",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    INDEX_CD = table.Column<string>(type: "text", nullable: false),
                    INDEX_WORD = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_SUPPLE", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_PT_TAG",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MEMO = table.Column<string>(type: "text", nullable: false),
                    MEMO_DATA = table.Column<byte[]>(type: "bytea", nullable: false),
                    START_DATE = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_UKETUKE = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_KARTE = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_KAIKEI = table.Column<int>(type: "integer", nullable: false),
                    IS_DSP_RECE = table.Column<int>(type: "integer", nullable: false),
                    BACKGROUND_COLOR = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    TAG_GRP_CD = table.Column<int>(type: "integer", nullable: false),
                    ALPHABLEND_VAL = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    FONTSIZE = table.Column<int>(type: "integer", nullable: false),
                    WIDTH = table.Column<int>(type: "integer", nullable: false),
                    HEIGHT = table.Column<int>(type: "integer", nullable: false),
                    LEFT = table.Column<int>(type: "integer", nullable: false),
                    TOP = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_PT_TAG", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_CMT_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_CMT_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    OYA_RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    IS_YOYAKU = table.Column<int>(type: "integer", nullable: false),
                    YOYAKU_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    YOYAKU_ID = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    UKETUKE_ID = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_NO = table.Column<int>(type: "integer", nullable: false),
                    SIN_START_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    SIN_END_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    KAIKEI_TIME = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    KAIKEI_ID = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    TANTO_ID = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_PID = table.Column<int>(type: "integer", nullable: false),
                    SYOSAISIN_KBN = table.Column<int>(type: "integer", nullable: false),
                    JIKAN_KBN = table.Column<int>(type: "integer", nullable: false),
                    CONFIRMATION_RESULT = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CONFIRMATION_STATE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    SANTEI_KBN = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_KBN_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    GRP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KBN_CD = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETE = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_KBN_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_LIST_CMT",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false),
                    TEXT = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_LIST_CMT", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RAIIN_LIST_TAG",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    TAG_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RAIIN_LIST_TAG", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_CHECK_CMT",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_PENDING = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IS_CHECKED = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RECE_CHECK_CMT", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_CMT",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    CMT_KBN = table.Column<int>(type: "integer", nullable: false),
                    CMT_SBT = table.Column<int>(type: "integer", nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CMT = table.Column<string>(type: "text", nullable: false),
                    CMT_DATA = table.Column<string>(type: "character varying(38)", maxLength: 38, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RECE_CMT", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_INF_EDIT",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RECE_SBT = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI1_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI2_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI3_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    KOHI4_HOUBETU = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HOKEN_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_TENSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_FUTAN = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_RECE_KYUFU = table.Column<int>(type: "integer", nullable: true),
                    HOKEN_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI1_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI2_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI3_NISSU = table.Column<int>(type: "integer", nullable: true),
                    KOHI4_NISSU = table.Column<int>(type: "integer", nullable: true),
                    TOKKI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TOKKI5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RECE_INF_EDIT", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RECE_SEIKYU",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SEIKYU_YM = table.Column<int>(type: "integer", nullable: false),
                    SEIKYU_KBN = table.Column<int>(type: "integer", nullable: false),
                    PRE_HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    CMT = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RECE_SEIKYU", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RSV_DAY_COMMENT",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RSV_DAY_COMMENT", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_RSV_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RSV_FRAME_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    START_TIME = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    RSV_SBT = table.Column<int>(type: "integer", nullable: false),
                    TANTO_ID = table.Column<int>(type: "integer", nullable: false),
                    KA_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_RSV_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SANTEI_INF_DETAIL",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_CD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    END_DATE = table.Column<int>(type: "integer", nullable: false),
                    KISAN_SBT = table.Column<int>(type: "integer", nullable: false),
                    KISAN_DATE = table.Column<int>(type: "integer", nullable: false),
                    BYOMEI = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    HOSOKU_COMMENT = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    COMMENT = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SANTEI_INF_DETAIL", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SEIKATUREKI_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SEIKATUREKI_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SUMMARY_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TEXT = table.Column<string>(type: "text", nullable: false),
                    RTEXT = table.Column<byte[]>(type: "bytea", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SUMMARY_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SYOBYO_KEIKA",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    SIN_DAY = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KEIKA = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SYOBYO_KEIKA", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SYOUKI_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_YM = table.Column<int>(type: "integer", nullable: false),
                    HOKEN_ID = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    SYOUKI_KBN = table.Column<int>(type: "integer", nullable: false),
                    SYOUKI = table.Column<string>(type: "text", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SYOUKI_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_SYUNO_NYUKIN",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SORT_NO = table.Column<int>(type: "integer", nullable: false),
                    ADJUST_FUTAN = table.Column<int>(type: "integer", nullable: false),
                    NYUKIN_GAKU = table.Column<int>(type: "integer", nullable: false),
                    PAYMENT_METHOD_CD = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    NYUKIN_CMT = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    SEQ_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NYUKIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    NYUKINJI_TENSU = table.Column<int>(type: "integer", nullable: false),
                    NYUKINJI_SEIKYU = table.Column<int>(type: "integer", nullable: false),
                    NYUKINJI_DETAIL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_SYUNO_NYUKIN", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_TODO_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    TODO_NO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TODO_EDA_NO = table.Column<int>(type: "integer", nullable: false),
                    PT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    RAIIN_NO = table.Column<long>(type: "bigint", nullable: false),
                    TODO_KBN_NO = table.Column<int>(type: "integer", nullable: false),
                    TODO_GRP_NO = table.Column<int>(type: "integer", nullable: false),
                    TANTO = table.Column<int>(type: "integer", nullable: false),
                    TERM = table.Column<int>(type: "integer", nullable: false),
                    CMT1 = table.Column<string>(type: "text", nullable: false),
                    CMT2 = table.Column<string>(type: "text", nullable: false),
                    IS_DONE = table.Column<int>(type: "integer", nullable: false),
                    IS_DELETED = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATE_ID = table.Column<int>(type: "integer", nullable: false),
                    UPDATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_TODO_INF", x => x.OP_ID);
                });

            migrationBuilder.CreateTable(
                name: "Z_UKETUKE_SBT_DAY_INF",
                columns: table => new
                {
                    OP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OP_TYPE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OP_TIME = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OP_ADDR = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OP_HOSTNAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HP_ID = table.Column<int>(type: "integer", nullable: false),
                    SIN_DATE = table.Column<int>(type: "integer", nullable: false),
                    SEQ_NO = table.Column<int>(type: "integer", nullable: false),
                    UKETUKE_SBT = table.Column<int>(type: "integer", nullable: false),
                    CREATE_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_MACHINE = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Z_UKETUKE_SBT_DAY_INF", x => x.OP_ID);
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
                name: "COLUMN_SETTING");

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
                name: "JSON_SETTING");

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
                name: "M56_EX_ANALOGUE   ");

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
