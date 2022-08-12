using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreDataContext.Migrations
{
    public partial class AddColumnSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COLUMN_SETTING");
        }
    }
}
