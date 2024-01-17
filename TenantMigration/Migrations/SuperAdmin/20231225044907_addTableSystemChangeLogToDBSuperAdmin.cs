using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class addTableSystemChangeLogToDBSuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYSTEM_CHANGE_LOG");
        }
    }
}
