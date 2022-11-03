using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreDataContext.Migrations
{
    public partial class AddColumnHokenMst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                            name: "RECE_KISAI_KOKHO",
                            table: "HOKEN_MST",
                            nullable: false,
                            defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                            name: "KOGAKU_HAIRYO_KBN",
                            table: "HOKEN_MST",
                            nullable: false,
                            defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "RECE_KISAI_KOKHO",
                                        table: "HOKEN_MST");

            migrationBuilder.DropColumn(name: "KOGAKU_HAIRYO_KBN",
                                        table: "HOKEN_MST");
        }
    }
}
