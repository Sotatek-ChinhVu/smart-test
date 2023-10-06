using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class createkeyRsvkrtMs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RSVKRT_MST_HP_ID_PT_ID_RSV_DATE",
                table: "RSVKRT_MST",
                columns: new[] { "HP_ID", "PT_ID", "RSV_DATE" },
                unique: true,
                filter: $"\"RSVKRT_KBN\" = 0 AND \"IS_DELETED\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RSVKRT_MST_HP_ID_PT_ID_RSV_DATE",
                table: "RSVKRT_MST");
        }
    }
}
