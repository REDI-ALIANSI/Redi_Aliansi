using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class SubscriptionReportConfiguration : IEntityTypeConfiguration<SubscriptionReport>
    {
        public void Configure(EntityTypeBuilder<SubscriptionReport> builder)
        {
            builder.HasKey(e => e.SubscriptionReportId);

            builder.Property(e => e.SubscriptionReportId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.HasOne(e => e.Service)
                .WithMany(e => e.SubscriptionReports)
                .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.SubscriptionReports)
                .HasForeignKey(e => e.OperatorId);
        }
    }
}
