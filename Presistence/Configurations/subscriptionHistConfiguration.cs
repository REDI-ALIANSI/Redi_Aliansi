using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class subscriptionHistConfiguration : IEntityTypeConfiguration<SubscriptionHist>
    {
        public void Configure(EntityTypeBuilder<SubscriptionHist> builder)
        {
            builder.HasKey(e => e.SubscriptionHistId);

            builder.Property(e => e.SubscriptionHistId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Msisdn)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Reg_Keyword)
                .HasMaxLength(200);

            builder.Property(e => e.Unreg_keyword)
                .HasMaxLength(200);

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.State)
                .HasMaxLength(20);

            builder.HasOne(e => e.Service)
                .WithMany(e => e.GetSubscriptionHists)
                .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.SubscriptionHists)
                .HasForeignKey(e => e.OperatorId);
        }
    }
}
