using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableOutDrug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YOHO_CD",
                table: "TEN_MST",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YOHO_HOSOKU_KBN",
                table: "TEN_MST",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YOHO_HOSOKU_REC",
                table: "TEN_MST",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EPS_CHK",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    CHECKRESULT = table.Column<int>(name: "CHECK_RESULT", type: "integer", nullable: false),
                    SAMEMEDICALINSTITUTIONALERTFLG = table.Column<int>(name: "SAME_MEDICAL_INSTITUTION_ALERT_FLG", type: "integer", nullable: false),
                    ONLINECONSENT = table.Column<int>(name: "ONLINE_CONSENT", type: "integer", nullable: false),
                    ORALBROWSINGCONSENT = table.Column<int>(name: "ORAL_BROWSING_CONSENT", type: "integer", nullable: false),
                    DRUGINFO = table.Column<string>(name: "DRUG_INFO", type: "text", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPS_CHK", x => new { x.HPID, x.PTID, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "EPS_CHK_DETAIL",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false),
                    MESSAGEID = table.Column<string>(name: "MESSAGE_ID", type: "character varying(3)", maxLength: 3, nullable: false),
                    MESSAGECATEGORY = table.Column<string>(name: "MESSAGE_CATEGORY", type: "character varying(50)", maxLength: 50, nullable: false),
                    PHARMACEUTICALSINGREDIENTNAME = table.Column<string>(name: "PHARMACEUTICALS_INGREDIENT_NAME", type: "character varying(80)", maxLength: 80, nullable: false),
                    MESSAGE = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    TARGETPHARMACEUTICALCODETYPE = table.Column<string>(name: "TARGET_PHARMACEUTICAL_CODE_TYPE", type: "character varying(1)", maxLength: 1, nullable: false),
                    TARGETPHARMACEUTICALCODE = table.Column<string>(name: "TARGET_PHARMACEUTICAL_CODE", type: "character varying(13)", maxLength: 13, nullable: false),
                    TARGETPHARMACEUTICALNAME = table.Column<string>(name: "TARGET_PHARMACEUTICAL_NAME", type: "character varying(80)", maxLength: 80, nullable: false),
                    TARGETDISPENSINGQUANTITY = table.Column<string>(name: "TARGET_DISPENSING_QUANTITY", type: "character varying(3)", maxLength: 3, nullable: false),
                    TARGETUSAGE = table.Column<string>(name: "TARGET_USAGE", type: "character varying(100)", maxLength: 100, nullable: false),
                    TARGETDOSAGEFORM = table.Column<string>(name: "TARGET_DOSAGE_FORM", type: "character varying(10)", maxLength: 10, nullable: false),
                    PASTDATE = table.Column<string>(name: "PAST_DATE", type: "character varying(8)", maxLength: 8, nullable: false),
                    PASTPHARMACEUTICALCODETYPE = table.Column<string>(name: "PAST_PHARMACEUTICAL_CODE_TYPE", type: "character varying(1)", maxLength: 1, nullable: false),
                    PASTPHARMACEUTICALCODE = table.Column<string>(name: "PAST_PHARMACEUTICAL_CODE", type: "character varying(13)", maxLength: 13, nullable: false),
                    PASTPHARMACEUTICALNAME = table.Column<string>(name: "PAST_PHARMACEUTICAL_NAME", type: "character varying(80)", maxLength: 80, nullable: false),
                    PASTMEDICALINSTITUTIONNAME = table.Column<string>(name: "PAST_MEDICAL_INSTITUTION_NAME", type: "character varying(120)", maxLength: 120, nullable: false),
                    PASTINSURANCEPHARMACYNAME = table.Column<string>(name: "PAST_INSURANCE_PHARMACY_NAME", type: "character varying(120)", maxLength: 120, nullable: false),
                    PASTDISPENSINGQUANTITY = table.Column<string>(name: "PAST_DISPENSING_QUANTITY", type: "character varying(3)", maxLength: 3, nullable: false),
                    PASTUSAGE = table.Column<string>(name: "PAST_USAGE", type: "character varying(100)", maxLength: 100, nullable: false),
                    PASTDOSAGEFORM = table.Column<string>(name: "PAST_DOSAGE_FORM", type: "character varying(10)", maxLength: 10, nullable: false),
                    COMMENT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPS_CHK_DETAIL", x => new { x.HPID, x.PTID, x.RAIINNO, x.SEQNO, x.MESSAGEID });
                });

            migrationBuilder.CreateTable(
                name: "EPS_PRESCRIPTION",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    REFILECOUNT = table.Column<int>(name: "REFILE_COUNT", type: "integer", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    HOKENSYANO = table.Column<string>(name: "HOKENSYA_NO", type: "character varying(8)", maxLength: 8, nullable: false),
                    KIGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    BANGO = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    EDANO = table.Column<string>(name: "EDA_NO", type: "character varying(2)", maxLength: 2, nullable: false),
                    KOHIFUTANSYANO = table.Column<string>(name: "KOHI_FUTANSYA_NO", type: "character varying(8)", maxLength: 8, nullable: false),
                    KOHIJYUKYUSYANO = table.Column<string>(name: "KOHI_JYUKYUSYA_NO", type: "character varying(7)", maxLength: 7, nullable: false),
                    PRESCRIPTIONID = table.Column<string>(name: "PRESCRIPTION_ID", type: "character varying(36)", maxLength: 36, nullable: false),
                    ACCESSCODE = table.Column<string>(name: "ACCESS_CODE", type: "character varying(16)", maxLength: 16, nullable: false),
                    ISSUETYPE = table.Column<int>(name: "ISSUE_TYPE", type: "integer", nullable: false),
                    PRESCRIPTIONDOCUMENT = table.Column<string>(name: "PRESCRIPTION_DOCUMENT", type: "text", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false),
                    DELETEDREASON = table.Column<int>(name: "DELETED_REASON", type: "integer", nullable: false),
                    DELETEDDATE = table.Column<DateTime>(name: "DELETED_DATE", type: "timestamp with time zone", nullable: true),
                    KAID = table.Column<int>(name: "KA_ID", type: "integer", nullable: false),
                    TANTOID = table.Column<int>(name: "TANTO_ID", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPS_PRESCRIPTION", x => new { x.HPID, x.PTID, x.RAIINNO, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "EPS_REFERENCE",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    PTID = table.Column<long>(name: "PT_ID", type: "bigint", nullable: false),
                    PRESCRIPTIONID = table.Column<string>(name: "PRESCRIPTION_ID", type: "character varying(36)", maxLength: 36, nullable: false),
                    RAIINNO = table.Column<long>(name: "RAIIN_NO", type: "bigint", nullable: false),
                    SINDATE = table.Column<int>(name: "SIN_DATE", type: "integer", nullable: false),
                    PRESCRIPTIONREFERENCEINFORMATION = table.Column<string>(name: "PRESCRIPTION_REFERENCE_INFORMATION", type: "text", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPS_REFERENCE", x => new { x.HPID, x.PTID, x.PRESCRIPTIONID });
                });

            migrationBuilder.CreateTable(
                name: "YOHO_HOSOKU",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    ITEMCD = table.Column<string>(name: "ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    SEQNO = table.Column<long>(name: "SEQ_NO", type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SORTNO = table.Column<int>(name: "SORT_NO", type: "integer", nullable: false),
                    YOHOHOSOKUKBN = table.Column<int>(name: "YOHO_HOSOKU_KBN", type: "integer", nullable: false),
                    YOHOHOSOKUREC = table.Column<int>(name: "YOHO_HOSOKU_REC", type: "integer", nullable: false),
                    HOSOKUITEMCD = table.Column<string>(name: "HOSOKU_ITEM_CD", type: "character varying(10)", maxLength: 10, nullable: false),
                    HOSOKU = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOHO_HOSOKU", x => new { x.HPID, x.ITEMCD, x.STARTDATE, x.SEQNO });
                });

            migrationBuilder.CreateTable(
                name: "YOHO_MST",
                columns: table => new
                {
                    HPID = table.Column<int>(name: "HP_ID", type: "integer", nullable: false),
                    YOHOCD = table.Column<string>(name: "YOHO_CD", type: "character varying(16)", maxLength: 16, nullable: false),
                    STARTDATE = table.Column<int>(name: "START_DATE", type: "integer", nullable: false),
                    YOHOKBNCD = table.Column<string>(name: "YOHO_KBN_CD", type: "character varying(1)", maxLength: 1, nullable: false),
                    YOHOKBN = table.Column<string>(name: "YOHO_KBN", type: "character varying(2)", maxLength: 2, nullable: false),
                    YOHODETAILKBNCD = table.Column<string>(name: "YOHO_DETAIL_KBN_CD", type: "character varying(1)", maxLength: 1, nullable: false),
                    YOHODETAILKBN = table.Column<string>(name: "YOHO_DETAIL_KBN", type: "character varying(15)", maxLength: 15, nullable: false),
                    TIMINGKBNCD = table.Column<int>(name: "TIMING_KBN_CD", type: "integer", nullable: false),
                    TIMINGKBN = table.Column<string>(name: "TIMING_KBN", type: "character varying(60)", maxLength: 60, nullable: false),
                    YOHONAME = table.Column<string>(name: "YOHO_NAME", type: "character varying(50)", maxLength: 50, nullable: false),
                    REFERENCENO = table.Column<int>(name: "REFERENCE_NO", type: "integer", nullable: false),
                    ENDDATE = table.Column<int>(name: "END_DATE", type: "integer", nullable: false),
                    YOHOCDKBN = table.Column<int>(name: "YOHO_CD_KBN", type: "integer", nullable: false),
                    TONYOJOKEN = table.Column<int>(name: "TONYO_JOKEN", type: "integer", nullable: false),
                    TOYOTIMING = table.Column<int>(name: "TOYO_TIMING", type: "integer", nullable: false),
                    TOYOTIME = table.Column<int>(name: "TOYO_TIME", type: "integer", nullable: false),
                    TOYOINTERVAL = table.Column<int>(name: "TOYO_INTERVAL", type: "integer", nullable: false),
                    BUI = table.Column<int>(type: "integer", nullable: false),
                    YOHOKANANAME = table.Column<string>(name: "YOHO_KANA_NAME", type: "character varying(120)", maxLength: 120, nullable: false),
                    CHOZAIYOHOCD = table.Column<int>(name: "CHOZAI_YOHO_CD", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    CREATEID = table.Column<int>(name: "CREATE_ID", type: "integer", nullable: false),
                    CREATEMACHINE = table.Column<string>(name: "CREATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEID = table.Column<int>(name: "UPDATE_ID", type: "integer", nullable: false),
                    UPDATEMACHINE = table.Column<string>(name: "UPDATE_MACHINE", type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YOHO_MST", x => new { x.HPID, x.YOHOCD, x.STARTDATE });
                });

            migrationBuilder.CreateIndex(
                name: "EPS_PRESCRIPTION_IDX01",
                table: "EPS_PRESCRIPTION",
                columns: new[] { "HP_ID", "PRESCRIPTION_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EPS_CHK");

            migrationBuilder.DropTable(
                name: "EPS_CHK_DETAIL");

            migrationBuilder.DropTable(
                name: "EPS_PRESCRIPTION");

            migrationBuilder.DropTable(
                name: "EPS_REFERENCE");

            migrationBuilder.DropTable(
                name: "YOHO_HOSOKU");

            migrationBuilder.DropTable(
                name: "YOHO_MST");

            migrationBuilder.DropColumn(
                name: "YOHO_CD",
                table: "TEN_MST");

            migrationBuilder.DropColumn(
                name: "YOHO_HOSOKU_KBN",
                table: "TEN_MST");

            migrationBuilder.DropColumn(
                name: "YOHO_HOSOKU_REC",
                table: "TEN_MST");
        }
    }
}
