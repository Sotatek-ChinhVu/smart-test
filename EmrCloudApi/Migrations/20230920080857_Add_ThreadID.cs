using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmrCloudApi.Migrations
{
    /// <inheritdoc />
    public partial class AddThreadID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThreadId",
                table: "AuditLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "AuditLogs");
        }
    }
}
