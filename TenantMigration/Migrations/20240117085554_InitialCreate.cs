using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounting_form_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    formno = table.Column<int>(name: "form_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    formname = table.Column<string>(name: "form_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    formtype = table.Column<int>(name: "form_type", type: "integer", nullable: false),
                    printsort = table.Column<int>(name: "print_sort", type: "integer", nullable: false),
                    miseisankbn = table.Column<int>(name: "miseisan_kbn", type: "integer", nullable: false),
                    saikbn = table.Column<int>(name: "sai_kbn", type: "integer", nullable: false),
                    misyukbn = table.Column<int>(name: "misyu_kbn", type: "integer", nullable: false),
                    seikyukbn = table.Column<int>(name: "seikyu_kbn", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    form = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    @base = table.Column<int>(name: "base", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounting_form_mst", x => new { x.hpid, x.formno });
                });

            migrationBuilder.CreateTable(
                name: "approval_inf",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: true),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: true),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_inf", x => new { x.id, x.hpid, x.raiinno });
                });

            migrationBuilder.CreateTable(
                name: "audit_trail_log",
                columns: table => new
                {
                    logid = table.Column<long>(name: "log_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    logdate = table.Column<DateTime>(name: "log_date", type: "timestamp with time zone", nullable: false),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    eventcd = table.Column<string>(name: "event_cd", type: "character varying(11)", maxLength: 11, nullable: true),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinday = table.Column<int>(name: "sin_day", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    machine = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_trail_log", x => x.logid);
                });

            migrationBuilder.CreateTable(
                name: "audit_trail_log_detail",
                columns: table => new
                {
                    logid = table.Column<long>(name: "log_id", type: "bigint", nullable: false),
                    hosoku = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_trail_log_detail", x => x.logid);
                });

            migrationBuilder.CreateTable(
                name: "auto_santei_mst",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auto_santei_mst", x => new { x.id, x.hpid, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "backup_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    outputtype = table.Column<int>(name: "output_type", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    fromdate = table.Column<int>(name: "from_date", type: "integer", nullable: false),
                    todate = table.Column<int>(name: "to_date", type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_backup_req", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bui_odr_byomei_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    buiid = table.Column<int>(name: "bui_id", type: "integer", nullable: false),
                    byomeibui = table.Column<string>(name: "byomei_bui", type: "character varying(100)", maxLength: 100, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bui_odr_byomei_mst", x => new { x.hpid, x.buiid, x.byomeibui });
                });

            migrationBuilder.CreateTable(
                name: "bui_odr_item_byomei_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    byomeibui = table.Column<string>(name: "byomei_bui", type: "character varying(100)", maxLength: 100, nullable: false),
                    lrkbn = table.Column<int>(name: "lr_kbn", type: "integer", nullable: false),
                    bothkbn = table.Column<int>(name: "both_kbn", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bui_odr_item_byomei_mst", x => new { x.hpid, x.itemcd, x.byomeibui });
                });

            migrationBuilder.CreateTable(
                name: "bui_odr_item_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bui_odr_item_mst", x => new { x.hpid, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "bui_odr_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    buiid = table.Column<int>(name: "bui_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    odrbui = table.Column<string>(name: "odr_bui", type: "character varying(100)", maxLength: 100, nullable: true),
                    lrkbn = table.Column<int>(name: "lr_kbn", type: "integer", nullable: false),
                    mustlrkbn = table.Column<int>(name: "must_lr_kbn", type: "integer", nullable: false),
                    bothkbn = table.Column<int>(name: "both_kbn", type: "integer", nullable: false),
                    koui30 = table.Column<int>(name: "koui_30", type: "integer", nullable: false),
                    koui40 = table.Column<int>(name: "koui_40", type: "integer", nullable: false),
                    koui50 = table.Column<int>(name: "koui_50", type: "integer", nullable: false),
                    koui60 = table.Column<int>(name: "koui_60", type: "integer", nullable: false),
                    koui70 = table.Column<int>(name: "koui_70", type: "integer", nullable: false),
                    koui80 = table.Column<int>(name: "koui_80", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bui_odr_mst", x => new { x.hpid, x.buiid });
                });

            migrationBuilder.CreateTable(
                name: "byomei_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    byomei = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    sbyomei = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame1 = table.Column<string>(name: "kana_name1", type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame2 = table.Column<string>(name: "kana_name2", type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame3 = table.Column<string>(name: "kana_name3", type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame4 = table.Column<string>(name: "kana_name4", type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame5 = table.Column<string>(name: "kana_name5", type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame6 = table.Column<string>(name: "kana_name6", type: "character varying(200)", maxLength: 200, nullable: true),
                    kananame7 = table.Column<string>(name: "kana_name7", type: "character varying(200)", maxLength: 200, nullable: true),
                    ikocd = table.Column<string>(name: "iko_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    sikkancd = table.Column<int>(name: "sikkan_cd", type: "integer", nullable: false),
                    tandokukinsi = table.Column<int>(name: "tandoku_kinsi", type: "integer", nullable: false),
                    hokengai = table.Column<int>(name: "hoken_gai", type: "integer", nullable: false),
                    byomeikanri = table.Column<string>(name: "byomei_kanri", type: "character varying(8)", maxLength: 8, nullable: true),
                    saitakukbn = table.Column<string>(name: "saitaku_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    koukancd = table.Column<string>(name: "koukan_cd", type: "character varying(4)", maxLength: 4, nullable: true),
                    syusaidate = table.Column<int>(name: "syusai_date", type: "integer", nullable: false),
                    upddate = table.Column<int>(name: "upd_date", type: "integer", nullable: false),
                    deldate = table.Column<int>(name: "del_date", type: "integer", nullable: false),
                    nanbyocd = table.Column<int>(name: "nanbyo_cd", type: "integer", nullable: false),
                    icd101 = table.Column<string>(name: "icd10_1", type: "character varying(5)", maxLength: 5, nullable: true),
                    icd102 = table.Column<string>(name: "icd10_2", type: "character varying(5)", maxLength: 5, nullable: true),
                    icd1012013 = table.Column<string>(name: "icd10_1_2013", type: "character varying(5)", maxLength: 5, nullable: true),
                    icd1022013 = table.Column<string>(name: "icd10_2_2013", type: "character varying(5)", maxLength: 5, nullable: true),
                    isadopted = table.Column<int>(name: "is_adopted", type: "integer", nullable: false),
                    syusyokukbn = table.Column<string>(name: "syusyoku_kbn", type: "character varying(8)", maxLength: 8, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_byomei_mst", x => new { x.hpid, x.byomeicd });
                });

            migrationBuilder.CreateTable(
                name: "byomei_mst_aftercare",
                columns: table => new
                {
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    byomei = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_byomei_mst_aftercare", x => new { x.byomeicd, x.byomei, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "byomei_set_generation_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_byomei_set_generation_mst", x => new { x.hpid, x.generationid });
                });

            migrationBuilder.CreateTable(
                name: "byomei_set_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level1 = table.Column<int>(type: "integer", nullable: false),
                    level2 = table.Column<int>(type: "integer", nullable: false),
                    level3 = table.Column<int>(type: "integer", nullable: false),
                    level4 = table.Column<int>(type: "integer", nullable: false),
                    level5 = table.Column<int>(type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    setname = table.Column<string>(name: "set_name", type: "character varying(60)", maxLength: 60, nullable: true),
                    istitle = table.Column<int>(name: "is_title", type: "integer", nullable: false),
                    selecttype = table.Column<int>(name: "select_type", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_byomei_set_mst", x => new { x.hpid, x.generationid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "calc_log",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    logsbt = table.Column<int>(name: "log_sbt", type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    delitemcd = table.Column<string>(name: "del_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    delsbt = table.Column<int>(name: "del_sbt", type: "integer", nullable: false),
                    iswarning = table.Column<int>(name: "is_warning", type: "integer", nullable: false),
                    termcnt = table.Column<int>(name: "term_cnt", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calc_log", x => new { x.hpid, x.ptid, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "calc_status",
                columns: table => new
                {
                    calcid = table.Column<long>(name: "calc_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seikyuup = table.Column<int>(name: "seikyu_up", type: "integer", nullable: false),
                    calcmode = table.Column<int>(name: "calc_mode", type: "integer", nullable: false),
                    clearrecechk = table.Column<int>(name: "clear_rece_chk", type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calc_status", x => x.calcid);
                });

            migrationBuilder.CreateTable(
                name: "cmt_check_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cmt_check_mst", x => new { x.hpid, x.itemcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "cmt_kbn_mst",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cmt_kbn_mst", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "column_setting",
                columns: table => new
                {
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    tablename = table.Column<string>(name: "table_name", type: "text", nullable: false),
                    columnname = table.Column<string>(name: "column_name", type: "text", nullable: false),
                    displayorder = table.Column<int>(name: "display_order", type: "integer", nullable: false),
                    ispinned = table.Column<bool>(name: "is_pinned", type: "boolean", nullable: false),
                    ishidden = table.Column<bool>(name: "is_hidden", type: "boolean", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    orderby = table.Column<string>(name: "order_by", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_column_setting", x => new { x.userid, x.tablename, x.columnname });
                });

            migrationBuilder.CreateTable(
                name: "container_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    containercd = table.Column<long>(name: "container_cd", type: "bigint", nullable: false),
                    containername = table.Column<string>(name: "container_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_container_mst", x => new { x.hpid, x.containercd });
                });

            migrationBuilder.CreateTable(
                name: "conversion_item_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sourceitemcd = table.Column<string>(name: "source_item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    destitemcd = table.Column<string>(name: "dest_item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversion_item_inf", x => new { x.hpid, x.sourceitemcd, x.destitemcd });
                });

            migrationBuilder.CreateTable(
                name: "def_hoken_no",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    digit1 = table.Column<string>(name: "digit_1", type: "character varying(1)", maxLength: 1, nullable: false),
                    digit2 = table.Column<string>(name: "digit_2", type: "character varying(1)", maxLength: 1, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    digit3 = table.Column<string>(name: "digit_3", type: "character varying(1)", maxLength: 1, nullable: true),
                    digit4 = table.Column<string>(name: "digit_4", type: "character varying(1)", maxLength: 1, nullable: true),
                    digit5 = table.Column<string>(name: "digit_5", type: "character varying(1)", maxLength: 1, nullable: true),
                    digit6 = table.Column<string>(name: "digit_6", type: "character varying(1)", maxLength: 1, nullable: true),
                    digit7 = table.Column<string>(name: "digit_7", type: "character varying(1)", maxLength: 1, nullable: true),
                    digit8 = table.Column<string>(name: "digit_8", type: "character varying(1)", maxLength: 1, nullable: true),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_def_hoken_no", x => new { x.hpid, x.digit1, x.digit2, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "densi_haihan_custom",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd1 = table.Column<string>(name: "item_cd1", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    itemcd2 = table.Column<string>(name: "item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    haihankbn = table.Column<int>(name: "haihan_kbn", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    termcnt = table.Column<int>(name: "term_cnt", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_haihan_custom", x => new { x.id, x.hpid, x.itemcd1, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "densi_haihan_day",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd1 = table.Column<string>(name: "item_cd1", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    itemcd2 = table.Column<string>(name: "item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    haihankbn = table.Column<int>(name: "haihan_kbn", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_haihan_day", x => new { x.id, x.hpid, x.itemcd1, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "densi_haihan_karte",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd1 = table.Column<string>(name: "item_cd1", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    itemcd2 = table.Column<string>(name: "item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    haihankbn = table.Column<int>(name: "haihan_kbn", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_haihan_karte", x => new { x.id, x.hpid, x.itemcd1, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "densi_haihan_month",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd1 = table.Column<string>(name: "item_cd1", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    itemcd2 = table.Column<string>(name: "item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    haihankbn = table.Column<int>(name: "haihan_kbn", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    incafter = table.Column<int>(name: "inc_after", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_haihan_month", x => new { x.id, x.hpid, x.itemcd1, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "densi_haihan_week",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd1 = table.Column<string>(name: "item_cd1", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    itemcd2 = table.Column<string>(name: "item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    haihankbn = table.Column<int>(name: "haihan_kbn", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    incafter = table.Column<int>(name: "inc_after", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_haihan_week", x => new { x.id, x.hpid, x.itemcd1, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "densi_hojyo",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    houkatuterm1 = table.Column<int>(name: "houkatu_term1", type: "integer", nullable: false),
                    houkatugrpno1 = table.Column<string>(name: "houkatu_grp_no1", type: "character varying(7)", maxLength: 7, nullable: true),
                    houkatuterm2 = table.Column<int>(name: "houkatu_term2", type: "integer", nullable: false),
                    houkatugrpno2 = table.Column<string>(name: "houkatu_grp_no2", type: "character varying(7)", maxLength: 7, nullable: true),
                    houkatuterm3 = table.Column<int>(name: "houkatu_term3", type: "integer", nullable: false),
                    houkatugrpno3 = table.Column<string>(name: "houkatu_grp_no3", type: "character varying(7)", maxLength: 7, nullable: true),
                    haihanday = table.Column<int>(name: "haihan_day", type: "integer", nullable: false),
                    haihanmonth = table.Column<int>(name: "haihan_month", type: "integer", nullable: false),
                    haihankarte = table.Column<int>(name: "haihan_karte", type: "integer", nullable: false),
                    haihanweek = table.Column<int>(name: "haihan_week", type: "integer", nullable: false),
                    nyuinid = table.Column<int>(name: "nyuin_id", type: "integer", nullable: false),
                    santeikaisu = table.Column<int>(name: "santei_kaisu", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_hojyo", x => new { x.hpid, x.itemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "densi_houkatu",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    houkatuterm = table.Column<int>(name: "houkatu_term", type: "integer", nullable: false),
                    houkatugrpno = table.Column<string>(name: "houkatu_grp_no", type: "character varying(7)", maxLength: 7, nullable: true),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_houkatu", x => new { x.startdate, x.hpid, x.itemcd, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "densi_houkatu_grp",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    houkatugrpno = table.Column<string>(name: "houkatu_grp_no", type: "character varying(7)", maxLength: 7, nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_houkatu_grp", x => new { x.hpid, x.houkatugrpno, x.itemcd, x.seqno, x.usersetting, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "densi_santei_kaisu",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    unitcd = table.Column<int>(name: "unit_cd", type: "integer", nullable: false),
                    maxcount = table.Column<int>(name: "max_count", type: "integer", nullable: false),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    termcount = table.Column<int>(name: "term_count", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    itemgrpcd = table.Column<int>(name: "item_grp_cd", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_densi_santei_kaisu", x => new { x.hpid, x.id, x.itemcd, x.seqno, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "doc_category_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryname = table.Column<string>(name: "category_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_category_mst", x => new { x.hpid, x.categorycd });
                });

            migrationBuilder.CreateTable(
                name: "doc_comment",
                columns: table => new
                {
                    categoryid = table.Column<int>(name: "category_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryname = table.Column<string>(name: "category_name", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    replaceword = table.Column<string>(name: "replace_word", type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_comment", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "doc_comment_detail",
                columns: table => new
                {
                    categoryid = table.Column<int>(name: "category_id", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_comment_detail", x => new { x.categoryid, x.edano });
                });

            migrationBuilder.CreateTable(
                name: "doc_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(300)", maxLength: 300, nullable: true),
                    dspfilename = table.Column<string>(name: "dsp_file_name", type: "character varying(300)", maxLength: 300, nullable: true),
                    islocked = table.Column<int>(name: "is_locked", type: "integer", nullable: false),
                    lockdate = table.Column<DateTime>(name: "lock_date", type: "timestamp with time zone", nullable: true),
                    lockid = table.Column<int>(name: "lock_id", type: "integer", nullable: false),
                    lockmachine = table.Column<string>(name: "lock_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_inf", x => new { x.hpid, x.ptid, x.sindate, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "dosage_mst",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    oncemin = table.Column<double>(name: "once_min", type: "double precision", nullable: false),
                    oncemax = table.Column<double>(name: "once_max", type: "double precision", nullable: false),
                    oncelimit = table.Column<double>(name: "once_limit", type: "double precision", nullable: false),
                    onceunit = table.Column<int>(name: "once_unit", type: "integer", nullable: false),
                    daymin = table.Column<double>(name: "day_min", type: "double precision", nullable: false),
                    daymax = table.Column<double>(name: "day_max", type: "double precision", nullable: false),
                    daylimit = table.Column<double>(name: "day_limit", type: "double precision", nullable: false),
                    dayunit = table.Column<int>(name: "day_unit", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dosage_mst", x => new { x.id, x.hpid, x.itemcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "drug_day_limit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    limitday = table.Column<int>(name: "limit_day", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drug_day_limit", x => new { x.id, x.hpid, x.itemcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "drug_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    infkbn = table.Column<int>(name: "inf_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    druginf = table.Column<string>(name: "drug_inf", type: "character varying(2000)", maxLength: 2000, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drug_inf", x => new { x.infkbn, x.hpid, x.itemcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "drug_unit_conv",
                columns: table => new
                {
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cnvval = table.Column<double>(name: "cnv_val", type: "double precision", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drug_unit_conv", x => new { x.itemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "eps_chk",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    checkresult = table.Column<int>(name: "check_result", type: "integer", nullable: false),
                    samemedicalinstitutionalertflg = table.Column<int>(name: "same_medical_institution_alert_flg", type: "integer", nullable: false),
                    onlineconsent = table.Column<int>(name: "online_consent", type: "integer", nullable: false),
                    oralbrowsingconsent = table.Column<int>(name: "oral_browsing_consent", type: "integer", nullable: false),
                    druginfo = table.Column<string>(name: "drug_info", type: "text", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eps_chk", x => new { x.hpid, x.ptid, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "eps_chk_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    messageid = table.Column<string>(name: "message_id", type: "character varying(3)", maxLength: 3, nullable: false),
                    messagecategory = table.Column<string>(name: "message_category", type: "character varying(50)", maxLength: 50, nullable: false),
                    pharmaceuticalsingredientname = table.Column<string>(name: "pharmaceuticals_ingredient_name", type: "character varying(80)", maxLength: 80, nullable: false),
                    message = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    targetpharmaceuticalcodetype = table.Column<string>(name: "target_pharmaceutical_code_type", type: "character varying(1)", maxLength: 1, nullable: false),
                    targetpharmaceuticalcode = table.Column<string>(name: "target_pharmaceutical_code", type: "character varying(13)", maxLength: 13, nullable: false),
                    targetpharmaceuticalname = table.Column<string>(name: "target_pharmaceutical_name", type: "character varying(80)", maxLength: 80, nullable: false),
                    targetdispensingquantity = table.Column<string>(name: "target_dispensing_quantity", type: "character varying(3)", maxLength: 3, nullable: false),
                    targetusage = table.Column<string>(name: "target_usage", type: "character varying(100)", maxLength: 100, nullable: false),
                    targetdosageform = table.Column<string>(name: "target_dosage_form", type: "character varying(10)", maxLength: 10, nullable: false),
                    pastdate = table.Column<string>(name: "past_date", type: "character varying(8)", maxLength: 8, nullable: false),
                    pastpharmaceuticalcodetype = table.Column<string>(name: "past_pharmaceutical_code_type", type: "character varying(1)", maxLength: 1, nullable: false),
                    pastpharmaceuticalcode = table.Column<string>(name: "past_pharmaceutical_code", type: "character varying(13)", maxLength: 13, nullable: false),
                    pastpharmaceuticalname = table.Column<string>(name: "past_pharmaceutical_name", type: "character varying(80)", maxLength: 80, nullable: false),
                    pastmedicalinstitutionname = table.Column<string>(name: "past_medical_institution_name", type: "character varying(120)", maxLength: 120, nullable: false),
                    pastinsurancepharmacyname = table.Column<string>(name: "past_insurance_pharmacy_name", type: "character varying(120)", maxLength: 120, nullable: false),
                    pastdispensingquantity = table.Column<string>(name: "past_dispensing_quantity", type: "character varying(3)", maxLength: 3, nullable: false),
                    pastusage = table.Column<string>(name: "past_usage", type: "character varying(100)", maxLength: 100, nullable: false),
                    pastdosageform = table.Column<string>(name: "past_dosage_form", type: "character varying(10)", maxLength: 10, nullable: false),
                    comment = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eps_chk_detail", x => new { x.hpid, x.ptid, x.raiinno, x.seqno, x.messageid });
                });

            migrationBuilder.CreateTable(
                name: "eps_prescription",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    refilecount = table.Column<int>(name: "refile_count", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    hokensyano = table.Column<string>(name: "hokensya_no", type: "character varying(8)", maxLength: 8, nullable: false),
                    kigo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    bango = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    edano = table.Column<string>(name: "eda_no", type: "character varying(2)", maxLength: 2, nullable: false),
                    kohifutansyano = table.Column<string>(name: "kohi_futansya_no", type: "character varying(8)", maxLength: 8, nullable: false),
                    kohijyukyusyano = table.Column<string>(name: "kohi_jyukyusya_no", type: "character varying(7)", maxLength: 7, nullable: false),
                    prescriptionid = table.Column<string>(name: "prescription_id", type: "character varying(36)", maxLength: 36, nullable: false),
                    accesscode = table.Column<string>(name: "access_code", type: "character varying(16)", maxLength: 16, nullable: false),
                    issuetype = table.Column<int>(name: "issue_type", type: "integer", nullable: false),
                    prescriptiondocument = table.Column<string>(name: "prescription_document", type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    deletedreason = table.Column<int>(name: "deleted_reason", type: "integer", nullable: false),
                    deleteddate = table.Column<DateTime>(name: "deleted_date", type: "timestamp with time zone", nullable: true),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eps_prescription", x => new { x.hpid, x.ptid, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "eps_reference",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    prescriptionid = table.Column<string>(name: "prescription_id", type: "character varying(36)", maxLength: 36, nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    prescriptionreferenceinformation = table.Column<string>(name: "prescription_reference_information", type: "text", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eps_reference", x => new { x.hpid, x.ptid, x.prescriptionid });
                });

            migrationBuilder.CreateTable(
                name: "event_mst",
                columns: table => new
                {
                    eventcd = table.Column<string>(name: "event_cd", type: "character varying(11)", maxLength: 11, nullable: false),
                    eventname = table.Column<string>(name: "event_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    audittrailing = table.Column<int>(name: "audit_trailing", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_mst", x => x.eventcd);
                });

            migrationBuilder.CreateTable(
                name: "except_hokensya",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    hokensyano = table.Column<string>(name: "hokensya_no", type: "character varying(8)", maxLength: 8, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_except_hokensya", x => new { x.id, x.hpid, x.prefno, x.hokenno, x.hokenedano, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "filing_auto_imp",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    machine = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    imppath = table.Column<string>(name: "imp_path", type: "character varying(300)", maxLength: 300, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filing_auto_imp", x => new { x.seqno, x.hpid });
                });

            migrationBuilder.CreateTable(
                name: "filing_category_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryname = table.Column<string>(name: "category_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    dspkanzok = table.Column<int>(name: "dsp_kanzok", type: "integer", nullable: false),
                    isfiledeleted = table.Column<int>(name: "is_file_deleted", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filing_category_mst", x => new { x.categorycd, x.hpid });
                });

            migrationBuilder.CreateTable(
                name: "filing_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    fileid = table.Column<int>(name: "file_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    getdate = table.Column<int>(name: "get_date", type: "integer", nullable: false),
                    fileno = table.Column<int>(name: "file_no", type: "integer", nullable: false),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(300)", maxLength: 300, nullable: true),
                    dspfilename = table.Column<string>(name: "dsp_file_name", type: "character varying(1024)", maxLength: 1024, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filing_inf", x => new { x.hpid, x.fileid });
                });

            migrationBuilder.CreateTable(
                name: "function_mst",
                columns: table => new
                {
                    functioncd = table.Column<string>(name: "function_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    functionname = table.Column<string>(name: "function_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_function_mst", x => x.functioncd);
                });

            migrationBuilder.CreateTable(
                name: "gc_std_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    stdkbn = table.Column<int>(name: "std_kbn", type: "integer", nullable: false),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    point = table.Column<double>(type: "double precision", nullable: false),
                    sdm25 = table.Column<double>(name: "sd_m25", type: "double precision", nullable: false),
                    sdm20 = table.Column<double>(name: "sd_m20", type: "double precision", nullable: false),
                    sdm10 = table.Column<double>(name: "sd_m10", type: "double precision", nullable: false),
                    sdavg = table.Column<double>(name: "sd_avg", type: "double precision", nullable: false),
                    sdp10 = table.Column<double>(name: "sd_p10", type: "double precision", nullable: false),
                    sdp20 = table.Column<double>(name: "sd_p20", type: "double precision", nullable: false),
                    sdp25 = table.Column<double>(name: "sd_p25", type: "double precision", nullable: false),
                    per03 = table.Column<double>(name: "per_03", type: "double precision", nullable: false),
                    per10 = table.Column<double>(name: "per_10", type: "double precision", nullable: false),
                    per25 = table.Column<double>(name: "per_25", type: "double precision", nullable: false),
                    per50 = table.Column<double>(name: "per_50", type: "double precision", nullable: false),
                    per75 = table.Column<double>(name: "per_75", type: "double precision", nullable: false),
                    per90 = table.Column<double>(name: "per_90", type: "double precision", nullable: false),
                    per97 = table.Column<double>(name: "per_97", type: "double precision", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gc_std_mst", x => new { x.hpid, x.stdkbn, x.sex, x.point });
                });

            migrationBuilder.CreateTable(
                name: "hoken_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    hokensbtkbn = table.Column<int>(name: "hoken_sbt_kbn", type: "integer", nullable: false),
                    hokenkohikbn = table.Column<int>(name: "hoken_kohi_kbn", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    hokenname = table.Column<string>(name: "hoken_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    hokensname = table.Column<string>(name: "hoken_sname", type: "character varying(20)", maxLength: 20, nullable: true),
                    hokennamecd = table.Column<string>(name: "hoken_name_cd", type: "character varying(5)", maxLength: 5, nullable: true),
                    checkdigit = table.Column<int>(name: "check_digit", type: "integer", nullable: false),
                    jyukyucheckdigit = table.Column<int>(name: "jyukyu_check_digit", type: "integer", nullable: false),
                    isfutansyanocheck = table.Column<int>(name: "is_futansya_no_check", type: "integer", nullable: false),
                    isjyukyusyanocheck = table.Column<int>(name: "is_jyukyusya_no_check", type: "integer", nullable: false),
                    istokusyunocheck = table.Column<int>(name: "is_tokusyu_no_check", type: "integer", nullable: false),
                    islimitlist = table.Column<int>(name: "is_limit_list", type: "integer", nullable: false),
                    islimitlistsum = table.Column<int>(name: "is_limit_list_sum", type: "integer", nullable: false),
                    isotherprefvalid = table.Column<int>(name: "is_other_pref_valid", type: "integer", nullable: false),
                    agestart = table.Column<int>(name: "age_start", type: "integer", nullable: false),
                    ageend = table.Column<int>(name: "age_end", type: "integer", nullable: false),
                    enten = table.Column<int>(name: "en_ten", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    recespkbn = table.Column<int>(name: "rece_sp_kbn", type: "integer", nullable: false),
                    receseikyukbn = table.Column<int>(name: "rece_seikyu_kbn", type: "integer", nullable: false),
                    recefutanround = table.Column<int>(name: "rece_futan_round", type: "integer", nullable: false),
                    recekisai = table.Column<int>(name: "rece_kisai", type: "integer", nullable: false),
                    recekisai2 = table.Column<int>(name: "rece_kisai2", type: "integer", nullable: false),
                    recezerokisai = table.Column<int>(name: "rece_zero_kisai", type: "integer", nullable: false),
                    recefutanhide = table.Column<int>(name: "rece_futan_hide", type: "integer", nullable: false),
                    recefutankbn = table.Column<int>(name: "rece_futan_kbn", type: "integer", nullable: false),
                    recetenkisai = table.Column<int>(name: "rece_ten_kisai", type: "integer", nullable: false),
                    kogakutotalkbn = table.Column<int>(name: "kogaku_total_kbn", type: "integer", nullable: false),
                    kogakutotalall = table.Column<int>(name: "kogaku_total_all", type: "integer", nullable: false),
                    calcspkbn = table.Column<int>(name: "calc_sp_kbn", type: "integer", nullable: false),
                    kogakutotalexcfutan = table.Column<int>(name: "kogaku_total_exc_futan", type: "integer", nullable: false),
                    kogakutekiyo = table.Column<int>(name: "kogaku_tekiyo", type: "integer", nullable: false),
                    futanyusen = table.Column<int>(name: "futan_yusen", type: "integer", nullable: false),
                    limitkbn = table.Column<int>(name: "limit_kbn", type: "integer", nullable: false),
                    countkbn = table.Column<int>(name: "count_kbn", type: "integer", nullable: false),
                    futankbn = table.Column<int>(name: "futan_kbn", type: "integer", nullable: false),
                    futanrate = table.Column<int>(name: "futan_rate", type: "integer", nullable: false),
                    kaifutangaku = table.Column<int>(name: "kai_futangaku", type: "integer", nullable: false),
                    kailimitfutan = table.Column<int>(name: "kai_limit_futan", type: "integer", nullable: false),
                    daylimitfutan = table.Column<int>(name: "day_limit_futan", type: "integer", nullable: false),
                    daylimitcount = table.Column<int>(name: "day_limit_count", type: "integer", nullable: false),
                    monthlimitfutan = table.Column<int>(name: "month_limit_futan", type: "integer", nullable: false),
                    monthsplimit = table.Column<int>(name: "month_sp_limit", type: "integer", nullable: false),
                    monthlimitcount = table.Column<int>(name: "month_limit_count", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    recekisaikokho = table.Column<int>(name: "rece_kisai_kokho", type: "integer", nullable: false),
                    kogakuhairyokbn = table.Column<int>(name: "kogaku_hairyo_kbn", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoken_mst", x => new { x.hpid, x.prefno, x.hokenno, x.hokenedano, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "hokensya_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    hokensyano = table.Column<string>(name: "hokensya_no", type: "character varying(8)", maxLength: 8, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    houbetukbn = table.Column<string>(name: "houbetu_kbn", type: "character varying(2)", maxLength: 2, nullable: true),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    kigo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    bango = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    iskigona = table.Column<int>(name: "is_kigo_na", type: "integer", nullable: false),
                    ratehonnin = table.Column<int>(name: "rate_honnin", type: "integer", nullable: false),
                    ratekazoku = table.Column<int>(name: "rate_kazoku", type: "integer", nullable: false),
                    postcode = table.Column<string>(name: "post_code", type: "character varying(7)", maxLength: 7, nullable: true),
                    address1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    tel1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    deletedate = table.Column<int>(name: "delete_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hokensya_mst", x => new { x.hpid, x.hokensyano });
                });

            migrationBuilder.CreateTable(
                name: "holiday_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    holidaykbn = table.Column<int>(name: "holiday_kbn", type: "integer", nullable: false),
                    kyusinkbn = table.Column<int>(name: "kyusin_kbn", type: "integer", nullable: false),
                    holidayname = table.Column<string>(name: "holiday_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holiday_mst", x => new { x.hpid, x.sindate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "hp_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    hpcd = table.Column<string>(name: "hp_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    rousaihpcd = table.Column<string>(name: "rousai_hp_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    hpname = table.Column<string>(name: "hp_name", type: "character varying(80)", maxLength: 80, nullable: true),
                    recehpname = table.Column<string>(name: "rece_hp_name", type: "character varying(80)", maxLength: 80, nullable: true),
                    kaisetuname = table.Column<string>(name: "kaisetu_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    postcd = table.Column<string>(name: "post_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tel = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    faxno = table.Column<string>(name: "fax_no", type: "character varying(15)", maxLength: 15, nullable: true),
                    othercontacts = table.Column<string>(name: "other_contacts", type: "character varying(100)", maxLength: 100, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hp_inf", x => new { x.hpid, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "ipn_kasan_exclude",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ipnnamecd = table.Column<string>(name: "ipn_name_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ipn_kasan_exclude", x => new { x.hpid, x.startdate, x.ipnnamecd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "ipn_kasan_exclude_item",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ipn_kasan_exclude_item", x => new { x.hpid, x.startdate, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "ipn_kasan_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ipnnamecd = table.Column<string>(name: "ipn_name_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    kasan1 = table.Column<int>(type: "integer", nullable: false),
                    kasan2 = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ipn_kasan_mst", x => new { x.hpid, x.startdate, x.ipnnamecd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "ipn_min_yakka_mst",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ipnnamecd = table.Column<string>(name: "ipn_name_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    yakka = table.Column<double>(type: "double precision", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ipn_min_yakka_mst", x => new { x.hpid, x.id, x.ipnnamecd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "ipn_name_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ipnnamecd = table.Column<string>(name: "ipn_name_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ipn_name_mst", x => new { x.hpid, x.ipnnamecd, x.startdate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "item_grp_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpsbt = table.Column<long>(name: "grp_sbt", type: "bigint", nullable: false),
                    itemgrpcd = table.Column<long>(name: "item_grp_cd", type: "bigint", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_grp_mst", x => new { x.hpid, x.grpsbt, x.itemgrpcd, x.seqno, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "jihi_sbt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    jihisbt = table.Column<int>(name: "jihi_sbt", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    isyobo = table.Column<int>(name: "is_yobo", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jihi_sbt_mst", x => new { x.hpid, x.jihisbt });
                });

            migrationBuilder.CreateTable(
                name: "job_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    jobcd = table.Column<int>(name: "job_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jobname = table.Column<string>(name: "job_name", type: "character varying(10)", maxLength: 10, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_mst", x => new { x.jobcd, x.hpid });
                });

            migrationBuilder.CreateTable(
                name: "json_setting",
                columns: table => new
                {
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_json_setting", x => new { x.userid, x.key });
                });

            migrationBuilder.CreateTable(
                name: "ka_mst",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    recekacd = table.Column<string>(name: "rece_ka_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    kasname = table.Column<string>(name: "ka_sname", type: "character varying(20)", maxLength: 20, nullable: true),
                    kaname = table.Column<string>(name: "ka_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    yousikikacd = table.Column<string>(name: "yousiki_ka_cd", type: "character varying(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ka_mst", x => new { x.id, x.hpid });
                });

            migrationBuilder.CreateTable(
                name: "kacode_mst",
                columns: table => new
                {
                    recekacd = table.Column<string>(name: "rece_ka_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    kaname = table.Column<string>(name: "ka_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kacode_mst", x => x.recekacd);
                });

            migrationBuilder.CreateTable(
                name: "kacode_rece_yousiki",
                columns: table => new
                {
                    recekacd = table.Column<string>(name: "rece_ka_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    yousikikacd = table.Column<string>(name: "yousiki_ka_cd", type: "character varying(3)", maxLength: 3, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kacode_rece_yousiki", x => new { x.recekacd, x.yousikikacd });
                });

            migrationBuilder.CreateTable(
                name: "kacode_yousiki_mst",
                columns: table => new
                {
                    yousikikacd = table.Column<string>(name: "yousiki_ka_cd", type: "character varying(3)", maxLength: 3, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    kaname = table.Column<string>(name: "ka_name", type: "character varying(40)", maxLength: 40, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kacode_yousiki_mst", x => x.yousikikacd);
                });

            migrationBuilder.CreateTable(
                name: "kaikei_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    adjustpid = table.Column<int>(name: "adjust_pid", type: "integer", nullable: false),
                    oyaraiinno = table.Column<long>(name: "oya_raiin_no", type: "bigint", nullable: false),
                    adjustkid = table.Column<int>(name: "adjust_kid", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    hokensbtcd = table.Column<int>(name: "hoken_sbt_cd", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    kohi1id = table.Column<int>(name: "kohi1_id", type: "integer", nullable: false),
                    kohi2id = table.Column<int>(name: "kohi2_id", type: "integer", nullable: false),
                    kohi3id = table.Column<int>(name: "kohi3_id", type: "integer", nullable: false),
                    kohi4id = table.Column<int>(name: "kohi4_id", type: "integer", nullable: false),
                    rousaiid = table.Column<int>(name: "rousai_id", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1houbetu = table.Column<string>(name: "kohi1_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi2houbetu = table.Column<string>(name: "kohi2_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi3houbetu = table.Column<string>(name: "kohi3_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi4houbetu = table.Column<string>(name: "kohi4_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1priority = table.Column<string>(name: "kohi1_priority", type: "character varying(8)", maxLength: 8, nullable: true),
                    kohi2priority = table.Column<string>(name: "kohi2_priority", type: "character varying(8)", maxLength: 8, nullable: true),
                    kohi3priority = table.Column<string>(name: "kohi3_priority", type: "character varying(8)", maxLength: 8, nullable: true),
                    kohi4priority = table.Column<string>(name: "kohi4_priority", type: "character varying(8)", maxLength: 8, nullable: true),
                    honkekbn = table.Column<int>(name: "honke_kbn", type: "integer", nullable: false),
                    kogakukbn = table.Column<int>(name: "kogaku_kbn", type: "integer", nullable: false),
                    kogakutekiyokbn = table.Column<int>(name: "kogaku_tekiyo_kbn", type: "integer", nullable: false),
                    istokurei = table.Column<int>(name: "is_tokurei", type: "integer", nullable: false),
                    istasukai = table.Column<int>(name: "is_tasukai", type: "integer", nullable: false),
                    kogakutotalkbn = table.Column<int>(name: "kogaku_total_kbn", type: "integer", nullable: false),
                    ischoki = table.Column<int>(name: "is_choki", type: "integer", nullable: false),
                    kogakulimit = table.Column<int>(name: "kogaku_limit", type: "integer", nullable: false),
                    totalkogakulimit = table.Column<int>(name: "total_kogaku_limit", type: "integer", nullable: false),
                    genmenkbn = table.Column<int>(name: "genmen_kbn", type: "integer", nullable: false),
                    enten = table.Column<int>(name: "en_ten", type: "integer", nullable: false),
                    hokenrate = table.Column<int>(name: "hoken_rate", type: "integer", nullable: false),
                    ptrate = table.Column<int>(name: "pt_rate", type: "integer", nullable: false),
                    kohi1limit = table.Column<int>(name: "kohi1_limit", type: "integer", nullable: false),
                    kohi1otherfutan = table.Column<int>(name: "kohi1_other_futan", type: "integer", nullable: false),
                    kohi2limit = table.Column<int>(name: "kohi2_limit", type: "integer", nullable: false),
                    kohi2otherfutan = table.Column<int>(name: "kohi2_other_futan", type: "integer", nullable: false),
                    kohi3limit = table.Column<int>(name: "kohi3_limit", type: "integer", nullable: false),
                    kohi3otherfutan = table.Column<int>(name: "kohi3_other_futan", type: "integer", nullable: false),
                    kohi4limit = table.Column<int>(name: "kohi4_limit", type: "integer", nullable: false),
                    kohi4otherfutan = table.Column<int>(name: "kohi4_other_futan", type: "integer", nullable: false),
                    tensu = table.Column<int>(type: "integer", nullable: false),
                    totaliryohi = table.Column<int>(name: "total_iryohi", type: "integer", nullable: false),
                    hokenfutan = table.Column<int>(name: "hoken_futan", type: "integer", nullable: false),
                    kogakufutan = table.Column<int>(name: "kogaku_futan", type: "integer", nullable: false),
                    kohi1futan = table.Column<int>(name: "kohi1_futan", type: "integer", nullable: false),
                    kohi2futan = table.Column<int>(name: "kohi2_futan", type: "integer", nullable: false),
                    kohi3futan = table.Column<int>(name: "kohi3_futan", type: "integer", nullable: false),
                    kohi4futan = table.Column<int>(name: "kohi4_futan", type: "integer", nullable: false),
                    ichibufutan = table.Column<int>(name: "ichibu_futan", type: "integer", nullable: false),
                    genmengaku = table.Column<int>(name: "genmen_gaku", type: "integer", nullable: false),
                    hokenfutan10en = table.Column<int>(name: "hoken_futan_10en", type: "integer", nullable: false),
                    kogakufutan10en = table.Column<int>(name: "kogaku_futan_10en", type: "integer", nullable: false),
                    kohi1futan10en = table.Column<int>(name: "kohi1_futan_10en", type: "integer", nullable: false),
                    kohi2futan10en = table.Column<int>(name: "kohi2_futan_10en", type: "integer", nullable: false),
                    kohi3futan10en = table.Column<int>(name: "kohi3_futan_10en", type: "integer", nullable: false),
                    kohi4futan10en = table.Column<int>(name: "kohi4_futan_10en", type: "integer", nullable: false),
                    ichibufutan10en = table.Column<int>(name: "ichibu_futan_10en", type: "integer", nullable: false),
                    genmengaku10en = table.Column<int>(name: "genmen_gaku_10en", type: "integer", nullable: false),
                    adjustround = table.Column<int>(name: "adjust_round", type: "integer", nullable: false),
                    ptfutan = table.Column<int>(name: "pt_futan", type: "integer", nullable: false),
                    kogakuoverkbn = table.Column<int>(name: "kogaku_over_kbn", type: "integer", nullable: false),
                    recesbt = table.Column<string>(name: "rece_sbt", type: "character varying(4)", maxLength: 4, nullable: true),
                    jitunisu = table.Column<int>(type: "integer", nullable: false),
                    rousaiifutan = table.Column<int>(name: "rousai_i_futan", type: "integer", nullable: false),
                    rousairofutan = table.Column<int>(name: "rousai_ro_futan", type: "integer", nullable: false),
                    jibaiitensu = table.Column<int>(name: "jibai_i_tensu", type: "integer", nullable: false),
                    jibairotensu = table.Column<int>(name: "jibai_ro_tensu", type: "integer", nullable: false),
                    jibaihafutan = table.Column<int>(name: "jibai_ha_futan", type: "integer", nullable: false),
                    jibainifutan = table.Column<int>(name: "jibai_ni_futan", type: "integer", nullable: false),
                    jibaihosindan = table.Column<int>(name: "jibai_ho_sindan", type: "integer", nullable: false),
                    jibaihosindancount = table.Column<int>(name: "jibai_ho_sindan_count", type: "integer", nullable: false),
                    jibaihemeisai = table.Column<int>(name: "jibai_he_meisai", type: "integer", nullable: false),
                    jibaihemeisaicount = table.Column<int>(name: "jibai_he_meisai_count", type: "integer", nullable: false),
                    jibaiafutan = table.Column<int>(name: "jibai_a_futan", type: "integer", nullable: false),
                    jibaibfutan = table.Column<int>(name: "jibai_b_futan", type: "integer", nullable: false),
                    jibaicfutan = table.Column<int>(name: "jibai_c_futan", type: "integer", nullable: false),
                    jibaidfutan = table.Column<int>(name: "jibai_d_futan", type: "integer", nullable: false),
                    jibaikenpotensu = table.Column<int>(name: "jibai_kenpo_tensu", type: "integer", nullable: false),
                    jibaikenpofutan = table.Column<int>(name: "jibai_kenpo_futan", type: "integer", nullable: false),
                    jihifutan = table.Column<int>(name: "jihi_futan", type: "integer", nullable: false),
                    jihitax = table.Column<int>(name: "jihi_tax", type: "integer", nullable: false),
                    jihiouttax = table.Column<int>(name: "jihi_outtax", type: "integer", nullable: false),
                    jihifutantaxfree = table.Column<int>(name: "jihi_futan_taxfree", type: "integer", nullable: false),
                    jihifutantaxnr = table.Column<int>(name: "jihi_futan_tax_nr", type: "integer", nullable: false),
                    jihifutantaxgen = table.Column<int>(name: "jihi_futan_tax_gen", type: "integer", nullable: false),
                    jihifutanouttaxnr = table.Column<int>(name: "jihi_futan_outtax_nr", type: "integer", nullable: false),
                    jihifutanouttaxgen = table.Column<int>(name: "jihi_futan_outtax_gen", type: "integer", nullable: false),
                    jihitaxnr = table.Column<int>(name: "jihi_tax_nr", type: "integer", nullable: false),
                    jihitaxgen = table.Column<int>(name: "jihi_tax_gen", type: "integer", nullable: false),
                    jihiouttaxnr = table.Column<int>(name: "jihi_outtax_nr", type: "integer", nullable: false),
                    jihiouttaxgen = table.Column<int>(name: "jihi_outtax_gen", type: "integer", nullable: false),
                    totalptfutan = table.Column<int>(name: "total_pt_futan", type: "integer", nullable: false),
                    sortkey = table.Column<string>(name: "sort_key", type: "character varying(61)", maxLength: 61, nullable: true),
                    isninpu = table.Column<int>(name: "is_ninpu", type: "integer", nullable: false),
                    iszaiiso = table.Column<int>(name: "is_zaiiso", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kaikei_detail", x => new { x.hpid, x.ptid, x.sindate, x.raiinno, x.hokenpid, x.adjustpid });
                });

            migrationBuilder.CreateTable(
                name: "kaikei_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    kohi1id = table.Column<int>(name: "kohi1_id", type: "integer", nullable: false),
                    kohi2id = table.Column<int>(name: "kohi2_id", type: "integer", nullable: false),
                    kohi3id = table.Column<int>(name: "kohi3_id", type: "integer", nullable: false),
                    kohi4id = table.Column<int>(name: "kohi4_id", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    hokensbtcd = table.Column<int>(name: "hoken_sbt_cd", type: "integer", nullable: false),
                    recesbt = table.Column<string>(name: "rece_sbt", type: "character varying(4)", maxLength: 4, nullable: true),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1houbetu = table.Column<string>(name: "kohi1_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi2houbetu = table.Column<string>(name: "kohi2_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi3houbetu = table.Column<string>(name: "kohi3_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi4houbetu = table.Column<string>(name: "kohi4_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    honkekbn = table.Column<int>(name: "honke_kbn", type: "integer", nullable: false),
                    hokenrate = table.Column<int>(name: "hoken_rate", type: "integer", nullable: false),
                    ptrate = table.Column<int>(name: "pt_rate", type: "integer", nullable: false),
                    disprate = table.Column<int>(name: "disp_rate", type: "integer", nullable: false),
                    tensu = table.Column<int>(type: "integer", nullable: false),
                    totaliryohi = table.Column<int>(name: "total_iryohi", type: "integer", nullable: false),
                    ptfutan = table.Column<int>(name: "pt_futan", type: "integer", nullable: false),
                    jihifutan = table.Column<int>(name: "jihi_futan", type: "integer", nullable: false),
                    jihitax = table.Column<int>(name: "jihi_tax", type: "integer", nullable: false),
                    jihiouttax = table.Column<int>(name: "jihi_outtax", type: "integer", nullable: false),
                    jihifutantaxfree = table.Column<int>(name: "jihi_futan_taxfree", type: "integer", nullable: false),
                    jihifutantaxnr = table.Column<int>(name: "jihi_futan_tax_nr", type: "integer", nullable: false),
                    jihifutantaxgen = table.Column<int>(name: "jihi_futan_tax_gen", type: "integer", nullable: false),
                    jihifutanouttaxnr = table.Column<int>(name: "jihi_futan_outtax_nr", type: "integer", nullable: false),
                    jihifutanouttaxgen = table.Column<int>(name: "jihi_futan_outtax_gen", type: "integer", nullable: false),
                    jihitaxnr = table.Column<int>(name: "jihi_tax_nr", type: "integer", nullable: false),
                    jihitaxgen = table.Column<int>(name: "jihi_tax_gen", type: "integer", nullable: false),
                    jihiouttaxnr = table.Column<int>(name: "jihi_outtax_nr", type: "integer", nullable: false),
                    jihiouttaxgen = table.Column<int>(name: "jihi_outtax_gen", type: "integer", nullable: false),
                    adjustfutan = table.Column<int>(name: "adjust_futan", type: "integer", nullable: false),
                    adjustround = table.Column<int>(name: "adjust_round", type: "integer", nullable: false),
                    totalptfutan = table.Column<int>(name: "total_pt_futan", type: "integer", nullable: false),
                    adjustfutanval = table.Column<int>(name: "adjust_futan_val", type: "integer", nullable: false),
                    adjustfutanrange = table.Column<int>(name: "adjust_futan_range", type: "integer", nullable: false),
                    adjustrateval = table.Column<int>(name: "adjust_rate_val", type: "integer", nullable: false),
                    adjustraterange = table.Column<int>(name: "adjust_rate_range", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kaikei_inf", x => new { x.hpid, x.ptid, x.sindate, x.raiinno, x.hokenid });
                });

            migrationBuilder.CreateTable(
                name: "kantoku_mst",
                columns: table => new
                {
                    roudoucd = table.Column<string>(name: "roudou_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    kantokucd = table.Column<string>(name: "kantoku_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    kantokuname = table.Column<string>(name: "kantoku_name", type: "character varying(60)", maxLength: 60, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kantoku_mst", x => new { x.roudoucd, x.kantokucd });
                });

            migrationBuilder.CreateTable(
                name: "karte_filter_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    filterid = table.Column<long>(name: "filter_id", type: "bigint", nullable: false),
                    filteritemcd = table.Column<int>(name: "filter_item_cd", type: "integer", nullable: false),
                    filteredano = table.Column<int>(name: "filter_eda_no", type: "integer", nullable: false),
                    val = table.Column<int>(type: "integer", nullable: false),
                    param = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_karte_filter_detail", x => new { x.hpid, x.userid, x.filterid, x.filteritemcd, x.filteredano });
                });

            migrationBuilder.CreateTable(
                name: "karte_filter_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    filterid = table.Column<long>(name: "filter_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    filtername = table.Column<string>(name: "filter_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    autoapply = table.Column<int>(name: "auto_apply", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_karte_filter_mst", x => new { x.hpid, x.userid, x.filterid });
                });

            migrationBuilder.CreateTable(
                name: "karte_img_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    position = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_karte_img_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "karte_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    richtext = table.Column<byte[]>(name: "rich_text", type: "bytea", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_karte_inf", x => new { x.hpid, x.raiinno, x.seqno, x.kartekbn });
                });

            migrationBuilder.CreateTable(
                name: "karte_kbn_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    kbnname = table.Column<string>(name: "kbn_name", type: "character varying(10)", maxLength: 10, nullable: true),
                    kbnshortname = table.Column<string>(name: "kbn_short_name", type: "character varying(1)", maxLength: 1, nullable: true),
                    canimg = table.Column<int>(name: "can_img", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_karte_kbn_mst", x => new { x.hpid, x.kartekbn });
                });

            migrationBuilder.CreateTable(
                name: "kensa_center_mst",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    centercd = table.Column<string>(name: "center_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    centername = table.Column<string>(name: "center_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    primarykbn = table.Column<int>(name: "primary_kbn", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_center_mst", x => new { x.hpid, x.id });
                });

            migrationBuilder.CreateTable(
                name: "kensa_cmt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", maxLength: 2, nullable: false),
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(3)", maxLength: 3, nullable: false),
                    cmtseqno = table.Column<int>(name: "cmt_seq_no", type: "integer", maxLength: 9, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    centercd = table.Column<string>(name: "center_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", maxLength: 1, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", maxLength: 8, nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", maxLength: 8, nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_cmt_mst", x => new { x.hpid, x.cmtcd, x.cmtseqno });
                });

            migrationBuilder.CreateTable(
                name: "kensa_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    iraicd = table.Column<long>(name: "irai_cd", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iraidate = table.Column<int>(name: "irai_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    resultcheck = table.Column<int>(name: "result_check", type: "integer", nullable: false),
                    centercd = table.Column<string>(name: "center_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    nyubi = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    yoketu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    bilirubin = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_inf", x => new { x.hpid, x.ptid, x.iraicd });
                });

            migrationBuilder.CreateTable(
                name: "kensa_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    iraicd = table.Column<long>(name: "irai_cd", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iraidate = table.Column<int>(name: "irai_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    resultval = table.Column<string>(name: "result_val", type: "character varying(10)", maxLength: 10, nullable: true),
                    resulttype = table.Column<string>(name: "result_type", type: "character varying(1)", maxLength: 1, nullable: true),
                    abnormalkbn = table.Column<string>(name: "abnormal_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    cmtcd1 = table.Column<string>(name: "cmt_cd1", type: "character varying(3)", maxLength: 3, nullable: true),
                    cmtcd2 = table.Column<string>(name: "cmt_cd2", type: "character varying(3)", maxLength: 3, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    seqparentno = table.Column<long>(name: "seq_parent_no", type: "bigint", nullable: false),
                    seqgroupno = table.Column<long>(name: "seq_group_no", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_inf_detail", x => new { x.hpid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "kensa_irai_log",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    centercd = table.Column<string>(name: "center_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    iraidate = table.Column<int>(name: "irai_date", type: "integer", nullable: false),
                    fromdate = table.Column<int>(name: "from_date", type: "integer", nullable: false),
                    todate = table.Column<int>(name: "to_date", type: "integer", nullable: false),
                    iraifile = table.Column<string>(name: "irai_file", type: "text", nullable: true),
                    irailist = table.Column<byte[]>(name: "irai_list", type: "bytea", nullable: true),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_irai_log", x => new { x.hpid, x.centercd, x.createdate });
                });

            migrationBuilder.CreateTable(
                name: "kensa_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    kensaitemseqno = table.Column<int>(name: "kensa_item_seq_no", type: "integer", nullable: false),
                    centercd = table.Column<string>(name: "center_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    kensaname = table.Column<string>(name: "kensa_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    kensakana = table.Column<string>(name: "kensa_kana", type: "character varying(20)", maxLength: 20, nullable: true),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    materialcd = table.Column<int>(name: "material_cd", type: "integer", nullable: false),
                    containercd = table.Column<int>(name: "container_cd", type: "integer", nullable: false),
                    malestd = table.Column<string>(name: "male_std", type: "character varying(60)", maxLength: 60, nullable: true),
                    malestdlow = table.Column<string>(name: "male_std_low", type: "character varying(60)", maxLength: 60, nullable: true),
                    malestdhigh = table.Column<string>(name: "male_std_high", type: "character varying(60)", maxLength: 60, nullable: true),
                    femalestd = table.Column<string>(name: "female_std", type: "character varying(60)", maxLength: 60, nullable: true),
                    femalestdlow = table.Column<string>(name: "female_std_low", type: "character varying(60)", maxLength: 60, nullable: true),
                    femalestdhigh = table.Column<string>(name: "female_std_high", type: "character varying(60)", maxLength: 60, nullable: true),
                    formula = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    digit = table.Column<int>(type: "integer", nullable: false),
                    oyaitemcd = table.Column<string>(name: "oya_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    oyaitemseqno = table.Column<int>(name: "oya_item_seq_no", type: "integer", nullable: false),
                    sortno = table.Column<long>(name: "sort_no", type: "bigint", nullable: false),
                    centeritemcd1 = table.Column<string>(name: "center_item_cd1", type: "character varying(10)", maxLength: 10, nullable: true),
                    centeritemcd2 = table.Column<string>(name: "center_item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_mst", x => new { x.hpid, x.kensaitemcd, x.kensaitemseqno });
                });

            migrationBuilder.CreateTable(
                name: "kensa_result_log",
                columns: table => new
                {
                    opid = table.Column<int>(name: "op_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    impdate = table.Column<int>(name: "imp_date", type: "integer", nullable: false),
                    kekafile = table.Column<string>(name: "keka_file", type: "text", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_result_log", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "kensa_set",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", maxLength: 2, nullable: false),
                    setid = table.Column<int>(name: "set_id", type: "integer", maxLength: 9, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    setname = table.Column<string>(name: "set_name", type: "character varying(30)", maxLength: 30, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", maxLength: 9, nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", maxLength: 1, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", maxLength: 8, nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", maxLength: 8, nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_set", x => new { x.hpid, x.setid });
                });

            migrationBuilder.CreateTable(
                name: "kensa_set_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", maxLength: 2, nullable: false),
                    setid = table.Column<int>(name: "set_id", type: "integer", maxLength: 9, nullable: false),
                    setedano = table.Column<int>(name: "set_eda_no", type: "integer", maxLength: 9, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seqparentno = table.Column<int>(name: "seq_parent_no", type: "integer", maxLength: 9, nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    kensaitemseqno = table.Column<int>(name: "kensa_item_seq_no", type: "integer", maxLength: 2, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", maxLength: 9, nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", maxLength: 1, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", maxLength: 8, nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", maxLength: 8, nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_set_detail", x => new { x.hpid, x.setid, x.setedano });
                });

            migrationBuilder.CreateTable(
                name: "kensa_std_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    malestd = table.Column<string>(name: "male_std", type: "character varying(60)", maxLength: 60, nullable: true),
                    malestdlow = table.Column<string>(name: "male_std_low", type: "character varying(60)", maxLength: 60, nullable: true),
                    malestdhigh = table.Column<string>(name: "male_std_high", type: "character varying(60)", maxLength: 60, nullable: true),
                    femalestd = table.Column<string>(name: "female_std", type: "character varying(60)", maxLength: 60, nullable: true),
                    femalestdlow = table.Column<string>(name: "female_std_low", type: "character varying(60)", maxLength: 60, nullable: true),
                    femalestdhigh = table.Column<string>(name: "female_std_high", type: "character varying(60)", maxLength: 60, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kensa_std_mst", x => new { x.hpid, x.kensaitemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "kinki_mst",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    acd = table.Column<string>(name: "a_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    bcd = table.Column<string>(name: "b_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kinki_mst", x => new { x.hpid, x.id, x.acd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "kogaku_limit",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    agekbn = table.Column<int>(name: "age_kbn", type: "integer", nullable: false),
                    kogakukbn = table.Column<int>(name: "kogaku_kbn", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    incomekbn = table.Column<string>(name: "income_kbn", type: "character varying(20)", maxLength: 20, nullable: true),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    baselimit = table.Column<int>(name: "base_limit", type: "integer", nullable: false),
                    adjustlimit = table.Column<int>(name: "adjust_limit", type: "integer", nullable: false),
                    tasulimit = table.Column<int>(name: "tasu_limit", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kogaku_limit", x => new { x.hpid, x.agekbn, x.kogakukbn, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "kohi_priority",
                columns: table => new
                {
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    priorityno = table.Column<string>(name: "priority_no", type: "character varying(5)", maxLength: 5, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kohi_priority", x => new { x.prefno, x.houbetu, x.priorityno });
                });

            migrationBuilder.CreateTable(
                name: "koui_houkatu_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    houkatuterm = table.Column<int>(name: "houkatu_term", type: "integer", nullable: false),
                    kouifrom = table.Column<int>(name: "koui_from", type: "integer", nullable: false),
                    kouito = table.Column<int>(name: "koui_to", type: "integer", nullable: false),
                    ignoresanteikbn = table.Column<int>(name: "ignore_santei_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koui_houkatu_mst", x => new { x.hpid, x.itemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "koui_kbn_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    kouikbnid = table.Column<int>(name: "koui_kbn_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    kouikbn1 = table.Column<int>(name: "koui_kbn1", type: "integer", nullable: false),
                    kouikbn2 = table.Column<int>(name: "koui_kbn2", type: "integer", nullable: false),
                    kouigrpname = table.Column<string>(name: "koui_grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    kouiname = table.Column<string>(name: "koui_name", type: "character varying(20)", maxLength: 20, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    exckouikbn = table.Column<int>(name: "exc_koui_kbn", type: "integer", nullable: false),
                    oyakouikbnid = table.Column<int>(name: "oya_koui_kbn_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koui_kbn_mst", x => new { x.hpid, x.kouikbnid });
                });

            migrationBuilder.CreateTable(
                name: "limit_cnt_list_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    kohiid = table.Column<int>(name: "kohi_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    sortkey = table.Column<string>(name: "sort_key", type: "character varying(61)", maxLength: 61, nullable: true),
                    oyaraiinno = table.Column<long>(name: "oya_raiin_no", type: "bigint", nullable: false),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_limit_cnt_list_inf", x => new { x.hpid, x.ptid, x.kohiid, x.sindate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "limit_list_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    kohiid = table.Column<int>(name: "kohi_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    sortkey = table.Column<string>(name: "sort_key", type: "character varying(61)", maxLength: 61, nullable: true),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    futangaku = table.Column<int>(name: "futan_gaku", type: "integer", nullable: false),
                    totalgaku = table.Column<int>(name: "total_gaku", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_limit_list_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "list_set_generation_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_set_generation_mst", x => new { x.hpid, x.generationid });
                });

            migrationBuilder.CreateTable(
                name: "list_set_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false),
                    setid = table.Column<int>(name: "set_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    setkbn = table.Column<int>(name: "set_kbn", type: "integer", nullable: false),
                    level1 = table.Column<int>(type: "integer", nullable: false),
                    level2 = table.Column<int>(type: "integer", nullable: false),
                    level3 = table.Column<int>(type: "integer", nullable: false),
                    level4 = table.Column<int>(type: "integer", nullable: false),
                    level5 = table.Column<int>(type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    setname = table.Column<string>(name: "set_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    istitle = table.Column<int>(name: "is_title", type: "integer", nullable: false),
                    selecttype = table.Column<int>(name: "select_type", type: "integer", nullable: false),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    unitsbt = table.Column<int>(name: "unit_sbt", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_set_mst", x => new { x.hpid, x.generationid, x.setid });
                });

            migrationBuilder.CreateTable(
                name: "lock_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    functioncd = table.Column<string>(name: "function_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    sindate = table.Column<long>(name: "sin_date", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    oyaraiinno = table.Column<long>(name: "oya_raiin_no", type: "bigint", nullable: false),
                    machine = table.Column<string>(type: "text", nullable: true),
                    loginkey = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    lockdate = table.Column<DateTime>(name: "lock_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lock_inf", x => new { x.hpid, x.ptid, x.functioncd, x.sindate, x.raiinno, x.oyaraiinno });
                });

            migrationBuilder.CreateTable(
                name: "lock_mst",
                columns: table => new
                {
                    functioncda = table.Column<string>(name: "function_cd_a", type: "character varying(8)", maxLength: 8, nullable: false),
                    functioncdb = table.Column<string>(name: "function_cd_b", type: "character varying(8)", maxLength: 8, nullable: false),
                    lockrange = table.Column<int>(name: "lock_range", type: "integer", nullable: false),
                    locklevel = table.Column<int>(name: "lock_level", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lock_mst", x => new { x.functioncda, x.functioncdb });
                });

            migrationBuilder.CreateTable(
                name: "m01_kijyo_cmt",
                columns: table => new
                {
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(6)", maxLength: 6, nullable: false),
                    cmt = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m01_kijyo_cmt", x => x.cmtcd);
                });

            migrationBuilder.CreateTable(
                name: "m01_kinki",
                columns: table => new
                {
                    acd = table.Column<string>(name: "a_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    bcd = table.Column<string>(name: "b_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(6)", maxLength: 6, nullable: false),
                    sayokijyocd = table.Column<string>(name: "sayokijyo_cd", type: "character varying(6)", maxLength: 6, nullable: false),
                    kyodocd = table.Column<string>(name: "kyodo_cd", type: "character varying(3)", maxLength: 3, nullable: true),
                    kyodo = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    datakbn = table.Column<string>(name: "data_kbn", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m01_kinki", x => new { x.acd, x.bcd, x.cmtcd, x.sayokijyocd });
                });

            migrationBuilder.CreateTable(
                name: "m01_kinki_cmt",
                columns: table => new
                {
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(6)", maxLength: 6, nullable: false),
                    cmt = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m01_kinki_cmt", x => x.cmtcd);
                });

            migrationBuilder.CreateTable(
                name: "m10_day_limit",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    limitday = table.Column<int>(name: "limit_day", type: "integer", nullable: false),
                    stdate = table.Column<string>(name: "st_date", type: "character varying(8)", maxLength: 8, nullable: true),
                    eddate = table.Column<string>(name: "ed_date", type: "character varying(8)", maxLength: 8, nullable: true),
                    cmt = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m10_day_limit", x => new { x.yjcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "m12_food_alrgy",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    foodkbn = table.Column<string>(name: "food_kbn", type: "character varying(2)", maxLength: 2, nullable: false),
                    tenpulevel = table.Column<string>(name: "tenpu_level", type: "character varying(2)", maxLength: 2, nullable: false),
                    kikincd = table.Column<string>(name: "kikin_cd", type: "character varying(9)", maxLength: 9, nullable: true),
                    attentioncmt = table.Column<string>(name: "attention_cmt", type: "character varying(500)", maxLength: 500, nullable: true),
                    workingmechanism = table.Column<string>(name: "working_mechanism", type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m12_food_alrgy", x => new { x.yjcd, x.foodkbn, x.tenpulevel });
                });

            migrationBuilder.CreateTable(
                name: "m12_food_alrgy_kbn",
                columns: table => new
                {
                    foodkbn = table.Column<string>(name: "food_kbn", type: "character varying(2)", maxLength: 2, nullable: false),
                    foodname = table.Column<string>(name: "food_name", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m12_food_alrgy_kbn", x => x.foodkbn);
                });

            migrationBuilder.CreateTable(
                name: "m14_age_check",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    attentioncmtcd = table.Column<string>(name: "attention_cmt_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    workingmechanism = table.Column<string>(name: "working_mechanism", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    tenpulevel = table.Column<string>(name: "tenpu_level", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekbn = table.Column<string>(name: "age_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    weightkbn = table.Column<string>(name: "weight_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    sexkbn = table.Column<string>(name: "sex_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    agemin = table.Column<double>(name: "age_min", type: "double precision", nullable: false),
                    agemax = table.Column<double>(name: "age_max", type: "double precision", nullable: false),
                    weightmin = table.Column<double>(name: "weight_min", type: "double precision", nullable: false),
                    weightmax = table.Column<double>(name: "weight_max", type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m14_age_check", x => new { x.yjcd, x.attentioncmtcd });
                });

            migrationBuilder.CreateTable(
                name: "m14_cmt_code",
                columns: table => new
                {
                    attentioncmtcd = table.Column<string>(name: "attention_cmt_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    attentioncmt = table.Column<string>(name: "attention_cmt", type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m14_cmt_code", x => x.attentioncmtcd);
                });

            migrationBuilder.CreateTable(
                name: "m28_drug_mst",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    koseisyocd = table.Column<string>(name: "koseisyo_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    kikincd = table.Column<string>(name: "kikin_cd", type: "character varying(9)", maxLength: 9, nullable: true),
                    drugname = table.Column<string>(name: "drug_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    drugkana1 = table.Column<string>(name: "drug_kana1", type: "character varying(100)", maxLength: 100, nullable: true),
                    drugkana2 = table.Column<string>(name: "drug_kana2", type: "character varying(100)", maxLength: 100, nullable: true),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(400)", maxLength: 400, nullable: true),
                    ipnkana = table.Column<string>(name: "ipn_kana", type: "character varying(100)", maxLength: 100, nullable: true),
                    yakkaval = table.Column<int>(name: "yakka_val", type: "integer", nullable: false),
                    yakkaunit = table.Column<string>(name: "yakka_unit", type: "character varying(20)", maxLength: 20, nullable: true),
                    seibunrikika = table.Column<double>(name: "seibun_rikika", type: "double precision", nullable: false),
                    seibunrikikaunit = table.Column<string>(name: "seibun_rikika_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    yoryojyuryo = table.Column<double>(name: "yoryo_jyuryo", type: "double precision", nullable: false),
                    yoryojyuryounit = table.Column<string>(name: "yoryo_jyuryo_unit", type: "character varying(20)", maxLength: 20, nullable: true),
                    seirikiyoryorate = table.Column<double>(name: "seiriki_yoryo_rate", type: "double precision", nullable: false),
                    seirikiyoryounit = table.Column<string>(name: "seiriki_yoryo_unit", type: "character varying(40)", maxLength: 40, nullable: true),
                    makercd = table.Column<string>(name: "maker_cd", type: "character varying(4)", maxLength: 4, nullable: true),
                    makername = table.Column<string>(name: "maker_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    drugkbncd = table.Column<string>(name: "drug_kbn_cd", type: "character varying(1)", maxLength: 1, nullable: true),
                    drugkbn = table.Column<string>(name: "drug_kbn", type: "character varying(10)", maxLength: 10, nullable: true),
                    formkbncd = table.Column<string>(name: "form_kbn_cd", type: "character varying(3)", maxLength: 3, nullable: true),
                    formkbn = table.Column<string>(name: "form_kbn", type: "character varying(100)", maxLength: 100, nullable: true),
                    dokuyakuflg = table.Column<string>(name: "dokuyaku_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    gekiyakuflg = table.Column<string>(name: "gekiyaku_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    mayakuflg = table.Column<string>(name: "mayaku_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    koseisinyakuflg = table.Column<string>(name: "koseisinyaku_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    kakuseizaiflg = table.Column<string>(name: "kakuseizai_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    kakuseizaigenryoflg = table.Column<string>(name: "kakuseizai_genryo_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    seibutuflg = table.Column<string>(name: "seibutu_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    spseibutuflg = table.Column<string>(name: "sp_seibutu_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    kohatuflg = table.Column<string>(name: "kohatu_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    yakka = table.Column<double>(type: "double precision", nullable: false),
                    kikakuunit = table.Column<string>(name: "kikaku_unit", type: "character varying(100)", maxLength: 100, nullable: true),
                    yakkarateformura = table.Column<string>(name: "yakka_rate_formura", type: "character varying(30)", maxLength: 30, nullable: true),
                    yakkarateunit = table.Column<string>(name: "yakka_rate_unit", type: "character varying(40)", maxLength: 40, nullable: true),
                    yakkasyusaidate = table.Column<string>(name: "yakka_syusai_date", type: "character varying(8)", maxLength: 8, nullable: true),
                    keikasotidate = table.Column<string>(name: "keikasoti_date", type: "character varying(8)", maxLength: 8, nullable: true),
                    maindrugcd = table.Column<string>(name: "main_drug_cd", type: "character varying(8)", maxLength: 8, nullable: true),
                    maindrugname = table.Column<string>(name: "main_drug_name", type: "character varying(400)", maxLength: 400, nullable: true),
                    maindrugkana = table.Column<string>(name: "main_drug_kana", type: "character varying(400)", maxLength: 400, nullable: true),
                    keyseibun = table.Column<string>(name: "key_seibun", type: "character varying(200)", maxLength: 200, nullable: true),
                    haigoflg = table.Column<string>(name: "haigo_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    maindrugnameflg = table.Column<string>(name: "main_drug_name_flg", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m28_drug_mst", x => x.yjcd);
                });

            migrationBuilder.CreateTable(
                name: "m34_ar_code",
                columns: table => new
                {
                    fukusayocd = table.Column<string>(name: "fukusayo_cd", type: "text", nullable: false),
                    fukusayocmt = table.Column<string>(name: "fukusayo_cmt", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_ar_code", x => x.fukusayocd);
                });

            migrationBuilder.CreateTable(
                name: "m34_ar_discon",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "text", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    fukusayocd = table.Column<string>(name: "fukusayo_cd", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_ar_discon", x => new { x.yjcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "m34_ar_discon_code",
                columns: table => new
                {
                    fukusayocd = table.Column<string>(name: "fukusayo_cd", type: "character varying(6)", maxLength: 6, nullable: false),
                    fukusayocmt = table.Column<string>(name: "fukusayo_cmt", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_ar_discon_code", x => x.fukusayocd);
                });

            migrationBuilder.CreateTable(
                name: "m34_drug_info_main",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "text", nullable: false),
                    formcd = table.Column<string>(name: "form_cd", type: "text", nullable: true),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    mark = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    konocd = table.Column<string>(name: "kono_cd", type: "text", nullable: true),
                    fukusayocd = table.Column<string>(name: "fukusayo_cd", type: "text", nullable: true),
                    fukusayoinitcd = table.Column<string>(name: "fukusayo_init_cd", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_drug_info_main", x => x.yjcd);
                });

            migrationBuilder.CreateTable(
                name: "m34_form_code",
                columns: table => new
                {
                    formcd = table.Column<string>(name: "form_cd", type: "character varying(4)", maxLength: 4, nullable: false),
                    form = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_form_code", x => x.formcd);
                });

            migrationBuilder.CreateTable(
                name: "m34_indication_code",
                columns: table => new
                {
                    konocd = table.Column<string>(name: "kono_cd", type: "text", nullable: false),
                    konodetailcmt = table.Column<string>(name: "kono_detail_cmt", type: "character varying(200)", maxLength: 200, nullable: true),
                    konosimplecmt = table.Column<string>(name: "kono_simple_cmt", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_indication_code", x => x.konocd);
                });

            migrationBuilder.CreateTable(
                name: "m34_interaction_pat",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "text", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    interactionpatcd = table.Column<string>(name: "interaction_pat_cd", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_interaction_pat", x => new { x.yjcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "m34_interaction_pat_code",
                columns: table => new
                {
                    interactionpatcd = table.Column<string>(name: "interaction_pat_cd", type: "text", nullable: false),
                    interactionpatcmt = table.Column<string>(name: "interaction_pat_cmt", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_interaction_pat_code", x => x.interactionpatcd);
                });

            migrationBuilder.CreateTable(
                name: "m34_precaution_code",
                columns: table => new
                {
                    precautioncd = table.Column<string>(name: "precaution_cd", type: "text", nullable: false),
                    extendcd = table.Column<string>(name: "extend_cd", type: "text", nullable: false),
                    precautioncmt = table.Column<string>(name: "precaution_cmt", type: "character varying(200)", maxLength: 200, nullable: true),
                    propertycd = table.Column<int>(name: "property_cd", type: "integer", nullable: false),
                    agemax = table.Column<int>(name: "age_max", type: "integer", nullable: false),
                    agemin = table.Column<int>(name: "age_min", type: "integer", nullable: false),
                    sexcd = table.Column<string>(name: "sex_cd", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_precaution_code", x => new { x.precautioncd, x.extendcd });
                });

            migrationBuilder.CreateTable(
                name: "m34_precautions",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "text", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    precautioncd = table.Column<string>(name: "precaution_cd", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_precautions", x => new { x.yjcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "m34_property_code",
                columns: table => new
                {
                    propertycd = table.Column<int>(name: "property_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_property_code", x => x.propertycd);
                });

            migrationBuilder.CreateTable(
                name: "m34_sar_symptom_code",
                columns: table => new
                {
                    fukusayoinitcd = table.Column<string>(name: "fukusayo_init_cd", type: "character varying(6)", maxLength: 6, nullable: false),
                    fukusayoinitcmt = table.Column<string>(name: "fukusayo_init_cmt", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m34_sar_symptom_code", x => x.fukusayoinitcd);
                });

            migrationBuilder.CreateTable(
                name: "m38_class_code",
                columns: table => new
                {
                    classcd = table.Column<string>(name: "class_cd", type: "text", nullable: false),
                    classname = table.Column<string>(name: "class_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    majordivcd = table.Column<string>(name: "major_div_cd", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_class_code", x => x.classcd);
                });

            migrationBuilder.CreateTable(
                name: "m38_ing_code",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    seibun = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_ing_code", x => x.seibuncd);
                });

            migrationBuilder.CreateTable(
                name: "m38_ingredients",
                columns: table => new
                {
                    serialnum = table.Column<int>(name: "serial_num", type: "integer", nullable: false),
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    sbt = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_ingredients", x => new { x.seibuncd, x.serialnum, x.sbt });
                });

            migrationBuilder.CreateTable(
                name: "m38_major_div_code",
                columns: table => new
                {
                    majordivcd = table.Column<string>(name: "major_div_cd", type: "text", nullable: false),
                    majordivname = table.Column<string>(name: "major_div_name", type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_major_div_code", x => x.majordivcd);
                });

            migrationBuilder.CreateTable(
                name: "m38_otc_form_code",
                columns: table => new
                {
                    formcd = table.Column<string>(name: "form_cd", type: "text", nullable: false),
                    form = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_otc_form_code", x => x.formcd);
                });

            migrationBuilder.CreateTable(
                name: "m38_otc_main",
                columns: table => new
                {
                    serialnum = table.Column<int>(name: "serial_num", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    otccd = table.Column<string>(name: "otc_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    tradename = table.Column<string>(name: "trade_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    tradekana = table.Column<string>(name: "trade_kana", type: "character varying(400)", maxLength: 400, nullable: true),
                    classcd = table.Column<string>(name: "class_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    companycd = table.Column<string>(name: "company_cd", type: "character varying(4)", maxLength: 4, nullable: true),
                    tradecd = table.Column<string>(name: "trade_cd", type: "character varying(3)", maxLength: 3, nullable: true),
                    drugformcd = table.Column<string>(name: "drug_form_cd", type: "character varying(6)", maxLength: 6, nullable: true),
                    yohocd = table.Column<string>(name: "yoho_cd", type: "character varying(6)", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_otc_main", x => x.serialnum);
                });

            migrationBuilder.CreateTable(
                name: "m38_otc_maker_code",
                columns: table => new
                {
                    makercd = table.Column<string>(name: "maker_cd", type: "text", nullable: false),
                    makername = table.Column<string>(name: "maker_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    makerkana = table.Column<string>(name: "maker_kana", type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m38_otc_maker_code", x => x.makercd);
                });

            migrationBuilder.CreateTable(
                name: "m41_supple_indexcode",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    indexcd = table.Column<string>(name: "index_cd", type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m41_supple_indexcode", x => new { x.seibuncd, x.indexcd });
                });

            migrationBuilder.CreateTable(
                name: "m41_supple_indexdef",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    indexword = table.Column<string>(name: "index_word", type: "character varying(200)", maxLength: 200, nullable: true),
                    tokuhoflg = table.Column<string>(name: "tokuho_flg", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m41_supple_indexdef", x => x.seibuncd);
                });

            migrationBuilder.CreateTable(
                name: "m41_supple_ingre",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    seibun = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m41_supple_ingre", x => x.seibuncd);
                });

            migrationBuilder.CreateTable(
                name: "m42_contra_cmt",
                columns: table => new
                {
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    cmt = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m42_contra_cmt", x => x.cmtcd);
                });

            migrationBuilder.CreateTable(
                name: "m42_contraindi_dis_bc",
                columns: table => new
                {
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    byotaiclasscd = table.Column<string>(name: "byotai_class_cd", type: "character varying(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m42_contraindi_dis_bc", x => new { x.byotaicd, x.byotaiclasscd });
                });

            migrationBuilder.CreateTable(
                name: "m42_contraindi_dis_class",
                columns: table => new
                {
                    byotaiclasscd = table.Column<string>(name: "byotai_class_cd", type: "character varying(4)", maxLength: 4, nullable: false),
                    byotai = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m42_contraindi_dis_class", x => x.byotaiclasscd);
                });

            migrationBuilder.CreateTable(
                name: "m42_contraindi_dis_con",
                columns: table => new
                {
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    standardbyotai = table.Column<string>(name: "standard_byotai", type: "character varying(400)", maxLength: 400, nullable: true),
                    byotaikbn = table.Column<int>(name: "byotai_kbn", type: "integer", nullable: false),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    icd10 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    rececd = table.Column<string>(name: "rece_cd", type: "character varying(33)", maxLength: 33, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m42_contraindi_dis_con", x => x.byotaicd);
                });

            migrationBuilder.CreateTable(
                name: "m42_contraindi_drug_main_ex",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    tenpulevel = table.Column<int>(name: "tenpu_level", type: "integer", nullable: false),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    stage = table.Column<int>(type: "integer", nullable: false),
                    kiocd = table.Column<string>(name: "kio_cd", type: "character varying(1)", maxLength: 1, nullable: true),
                    familycd = table.Column<string>(name: "family_cd", type: "character varying(1)", maxLength: 1, nullable: true),
                    kijyocd = table.Column<string>(name: "kijyo_cd", type: "character varying(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m42_contraindi_drug_main_ex", x => new { x.yjcd, x.tenpulevel, x.byotaicd, x.cmtcd });
                });

            migrationBuilder.CreateTable(
                name: "m46_dosage_dosage",
                columns: table => new
                {
                    doeicd = table.Column<string>(name: "doei_cd", type: "text", nullable: false),
                    doeiseqno = table.Column<int>(name: "doei_seq_no", type: "integer", nullable: false),
                    konokokacd = table.Column<string>(name: "konokoka_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    kensapcd = table.Column<string>(name: "kensa_pcd", type: "character varying(7)", maxLength: 7, nullable: true),
                    ageover = table.Column<double>(name: "age_over", type: "double precision", nullable: false),
                    ageunder = table.Column<double>(name: "age_under", type: "double precision", nullable: false),
                    agecd = table.Column<string>(name: "age_cd", type: "character varying(1)", maxLength: 1, nullable: true),
                    weightover = table.Column<double>(name: "weight_over", type: "double precision", nullable: false),
                    weightunder = table.Column<double>(name: "weight_under", type: "double precision", nullable: false),
                    bodyover = table.Column<double>(name: "body_over", type: "double precision", nullable: false),
                    bodyunder = table.Column<double>(name: "body_under", type: "double precision", nullable: false),
                    drugroute = table.Column<string>(name: "drug_route", type: "character varying(40)", maxLength: 40, nullable: true),
                    useflg = table.Column<string>(name: "use_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    drugcondition = table.Column<string>(name: "drug_condition", type: "character varying(400)", maxLength: 400, nullable: true),
                    konokoka = table.Column<string>(type: "text", nullable: true),
                    usagedosage = table.Column<string>(name: "usage_dosage", type: "text", nullable: true),
                    filenamecd = table.Column<string>(name: "filename_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    drugsyugi = table.Column<string>(name: "drug_syugi", type: "text", nullable: true),
                    tekiobui = table.Column<string>(name: "tekio_bui", type: "character varying(300)", maxLength: 300, nullable: true),
                    youkaikisyaku = table.Column<string>(name: "youkai_kisyaku", type: "character varying(1500)", maxLength: 1500, nullable: true),
                    kisyakueki = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    youkaieki = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    haitaflg = table.Column<string>(name: "haita_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    ngkisyakueki = table.Column<string>(name: "ng_kisyakueki", type: "character varying(500)", maxLength: 500, nullable: true),
                    ngyoukaieki = table.Column<string>(name: "ng_youkaieki", type: "character varying(500)", maxLength: 500, nullable: true),
                    combidrug = table.Column<string>(name: "combi_drug", type: "character varying(200)", maxLength: 200, nullable: true),
                    druglinkcd = table.Column<int>(name: "drug_link_cd", type: "integer", nullable: false),
                    drugorder = table.Column<int>(name: "drug_order", type: "integer", nullable: false),
                    singledrugflg = table.Column<string>(name: "single_drug_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    kyugencd = table.Column<string>(name: "kyugen_cd", type: "character varying(1)", maxLength: 1, nullable: true),
                    dosagecheckflg = table.Column<string>(name: "dosage_check_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    oncemin = table.Column<double>(name: "once_min", type: "double precision", nullable: false),
                    oncemax = table.Column<double>(name: "once_max", type: "double precision", nullable: false),
                    onceunit = table.Column<string>(name: "once_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    oncelimit = table.Column<double>(name: "once_limit", type: "double precision", nullable: false),
                    oncelimitunit = table.Column<string>(name: "once_limit_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    daymincnt = table.Column<double>(name: "day_min_cnt", type: "double precision", nullable: false),
                    daymaxcnt = table.Column<double>(name: "day_max_cnt", type: "double precision", nullable: false),
                    daymin = table.Column<double>(name: "day_min", type: "double precision", nullable: false),
                    daymax = table.Column<double>(name: "day_max", type: "double precision", nullable: false),
                    dayunit = table.Column<string>(name: "day_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    daylimit = table.Column<double>(name: "day_limit", type: "double precision", nullable: false),
                    daylimitunit = table.Column<string>(name: "day_limit_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    rise = table.Column<int>(type: "integer", nullable: false),
                    morning = table.Column<int>(type: "integer", nullable: false),
                    daytime = table.Column<int>(type: "integer", nullable: false),
                    night = table.Column<int>(type: "integer", nullable: false),
                    sleep = table.Column<int>(type: "integer", nullable: false),
                    beforemeal = table.Column<int>(name: "before_meal", type: "integer", nullable: false),
                    justbeforemeal = table.Column<int>(name: "just_before_meal", type: "integer", nullable: false),
                    aftermeal = table.Column<int>(name: "after_meal", type: "integer", nullable: false),
                    justaftermeal = table.Column<int>(name: "just_after_meal", type: "integer", nullable: false),
                    betweenmeal = table.Column<int>(name: "between_meal", type: "integer", nullable: false),
                    elsetime = table.Column<int>(name: "else_time", type: "integer", nullable: false),
                    dosagelimitterm = table.Column<int>(name: "dosage_limit_term", type: "integer", nullable: false),
                    dosagelimitunit = table.Column<string>(name: "dosage_limit_unit", type: "character varying(1)", maxLength: 1, nullable: true),
                    unittermlimit = table.Column<double>(name: "unitterm_limit", type: "double precision", nullable: false),
                    unittermunit = table.Column<string>(name: "unitterm_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    dosageaddflg = table.Column<string>(name: "dosage_add_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    incdecflg = table.Column<string>(name: "inc_dec_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    decflg = table.Column<string>(name: "dec_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    incdecinterval = table.Column<int>(name: "inc_dec_interval", type: "integer", nullable: false),
                    incdecintervalunit = table.Column<string>(name: "inc_dec_interval_unit", type: "character varying(1)", maxLength: 1, nullable: true),
                    declimit = table.Column<double>(name: "dec_limit", type: "double precision", nullable: false),
                    inclimit = table.Column<double>(name: "inc_limit", type: "double precision", nullable: false),
                    incdeclimitunit = table.Column<string>(name: "inc_dec_limit_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    timedepend = table.Column<string>(name: "time_depend", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    judgeterm = table.Column<int>(name: "judge_term", type: "integer", nullable: false),
                    judgetermunit = table.Column<string>(name: "judge_term_unit", type: "character varying(1)", maxLength: 1, nullable: true),
                    extendflg = table.Column<string>(name: "extend_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    addterm = table.Column<int>(name: "add_term", type: "integer", nullable: false),
                    addtermunit = table.Column<string>(name: "add_term_unit", type: "character varying(1)", maxLength: 1, nullable: true),
                    intervalwarningflg = table.Column<string>(name: "interval_warning_flg", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m46_dosage_dosage", x => new { x.doeicd, x.doeiseqno });
                });

            migrationBuilder.CreateTable(
                name: "m46_dosage_drug",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    doeicd = table.Column<string>(name: "doei_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    drugkbn = table.Column<string>(name: "drug_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    kikakuunit = table.Column<string>(name: "kikaku_unit", type: "character varying(100)", maxLength: 100, nullable: true),
                    yakkaunit = table.Column<string>(name: "yakka_unit", type: "character varying(20)", maxLength: 20, nullable: true),
                    rikikarate = table.Column<decimal>(name: "rikika_rate", type: "numeric", nullable: false),
                    rikikaunit = table.Column<string>(name: "rikika_unit", type: "character varying(30)", maxLength: 30, nullable: true),
                    youkaiekicd = table.Column<string>(name: "youkaieki_cd", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m46_dosage_drug", x => new { x.doeicd, x.yjcd });
                });

            migrationBuilder.CreateTable(
                name: "m56_alrgy_derivatives",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    drvalrgycd = table.Column<string>(name: "drvalrgy_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_alrgy_derivatives", x => new { x.seibuncd, x.drvalrgycd, x.yjcd });
                });

            migrationBuilder.CreateTable(
                name: "m56_analogue_cd",
                columns: table => new
                {
                    analoguecd = table.Column<string>(name: "analogue_cd", type: "character varying(9)", maxLength: 9, nullable: false),
                    analoguename = table.Column<string>(name: "analogue_name", type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_analogue_cd", x => x.analoguecd);
                });

            migrationBuilder.CreateTable(
                name: "m56_drug_class",
                columns: table => new
                {
                    classcd = table.Column<string>(name: "class_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    classname = table.Column<string>(name: "class_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    classduplication = table.Column<string>(name: "class_duplication", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_drug_class", x => x.classcd);
                });

            migrationBuilder.CreateTable(
                name: "m56_drvalrgy_code",
                columns: table => new
                {
                    drvalrgycd = table.Column<string>(name: "drvalrgy_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    drvalrgyname = table.Column<string>(name: "drvalrgy_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    drvalrgygrp = table.Column<string>(name: "drvalrgy_grp", type: "character varying(4)", maxLength: 4, nullable: true),
                    rankno = table.Column<int>(name: "rank_no", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_drvalrgy_code", x => x.drvalrgycd);
                });

            migrationBuilder.CreateTable(
                name: "m56_ex_analogue",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(9)", maxLength: 9, nullable: false),
                    seqno = table.Column<string>(name: "seq_no", type: "character varying(2)", maxLength: 2, nullable: false),
                    analoguecd = table.Column<string>(name: "analogue_cd", type: "character varying(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_ex_analogue", x => new { x.seibuncd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "m56_ex_ed_ingredients",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    seqno = table.Column<string>(name: "seq_no", type: "character varying(3)", maxLength: 3, nullable: false),
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(9)", maxLength: 9, nullable: true),
                    seibunindexcd = table.Column<string>(name: "seibun_index_cd", type: "character varying(3)", maxLength: 3, nullable: true),
                    sbt = table.Column<int>(type: "integer", nullable: false),
                    prodrugcheck = table.Column<string>(name: "prodrug_check", type: "character varying(1)", maxLength: 1, nullable: true),
                    analoguecheck = table.Column<string>(name: "analogue_check", type: "character varying(1)", maxLength: 1, nullable: true),
                    yokaiekicheck = table.Column<string>(name: "yokaieki_check", type: "character varying(1)", maxLength: 1, nullable: true),
                    tenkabutucheck = table.Column<string>(name: "tenkabutu_check", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_ex_ed_ingredients", x => new { x.yjcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "m56_ex_ing_code",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(9)", maxLength: 9, nullable: false),
                    seibunindexcd = table.Column<string>(name: "seibun_index_cd", type: "character varying(3)", maxLength: 3, nullable: false),
                    seibunname = table.Column<string>(name: "seibun_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    yohocd = table.Column<string>(name: "yoho_cd", type: "character varying(6)", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_ex_ing_code", x => new { x.seibuncd, x.seibunindexcd });
                });

            migrationBuilder.CreateTable(
                name: "m56_ex_ingrdt_main",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    drugkbn = table.Column<string>(name: "drug_kbn", type: "character varying(2)", maxLength: 2, nullable: true),
                    yohocd = table.Column<string>(name: "yoho_cd", type: "character varying(6)", maxLength: 6, nullable: true),
                    haigouflg = table.Column<string>(name: "haigou_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    yuekiflg = table.Column<string>(name: "yueki_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    kanpoflg = table.Column<string>(name: "kanpo_flg", type: "character varying(1)", maxLength: 1, nullable: true),
                    zensinsayoflg = table.Column<string>(name: "zensinsayo_flg", type: "character varying(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_ex_ingrdt_main", x => x.yjcd);
                });

            migrationBuilder.CreateTable(
                name: "m56_prodrug_cd",
                columns: table => new
                {
                    seibuncd = table.Column<string>(name: "seibun_cd", type: "character varying(9)", maxLength: 9, nullable: false),
                    seqno = table.Column<string>(name: "seq_no", type: "character varying(2)", maxLength: 2, nullable: false),
                    kasseitaicd = table.Column<string>(name: "kasseitai_cd", type: "character varying(9)", maxLength: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_prodrug_cd", x => new { x.seqno, x.seibuncd });
                });

            migrationBuilder.CreateTable(
                name: "m56_usage_code",
                columns: table => new
                {
                    yohocd = table.Column<string>(name: "yoho_cd", type: "text", nullable: false),
                    yoho = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_usage_code", x => x.yohocd);
                });

            migrationBuilder.CreateTable(
                name: "m56_yj_drug_class",
                columns: table => new
                {
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    classcd = table.Column<string>(name: "class_cd", type: "character varying(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m56_yj_drug_class", x => new { x.yjcd, x.classcd });
                });

            migrationBuilder.CreateTable(
                name: "mall_message_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receiveno = table.Column<int>(name: "receive_no", type: "integer", nullable: false),
                    sendno = table.Column<int>(name: "send_no", type: "integer", nullable: false),
                    message = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mall_message_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mall_renkei_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    sinsatuno = table.Column<int>(name: "sinsatu_no", type: "integer", nullable: false),
                    kaikeino = table.Column<int>(name: "kaikei_no", type: "integer", nullable: false),
                    receiveno = table.Column<int>(name: "receive_no", type: "integer", nullable: false),
                    sendno = table.Column<int>(name: "send_no", type: "integer", nullable: false),
                    sendflg = table.Column<int>(name: "send_flg", type: "integer", nullable: false),
                    cliniccd = table.Column<int>(name: "clinic_cd", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mall_renkei_inf", x => new { x.hpid, x.raiinno });
                });

            migrationBuilder.CreateTable(
                name: "material_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    materialcd = table.Column<long>(name: "material_cd", type: "bigint", nullable: false),
                    materialname = table.Column<string>(name: "material_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_mst", x => new { x.hpid, x.materialcd });
                });

            migrationBuilder.CreateTable(
                name: "monshin_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    rtext = table.Column<string>(type: "text", nullable: true),
                    getkbn = table.Column<int>(name: "get_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monshin_inf", x => new { x.hpid, x.ptid, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "odr_date_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odr_date_detail", x => new { x.hpid, x.grpid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "odr_date_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    grpname = table.Column<string>(name: "grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odr_date_inf", x => new { x.hpid, x.grpid });
                });

            migrationBuilder.CreateTable(
                name: "odr_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    odrkouikbn = table.Column<int>(name: "odr_koui_kbn", type: "integer", nullable: false),
                    rpname = table.Column<string>(name: "rp_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    syohosbt = table.Column<int>(name: "syoho_sbt", type: "integer", nullable: false),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    dayscnt = table.Column<int>(name: "days_cnt", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odr_inf", x => new { x.hpid, x.raiinno, x.rpno, x.rpedano, x.id });
                });

            migrationBuilder.CreateTable(
                name: "odr_inf_cmt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    fontcolor = table.Column<int>(name: "font_color", type: "integer", nullable: false),
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(32)", maxLength: 32, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odr_inf_cmt", x => new { x.hpid, x.raiinno, x.rpno, x.rpedano, x.rowno, x.edano });
                });

            migrationBuilder.CreateTable(
                name: "odr_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    unitsbt = table.Column<int>(name: "unit_sbt", type: "integer", nullable: false),
                    termval = table.Column<double>(name: "term_val", type: "double precision", nullable: false),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    syohokbn = table.Column<int>(name: "syoho_kbn", type: "integer", nullable: false),
                    syoholimitkbn = table.Column<int>(name: "syoho_limit_kbn", type: "integer", nullable: false),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    kokuji1 = table.Column<string>(type: "text", nullable: true),
                    kokuji2 = table.Column<string>(type: "text", nullable: true),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    ipncd = table.Column<string>(name: "ipn_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    jissikbn = table.Column<int>(name: "jissi_kbn", type: "integer", nullable: false),
                    jissidate = table.Column<DateTime>(name: "jissi_date", type: "timestamp with time zone", nullable: true),
                    jissiid = table.Column<int>(name: "jissi_id", type: "integer", nullable: false),
                    jissimachine = table.Column<string>(name: "jissi_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    reqcd = table.Column<string>(name: "req_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    bunkatu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true),
                    fontcolor = table.Column<string>(name: "font_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    commentnewline = table.Column<int>(name: "comment_newline", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odr_inf_detail", x => new { x.hpid, x.raiinno, x.rpno, x.rpedano, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "online_confirmation",
                columns: table => new
                {
                    receptionno = table.Column<string>(name: "reception_no", type: "text", nullable: false),
                    receptiondatetime = table.Column<DateTime>(name: "reception_datetime", type: "timestamp with time zone", nullable: false),
                    yoyakudate = table.Column<int>(name: "yoyaku_date", type: "integer", nullable: false),
                    segmentofresult = table.Column<string>(name: "segment_of_result", type: "character varying(1)", maxLength: 1, nullable: true),
                    errormessage = table.Column<string>(name: "error_message", type: "character varying(60)", maxLength: 60, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_confirmation", x => x.receptionno);
                });

            migrationBuilder.CreateTable(
                name: "online_confirmation_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    onlineconfirmationdate = table.Column<DateTime>(name: "online_confirmation_date", type: "timestamp with time zone", nullable: false),
                    infoconsflg = table.Column<string>(name: "info_cons_flg", type: "character varying(10)", maxLength: 10, nullable: true),
                    confirmationtype = table.Column<int>(name: "confirmation_type", type: "integer", nullable: false),
                    prescriptionissuetype = table.Column<int>(name: "prescription_issue_type", type: "integer", nullable: false),
                    confirmationresult = table.Column<string>(name: "confirmation_result", type: "text", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    uketukestatus = table.Column<int>(name: "uketuke_status", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_confirmation_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "online_consent",
                columns: table => new
                {
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    conskbn = table.Column<int>(name: "cons_kbn", type: "integer", nullable: false),
                    consdate = table.Column<DateTime>(name: "cons_date", type: "timestamp with time zone", nullable: false),
                    limitdate = table.Column<DateTime>(name: "limit_date", type: "timestamp with time zone", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_online_consent", x => new { x.ptid, x.conskbn });
                });

            migrationBuilder.CreateTable(
                name: "path_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    grpedano = table.Column<int>(name: "grp_eda_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    machine = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    param = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    charcd = table.Column<int>(name: "char_cd", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_path_conf", x => new { x.hpid, x.grpcd, x.grpedano, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "payment_method_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    paymentmethodcd = table.Column<int>(name: "payment_method_cd", type: "integer", nullable: false),
                    payname = table.Column<string>(name: "pay_name", type: "character varying(60)", maxLength: 60, nullable: true),
                    paysname = table.Column<string>(name: "pay_sname", type: "character varying(1)", maxLength: 1, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_method_mst", x => new { x.hpid, x.paymentmethodcd });
                });

            migrationBuilder.CreateTable(
                name: "permission_mst",
                columns: table => new
                {
                    functioncd = table.Column<string>(name: "function_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    permission = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_mst", x => new { x.functioncd, x.permission });
                });

            migrationBuilder.CreateTable(
                name: "physical_average",
                columns: table => new
                {
                    jissiyear = table.Column<int>(name: "jissi_year", type: "integer", nullable: false),
                    ageyear = table.Column<int>(name: "age_year", type: "integer", nullable: false),
                    agemonth = table.Column<int>(name: "age_month", type: "integer", nullable: false),
                    ageday = table.Column<int>(name: "age_day", type: "integer", nullable: false),
                    maleheight = table.Column<double>(name: "male_height", type: "double precision", nullable: false),
                    maleweight = table.Column<double>(name: "male_weight", type: "double precision", nullable: false),
                    malechest = table.Column<double>(name: "male_chest", type: "double precision", nullable: false),
                    malehead = table.Column<double>(name: "male_head", type: "double precision", nullable: false),
                    femaleheight = table.Column<double>(name: "female_height", type: "double precision", nullable: false),
                    femaleweight = table.Column<double>(name: "female_weight", type: "double precision", nullable: false),
                    femalechest = table.Column<double>(name: "female_chest", type: "double precision", nullable: false),
                    femalehead = table.Column<double>(name: "female_head", type: "double precision", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_physical_average", x => new { x.jissiyear, x.ageyear, x.agemonth, x.ageday });
                });

            migrationBuilder.CreateTable(
                name: "pi_image",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    imagetype = table.Column<int>(name: "image_type", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(30)", maxLength: 30, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pi_image", x => new { x.hpid, x.imagetype, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "pi_inf",
                columns: table => new
                {
                    piid = table.Column<string>(name: "pi_id", type: "character varying(6)", maxLength: 6, nullable: false),
                    wdate = table.Column<int>(name: "w_date", type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    rdate = table.Column<int>(name: "r_date", type: "integer", nullable: false),
                    revision = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    rtype = table.Column<string>(name: "r_type", type: "character varying(20)", maxLength: 20, nullable: true),
                    rreason = table.Column<string>(name: "r_reason", type: "character varying(200)", maxLength: 200, nullable: true),
                    sccjno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    therapeuticclassification = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    preparationname = table.Column<string>(name: "preparation_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    highlight = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    feature = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    relatedmatter = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    commonname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    genericname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pi_inf", x => x.piid);
                });

            migrationBuilder.CreateTable(
                name: "pi_inf_detail",
                columns: table => new
                {
                    piid = table.Column<string>(name: "pi_id", type: "character varying(6)", maxLength: 6, nullable: false),
                    branch = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    jpn = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pi_inf_detail", x => new { x.piid, x.branch, x.jpn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pi_product_inf",
                columns: table => new
                {
                    piid = table.Column<string>(name: "pi_id", type: "text", nullable: false),
                    branch = table.Column<string>(type: "text", nullable: false),
                    jpn = table.Column<string>(type: "text", nullable: false),
                    piidfull = table.Column<string>(name: "pi_id_full", type: "text", nullable: false),
                    productname = table.Column<string>(name: "product_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    unit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    maker = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    vender = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    marketer = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    other = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    yjcd = table.Column<string>(name: "yj_cd", type: "text", nullable: true),
                    hotcd = table.Column<string>(name: "hot_cd", type: "text", nullable: true),
                    sosyoname = table.Column<string>(name: "sosyo_name", type: "character varying(80)", maxLength: 80, nullable: true),
                    genericname = table.Column<string>(name: "generic_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    genericengname = table.Column<string>(name: "generic_eng_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    generalno = table.Column<string>(name: "general_no", type: "character varying(50)", maxLength: 50, nullable: true),
                    verdate = table.Column<string>(name: "ver_date", type: "text", nullable: true),
                    yakkareg = table.Column<string>(name: "yakka_reg", type: "text", nullable: true),
                    yakkadel = table.Column<string>(name: "yakka_del", type: "text", nullable: true),
                    isstoped = table.Column<string>(name: "is_stoped", type: "text", nullable: true),
                    stopdate = table.Column<string>(name: "stop_date", type: "text", nullable: true),
                    pistate = table.Column<string>(name: "pi_state", type: "text", nullable: true),
                    pisbt = table.Column<string>(name: "pi_sbt", type: "text", nullable: true),
                    bikopiunit = table.Column<string>(name: "biko_pi_unit", type: "character varying(512)", maxLength: 512, nullable: true),
                    bikopibranch = table.Column<string>(name: "biko_pi_branch", type: "character varying(256)", maxLength: 256, nullable: true),
                    upddateimg = table.Column<DateTime>(name: "upd_date_img", type: "timestamp with time zone", nullable: true),
                    upddatepi = table.Column<DateTime>(name: "upd_date_pi", type: "timestamp with time zone", nullable: true),
                    upddateproduct = table.Column<DateTime>(name: "upd_date_product", type: "timestamp with time zone", nullable: true),
                    upddatexml = table.Column<DateTime>(name: "upd_date_xml", type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pi_product_inf", x => new { x.piidfull, x.piid, x.branch, x.jpn });
                });

            migrationBuilder.CreateTable(
                name: "post_code_mst",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    postcd = table.Column<string>(name: "post_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    prefkana = table.Column<string>(name: "pref_kana", type: "character varying(60)", maxLength: 60, nullable: true),
                    citykana = table.Column<string>(name: "city_kana", type: "character varying(60)", maxLength: 60, nullable: true),
                    postaltermkana = table.Column<string>(name: "postal_term_kana", type: "character varying(150)", maxLength: 150, nullable: true),
                    prefname = table.Column<string>(name: "pref_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    cityname = table.Column<string>(name: "city_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    banti = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_code_mst", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "priority_haihan_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    haihangrp = table.Column<long>(name: "haihan_grp", type: "bigint", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    usersetting = table.Column<int>(name: "user_setting", type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    itemcd1 = table.Column<string>(name: "item_cd1", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd2 = table.Column<string>(name: "item_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd3 = table.Column<string>(name: "item_cd3", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd4 = table.Column<string>(name: "item_cd4", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd5 = table.Column<string>(name: "item_cd5", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd6 = table.Column<string>(name: "item_cd6", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd7 = table.Column<string>(name: "item_cd7", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd8 = table.Column<string>(name: "item_cd8", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemcd9 = table.Column<string>(name: "item_cd9", type: "character varying(10)", maxLength: 10, nullable: true),
                    spjyoken = table.Column<int>(name: "sp_jyoken", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    termcnt = table.Column<int>(name: "term_cnt", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false),
                    targetkbn = table.Column<int>(name: "target_kbn", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priority_haihan_mst", x => new { x.hpid, x.haihangrp, x.startdate, x.usersetting });
                });

            migrationBuilder.CreateTable(
                name: "pt_alrgy_drug",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    drugname = table.Column<string>(name: "drug_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_alrgy_drug", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_alrgy_else",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    alrgyname = table.Column<string>(name: "alrgy_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_alrgy_else", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_alrgy_food",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    alrgykbn = table.Column<string>(name: "alrgy_kbn", type: "text", nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_alrgy_food", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_byomei",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    syusyokucd1 = table.Column<string>(name: "syusyoku_cd1", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd2 = table.Column<string>(name: "syusyoku_cd2", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd3 = table.Column<string>(name: "syusyoku_cd3", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd4 = table.Column<string>(name: "syusyoku_cd4", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd5 = table.Column<string>(name: "syusyoku_cd5", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd6 = table.Column<string>(name: "syusyoku_cd6", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd7 = table.Column<string>(name: "syusyoku_cd7", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd8 = table.Column<string>(name: "syusyoku_cd8", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd9 = table.Column<string>(name: "syusyoku_cd9", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd10 = table.Column<string>(name: "syusyoku_cd10", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd11 = table.Column<string>(name: "syusyoku_cd11", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd12 = table.Column<string>(name: "syusyoku_cd12", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd13 = table.Column<string>(name: "syusyoku_cd13", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd14 = table.Column<string>(name: "syusyoku_cd14", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd15 = table.Column<string>(name: "syusyoku_cd15", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd16 = table.Column<string>(name: "syusyoku_cd16", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd17 = table.Column<string>(name: "syusyoku_cd17", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd18 = table.Column<string>(name: "syusyoku_cd18", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd19 = table.Column<string>(name: "syusyoku_cd19", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd20 = table.Column<string>(name: "syusyoku_cd20", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd21 = table.Column<string>(name: "syusyoku_cd21", type: "character varying(7)", maxLength: 7, nullable: true),
                    byomei = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    tenkikbn = table.Column<int>(name: "tenki_kbn", type: "integer", nullable: false),
                    tenkidate = table.Column<int>(name: "tenki_date", type: "integer", nullable: false),
                    syubyokbn = table.Column<int>(name: "syubyo_kbn", type: "integer", nullable: false),
                    sikkankbn = table.Column<int>(name: "sikkan_kbn", type: "integer", nullable: false),
                    nanbyocd = table.Column<int>(name: "nanbyo_cd", type: "integer", nullable: false),
                    hosokucmt = table.Column<string>(name: "hosoku_cmt", type: "character varying(80)", maxLength: 80, nullable: true),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    togetubyomei = table.Column<int>(name: "togetu_byomei", type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodspkarte = table.Column<int>(name: "is_nodsp_karte", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    isimportant = table.Column<int>(name: "is_important", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_byomei", x => new { x.hpid, x.ptid, x.id });
                });

            migrationBuilder.CreateTable(
                name: "pt_cmt_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_cmt_inf", x => new { x.id, x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_family",
                columns: table => new
                {
                    familyid = table.Column<long>(name: "family_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    zokugaracd = table.Column<string>(name: "zokugara_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    parentid = table.Column<int>(name: "parent_id", type: "integer", nullable: false),
                    familyptid = table.Column<long>(name: "family_pt_id", type: "bigint", nullable: false),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    birthday = table.Column<int>(type: "integer", nullable: false),
                    isdead = table.Column<int>(name: "is_dead", type: "integer", nullable: false),
                    isseparated = table.Column<int>(name: "is_separated", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_family", x => x.familyid);
                });

            migrationBuilder.CreateTable(
                name: "pt_family_reki",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    familyid = table.Column<long>(name: "family_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_family_reki", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pt_grp_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    grpcode = table.Column<string>(name: "grp_code", type: "character varying(4)", maxLength: 4, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_grp_inf", x => new { x.hpid, x.grpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_grp_item",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    grpcode = table.Column<string>(name: "grp_code", type: "character varying(2)", maxLength: 2, nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grpcodename = table.Column<string>(name: "grp_code_name", type: "character varying(30)", maxLength: 30, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_grp_item", x => new { x.hpid, x.grpid, x.grpcode, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_grp_name_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    grpname = table.Column<string>(name: "grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_grp_name_mst", x => new { x.hpid, x.grpid });
                });

            migrationBuilder.CreateTable(
                name: "pt_hoken_check",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokengrp = table.Column<int>(name: "hoken_grp", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkdate = table.Column<DateTime>(name: "check_date", type: "timestamp with time zone", nullable: false),
                    checkid = table.Column<int>(name: "check_id", type: "integer", nullable: false),
                    checkmachine = table.Column<string>(name: "check_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    checkcmt = table.Column<string>(name: "check_cmt", type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_hoken_check", x => new { x.hpid, x.ptid, x.hokengrp, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_hoken_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    edano = table.Column<string>(name: "eda_no", type: "character varying(2)", maxLength: 2, nullable: true),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    hokensyano = table.Column<string>(name: "hokensya_no", type: "character varying(8)", maxLength: 8, nullable: true),
                    kigo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    bango = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    honkekbn = table.Column<int>(name: "honke_kbn", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    hokensyaname = table.Column<string>(name: "hokensya_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    hokensyapost = table.Column<string>(name: "hokensya_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    hokensyaaddress = table.Column<string>(name: "hokensya_address", type: "character varying(100)", maxLength: 100, nullable: true),
                    hokensyatel = table.Column<string>(name: "hokensya_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    keizokukbn = table.Column<int>(name: "keizoku_kbn", type: "integer", nullable: false),
                    sikakudate = table.Column<int>(name: "sikaku_date", type: "integer", nullable: false),
                    kofudate = table.Column<int>(name: "kofu_date", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    gendogaku = table.Column<int>(type: "integer", nullable: false),
                    kogakukbn = table.Column<int>(name: "kogaku_kbn", type: "integer", nullable: false),
                    kogakutype = table.Column<int>(name: "kogaku_type", type: "integer", nullable: false),
                    tokureiym1 = table.Column<int>(name: "tokurei_ym1", type: "integer", nullable: false),
                    tokureiym2 = table.Column<int>(name: "tokurei_ym2", type: "integer", nullable: false),
                    tasukaiym = table.Column<int>(name: "tasukai_ym", type: "integer", nullable: false),
                    syokumukbn = table.Column<int>(name: "syokumu_kbn", type: "integer", nullable: false),
                    genmenkbn = table.Column<int>(name: "genmen_kbn", type: "integer", nullable: false),
                    genmenrate = table.Column<int>(name: "genmen_rate", type: "integer", nullable: false),
                    genmengaku = table.Column<int>(name: "genmen_gaku", type: "integer", nullable: false),
                    tokki1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki5 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    rousaikofuno = table.Column<string>(name: "rousai_kofu_no", type: "character varying(14)", maxLength: 14, nullable: true),
                    rousaisaigaikbn = table.Column<int>(name: "rousai_saigai_kbn", type: "integer", nullable: false),
                    rousaijigyosyoname = table.Column<string>(name: "rousai_jigyosyo_name", type: "character varying(80)", maxLength: 80, nullable: true),
                    rousaiprefname = table.Column<string>(name: "rousai_pref_name", type: "character varying(10)", maxLength: 10, nullable: true),
                    rousaicityname = table.Column<string>(name: "rousai_city_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    rousaisyobyodate = table.Column<int>(name: "rousai_syobyo_date", type: "integer", nullable: false),
                    rousaisyobyocd = table.Column<string>(name: "rousai_syobyo_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    rousairoudoucd = table.Column<string>(name: "rousai_roudou_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    rousaikantokucd = table.Column<string>(name: "rousai_kantoku_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    rousairececount = table.Column<int>(name: "rousai_rece_count", type: "integer", nullable: false),
                    ryoyostartdate = table.Column<int>(name: "ryoyo_start_date", type: "integer", nullable: false),
                    ryoyoenddate = table.Column<int>(name: "ryoyo_end_date", type: "integer", nullable: false),
                    jibaihokenname = table.Column<string>(name: "jibai_hoken_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    jibaihokentanto = table.Column<string>(name: "jibai_hoken_tanto", type: "character varying(40)", maxLength: 40, nullable: true),
                    jibaihokentel = table.Column<string>(name: "jibai_hoken_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    jibaijyusyoudate = table.Column<int>(name: "jibai_jyusyou_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_hoken_inf", x => new { x.hpid, x.ptid, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_hoken_pattern",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    hokensbtcd = table.Column<int>(name: "hoken_sbt_cd", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    kohi1id = table.Column<int>(name: "kohi1_id", type: "integer", nullable: false),
                    kohi2id = table.Column<int>(name: "kohi2_id", type: "integer", nullable: false),
                    kohi3id = table.Column<int>(name: "kohi3_id", type: "integer", nullable: false),
                    kohi4id = table.Column<int>(name: "kohi4_id", type: "integer", nullable: false),
                    hokenmemo = table.Column<string>(name: "hoken_memo", type: "character varying(400)", maxLength: 400, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_hoken_pattern", x => new { x.hpid, x.ptid, x.seqno, x.hokenpid });
                });

            migrationBuilder.CreateTable(
                name: "pt_hoken_scan",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokengrp = table.Column<int>(name: "hoken_grp", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    filename = table.Column<string>(name: "file_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_hoken_scan", x => new { x.hpid, x.ptid, x.hokengrp, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptnum = table.Column<long>(name: "pt_num", type: "bigint", nullable: false),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    birthday = table.Column<int>(type: "integer", nullable: false),
                    isdead = table.Column<int>(name: "is_dead", type: "integer", nullable: false),
                    deathdate = table.Column<int>(name: "death_date", type: "integer", nullable: false),
                    homepost = table.Column<string>(name: "home_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    homeaddress1 = table.Column<string>(name: "home_address1", type: "character varying(100)", maxLength: 100, nullable: true),
                    homeaddress2 = table.Column<string>(name: "home_address2", type: "character varying(100)", maxLength: 100, nullable: true),
                    tel1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    tel2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    mail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    setainusi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    zokugara = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    job = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    renrakuname = table.Column<string>(name: "renraku_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    renrakupost = table.Column<string>(name: "renraku_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    renrakuaddress1 = table.Column<string>(name: "renraku_address1", type: "character varying(100)", maxLength: 100, nullable: true),
                    renrakuaddress2 = table.Column<string>(name: "renraku_address2", type: "character varying(100)", maxLength: 100, nullable: true),
                    renrakutel = table.Column<string>(name: "renraku_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    renrakumemo = table.Column<string>(name: "renraku_memo", type: "character varying(100)", maxLength: 100, nullable: true),
                    officename = table.Column<string>(name: "office_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    officepost = table.Column<string>(name: "office_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    officeaddress1 = table.Column<string>(name: "office_address1", type: "character varying(100)", maxLength: 100, nullable: true),
                    officeaddress2 = table.Column<string>(name: "office_address2", type: "character varying(100)", maxLength: 100, nullable: true),
                    officetel = table.Column<string>(name: "office_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    officememo = table.Column<string>(name: "office_memo", type: "character varying(100)", maxLength: 100, nullable: true),
                    isryosyodetail = table.Column<int>(name: "is_ryosyo_detail", type: "integer", nullable: false),
                    primarydoctor = table.Column<int>(name: "primary_doctor", type: "integer", nullable: false),
                    istester = table.Column<int>(name: "is_tester", type: "integer", nullable: false),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    mainhokenpid = table.Column<int>(name: "main_hoken_pid", type: "integer", nullable: false),
                    referenceno = table.Column<long>(name: "reference_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    limitconsflg = table.Column<int>(name: "limit_cons_flg", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_inf", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_infection",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "text", nullable: true),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_infection", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_jibai_doc",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    sindancost = table.Column<int>(name: "sindan_cost", type: "integer", nullable: false),
                    sindannum = table.Column<int>(name: "sindan_num", type: "integer", nullable: false),
                    meisaicost = table.Column<int>(name: "meisai_cost", type: "integer", nullable: false),
                    meisainum = table.Column<int>(name: "meisai_num", type: "integer", nullable: false),
                    elsecost = table.Column<int>(name: "else_cost", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_jibai_doc", x => new { x.hpid, x.ptid, x.sinym, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_jibkar",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    webid = table.Column<string>(name: "web_id", type: "character varying(16)", maxLength: 16, nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    odrkaiji = table.Column<int>(name: "odr_kaiji", type: "integer", nullable: false),
                    odrupdatedate = table.Column<DateTime>(name: "odr_update_date", type: "timestamp with time zone", nullable: false),
                    kartekaiji = table.Column<int>(name: "karte_kaiji", type: "integer", nullable: false),
                    karteupdatedate = table.Column<DateTime>(name: "karte_update_date", type: "timestamp with time zone", nullable: false),
                    kensakaiji = table.Column<int>(name: "kensa_kaiji", type: "integer", nullable: false),
                    kensaupdatedate = table.Column<DateTime>(name: "kensa_update_date", type: "timestamp with time zone", nullable: false),
                    byomeikaiji = table.Column<int>(name: "byomei_kaiji", type: "integer", nullable: false),
                    byomeiupdatedate = table.Column<DateTime>(name: "byomei_update_date", type: "timestamp with time zone", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_jibkar", x => new { x.hpid, x.webid });
                });

            migrationBuilder.CreateTable(
                name: "pt_kio_reki",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "text", nullable: true),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_kio_reki", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_kohi",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    futansyano = table.Column<string>(name: "futansya_no", type: "character varying(8)", maxLength: 8, nullable: true),
                    jyukyusyano = table.Column<string>(name: "jyukyusya_no", type: "character varying(7)", maxLength: 7, nullable: true),
                    hokensbtkbn = table.Column<int>(name: "hoken_sbt_kbn", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    tokusyuno = table.Column<string>(name: "tokusyu_no", type: "character varying(20)", maxLength: 20, nullable: true),
                    sikakudate = table.Column<int>(name: "sikaku_date", type: "integer", nullable: false),
                    kofudate = table.Column<int>(name: "kofu_date", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    gendogaku = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_kohi", x => new { x.hpid, x.ptid, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_kyusei",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_kyusei", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_last_visit_date",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    lastvisitdate = table.Column<int>(name: "last_visit_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_last_visit_date", x => new { x.hpid, x.ptid });
                });

            migrationBuilder.CreateTable(
                name: "pt_memo",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    memo = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_memo", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_otc_drug",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    serialnum = table.Column<int>(name: "serial_num", type: "integer", nullable: false),
                    tradename = table.Column<string>(name: "trade_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_otc_drug", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_other_drug",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    drugname = table.Column<string>(name: "drug_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_other_drug", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_pregnancy",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    perioddate = table.Column<int>(name: "period_date", type: "integer", nullable: false),
                    periodduedate = table.Column<int>(name: "period_due_date", type: "integer", nullable: false),
                    ovulationdate = table.Column<int>(name: "ovulation_date", type: "integer", nullable: false),
                    ovulationduedate = table.Column<int>(name: "ovulation_due_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_pregnancy", x => new { x.id, x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_rousai_tenki",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    sinkei = table.Column<int>(type: "integer", nullable: false),
                    tenki = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_rousai_tenki", x => new { x.hpid, x.ptid, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_santei_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kbnno = table.Column<int>(name: "kbn_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    kbnval = table.Column<int>(name: "kbn_val", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_santei_conf", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_supple",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    indexcd = table.Column<string>(name: "index_cd", type: "text", nullable: true),
                    indexword = table.Column<string>(name: "index_word", type: "character varying(200)", maxLength: 200, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_supple", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "pt_tag",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    memo = table.Column<string>(type: "text", nullable: true),
                    memodata = table.Column<byte[]>(name: "memo_data", type: "bytea", nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdspuketuke = table.Column<int>(name: "is_dsp_uketuke", type: "integer", nullable: false),
                    isdspkarte = table.Column<int>(name: "is_dsp_karte", type: "integer", nullable: false),
                    isdspkaikei = table.Column<int>(name: "is_dsp_kaikei", type: "integer", nullable: false),
                    isdsprece = table.Column<int>(name: "is_dsp_rece", type: "integer", nullable: false),
                    backgroundcolor = table.Column<string>(name: "background_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    taggrpcd = table.Column<int>(name: "tag_grp_cd", type: "integer", nullable: false),
                    alphablendval = table.Column<int>(name: "alphablend_val", type: "integer", nullable: false),
                    fontsize = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    left = table.Column<int>(type: "integer", nullable: false),
                    top = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pt_tag", x => new { x.hpid, x.ptid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_cmt_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_cmt_inf", x => new { x.hpid, x.raiinno, x.cmtkbn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_filter_kbn",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    filterid = table.Column<int>(name: "filter_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_filter_kbn", x => new { x.hpid, x.filterid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_filter_mst",
                columns: table => new
                {
                    filterid = table.Column<int>(name: "filter_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    filtername = table.Column<string>(name: "filter_name", type: "text", nullable: true),
                    selectkbn = table.Column<int>(name: "select_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    shortcut = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_filter_mst", x => x.filterid);
                });

            migrationBuilder.CreateTable(
                name: "raiin_filter_sort",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    filterid = table.Column<int>(name: "filter_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    columnname = table.Column<string>(name: "column_name", type: "text", nullable: true),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    sortkbn = table.Column<int>(name: "sort_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_filter_sort", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "raiin_filter_state",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    filterid = table.Column<int>(name: "filter_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_filter_state", x => new { x.hpid, x.filterid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    oyaraiinno = table.Column<long>(name: "oya_raiin_no", type: "bigint", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    isyoyaku = table.Column<int>(name: "is_yoyaku", type: "integer", nullable: false),
                    yoyakutime = table.Column<string>(name: "yoyaku_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    yoyakuid = table.Column<int>(name: "yoyaku_id", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    uketuketime = table.Column<string>(name: "uketuke_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    uketukeid = table.Column<int>(name: "uketuke_id", type: "integer", nullable: false),
                    uketukeno = table.Column<int>(name: "uketuke_no", type: "integer", nullable: false),
                    sinstarttime = table.Column<string>(name: "sin_start_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    sinendtime = table.Column<string>(name: "sin_end_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    kaikeitime = table.Column<string>(name: "kaikei_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    kaikeiid = table.Column<int>(name: "kaikei_id", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    syosaisinkbn = table.Column<int>(name: "syosaisin_kbn", type: "integer", nullable: false),
                    jikankbn = table.Column<int>(name: "jikan_kbn", type: "integer", nullable: false),
                    confirmationresult = table.Column<string>(name: "confirmation_result", type: "character varying(120)", maxLength: 120, nullable: true),
                    confirmationstate = table.Column<int>(name: "confirmation_state", type: "integer", nullable: false),
                    confirmationtype = table.Column<int>(name: "confirmation_type", type: "integer", nullable: false),
                    infoconsflg = table.Column<string>(name: "info_cons_flg", type: "character varying(10)", maxLength: 10, nullable: true),
                    prescriptionissuetype = table.Column<int>(name: "prescription_issue_type", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_inf", x => new { x.hpid, x.raiinno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_kbn_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    kbnname = table.Column<string>(name: "kbn_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    colorcd = table.Column<string>(name: "color_cd", type: "character varying(8)", maxLength: 8, nullable: true),
                    isconfirmed = table.Column<int>(name: "is_confirmed", type: "integer", nullable: false),
                    isauto = table.Column<int>(name: "is_auto", type: "integer", nullable: false),
                    isautodelete = table.Column<int>(name: "is_auto_delete", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_kbn_detail", x => new { x.hpid, x.grpid, x.kbncd });
                });

            migrationBuilder.CreateTable(
                name: "raiin_kbn_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_kbn_inf", x => new { x.hpid, x.ptid, x.raiinno, x.grpid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_kbn_item",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    isexclude = table.Column<int>(name: "is_exclude", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_kbn_item", x => new { x.hpid, x.grpid, x.kbncd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_kbn_koui",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kouikbnid = table.Column<int>(name: "koui_kbn_id", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_kbn_koui", x => new { x.hpid, x.grpid, x.kbncd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_kbn_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    grpname = table.Column<string>(name: "grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_kbn_mst", x => new { x.hpid, x.grpid });
                });

            migrationBuilder.CreateTable(
                name: "raiin_kbn_yoyaku",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    yoyakucd = table.Column<int>(name: "yoyaku_cd", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_kbn_yoyaku", x => new { x.hpid, x.grpid, x.kbncd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_cmt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    text = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_cmt", x => new { x.hpid, x.raiinno, x.cmtkbn });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    kbnname = table.Column<string>(name: "kbn_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    colorcd = table.Column<string>(name: "color_cd", type: "character varying(8)", maxLength: 8, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_detail", x => new { x.hpid, x.grpid, x.kbncd });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_doc",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_doc", x => new { x.hpid, x.grpid, x.kbncd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_file",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_file", x => new { x.hpid, x.grpid, x.kbncd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_inf",
                columns: table => new
                {
                    raiinlistkbn = table.Column<int>(name: "raiin_list_kbn", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_inf", x => new { x.hpid, x.ptid, x.sindate, x.raiinno, x.grpid, x.raiinlistkbn });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_item",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    isexclude = table.Column<int>(name: "is_exclude", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_item", x => new { x.hpid, x.kbncd, x.seqno, x.grpid });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_koui",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kouikbnid = table.Column<int>(name: "koui_kbn_id", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_koui", x => new { x.hpid, x.kbncd, x.seqno, x.grpid });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grpname = table.Column<string>(name: "grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_mst", x => new { x.hpid, x.grpid });
                });

            migrationBuilder.CreateTable(
                name: "raiin_list_tag",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    tagno = table.Column<int>(name: "tag_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raiin_list_tag", x => new { x.hpid, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rece_check_cmt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    ispending = table.Column<int>(name: "is_pending", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ischecked = table.Column<int>(name: "is_checked", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_check_cmt", x => new { x.hpid, x.ptid, x.sinym, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rece_check_err",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    errcd = table.Column<string>(name: "err_cd", type: "character varying(5)", maxLength: 5, nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    acd = table.Column<string>(name: "a_cd", type: "character varying(100)", maxLength: 100, nullable: false),
                    bcd = table.Column<string>(name: "b_cd", type: "character varying(100)", maxLength: 100, nullable: false),
                    message1 = table.Column<string>(name: "message_1", type: "character varying(100)", maxLength: 100, nullable: true),
                    message2 = table.Column<string>(name: "message_2", type: "character varying(100)", maxLength: 100, nullable: true),
                    ischecked = table.Column<int>(name: "is_checked", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_check_err", x => new { x.hpid, x.ptid, x.sinym, x.hokenid, x.errcd, x.sindate, x.acd, x.bcd });
                });

            migrationBuilder.CreateTable(
                name: "rece_check_opt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    errcd = table.Column<string>(name: "err_cd", type: "character varying(5)", maxLength: 5, nullable: false),
                    checkopt = table.Column<int>(name: "check_opt", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_check_opt", x => new { x.hpid, x.errcd });
                });

            migrationBuilder.CreateTable(
                name: "rece_cmt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    cmtsbt = table.Column<int>(name: "cmt_sbt", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmt = table.Column<string>(type: "text", nullable: true),
                    cmtdata = table.Column<string>(name: "cmt_data", type: "character varying(38)", maxLength: 38, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_cmt", x => new { x.hpid, x.ptid, x.sinym, x.hokenid, x.cmtkbn, x.cmtsbt, x.id });
                });

            migrationBuilder.CreateTable(
                name: "rece_futan_kbn",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    futankbncd = table.Column<string>(name: "futan_kbn_cd", type: "character varying(1)", maxLength: 1, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_futan_kbn", x => new { x.hpid, x.seikyuym, x.ptid, x.sinym, x.hokenid, x.hokenpid });
                });

            migrationBuilder.CreateTable(
                name: "rece_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    hokenid2 = table.Column<int>(name: "hoken_id2", type: "integer", nullable: false),
                    kohi1id = table.Column<int>(name: "kohi1_id", type: "integer", nullable: false),
                    kohi2id = table.Column<int>(name: "kohi2_id", type: "integer", nullable: false),
                    kohi3id = table.Column<int>(name: "kohi3_id", type: "integer", nullable: false),
                    kohi4id = table.Column<int>(name: "kohi4_id", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    hokensbtcd = table.Column<int>(name: "hoken_sbt_cd", type: "integer", nullable: false),
                    recesbt = table.Column<string>(name: "rece_sbt", type: "character varying(4)", maxLength: 4, nullable: true),
                    hokensyano = table.Column<string>(name: "hokensya_no", type: "character varying(8)", maxLength: 8, nullable: true),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1houbetu = table.Column<string>(name: "kohi1_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi2houbetu = table.Column<string>(name: "kohi2_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi3houbetu = table.Column<string>(name: "kohi3_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi4houbetu = table.Column<string>(name: "kohi4_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    honkekbn = table.Column<int>(name: "honke_kbn", type: "integer", nullable: false),
                    kogakukbn = table.Column<int>(name: "kogaku_kbn", type: "integer", nullable: false),
                    kogakutekiyokbn = table.Column<int>(name: "kogaku_tekiyo_kbn", type: "integer", nullable: false),
                    istokurei = table.Column<int>(name: "is_tokurei", type: "integer", nullable: false),
                    istasukai = table.Column<int>(name: "is_tasukai", type: "integer", nullable: false),
                    kogakukohi1limit = table.Column<int>(name: "kogaku_kohi1_limit", type: "integer", nullable: false),
                    kogakukohi2limit = table.Column<int>(name: "kogaku_kohi2_limit", type: "integer", nullable: false),
                    kogakukohi3limit = table.Column<int>(name: "kogaku_kohi3_limit", type: "integer", nullable: false),
                    kogakukohi4limit = table.Column<int>(name: "kogaku_kohi4_limit", type: "integer", nullable: false),
                    totalkogakulimit = table.Column<int>(name: "total_kogaku_limit", type: "integer", nullable: false),
                    genmenkbn = table.Column<int>(name: "genmen_kbn", type: "integer", nullable: false),
                    hokenrate = table.Column<int>(name: "hoken_rate", type: "integer", nullable: false),
                    ptrate = table.Column<int>(name: "pt_rate", type: "integer", nullable: false),
                    enten = table.Column<int>(name: "en_ten", type: "integer", nullable: false),
                    kohi1limit = table.Column<int>(name: "kohi1_limit", type: "integer", nullable: false),
                    kohi1otherfutan = table.Column<int>(name: "kohi1_other_futan", type: "integer", nullable: false),
                    kohi2limit = table.Column<int>(name: "kohi2_limit", type: "integer", nullable: false),
                    kohi2otherfutan = table.Column<int>(name: "kohi2_other_futan", type: "integer", nullable: false),
                    kohi3limit = table.Column<int>(name: "kohi3_limit", type: "integer", nullable: false),
                    kohi3otherfutan = table.Column<int>(name: "kohi3_other_futan", type: "integer", nullable: false),
                    kohi4limit = table.Column<int>(name: "kohi4_limit", type: "integer", nullable: false),
                    kohi4otherfutan = table.Column<int>(name: "kohi4_other_futan", type: "integer", nullable: false),
                    tensu = table.Column<int>(type: "integer", nullable: false),
                    totaliryohi = table.Column<int>(name: "total_iryohi", type: "integer", nullable: false),
                    hokenfutan = table.Column<int>(name: "hoken_futan", type: "integer", nullable: false),
                    kogakufutan = table.Column<int>(name: "kogaku_futan", type: "integer", nullable: false),
                    kohi1futan = table.Column<int>(name: "kohi1_futan", type: "integer", nullable: false),
                    kohi2futan = table.Column<int>(name: "kohi2_futan", type: "integer", nullable: false),
                    kohi3futan = table.Column<int>(name: "kohi3_futan", type: "integer", nullable: false),
                    kohi4futan = table.Column<int>(name: "kohi4_futan", type: "integer", nullable: false),
                    ichibufutan = table.Column<int>(name: "ichibu_futan", type: "integer", nullable: false),
                    genmengaku = table.Column<int>(name: "genmen_gaku", type: "integer", nullable: false),
                    hokenfutan10en = table.Column<int>(name: "hoken_futan_10en", type: "integer", nullable: false),
                    kogakufutan10en = table.Column<int>(name: "kogaku_futan_10en", type: "integer", nullable: false),
                    kohi1futan10en = table.Column<int>(name: "kohi1_futan_10en", type: "integer", nullable: false),
                    kohi2futan10en = table.Column<int>(name: "kohi2_futan_10en", type: "integer", nullable: false),
                    kohi3futan10en = table.Column<int>(name: "kohi3_futan_10en", type: "integer", nullable: false),
                    kohi4futan10en = table.Column<int>(name: "kohi4_futan_10en", type: "integer", nullable: false),
                    ichibufutan10en = table.Column<int>(name: "ichibu_futan_10en", type: "integer", nullable: false),
                    genmengaku10en = table.Column<int>(name: "genmen_gaku_10en", type: "integer", nullable: false),
                    ptfutan = table.Column<int>(name: "pt_futan", type: "integer", nullable: false),
                    kogakuoverkbn = table.Column<int>(name: "kogaku_over_kbn", type: "integer", nullable: false),
                    hokentensu = table.Column<int>(name: "hoken_tensu", type: "integer", nullable: false),
                    hokenichibufutan = table.Column<int>(name: "hoken_ichibu_futan", type: "integer", nullable: false),
                    hokenichibufutan10en = table.Column<int>(name: "hoken_ichibu_futan_10en", type: "integer", nullable: false),
                    kohi1tensu = table.Column<int>(name: "kohi1_tensu", type: "integer", nullable: false),
                    kohi1ichibusotogaku = table.Column<int>(name: "kohi1_ichibu_sotogaku", type: "integer", nullable: false),
                    kohi1ichibusotogaku10en = table.Column<int>(name: "kohi1_ichibu_sotogaku_10en", type: "integer", nullable: false),
                    kohi1ichibufutan = table.Column<int>(name: "kohi1_ichibu_futan", type: "integer", nullable: false),
                    kohi2tensu = table.Column<int>(name: "kohi2_tensu", type: "integer", nullable: false),
                    kohi2ichibusotogaku = table.Column<int>(name: "kohi2_ichibu_sotogaku", type: "integer", nullable: false),
                    kohi2ichibusotogaku10en = table.Column<int>(name: "kohi2_ichibu_sotogaku_10en", type: "integer", nullable: false),
                    kohi2ichibufutan = table.Column<int>(name: "kohi2_ichibu_futan", type: "integer", nullable: false),
                    kohi3tensu = table.Column<int>(name: "kohi3_tensu", type: "integer", nullable: false),
                    kohi3ichibusotogaku = table.Column<int>(name: "kohi3_ichibu_sotogaku", type: "integer", nullable: false),
                    kohi3ichibusotogaku10en = table.Column<int>(name: "kohi3_ichibu_sotogaku_10en", type: "integer", nullable: false),
                    kohi3ichibufutan = table.Column<int>(name: "kohi3_ichibu_futan", type: "integer", nullable: false),
                    kohi4tensu = table.Column<int>(name: "kohi4_tensu", type: "integer", nullable: false),
                    kohi4ichibusotogaku = table.Column<int>(name: "kohi4_ichibu_sotogaku", type: "integer", nullable: false),
                    kohi4ichibusotogaku10en = table.Column<int>(name: "kohi4_ichibu_sotogaku_10en", type: "integer", nullable: false),
                    kohi4ichibufutan = table.Column<int>(name: "kohi4_ichibu_futan", type: "integer", nullable: false),
                    totalichibufutan = table.Column<int>(name: "total_ichibu_futan", type: "integer", nullable: false),
                    totalichibufutan10en = table.Column<int>(name: "total_ichibu_futan_10en", type: "integer", nullable: false),
                    hokenrecetensu = table.Column<int>(name: "hoken_rece_tensu", type: "integer", nullable: true),
                    hokenrecefutan = table.Column<int>(name: "hoken_rece_futan", type: "integer", nullable: true),
                    kohi1recetensu = table.Column<int>(name: "kohi1_rece_tensu", type: "integer", nullable: true),
                    kohi1recefutan = table.Column<int>(name: "kohi1_rece_futan", type: "integer", nullable: true),
                    kohi1recekyufu = table.Column<int>(name: "kohi1_rece_kyufu", type: "integer", nullable: true),
                    kohi2recetensu = table.Column<int>(name: "kohi2_rece_tensu", type: "integer", nullable: true),
                    kohi2recefutan = table.Column<int>(name: "kohi2_rece_futan", type: "integer", nullable: true),
                    kohi2recekyufu = table.Column<int>(name: "kohi2_rece_kyufu", type: "integer", nullable: true),
                    kohi3recetensu = table.Column<int>(name: "kohi3_rece_tensu", type: "integer", nullable: true),
                    kohi3recefutan = table.Column<int>(name: "kohi3_rece_futan", type: "integer", nullable: true),
                    kohi3recekyufu = table.Column<int>(name: "kohi3_rece_kyufu", type: "integer", nullable: true),
                    kohi4recetensu = table.Column<int>(name: "kohi4_rece_tensu", type: "integer", nullable: true),
                    kohi4recefutan = table.Column<int>(name: "kohi4_rece_futan", type: "integer", nullable: true),
                    kohi4recekyufu = table.Column<int>(name: "kohi4_rece_kyufu", type: "integer", nullable: true),
                    hokennissu = table.Column<int>(name: "hoken_nissu", type: "integer", nullable: true),
                    kohi1nissu = table.Column<int>(name: "kohi1_nissu", type: "integer", nullable: true),
                    kohi2nissu = table.Column<int>(name: "kohi2_nissu", type: "integer", nullable: true),
                    kohi3nissu = table.Column<int>(name: "kohi3_nissu", type: "integer", nullable: true),
                    kohi4nissu = table.Column<int>(name: "kohi4_nissu", type: "integer", nullable: true),
                    kohi1recekisai = table.Column<int>(name: "kohi1_rece_kisai", type: "integer", nullable: false),
                    kohi2recekisai = table.Column<int>(name: "kohi2_rece_kisai", type: "integer", nullable: false),
                    kohi3recekisai = table.Column<int>(name: "kohi3_rece_kisai", type: "integer", nullable: false),
                    kohi4recekisai = table.Column<int>(name: "kohi4_rece_kisai", type: "integer", nullable: false),
                    kohi1namecd = table.Column<string>(name: "kohi1_name_cd", type: "character varying(5)", maxLength: 5, nullable: true),
                    kohi2namecd = table.Column<string>(name: "kohi2_name_cd", type: "character varying(5)", maxLength: 5, nullable: true),
                    kohi3namecd = table.Column<string>(name: "kohi3_name_cd", type: "character varying(5)", maxLength: 5, nullable: true),
                    kohi4namecd = table.Column<string>(name: "kohi4_name_cd", type: "character varying(5)", maxLength: 5, nullable: true),
                    seikyukbn = table.Column<int>(name: "seikyu_kbn", type: "integer", nullable: false),
                    tokki = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ptstatus = table.Column<string>(name: "pt_status", type: "character varying(60)", maxLength: 60, nullable: true),
                    rousaiifutan = table.Column<int>(name: "rousai_i_futan", type: "integer", nullable: false),
                    rousairofutan = table.Column<int>(name: "rousai_ro_futan", type: "integer", nullable: false),
                    jibaiitensu = table.Column<int>(name: "jibai_i_tensu", type: "integer", nullable: false),
                    jibairotensu = table.Column<int>(name: "jibai_ro_tensu", type: "integer", nullable: false),
                    jibaihafutan = table.Column<int>(name: "jibai_ha_futan", type: "integer", nullable: false),
                    jibainifutan = table.Column<int>(name: "jibai_ni_futan", type: "integer", nullable: false),
                    jibaihosindan = table.Column<int>(name: "jibai_ho_sindan", type: "integer", nullable: false),
                    jibaihosindancount = table.Column<int>(name: "jibai_ho_sindan_count", type: "integer", nullable: false),
                    jibaihemeisai = table.Column<int>(name: "jibai_he_meisai", type: "integer", nullable: false),
                    jibaihemeisaicount = table.Column<int>(name: "jibai_he_meisai_count", type: "integer", nullable: false),
                    jibaiafutan = table.Column<int>(name: "jibai_a_futan", type: "integer", nullable: false),
                    jibaibfutan = table.Column<int>(name: "jibai_b_futan", type: "integer", nullable: false),
                    jibaicfutan = table.Column<int>(name: "jibai_c_futan", type: "integer", nullable: false),
                    jibaidfutan = table.Column<int>(name: "jibai_d_futan", type: "integer", nullable: false),
                    jibaikenpotensu = table.Column<int>(name: "jibai_kenpo_tensu", type: "integer", nullable: false),
                    jibaikenpofutan = table.Column<int>(name: "jibai_kenpo_futan", type: "integer", nullable: false),
                    sinkei = table.Column<int>(type: "integer", nullable: false),
                    tenki = table.Column<int>(type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    istester = table.Column<int>(name: "is_tester", type: "integer", nullable: false),
                    iszaiiso = table.Column<int>(name: "is_zaiiso", type: "integer", nullable: false),
                    chokikbn = table.Column<int>(name: "choki_kbn", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_inf", x => new { x.hpid, x.seikyuym, x.ptid, x.sinym, x.hokenid });
                });

            migrationBuilder.CreateTable(
                name: "rece_inf_edit",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recesbt = table.Column<string>(name: "rece_sbt", type: "character varying(4)", maxLength: 4, nullable: true),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1houbetu = table.Column<string>(name: "kohi1_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi2houbetu = table.Column<string>(name: "kohi2_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi3houbetu = table.Column<string>(name: "kohi3_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi4houbetu = table.Column<string>(name: "kohi4_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    hokenrecetensu = table.Column<int>(name: "hoken_rece_tensu", type: "integer", nullable: true),
                    hokenrecefutan = table.Column<int>(name: "hoken_rece_futan", type: "integer", nullable: true),
                    kohi1recetensu = table.Column<int>(name: "kohi1_rece_tensu", type: "integer", nullable: true),
                    kohi1recefutan = table.Column<int>(name: "kohi1_rece_futan", type: "integer", nullable: true),
                    kohi1recekyufu = table.Column<int>(name: "kohi1_rece_kyufu", type: "integer", nullable: true),
                    kohi2recetensu = table.Column<int>(name: "kohi2_rece_tensu", type: "integer", nullable: true),
                    kohi2recefutan = table.Column<int>(name: "kohi2_rece_futan", type: "integer", nullable: true),
                    kohi2recekyufu = table.Column<int>(name: "kohi2_rece_kyufu", type: "integer", nullable: true),
                    kohi3recetensu = table.Column<int>(name: "kohi3_rece_tensu", type: "integer", nullable: true),
                    kohi3recefutan = table.Column<int>(name: "kohi3_rece_futan", type: "integer", nullable: true),
                    kohi3recekyufu = table.Column<int>(name: "kohi3_rece_kyufu", type: "integer", nullable: true),
                    kohi4recetensu = table.Column<int>(name: "kohi4_rece_tensu", type: "integer", nullable: true),
                    kohi4recefutan = table.Column<int>(name: "kohi4_rece_futan", type: "integer", nullable: true),
                    kohi4recekyufu = table.Column<int>(name: "kohi4_rece_kyufu", type: "integer", nullable: true),
                    hokennissu = table.Column<int>(name: "hoken_nissu", type: "integer", nullable: true),
                    kohi1nissu = table.Column<int>(name: "kohi1_nissu", type: "integer", nullable: true),
                    kohi2nissu = table.Column<int>(name: "kohi2_nissu", type: "integer", nullable: true),
                    kohi3nissu = table.Column<int>(name: "kohi3_nissu", type: "integer", nullable: true),
                    kohi4nissu = table.Column<int>(name: "kohi4_nissu", type: "integer", nullable: true),
                    tokki = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_inf_edit", x => new { x.hpid, x.seikyuym, x.ptid, x.sinym, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rece_inf_jd",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    kohiid = table.Column<int>(name: "kohi_id", type: "integer", nullable: false),
                    futansbtcd = table.Column<int>(name: "futan_sbt_cd", type: "integer", nullable: false),
                    nissu1 = table.Column<int>(type: "integer", nullable: false),
                    nissu2 = table.Column<int>(type: "integer", nullable: false),
                    nissu3 = table.Column<int>(type: "integer", nullable: false),
                    nissu4 = table.Column<int>(type: "integer", nullable: false),
                    nissu5 = table.Column<int>(type: "integer", nullable: false),
                    nissu6 = table.Column<int>(type: "integer", nullable: false),
                    nissu7 = table.Column<int>(type: "integer", nullable: false),
                    nissu8 = table.Column<int>(type: "integer", nullable: false),
                    nissu9 = table.Column<int>(type: "integer", nullable: false),
                    nissu10 = table.Column<int>(type: "integer", nullable: false),
                    nissu11 = table.Column<int>(type: "integer", nullable: false),
                    nissu12 = table.Column<int>(type: "integer", nullable: false),
                    nissu13 = table.Column<int>(type: "integer", nullable: false),
                    nissu14 = table.Column<int>(type: "integer", nullable: false),
                    nissu15 = table.Column<int>(type: "integer", nullable: false),
                    nissu16 = table.Column<int>(type: "integer", nullable: false),
                    nissu17 = table.Column<int>(type: "integer", nullable: false),
                    nissu18 = table.Column<int>(type: "integer", nullable: false),
                    nissu19 = table.Column<int>(type: "integer", nullable: false),
                    nissu20 = table.Column<int>(type: "integer", nullable: false),
                    nissu21 = table.Column<int>(type: "integer", nullable: false),
                    nissu22 = table.Column<int>(type: "integer", nullable: false),
                    nissu23 = table.Column<int>(type: "integer", nullable: false),
                    nissu24 = table.Column<int>(type: "integer", nullable: false),
                    nissu25 = table.Column<int>(type: "integer", nullable: false),
                    nissu26 = table.Column<int>(type: "integer", nullable: false),
                    nissu27 = table.Column<int>(type: "integer", nullable: false),
                    nissu28 = table.Column<int>(type: "integer", nullable: false),
                    nissu29 = table.Column<int>(type: "integer", nullable: false),
                    nissu30 = table.Column<int>(type: "integer", nullable: false),
                    nissu31 = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_inf_jd", x => new { x.hpid, x.seikyuym, x.ptid, x.sinym, x.hokenid, x.kohiid });
                });

            migrationBuilder.CreateTable(
                name: "rece_inf_pre_edit",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    recesbt = table.Column<string>(name: "rece_sbt", type: "character varying(4)", maxLength: 4, nullable: true),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1houbetu = table.Column<string>(name: "kohi1_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi2houbetu = table.Column<string>(name: "kohi2_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi3houbetu = table.Column<string>(name: "kohi3_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi4houbetu = table.Column<string>(name: "kohi4_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    hokenrecetensu = table.Column<int>(name: "hoken_rece_tensu", type: "integer", nullable: true),
                    hokenrecefutan = table.Column<int>(name: "hoken_rece_futan", type: "integer", nullable: true),
                    kohi1recetensu = table.Column<int>(name: "kohi1_rece_tensu", type: "integer", nullable: true),
                    kohi1recefutan = table.Column<int>(name: "kohi1_rece_futan", type: "integer", nullable: true),
                    kohi1recekyufu = table.Column<int>(name: "kohi1_rece_kyufu", type: "integer", nullable: true),
                    kohi2recetensu = table.Column<int>(name: "kohi2_rece_tensu", type: "integer", nullable: true),
                    kohi2recefutan = table.Column<int>(name: "kohi2_rece_futan", type: "integer", nullable: true),
                    kohi2recekyufu = table.Column<int>(name: "kohi2_rece_kyufu", type: "integer", nullable: true),
                    kohi3recetensu = table.Column<int>(name: "kohi3_rece_tensu", type: "integer", nullable: true),
                    kohi3recefutan = table.Column<int>(name: "kohi3_rece_futan", type: "integer", nullable: true),
                    kohi3recekyufu = table.Column<int>(name: "kohi3_rece_kyufu", type: "integer", nullable: true),
                    kohi4recetensu = table.Column<int>(name: "kohi4_rece_tensu", type: "integer", nullable: true),
                    kohi4recefutan = table.Column<int>(name: "kohi4_rece_futan", type: "integer", nullable: true),
                    kohi4recekyufu = table.Column<int>(name: "kohi4_rece_kyufu", type: "integer", nullable: true),
                    hokennissu = table.Column<int>(name: "hoken_nissu", type: "integer", nullable: true),
                    kohi1nissu = table.Column<int>(name: "kohi1_nissu", type: "integer", nullable: true),
                    kohi2nissu = table.Column<int>(name: "kohi2_nissu", type: "integer", nullable: true),
                    kohi3nissu = table.Column<int>(name: "kohi3_nissu", type: "integer", nullable: true),
                    kohi4nissu = table.Column<int>(name: "kohi4_nissu", type: "integer", nullable: true),
                    tokki = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_inf_pre_edit", x => new { x.hpid, x.seikyuym, x.ptid, x.sinym, x.hokenid });
                });

            migrationBuilder.CreateTable(
                name: "rece_seikyu",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    seikyukbn = table.Column<int>(name: "seikyu_kbn", type: "integer", nullable: false),
                    prehokenid = table.Column<int>(name: "pre_hoken_id", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_seikyu", x => new { x.hpid, x.sinym, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rece_status",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    fusenkbn = table.Column<int>(name: "fusen_kbn", type: "integer", nullable: false),
                    ispaperrece = table.Column<int>(name: "is_paper_rece", type: "integer", nullable: false),
                    isprechecked = table.Column<int>(name: "is_prechecked", type: "integer", nullable: false),
                    output = table.Column<int>(type: "integer", nullable: false),
                    statuskbn = table.Column<int>(name: "status_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rece_status", x => new { x.hpid, x.ptid, x.seikyuym, x.hokenid, x.sinym });
                });

            migrationBuilder.CreateTable(
                name: "receden_cmt_select",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemno = table.Column<int>(name: "item_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    commentcd = table.Column<string>(name: "comment_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    kbnno = table.Column<string>(name: "kbn_no", type: "character varying(64)", maxLength: 64, nullable: true),
                    ptstatus = table.Column<int>(name: "pt_status", type: "integer", nullable: false),
                    condkbn = table.Column<int>(name: "cond_kbn", type: "integer", nullable: false),
                    notsanteikbn = table.Column<int>(name: "not_santei_kbn", type: "integer", nullable: false),
                    nyugaikbn = table.Column<int>(name: "nyugai_kbn", type: "integer", nullable: false),
                    santeicnt = table.Column<int>(name: "santei_cnt", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receden_cmt_select", x => new { x.hpid, x.itemno, x.edano, x.itemcd, x.startdate, x.commentcd });
                });

            migrationBuilder.CreateTable(
                name: "receden_hen_jiyuu",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    henreijiyuucd = table.Column<string>(name: "henrei_jiyuu_cd", type: "character varying(9)", maxLength: 9, nullable: true),
                    henreijiyuu = table.Column<string>(name: "henrei_jiyuu", type: "text", nullable: true),
                    hosoku = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receden_hen_jiyuu", x => new { x.hpid, x.ptid, x.hokenid, x.sinym, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "receden_rireki_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    searchno = table.Column<string>(name: "search_no", type: "character varying(30)", maxLength: 30, nullable: true),
                    rireki = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receden_rireki_inf", x => new { x.hpid, x.ptid, x.hokenid, x.sinym, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "releasenote_read",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    version = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_releasenote_read", x => new { x.hpid, x.userid, x.version });
                });

            migrationBuilder.CreateTable(
                name: "renkei_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    renkeiid = table.Column<int>(name: "renkei_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    param = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ptnumlength = table.Column<int>(name: "pt_num_length", type: "integer", nullable: false),
                    templateid = table.Column<int>(name: "template_id", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_conf", x => new { x.hpid, x.id });
                });

            migrationBuilder.CreateTable(
                name: "renkei_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    renkeiid = table.Column<int>(name: "renkei_id", type: "integer", nullable: false),
                    renkeiname = table.Column<string>(name: "renkei_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    renkeisbt = table.Column<int>(name: "renkei_sbt", type: "integer", nullable: false),
                    functiontype = table.Column<int>(name: "function_type", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_mst", x => new { x.hpid, x.renkeiid });
                });

            migrationBuilder.CreateTable(
                name: "renkei_path_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    renkeiid = table.Column<int>(name: "renkei_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    machine = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    charcd = table.Column<int>(name: "char_cd", type: "integer", nullable: false),
                    workpath = table.Column<string>(name: "work_path", type: "character varying(300)", maxLength: 300, nullable: true),
                    interval = table.Column<int>(type: "integer", nullable: false),
                    param = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    user = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_path_conf", x => new { x.hpid, x.edano, x.id });
                });

            migrationBuilder.CreateTable(
                name: "renkei_req",
                columns: table => new
                {
                    reqid = table.Column<long>(name: "req_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    reqsbt = table.Column<int>(name: "req_sbt", type: "integer", nullable: false),
                    reqtype = table.Column<string>(name: "req_type", type: "character varying(2)", maxLength: 2, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    errmst = table.Column<string>(name: "err_mst", type: "character varying(120)", maxLength: 120, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_req", x => x.reqid);
                });

            migrationBuilder.CreateTable(
                name: "renkei_template_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    templateid = table.Column<int>(name: "template_id", type: "integer", nullable: false),
                    templatename = table.Column<string>(name: "template_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    param = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    file = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_template_mst", x => new { x.hpid, x.templateid });
                });

            migrationBuilder.CreateTable(
                name: "renkei_timing_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    renkeiid = table.Column<int>(name: "renkei_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    eventcd = table.Column<string>(name: "event_cd", type: "character varying(11)", maxLength: 11, nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_timing_conf", x => new { x.hpid, x.eventcd, x.id });
                });

            migrationBuilder.CreateTable(
                name: "renkei_timing_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    renkeiid = table.Column<int>(name: "renkei_id", type: "integer", nullable: false),
                    eventcd = table.Column<string>(name: "event_cd", type: "character varying(11)", maxLength: 11, nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renkei_timing_mst", x => new { x.hpid, x.renkeiid, x.eventcd });
                });

            migrationBuilder.CreateTable(
                name: "roudou_mst",
                columns: table => new
                {
                    roudoucd = table.Column<string>(name: "roudou_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    roudouname = table.Column<string>(name: "roudou_name", type: "character varying(60)", maxLength: 60, nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roudou_mst", x => x.roudoucd);
                });

            migrationBuilder.CreateTable(
                name: "rousai_gosei_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    goseigrp = table.Column<int>(name: "gosei_grp", type: "integer", nullable: false),
                    goseiitemcd = table.Column<string>(name: "gosei_item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    sisikbn = table.Column<int>(name: "sisi_kbn", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rousai_gosei_mst", x => new { x.hpid, x.goseigrp, x.goseiitemcd, x.itemcd, x.sisikbn, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "rsv_day_comment",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_day_comment", x => new { x.hpid, x.sindate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rsv_frame_day_ptn",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    endtime = table.Column<int>(name: "end_time", type: "integer", nullable: false),
                    minutes = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    isholiday = table.Column<int>(name: "is_holiday", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_frame_day_ptn", x => new { x.hpid, x.rsvframeid, x.sindate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rsv_frame_inf",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    endtime = table.Column<int>(name: "end_time", type: "integer", nullable: false),
                    frameno = table.Column<int>(name: "frame_no", type: "integer", nullable: false),
                    isholiday = table.Column<int>(name: "is_holiday", type: "integer", nullable: false),
                    number = table.Column<long>(type: "bigint", nullable: false),
                    framesbt = table.Column<int>(name: "frame_sbt", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_frame_inf", x => new { x.hpid, x.rsvframeid, x.sindate, x.starttime, x.id });
                });

            migrationBuilder.CreateTable(
                name: "rsv_frame_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rsvgrpid = table.Column<int>(name: "rsv_grp_id", type: "integer", nullable: false),
                    sortkey = table.Column<int>(name: "sort_key", type: "integer", nullable: false),
                    rsvframename = table.Column<string>(name: "rsv_frame_name", type: "character varying(60)", maxLength: 60, nullable: true),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    makeraiin = table.Column<int>(name: "make_raiin", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_frame_mst", x => new { x.hpid, x.rsvframeid });
                });

            migrationBuilder.CreateTable(
                name: "rsv_frame_week_ptn",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    week = table.Column<int>(type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    endtime = table.Column<int>(name: "end_time", type: "integer", nullable: false),
                    minutes = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    isholiday = table.Column<int>(name: "is_holiday", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_frame_week_ptn", x => new { x.id, x.hpid, x.rsvframeid, x.week, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rsv_frame_with",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false),
                    withframeid = table.Column<int>(name: "with_frame_id", type: "integer", nullable: false),
                    sortkey = table.Column<int>(name: "sort_key", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_frame_with", x => new { x.id, x.hpid, x.rsvframeid });
                });

            migrationBuilder.CreateTable(
                name: "rsv_grp_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvgrpid = table.Column<int>(name: "rsv_grp_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortkey = table.Column<int>(name: "sort_key", type: "integer", nullable: false),
                    rsvgrpname = table.Column<string>(name: "rsv_grp_name", type: "character varying(60)", maxLength: 60, nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_grp_mst", x => new { x.hpid, x.rsvgrpid });
                });

            migrationBuilder.CreateTable(
                name: "rsv_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvsbt = table.Column<int>(name: "rsv_sbt", type: "integer", nullable: false),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_inf", x => new { x.hpid, x.rsvframeid, x.sindate, x.starttime, x.raiinno });
                });

            migrationBuilder.CreateTable(
                name: "rsv_renkei_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    otherseqno = table.Column<long>(name: "other_seq_no", type: "bigint", nullable: false),
                    otherseqno2 = table.Column<long>(name: "other_seq_no2", type: "bigint", nullable: false),
                    otherptid = table.Column<long>(name: "other_pt_id", type: "bigint", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_renkei_inf", x => new { x.hpid, x.raiinno });
                });

            migrationBuilder.CreateTable(
                name: "rsv_renkei_inf_tk",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    systemkbn = table.Column<int>(name: "system_kbn", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    otherseqno = table.Column<long>(name: "other_seq_no", type: "bigint", nullable: false),
                    otherseqno2 = table.Column<long>(name: "other_seq_no2", type: "bigint", nullable: false),
                    otherptid = table.Column<long>(name: "other_pt_id", type: "bigint", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsv_renkei_inf_tk", x => new { x.hpid, x.raiinno, x.systemkbn });
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_byomei",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd1 = table.Column<string>(name: "syusyoku_cd1", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd2 = table.Column<string>(name: "syusyoku_cd2", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd3 = table.Column<string>(name: "syusyoku_cd3", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd4 = table.Column<string>(name: "syusyoku_cd4", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd5 = table.Column<string>(name: "syusyoku_cd5", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd6 = table.Column<string>(name: "syusyoku_cd6", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd7 = table.Column<string>(name: "syusyoku_cd7", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd8 = table.Column<string>(name: "syusyoku_cd8", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd9 = table.Column<string>(name: "syusyoku_cd9", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd10 = table.Column<string>(name: "syusyoku_cd10", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd11 = table.Column<string>(name: "syusyoku_cd11", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd12 = table.Column<string>(name: "syusyoku_cd12", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd13 = table.Column<string>(name: "syusyoku_cd13", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd14 = table.Column<string>(name: "syusyoku_cd14", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd15 = table.Column<string>(name: "syusyoku_cd15", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd16 = table.Column<string>(name: "syusyoku_cd16", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd17 = table.Column<string>(name: "syusyoku_cd17", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd18 = table.Column<string>(name: "syusyoku_cd18", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd19 = table.Column<string>(name: "syusyoku_cd19", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd20 = table.Column<string>(name: "syusyoku_cd20", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd21 = table.Column<string>(name: "syusyoku_cd21", type: "character varying(7)", maxLength: 7, nullable: true),
                    byomei = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    syubyokbn = table.Column<int>(name: "syubyo_kbn", type: "integer", nullable: false),
                    sikkankbn = table.Column<int>(name: "sikkan_kbn", type: "integer", nullable: false),
                    nanbyocd = table.Column<int>(name: "nanbyo_cd", type: "integer", nullable: false),
                    hosokucmt = table.Column<string>(name: "hosoku_cmt", type: "character varying(80)", maxLength: 80, nullable: true),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodspkarte = table.Column<int>(name: "is_nodsp_karte", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_byomei", x => new { x.hpid, x.ptid, x.rsvkrtno, x.seqno, x.id });
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_karte_img_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    position = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_karte_img_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_karte_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    rsvdate = table.Column<int>(name: "rsv_date", type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    richtext = table.Column<byte[]>(name: "rich_text", type: "bytea", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_karte_inf", x => new { x.hpid, x.ptid, x.rsvkrtno, x.kartekbn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rsvkrtkbn = table.Column<int>(name: "rsvkrt_kbn", type: "integer", nullable: false),
                    rsvdate = table.Column<int>(name: "rsv_date", type: "integer", nullable: false),
                    rsvname = table.Column<string>(name: "rsv_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_mst", x => new { x.hpid, x.ptid, x.rsvkrtno });
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_odr_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rsvdate = table.Column<int>(name: "rsv_date", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    odrkouikbn = table.Column<int>(name: "odr_koui_kbn", type: "integer", nullable: false),
                    rpname = table.Column<string>(name: "rp_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    syohosbt = table.Column<int>(name: "syoho_sbt", type: "integer", nullable: false),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    dayscnt = table.Column<int>(name: "days_cnt", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_odr_inf", x => new { x.hpid, x.ptid, x.rsvkrtno, x.rpno, x.rpedano, x.id });
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_odr_inf_cmt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    rsvdate = table.Column<int>(name: "rsv_date", type: "integer", nullable: false),
                    fontcolor = table.Column<int>(name: "font_color", type: "integer", nullable: false),
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(32)", maxLength: 32, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_odr_inf_cmt", x => new { x.hpid, x.ptid, x.rsvkrtno, x.rpedano, x.rpno, x.rowno, x.edano });
                });

            migrationBuilder.CreateTable(
                name: "rsvkrt_odr_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvkrtno = table.Column<long>(name: "rsvkrt_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    rsvdate = table.Column<int>(name: "rsv_date", type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    unitsbt = table.Column<int>(name: "unit_sbt", type: "integer", nullable: false),
                    termval = table.Column<double>(name: "term_val", type: "double precision", nullable: false),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    syohokbn = table.Column<int>(name: "syoho_kbn", type: "integer", nullable: false),
                    syoholimitkbn = table.Column<int>(name: "syoho_limit_kbn", type: "integer", nullable: false),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    kokuji1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kokuji2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    ipncd = table.Column<string>(name: "ipn_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    bunkatu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true),
                    fontcolor = table.Column<string>(name: "font_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    commentnewline = table.Column<int>(name: "comment_newline", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rsvkrt_odr_inf_detail", x => new { x.hpid, x.ptid, x.rsvkrtno, x.rpno, x.rpedano, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "santei_auto_order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    santeigrpcd = table.Column<int>(name: "santei_grp_cd", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    addtype = table.Column<int>(name: "add_type", type: "integer", nullable: false),
                    addtarget = table.Column<int>(name: "add_target", type: "integer", nullable: false),
                    termcnt = table.Column<int>(name: "term_cnt", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false),
                    cnttype = table.Column<int>(name: "cnt_type", type: "integer", nullable: false),
                    maxcnt = table.Column<long>(name: "max_cnt", type: "bigint", nullable: false),
                    spcondition = table.Column<int>(name: "sp_condition", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_auto_order", x => new { x.id, x.hpid, x.santeigrpcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "santei_auto_order_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    santeigrpcd = table.Column<int>(name: "santei_grp_cd", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_auto_order_detail", x => new { x.id, x.hpid, x.santeigrpcd, x.seqno, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "santei_cnt_check",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    santeigrpcd = table.Column<int>(name: "santei_grp_cd", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    termcnt = table.Column<int>(name: "term_cnt", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false),
                    cnttype = table.Column<int>(name: "cnt_type", type: "integer", nullable: false),
                    maxcnt = table.Column<long>(name: "max_cnt", type: "bigint", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(10)", maxLength: 10, nullable: true),
                    errkbn = table.Column<int>(name: "err_kbn", type: "integer", nullable: false),
                    targetcd = table.Column<string>(name: "target_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    spcondition = table.Column<int>(name: "sp_condition", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_cnt_check", x => new { x.hpid, x.santeigrpcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "santei_grp_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    santeigrpcd = table.Column<int>(name: "santei_grp_cd", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "text", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_grp_detail", x => new { x.hpid, x.santeigrpcd, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "santei_grp_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    santeigrpcd = table.Column<int>(name: "santei_grp_cd", type: "integer", nullable: false),
                    santeigrpname = table.Column<string>(name: "santei_grp_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_grp_mst", x => new { x.hpid, x.santeigrpcd });
                });

            migrationBuilder.CreateTable(
                name: "santei_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    alertdays = table.Column<int>(name: "alert_days", type: "integer", nullable: false),
                    alertterm = table.Column<int>(name: "alert_term", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "santei_inf_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    kisansbt = table.Column<int>(name: "kisan_sbt", type: "integer", nullable: false),
                    kisandate = table.Column<int>(name: "kisan_date", type: "integer", nullable: false),
                    byomei = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    hosokucomment = table.Column<string>(name: "hosoku_comment", type: "character varying(80)", maxLength: 80, nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santei_inf_detail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schema_cmt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    commentcd = table.Column<int>(name: "comment_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comment = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schema_cmt_mst", x => new { x.hpid, x.commentcd });
                });

            migrationBuilder.CreateTable(
                name: "seikatureki_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seikatureki_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sentence_list",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sentencecd = table.Column<int>(name: "sentence_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sentence = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    setkbn = table.Column<int>(name: "set_kbn", type: "integer", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    level1 = table.Column<long>(type: "bigint", nullable: false),
                    level2 = table.Column<long>(type: "bigint", nullable: false),
                    level3 = table.Column<long>(type: "bigint", nullable: false),
                    selecttype = table.Column<int>(name: "select_type", type: "integer", nullable: false),
                    newline = table.Column<int>(name: "new_line", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sentence_list", x => new { x.hpid, x.sentence });
                });

            migrationBuilder.CreateTable(
                name: "session_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    machine = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    logindate = table.Column<DateTime>(name: "login_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_inf", x => new { x.hpid, x.machine });
                });

            migrationBuilder.CreateTable(
                name: "set_byomei",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd1 = table.Column<string>(name: "syusyoku_cd1", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd2 = table.Column<string>(name: "syusyoku_cd2", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd3 = table.Column<string>(name: "syusyoku_cd3", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd4 = table.Column<string>(name: "syusyoku_cd4", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd5 = table.Column<string>(name: "syusyoku_cd5", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd6 = table.Column<string>(name: "syusyoku_cd6", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd7 = table.Column<string>(name: "syusyoku_cd7", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd8 = table.Column<string>(name: "syusyoku_cd8", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd9 = table.Column<string>(name: "syusyoku_cd9", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd10 = table.Column<string>(name: "syusyoku_cd10", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd11 = table.Column<string>(name: "syusyoku_cd11", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd12 = table.Column<string>(name: "syusyoku_cd12", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd13 = table.Column<string>(name: "syusyoku_cd13", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd14 = table.Column<string>(name: "syusyoku_cd14", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd15 = table.Column<string>(name: "syusyoku_cd15", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd16 = table.Column<string>(name: "syusyoku_cd16", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd17 = table.Column<string>(name: "syusyoku_cd17", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd18 = table.Column<string>(name: "syusyoku_cd18", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd19 = table.Column<string>(name: "syusyoku_cd19", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd20 = table.Column<string>(name: "syusyoku_cd20", type: "character varying(7)", maxLength: 7, nullable: true),
                    syusyokucd21 = table.Column<string>(name: "syusyoku_cd21", type: "character varying(7)", maxLength: 7, nullable: true),
                    byomei = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    syubyokbn = table.Column<int>(name: "syubyo_kbn", type: "integer", nullable: false),
                    sikkankbn = table.Column<int>(name: "sikkan_kbn", type: "integer", nullable: false),
                    nanbyocd = table.Column<int>(name: "nanbyo_cd", type: "integer", nullable: false),
                    hosokucmt = table.Column<string>(name: "hosoku_cmt", type: "character varying(80)", maxLength: 80, nullable: true),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodspkarte = table.Column<int>(name: "is_nodsp_karte", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_byomei", x => new { x.id, x.hpid, x.setcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "set_generation_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_generation_mst", x => new { x.hpid, x.generationid });
                });

            migrationBuilder.CreateTable(
                name: "set_karte_img_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    position = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_karte_img_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "set_karte_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    richtext = table.Column<byte[]>(name: "rich_text", type: "bytea", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_karte_inf", x => new { x.hpid, x.setcd, x.kartekbn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "set_kbn_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setkbn = table.Column<int>(name: "set_kbn", type: "integer", nullable: false),
                    setkbnedano = table.Column<int>(name: "set_kbn_eda_no", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false),
                    setkbnname = table.Column<string>(name: "set_kbn_name", type: "character varying(60)", maxLength: 60, nullable: true),
                    kacd = table.Column<int>(name: "ka_cd", type: "integer", nullable: false),
                    doccd = table.Column<int>(name: "doc_cd", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_kbn_mst", x => new { x.hpid, x.setkbn, x.setkbnedano, x.generationid });
                });

            migrationBuilder.CreateTable(
                name: "set_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    setkbn = table.Column<int>(name: "set_kbn", type: "integer", nullable: false),
                    setkbnedano = table.Column<int>(name: "set_kbn_eda_no", type: "integer", nullable: false),
                    generationid = table.Column<int>(name: "generation_id", type: "integer", nullable: false),
                    level1 = table.Column<int>(type: "integer", nullable: false),
                    level2 = table.Column<int>(type: "integer", nullable: false),
                    level3 = table.Column<int>(type: "integer", nullable: false),
                    setname = table.Column<string>(name: "set_name", type: "character varying(60)", maxLength: 60, nullable: true),
                    weightkbn = table.Column<int>(name: "weight_kbn", type: "integer", nullable: false),
                    color = table.Column<int>(type: "integer", nullable: false),
                    isgroup = table.Column<int>(name: "is_group", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_mst", x => new { x.hpid, x.setcd });
                });

            migrationBuilder.CreateTable(
                name: "set_odr_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    odrkouikbn = table.Column<int>(name: "odr_koui_kbn", type: "integer", nullable: false),
                    rpname = table.Column<string>(name: "rp_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    syohosbt = table.Column<int>(name: "syoho_sbt", type: "integer", nullable: false),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    dayscnt = table.Column<int>(name: "days_cnt", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_odr_inf", x => new { x.hpid, x.setcd, x.rpno, x.rpedano, x.id });
                });

            migrationBuilder.CreateTable(
                name: "set_odr_inf_cmt",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    fontcolor = table.Column<int>(name: "font_color", type: "integer", nullable: false),
                    cmtcd = table.Column<string>(name: "cmt_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(32)", maxLength: 32, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_odr_inf_cmt", x => new { x.hpid, x.setcd, x.rpno, x.rpedano, x.rowno, x.edano });
                });

            migrationBuilder.CreateTable(
                name: "set_odr_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    setcd = table.Column<int>(name: "set_cd", type: "integer", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    unitsbt = table.Column<int>(name: "unit_sbt", type: "integer", nullable: false),
                    odrtermval = table.Column<double>(name: "odr_term_val", type: "double precision", nullable: false),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    syohokbn = table.Column<int>(name: "syoho_kbn", type: "integer", nullable: false),
                    syoholimitkbn = table.Column<int>(name: "syoho_limit_kbn", type: "integer", nullable: false),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    kokuji1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kokuji2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    ipncd = table.Column<string>(name: "ipn_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    bunkatu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true),
                    fontcolor = table.Column<string>(name: "font_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    commentnewline = table.Column<int>(name: "comment_newline", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_odr_inf_detail", x => new { x.hpid, x.setcd, x.rpno, x.rpedano, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "sin_koui",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    syukeisaki = table.Column<string>(name: "syukei_saki", type: "character varying(4)", maxLength: 4, nullable: true),
                    hokatukensa = table.Column<int>(name: "hokatu_kensa", type: "integer", nullable: false),
                    totalten = table.Column<double>(name: "total_ten", type: "double precision", nullable: false),
                    ten = table.Column<double>(type: "double precision", nullable: false),
                    zei = table.Column<double>(type: "double precision", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    tencount = table.Column<string>(name: "ten_count", type: "character varying(20)", maxLength: 20, nullable: true),
                    tencolcount = table.Column<int>(name: "ten_col_count", type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodsppaperrece = table.Column<int>(name: "is_nodsp_paper_rece", type: "integer", nullable: false),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    entenkbn = table.Column<int>(name: "enten_kbn", type: "integer", nullable: false),
                    cdkbn = table.Column<string>(name: "cd_kbn", type: "character varying(2)", maxLength: 2, nullable: true),
                    recid = table.Column<string>(name: "rec_id", type: "character varying(2)", maxLength: 2, nullable: true),
                    jihisbt = table.Column<int>(name: "jihi_sbt", type: "integer", nullable: false),
                    kazeikbn = table.Column<int>(name: "kazei_kbn", type: "integer", nullable: false),
                    detaildata = table.Column<string>(name: "detail_data", type: "text", nullable: true),
                    day1 = table.Column<int>(type: "integer", nullable: false),
                    day2 = table.Column<int>(type: "integer", nullable: false),
                    day3 = table.Column<int>(type: "integer", nullable: false),
                    day4 = table.Column<int>(type: "integer", nullable: false),
                    day5 = table.Column<int>(type: "integer", nullable: false),
                    day6 = table.Column<int>(type: "integer", nullable: false),
                    day7 = table.Column<int>(type: "integer", nullable: false),
                    day8 = table.Column<int>(type: "integer", nullable: false),
                    day9 = table.Column<int>(type: "integer", nullable: false),
                    day10 = table.Column<int>(type: "integer", nullable: false),
                    day11 = table.Column<int>(type: "integer", nullable: false),
                    day12 = table.Column<int>(type: "integer", nullable: false),
                    day13 = table.Column<int>(type: "integer", nullable: false),
                    day14 = table.Column<int>(type: "integer", nullable: false),
                    day15 = table.Column<int>(type: "integer", nullable: false),
                    day16 = table.Column<int>(type: "integer", nullable: false),
                    day17 = table.Column<int>(type: "integer", nullable: false),
                    day18 = table.Column<int>(type: "integer", nullable: false),
                    day19 = table.Column<int>(type: "integer", nullable: false),
                    day20 = table.Column<int>(type: "integer", nullable: false),
                    day21 = table.Column<int>(type: "integer", nullable: false),
                    day22 = table.Column<int>(type: "integer", nullable: false),
                    day23 = table.Column<int>(type: "integer", nullable: false),
                    day24 = table.Column<int>(type: "integer", nullable: false),
                    day25 = table.Column<int>(type: "integer", nullable: false),
                    day26 = table.Column<int>(type: "integer", nullable: false),
                    day27 = table.Column<int>(type: "integer", nullable: false),
                    day28 = table.Column<int>(type: "integer", nullable: false),
                    day29 = table.Column<int>(type: "integer", nullable: false),
                    day30 = table.Column<int>(type: "integer", nullable: false),
                    day31 = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sin_koui", x => new { x.hpid, x.ptid, x.sinym, x.rpno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "sin_koui_count",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    sinday = table.Column<int>(name: "sin_day", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sin_koui_count", x => new { x.hpid, x.ptid, x.sinym, x.sinday, x.raiinno, x.rpno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "sin_koui_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    recid = table.Column<string>(name: "rec_id", type: "character varying(2)", maxLength: 2, nullable: true),
                    itemsbt = table.Column<int>(name: "item_sbt", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    odritemcd = table.Column<string>(name: "odr_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    suryo2 = table.Column<double>(type: "double precision", nullable: false),
                    fmtkbn = table.Column<int>(name: "fmt_kbn", type: "integer", nullable: false),
                    unitcd = table.Column<int>(name: "unit_cd", type: "integer", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    ten = table.Column<double>(type: "double precision", nullable: false),
                    zei = table.Column<double>(type: "double precision", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodsppaperrece = table.Column<int>(name: "is_nodsp_paper_rece", type: "integer", nullable: false),
                    isnodspryosyu = table.Column<int>(name: "is_nodsp_ryosyu", type: "integer", nullable: false),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmt1 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cmtcd1 = table.Column<string>(name: "cmt_cd1", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtopt1 = table.Column<string>(name: "cmt_opt1", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmt2 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cmtcd2 = table.Column<string>(name: "cmt_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtopt2 = table.Column<string>(name: "cmt_opt2", type: "character varying(240)", maxLength: 240, nullable: true),
                    cmt3 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cmtcd3 = table.Column<string>(name: "cmt_cd3", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtopt3 = table.Column<string>(name: "cmt_opt3", type: "character varying(240)", maxLength: 240, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sin_koui_detail", x => new { x.hpid, x.ptid, x.sinym, x.rpno, x.seqno, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "sin_rp_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstday = table.Column<int>(name: "first_day", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    sinid = table.Column<int>(name: "sin_id", type: "integer", nullable: false),
                    cdno = table.Column<string>(name: "cd_no", type: "character varying(15)", maxLength: 15, nullable: true),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    kouidata = table.Column<string>(name: "koui_data", type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sin_rp_inf", x => new { x.hpid, x.ptid, x.sinym, x.rpno });
                });

            migrationBuilder.CreateTable(
                name: "sin_rp_no_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    sinday = table.Column<int>(name: "sin_day", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sin_rp_no_inf", x => new { x.hpid, x.ptid, x.sinym, x.sinday, x.raiinno, x.rpno });
                });

            migrationBuilder.CreateTable(
                name: "single_dose_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_single_dose_mst", x => new { x.hpid, x.id });
                });

            migrationBuilder.CreateTable(
                name: "sinreki_filter_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sinreki_filter_mst", x => new { x.hpid, x.grpcd });
                });

            migrationBuilder.CreateTable(
                name: "sinreki_filter_mst_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isexclude = table.Column<int>(name: "is_exclude", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sinreki_filter_mst_detail", x => new { x.hpid, x.grpcd, x.id });
                });

            migrationBuilder.CreateTable(
                name: "sinreki_filter_mst_koui",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kouikbnid = table.Column<int>(name: "koui_kbn_id", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sinreki_filter_mst_koui", x => new { x.hpid, x.grpcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "smartkarte_app_signalr_port",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    portnumber = table.Column<int>(name: "port_number", type: "integer", nullable: false),
                    machinename = table.Column<string>(name: "machine_name", type: "character varying(60)", maxLength: 60, nullable: true),
                    ip = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_smartkarte_app_signalr_port", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sokatu_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    startym = table.Column<int>(name: "start_ym", type: "integer", nullable: false),
                    reportid = table.Column<int>(name: "report_id", type: "integer", nullable: false),
                    reportedano = table.Column<int>(name: "report_eda_no", type: "integer", nullable: false),
                    endym = table.Column<int>(name: "end_ym", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    reportname = table.Column<string>(name: "report_name", type: "character varying(30)", maxLength: 30, nullable: true),
                    printtype = table.Column<int>(name: "print_type", type: "integer", nullable: false),
                    printnotype = table.Column<int>(name: "print_no_type", type: "integer", nullable: false),
                    dataall = table.Column<int>(name: "data_all", type: "integer", nullable: false),
                    datadisk = table.Column<int>(name: "data_disk", type: "integer", nullable: false),
                    datapaper = table.Column<int>(name: "data_paper", type: "integer", nullable: false),
                    datakbn = table.Column<int>(name: "data_kbn", type: "integer", nullable: false),
                    diskkind = table.Column<string>(name: "disk_kind", type: "character varying(10)", maxLength: 10, nullable: true),
                    diskcnt = table.Column<int>(name: "disk_cnt", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    issort = table.Column<int>(name: "is_sort", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sokatu_mst", x => new { x.hpid, x.prefno, x.startym, x.reportedano, x.reportid });
                });

            migrationBuilder.CreateTable(
                name: "sta_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    menuid = table.Column<int>(name: "menu_id", type: "integer", nullable: false),
                    confid = table.Column<int>(name: "conf_id", type: "integer", nullable: false),
                    val = table.Column<string>(type: "character varying(1200)", maxLength: 1200, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sta_conf", x => new { x.hpid, x.menuid, x.confid });
                });

            migrationBuilder.CreateTable(
                name: "sta_csv",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    reportid = table.Column<int>(name: "report_id", type: "integer", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    confname = table.Column<string>(name: "conf_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    datasbt = table.Column<int>(name: "data_sbt", type: "integer", nullable: false),
                    columns = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    sortkbn = table.Column<int>(name: "sort_kbn", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sta_csv", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sta_grp",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    reportid = table.Column<int>(name: "report_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sta_grp", x => new { x.hpid, x.grpid, x.reportid });
                });

            migrationBuilder.CreateTable(
                name: "sta_menu",
                columns: table => new
                {
                    menuid = table.Column<int>(name: "menu_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    reportid = table.Column<int>(name: "report_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    menuname = table.Column<string>(name: "menu_name", type: "character varying(130)", maxLength: 130, nullable: true),
                    isprint = table.Column<int>(name: "is_print", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sta_menu", x => x.menuid);
                });

            migrationBuilder.CreateTable(
                name: "sta_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    reportid = table.Column<int>(name: "report_id", type: "integer", nullable: false),
                    reportname = table.Column<string>(name: "report_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sta_mst", x => new { x.hpid, x.reportid });
                });

            migrationBuilder.CreateTable(
                name: "summary_inf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    rtext = table.Column<byte[]>(type: "bytea", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_summary_inf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "syobyo_keika",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    sinday = table.Column<int>(name: "sin_day", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keika = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syobyo_keika", x => new { x.hpid, x.ptid, x.sinym, x.sinday, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "syouki_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    syoukikbn = table.Column<int>(name: "syouki_kbn", type: "integer", nullable: false),
                    syouki = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syouki_inf", x => new { x.hpid, x.ptid, x.sinym, x.hokenid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "syouki_kbn_mst",
                columns: table => new
                {
                    syoukikbn = table.Column<int>(name: "syouki_kbn", type: "integer", nullable: false),
                    startym = table.Column<int>(name: "start_ym", type: "integer", nullable: false),
                    endym = table.Column<int>(name: "end_ym", type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syouki_kbn_mst", x => new { x.syoukikbn, x.startym });
                });

            migrationBuilder.CreateTable(
                name: "system_change_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    filename = table.Column<string>(name: "file_name", type: "text", nullable: true),
                    version = table.Column<string>(type: "text", nullable: true),
                    ispg = table.Column<int>(name: "is_pg", type: "integer", nullable: false),
                    isdb = table.Column<int>(name: "is_db", type: "integer", nullable: false),
                    ismaster = table.Column<int>(name: "is_master", type: "integer", nullable: false),
                    isrun = table.Column<int>(name: "is_run", type: "integer", nullable: false),
                    isnote = table.Column<int>(name: "is_note", type: "integer", nullable: false),
                    isdrugphoto = table.Column<int>(name: "is_drug_photo", type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    errmessage = table.Column<string>(name: "err_message", type: "text", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_change_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    grpedano = table.Column<int>(name: "grp_eda_no", type: "integer", nullable: false),
                    val = table.Column<double>(type: "double precision", nullable: false),
                    param = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_conf", x => new { x.hpid, x.grpcd, x.grpedano });
                });

            migrationBuilder.CreateTable(
                name: "system_conf_item",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    menuid = table.Column<int>(name: "menu_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    val = table.Column<int>(type: "integer", nullable: false),
                    parammin = table.Column<int>(name: "param_min", type: "integer", nullable: false),
                    parammax = table.Column<int>(name: "param_max", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_conf_item", x => new { x.hpid, x.menuid, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "system_conf_menu",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    menuid = table.Column<int>(name: "menu_id", type: "integer", nullable: false),
                    menugrp = table.Column<int>(name: "menu_grp", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    menuname = table.Column<string>(name: "menu_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    grpedano = table.Column<int>(name: "grp_eda_no", type: "integer", nullable: false),
                    pathgrpcd = table.Column<int>(name: "path_grp_cd", type: "integer", nullable: false),
                    isparam = table.Column<int>(name: "is_param", type: "integer", nullable: false),
                    parammask = table.Column<int>(name: "param_mask", type: "integer", nullable: false),
                    paramtype = table.Column<int>(name: "param_type", type: "integer", nullable: false),
                    paramhint = table.Column<string>(name: "param_hint", type: "character varying(100)", maxLength: 100, nullable: true),
                    valmin = table.Column<double>(name: "val_min", type: "double precision", nullable: false),
                    valmax = table.Column<double>(name: "val_max", type: "double precision", nullable: false),
                    parammin = table.Column<double>(name: "param_min", type: "double precision", nullable: false),
                    parammax = table.Column<double>(name: "param_max", type: "double precision", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    isvisible = table.Column<int>(name: "is_visible", type: "integer", nullable: false),
                    managerkbn = table.Column<int>(name: "manager_kbn", type: "integer", nullable: false),
                    isvalue = table.Column<int>(name: "is_value", type: "integer", nullable: false),
                    parammaxlength = table.Column<int>(name: "param_max_length", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_conf_menu", x => new { x.hpid, x.menuid });
                });

            migrationBuilder.CreateTable(
                name: "system_generation_conf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    grpedano = table.Column<int>(name: "grp_eda_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    val = table.Column<int>(type: "integer", nullable: false),
                    param = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_generation_conf", x => new { x.hpid, x.grpedano, x.grpcd, x.id });
                });

            migrationBuilder.CreateTable(
                name: "syuno_nyukin",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    adjustfutan = table.Column<int>(name: "adjust_futan", type: "integer", nullable: false),
                    nyukingaku = table.Column<int>(name: "nyukin_gaku", type: "integer", nullable: false),
                    paymentmethodcd = table.Column<int>(name: "payment_method_cd", type: "integer", nullable: false),
                    nyukindate = table.Column<int>(name: "nyukin_date", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    nyukincmt = table.Column<string>(name: "nyukin_cmt", type: "character varying(100)", maxLength: 100, nullable: true),
                    nyukinjitensu = table.Column<int>(name: "nyukinji_tensu", type: "integer", nullable: false),
                    nyukinjiseikyu = table.Column<int>(name: "nyukinji_seikyu", type: "integer", nullable: false),
                    nyukinjidetail = table.Column<string>(name: "nyukinji_detail", type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syuno_nyukin", x => new { x.hpid, x.raiinno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "syuno_seikyu",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    nyukinkbn = table.Column<int>(name: "nyukin_kbn", type: "integer", nullable: false),
                    seikyutensu = table.Column<int>(name: "seikyu_tensu", type: "integer", nullable: false),
                    adjustfutan = table.Column<int>(name: "adjust_futan", type: "integer", nullable: false),
                    seikyugaku = table.Column<int>(name: "seikyu_gaku", type: "integer", nullable: false),
                    seikyudetail = table.Column<string>(name: "seikyu_detail", type: "text", nullable: true),
                    newseikyutensu = table.Column<int>(name: "new_seikyu_tensu", type: "integer", nullable: false),
                    newadjustfutan = table.Column<int>(name: "new_adjust_futan", type: "integer", nullable: false),
                    newseikyugaku = table.Column<int>(name: "new_seikyu_gaku", type: "integer", nullable: false),
                    newseikyudetail = table.Column<string>(name: "new_seikyu_detail", type: "text", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syuno_seikyu", x => new { x.hpid, x.raiinno, x.ptid, x.sindate });
                });

            migrationBuilder.CreateTable(
                name: "tag_grp_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    taggrpno = table.Column<int>(name: "tag_grp_no", type: "integer", nullable: false),
                    taggrpname = table.Column<string>(name: "tag_grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    grpcolor = table.Column<string>(name: "grp_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_grp_mst", x => new { x.hpid, x.taggrpno });
                });

            migrationBuilder.CreateTable(
                name: "tekiou_byomei_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: false),
                    systemdata = table.Column<int>(name: "system_data", type: "integer", nullable: false),
                    startym = table.Column<int>(name: "start_ym", type: "integer", nullable: false),
                    endym = table.Column<int>(name: "end_ym", type: "integer", nullable: false),
                    isinvalid = table.Column<int>(name: "is_invalid", type: "integer", nullable: false),
                    isinvalidtokusyo = table.Column<int>(name: "is_invalid_tokusyo", type: "integer", nullable: false),
                    editkbn = table.Column<int>(name: "edit_kbn", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tekiou_byomei_mst", x => new { x.hpid, x.itemcd, x.byomeicd, x.systemdata });
                });

            migrationBuilder.CreateTable(
                name: "tekiou_byomei_mst_excluded",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tekiou_byomei_mst_excluded", x => new { x.hpid, x.itemcd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "template_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    templatecd = table.Column<int>(name: "template_cd", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    controlid = table.Column<int>(name: "control_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    oyacontrolid = table.Column<int>(name: "oya_control_id", type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    controltype = table.Column<int>(name: "control_type", type: "integer", nullable: false),
                    menukbn = table.Column<int>(name: "menu_kbn", type: "integer", nullable: false),
                    defaultval = table.Column<string>(name: "default_val", type: "character varying(200)", maxLength: 200, nullable: true),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    newline = table.Column<int>(name: "new_line", type: "integer", nullable: false),
                    kartekbn = table.Column<int>(name: "karte_kbn", type: "integer", nullable: false),
                    controlwidth = table.Column<int>(name: "control_width", type: "integer", nullable: false),
                    titlewidth = table.Column<int>(name: "title_width", type: "integer", nullable: false),
                    unitwidth = table.Column<int>(name: "unit_width", type: "integer", nullable: false),
                    leftmargin = table.Column<int>(name: "left_margin", type: "integer", nullable: false),
                    wordwrap = table.Column<int>(type: "integer", nullable: false),
                    val = table.Column<double>(type: "double precision", nullable: true),
                    formula = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    @decimal = table.Column<int>(name: "decimal", type: "integer", nullable: false),
                    ime = table.Column<int>(type: "integer", nullable: false),
                    colcount = table.Column<int>(name: "col_count", type: "integer", nullable: false),
                    renkeicd = table.Column<string>(name: "renkei_cd", type: "character varying(20)", maxLength: 20, nullable: true),
                    backgroundcolor = table.Column<string>(name: "background_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    fontcolor = table.Column<string>(name: "font_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    fontbold = table.Column<int>(name: "font_bold", type: "integer", nullable: false),
                    fontitalic = table.Column<int>(name: "font_italic", type: "integer", nullable: false),
                    fontunderline = table.Column<int>(name: "font_under_line", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template_detail", x => new { x.hpid, x.templatecd, x.seqno, x.controlid });
                });

            migrationBuilder.CreateTable(
                name: "template_dsp_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    templatecd = table.Column<int>(name: "template_cd", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dspkbn = table.Column<int>(name: "dsp_kbn", type: "integer", nullable: false),
                    isdsp = table.Column<int>(name: "is_dsp", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template_dsp_conf", x => new { x.hpid, x.templatecd, x.seqno, x.dspkbn });
                });

            migrationBuilder.CreateTable(
                name: "template_menu_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    menukbn = table.Column<int>(name: "menu_kbn", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    val = table.Column<double>(type: "double precision", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template_menu_detail", x => new { x.hpid, x.menukbn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "template_menu_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    menukbn = table.Column<int>(name: "menu_kbn", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kbnname = table.Column<string>(name: "kbn_name", type: "character varying(30)", maxLength: 30, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template_menu_mst", x => new { x.hpid, x.menukbn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "template_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    templatecd = table.Column<int>(name: "template_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    insertiondestination = table.Column<int>(name: "insertion_destination", type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template_mst", x => new { x.hpid, x.templatecd, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "ten_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    mastersbt = table.Column<string>(name: "master_sbt", type: "character varying(1)", maxLength: 1, nullable: true),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    kananame1 = table.Column<string>(name: "kana_name1", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame2 = table.Column<string>(name: "kana_name2", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame3 = table.Column<string>(name: "kana_name3", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame4 = table.Column<string>(name: "kana_name4", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame5 = table.Column<string>(name: "kana_name5", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame6 = table.Column<string>(name: "kana_name6", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame7 = table.Column<string>(name: "kana_name7", type: "character varying(120)", maxLength: 120, nullable: true),
                    ryosyuname = table.Column<string>(name: "ryosyu_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    recename = table.Column<string>(name: "rece_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    tenid = table.Column<int>(name: "ten_id", type: "integer", nullable: false),
                    ten = table.Column<double>(type: "double precision", nullable: false),
                    receunitcd = table.Column<string>(name: "rece_unit_cd", type: "character varying(3)", maxLength: 3, nullable: true),
                    receunitname = table.Column<string>(name: "rece_unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    odrunitname = table.Column<string>(name: "odr_unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    cnvunitname = table.Column<string>(name: "cnv_unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    odrtermval = table.Column<double>(name: "odr_term_val", type: "double precision", nullable: false),
                    cnvtermval = table.Column<double>(name: "cnv_term_val", type: "double precision", nullable: false),
                    defaultval = table.Column<double>(name: "default_val", type: "double precision", nullable: false),
                    isadopted = table.Column<int>(name: "is_adopted", type: "integer", nullable: false),
                    koukikbn = table.Column<int>(name: "kouki_kbn", type: "integer", nullable: false),
                    hokatukensa = table.Column<int>(name: "hokatu_kensa", type: "integer", nullable: false),
                    byomeikbn = table.Column<int>(name: "byomei_kbn", type: "integer", nullable: false),
                    igakukanri = table.Column<int>(type: "integer", nullable: false),
                    jitudaycount = table.Column<int>(name: "jituday_count", type: "integer", nullable: false),
                    jituday = table.Column<int>(type: "integer", nullable: false),
                    daycount = table.Column<int>(name: "day_count", type: "integer", nullable: false),
                    drugkanrenkbn = table.Column<int>(name: "drug_kanren_kbn", type: "integer", nullable: false),
                    kizamiid = table.Column<int>(name: "kizami_id", type: "integer", nullable: false),
                    kizamimin = table.Column<int>(name: "kizami_min", type: "integer", nullable: false),
                    kizamimax = table.Column<int>(name: "kizami_max", type: "integer", nullable: false),
                    kizamival = table.Column<int>(name: "kizami_val", type: "integer", nullable: false),
                    kizamiten = table.Column<double>(name: "kizami_ten", type: "double precision", nullable: false),
                    kizamierr = table.Column<int>(name: "kizami_err", type: "integer", nullable: false),
                    maxcount = table.Column<int>(name: "max_count", type: "integer", nullable: false),
                    maxcounterr = table.Column<int>(name: "max_count_err", type: "integer", nullable: false),
                    tyucd = table.Column<string>(name: "tyu_cd", type: "character varying(4)", maxLength: 4, nullable: true),
                    tyuseq = table.Column<string>(name: "tyu_seq", type: "character varying(1)", maxLength: 1, nullable: true),
                    tusokuage = table.Column<int>(name: "tusoku_age", type: "integer", nullable: false),
                    minage = table.Column<string>(name: "min_age", type: "character varying(2)", maxLength: 2, nullable: true),
                    maxage = table.Column<string>(name: "max_age", type: "character varying(2)", maxLength: 2, nullable: true),
                    agecheck = table.Column<int>(name: "age_check", type: "integer", nullable: false),
                    timekasankbn = table.Column<int>(name: "time_kasan_kbn", type: "integer", nullable: false),
                    futekikbn = table.Column<int>(name: "futeki_kbn", type: "integer", nullable: false),
                    futekisisetukbn = table.Column<int>(name: "futeki_sisetu_kbn", type: "integer", nullable: false),
                    syotinyuyojikbn = table.Column<int>(name: "syoti_nyuyoji_kbn", type: "integer", nullable: false),
                    lowweightkbn = table.Column<int>(name: "low_weight_kbn", type: "integer", nullable: false),
                    handankbn = table.Column<int>(name: "handan_kbn", type: "integer", nullable: false),
                    handangrpkbn = table.Column<int>(name: "handan_grp_kbn", type: "integer", nullable: false),
                    teigenkbn = table.Column<int>(name: "teigen_kbn", type: "integer", nullable: false),
                    sekituikbn = table.Column<int>(name: "sekitui_kbn", type: "integer", nullable: false),
                    keibukbn = table.Column<int>(name: "keibu_kbn", type: "integer", nullable: false),
                    autohougoukbn = table.Column<int>(name: "auto_hougou_kbn", type: "integer", nullable: false),
                    gairaikanrikbn = table.Column<int>(name: "gairai_kanri_kbn", type: "integer", nullable: false),
                    tusokutargetkbn = table.Column<int>(name: "tusoku_target_kbn", type: "integer", nullable: false),
                    hokatukbn = table.Column<int>(name: "hokatu_kbn", type: "integer", nullable: false),
                    tyoonpanaisikbn = table.Column<int>(name: "tyoonpa_naisi_kbn", type: "integer", nullable: false),
                    autofungokbn = table.Column<int>(name: "auto_fungo_kbn", type: "integer", nullable: false),
                    tyoonpagyokokbn = table.Column<int>(name: "tyoonpa_gyoko_kbn", type: "integer", nullable: false),
                    gazokasan = table.Column<int>(name: "gazo_kasan", type: "integer", nullable: false),
                    kansatukbn = table.Column<int>(name: "kansatu_kbn", type: "integer", nullable: false),
                    masuikbn = table.Column<int>(name: "masui_kbn", type: "integer", nullable: false),
                    fukubikunaisikasan = table.Column<int>(name: "fukubiku_naisi_kasan", type: "integer", nullable: false),
                    fukubikukotunankasan = table.Column<int>(name: "fukubiku_kotunan_kasan", type: "integer", nullable: false),
                    masuikasan = table.Column<int>(name: "masui_kasan", type: "integer", nullable: false),
                    moniterkasan = table.Column<int>(name: "moniter_kasan", type: "integer", nullable: false),
                    toketukasan = table.Column<int>(name: "toketu_kasan", type: "integer", nullable: false),
                    tenkbnno = table.Column<string>(name: "ten_kbn_no", type: "character varying(30)", maxLength: 30, nullable: true),
                    shortstayope = table.Column<int>(name: "shortstay_ope", type: "integer", nullable: false),
                    buikbn = table.Column<int>(name: "bui_kbn", type: "integer", nullable: false),
                    sisetucd1 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd2 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd3 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd4 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd5 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd6 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd7 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd8 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd9 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd10 = table.Column<int>(type: "integer", nullable: false),
                    agekasanmin1 = table.Column<string>(name: "agekasan_min1", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax1 = table.Column<string>(name: "agekasan_max1", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd1 = table.Column<string>(name: "agekasan_cd1", type: "character varying(10)", maxLength: 10, nullable: true),
                    agekasanmin2 = table.Column<string>(name: "agekasan_min2", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax2 = table.Column<string>(name: "agekasan_max2", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd2 = table.Column<string>(name: "agekasan_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    agekasanmin3 = table.Column<string>(name: "agekasan_min3", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax3 = table.Column<string>(name: "agekasan_max3", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd3 = table.Column<string>(name: "agekasan_cd3", type: "character varying(10)", maxLength: 10, nullable: true),
                    agekasanmin4 = table.Column<string>(name: "agekasan_min4", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax4 = table.Column<string>(name: "agekasan_max4", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd4 = table.Column<string>(name: "agekasan_cd4", type: "character varying(10)", maxLength: 10, nullable: true),
                    kensacmt = table.Column<int>(name: "kensa_cmt", type: "integer", nullable: false),
                    madokukbn = table.Column<int>(name: "madoku_kbn", type: "integer", nullable: false),
                    sinkeikbn = table.Column<int>(name: "sinkei_kbn", type: "integer", nullable: false),
                    seibutukbn = table.Column<int>(name: "seibutu_kbn", type: "integer", nullable: false),
                    zoueikbn = table.Column<int>(name: "zouei_kbn", type: "integer", nullable: false),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    zaikbn = table.Column<int>(name: "zai_kbn", type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    tokuzaiagekbn = table.Column<int>(name: "tokuzai_age_kbn", type: "integer", nullable: false),
                    sansokbn = table.Column<int>(name: "sanso_kbn", type: "integer", nullable: false),
                    tokuzaisbt = table.Column<int>(name: "tokuzai_sbt", type: "integer", nullable: false),
                    maxprice = table.Column<int>(name: "max_price", type: "integer", nullable: false),
                    maxten = table.Column<int>(name: "max_ten", type: "integer", nullable: false),
                    syukeisaki = table.Column<string>(name: "syukei_saki", type: "character varying(3)", maxLength: 3, nullable: true),
                    cdkbn = table.Column<string>(name: "cd_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    cdsyo = table.Column<int>(name: "cd_syo", type: "integer", nullable: false),
                    cdbu = table.Column<int>(name: "cd_bu", type: "integer", nullable: false),
                    cdkbnno = table.Column<int>(name: "cd_kbnno", type: "integer", nullable: false),
                    cdedano = table.Column<int>(name: "cd_edano", type: "integer", nullable: false),
                    cdkouno = table.Column<int>(name: "cd_kouno", type: "integer", nullable: false),
                    kokujikbn = table.Column<string>(name: "kokuji_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    kokujisyo = table.Column<int>(name: "kokuji_syo", type: "integer", nullable: false),
                    kokujibu = table.Column<int>(name: "kokuji_bu", type: "integer", nullable: false),
                    kokujikbnno = table.Column<int>(name: "kokuji_kbn_no", type: "integer", nullable: false),
                    kokujiedano = table.Column<int>(name: "kokuji_eda_no", type: "integer", nullable: false),
                    kokujikouno = table.Column<int>(name: "kokuji_kou_no", type: "integer", nullable: false),
                    kokuji1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kokuji2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kohyojun = table.Column<int>(name: "kohyo_jun", type: "integer", nullable: false),
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    yakkacd = table.Column<string>(name: "yakka_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    syusaisbt = table.Column<int>(name: "syusai_sbt", type: "integer", nullable: false),
                    syohinkanren = table.Column<string>(name: "syohin_kanren", type: "character varying(9)", maxLength: 9, nullable: true),
                    upddate = table.Column<int>(name: "upd_date", type: "integer", nullable: false),
                    deldate = table.Column<int>(name: "del_date", type: "integer", nullable: false),
                    keikadate = table.Column<int>(name: "keika_date", type: "integer", nullable: false),
                    kokujibetuno = table.Column<int>(name: "kokuji_betuno", type: "integer", nullable: false),
                    kokujikbnno0 = table.Column<int>(name: "kokuji_kbnno", type: "integer", nullable: false),
                    rousaikbn = table.Column<int>(name: "rousai_kbn", type: "integer", nullable: false),
                    sisikbn = table.Column<int>(name: "sisi_kbn", type: "integer", nullable: false),
                    shotcnt = table.Column<int>(name: "shot_cnt", type: "integer", nullable: false),
                    isnosearch = table.Column<int>(name: "is_nosearch", type: "integer", nullable: false),
                    isnodsppaperrece = table.Column<int>(name: "is_nodsp_paper_rece", type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodspryosyu = table.Column<int>(name: "is_nodsp_ryosyu", type: "integer", nullable: false),
                    isnodspkarte = table.Column<int>(name: "is_nodsp_karte", type: "integer", nullable: false),
                    jihisbt = table.Column<int>(name: "jihi_sbt", type: "integer", nullable: false),
                    kazeikbn = table.Column<int>(name: "kazei_kbn", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    ipnnamecd = table.Column<string>(name: "ipn_name_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    fukuyorise = table.Column<int>(name: "fukuyo_rise", type: "integer", nullable: false),
                    fukuyomorning = table.Column<int>(name: "fukuyo_morning", type: "integer", nullable: false),
                    fukuyodaytime = table.Column<int>(name: "fukuyo_daytime", type: "integer", nullable: false),
                    fukuyonight = table.Column<int>(name: "fukuyo_night", type: "integer", nullable: false),
                    fukuyosleep = table.Column<int>(name: "fukuyo_sleep", type: "integer", nullable: false),
                    isnodspyakutai = table.Column<int>(name: "is_nodsp_yakutai", type: "integer", nullable: false),
                    zaikeipoint = table.Column<double>(name: "zaikei_point", type: "double precision", nullable: false),
                    suryoroundupkbn = table.Column<int>(name: "suryo_roundup_kbn", type: "integer", nullable: false),
                    kouseisinkbn = table.Column<int>(name: "kouseisin_kbn", type: "integer", nullable: false),
                    chusyadrugsbt = table.Column<int>(name: "chusya_drug_sbt", type: "integer", nullable: false),
                    kensafukususantei = table.Column<int>(name: "kensa_fukusu_santei", type: "integer", nullable: false),
                    santeiitemcd = table.Column<string>(name: "santei_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    santeigaikbn = table.Column<int>(name: "santeigai_kbn", type: "integer", nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(20)", maxLength: 20, nullable: true),
                    kensaitemseqno = table.Column<int>(name: "kensa_item_seq_no", type: "integer", nullable: false),
                    renkeicd1 = table.Column<string>(name: "renkei_cd1", type: "character varying(20)", maxLength: 20, nullable: true),
                    renkeicd2 = table.Column<string>(name: "renkei_cd2", type: "character varying(20)", maxLength: 20, nullable: true),
                    saiketukbn = table.Column<int>(name: "saiketu_kbn", type: "integer", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    cmtcol1 = table.Column<int>(name: "cmt_col1", type: "integer", nullable: false),
                    cmtcolketa1 = table.Column<int>(name: "cmt_col_keta1", type: "integer", nullable: false),
                    cmtcol2 = table.Column<int>(name: "cmt_col2", type: "integer", nullable: false),
                    cmtcolketa2 = table.Column<int>(name: "cmt_col_keta2", type: "integer", nullable: false),
                    cmtcol3 = table.Column<int>(name: "cmt_col3", type: "integer", nullable: false),
                    cmtcolketa3 = table.Column<int>(name: "cmt_col_keta3", type: "integer", nullable: false),
                    cmtcol4 = table.Column<int>(name: "cmt_col4", type: "integer", nullable: false),
                    cmtcolketa4 = table.Column<int>(name: "cmt_col_keta4", type: "integer", nullable: false),
                    selectcmtid = table.Column<int>(name: "select_cmt_id", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    cmtsbt = table.Column<int>(name: "cmt_sbt", type: "integer", nullable: false),
                    kensalabel = table.Column<int>(name: "kensa_label", type: "integer", nullable: false),
                    gairaikansen = table.Column<int>(name: "gairai_kansen", type: "integer", nullable: false),
                    jibiagekasan = table.Column<int>(name: "jibi_age_kasan", type: "integer", nullable: false),
                    jibisyonikokin = table.Column<int>(name: "jibi_syonikokin", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    yohocd = table.Column<string>(name: "yoho_cd", type: "character varying(16)", maxLength: 16, nullable: true),
                    yohohosokukbn = table.Column<int>(name: "yoho_hosoku_kbn", type: "integer", nullable: false),
                    yohohosokurec = table.Column<int>(name: "yoho_hosoku_rec", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ten_mst", x => new { x.hpid, x.itemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "ten_mst_mother",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    mastersbt = table.Column<string>(name: "master_sbt", type: "character varying(1)", maxLength: 1, nullable: true),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    kananame1 = table.Column<string>(name: "kana_name1", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame2 = table.Column<string>(name: "kana_name2", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame3 = table.Column<string>(name: "kana_name3", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame4 = table.Column<string>(name: "kana_name4", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame5 = table.Column<string>(name: "kana_name5", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame6 = table.Column<string>(name: "kana_name6", type: "character varying(120)", maxLength: 120, nullable: true),
                    kananame7 = table.Column<string>(name: "kana_name7", type: "character varying(120)", maxLength: 120, nullable: true),
                    ryosyuname = table.Column<string>(name: "ryosyu_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    recename = table.Column<string>(name: "rece_name", type: "character varying(240)", maxLength: 240, nullable: true),
                    tenid = table.Column<int>(name: "ten_id", type: "integer", nullable: false),
                    ten = table.Column<double>(type: "double precision", nullable: false),
                    receunitcd = table.Column<string>(name: "rece_unit_cd", type: "character varying(3)", maxLength: 3, nullable: true),
                    receunitname = table.Column<string>(name: "rece_unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    odrunitname = table.Column<string>(name: "odr_unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    cnvunitname = table.Column<string>(name: "cnv_unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    odrtermval = table.Column<double>(name: "odr_term_val", type: "double precision", nullable: false),
                    cnvtermval = table.Column<double>(name: "cnv_term_val", type: "double precision", nullable: false),
                    defaultval = table.Column<double>(name: "default_val", type: "double precision", nullable: false),
                    isadopted = table.Column<int>(name: "is_adopted", type: "integer", nullable: false),
                    koukikbn = table.Column<int>(name: "kouki_kbn", type: "integer", nullable: false),
                    hokatukensa = table.Column<int>(name: "hokatu_kensa", type: "integer", nullable: false),
                    byomeikbn = table.Column<int>(name: "byomei_kbn", type: "integer", nullable: false),
                    igakukanri = table.Column<int>(type: "integer", nullable: false),
                    jitudaycount = table.Column<int>(name: "jituday_count", type: "integer", nullable: false),
                    jituday = table.Column<int>(type: "integer", nullable: false),
                    daycount = table.Column<int>(name: "day_count", type: "integer", nullable: false),
                    drugkanrenkbn = table.Column<int>(name: "drug_kanren_kbn", type: "integer", nullable: false),
                    kizamiid = table.Column<int>(name: "kizami_id", type: "integer", nullable: false),
                    kizamimin = table.Column<int>(name: "kizami_min", type: "integer", nullable: false),
                    kizamimax = table.Column<int>(name: "kizami_max", type: "integer", nullable: false),
                    kizamival = table.Column<int>(name: "kizami_val", type: "integer", nullable: false),
                    kizamiten = table.Column<double>(name: "kizami_ten", type: "double precision", nullable: false),
                    kizamierr = table.Column<int>(name: "kizami_err", type: "integer", nullable: false),
                    maxcount = table.Column<int>(name: "max_count", type: "integer", nullable: false),
                    maxcounterr = table.Column<int>(name: "max_count_err", type: "integer", nullable: false),
                    tyucd = table.Column<string>(name: "tyu_cd", type: "character varying(4)", maxLength: 4, nullable: true),
                    tyuseq = table.Column<string>(name: "tyu_seq", type: "character varying(1)", maxLength: 1, nullable: true),
                    tusokuage = table.Column<int>(name: "tusoku_age", type: "integer", nullable: false),
                    minage = table.Column<string>(name: "min_age", type: "character varying(2)", maxLength: 2, nullable: true),
                    maxage = table.Column<string>(name: "max_age", type: "character varying(2)", maxLength: 2, nullable: true),
                    agecheck = table.Column<int>(name: "age_check", type: "integer", nullable: false),
                    timekasankbn = table.Column<int>(name: "time_kasan_kbn", type: "integer", nullable: false),
                    futekikbn = table.Column<int>(name: "futeki_kbn", type: "integer", nullable: false),
                    futekisisetukbn = table.Column<int>(name: "futeki_sisetu_kbn", type: "integer", nullable: false),
                    syotinyuyojikbn = table.Column<int>(name: "syoti_nyuyoji_kbn", type: "integer", nullable: false),
                    lowweightkbn = table.Column<int>(name: "low_weight_kbn", type: "integer", nullable: false),
                    handankbn = table.Column<int>(name: "handan_kbn", type: "integer", nullable: false),
                    handangrpkbn = table.Column<int>(name: "handan_grp_kbn", type: "integer", nullable: false),
                    teigenkbn = table.Column<int>(name: "teigen_kbn", type: "integer", nullable: false),
                    sekituikbn = table.Column<int>(name: "sekitui_kbn", type: "integer", nullable: false),
                    keibukbn = table.Column<int>(name: "keibu_kbn", type: "integer", nullable: false),
                    autohougoukbn = table.Column<int>(name: "auto_hougou_kbn", type: "integer", nullable: false),
                    gairaikanrikbn = table.Column<int>(name: "gairai_kanri_kbn", type: "integer", nullable: false),
                    tusokutargetkbn = table.Column<int>(name: "tusoku_target_kbn", type: "integer", nullable: false),
                    hokatukbn = table.Column<int>(name: "hokatu_kbn", type: "integer", nullable: false),
                    tyoonpanaisikbn = table.Column<int>(name: "tyoonpa_naisi_kbn", type: "integer", nullable: false),
                    autofungokbn = table.Column<int>(name: "auto_fungo_kbn", type: "integer", nullable: false),
                    tyoonpagyokokbn = table.Column<int>(name: "tyoonpa_gyoko_kbn", type: "integer", nullable: false),
                    gazokasan = table.Column<int>(name: "gazo_kasan", type: "integer", nullable: false),
                    kansatukbn = table.Column<int>(name: "kansatu_kbn", type: "integer", nullable: false),
                    masuikbn = table.Column<int>(name: "masui_kbn", type: "integer", nullable: false),
                    fukubikunaisikasan = table.Column<int>(name: "fukubiku_naisi_kasan", type: "integer", nullable: false),
                    fukubikukotunankasan = table.Column<int>(name: "fukubiku_kotunan_kasan", type: "integer", nullable: false),
                    masuikasan = table.Column<int>(name: "masui_kasan", type: "integer", nullable: false),
                    moniterkasan = table.Column<int>(name: "moniter_kasan", type: "integer", nullable: false),
                    toketukasan = table.Column<int>(name: "toketu_kasan", type: "integer", nullable: false),
                    tenkbnno = table.Column<string>(name: "ten_kbn_no", type: "character varying(30)", maxLength: 30, nullable: true),
                    shortstayope = table.Column<int>(name: "shortstay_ope", type: "integer", nullable: false),
                    buikbn = table.Column<int>(name: "bui_kbn", type: "integer", nullable: false),
                    sisetucd1 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd2 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd3 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd4 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd5 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd6 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd7 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd8 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd9 = table.Column<int>(type: "integer", nullable: false),
                    sisetucd10 = table.Column<int>(type: "integer", nullable: false),
                    agekasanmin1 = table.Column<string>(name: "agekasan_min1", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax1 = table.Column<string>(name: "agekasan_max1", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd1 = table.Column<string>(name: "agekasan_cd1", type: "character varying(10)", maxLength: 10, nullable: true),
                    agekasanmin2 = table.Column<string>(name: "agekasan_min2", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax2 = table.Column<string>(name: "agekasan_max2", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd2 = table.Column<string>(name: "agekasan_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    agekasanmin3 = table.Column<string>(name: "agekasan_min3", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax3 = table.Column<string>(name: "agekasan_max3", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd3 = table.Column<string>(name: "agekasan_cd3", type: "character varying(10)", maxLength: 10, nullable: true),
                    agekasanmin4 = table.Column<string>(name: "agekasan_min4", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasanmax4 = table.Column<string>(name: "agekasan_max4", type: "character varying(2)", maxLength: 2, nullable: true),
                    agekasancd4 = table.Column<string>(name: "agekasan_cd4", type: "character varying(10)", maxLength: 10, nullable: true),
                    kensacmt = table.Column<int>(name: "kensa_cmt", type: "integer", nullable: false),
                    madokukbn = table.Column<int>(name: "madoku_kbn", type: "integer", nullable: false),
                    sinkeikbn = table.Column<int>(name: "sinkei_kbn", type: "integer", nullable: false),
                    seibutukbn = table.Column<int>(name: "seibutu_kbn", type: "integer", nullable: false),
                    zoueikbn = table.Column<int>(name: "zouei_kbn", type: "integer", nullable: false),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    zaikbn = table.Column<int>(name: "zai_kbn", type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    tokuzaiagekbn = table.Column<int>(name: "tokuzai_age_kbn", type: "integer", nullable: false),
                    sansokbn = table.Column<int>(name: "sanso_kbn", type: "integer", nullable: false),
                    tokuzaisbt = table.Column<int>(name: "tokuzai_sbt", type: "integer", nullable: false),
                    maxprice = table.Column<int>(name: "max_price", type: "integer", nullable: false),
                    maxten = table.Column<int>(name: "max_ten", type: "integer", nullable: false),
                    syukeisaki = table.Column<string>(name: "syukei_saki", type: "character varying(3)", maxLength: 3, nullable: true),
                    cdkbn = table.Column<string>(name: "cd_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    cdsyo = table.Column<int>(name: "cd_syo", type: "integer", nullable: false),
                    cdbu = table.Column<int>(name: "cd_bu", type: "integer", nullable: false),
                    cdkbnno = table.Column<int>(name: "cd_kbnno", type: "integer", nullable: false),
                    cdedano = table.Column<int>(name: "cd_edano", type: "integer", nullable: false),
                    cdkouno = table.Column<int>(name: "cd_kouno", type: "integer", nullable: false),
                    kokujikbn = table.Column<string>(name: "kokuji_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    kokujisyo = table.Column<int>(name: "kokuji_syo", type: "integer", nullable: false),
                    kokujibu = table.Column<int>(name: "kokuji_bu", type: "integer", nullable: false),
                    kokujikbnno = table.Column<int>(name: "kokuji_kbn_no", type: "integer", nullable: false),
                    kokujiedano = table.Column<int>(name: "kokuji_eda_no", type: "integer", nullable: false),
                    kokujikouno = table.Column<int>(name: "kokuji_kou_no", type: "integer", nullable: false),
                    kokuji1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kokuji2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kohyojun = table.Column<int>(name: "kohyo_jun", type: "integer", nullable: false),
                    yjcd = table.Column<string>(name: "yj_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    yakkacd = table.Column<string>(name: "yakka_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    syusaisbt = table.Column<int>(name: "syusai_sbt", type: "integer", nullable: false),
                    syohinkanren = table.Column<string>(name: "syohin_kanren", type: "character varying(9)", maxLength: 9, nullable: true),
                    upddate = table.Column<int>(name: "upd_date", type: "integer", nullable: false),
                    deldate = table.Column<int>(name: "del_date", type: "integer", nullable: false),
                    keikadate = table.Column<int>(name: "keika_date", type: "integer", nullable: false),
                    rousaikbn = table.Column<int>(name: "rousai_kbn", type: "integer", nullable: false),
                    sisikbn = table.Column<int>(name: "sisi_kbn", type: "integer", nullable: false),
                    shotcnt = table.Column<int>(name: "shot_cnt", type: "integer", nullable: false),
                    isnosearch = table.Column<int>(name: "is_nosearch", type: "integer", nullable: false),
                    isnodsppaperrece = table.Column<int>(name: "is_nodsp_paper_rece", type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodspryosyu = table.Column<int>(name: "is_nodsp_ryosyu", type: "integer", nullable: false),
                    isnodspkarte = table.Column<int>(name: "is_nodsp_karte", type: "integer", nullable: false),
                    jihisbt = table.Column<int>(name: "jihi_sbt", type: "integer", nullable: false),
                    kazeikbn = table.Column<int>(name: "kazei_kbn", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    ipnnamecd = table.Column<string>(name: "ipn_name_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    fukuyorise = table.Column<int>(name: "fukuyo_rise", type: "integer", nullable: false),
                    fukuyomorning = table.Column<int>(name: "fukuyo_morning", type: "integer", nullable: false),
                    fukuyodaytime = table.Column<int>(name: "fukuyo_daytime", type: "integer", nullable: false),
                    fukuyonight = table.Column<int>(name: "fukuyo_night", type: "integer", nullable: false),
                    fukuyosleep = table.Column<int>(name: "fukuyo_sleep", type: "integer", nullable: false),
                    suryoroundupkbn = table.Column<int>(name: "suryo_roundup_kbn", type: "integer", nullable: false),
                    kouseisinkbn = table.Column<int>(name: "kouseisin_kbn", type: "integer", nullable: false),
                    chusyadrugsbt = table.Column<int>(name: "chusya_drug_sbt", type: "integer", nullable: false),
                    kensafukususantei = table.Column<int>(name: "kensa_fukusu_santei", type: "integer", nullable: false),
                    santeiitemcd = table.Column<string>(name: "santei_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    santeigaikbn = table.Column<int>(name: "santeigai_kbn", type: "integer", nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(20)", maxLength: 20, nullable: true),
                    kensaitemseqno = table.Column<int>(name: "kensa_item_seq_no", type: "integer", nullable: false),
                    renkeicd1 = table.Column<string>(name: "renkei_cd1", type: "character varying(20)", maxLength: 20, nullable: true),
                    renkeicd2 = table.Column<string>(name: "renkei_cd2", type: "character varying(20)", maxLength: 20, nullable: true),
                    saiketukbn = table.Column<int>(name: "saiketu_kbn", type: "integer", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    cmtcol1 = table.Column<int>(name: "cmt_col1", type: "integer", nullable: false),
                    cmtcolketa1 = table.Column<int>(name: "cmt_col_keta1", type: "integer", nullable: false),
                    cmtcol2 = table.Column<int>(name: "cmt_col2", type: "integer", nullable: false),
                    cmtcolketa2 = table.Column<int>(name: "cmt_col_keta2", type: "integer", nullable: false),
                    cmtcol3 = table.Column<int>(name: "cmt_col3", type: "integer", nullable: false),
                    cmtcolketa3 = table.Column<int>(name: "cmt_col_keta3", type: "integer", nullable: false),
                    cmtcol4 = table.Column<int>(name: "cmt_col4", type: "integer", nullable: false),
                    cmtcolketa4 = table.Column<int>(name: "cmt_col_keta4", type: "integer", nullable: false),
                    selectcmtid = table.Column<int>(name: "select_cmt_id", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ten_mst_mother", x => new { x.hpid, x.itemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "time_zone_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    youbikbn = table.Column<int>(name: "youbi_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    endtime = table.Column<int>(name: "end_time", type: "integer", nullable: false),
                    timekbn = table.Column<int>(name: "time_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_time_zone_conf", x => new { x.hpid, x.youbikbn, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "time_zone_day_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    endtime = table.Column<int>(name: "end_time", type: "integer", nullable: false),
                    timekbn = table.Column<int>(name: "time_kbn", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_time_zone_day_inf", x => new { x.hpid, x.id, x.sindate });
                });

            migrationBuilder.CreateTable(
                name: "todo_grp_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    todogrpno = table.Column<int>(name: "todo_grp_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    todogrpname = table.Column<string>(name: "todo_grp_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    grpcolor = table.Column<string>(name: "grp_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_grp_mst", x => new { x.hpid, x.todogrpno });
                });

            migrationBuilder.CreateTable(
                name: "todo_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    todono = table.Column<int>(name: "todo_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    todoedano = table.Column<int>(name: "todo_eda_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    todokbnno = table.Column<int>(name: "todo_kbn_no", type: "integer", nullable: false),
                    todogrpno = table.Column<int>(name: "todo_grp_no", type: "integer", nullable: false),
                    tanto = table.Column<int>(type: "integer", nullable: false),
                    term = table.Column<int>(type: "integer", nullable: false),
                    cmt1 = table.Column<string>(type: "text", nullable: true),
                    cmt2 = table.Column<string>(type: "text", nullable: true),
                    isdone = table.Column<int>(name: "is_done", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_inf", x => new { x.hpid, x.todono, x.todoedano, x.ptid });
                });

            migrationBuilder.CreateTable(
                name: "todo_kbn_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    todokbnno = table.Column<int>(name: "todo_kbn_no", type: "integer", nullable: false),
                    todokbnname = table.Column<string>(name: "todo_kbn_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    actcd = table.Column<int>(name: "act_cd", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_kbn_mst", x => new { x.hpid, x.todokbnno });
                });

            migrationBuilder.CreateTable(
                name: "tokki_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    tokkicd = table.Column<string>(name: "tokki_cd", type: "character varying(2)", maxLength: 2, nullable: false),
                    tokkiname = table.Column<string>(name: "tokki_name", type: "character varying(20)", maxLength: 20, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokki_mst", x => new { x.hpid, x.tokkicd });
                });

            migrationBuilder.CreateTable(
                name: "uketuke_sbt_day_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uketuke_sbt_day_inf", x => new { x.hpid, x.sindate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "uketuke_sbt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    kbnid = table.Column<int>(name: "kbn_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kbnname = table.Column<string>(name: "kbn_name", type: "character varying(20)", maxLength: 20, nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uketuke_sbt_mst", x => new { x.hpid, x.kbnid });
                });

            migrationBuilder.CreateTable(
                name: "unit_mst",
                columns: table => new
                {
                    unitcd = table.Column<int>(name: "unit_cd", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit_mst", x => x.unitcd);
                });

            migrationBuilder.CreateTable(
                name: "user_conf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    grpcd = table.Column<int>(name: "grp_cd", type: "integer", nullable: false),
                    grpitemcd = table.Column<int>(name: "grp_item_cd", type: "integer", nullable: false),
                    grpitemedano = table.Column<int>(name: "grp_item_eda_no", type: "integer", nullable: false),
                    val = table.Column<int>(type: "integer", nullable: false),
                    param = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_conf", x => new { x.hpid, x.userid, x.grpcd, x.grpitemcd, x.grpitemedano });
                });

            migrationBuilder.CreateTable(
                name: "user_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    jobcd = table.Column<int>(name: "job_cd", type: "integer", nullable: false),
                    managerkbn = table.Column<int>(name: "manager_kbn", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    sname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    drname = table.Column<string>(name: "dr_name", type: "character varying(40)", maxLength: 40, nullable: true),
                    loginid = table.Column<string>(name: "login_id", type: "character varying(20)", maxLength: 20, nullable: false),
                    mayakulicenseno = table.Column<string>(name: "mayaku_license_no", type: "character varying(20)", maxLength: 20, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    renkeicd1 = table.Column<string>(name: "renkei_cd1", type: "character varying(14)", maxLength: 14, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    logintype = table.Column<int>(name: "login_type", type: "integer", nullable: false),
                    hpkisn = table.Column<string>(name: "hpki_sn", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpkiissuerdn = table.Column<string>(name: "hpki_issuer_dn", type: "character varying(100)", maxLength: 100, nullable: true),
                    hashpassword = table.Column<byte[]>(name: "hash_password", type: "bytea", nullable: true),
                    salt = table.Column<byte[]>(type: "bytea", maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_mst", x => new { x.id, x.hpid });
                });

            migrationBuilder.CreateTable(
                name: "user_permission",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    functioncd = table.Column<string>(name: "function_cd", type: "character varying(8)", maxLength: 8, nullable: false),
                    permission = table.Column<int>(type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permission", x => new { x.hpid, x.userid, x.functioncd });
                });

            migrationBuilder.CreateTable(
                name: "user_token",
                columns: table => new
                {
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    refreshtoken = table.Column<string>(name: "refresh_token", type: "text", nullable: false),
                    tokenexpirytime = table.Column<DateTime>(name: "token_expiry_time", type: "timestamp with time zone", nullable: false),
                    refreshtokenisused = table.Column<bool>(name: "refresh_token_is_used", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_token", x => new { x.userid, x.refreshtoken });
                });

            migrationBuilder.CreateTable(
                name: "wrk_sin_koui",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    syukeisaki = table.Column<string>(name: "syukei_saki", type: "character varying(4)", maxLength: 4, nullable: true),
                    hokatukensa = table.Column<int>(name: "hokatu_kensa", type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodsppaperrece = table.Column<int>(name: "is_nodsp_paper_rece", type: "integer", nullable: false),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    cdkbn = table.Column<string>(name: "cd_kbn", type: "character varying(2)", maxLength: 2, nullable: true),
                    recid = table.Column<string>(name: "rec_id", type: "character varying(2)", maxLength: 2, nullable: true),
                    jihisbt = table.Column<int>(name: "jihi_sbt", type: "integer", nullable: false),
                    kazeikbn = table.Column<int>(name: "kazei_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wrk_sin_koui", x => new { x.hpid, x.raiinno, x.hokenkbn, x.rpno, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "wrk_sin_koui_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    recid = table.Column<string>(name: "rec_id", type: "character varying(2)", maxLength: 2, nullable: true),
                    itemsbt = table.Column<int>(name: "item_sbt", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    odritemcd = table.Column<string>(name: "odr_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(1000)", maxLength: 1000, nullable: true),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    suryo2 = table.Column<double>(type: "double precision", nullable: false),
                    fmtkbn = table.Column<int>(name: "fmt_kbn", type: "integer", nullable: false),
                    unitcd = table.Column<int>(name: "unit_cd", type: "integer", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    tenid = table.Column<int>(name: "ten_id", type: "integer", nullable: false),
                    ten = table.Column<double>(type: "double precision", nullable: false),
                    cdkbn = table.Column<string>(name: "cd_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    cdkbnno = table.Column<int>(name: "cd_kbnno", type: "integer", nullable: false),
                    cdedano = table.Column<int>(name: "cd_edano", type: "integer", nullable: false),
                    cdkouno = table.Column<int>(name: "cd_kouno", type: "integer", nullable: false),
                    kokuji1 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    kokuji2 = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    tyucd = table.Column<string>(name: "tyu_cd", type: "character varying(4)", maxLength: 4, nullable: true),
                    tyuseq = table.Column<string>(name: "tyu_seq", type: "character varying(1)", maxLength: 1, nullable: true),
                    tusokuage = table.Column<int>(name: "tusoku_age", type: "integer", nullable: false),
                    itemseqno = table.Column<int>(name: "item_seq_no", type: "integer", nullable: false),
                    itemedano = table.Column<int>(name: "item_eda_no", type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    isnodsppaperrece = table.Column<int>(name: "is_nodsp_paper_rece", type: "integer", nullable: false),
                    isnodspryosyu = table.Column<int>(name: "is_nodsp_ryosyu", type: "integer", nullable: false),
                    isautoadd = table.Column<int>(name: "is_auto_add", type: "integer", nullable: false),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(160)", maxLength: 160, nullable: true),
                    cmt1 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cmtcd1 = table.Column<string>(name: "cmt_cd1", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtopt1 = table.Column<string>(name: "cmt_opt1", type: "character varying(160)", maxLength: 160, nullable: true),
                    cmt2 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cmtcd2 = table.Column<string>(name: "cmt_cd2", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtopt2 = table.Column<string>(name: "cmt_opt2", type: "character varying(160)", maxLength: 160, nullable: true),
                    cmt3 = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cmtcd3 = table.Column<string>(name: "cmt_cd3", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtopt3 = table.Column<string>(name: "cmt_opt3", type: "character varying(160)", maxLength: 160, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wrk_sin_koui_detail", x => new { x.hpid, x.raiinno, x.hokenkbn, x.rpno, x.seqno, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "wrk_sin_koui_detail_del",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    itemseqno = table.Column<int>(name: "item_seq_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    delitemcd = table.Column<string>(name: "del_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    santeidate = table.Column<int>(name: "santei_date", type: "integer", nullable: false),
                    delsbt = table.Column<int>(name: "del_sbt", type: "integer", nullable: false),
                    iswarning = table.Column<int>(name: "is_warning", type: "integer", nullable: false),
                    termcnt = table.Column<int>(name: "term_cnt", type: "integer", nullable: false),
                    termsbt = table.Column<int>(name: "term_sbt", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wrk_sin_koui_detail_del", x => new { x.hpid, x.raiinno, x.hokenkbn, x.rpno, x.seqno, x.rowno, x.itemseqno });
                });

            migrationBuilder.CreateTable(
                name: "wrk_sin_rp_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    rpno = table.Column<int>(name: "rp_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    sinid = table.Column<int>(name: "sin_id", type: "integer", nullable: false),
                    cdno = table.Column<string>(name: "cd_no", type: "character varying(15)", maxLength: 15, nullable: true),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wrk_sin_rp_inf", x => new { x.hpid, x.raiinno, x.hokenkbn, x.rpno });
                });

            migrationBuilder.CreateTable(
                name: "yakka_syusai_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    yakkacd = table.Column<string>(name: "yakka_cd", type: "character varying(12)", maxLength: 12, nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    seibun = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    hinmoku = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    kbn = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    syusaidate = table.Column<int>(name: "syusai_date", type: "integer", nullable: false),
                    keika = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    biko = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    junsenpatu = table.Column<int>(name: "jun_senpatu", type: "integer", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    yakka = table.Column<double>(type: "double precision", nullable: false),
                    isnotarget = table.Column<int>(name: "is_notarget", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yakka_syusai_mst", x => new { x.hpid, x.yakkacd, x.itemcd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "yoho_hosoku",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    yohohosokukbn = table.Column<int>(name: "yoho_hosoku_kbn", type: "integer", nullable: false),
                    yohohosokurec = table.Column<int>(name: "yoho_hosoku_rec", type: "integer", nullable: false),
                    hosokuitemcd = table.Column<string>(name: "hosoku_item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    hosoku = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoho_hosoku", x => new { x.hpid, x.itemcd, x.startdate, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "yoho_inf_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    yohosuffix = table.Column<string>(name: "yoho_suffix", type: "character varying(240)", maxLength: 240, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoho_inf_mst", x => new { x.hpid, x.itemcd });
                });

            migrationBuilder.CreateTable(
                name: "yoho_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    yohocd = table.Column<string>(name: "yoho_cd", type: "character varying(16)", maxLength: 16, nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    yohokbncd = table.Column<string>(name: "yoho_kbn_cd", type: "character varying(1)", maxLength: 1, nullable: false),
                    yohokbn = table.Column<string>(name: "yoho_kbn", type: "character varying(2)", maxLength: 2, nullable: false),
                    yohodetailkbncd = table.Column<string>(name: "yoho_detail_kbn_cd", type: "character varying(1)", maxLength: 1, nullable: false),
                    yohodetailkbn = table.Column<string>(name: "yoho_detail_kbn", type: "character varying(15)", maxLength: 15, nullable: false),
                    timingkbncd = table.Column<int>(name: "timing_kbn_cd", type: "integer", nullable: false),
                    timingkbn = table.Column<string>(name: "timing_kbn", type: "character varying(60)", maxLength: 60, nullable: false),
                    yohoname = table.Column<string>(name: "yoho_name", type: "character varying(50)", maxLength: 50, nullable: false),
                    referenceno = table.Column<int>(name: "reference_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    yohocdkbn = table.Column<int>(name: "yoho_cd_kbn", type: "integer", nullable: false),
                    tonyojoken = table.Column<int>(name: "tonyo_joken", type: "integer", nullable: false),
                    toyotiming = table.Column<int>(name: "toyo_timing", type: "integer", nullable: false),
                    toyotime = table.Column<int>(name: "toyo_time", type: "integer", nullable: false),
                    toyointerval = table.Column<int>(name: "toyo_interval", type: "integer", nullable: false),
                    bui = table.Column<int>(type: "integer", nullable: false),
                    yohokananame = table.Column<string>(name: "yoho_kana_name", type: "character varying(120)", maxLength: 120, nullable: false),
                    chozaiyohocd = table.Column<int>(name: "chozai_yoho_cd", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: false),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoho_mst", x => new { x.hpid, x.yohocd, x.startdate });
                });

            migrationBuilder.CreateTable(
                name: "yoho_set_mst",
                columns: table => new
                {
                    setid = table.Column<int>(name: "set_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoho_set_mst", x => x.setid);
                });

            migrationBuilder.CreateTable(
                name: "yoyaku_odr_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    yoyakukarteno = table.Column<long>(name: "yoyaku_karte_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    yoyakudate = table.Column<int>(name: "yoyaku_date", type: "integer", nullable: false),
                    odrkouikbn = table.Column<int>(name: "odr_koui_kbn", type: "integer", nullable: false),
                    rpname = table.Column<string>(name: "rp_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    syohosbt = table.Column<int>(name: "syoho_sbt", type: "integer", nullable: false),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    dayscnt = table.Column<int>(name: "days_cnt", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoyaku_odr_inf", x => new { x.hpid, x.ptid, x.yoyakukarteno, x.rpno, x.rpedano });
                });

            migrationBuilder.CreateTable(
                name: "yoyaku_odr_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    yoyakukarteno = table.Column<long>(name: "yoyaku_karte_no", type: "bigint", nullable: false),
                    rpno = table.Column<long>(name: "rp_no", type: "bigint", nullable: false),
                    rpedano = table.Column<long>(name: "rp_eda_no", type: "bigint", nullable: false),
                    rowno = table.Column<long>(name: "row_no", type: "bigint", nullable: false),
                    yoyakudate = table.Column<int>(name: "yoyaku_date", type: "integer", nullable: false),
                    sinkouikbn = table.Column<int>(name: "sin_koui_kbn", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    itemname = table.Column<string>(name: "item_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    suryo = table.Column<double>(type: "double precision", nullable: false),
                    unitname = table.Column<string>(name: "unit_name", type: "character varying(24)", maxLength: 24, nullable: true),
                    unitsbt = table.Column<int>(name: "unit_sbt", type: "integer", nullable: false),
                    termval = table.Column<double>(name: "term_val", type: "double precision", nullable: false),
                    kohatukbn = table.Column<int>(name: "kohatu_kbn", type: "integer", nullable: false),
                    syohokbn = table.Column<int>(name: "syoho_kbn", type: "integer", nullable: false),
                    syoholimitkbn = table.Column<int>(name: "syoho_limit_kbn", type: "integer", nullable: false),
                    drugkbn = table.Column<int>(name: "drug_kbn", type: "integer", nullable: false),
                    yohokbn = table.Column<int>(name: "yoho_kbn", type: "integer", nullable: false),
                    kokuji1 = table.Column<int>(type: "integer", nullable: false),
                    isnodsprece = table.Column<int>(name: "is_nodsp_rece", type: "integer", nullable: false),
                    ipncd = table.Column<string>(name: "ipn_cd", type: "character varying(12)", maxLength: 12, nullable: true),
                    ipnname = table.Column<string>(name: "ipn_name", type: "character varying(120)", maxLength: 120, nullable: true),
                    bunkatu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    cmtname = table.Column<string>(name: "cmt_name", type: "character varying(32)", maxLength: 32, nullable: true),
                    cmtopt = table.Column<string>(name: "cmt_opt", type: "character varying(38)", maxLength: 38, nullable: true),
                    fontcolor = table.Column<int>(name: "font_color", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoyaku_odr_inf_detail", x => new { x.hpid, x.ptid, x.yoyakukarteno, x.rpno, x.rpedano, x.rowno });
                });

            migrationBuilder.CreateTable(
                name: "yoyaku_sbt_mst",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    yoyakusbt = table.Column<int>(name: "yoyaku_sbt", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sbtname = table.Column<string>(name: "sbt_name", type: "character varying(120)", maxLength: 120, nullable: false),
                    defaultcmt = table.Column<string>(name: "default_cmt", type: "character varying(120)", maxLength: 120, nullable: true),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yoyaku_sbt_mst", x => new { x.hpid, x.yoyakusbt });
                });

            migrationBuilder.CreateTable(
                name: "z_doc_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(300)", maxLength: 300, nullable: true),
                    dspfilename = table.Column<string>(name: "dsp_file_name", type: "character varying(300)", maxLength: 300, nullable: true),
                    islocked = table.Column<int>(name: "is_locked", type: "integer", nullable: false),
                    lockdate = table.Column<DateTime>(name: "lock_date", type: "timestamp with time zone", nullable: true),
                    lockid = table.Column<int>(name: "lock_id", type: "integer", nullable: false),
                    lockmachine = table.Column<string>(name: "lock_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_doc_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_filing_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    getdate = table.Column<int>(name: "get_date", type: "integer", nullable: false),
                    categorycd = table.Column<int>(name: "category_cd", type: "integer", nullable: false),
                    fileno = table.Column<int>(name: "file_no", type: "integer", nullable: false),
                    filename = table.Column<string>(name: "file_name", type: "character varying(300)", maxLength: 300, nullable: true),
                    dspfilename = table.Column<string>(name: "dsp_file_name", type: "character varying(1024)", maxLength: 1024, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    fileid = table.Column<int>(name: "file_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_filing_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_kensa_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    iraicd = table.Column<long>(name: "irai_cd", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iraidate = table.Column<int>(name: "irai_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    inoutkbn = table.Column<int>(name: "inout_kbn", type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    tosekikbn = table.Column<int>(name: "toseki_kbn", type: "integer", nullable: false),
                    sikyukbn = table.Column<int>(name: "sikyu_kbn", type: "integer", nullable: false),
                    resultcheck = table.Column<int>(name: "result_check", type: "integer", nullable: false),
                    centercd = table.Column<string>(name: "center_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    nyubi = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    yoketu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    bilirubin = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_kensa_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_kensa_inf_detail",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    iraicd = table.Column<long>(name: "irai_cd", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    iraidate = table.Column<int>(name: "irai_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    kensaitemcd = table.Column<string>(name: "kensa_item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    resultval = table.Column<string>(name: "result_val", type: "character varying(10)", maxLength: 10, nullable: true),
                    resulttype = table.Column<string>(name: "result_type", type: "character varying(1)", maxLength: 1, nullable: true),
                    abnormalkbn = table.Column<string>(name: "abnormal_kbn", type: "character varying(1)", maxLength: 1, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    cmtcd1 = table.Column<string>(name: "cmt_cd1", type: "character varying(3)", maxLength: 3, nullable: true),
                    cmtcd2 = table.Column<string>(name: "cmt_cd2", type: "character varying(3)", maxLength: 3, nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    seqparentno = table.Column<long>(name: "seq_parent_no", type: "bigint", nullable: false),
                    seqgroupno = table.Column<long>(name: "seq_group_no", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_kensa_inf_detail", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_limit_cnt_list_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    kohiid = table.Column<int>(name: "kohi_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    sortkey = table.Column<string>(name: "sort_key", type: "character varying(61)", maxLength: 61, nullable: true),
                    oyaraiinno = table.Column<long>(name: "oya_raiin_no", type: "bigint", nullable: false),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_limit_cnt_list_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_limit_list_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    kohiid = table.Column<int>(name: "kohi_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    sortkey = table.Column<string>(name: "sort_key", type: "character varying(61)", maxLength: 61, nullable: true),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    futangaku = table.Column<int>(name: "futan_gaku", type: "integer", nullable: false),
                    totalgaku = table.Column<int>(name: "total_gaku", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_limit_list_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_monshin_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: true),
                    rtext = table.Column<string>(type: "text", nullable: true),
                    getkbn = table.Column<int>(name: "get_kbn", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_monshin_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_alrgy_drug",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    drugname = table.Column<string>(name: "drug_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_alrgy_drug", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_alrgy_else",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    alrgyname = table.Column<string>(name: "alrgy_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_alrgy_else", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_alrgy_food",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    alrgykbn = table.Column<string>(name: "alrgy_kbn", type: "text", nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_alrgy_food", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_cmt_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_cmt_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_family",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    familyid = table.Column<long>(name: "family_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    zokugaracd = table.Column<string>(name: "zokugara_cd", type: "character varying(10)", maxLength: 10, nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    parentid = table.Column<int>(name: "parent_id", type: "integer", nullable: false),
                    familyptid = table.Column<long>(name: "family_pt_id", type: "bigint", nullable: false),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    birthday = table.Column<int>(type: "integer", nullable: false),
                    isdead = table.Column<int>(name: "is_dead", type: "integer", nullable: false),
                    isseparated = table.Column<int>(name: "is_separated", type: "integer", nullable: false),
                    biko = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_family", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_family_reki",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    familyid = table.Column<long>(name: "family_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_family_reki", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_grp_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    grpcode = table.Column<string>(name: "grp_code", type: "character varying(4)", maxLength: 4, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_grp_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_hoken_check",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokengrp = table.Column<int>(name: "hoken_grp", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkdate = table.Column<DateTime>(name: "check_date", type: "timestamp with time zone", nullable: false),
                    checkid = table.Column<int>(name: "check_id", type: "integer", nullable: false),
                    checkmachine = table.Column<string>(name: "check_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    checkcmt = table.Column<string>(name: "check_cmt", type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_hoken_check", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_hoken_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    edano = table.Column<string>(name: "eda_no", type: "character varying(2)", maxLength: 2, nullable: true),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    hokensyano = table.Column<string>(name: "hokensya_no", type: "character varying(8)", maxLength: 8, nullable: true),
                    kigo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    bango = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    honkekbn = table.Column<int>(name: "honke_kbn", type: "integer", nullable: false),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    hokensyaname = table.Column<string>(name: "hokensya_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    hokensyapost = table.Column<string>(name: "hokensya_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    hokensyaaddress = table.Column<string>(name: "hokensya_address", type: "character varying(100)", maxLength: 100, nullable: true),
                    hokensyatel = table.Column<string>(name: "hokensya_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    keizokukbn = table.Column<int>(name: "keizoku_kbn", type: "integer", nullable: false),
                    sikakudate = table.Column<int>(name: "sikaku_date", type: "integer", nullable: false),
                    kofudate = table.Column<int>(name: "kofu_date", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    gendogaku = table.Column<int>(type: "integer", nullable: false),
                    kogakukbn = table.Column<int>(name: "kogaku_kbn", type: "integer", nullable: false),
                    kogakutype = table.Column<int>(name: "kogaku_type", type: "integer", nullable: false),
                    tokureiym1 = table.Column<int>(name: "tokurei_ym1", type: "integer", nullable: false),
                    tokureiym2 = table.Column<int>(name: "tokurei_ym2", type: "integer", nullable: false),
                    tasukaiym = table.Column<int>(name: "tasukai_ym", type: "integer", nullable: false),
                    syokumukbn = table.Column<int>(name: "syokumu_kbn", type: "integer", nullable: false),
                    genmenkbn = table.Column<int>(name: "genmen_kbn", type: "integer", nullable: false),
                    genmenrate = table.Column<int>(name: "genmen_rate", type: "integer", nullable: false),
                    genmengaku = table.Column<int>(name: "genmen_gaku", type: "integer", nullable: false),
                    tokki1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki3 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki4 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    tokki5 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    rousaikofuno = table.Column<string>(name: "rousai_kofu_no", type: "character varying(14)", maxLength: 14, nullable: true),
                    rousaisaigaikbn = table.Column<int>(name: "rousai_saigai_kbn", type: "integer", nullable: false),
                    rousaijigyosyoname = table.Column<string>(name: "rousai_jigyosyo_name", type: "character varying(80)", maxLength: 80, nullable: true),
                    rousaiprefname = table.Column<string>(name: "rousai_pref_name", type: "character varying(10)", maxLength: 10, nullable: true),
                    rousaicityname = table.Column<string>(name: "rousai_city_name", type: "character varying(20)", maxLength: 20, nullable: true),
                    rousaisyobyodate = table.Column<int>(name: "rousai_syobyo_date", type: "integer", nullable: false),
                    rousaisyobyocd = table.Column<string>(name: "rousai_syobyo_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    rousairoudoucd = table.Column<string>(name: "rousai_roudou_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    rousaikantokucd = table.Column<string>(name: "rousai_kantoku_cd", type: "character varying(2)", maxLength: 2, nullable: true),
                    rousairececount = table.Column<int>(name: "rousai_rece_count", type: "integer", nullable: false),
                    jibaihokenname = table.Column<string>(name: "jibai_hoken_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    jibaihokentanto = table.Column<string>(name: "jibai_hoken_tanto", type: "character varying(40)", maxLength: 40, nullable: true),
                    jibaihokentel = table.Column<string>(name: "jibai_hoken_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    jibaijyusyoudate = table.Column<int>(name: "jibai_jyusyou_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    ryoyostartdate = table.Column<int>(name: "ryoyo_start_date", type: "integer", nullable: false),
                    ryoyoenddate = table.Column<int>(name: "ryoyo_end_date", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_hoken_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_hoken_pattern",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hokenkbn = table.Column<int>(name: "hoken_kbn", type: "integer", nullable: false),
                    hokensbtcd = table.Column<int>(name: "hoken_sbt_cd", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    kohi1id = table.Column<int>(name: "kohi1_id", type: "integer", nullable: false),
                    kohi2id = table.Column<int>(name: "kohi2_id", type: "integer", nullable: false),
                    kohi3id = table.Column<int>(name: "kohi3_id", type: "integer", nullable: false),
                    kohi4id = table.Column<int>(name: "kohi4_id", type: "integer", nullable: false),
                    hokenmemo = table.Column<string>(name: "hoken_memo", type: "character varying(400)", maxLength: 400, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_hoken_pattern", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_hoken_scan",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokengrp = table.Column<int>(name: "hoken_grp", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    filename = table.Column<string>(name: "file_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_hoken_scan", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    ptnum = table.Column<long>(name: "pt_num", type: "bigint", nullable: false),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    birthday = table.Column<int>(type: "integer", nullable: false),
                    isdead = table.Column<int>(name: "is_dead", type: "integer", nullable: false),
                    deathdate = table.Column<int>(name: "death_date", type: "integer", nullable: false),
                    homepost = table.Column<string>(name: "home_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    homeaddress1 = table.Column<string>(name: "home_address1", type: "character varying(100)", maxLength: 100, nullable: true),
                    homeaddress2 = table.Column<string>(name: "home_address2", type: "character varying(100)", maxLength: 100, nullable: true),
                    tel1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    tel2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    mail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    setainusi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    zokugara = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    job = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    renrakuname = table.Column<string>(name: "renraku_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    renrakupost = table.Column<string>(name: "renraku_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    renrakuaddress1 = table.Column<string>(name: "renraku_address1", type: "character varying(100)", maxLength: 100, nullable: true),
                    renrakuaddress2 = table.Column<string>(name: "renraku_address2", type: "character varying(100)", maxLength: 100, nullable: true),
                    renrakutel = table.Column<string>(name: "renraku_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    renrakumemo = table.Column<string>(name: "renraku_memo", type: "character varying(100)", maxLength: 100, nullable: true),
                    officename = table.Column<string>(name: "office_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    officepost = table.Column<string>(name: "office_post", type: "character varying(7)", maxLength: 7, nullable: true),
                    officeaddress1 = table.Column<string>(name: "office_address1", type: "character varying(100)", maxLength: 100, nullable: true),
                    officeaddress2 = table.Column<string>(name: "office_address2", type: "character varying(100)", maxLength: 100, nullable: true),
                    officetel = table.Column<string>(name: "office_tel", type: "character varying(15)", maxLength: 15, nullable: true),
                    officememo = table.Column<string>(name: "office_memo", type: "character varying(100)", maxLength: 100, nullable: true),
                    isryosyodetail = table.Column<int>(name: "is_ryosyo_detail", type: "integer", nullable: false),
                    primarydoctor = table.Column<int>(name: "primary_doctor", type: "integer", nullable: false),
                    istester = table.Column<int>(name: "is_tester", type: "integer", nullable: false),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    mainhokenpid = table.Column<int>(name: "main_hoken_pid", type: "integer", nullable: false),
                    referenceno = table.Column<long>(name: "reference_no", type: "bigint", nullable: false),
                    limitconsflg = table.Column<int>(name: "limit_cons_flg", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_infection",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "text", nullable: true),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_infection", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_jibkar",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    webid = table.Column<string>(name: "web_id", type: "character varying(16)", maxLength: 16, nullable: true),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    odrkaiji = table.Column<int>(name: "odr_kaiji", type: "integer", nullable: false),
                    odrupdatedate = table.Column<DateTime>(name: "odr_update_date", type: "timestamp with time zone", nullable: false),
                    kartekaiji = table.Column<int>(name: "karte_kaiji", type: "integer", nullable: false),
                    karteupdatedate = table.Column<DateTime>(name: "karte_update_date", type: "timestamp with time zone", nullable: false),
                    kensakaiji = table.Column<int>(name: "kensa_kaiji", type: "integer", nullable: false),
                    kensaupdatedate = table.Column<DateTime>(name: "kensa_update_date", type: "timestamp with time zone", nullable: false),
                    byomeikaiji = table.Column<int>(name: "byomei_kaiji", type: "integer", nullable: false),
                    byomeiupdatedate = table.Column<DateTime>(name: "byomei_update_date", type: "timestamp with time zone", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_jibkar", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_kio_reki",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    byomeicd = table.Column<string>(name: "byomei_cd", type: "character varying(7)", maxLength: 7, nullable: true),
                    byotaicd = table.Column<string>(name: "byotai_cd", type: "text", nullable: true),
                    byomei = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_kio_reki", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_kohi",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prefno = table.Column<int>(name: "pref_no", type: "integer", nullable: false),
                    hokenno = table.Column<int>(name: "hoken_no", type: "integer", nullable: false),
                    hokenedano = table.Column<int>(name: "hoken_eda_no", type: "integer", nullable: false),
                    futansyano = table.Column<string>(name: "futansya_no", type: "character varying(8)", maxLength: 8, nullable: true),
                    jyukyusyano = table.Column<string>(name: "jyukyusya_no", type: "character varying(7)", maxLength: 7, nullable: true),
                    tokusyuno = table.Column<string>(name: "tokusyu_no", type: "character varying(20)", maxLength: 20, nullable: true),
                    sikakudate = table.Column<int>(name: "sikaku_date", type: "integer", nullable: false),
                    kofudate = table.Column<int>(name: "kofu_date", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    gendogaku = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    hokensbtkbn = table.Column<int>(name: "hoken_sbt_kbn", type: "integer", nullable: false),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_kohi", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_kyusei",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kananame = table.Column<string>(name: "kana_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_kyusei", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_memo",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    memo = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_memo", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_otc_drug",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    serialnum = table.Column<int>(name: "serial_num", type: "integer", nullable: false),
                    tradename = table.Column<string>(name: "trade_name", type: "character varying(200)", maxLength: 200, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_otc_drug", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_other_drug",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    drugname = table.Column<string>(name: "drug_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_other_drug", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_pregnancy",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    perioddate = table.Column<int>(name: "period_date", type: "integer", nullable: false),
                    periodduedate = table.Column<int>(name: "period_due_date", type: "integer", nullable: false),
                    ovulationdate = table.Column<int>(name: "ovulation_date", type: "integer", nullable: false),
                    ovulationduedate = table.Column<int>(name: "ovulation_due_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_pregnancy", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_rousai_tenki",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    sinkei = table.Column<int>(type: "integer", nullable: false),
                    tenki = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_rousai_tenki", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_santei_conf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    kbnno = table.Column<int>(name: "kbn_no", type: "integer", nullable: false),
                    edano = table.Column<int>(name: "eda_no", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kbnval = table.Column<int>(name: "kbn_val", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_santei_conf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_supple",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    indexcd = table.Column<string>(name: "index_cd", type: "text", nullable: true),
                    indexword = table.Column<string>(name: "index_word", type: "character varying(200)", maxLength: 200, nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_supple", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_pt_tag",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    memo = table.Column<string>(type: "text", nullable: true),
                    memodata = table.Column<byte[]>(name: "memo_data", type: "bytea", nullable: true),
                    startdate = table.Column<int>(name: "start_date", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    isdspuketuke = table.Column<int>(name: "is_dsp_uketuke", type: "integer", nullable: false),
                    isdspkarte = table.Column<int>(name: "is_dsp_karte", type: "integer", nullable: false),
                    isdspkaikei = table.Column<int>(name: "is_dsp_kaikei", type: "integer", nullable: false),
                    isdsprece = table.Column<int>(name: "is_dsp_rece", type: "integer", nullable: false),
                    backgroundcolor = table.Column<string>(name: "background_color", type: "character varying(8)", maxLength: 8, nullable: true),
                    taggrpcd = table.Column<int>(name: "tag_grp_cd", type: "integer", nullable: false),
                    alphablendval = table.Column<int>(name: "alphablend_val", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    fontsize = table.Column<int>(type: "integer", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    left = table.Column<int>(type: "integer", nullable: false),
                    top = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_pt_tag", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_raiin_cmt_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_raiin_cmt_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_raiin_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    oyaraiinno = table.Column<long>(name: "oya_raiin_no", type: "bigint", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    isyoyaku = table.Column<int>(name: "is_yoyaku", type: "integer", nullable: false),
                    yoyakutime = table.Column<string>(name: "yoyaku_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    yoyakuid = table.Column<int>(name: "yoyaku_id", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    uketuketime = table.Column<string>(name: "uketuke_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    uketukeid = table.Column<int>(name: "uketuke_id", type: "integer", nullable: false),
                    uketukeno = table.Column<int>(name: "uketuke_no", type: "integer", nullable: false),
                    sinstarttime = table.Column<string>(name: "sin_start_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    sinendtime = table.Column<string>(name: "sin_end_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    kaikeitime = table.Column<string>(name: "kaikei_time", type: "character varying(6)", maxLength: 6, nullable: true),
                    kaikeiid = table.Column<int>(name: "kaikei_id", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    hokenpid = table.Column<int>(name: "hoken_pid", type: "integer", nullable: false),
                    syosaisinkbn = table.Column<int>(name: "syosaisin_kbn", type: "integer", nullable: false),
                    jikankbn = table.Column<int>(name: "jikan_kbn", type: "integer", nullable: false),
                    confirmationresult = table.Column<string>(name: "confirmation_result", type: "character varying(120)", maxLength: 120, nullable: true),
                    confirmationstate = table.Column<int>(name: "confirmation_state", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    santeikbn = table.Column<int>(name: "santei_kbn", type: "integer", nullable: false),
                    confirmationtype = table.Column<int>(name: "confirmation_type", type: "integer", nullable: false),
                    infoconsflg = table.Column<string>(name: "info_cons_flg", type: "character varying(10)", maxLength: 10, nullable: true),
                    prescriptionissuetype = table.Column<int>(name: "prescription_issue_type", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_raiin_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_raiin_kbn_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    grpid = table.Column<int>(name: "grp_id", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kbncd = table.Column<int>(name: "kbn_cd", type: "integer", nullable: false),
                    isdelete = table.Column<int>(name: "is_delete", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_raiin_kbn_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_raiin_list_cmt",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false),
                    text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_raiin_list_cmt", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_raiin_list_tag",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    tagno = table.Column<int>(name: "tag_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_raiin_list_tag", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_rece_check_cmt",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    ispending = table.Column<int>(name: "is_pending", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ischecked = table.Column<int>(name: "is_checked", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_rece_check_cmt", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_rece_cmt",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    cmtkbn = table.Column<int>(name: "cmt_kbn", type: "integer", nullable: false),
                    cmtsbt = table.Column<int>(name: "cmt_sbt", type: "integer", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    cmt = table.Column<string>(type: "text", nullable: true),
                    cmtdata = table.Column<string>(name: "cmt_data", type: "character varying(38)", maxLength: 38, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_rece_cmt", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_rece_inf_edit",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recesbt = table.Column<string>(name: "rece_sbt", type: "character varying(4)", maxLength: 4, nullable: true),
                    houbetu = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi1houbetu = table.Column<string>(name: "kohi1_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi2houbetu = table.Column<string>(name: "kohi2_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi3houbetu = table.Column<string>(name: "kohi3_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    kohi4houbetu = table.Column<string>(name: "kohi4_houbetu", type: "character varying(3)", maxLength: 3, nullable: true),
                    hokenrecetensu = table.Column<int>(name: "hoken_rece_tensu", type: "integer", nullable: true),
                    hokenrecefutan = table.Column<int>(name: "hoken_rece_futan", type: "integer", nullable: true),
                    kohi1recetensu = table.Column<int>(name: "kohi1_rece_tensu", type: "integer", nullable: true),
                    kohi1recefutan = table.Column<int>(name: "kohi1_rece_futan", type: "integer", nullable: true),
                    kohi1recekyufu = table.Column<int>(name: "kohi1_rece_kyufu", type: "integer", nullable: true),
                    kohi2recetensu = table.Column<int>(name: "kohi2_rece_tensu", type: "integer", nullable: true),
                    kohi2recefutan = table.Column<int>(name: "kohi2_rece_futan", type: "integer", nullable: true),
                    kohi2recekyufu = table.Column<int>(name: "kohi2_rece_kyufu", type: "integer", nullable: true),
                    kohi3recetensu = table.Column<int>(name: "kohi3_rece_tensu", type: "integer", nullable: true),
                    kohi3recefutan = table.Column<int>(name: "kohi3_rece_futan", type: "integer", nullable: true),
                    kohi3recekyufu = table.Column<int>(name: "kohi3_rece_kyufu", type: "integer", nullable: true),
                    kohi4recetensu = table.Column<int>(name: "kohi4_rece_tensu", type: "integer", nullable: true),
                    kohi4recefutan = table.Column<int>(name: "kohi4_rece_futan", type: "integer", nullable: true),
                    kohi4recekyufu = table.Column<int>(name: "kohi4_rece_kyufu", type: "integer", nullable: true),
                    hokennissu = table.Column<int>(name: "hoken_nissu", type: "integer", nullable: true),
                    kohi1nissu = table.Column<int>(name: "kohi1_nissu", type: "integer", nullable: true),
                    kohi2nissu = table.Column<int>(name: "kohi2_nissu", type: "integer", nullable: true),
                    kohi3nissu = table.Column<int>(name: "kohi3_nissu", type: "integer", nullable: true),
                    kohi4nissu = table.Column<int>(name: "kohi4_nissu", type: "integer", nullable: true),
                    tokki = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki3 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki4 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tokki5 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_rece_inf_edit", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_rece_seikyu",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seikyuym = table.Column<int>(name: "seikyu_ym", type: "integer", nullable: false),
                    seikyukbn = table.Column<int>(name: "seikyu_kbn", type: "integer", nullable: false),
                    prehokenid = table.Column<int>(name: "pre_hoken_id", type: "integer", nullable: false),
                    cmt = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_rece_seikyu", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_rsv_day_comment",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_rsv_day_comment", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_rsv_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    rsvframeid = table.Column<int>(name: "rsv_frame_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    starttime = table.Column<int>(name: "start_time", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    rsvsbt = table.Column<int>(name: "rsv_sbt", type: "integer", nullable: false),
                    tantoid = table.Column<int>(name: "tanto_id", type: "integer", nullable: false),
                    kaid = table.Column<int>(name: "ka_id", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_rsv_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_santei_inf_detail",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    itemcd = table.Column<string>(name: "item_cd", type: "character varying(10)", maxLength: 10, nullable: true),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    enddate = table.Column<int>(name: "end_date", type: "integer", nullable: false),
                    kisansbt = table.Column<int>(name: "kisan_sbt", type: "integer", nullable: false),
                    kisandate = table.Column<int>(name: "kisan_date", type: "integer", nullable: false),
                    byomei = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    hosokucomment = table.Column<string>(name: "hosoku_comment", type: "character varying(80)", maxLength: 80, nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_santei_inf_detail", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_seikatureki_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_seikatureki_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_summary_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: true),
                    rtext = table.Column<byte[]>(type: "bytea", nullable: true),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_summary_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_syobyo_keika",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    sinday = table.Column<int>(name: "sin_day", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keika = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_syobyo_keika", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_syouki_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    hokenid = table.Column<int>(name: "hoken_id", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    syoukikbn = table.Column<int>(name: "syouki_kbn", type: "integer", nullable: false),
                    syouki = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_syouki_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_syuno_nyukin",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    sortno = table.Column<int>(name: "sort_no", type: "integer", nullable: false),
                    adjustfutan = table.Column<int>(name: "adjust_futan", type: "integer", nullable: false),
                    nyukingaku = table.Column<int>(name: "nyukin_gaku", type: "integer", nullable: false),
                    paymentmethodcd = table.Column<int>(name: "payment_method_cd", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    nyukincmt = table.Column<string>(name: "nyukin_cmt", type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    seqno = table.Column<long>(name: "seq_no", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nyukindate = table.Column<int>(name: "nyukin_date", type: "integer", nullable: false),
                    nyukinjitensu = table.Column<int>(name: "nyukinji_tensu", type: "integer", nullable: false),
                    nyukinjiseikyu = table.Column<int>(name: "nyukinji_seikyu", type: "integer", nullable: false),
                    nyukinjidetail = table.Column<string>(name: "nyukinji_detail", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_syuno_nyukin", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_todo_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    todono = table.Column<int>(name: "todo_no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    todoedano = table.Column<int>(name: "todo_eda_no", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    raiinno = table.Column<long>(name: "raiin_no", type: "bigint", nullable: false),
                    todokbnno = table.Column<int>(name: "todo_kbn_no", type: "integer", nullable: false),
                    todogrpno = table.Column<int>(name: "todo_grp_no", type: "integer", nullable: false),
                    tanto = table.Column<int>(type: "integer", nullable: false),
                    term = table.Column<int>(type: "integer", nullable: false),
                    cmt1 = table.Column<string>(type: "text", nullable: true),
                    cmt2 = table.Column<string>(type: "text", nullable: true),
                    isdone = table.Column<int>(name: "is_done", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true),
                    updatedate = table.Column<DateTime>(name: "update_date", type: "timestamp with time zone", nullable: false),
                    updateid = table.Column<int>(name: "update_id", type: "integer", nullable: false),
                    updatemachine = table.Column<string>(name: "update_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_todo_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_uketuke_sbt_day_inf",
                columns: table => new
                {
                    opid = table.Column<long>(name: "op_id", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    optype = table.Column<string>(name: "op_type", type: "character varying(10)", maxLength: 10, nullable: true),
                    optime = table.Column<DateTime>(name: "op_time", type: "timestamp with time zone", nullable: false),
                    opaddr = table.Column<string>(name: "op_addr", type: "character varying(100)", maxLength: 100, nullable: true),
                    ophostname = table.Column<string>(name: "op_hostname", type: "character varying(100)", maxLength: 100, nullable: true),
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    sindate = table.Column<int>(name: "sin_date", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    uketukesbt = table.Column<int>(name: "uketuke_sbt", type: "integer", nullable: false),
                    createdate = table.Column<DateTime>(name: "create_date", type: "timestamp with time zone", nullable: false),
                    createid = table.Column<int>(name: "create_id", type: "integer", nullable: false),
                    createmachine = table.Column<string>(name: "create_machine", type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_uketuke_sbt_day_inf", x => x.opid);
                });

            migrationBuilder.CreateIndex(
                name: "calc_status_idx01",
                table: "calc_status",
                columns: new[] { "hp_id", "pt_id", "sin_date", "status", "create_machine" });

            migrationBuilder.CreateIndex(
                name: "calc_status_idx02",
                table: "calc_status",
                columns: new[] { "hp_id", "status", "create_machine" });

            migrationBuilder.CreateIndex(
                name: "cmt_kbn_mst_idx01",
                table: "cmt_kbn_mst",
                columns: new[] { "hp_id", "item_cd", "start_date" });

            migrationBuilder.CreateIndex(
                name: "conversion_item_inf_idx01",
                table: "conversion_item_inf",
                columns: new[] { "hp_id", "source_item_cd" });

            migrationBuilder.CreateIndex(
                name: "def_hoken_no_idx01",
                table: "def_hoken_no",
                columns: new[] { "hp_id", "digit_1", "digit_2", "digit_3", "digit_4", "digit_5", "digit_6", "digit_7", "digit_8", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "densi_haihan_custom_idx03",
                table: "densi_haihan_custom",
                columns: new[] { "hp_id", "item_cd1", "haihan_kbn", "start_date", "target_kbn", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "densi_haihan_day_idx03",
                table: "densi_haihan_day",
                columns: new[] { "hp_id", "item_cd1", "haihan_kbn", "start_date", "end_date", "target_kbn", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "densi_haihan_karte_idx03",
                table: "densi_haihan_karte",
                columns: new[] { "hp_id", "item_cd1", "haihan_kbn", "start_date", "end_date", "target_kbn", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "densi_haihan_month_idx03",
                table: "densi_haihan_month",
                columns: new[] { "hp_id", "item_cd1", "haihan_kbn", "start_date", "end_date", "target_kbn", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "densi_haihan_week_idx03",
                table: "densi_haihan_week",
                columns: new[] { "hp_id", "item_cd1", "haihan_kbn", "start_date", "end_date", "target_kbn", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "densi_houkatu_grp_idx02",
                table: "densi_houkatu_grp",
                columns: new[] { "hp_id", "item_cd", "start_date", "end_date", "target_kbn", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "drug_inf_ukey01",
                table: "drug_inf",
                columns: new[] { "hp_id", "item_cd", "inf_kbn", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "eps_prescription_idx01",
                table: "eps_prescription",
                columns: new[] { "hp_id", "prescription_id" });

            migrationBuilder.CreateIndex(
                name: "filing_inf_idx01",
                table: "filing_inf",
                columns: new[] { "pt_id", "get_date", "file_no", "category_cd" });

            migrationBuilder.CreateIndex(
                name: "function_mst_pkey",
                table: "function_mst",
                column: "function_cd");

            migrationBuilder.CreateIndex(
                name: "holiday_mst_ukey01",
                table: "holiday_mst",
                columns: new[] { "hp_id", "sin_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "ipn_kasan_exclude_idx01",
                table: "ipn_kasan_exclude",
                columns: new[] { "hp_id", "start_date", "end_date", "ipn_name_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_kasan_exclude_item_idx01",
                table: "ipn_kasan_exclude_item",
                columns: new[] { "hp_id", "start_date", "end_date", "item_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_min_yakka_mst_idx01",
                table: "ipn_min_yakka_mst",
                columns: new[] { "hp_id", "ipn_name_cd", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ipn_min_yakka_mst_idx02",
                table: "ipn_min_yakka_mst",
                columns: new[] { "hp_id", "start_date", "end_date", "ipn_name_cd" });

            migrationBuilder.CreateIndex(
                name: "ipn_name_mst_idx01",
                table: "ipn_name_mst",
                column: "ipn_name_cd");

            migrationBuilder.CreateIndex(
                name: "pt_ka_mst_idx01",
                table: "ka_mst",
                column: "ka_id");

            migrationBuilder.CreateIndex(
                name: "kaikei_inf_idx01",
                table: "kaikei_inf",
                columns: new[] { "hp_id", "raiin_no" });

            migrationBuilder.CreateIndex(
                name: "karte_inf_idx01",
                table: "karte_inf",
                columns: new[] { "hp_id", "pt_id", "karte_kbn" });

            migrationBuilder.CreateIndex(
                name: "kensa_cmt_mst_skey1",
                table: "kensa_cmt_mst",
                columns: new[] { "hp_id", "cmt_cd", "cmt_seq_no", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "kensa_inf_detail_pt_id_idx",
                table: "kensa_inf_detail",
                columns: new[] { "pt_id", "is_deleted", "kensa_item_cd" });

            migrationBuilder.CreateIndex(
                name: "kensa_mst_idx01",
                table: "kensa_mst",
                column: "kensa_item_cd");

            migrationBuilder.CreateIndex(
                name: "kensa_result_log_idx01",
                table: "kensa_result_log",
                columns: new[] { "hp_id", "imp_date" });

            migrationBuilder.CreateIndex(
                name: "kensa_set_pkey",
                table: "kensa_set",
                columns: new[] { "hp_id", "set_id" });

            migrationBuilder.CreateIndex(
                name: "kensa_set_detail_pkey",
                table: "kensa_set_detail",
                columns: new[] { "hp_id", "set_id", "set_eda_no" });

            migrationBuilder.CreateIndex(
                name: "limit_list_inf_idx01",
                table: "limit_list_inf",
                columns: new[] { "pt_id", "kohi_id", "sin_date", "seq_no" });

            migrationBuilder.CreateIndex(
                name: "IX_lock_inf_hp_id_pt_id_user_id",
                table: "lock_inf",
                columns: new[] { "hp_id", "pt_id", "user_id" },
                unique: true,
                filter: "\"FUNCTION_CD\" IN ('02000000', '03000000')");

            migrationBuilder.CreateIndex(
                name: "m12_food_alrgy_idx01",
                table: "m12_food_alrgy",
                columns: new[] { "kikin_cd", "yj_cd", "food_kbn", "tenpu_level" });

            migrationBuilder.CreateIndex(
                name: "m34_precaution_code_age_min_idx",
                table: "m34_precaution_code",
                columns: new[] { "age_min", "age_max", "sex_cd" });

            migrationBuilder.CreateIndex(
                name: "mall_message_inf_idx01",
                table: "mall_message_inf",
                column: "sin_date");

            migrationBuilder.CreateIndex(
                name: "odr_date_detail_idx01",
                table: "odr_date_detail",
                columns: new[] { "hp_id", "grp_id", "item_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "odr_date_inf_idx01",
                table: "odr_date_inf",
                columns: new[] { "hp_id", "grp_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "odr_inf_idx01",
                table: "odr_inf",
                columns: new[] { "hp_id", "pt_id", "sin_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "odr_inf_raiin_no_idx",
                table: "odr_inf",
                columns: new[] { "raiin_no", "odr_koui_kbn", "inout_kbn", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "odr_inf_detail_idx01",
                table: "odr_inf_detail",
                columns: new[] { "hp_id", "pt_id", "raiin_no", "item_cd" });

            migrationBuilder.CreateIndex(
                name: "odr_inf_detail_idx02",
                table: "odr_inf_detail",
                column: "item_cd");

            migrationBuilder.CreateIndex(
                name: "odr_inf_detail_idx03",
                table: "odr_inf_detail",
                columns: new[] { "sin_date", "pt_id", "raiin_no" });

            migrationBuilder.CreateIndex(
                name: "odr_inf_detail_idx04",
                table: "odr_inf_detail",
                columns: new[] { "pt_id", "sin_date", "item_cd" });

            migrationBuilder.CreateIndex(
                name: "online_confirmation_history_idx01",
                table: "online_confirmation_history",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "pt_path_conf_idx01",
                table: "path_conf",
                columns: new[] { "hp_id", "grp_cd", "grp_eda_no", "machine", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "pt_path_conf_pkey",
                table: "path_conf",
                columns: new[] { "hp_id", "grp_cd", "grp_eda_no", "seq_no" });

            migrationBuilder.CreateIndex(
                name: "permission_mst_pkey",
                table: "permission_mst",
                columns: new[] { "function_cd", "permission" });

            migrationBuilder.CreateIndex(
                name: "pt_post_code_mst_idx01",
                table: "post_code_mst",
                columns: new[] { "hp_id", "post_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "ptcmt_inf_idx01",
                table: "pt_cmt_inf",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "pt_family_idx01",
                table: "pt_family",
                columns: new[] { "family_id", "pt_id", "family_pt_id" });

            migrationBuilder.CreateIndex(
                name: "pt_family_reki_idx01",
                table: "pt_family_reki",
                columns: new[] { "id", "pt_id", "family_id" });

            migrationBuilder.CreateIndex(
                name: "pt_grp_inf_idx01",
                table: "pt_grp_inf",
                columns: new[] { "hp_id", "pt_id", "grp_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_grp_item_idx01",
                table: "pt_grp_item",
                columns: new[] { "hp_id", "grp_id", "grp_code", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_grp_name_idx01",
                table: "pt_grp_name_mst",
                columns: new[] { "hp_id", "grp_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_hoken_inf_idx01",
                table: "pt_hoken_inf",
                columns: new[] { "hp_id", "pt_id", "hoken_id", "hoken_kbn", "houbetu" });

            migrationBuilder.CreateIndex(
                name: "pt_hoken_pattern_idx01",
                table: "pt_hoken_pattern",
                columns: new[] { "hp_id", "pt_id", "start_date", "end_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_hoken_scan_idx01",
                table: "pt_hoken_scan",
                columns: new[] { "hp_id", "pt_id", "hoken_grp", "hoken_id" });

            migrationBuilder.CreateIndex(
                name: "pt_hoken_scan_pkey",
                table: "pt_hoken_scan",
                columns: new[] { "hp_id", "pt_id", "hoken_grp", "hoken_id", "seq_no", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_inf_idx01",
                table: "pt_inf",
                columns: new[] { "hp_id", "pt_num" });

            migrationBuilder.CreateIndex(
                name: "pt_inf_idx02",
                table: "pt_inf",
                columns: new[] { "hp_id", "pt_id", "is_delete" });

            migrationBuilder.CreateIndex(
                name: "pt_jibkar_idx01",
                table: "pt_jibkar",
                columns: new[] { "hp_id", "web_id", "pt_id" });

            migrationBuilder.CreateIndex(
                name: "pt_kyusei_idx01",
                table: "pt_kyusei",
                columns: new[] { "hp_id", "pt_id", "end_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_memo_idx01",
                table: "pt_memo",
                columns: new[] { "hp_id", "pt_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "ptpregnancy_idx01",
                table: "pt_pregnancy",
                columns: new[] { "id", "hp_id" });

            migrationBuilder.CreateIndex(
                name: "pt_rousai_tenki_idx01",
                table: "pt_rousai_tenki",
                columns: new[] { "hp_id", "pt_id", "hoken_id", "end_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_calc_conf_idx01",
                table: "pt_santei_conf",
                columns: new[] { "hp_id", "pt_id", "kbn_no", "eda_no", "start_date", "end_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "pt_calc_conf_pkey",
                table: "pt_santei_conf",
                columns: new[] { "hp_id", "pt_id", "kbn_no", "eda_no", "seq_no" });

            migrationBuilder.CreateIndex(
                name: "pt_tag_idx01",
                table: "pt_tag",
                columns: new[] { "hp_id", "pt_id", "start_date", "end_date", "is_dsp_uketuke", "is_dsp_karte", "is_dsp_kaikei", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_cmt_inf_idx01",
                table: "raiin_cmt_inf",
                columns: new[] { "hp_id", "raiin_no", "cmt_kbn", "is_delete" });

            migrationBuilder.CreateIndex(
                name: "raiin_filter_mst_idx01",
                table: "raiin_filter_mst",
                columns: new[] { "hp_id", "filter_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "karte_inf_idx011",
                table: "raiin_filter_sort",
                columns: new[] { "id", "hp_id", "filter_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_inf_idx01",
                table: "raiin_inf",
                columns: new[] { "hp_id", "pt_id", "sin_date", "status", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_inf_idx02",
                table: "raiin_inf",
                columns: new[] { "hp_id", "pt_id", "sin_date", "status", "syosaisin_kbn", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_inf_idx03",
                table: "raiin_inf",
                columns: new[] { "is_deleted", "sin_date", "pt_id" });

            migrationBuilder.CreateIndex(
                name: "raiin_inf_idx04",
                table: "raiin_inf",
                columns: new[] { "hp_id", "raiin_no", "is_deleted", "status" });

            migrationBuilder.CreateIndex(
                name: "raiin_kbn_detail_idx01",
                table: "raiin_kbn_detail",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_kbn_inf_idx01",
                table: "raiin_kbn_inf",
                columns: new[] { "hp_id", "pt_id", "sin_date", "raiin_no", "grp_id", "is_delete" });

            migrationBuilder.CreateIndex(
                name: "raiin_kbn_item_idx01",
                table: "raiin_kbn_item",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_kbn_koui_idx01",
                table: "raiin_kbn_koui",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_kbn_mst_idx01",
                table: "raiin_kbn_mst",
                columns: new[] { "hp_id", "grp_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_kbn_yoyaku_idx01",
                table: "raiin_kbn_yoyaku",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_cmt_ukey01",
                table: "raiin_list_cmt",
                columns: new[] { "hp_id", "raiin_no", "cmt_kbn", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_detail_idx01",
                table: "raiin_list_detail",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_detail_idx02",
                table: "raiin_list_detail",
                columns: new[] { "hp_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_file_idx01",
                table: "raiin_list_file",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_inf_idx01",
                table: "raiin_list_inf",
                columns: new[] { "grp_id", "kbn_cd", "raiin_list_kbn" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_inf_idx02",
                table: "raiin_list_inf",
                columns: new[] { "hp_id", "pt_id" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_item_idx01",
                table: "raiin_list_item",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_koui_idx01",
                table: "raiin_list_koui",
                columns: new[] { "hp_id", "grp_id", "kbn_cd", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_mst_idx01",
                table: "raiin_list_mst",
                columns: new[] { "hp_id", "grp_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "raiin_list_tag_ukey01",
                table: "raiin_list_tag",
                columns: new[] { "hp_id", "raiin_no", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "rece_cmt_idx01",
                table: "rece_cmt",
                columns: new[] { "hp_id", "pt_id", "sin_ym", "hoken_id", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "receden_cmt_select_idx01",
                table: "receden_cmt_select",
                columns: new[] { "hp_id", "item_cd", "start_date", "comment_cd", "is_invalid" });

            migrationBuilder.CreateIndex(
                name: "IX_rsvkrt_mst_hp_id_pt_id_rsv_date",
                table: "rsvkrt_mst",
                columns: new[] { "hp_id", "pt_id", "rsv_date" },
                unique: true,
                filter: "\"RSVKRT_KBN\" = 0 AND \"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_set_mst_hp_id_set_cd_set_kbn_set_kbn_eda_no_generation_id_l~",
                table: "set_mst",
                columns: new[] { "hp_id", "set_cd", "set_kbn", "set_kbn_eda_no", "generation_id", "level1", "level2", "level3" },
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "ten_mst_idx08",
                table: "ten_mst",
                columns: new[] { "hp_id", "item_cd", "start_date", "end_date", "name", "kana_name1", "kana_name2", "kana_name3", "kana_name4", "kana_name5", "kana_name6", "kana_name7", "is_deleted", "is_adopted" });

            migrationBuilder.CreateIndex(
                name: "IX_user_mst_user_id",
                table: "user_mst",
                column: "user_id",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "yakka_syusai_mst_idx01",
                table: "yakka_syusai_mst",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "pt_kyusei_idx011",
                table: "z_pt_kyusei",
                columns: new[] { "hp_id", "pt_id", "end_date", "is_deleted" });

            migrationBuilder.CreateIndex(
                name: "syuno_nyukin_idx01",
                table: "z_syuno_nyukin",
                columns: new[] { "hp_id", "pt_id", "sin_date", "raiin_no", "is_deleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_form_mst");

            migrationBuilder.DropTable(
                name: "approval_inf");

            migrationBuilder.DropTable(
                name: "audit_trail_log");

            migrationBuilder.DropTable(
                name: "audit_trail_log_detail");

            migrationBuilder.DropTable(
                name: "auto_santei_mst");

            migrationBuilder.DropTable(
                name: "backup_req");

            migrationBuilder.DropTable(
                name: "bui_odr_byomei_mst");

            migrationBuilder.DropTable(
                name: "bui_odr_item_byomei_mst");

            migrationBuilder.DropTable(
                name: "bui_odr_item_mst");

            migrationBuilder.DropTable(
                name: "bui_odr_mst");

            migrationBuilder.DropTable(
                name: "byomei_mst");

            migrationBuilder.DropTable(
                name: "byomei_mst_aftercare");

            migrationBuilder.DropTable(
                name: "byomei_set_generation_mst");

            migrationBuilder.DropTable(
                name: "byomei_set_mst");

            migrationBuilder.DropTable(
                name: "calc_log");

            migrationBuilder.DropTable(
                name: "calc_status");

            migrationBuilder.DropTable(
                name: "cmt_check_mst");

            migrationBuilder.DropTable(
                name: "cmt_kbn_mst");

            migrationBuilder.DropTable(
                name: "column_setting");

            migrationBuilder.DropTable(
                name: "container_mst");

            migrationBuilder.DropTable(
                name: "conversion_item_inf");

            migrationBuilder.DropTable(
                name: "def_hoken_no");

            migrationBuilder.DropTable(
                name: "densi_haihan_custom");

            migrationBuilder.DropTable(
                name: "densi_haihan_day");

            migrationBuilder.DropTable(
                name: "densi_haihan_karte");

            migrationBuilder.DropTable(
                name: "densi_haihan_month");

            migrationBuilder.DropTable(
                name: "densi_haihan_week");

            migrationBuilder.DropTable(
                name: "densi_hojyo");

            migrationBuilder.DropTable(
                name: "densi_houkatu");

            migrationBuilder.DropTable(
                name: "densi_houkatu_grp");

            migrationBuilder.DropTable(
                name: "densi_santei_kaisu");

            migrationBuilder.DropTable(
                name: "doc_category_mst");

            migrationBuilder.DropTable(
                name: "doc_comment");

            migrationBuilder.DropTable(
                name: "doc_comment_detail");

            migrationBuilder.DropTable(
                name: "doc_inf");

            migrationBuilder.DropTable(
                name: "dosage_mst");

            migrationBuilder.DropTable(
                name: "drug_day_limit");

            migrationBuilder.DropTable(
                name: "drug_inf");

            migrationBuilder.DropTable(
                name: "drug_unit_conv");

            migrationBuilder.DropTable(
                name: "eps_chk");

            migrationBuilder.DropTable(
                name: "eps_chk_detail");

            migrationBuilder.DropTable(
                name: "eps_prescription");

            migrationBuilder.DropTable(
                name: "eps_reference");

            migrationBuilder.DropTable(
                name: "event_mst");

            migrationBuilder.DropTable(
                name: "except_hokensya");

            migrationBuilder.DropTable(
                name: "filing_auto_imp");

            migrationBuilder.DropTable(
                name: "filing_category_mst");

            migrationBuilder.DropTable(
                name: "filing_inf");

            migrationBuilder.DropTable(
                name: "function_mst");

            migrationBuilder.DropTable(
                name: "gc_std_mst");

            migrationBuilder.DropTable(
                name: "hoken_mst");

            migrationBuilder.DropTable(
                name: "hokensya_mst");

            migrationBuilder.DropTable(
                name: "holiday_mst");

            migrationBuilder.DropTable(
                name: "hp_inf");

            migrationBuilder.DropTable(
                name: "ipn_kasan_exclude");

            migrationBuilder.DropTable(
                name: "ipn_kasan_exclude_item");

            migrationBuilder.DropTable(
                name: "ipn_kasan_mst");

            migrationBuilder.DropTable(
                name: "ipn_min_yakka_mst");

            migrationBuilder.DropTable(
                name: "ipn_name_mst");

            migrationBuilder.DropTable(
                name: "item_grp_mst");

            migrationBuilder.DropTable(
                name: "jihi_sbt_mst");

            migrationBuilder.DropTable(
                name: "job_mst");

            migrationBuilder.DropTable(
                name: "json_setting");

            migrationBuilder.DropTable(
                name: "ka_mst");

            migrationBuilder.DropTable(
                name: "kacode_mst");

            migrationBuilder.DropTable(
                name: "kacode_rece_yousiki");

            migrationBuilder.DropTable(
                name: "kacode_yousiki_mst");

            migrationBuilder.DropTable(
                name: "kaikei_detail");

            migrationBuilder.DropTable(
                name: "kaikei_inf");

            migrationBuilder.DropTable(
                name: "kantoku_mst");

            migrationBuilder.DropTable(
                name: "karte_filter_detail");

            migrationBuilder.DropTable(
                name: "karte_filter_mst");

            migrationBuilder.DropTable(
                name: "karte_img_inf");

            migrationBuilder.DropTable(
                name: "karte_inf");

            migrationBuilder.DropTable(
                name: "karte_kbn_mst");

            migrationBuilder.DropTable(
                name: "kensa_center_mst");

            migrationBuilder.DropTable(
                name: "kensa_cmt_mst");

            migrationBuilder.DropTable(
                name: "kensa_inf");

            migrationBuilder.DropTable(
                name: "kensa_inf_detail");

            migrationBuilder.DropTable(
                name: "kensa_irai_log");

            migrationBuilder.DropTable(
                name: "kensa_mst");

            migrationBuilder.DropTable(
                name: "kensa_result_log");

            migrationBuilder.DropTable(
                name: "kensa_set");

            migrationBuilder.DropTable(
                name: "kensa_set_detail");

            migrationBuilder.DropTable(
                name: "kensa_std_mst");

            migrationBuilder.DropTable(
                name: "kinki_mst");

            migrationBuilder.DropTable(
                name: "kogaku_limit");

            migrationBuilder.DropTable(
                name: "kohi_priority");

            migrationBuilder.DropTable(
                name: "koui_houkatu_mst");

            migrationBuilder.DropTable(
                name: "koui_kbn_mst");

            migrationBuilder.DropTable(
                name: "limit_cnt_list_inf");

            migrationBuilder.DropTable(
                name: "limit_list_inf");

            migrationBuilder.DropTable(
                name: "list_set_generation_mst");

            migrationBuilder.DropTable(
                name: "list_set_mst");

            migrationBuilder.DropTable(
                name: "lock_inf");

            migrationBuilder.DropTable(
                name: "lock_mst");

            migrationBuilder.DropTable(
                name: "m01_kijyo_cmt");

            migrationBuilder.DropTable(
                name: "m01_kinki");

            migrationBuilder.DropTable(
                name: "m01_kinki_cmt");

            migrationBuilder.DropTable(
                name: "m10_day_limit");

            migrationBuilder.DropTable(
                name: "m12_food_alrgy");

            migrationBuilder.DropTable(
                name: "m12_food_alrgy_kbn");

            migrationBuilder.DropTable(
                name: "m14_age_check");

            migrationBuilder.DropTable(
                name: "m14_cmt_code");

            migrationBuilder.DropTable(
                name: "m28_drug_mst");

            migrationBuilder.DropTable(
                name: "m34_ar_code");

            migrationBuilder.DropTable(
                name: "m34_ar_discon");

            migrationBuilder.DropTable(
                name: "m34_ar_discon_code");

            migrationBuilder.DropTable(
                name: "m34_drug_info_main");

            migrationBuilder.DropTable(
                name: "m34_form_code");

            migrationBuilder.DropTable(
                name: "m34_indication_code");

            migrationBuilder.DropTable(
                name: "m34_interaction_pat");

            migrationBuilder.DropTable(
                name: "m34_interaction_pat_code");

            migrationBuilder.DropTable(
                name: "m34_precaution_code");

            migrationBuilder.DropTable(
                name: "m34_precautions");

            migrationBuilder.DropTable(
                name: "m34_property_code");

            migrationBuilder.DropTable(
                name: "m34_sar_symptom_code");

            migrationBuilder.DropTable(
                name: "m38_class_code");

            migrationBuilder.DropTable(
                name: "m38_ing_code");

            migrationBuilder.DropTable(
                name: "m38_ingredients");

            migrationBuilder.DropTable(
                name: "m38_major_div_code");

            migrationBuilder.DropTable(
                name: "m38_otc_form_code");

            migrationBuilder.DropTable(
                name: "m38_otc_main");

            migrationBuilder.DropTable(
                name: "m38_otc_maker_code");

            migrationBuilder.DropTable(
                name: "m41_supple_indexcode");

            migrationBuilder.DropTable(
                name: "m41_supple_indexdef");

            migrationBuilder.DropTable(
                name: "m41_supple_ingre");

            migrationBuilder.DropTable(
                name: "m42_contra_cmt");

            migrationBuilder.DropTable(
                name: "m42_contraindi_dis_bc");

            migrationBuilder.DropTable(
                name: "m42_contraindi_dis_class");

            migrationBuilder.DropTable(
                name: "m42_contraindi_dis_con");

            migrationBuilder.DropTable(
                name: "m42_contraindi_drug_main_ex");

            migrationBuilder.DropTable(
                name: "m46_dosage_dosage");

            migrationBuilder.DropTable(
                name: "m46_dosage_drug");

            migrationBuilder.DropTable(
                name: "m56_alrgy_derivatives");

            migrationBuilder.DropTable(
                name: "m56_analogue_cd");

            migrationBuilder.DropTable(
                name: "m56_drug_class");

            migrationBuilder.DropTable(
                name: "m56_drvalrgy_code");

            migrationBuilder.DropTable(
                name: "m56_ex_analogue");

            migrationBuilder.DropTable(
                name: "m56_ex_ed_ingredients");

            migrationBuilder.DropTable(
                name: "m56_ex_ing_code");

            migrationBuilder.DropTable(
                name: "m56_ex_ingrdt_main");

            migrationBuilder.DropTable(
                name: "m56_prodrug_cd");

            migrationBuilder.DropTable(
                name: "m56_usage_code");

            migrationBuilder.DropTable(
                name: "m56_yj_drug_class");

            migrationBuilder.DropTable(
                name: "mall_message_inf");

            migrationBuilder.DropTable(
                name: "mall_renkei_inf");

            migrationBuilder.DropTable(
                name: "material_mst");

            migrationBuilder.DropTable(
                name: "monshin_inf");

            migrationBuilder.DropTable(
                name: "odr_date_detail");

            migrationBuilder.DropTable(
                name: "odr_date_inf");

            migrationBuilder.DropTable(
                name: "odr_inf");

            migrationBuilder.DropTable(
                name: "odr_inf_cmt");

            migrationBuilder.DropTable(
                name: "odr_inf_detail");

            migrationBuilder.DropTable(
                name: "online_confirmation");

            migrationBuilder.DropTable(
                name: "online_confirmation_history");

            migrationBuilder.DropTable(
                name: "online_consent");

            migrationBuilder.DropTable(
                name: "path_conf");

            migrationBuilder.DropTable(
                name: "payment_method_mst");

            migrationBuilder.DropTable(
                name: "permission_mst");

            migrationBuilder.DropTable(
                name: "physical_average");

            migrationBuilder.DropTable(
                name: "pi_image");

            migrationBuilder.DropTable(
                name: "pi_inf");

            migrationBuilder.DropTable(
                name: "pi_inf_detail");

            migrationBuilder.DropTable(
                name: "pi_product_inf");

            migrationBuilder.DropTable(
                name: "post_code_mst");

            migrationBuilder.DropTable(
                name: "priority_haihan_mst");

            migrationBuilder.DropTable(
                name: "pt_alrgy_drug");

            migrationBuilder.DropTable(
                name: "pt_alrgy_else");

            migrationBuilder.DropTable(
                name: "pt_alrgy_food");

            migrationBuilder.DropTable(
                name: "pt_byomei");

            migrationBuilder.DropTable(
                name: "pt_cmt_inf");

            migrationBuilder.DropTable(
                name: "pt_family");

            migrationBuilder.DropTable(
                name: "pt_family_reki");

            migrationBuilder.DropTable(
                name: "pt_grp_inf");

            migrationBuilder.DropTable(
                name: "pt_grp_item");

            migrationBuilder.DropTable(
                name: "pt_grp_name_mst");

            migrationBuilder.DropTable(
                name: "pt_hoken_check");

            migrationBuilder.DropTable(
                name: "pt_hoken_inf");

            migrationBuilder.DropTable(
                name: "pt_hoken_pattern");

            migrationBuilder.DropTable(
                name: "pt_hoken_scan");

            migrationBuilder.DropTable(
                name: "pt_inf");

            migrationBuilder.DropTable(
                name: "pt_infection");

            migrationBuilder.DropTable(
                name: "pt_jibai_doc");

            migrationBuilder.DropTable(
                name: "pt_jibkar");

            migrationBuilder.DropTable(
                name: "pt_kio_reki");

            migrationBuilder.DropTable(
                name: "pt_kohi");

            migrationBuilder.DropTable(
                name: "pt_kyusei");

            migrationBuilder.DropTable(
                name: "pt_last_visit_date");

            migrationBuilder.DropTable(
                name: "pt_memo");

            migrationBuilder.DropTable(
                name: "pt_otc_drug");

            migrationBuilder.DropTable(
                name: "pt_other_drug");

            migrationBuilder.DropTable(
                name: "pt_pregnancy");

            migrationBuilder.DropTable(
                name: "pt_rousai_tenki");

            migrationBuilder.DropTable(
                name: "pt_santei_conf");

            migrationBuilder.DropTable(
                name: "pt_supple");

            migrationBuilder.DropTable(
                name: "pt_tag");

            migrationBuilder.DropTable(
                name: "raiin_cmt_inf");

            migrationBuilder.DropTable(
                name: "raiin_filter_kbn");

            migrationBuilder.DropTable(
                name: "raiin_filter_mst");

            migrationBuilder.DropTable(
                name: "raiin_filter_sort");

            migrationBuilder.DropTable(
                name: "raiin_filter_state");

            migrationBuilder.DropTable(
                name: "raiin_inf");

            migrationBuilder.DropTable(
                name: "raiin_kbn_detail");

            migrationBuilder.DropTable(
                name: "raiin_kbn_inf");

            migrationBuilder.DropTable(
                name: "raiin_kbn_item");

            migrationBuilder.DropTable(
                name: "raiin_kbn_koui");

            migrationBuilder.DropTable(
                name: "raiin_kbn_mst");

            migrationBuilder.DropTable(
                name: "raiin_kbn_yoyaku");

            migrationBuilder.DropTable(
                name: "raiin_list_cmt");

            migrationBuilder.DropTable(
                name: "raiin_list_detail");

            migrationBuilder.DropTable(
                name: "raiin_list_doc");

            migrationBuilder.DropTable(
                name: "raiin_list_file");

            migrationBuilder.DropTable(
                name: "raiin_list_inf");

            migrationBuilder.DropTable(
                name: "raiin_list_item");

            migrationBuilder.DropTable(
                name: "raiin_list_koui");

            migrationBuilder.DropTable(
                name: "raiin_list_mst");

            migrationBuilder.DropTable(
                name: "raiin_list_tag");

            migrationBuilder.DropTable(
                name: "rece_check_cmt");

            migrationBuilder.DropTable(
                name: "rece_check_err");

            migrationBuilder.DropTable(
                name: "rece_check_opt");

            migrationBuilder.DropTable(
                name: "rece_cmt");

            migrationBuilder.DropTable(
                name: "rece_futan_kbn");

            migrationBuilder.DropTable(
                name: "rece_inf");

            migrationBuilder.DropTable(
                name: "rece_inf_edit");

            migrationBuilder.DropTable(
                name: "rece_inf_jd");

            migrationBuilder.DropTable(
                name: "rece_inf_pre_edit");

            migrationBuilder.DropTable(
                name: "rece_seikyu");

            migrationBuilder.DropTable(
                name: "rece_status");

            migrationBuilder.DropTable(
                name: "receden_cmt_select");

            migrationBuilder.DropTable(
                name: "receden_hen_jiyuu");

            migrationBuilder.DropTable(
                name: "receden_rireki_inf");

            migrationBuilder.DropTable(
                name: "releasenote_read");

            migrationBuilder.DropTable(
                name: "renkei_conf");

            migrationBuilder.DropTable(
                name: "renkei_mst");

            migrationBuilder.DropTable(
                name: "renkei_path_conf");

            migrationBuilder.DropTable(
                name: "renkei_req");

            migrationBuilder.DropTable(
                name: "renkei_template_mst");

            migrationBuilder.DropTable(
                name: "renkei_timing_conf");

            migrationBuilder.DropTable(
                name: "renkei_timing_mst");

            migrationBuilder.DropTable(
                name: "roudou_mst");

            migrationBuilder.DropTable(
                name: "rousai_gosei_mst");

            migrationBuilder.DropTable(
                name: "rsv_day_comment");

            migrationBuilder.DropTable(
                name: "rsv_frame_day_ptn");

            migrationBuilder.DropTable(
                name: "rsv_frame_inf");

            migrationBuilder.DropTable(
                name: "rsv_frame_mst");

            migrationBuilder.DropTable(
                name: "rsv_frame_week_ptn");

            migrationBuilder.DropTable(
                name: "rsv_frame_with");

            migrationBuilder.DropTable(
                name: "rsv_grp_mst");

            migrationBuilder.DropTable(
                name: "rsv_inf");

            migrationBuilder.DropTable(
                name: "rsv_renkei_inf");

            migrationBuilder.DropTable(
                name: "rsv_renkei_inf_tk");

            migrationBuilder.DropTable(
                name: "rsvkrt_byomei");

            migrationBuilder.DropTable(
                name: "rsvkrt_karte_img_inf");

            migrationBuilder.DropTable(
                name: "rsvkrt_karte_inf");

            migrationBuilder.DropTable(
                name: "rsvkrt_mst");

            migrationBuilder.DropTable(
                name: "rsvkrt_odr_inf");

            migrationBuilder.DropTable(
                name: "rsvkrt_odr_inf_cmt");

            migrationBuilder.DropTable(
                name: "rsvkrt_odr_inf_detail");

            migrationBuilder.DropTable(
                name: "santei_auto_order");

            migrationBuilder.DropTable(
                name: "santei_auto_order_detail");

            migrationBuilder.DropTable(
                name: "santei_cnt_check");

            migrationBuilder.DropTable(
                name: "santei_grp_detail");

            migrationBuilder.DropTable(
                name: "santei_grp_mst");

            migrationBuilder.DropTable(
                name: "santei_inf");

            migrationBuilder.DropTable(
                name: "santei_inf_detail");

            migrationBuilder.DropTable(
                name: "schema_cmt_mst");

            migrationBuilder.DropTable(
                name: "seikatureki_inf");

            migrationBuilder.DropTable(
                name: "sentence_list");

            migrationBuilder.DropTable(
                name: "session_inf");

            migrationBuilder.DropTable(
                name: "set_byomei");

            migrationBuilder.DropTable(
                name: "set_generation_mst");

            migrationBuilder.DropTable(
                name: "set_karte_img_inf");

            migrationBuilder.DropTable(
                name: "set_karte_inf");

            migrationBuilder.DropTable(
                name: "set_kbn_mst");

            migrationBuilder.DropTable(
                name: "set_mst");

            migrationBuilder.DropTable(
                name: "set_odr_inf");

            migrationBuilder.DropTable(
                name: "set_odr_inf_cmt");

            migrationBuilder.DropTable(
                name: "set_odr_inf_detail");

            migrationBuilder.DropTable(
                name: "sin_koui");

            migrationBuilder.DropTable(
                name: "sin_koui_count");

            migrationBuilder.DropTable(
                name: "sin_koui_detail");

            migrationBuilder.DropTable(
                name: "sin_rp_inf");

            migrationBuilder.DropTable(
                name: "sin_rp_no_inf");

            migrationBuilder.DropTable(
                name: "single_dose_mst");

            migrationBuilder.DropTable(
                name: "sinreki_filter_mst");

            migrationBuilder.DropTable(
                name: "sinreki_filter_mst_detail");

            migrationBuilder.DropTable(
                name: "sinreki_filter_mst_koui");

            migrationBuilder.DropTable(
                name: "smartkarte_app_signalr_port");

            migrationBuilder.DropTable(
                name: "sokatu_mst");

            migrationBuilder.DropTable(
                name: "sta_conf");

            migrationBuilder.DropTable(
                name: "sta_csv");

            migrationBuilder.DropTable(
                name: "sta_grp");

            migrationBuilder.DropTable(
                name: "sta_menu");

            migrationBuilder.DropTable(
                name: "sta_mst");

            migrationBuilder.DropTable(
                name: "summary_inf");

            migrationBuilder.DropTable(
                name: "syobyo_keika");

            migrationBuilder.DropTable(
                name: "syouki_inf");

            migrationBuilder.DropTable(
                name: "syouki_kbn_mst");

            migrationBuilder.DropTable(
                name: "system_change_log");

            migrationBuilder.DropTable(
                name: "system_conf");

            migrationBuilder.DropTable(
                name: "system_conf_item");

            migrationBuilder.DropTable(
                name: "system_conf_menu");

            migrationBuilder.DropTable(
                name: "system_generation_conf");

            migrationBuilder.DropTable(
                name: "syuno_nyukin");

            migrationBuilder.DropTable(
                name: "syuno_seikyu");

            migrationBuilder.DropTable(
                name: "tag_grp_mst");

            migrationBuilder.DropTable(
                name: "tekiou_byomei_mst");

            migrationBuilder.DropTable(
                name: "tekiou_byomei_mst_excluded");

            migrationBuilder.DropTable(
                name: "template_detail");

            migrationBuilder.DropTable(
                name: "template_dsp_conf");

            migrationBuilder.DropTable(
                name: "template_menu_detail");

            migrationBuilder.DropTable(
                name: "template_menu_mst");

            migrationBuilder.DropTable(
                name: "template_mst");

            migrationBuilder.DropTable(
                name: "ten_mst");

            migrationBuilder.DropTable(
                name: "ten_mst_mother");

            migrationBuilder.DropTable(
                name: "time_zone_conf");

            migrationBuilder.DropTable(
                name: "time_zone_day_inf");

            migrationBuilder.DropTable(
                name: "todo_grp_mst");

            migrationBuilder.DropTable(
                name: "todo_inf");

            migrationBuilder.DropTable(
                name: "todo_kbn_mst");

            migrationBuilder.DropTable(
                name: "tokki_mst");

            migrationBuilder.DropTable(
                name: "uketuke_sbt_day_inf");

            migrationBuilder.DropTable(
                name: "uketuke_sbt_mst");

            migrationBuilder.DropTable(
                name: "unit_mst");

            migrationBuilder.DropTable(
                name: "user_conf");

            migrationBuilder.DropTable(
                name: "user_mst");

            migrationBuilder.DropTable(
                name: "user_permission");

            migrationBuilder.DropTable(
                name: "user_token");

            migrationBuilder.DropTable(
                name: "wrk_sin_koui");

            migrationBuilder.DropTable(
                name: "wrk_sin_koui_detail");

            migrationBuilder.DropTable(
                name: "wrk_sin_koui_detail_del");

            migrationBuilder.DropTable(
                name: "wrk_sin_rp_inf");

            migrationBuilder.DropTable(
                name: "yakka_syusai_mst");

            migrationBuilder.DropTable(
                name: "yoho_hosoku");

            migrationBuilder.DropTable(
                name: "yoho_inf_mst");

            migrationBuilder.DropTable(
                name: "yoho_mst");

            migrationBuilder.DropTable(
                name: "yoho_set_mst");

            migrationBuilder.DropTable(
                name: "yoyaku_odr_inf");

            migrationBuilder.DropTable(
                name: "yoyaku_odr_inf_detail");

            migrationBuilder.DropTable(
                name: "yoyaku_sbt_mst");

            migrationBuilder.DropTable(
                name: "z_doc_inf");

            migrationBuilder.DropTable(
                name: "z_filing_inf");

            migrationBuilder.DropTable(
                name: "z_kensa_inf");

            migrationBuilder.DropTable(
                name: "z_kensa_inf_detail");

            migrationBuilder.DropTable(
                name: "z_limit_cnt_list_inf");

            migrationBuilder.DropTable(
                name: "z_limit_list_inf");

            migrationBuilder.DropTable(
                name: "z_monshin_inf");

            migrationBuilder.DropTable(
                name: "z_pt_alrgy_drug");

            migrationBuilder.DropTable(
                name: "z_pt_alrgy_else");

            migrationBuilder.DropTable(
                name: "z_pt_alrgy_food");

            migrationBuilder.DropTable(
                name: "z_pt_cmt_inf");

            migrationBuilder.DropTable(
                name: "z_pt_family");

            migrationBuilder.DropTable(
                name: "z_pt_family_reki");

            migrationBuilder.DropTable(
                name: "z_pt_grp_inf");

            migrationBuilder.DropTable(
                name: "z_pt_hoken_check");

            migrationBuilder.DropTable(
                name: "z_pt_hoken_inf");

            migrationBuilder.DropTable(
                name: "z_pt_hoken_pattern");

            migrationBuilder.DropTable(
                name: "z_pt_hoken_scan");

            migrationBuilder.DropTable(
                name: "z_pt_inf");

            migrationBuilder.DropTable(
                name: "z_pt_infection");

            migrationBuilder.DropTable(
                name: "z_pt_jibkar");

            migrationBuilder.DropTable(
                name: "z_pt_kio_reki");

            migrationBuilder.DropTable(
                name: "z_pt_kohi");

            migrationBuilder.DropTable(
                name: "z_pt_kyusei");

            migrationBuilder.DropTable(
                name: "z_pt_memo");

            migrationBuilder.DropTable(
                name: "z_pt_otc_drug");

            migrationBuilder.DropTable(
                name: "z_pt_other_drug");

            migrationBuilder.DropTable(
                name: "z_pt_pregnancy");

            migrationBuilder.DropTable(
                name: "z_pt_rousai_tenki");

            migrationBuilder.DropTable(
                name: "z_pt_santei_conf");

            migrationBuilder.DropTable(
                name: "z_pt_supple");

            migrationBuilder.DropTable(
                name: "z_pt_tag");

            migrationBuilder.DropTable(
                name: "z_raiin_cmt_inf");

            migrationBuilder.DropTable(
                name: "z_raiin_inf");

            migrationBuilder.DropTable(
                name: "z_raiin_kbn_inf");

            migrationBuilder.DropTable(
                name: "z_raiin_list_cmt");

            migrationBuilder.DropTable(
                name: "z_raiin_list_tag");

            migrationBuilder.DropTable(
                name: "z_rece_check_cmt");

            migrationBuilder.DropTable(
                name: "z_rece_cmt");

            migrationBuilder.DropTable(
                name: "z_rece_inf_edit");

            migrationBuilder.DropTable(
                name: "z_rece_seikyu");

            migrationBuilder.DropTable(
                name: "z_rsv_day_comment");

            migrationBuilder.DropTable(
                name: "z_rsv_inf");

            migrationBuilder.DropTable(
                name: "z_santei_inf_detail");

            migrationBuilder.DropTable(
                name: "z_seikatureki_inf");

            migrationBuilder.DropTable(
                name: "z_summary_inf");

            migrationBuilder.DropTable(
                name: "z_syobyo_keika");

            migrationBuilder.DropTable(
                name: "z_syouki_inf");

            migrationBuilder.DropTable(
                name: "z_syuno_nyukin");

            migrationBuilder.DropTable(
                name: "z_todo_inf");

            migrationBuilder.DropTable(
                name: "z_uketuke_sbt_day_inf");
        }
    }
}
