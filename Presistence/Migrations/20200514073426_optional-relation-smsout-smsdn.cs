using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Presistence.Migrations
{
    public partial class optionalrelationsmsoutsmsdn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutDs_SmsdnDs_MtTxId",
                table: "SmsoutDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutHists_SmsdnHists_MtTxId",
                table: "SmsoutHists");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmsdnHists_MtTxId",
                table: "SmsdnHists");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmsdnDs_MtTxId",
                table: "SmsdnDs");

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsdnHists",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsdnDs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 5, 14, 14, 34, 26, 56, DateTimeKind.Local).AddTicks(8394));

            migrationBuilder.CreateIndex(
                name: "IX_SmsdnHists_MtTxId",
                table: "SmsdnHists",
                column: "MtTxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmsdnDs_MtTxId",
                table: "SmsdnDs",
                column: "MtTxId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsdnDs_SmsoutDs_MtTxId",
                table: "SmsdnDs",
                column: "MtTxId",
                principalTable: "SmsoutDs",
                principalColumn: "MtTxId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsdnHists_SmsoutHists_MtTxId",
                table: "SmsdnHists",
                column: "MtTxId",
                principalTable: "SmsoutHists",
                principalColumn: "MtTxId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmsdnDs_SmsoutDs_MtTxId",
                table: "SmsdnDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsdnHists_SmsoutHists_MtTxId",
                table: "SmsdnHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsdnHists_MtTxId",
                table: "SmsdnHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsdnDs_MtTxId",
                table: "SmsdnDs");

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsdnHists",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsdnDs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmsdnHists_MtTxId",
                table: "SmsdnHists",
                column: "MtTxId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmsdnDs_MtTxId",
                table: "SmsdnDs",
                column: "MtTxId");

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 5, 13, 14, 20, 12, 59, DateTimeKind.Local).AddTicks(5754));

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutDs_SmsdnDs_MtTxId",
                table: "SmsoutDs",
                column: "MtTxId",
                principalTable: "SmsdnDs",
                principalColumn: "MtTxId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutHists_SmsdnHists_MtTxId",
                table: "SmsoutHists",
                column: "MtTxId",
                principalTable: "SmsdnHists",
                principalColumn: "MtTxId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
