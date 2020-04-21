using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Presistence.Migrations
{
    public partial class AddServiceIdSmsout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keywords_Service_ServiceId",
                table: "Keywords");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Service_ServiceId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceTypes_ServiceTypeId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ShortCodes_Shortcode",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCampaigns_Service_ServiceId",
                table: "ServiceCampaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRenewalConfiguration_Messages_MessageId",
                table: "ServiceRenewalConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRenewalConfiguration_Operators_OperatorId",
                table: "ServiceRenewalConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRenewalConfiguration_Service_ServiceId",
                table: "ServiceRenewalConfiguration");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsinDs_Service_ServiceId",
                table: "SmsinDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsinHists_Service_ServiceId",
                table: "SmsinHists");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionHists_Service_ServiceId",
                table: "SubscriptionHists");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Service_ServiceId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceRenewalConfiguration",
                table: "ServiceRenewalConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "ServiceRenewalConfiguration",
                newName: "ServiceRenewalConfigurations");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRenewalConfiguration_ServiceId",
                table: "ServiceRenewalConfigurations",
                newName: "IX_ServiceRenewalConfigurations_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRenewalConfiguration_OperatorId",
                table: "ServiceRenewalConfigurations",
                newName: "IX_ServiceRenewalConfigurations_OperatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRenewalConfiguration_MessageId",
                table: "ServiceRenewalConfigurations",
                newName: "IX_ServiceRenewalConfigurations_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Service_Shortcode",
                table: "Services",
                newName: "IX_Services_Shortcode");

            migrationBuilder.RenameIndex(
                name: "IX_Service_ServiceTypeId",
                table: "Services",
                newName: "IX_Services_ServiceTypeId");

            migrationBuilder.AddColumn<int>(
                name: "ServiceID",
                table: "SmsoutHists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "SmsoutDs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceRenewalConfigurations",
                table: "ServiceRenewalConfigurations",
                column: "ServiceRenewalConfigurationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "ServiceId");

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2020, 2, 6, 12, 17, 18, 464, DateTimeKind.Local).AddTicks(178));

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutHists_ServiceID",
                table: "SmsoutHists",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutDs_ServiceId",
                table: "SmsoutDs",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Keywords_Services_ServiceId",
                table: "Keywords",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Services_ServiceId",
                table: "Messages",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCampaigns_Services_ServiceId",
                table: "ServiceCampaigns",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRenewalConfigurations_Messages_MessageId",
                table: "ServiceRenewalConfigurations",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRenewalConfigurations_Operators_OperatorId",
                table: "ServiceRenewalConfigurations",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "OperatorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRenewalConfigurations_Services_ServiceId",
                table: "ServiceRenewalConfigurations",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "ServiceTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ShortCodes_Shortcode",
                table: "Services",
                column: "Shortcode",
                principalTable: "ShortCodes",
                principalColumn: "Shortcode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsinDs_Services_ServiceId",
                table: "SmsinDs",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsinHists_Services_ServiceId",
                table: "SmsinHists",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutDs_Services_ServiceId",
                table: "SmsoutDs",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsoutHists_Services_ServiceID",
                table: "SmsoutHists",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionHists_Services_ServiceId",
                table: "SubscriptionHists",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Services_ServiceId",
                table: "Subscriptions",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keywords_Services_ServiceId",
                table: "Keywords");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Services_ServiceId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCampaigns_Services_ServiceId",
                table: "ServiceCampaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRenewalConfigurations_Messages_MessageId",
                table: "ServiceRenewalConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRenewalConfigurations_Operators_OperatorId",
                table: "ServiceRenewalConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRenewalConfigurations_Services_ServiceId",
                table: "ServiceRenewalConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ShortCodes_Shortcode",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsinDs_Services_ServiceId",
                table: "SmsinDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsinHists_Services_ServiceId",
                table: "SmsinHists");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutDs_Services_ServiceId",
                table: "SmsoutDs");

            migrationBuilder.DropForeignKey(
                name: "FK_SmsoutHists_Services_ServiceID",
                table: "SmsoutHists");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionHists_Services_ServiceId",
                table: "SubscriptionHists");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Services_ServiceId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SmsoutHists_ServiceID",
                table: "SmsoutHists");

            migrationBuilder.DropIndex(
                name: "IX_SmsoutDs_ServiceId",
                table: "SmsoutDs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceRenewalConfigurations",
                table: "ServiceRenewalConfigurations");

            migrationBuilder.DropColumn(
                name: "ServiceID",
                table: "SmsoutHists");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "SmsoutDs");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.RenameTable(
                name: "ServiceRenewalConfigurations",
                newName: "ServiceRenewalConfiguration");

            migrationBuilder.RenameIndex(
                name: "IX_Services_Shortcode",
                table: "Service",
                newName: "IX_Service_Shortcode");

            migrationBuilder.RenameIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Service",
                newName: "IX_Service_ServiceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRenewalConfigurations_ServiceId",
                table: "ServiceRenewalConfiguration",
                newName: "IX_ServiceRenewalConfiguration_ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRenewalConfigurations_OperatorId",
                table: "ServiceRenewalConfiguration",
                newName: "IX_ServiceRenewalConfiguration_OperatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRenewalConfigurations_MessageId",
                table: "ServiceRenewalConfiguration",
                newName: "IX_ServiceRenewalConfiguration_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceRenewalConfiguration",
                table: "ServiceRenewalConfiguration",
                column: "ServiceRenewalConfigurationId");

            migrationBuilder.UpdateData(
                table: "ShortCodes",
                keyColumn: "Shortcode",
                keyValue: 93450,
                column: "Created",
                value: new DateTime(2019, 12, 17, 10, 55, 8, 508, DateTimeKind.Local).AddTicks(8787));

            migrationBuilder.AddForeignKey(
                name: "FK_Keywords_Service_ServiceId",
                table: "Keywords",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Service_ServiceId",
                table: "Messages",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceTypes_ServiceTypeId",
                table: "Service",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "ServiceTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ShortCodes_Shortcode",
                table: "Service",
                column: "Shortcode",
                principalTable: "ShortCodes",
                principalColumn: "Shortcode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCampaigns_Service_ServiceId",
                table: "ServiceCampaigns",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRenewalConfiguration_Messages_MessageId",
                table: "ServiceRenewalConfiguration",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRenewalConfiguration_Operators_OperatorId",
                table: "ServiceRenewalConfiguration",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "OperatorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRenewalConfiguration_Service_ServiceId",
                table: "ServiceRenewalConfiguration",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsinDs_Service_ServiceId",
                table: "SmsinDs",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmsinHists_Service_ServiceId",
                table: "SmsinHists",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionHists_Service_ServiceId",
                table: "SubscriptionHists",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Service_ServiceId",
                table: "Subscriptions",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
