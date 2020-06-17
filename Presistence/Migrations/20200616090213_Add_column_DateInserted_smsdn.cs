using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Presistence.Migrations
{
    public partial class Add_column_DateInserted_smsdn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "SmsdnHists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "SmsdnDs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 6, 16, 16, 2, 12, 620, DateTimeKind.Local).AddTicks(9002));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "SmsdnHists");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "SmsdnDs");

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 5, 14, 14, 34, 26, 56, DateTimeKind.Local).AddTicks(8394));
        }
    }
}
