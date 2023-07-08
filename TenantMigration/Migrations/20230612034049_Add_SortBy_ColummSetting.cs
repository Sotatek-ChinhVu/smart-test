using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddSortByColummSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ORDER_BY",
                table: "COLUMN_SETTING",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ORDER_BY",
                table: "COLUMN_SETTING");
        }
    }
}
