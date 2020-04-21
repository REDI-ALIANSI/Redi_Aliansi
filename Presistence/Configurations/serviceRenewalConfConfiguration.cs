using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class serviceRenewalConfConfiguration : IEntityTypeConfiguration<ServiceRenewalConfiguration>
    {
        public void Configure(EntityTypeBuilder<ServiceRenewalConfiguration> builder)
        {
            builder.HasKey(e => e.ServiceRenewalConfigurationId);

            builder.Property(e => e.ServiceRenewalConfigurationId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.Property(e => e.MessageId)
                .IsRequired();

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.HasOne(e => e.Service)
                .WithMany(e => e.ServiceRenewalConfigurations)
                .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.Message)
                .WithMany(e => e.ServiceRenewalConfigurations)
                .HasForeignKey(e => e.MessageId);
        }
    }
}
