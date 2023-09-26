using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmrCloudApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestInfoClientIP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Machine",
                table: "AuditLogs");

            migrationBuilder.AddColumn<string>(
                name: "ClientIP",
                table: "AuditLogs",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestInfo",
                table: "AuditLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientIP",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "RequestInfo",
                table: "AuditLogs");

            migrationBuilder.AddColumn<string>(
                name: "Machine",
                table: "AuditLogs",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);
        }
    }
}
