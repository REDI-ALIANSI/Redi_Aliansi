using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class serviceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(e => e.ServiceId);

            builder.Property(e => e.ServiceId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.ServiceCustom)
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Shortcode)
                .IsRequired();

            builder.Property(e => e.ServiceTypeId)
                .IsRequired();

            //builder.HasMany(e => e.Keywords)
            //    .WithOne(e => e.Service)
            //    .HasForeignKey(e => e.ServiceId);

            //builder.HasMany(e => e.Subscriptions)
            //    .WithOne(e => e.Service)
            //    .HasForeignKey(e => e.ServiceId);

            //builder.HasMany(e => e.ServiceCampaigns)
            //    .WithOne(e => e.Service)
            //    .HasForeignKey(e => e.ServiceId);

            //builder.HasMany(e => e.Messages)
            //    .WithOne(e => e.Service)
            //    .HasForeignKey(e => e.ServiceId);

            //builder.HasMany(e => e.ServiceRenewalConfigurations)
            //    .WithOne(e => e.Service)
            //    .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.ShortCode)
                .WithMany(e => e.Services)
                .HasForeignKey(e => e.Shortcode);

            builder.HasOne(e => e.ServiceType)
                .WithMany(e => e.Services)
                .HasForeignKey(e => e.ServiceTypeId);
        }
    }
}
