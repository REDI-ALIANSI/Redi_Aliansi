using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class CampaignReportConfiguration : IEntityTypeConfiguration<CampaignReport>
    {
        public void Configure(EntityTypeBuilder<CampaignReport> builder)
        {
            builder.HasKey(e => e.CampaignReportId);

            builder.Property(e => e.CampaignReportId)
                .ValueGeneratedOnAdd();

            builder.HasOne(e => e.ServiceCampaign)
                .WithMany(e => e.CampaignReports)
                .HasForeignKey(e => e.ServiceCampaignId);
        }
    }
}
