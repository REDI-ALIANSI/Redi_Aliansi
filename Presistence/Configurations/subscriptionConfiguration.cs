using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class subscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(e => e.SubscriptionId);

            builder.Property(e => e.SubscriptionId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Msisdn)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Reg_Keyword)
                .HasMaxLength(200);

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.HasOne(e => e.Service)
                .WithMany(e => e.Subscriptions)
                .HasForeignKey(e => e.ServiceId)
                .IsRequired();

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.Subscriptions)
                .HasForeignKey(e => e.OperatorId)
                .IsRequired();
        }
    }
}
