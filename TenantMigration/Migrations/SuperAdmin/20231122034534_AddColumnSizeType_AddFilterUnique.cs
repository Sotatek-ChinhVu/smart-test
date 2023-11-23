using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class AddColumnSizeTypeAddFilterUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_DB",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_END_POINT_DB",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_END_SUB_DOMAIN",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_HOSPITAL",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_SUB_DOMAIN",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_ADMIN_LOGIN_ID",
                table: "ADMIN");

            migrationBuilder.AddColumn<int>(
                name: "SIZE_TYPE",
                table: "TENANT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT",
                column: "ADMIN_ID",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_DB",
                table: "TENANT",
                column: "DB",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_END_POINT_DB",
                table: "TENANT",
                column: "END_POINT_DB",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_END_SUB_DOMAIN",
                table: "TENANT",
                column: "END_SUB_DOMAIN",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_HOSPITAL",
                table: "TENANT",
                column: "HOSPITAL",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_SUB_DOMAIN",
                table: "TENANT",
                column: "SUB_DOMAIN",
                unique: true,
                filter: "\"IS_DELETED\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_LOGIN_ID",
                table: "ADMIN",
                column: "LOGIN_ID",
                unique: true,
                filter: "\"IS_DELETED\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_DB",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_END_POINT_DB",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_END_SUB_DOMAIN",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_HOSPITAL",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_TENANT_SUB_DOMAIN",
                table: "TENANT");

            migrationBuilder.DropIndex(
                name: "IX_ADMIN_LOGIN_ID",
                table: "ADMIN");

            migrationBuilder.DropColumn(
                name: "SIZE_TYPE",
                table: "TENANT");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ADMIN_ID",
                table: "TENANT",
                column: "ADMIN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_DB",
                table: "TENANT",
                column: "DB",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_END_POINT_DB",
                table: "TENANT",
                column: "END_POINT_DB",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_END_SUB_DOMAIN",
                table: "TENANT",
                column: "END_SUB_DOMAIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_HOSPITAL",
                table: "TENANT",
                column: "HOSPITAL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_SUB_DOMAIN",
                table: "TENANT",
                column: "SUB_DOMAIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_LOGIN_ID",
                table: "ADMIN",
                column: "LOGIN_ID",
                unique: true);
        }
    }
}
