using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Presistence.Migrations
{
    public partial class UpdateEntitiysmsoutdn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmsinDs_ShortCodes_Shortcode1",
                table: "SmsinDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsinHists_ShortCodes_Shortcode1",
                table: "SmsinHists");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutDs_ShortCodes_Shortcode1",
                table: "SmsoutDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutDs_SmsdnDs_SmsdnDMtTxId",
                table: "SmsoutDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutHists_ShortCodes_Shortcode1",
                table: "SmsoutHists");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutHists_SmsdnHists_SmsdnHistMtTxId",
                table: "SmsoutHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsoutHists_Shortcode1",
                table: "SmsoutHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsoutHists_SmsdnHistMtTxId",
                table: "SmsoutHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsoutDs_Shortcode1",
                table: "SmsoutDs");

            migrationBuilder.DropIndex(
                name: "IX_SmsoutDs_SmsdnDMtTxId",
                table: "SmsoutDs");

            migrationBuilder.DropIndex(
                name: "IX_SmsinHists_Shortcode1",
                table: "SmsinHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsinDs_Shortcode1",
                table: "SmsinDs");

            migrationBuilder.DropColumn(
                name: "Shortcode",
                table: "SmsoutHists");

            migrationBuilder.DropColumn(
                name: "Shortcode1",
                table: "SmsoutHists");

            migrationBuilder.DropColumn(
                name: "SmsdnHistMtTxId",
                table: "SmsoutHists");

            migrationBuilder.DropColumn(
                name: "Shortcode",
                table: "SmsoutDs");

            migrationBuilder.DropColumn(
                name: "Shortcode1",
                table: "SmsoutDs");

            migrationBuilder.DropColumn(
                name: "SmsdnDMtTxId",
                table: "SmsoutDs");

            migrationBuilder.DropColumn(
                name: "Shortcode",
                table: "SmsinHists");

            migrationBuilder.DropColumn(
                name: "Shortcode1",
                table: "SmsinHists");

            migrationBuilder.DropColumn(
                name: "Shortcode",
                table: "SmsinDs");

            migrationBuilder.DropColumn(
                name: "Shortcode1",
                table: "SmsinDs");

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsoutHists",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsoutDs",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmsoutHists_MtTxId",
                table: "SmsoutHists",
                column: "MtTxId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmsoutDs_MtTxId",
                table: "SmsoutDs",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutDs_SmsdnDs_MtTxId",
                table: "SmsoutDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutHists_SmsdnHists_MtTxId",
                table: "SmsoutHists");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmsoutHists_MtTxId",
                table: "SmsoutHists");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmsoutDs_MtTxId",
                table: "SmsoutDs");

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsoutHists",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode",
                table: "SmsoutHists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode1",
                table: "SmsoutHists",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsdnHistMtTxId",
                table: "SmsoutHists",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MtTxId",
                table: "SmsoutDs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode",
                table: "SmsoutDs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode1",
                table: "SmsoutDs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsdnDMtTxId",
                table: "SmsoutDs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode",
                table: "SmsinHists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode1",
                table: "SmsinHists",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode",
                table: "SmsinDs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shortcode1",
                table: "SmsinDs",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 3, 4, 11, 33, 52, 460, DateTimeKind.Local).AddTicks(5404));

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutHists_Shortcode1",
                table: "SmsoutHists",
                column: "Shortcode1");

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutHists_SmsdnHistMtTxId",
                table: "SmsoutHists",
                column: "SmsdnHistMtTxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutDs_Shortcode1",
                table: "SmsoutDs",
                column: "Shortcode1");

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutDs_SmsdnDMtTxId",
                table: "SmsoutDs",
                column: "SmsdnDMtTxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmsinHists_Shortcode1",
                table: "SmsinHists",
                column: "Shortcode1");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinDs_Shortcode1",
                table: "SmsinDs",
                column: "Shortcode1");

            migrationBuilder.AddForeignKey(
                name: "FK_SmsinDs_ShortCodes_Shortcode1",
                table: "SmsinDs",
                column: "Shortcode1",
                principalTable: "ShortCodes",
                principalColumn: "Shortcode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsinHists_ShortCodes_Shortcode1",
                table: "SmsinHists",
                column: "Shortcode1",
                principalTable: "ShortCodes",
                principalColumn: "Shortcode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutDs_ShortCodes_Shortcode1",
                table: "SmsoutDs",
                column: "Shortcode1",
                principalTable: "ShortCodes",
                principalColumn: "Shortcode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutDs_SmsdnDs_SmsdnDMtTxId",
                table: "SmsoutDs",
                column: "SmsdnDMtTxId",
                principalTable: "SmsdnDs",
                principalColumn: "MtTxId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutHists_ShortCodes_Shortcode1",
                table: "SmsoutHists",
                column: "Shortcode1",
                principalTable: "ShortCodes",
                principalColumn: "Shortcode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutHists_SmsdnHists_SmsdnHistMtTxId",
                table: "SmsoutHists",
                column: "SmsdnHistMtTxId",
                principalTable: "SmsdnHists",
                principalColumn: "MtTxId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
