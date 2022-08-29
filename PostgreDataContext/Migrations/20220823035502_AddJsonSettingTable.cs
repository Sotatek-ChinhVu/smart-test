using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreDataContext.Migrations
{
    public partial class AddJsonSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JSON_SETTING",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    KEY = table.Column<string>(type: "text", nullable: false),
                    VALUE = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JSON_SETTING", x => new { x.USER_ID, x.KEY });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JSON_SETTING");
        }
    }
}
