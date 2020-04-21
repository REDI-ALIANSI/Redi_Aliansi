using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Presistence.Migrations
{
    public partial class Add_OperatorURICollums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlIn",
                table: "Operators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlOut",
                table: "Operators",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 3, 4, 11, 33, 52, 460, DateTimeKind.Local).AddTicks(5404));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlIn",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "UrlOut",
                table: "Operators");

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 2, 6, 12, 17, 18, 464, DateTimeKind.Local).AddTicks(178));
        }
    }
}
