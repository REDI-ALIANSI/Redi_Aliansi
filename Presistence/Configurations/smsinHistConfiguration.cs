using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class smsinHistConfiguration : IEntityTypeConfiguration<SmsinHist>
    {
        public void Configure(EntityTypeBuilder<SmsinHist> builder)
        {
            builder.HasKey(e => e.SmsinHistId);

            builder.Property(e => e.SmsinHistId)
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
