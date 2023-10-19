using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnSEQPARENTNOKensaSetDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SEQ_PARENT_NO",
                table: "KENSA_SET_DETAIL",
                type: "integer",
                maxLength: 9,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SEQ_PARENT_NO",
                table: "KENSA_SET_DETAIL");
        }
    }
}
