using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddSeqGroupNoToTableKensaInfDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KENSA_ITEM_CD",
                table: "KENSA_SET_DETAIL",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "SET_NAME",
                table: "KENSA_SET",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<long>(
                name: "SEQ_GROUP_NO",
                table: "KENSA_INF_DETAIL",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SEQ_GROUP_NO",
                table: "KENSA_INF_DETAIL");

            migrationBuilder.AlterColumn<string>(
                name: "KENSA_ITEM_CD",
                table: "KENSA_SET_DETAIL",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SET_NAME",
                table: "KENSA_SET",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
