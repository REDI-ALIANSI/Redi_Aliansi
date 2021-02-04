using Domain.Entities;
using Domain.Entities.SMS;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRediSmsDbContext
    {
        DbSet<Service> Services { get; set; }
        DbSet<Content> Contents { get; set; }
        DbSet<ContentType> ContentTypes { get; set; }
        DbSet<Keyword> Keywords { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<ServiceCampaign> ServiceCampaigns { get; set; }
        DbSet<ServiceRenewalConfiguration> ServiceRenewalConfigurations { get; set; }
        DbSet<ServiceType> ServiceTypes { get; set; }
        DbSet<ShortCode> ShortCodes { get; set; }
        DbSet<Sid> Sids { get; set; }
        DbSet<SmsdnD> SmsdnDs { get; set; }
        DbSet<SmsdnHist> SmsdnHists { get; set; }
        DbSet<SmsinD> SmsinDs { get; set; }
        DbSet<SmsinHist> SmsinHists { get; set; }
        DbSet<SmsoutD> SmsoutDs { get; set; }
        DbSet<SmsoutHist> SmsoutHists { get; set; }
        DbSet<SubKeyword> SubKeywords { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<SubscriptionHist> SubscriptionHists { get; set; }
        DbSet<Operator> Operators { get; set; }
        DbSet<RevenueReport> RevenueReports { get; set; }
        DbSet<CampaignReport> CampaignReports { get; set; }
        DbSet<SubscriptionReport> SubscriptionReports { get; set; }
        DbSet<GenReportStatus> GenReports { get; set; }
        DbSet<BlackList> BlackLists { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
