using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyTableKensaInfDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KENSA_INF_DETAIL",
                table: "KENSA_INF_DETAIL");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KENSA_INF_DETAIL",
                table: "KENSA_INF_DETAIL",
                columns: new[] { "HP_ID", "SEQ_NO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KENSA_INF_DETAIL",
                table: "KENSA_INF_DETAIL");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KENSA_INF_DETAIL",
                table: "KENSA_INF_DETAIL",
                columns: new[] { "HP_ID", "PT_ID", "IRAI_CD", "SEQ_NO" });
        }
    }
}
