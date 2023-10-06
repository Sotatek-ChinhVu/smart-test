using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class online : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CONFIRMATION_TYPE",
                table: "Z_RAIIN_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "INFO_CONS_FLG",
                table: "Z_RAIIN_INF",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PRESCRIPTION_ISSUE_TYPE",
                table: "Z_RAIIN_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CONFIRMATION_TYPE",
                table: "RAIIN_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "INFO_CONS_FLG",
                table: "RAIIN_INF",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PRESCRIPTION_ISSUE_TYPE",
                table: "RAIIN_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "INFO_CONS_FLG",
                table: "ONLINE_CONFIRMATION_HISTORY",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PRESCRIPTION_ISSUE_TYPE",
                table: "ONLINE_CONFIRMATION_HISTORY",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UKETUKE_STATUS",
                table: "ONLINE_CONFIRMATION_HISTORY",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATE_DATE",
                table: "ONLINE_CONFIRMATION_HISTORY",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UPDATE_ID",
                table: "ONLINE_CONFIRMATION_HISTORY",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UPDATE_MACHINE",
                table: "ONLINE_CONFIRMATION_HISTORY",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LOGINKEY",
                table: "LOCK_INF",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CONFIRMATION_TYPE",
                table: "Z_RAIIN_INF");

            migrationBuilder.DropColumn(
                name: "INFO_CONS_FLG",
                table: "Z_RAIIN_INF");

            migrationBuilder.DropColumn(
                name: "PRESCRIPTION_ISSUE_TYPE",
                table: "Z_RAIIN_INF");

            migrationBuilder.DropColumn(
                name: "CONFIRMATION_TYPE",
                table: "RAIIN_INF");

            migrationBuilder.DropColumn(
                name: "INFO_CONS_FLG",
                table: "RAIIN_INF");

            migrationBuilder.DropColumn(
                name: "PRESCRIPTION_ISSUE_TYPE",
                table: "RAIIN_INF");

            migrationBuilder.DropColumn(
                name: "INFO_CONS_FLG",
                table: "ONLINE_CONFIRMATION_HISTORY");
            migrationBuilder.DropColumn(
                name: "PRESCRIPTION_ISSUE_TYPE",
                table: "ONLINE_CONFIRMATION_HISTORY");

            migrationBuilder.DropColumn(
                name: "UKETUKE_STATUS",
                table: "ONLINE_CONFIRMATION_HISTORY");

            migrationBuilder.DropColumn(
                name: "UPDATE_DATE",
                table: "ONLINE_CONFIRMATION_HISTORY");

            migrationBuilder.DropColumn(
                name: "UPDATE_ID",
                table: "ONLINE_CONFIRMATION_HISTORY");

            migrationBuilder.DropColumn(
                name: "UPDATE_MACHINE",
                table: "ONLINE_CONFIRMATION_HISTORY");

            migrationBuilder.DropColumn(
                name: "LOGINKEY",
                table: "LOCK_INF");
        }
    }
}
