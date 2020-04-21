using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class serviceCampaignConfiguration : IEntityTypeConfiguration<ServiceCampaign>
    {
        public void Configure(EntityTypeBuilder<ServiceCampaign> builder)
        {
            builder.HasKey(e => e.CampaignId);

            builder.Property(e => e.CampaignId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CampaignName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.CampaignKeyword)
                .IsRequired();

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.HasOne(e => e.Service)
                .WithMany(e => e.ServiceCampaigns)
                .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.ServiceCampaigns)
                .HasForeignKey(e => e.OperatorId);
        }
    }
}
