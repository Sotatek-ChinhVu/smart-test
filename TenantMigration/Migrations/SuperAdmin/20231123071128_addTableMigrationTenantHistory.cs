using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations.SuperAdmin
{
    /// <inheritdoc />
    public partial class addTableMigrationTenantHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MIGRATION_TENANT_HISTORY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TENANTID = table.Column<int>(name: "TENANT_ID", type: "integer", nullable: false),
                    MIGRATIONID = table.Column<string>(name: "MIGRATION_ID", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MIGRATION_TENANT_HISTORY", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MIGRATION_TENANT_HISTORY");
        }
    }
}
