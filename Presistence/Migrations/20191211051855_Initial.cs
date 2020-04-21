using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Presistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentTypes",
                columns: table => new
                {
                    ContentTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.ContentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    OperatorId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperatorName = table.Column<string>(maxLength: 50, nullable: true),
                    RetryLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.OperatorId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    ServiceTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ShortCodes",
                columns: table => new
                {
                    Shortcode = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortCodes", x => x.Shortcode);
                });

            migrationBuilder.CreateTable(
                name: "Sids",
                columns: table => new
                {
                    SidBilling = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    OperatorId = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sids", x => x.SidBilling);
                });

            migrationBuilder.CreateTable(
                name: "SmsdnDs",
                columns: table => new
                {
                    SmsdnDId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ErrorCode = table.Column<string>(maxLength: 20, nullable: true),
                    Status = table.Column<string>(maxLength: 20, nullable: true),
                    ErrorDesc = table.Column<string>(maxLength: 100, nullable: true),
                    MtTxId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsdnDs", x => x.SmsdnDId);
                    table.UniqueConstraint("AK_SmsdnDs_MtTxId", x => x.MtTxId);
                });

            migrationBuilder.CreateTable(
                name: "SmsdnHists",
                columns: table => new
                {
                    SmsdnHistId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ErrorCode = table.Column<string>(maxLength: 20, nullable: true),
                    Status = table.Column<string>(maxLength: 20, nullable: true),
                    ErrorDesc = table.Column<string>(maxLength: 100, nullable: true),
                    MtTxId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsdnHists", x => x.SmsdnHistId);
                    table.UniqueConstraint("AK_SmsdnHists_MtTxId", x => x.MtTxId);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    ServiceCustom = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCustom = table.Column<bool>(nullable: false),
                    Shortcode = table.Column<int>(nullable: false),
                    ServiceTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Service_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Service_ShortCodes_Shortcode",
                        column: x => x.Shortcode,
                        principalTable: "ShortCodes",
                        principalColumn: "Shortcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    KeywordId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    KeyWord = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.KeywordId);
                    table.ForeignKey(
                        name: "FK_Keywords_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    MessageTxt = table.Column<string>(maxLength: 300, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Billing1 = table.Column<string>(maxLength: 100, nullable: true),
                    Billing2 = table.Column<string>(maxLength: 100, nullable: true),
                    Billing3 = table.Column<string>(maxLength: 100, nullable: true),
                    IsDynamicBilling = table.Column<bool>(nullable: false),
                    IsRichContent = table.Column<bool>(nullable: false),
                    MessageType = table.Column<string>(maxLength: 50, nullable: false),
                    Delay = table.Column<double>(nullable: false),
                    IsDnWatch = table.Column<bool>(nullable: false),
                    Sparam = table.Column<string>(maxLength: 500, nullable: true),
                    SidBilling = table.Column<string>(maxLength: 100, nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Sids_SidBilling",
                        column: x => x.SidBilling,
                        principalTable: "Sids",
                        principalColumn: "SidBilling",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCampaigns",
                columns: table => new
                {
                    CampaignId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CampaignName = table.Column<string>(maxLength: 50, nullable: false),
                    IsMainKeyword = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCallBackRequired = table.Column<bool>(nullable: false),
                    ExpiredDate = table.Column<DateTime>(nullable: false),
                    CampaignKeyword = table.Column<string>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCampaigns", x => x.CampaignId);
                    table.ForeignKey(
                        name: "FK_ServiceCampaigns_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceCampaigns_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsinDs",
                columns: table => new
                {
                    SmsindId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msisdn = table.Column<string>(maxLength: 20, nullable: false),
                    Mo_Message = table.Column<string>(maxLength: 200, nullable: false),
                    MotxId = table.Column<string>(maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    Shortcode = table.Column<int>(nullable: false),
                    Shortcode1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsinDs", x => x.SmsindId);
                    table.ForeignKey(
                        name: "FK_SmsinDs_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsinDs_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsinDs_ShortCodes_Shortcode1",
                        column: x => x.Shortcode1,
                        principalTable: "ShortCodes",
                        principalColumn: "Shortcode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmsinHists",
                columns: table => new
                {
                    SmsinHistId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msisdn = table.Column<string>(maxLength: 20, nullable: false),
                    Mo_Message = table.Column<string>(maxLength: 200, nullable: false),
                    MotxId = table.Column<string>(maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    Shortcode = table.Column<int>(nullable: false),
                    Shortcode1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsinHists", x => x.SmsinHistId);
                    table.ForeignKey(
                        name: "FK_SmsinHists_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsinHists_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsinHists_ShortCodes_Shortcode1",
                        column: x => x.Shortcode1,
                        principalTable: "ShortCodes",
                        principalColumn: "Shortcode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionHists",
                columns: table => new
                {
                    SubscriptionHistId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msisdn = table.Column<string>(maxLength: 20, nullable: false),
                    Reg_Keyword = table.Column<string>(maxLength: 200, nullable: true),
                    Unreg_keyword = table.Column<string>(maxLength: 200, nullable: true),
                    Subscription_Date = table.Column<DateTime>(nullable: false),
                    Unsubscription_Date = table.Column<DateTime>(nullable: false),
                    State = table.Column<string>(maxLength: 20, nullable: true),
                    Last_Renew_Time = table.Column<DateTime>(nullable: true),
                    Total_Revenue = table.Column<float>(nullable: false),
                    Mt_Sent = table.Column<int>(nullable: false),
                    Mt_Success = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionHists", x => x.SubscriptionHistId);
                    table.ForeignKey(
                        name: "FK_SubscriptionHists_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionHists_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msisdn = table.Column<string>(maxLength: 20, nullable: false),
                    Reg_Keyword = table.Column<string>(maxLength: 200, nullable: true),
                    Subscription_Date = table.Column<DateTime>(nullable: false),
                    Next_Renew_Time = table.Column<DateTime>(nullable: true),
                    Last_Renew_Time = table.Column<DateTime>(nullable: true),
                    Total_Revenue = table.Column<float>(nullable: false),
                    Mt_Sent = table.Column<int>(nullable: false),
                    Mt_Success = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubKeywords",
                columns: table => new
                {
                    SubKeywordId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    SubKeyWord = table.Column<string>(maxLength: 20, nullable: false),
                    KeywordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubKeywords", x => x.SubKeywordId);
                    table.ForeignKey(
                        name: "FK_SubKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "KeywordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    ContentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    ContentText = table.Column<string>(maxLength: 300, nullable: false),
                    ContentPath = table.Column<string>(maxLength: 500, nullable: true),
                    ContentSchedule = table.Column<DateTime>(nullable: true),
                    Processed = table.Column<bool>(nullable: false),
                    ContentTypeId = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_Contents_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentTypes",
                        principalColumn: "ContentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contents_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRenewalConfiguration",
                columns: table => new
                {
                    ServiceRenewalConfigurationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleDay = table.Column<int>(nullable: true),
                    ScheduleSequence = table.Column<int>(nullable: false),
                    ScheduleOrder = table.Column<int>(nullable: false),
                    ActiveDll = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    IsSequence = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRenewalConfiguration", x => x.ServiceRenewalConfigurationId);
                    table.ForeignKey(
                        name: "FK_ServiceRenewalConfiguration_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRenewalConfiguration_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRenewalConfiguration_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsoutDs",
                columns: table => new
                {
                    SmsoutDId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msisdn = table.Column<string>(maxLength: 20, nullable: false),
                    Mt_Message = table.Column<string>(maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateToProcessed = table.Column<DateTime>(nullable: false),
                    DateProcessed = table.Column<DateTime>(nullable: true),
                    Trx_Status = table.Column<string>(maxLength: 20, nullable: true),
                    IsDnWatch = table.Column<bool>(nullable: false),
                    Sparam = table.Column<string>(maxLength: 50, nullable: true),
                    Iparam = table.Column<int>(nullable: false),
                    MtTxId = table.Column<string>(maxLength: 200, nullable: true),
                    OperatorId = table.Column<int>(nullable: false),
                    Shortcode = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false),
                    Shortcode1 = table.Column<int>(nullable: true),
                    SmsdnDMtTxId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsoutDs", x => x.SmsoutDId);
                    table.ForeignKey(
                        name: "FK_SmsoutDs_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsoutDs_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsoutDs_ShortCodes_Shortcode1",
                        column: x => x.Shortcode1,
                        principalTable: "ShortCodes",
                        principalColumn: "Shortcode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmsoutDs_SmsdnDs_SmsdnDMtTxId",
                        column: x => x.SmsdnDMtTxId,
                        principalTable: "SmsdnDs",
                        principalColumn: "MtTxId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmsoutHists",
                columns: table => new
                {
                    SmsoutHistId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Msisdn = table.Column<string>(maxLength: 20, nullable: false),
                    Mt_Message = table.Column<string>(maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateToProcessed = table.Column<DateTime>(nullable: false),
                    DateProcessed = table.Column<DateTime>(nullable: false),
                    Trx_Status = table.Column<string>(maxLength: 20, nullable: true),
                    IsDnWatch = table.Column<bool>(nullable: false),
                    Sparam = table.Column<string>(maxLength: 50, nullable: true),
                    Iparam = table.Column<int>(nullable: false),
                    MtTxId = table.Column<string>(maxLength: 200, nullable: true),
                    Shortcode = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    Shortcode1 = table.Column<int>(nullable: true),
                    SmsdnHistMtTxId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsoutHists", x => x.SmsoutHistId);
                    table.ForeignKey(
                        name: "FK_SmsoutHists_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsoutHists_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "OperatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmsoutHists_ShortCodes_Shortcode1",
                        column: x => x.Shortcode1,
                        principalTable: "ShortCodes",
                        principalColumn: "Shortcode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmsoutHists_SmsdnHists_SmsdnHistMtTxId",
                        column: x => x.SmsdnHistMtTxId,
                        principalTable: "SmsdnHists",
                        principalColumn: "MtTxId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentTypeId",
                table: "Contents",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_MessageId",
                table: "Contents",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_ServiceId",
                table: "Keywords",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_OperatorId",
                table: "Messages",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ServiceId",
                table: "Messages",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SidBilling",
                table: "Messages",
                column: "SidBilling");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ServiceTypeId",
                table: "Service",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Shortcode",
                table: "Service",
                column: "Shortcode");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCampaigns_OperatorId",
                table: "ServiceCampaigns",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCampaigns_ServiceId",
                table: "ServiceCampaigns",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRenewalConfiguration_MessageId",
                table: "ServiceRenewalConfiguration",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRenewalConfiguration_OperatorId",
                table: "ServiceRenewalConfiguration",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRenewalConfiguration_ServiceId",
                table: "ServiceRenewalConfiguration",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinDs_OperatorId",
                table: "SmsinDs",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinDs_ServiceId",
                table: "SmsinDs",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinDs_Shortcode1",
                table: "SmsinDs",
                column: "Shortcode1");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinHists_OperatorId",
                table: "SmsinHists",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinHists_ServiceId",
                table: "SmsinHists",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsinHists_Shortcode1",
                table: "SmsinHists",
                column: "Shortcode1");

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutDs_MessageId",
                table: "SmsoutDs",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutDs_OperatorId",
                table: "SmsoutDs",
                column: "OperatorId");

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
                name: "IX_SmsoutHists_MessageId",
                table: "SmsoutHists",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsoutHists_OperatorId",
                table: "SmsoutHists",
                column: "OperatorId");

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
                name: "IX_SubKeywords_KeywordId",
                table: "SubKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHists_OperatorId",
                table: "SubscriptionHists",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHists_ServiceId",
                table: "SubscriptionHists",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OperatorId",
                table: "Subscriptions",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ServiceId",
                table: "Subscriptions",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "ServiceCampaigns");

            migrationBuilder.DropTable(
                name: "ServiceRenewalConfiguration");

            migrationBuilder.DropTable(
                name: "SmsinDs");

            migrationBuilder.DropTable(
                name: "SmsinHists");

            migrationBuilder.DropTable(
                name: "SmsoutDs");

            migrationBuilder.DropTable(
                name: "SmsoutHists");

            migrationBuilder.DropTable(
                name: "SubKeywords");

            migrationBuilder.DropTable(
                name: "SubscriptionHists");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "ContentTypes");

            migrationBuilder.DropTable(
                name: "SmsdnDs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SmsdnHists");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Sids");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "ShortCodes");
        }
    }
}
