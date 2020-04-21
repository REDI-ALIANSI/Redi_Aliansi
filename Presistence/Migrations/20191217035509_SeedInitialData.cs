using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Presistence.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContentTypes",
                columns: new[] { "ContentTypeId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "PLAIN CONTENT", "TEXT" },
                    { 2, "RICH PICTURE CONTENT", "PICTURE" },
                    { 3, "RICH VIDEO CONTENT", "VIDEO" }
                });

            migrationBuilder.InsertData(
                table: "Operators",
                columns: new[] { "OperatorId", "OperatorName", "RetryLimit" },
                values: new object[,]
                {
                    { 51010, "TELKOMSEL", 0 },
                    { 51011, "EXCELCOM", 0 },
                    { 51021, "INDOSAT", 0 }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "Type" },
                values: new object[,]
                {
                    { 1, "Entertaiment" },
                    { 2, "Sport" },
                    { 3, "Games" },
                    { 4, "Religion" }
                });

            migrationBuilder.InsertData(
                table: "ShortCodes",
                columns: new[] { "Shortcode", "Created", "CreatedBy", "Description", "LastModified", "LastModifiedBy" },
                values: new object[] { 93450, new DateTime(2019, 12, 17, 10, 55, 8, 508, DateTimeKind.Local).AddTicks(8787), "InitialMigration", "HM Short Code", null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "ContentTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "ContentTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ContentTypes",
                keyColumn: "ContentTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Operators",
                keyColumn: "OperatorId",
                keyValue: 51010);

            migrationBuilder.DeleteData(
                table: "Operators",
                keyColumn: "OperatorId",
                keyValue: 51011);

            migrationBuilder.DeleteData(
                table: "Operators",
                keyColumn: "OperatorId",
                keyValue: 51021);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450);
        }
    }
}
