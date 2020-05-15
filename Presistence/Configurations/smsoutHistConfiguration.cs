using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class smsoutHistConfiguration : IEntityTypeConfiguration<SmsoutHist>
    {
        public void Configure(EntityTypeBuilder<SmsoutHist> builder)
        {
            builder.HasKey(e => e.SmsoutHistId);

            builder.Property(e => e.SmsoutHistId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Msisdn)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Mt_Message)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Trx_Status)
                .HasMaxLength(20);

            builder.Property(e => e.Sparam)
                .HasMaxLength(50);

            builder.Property(e => e.MtTxId)
                .HasMaxLength(200);

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.MessageId)
                .IsRequired();

            builder.HasAlternateKey(e => e.MtTxId);
        }
    }
}
