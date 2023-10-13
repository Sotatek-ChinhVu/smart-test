using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class ReStructureRaiinListInf2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropIndex(
                name: "IX_RAIIN_LIST_INF_HP_ID_PT_ID_SIN_DATE_RAIIN_NO_GRP_ID_RAIIN_L~",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "RAIIN_LIST_INF");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "RAIIN_NO", "GRP_ID", "RAIIN_LIST_KBN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF");

            migrationBuilder.AddColumn<long>(
                name: "ID",
                table: "RAIIN_LIST_INF",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_RAIIN_LIST_INF_HP_ID_PT_ID_SIN_DATE_RAIIN_NO_GRP_ID_RAIIN_L~",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "RAIIN_NO", "GRP_ID", "RAIIN_LIST_KBN" },
                unique: true);
        }
    }
}
