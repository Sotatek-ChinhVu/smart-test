using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class CreateJsonSettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JSON_SETTING",
                columns: table => new
                {
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    KEY = table.Column<string>(type: "text", nullable: false),
                    VALUE = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JSON_SETTING", x => new { x.USERID, x.KEY });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JSON_SETTING");
        }
    }
}
