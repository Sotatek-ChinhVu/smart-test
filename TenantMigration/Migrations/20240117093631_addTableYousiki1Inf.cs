using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class addTableYousiki1Inf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "yousiki1_inf",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    datatype = table.Column<int>(name: "data_type", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_yousiki1_inf", x => new { x.hpid, x.ptid, x.sinym, x.datatype, x.seqno });
                });

            migrationBuilder.CreateTable(
                name: "yousiki1_inf_detail",
                columns: table => new
                {
                    hpid = table.Column<int>(name: "hp_id", type: "integer", nullable: false),
                    ptid = table.Column<long>(name: "pt_id", type: "bigint", nullable: false),
                    sinym = table.Column<int>(name: "sin_ym", type: "integer", nullable: false),
                    datatype = table.Column<int>(name: "data_type", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    codeno = table.Column<string>(name: "code_no", type: "character varying(10)", maxLength: 10, nullable: false),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    payload = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yousiki1_inf_detail", x => new { x.hpid, x.ptid, x.sinym, x.datatype, x.seqno, x.codeno, x.rowno, x.payload });
                });

            migrationBuilder.CreateTable(
                name: "z_yousiki1_inf",
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
                    datatype = table.Column<int>(name: "data_type", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    isdeleted = table.Column<int>(name: "is_deleted", type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_z_yousiki1_inf", x => x.opid);
                });

            migrationBuilder.CreateTable(
                name: "z_yousiki1_inf_detail",
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
                    datatype = table.Column<int>(name: "data_type", type: "integer", nullable: false),
                    seqno = table.Column<int>(name: "seq_no", type: "integer", nullable: false),
                    codeno = table.Column<string>(name: "code_no", type: "character varying(10)", maxLength: 10, nullable: true),
                    rowno = table.Column<int>(name: "row_no", type: "integer", nullable: false),
                    payload = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_z_yousiki1_inf_detail", x => x.opid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "yousiki1_inf");

            migrationBuilder.DropTable(
                name: "yousiki1_inf_detail");

            migrationBuilder.DropTable(
                name: "z_yousiki1_inf");

            migrationBuilder.DropTable(
                name: "z_yousiki1_inf_detail");
        }
    }
}
