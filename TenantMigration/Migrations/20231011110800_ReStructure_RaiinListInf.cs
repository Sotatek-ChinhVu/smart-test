using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class ReStructureRaiinListInf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF");

            migrationBuilder.AlterColumn<int>(
                name: "GRP_ID",
                table: "RAIIN_LIST_INF",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<long>(
                name: "RAIIN_NO",
                table: "RAIIN_LIST_INF",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "SIN_DATE",
                table: "RAIIN_LIST_INF",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<long>(
                name: "PT_ID",
                table: "RAIIN_LIST_INF",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "RAIIN_LIST_INF",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

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

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropIndex(
                name: "IX_RAIIN_LIST_INF_HP_ID_PT_ID_SIN_DATE_RAIIN_NO_GRP_ID_RAIIN_L~",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "RAIIN_LIST_INF");

            migrationBuilder.AlterColumn<int>(
                name: "SIN_DATE",
                table: "RAIIN_LIST_INF",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<long>(
                name: "RAIIN_NO",
                table: "RAIIN_LIST_INF",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<long>(
                name: "PT_ID",
                table: "RAIIN_LIST_INF",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "HP_ID",
                table: "RAIIN_LIST_INF",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "GRP_ID",
                table: "RAIIN_LIST_INF",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAIIN_LIST_INF",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID", "SIN_DATE", "RAIIN_NO", "GRP_ID", "RAIIN_LIST_KBN" });

            migrationBuilder.CreateIndex(
                name: "RAIIN_LIST_INF_IDX02",
                table: "RAIIN_LIST_INF",
                columns: new[] { "HP_ID", "PT_ID", "RAIIN_NO" });
        }
    }
}
