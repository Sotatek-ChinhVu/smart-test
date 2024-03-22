using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeqZRAIINLISTTAG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_json_setting",
                table: "json_setting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_column_setting",
                table: "column_setting");

            migrationBuilder.AlterColumn<string>(
                name: "salt",
                table: "user_mst",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "login_id",
                table: "user_mst",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "hash_password",
                table: "user_mst",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_mst",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "user_mst",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "is_init_password",
                table: "user_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "miss_login_count",
                table: "user_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "user_mst",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "sin_date",
                table: "lock_inf",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "json_setting",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hp_id",
                table: "column_setting",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_json_setting",
                table: "json_setting",
                columns: new[] { "user_id", "key", "hp_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_column_setting",
                table: "column_setting",
                columns: new[] { "hp_id", "user_id", "table_name", "column_name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_json_setting",
                table: "json_setting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_column_setting",
                table: "column_setting");

            migrationBuilder.DropColumn(
                name: "email",
                table: "user_mst");

            migrationBuilder.DropColumn(
                name: "is_init_password",
                table: "user_mst");

            migrationBuilder.DropColumn(
                name: "miss_login_count",
                table: "user_mst");

            migrationBuilder.DropColumn(
                name: "status",
                table: "user_mst");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "json_setting");

            migrationBuilder.DropColumn(
                name: "hp_id",
                table: "column_setting");

            migrationBuilder.AlterColumn<byte[]>(
                name: "salt",
                table: "user_mst",
                type: "bytea",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "login_id",
                table: "user_mst",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<byte[]>(
                name: "hash_password",
                table: "user_mst",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "user_mst",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "sin_date",
                table: "lock_inf",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_json_setting",
                table: "json_setting",
                columns: new[] { "user_id", "key" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_column_setting",
                table: "column_setting",
                columns: new[] { "user_id", "table_name", "column_name" });
        }
    }
}
