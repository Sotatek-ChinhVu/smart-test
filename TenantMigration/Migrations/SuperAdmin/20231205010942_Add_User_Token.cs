using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class AddUserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER_TOKEN",
                columns: table => new
                {
                    USERID = table.Column<int>(name: "USER_ID", type: "integer", nullable: false),
                    REFRESHTOKEN = table.Column<string>(name: "REFRESH_TOKEN", type: "text", nullable: false),
                    TOKENEXPIRYTIME = table.Column<DateTime>(name: "TOKEN_EXPIRY_TIME", type: "timestamp with time zone", nullable: false),
                    REFRESHTOKENISUSED = table.Column<bool>(name: "REFRESH_TOKEN_IS_USED", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_TOKEN", x => new { x.USERID, x.REFRESHTOKEN });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER_TOKEN");
        }
    }
}
