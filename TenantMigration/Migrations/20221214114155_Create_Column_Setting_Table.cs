using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class CreateColumnSettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COLUMN_SETTING",
                columns: table => new
                {
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    TABLENAME = table.Column<string>(name: "TABLE_NAME", type: "text", nullable: false),
                    COLUMNNAME = table.Column<string>(name: "COLUMN_NAME", type: "text", nullable: false),
                    DISPLAYORDER = table.Column<int>(name: "DISPLAY_ORDER", type: "integer", nullable: false),
                    ISPINNED = table.Column<bool>(name: "IS_PINNED", type: "boolean", nullable: false),
                    ISHIDDEN = table.Column<bool>(name: "IS_HIDDEN", type: "boolean", nullable: false),
                    WIDTH = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLUMN_SETTING", x => new { x.USERID, x.TABLENAME, x.COLUMNNAME });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COLUMN_SETTING");
        }
    }
}
