using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnSEQPARENTNOKensaInfDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SEQ_PARENT_NO",
                table: "Z_KENSA_INF_DETAIL",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SEQ_PARENT_NO",
                table: "KENSA_INF_DETAIL",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SEQ_PARENT_NO",
                table: "Z_KENSA_INF_DETAIL");

            migrationBuilder.DropColumn(
                name: "SEQ_PARENT_NO",
                table: "KENSA_INF_DETAIL");
        }
    }
}
