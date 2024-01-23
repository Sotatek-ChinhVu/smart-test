using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class _20240122UpdatePtNumToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "pt_num",
                table: "pt_inf",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "pt_num",
                table: "pt_inf",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
