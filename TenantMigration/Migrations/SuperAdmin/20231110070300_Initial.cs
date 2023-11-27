using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADMIN",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FULLNAME = table.Column<string>(name: "FULL_NAME", type: "character varying(100)", maxLength: 100, nullable: true),
                    ROLE = table.Column<int>(type: "integer", nullable: false),
                    LOGINID = table.Column<int>(name: "LOGIN_ID", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NOTIFICATION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STATUS = table.Column<byte>(type: "smallint", nullable: false),
                    MESSAGE = table.Column<string>(type: "text", nullable: true),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SCRIPTION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TENANTID = table.Column<int>(name: "TENANT_ID", type: "integer", nullable: false),
                    HOSPITAL = table.Column<string>(type: "text", nullable: true),
                    ScriptString = table.Column<string>(type: "text", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCRIPTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TENANT",
                columns: table => new
                {
                    TENANTID = table.Column<int>(name: "TENANT_ID", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HOSPITAL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    STATUS = table.Column<byte>(type: "smallint", nullable: false),
                    ADMINID = table.Column<int>(name: "ADMIN_ID", type: "integer", nullable: false),
                    PASSWORD = table.Column<string>(type: "text", nullable: false),
                    SUBDOMAIN = table.Column<string>(name: "SUB_DOMAIN", type: "text", nullable: false),
                    DB = table.Column<string>(type: "text", nullable: false),
                    SIZE = table.Column<int>(type: "integer", nullable: false),
                    TYPE = table.Column<byte>(type: "smallint", nullable: false),
                    ENDPOINTDB = table.Column<string>(name: "END_POINT_DB", type: "text", nullable: false),
                    ENDSUBDOMAIN = table.Column<string>(name: "END_SUB_DOMAIN", type: "text", nullable: false),
                    ACTION = table.Column<int>(type: "integer", nullable: false),
                    SCHEDULEDATE = table.Column<int>(name: "SCHEDULE_DATE", type: "integer", nullable: false),
                    SCHEDULETIME = table.Column<int>(name: "SCHEDULE_TIME", type: "integer", nullable: false),
                    ISDELETED = table.Column<int>(name: "IS_DELETED", type: "integer", nullable: false),
                    CREATEDATE = table.Column<DateTime>(name: "CREATE_DATE", type: "timestamp with time zone", nullable: false),
                    UPDATEDATE = table.Column<DateTime>(name: "UPDATE_DATE", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TENANT", x => x.TENANTID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_LOGIN_ID",
                table: "ADMIN",
                column: "LOGIN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT",
                column: "ADMIN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_DB",
                table: "TENANT",
                column: "DB",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_END_POINT_DB",
                table: "TENANT",
                column: "END_POINT_DB",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_END_SUB_DOMAIN",
                table: "TENANT",
                column: "END_SUB_DOMAIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_HOSPITAL",
                table: "TENANT",
                column: "HOSPITAL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_SUB_DOMAIN",
                table: "TENANT",
                column: "SUB_DOMAIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMIN");

            migrationBuilder.DropTable(
                name: "NOTIFICATION");

            migrationBuilder.DropTable(
                name: "SCRIPTION");

            migrationBuilder.DropTable(
                name: "TENANT");
        }
    }
}
