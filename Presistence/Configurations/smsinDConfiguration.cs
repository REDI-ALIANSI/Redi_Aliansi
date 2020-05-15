using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class smsinDConfiguration : IEntityTypeConfiguration<SmsinD>
    {
        public void Configure(EntityTypeBuilder<SmsinD> builder)
        {
            builder.HasKey(e => e.SmsindId);

            builder.Property(e => e.SmsindId)
                .ValueGeneratedOnAdd();
            
            builder.Property(e => e.Msisdn)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Mo_Message)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.MotxId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ServiceId)
                .IsRequired();

            builder.Property(e => e.OperatorId)
                .IsRequired();
        }
    }
}
