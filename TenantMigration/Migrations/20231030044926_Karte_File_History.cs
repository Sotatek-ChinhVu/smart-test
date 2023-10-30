using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantMigration.Migrations
{
    /// <inheritdoc />
    public partial class KarteFileHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CREATE_DATE",
                table: "KARTE_IMG_INF",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CREATE_ID",
                table: "KARTE_IMG_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATE_DATE",
                table: "KARTE_IMG_INF",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UPDATE_ID",
                table: "KARTE_IMG_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IS_DELETED",
                table: "CONVERSION_ITEM_INF",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CREATE_DATE",
                table: "KARTE_IMG_INF");

            migrationBuilder.DropColumn(
                name: "CREATE_ID",
                table: "KARTE_IMG_INF");

            migrationBuilder.DropColumn(
                name: "UPDATE_DATE",
                table: "KARTE_IMG_INF");

            migrationBuilder.DropColumn(
                name: "UPDATE_ID",
                table: "KARTE_IMG_INF");

            migrationBuilder.DropColumn(
                name: "IS_DELETED",
                table: "CONVERSION_ITEM_INF");
        }
    }
}
