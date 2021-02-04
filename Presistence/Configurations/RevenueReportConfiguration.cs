using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class RevenueReportConfiguration : IEntityTypeConfiguration<RevenueReport>
    {
        public void Configure(EntityTypeBuilder<RevenueReport> builder)
        {
            builder.HasKey(e => e.RevenueReportId);

            builder.Property(e => e.RevenueReportId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.HasOne(e => e.Service)
                .WithMany(e => e.RevenueReports)
                .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.RevenueReports)
                .HasForeignKey(e => e.OperatorId);
        }
    }
}
