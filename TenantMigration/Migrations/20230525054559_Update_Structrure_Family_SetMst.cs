using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStructrureFamilySetMst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PT_GRP_INF",
                table: "PT_GRP_INF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PT_FAMILY",
                table: "PT_FAMILY");

            migrationBuilder.AlterColumn<string>(
                name: "GRP_CODE",
                table: "PT_GRP_INF",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<long>(
                name: "PT_ID",
                table: "PT_FAMILY",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PT_GRP_INF",
                table: "PT_GRP_INF",
                columns: new[] { "HP_ID", "GRP_ID", "PT_ID", "SEQ_NO" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PT_FAMILY",
                table: "PT_FAMILY",
                column: "FAMILY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SET_MST_HP_ID_SET_KBN_SET_KBN_EDA_NO_GENERATION_ID",
                table: "SET_MST",
                columns: new[] { "HP_ID", "SET_CD", "SET_KBN", "SET_KBN_EDA_NO", "GENERATION_ID", "LEVEL1", "LEVEL2", "LEVEL3" },
                unique: true,
                filter: $"\"IS_DELETED\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SET_MST_HP_ID_SET_KBN_SET_KBN_EDA_NO_GENERATION_ID",
                table: "SET_MST");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PT_GRP_INF",
                table: "PT_GRP_INF");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PT_FAMILY",
                table: "PT_FAMILY");

            migrationBuilder.AlterColumn<string>(
                name: "GRP_CODE",
                table: "PT_GRP_INF",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PT_ID",
                table: "PT_FAMILY",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PT_GRP_INF",
                table: "PT_GRP_INF",
                columns: new[] { "HP_ID", "GRP_ID", "GRP_CODE", "SEQ_NO" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PT_FAMILY",
                table: "PT_FAMILY",
                column: "PT_ID");
        }
    }
}
