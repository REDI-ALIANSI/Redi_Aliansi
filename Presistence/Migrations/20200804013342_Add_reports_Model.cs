using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Presistence.Migrations
{
    public partial class Add_reports_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignReports",
                columns: table => new
                {
                    CampaignReportId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    New_Member = table.Column<int>(nullable: false),
                    Churn_Member = table.Column<int>(nullable: false),
                    Total_Member = table.Column<int>(nullable: false),
                    Mt_Sent = table.Column<int>(nullable: false),
                    Mt_Hits = table.Column<int>(nullable: false),
                    Revenue = table.Column<double>(nullable: false),
                    ServiceCampaignId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignReports", x => x.CampaignReportId);
                    table.ForeignKey(
                        name: "FK_CampaignReports_ServiceCampaigns_ServiceCampaignId",
                        column: x => x.ServiceCampaignId,
                        principalTable: "ServiceCampaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevenueReports",
                columns: table => new
                {
                    RevenueReportId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SDC = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Sid = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Mt_Hits = table.Column<int>(nullable: false),
                    Mt_Sent = table.Column<int>(nullable: false),
                    Mt_Type = table.Column<string>(nullable: true),
                    Revenue = table.Column<double>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueReports", x => x.RevenueReportId);
                    table.ForeignKey(
                        name: "FK_RevenueReports_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevenueReports_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionReports",
                columns: table => new
                {
                    SubscriptionReportId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceName = table.Column<string>(nullable: true),
                    SDC = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    New_Member = table.Column<int>(nullable: false),
                    Churn_Member = table.Column<int>(nullable: false),
                    Total_Member = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionReports", x => x.SubscriptionReportId);
                    table.ForeignKey(
                        name: "FK_SubscriptionReports_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionReports_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignReports_ServiceCampaignId",
                table: "CampaignReports",
                column: "ServiceCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueReports_OperatorId",
                table: "RevenueReports",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueReports_ServiceId",
                table: "RevenueReports",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionReports_OperatorId",
                table: "SubscriptionReports",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionReports_ServiceId",
                table: "SubscriptionReports",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignReports");

            migrationBuilder.DropTable(
                name: "RevenueReports");

            migrationBuilder.DropTable(
                name: "SubscriptionReports");
        }
    }
}
