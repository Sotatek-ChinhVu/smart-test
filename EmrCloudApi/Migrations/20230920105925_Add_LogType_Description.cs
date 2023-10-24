using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmrCloudApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLogTypeDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Desciption",
                table: "AuditLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LogType",
                table: "AuditLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desciption",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "LogType",
                table: "AuditLogs");
        }
    }
}
