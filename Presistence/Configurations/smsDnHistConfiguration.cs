using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class smsDnHistConfiguration : IEntityTypeConfiguration<SmsdnHist>
    {
        public void Configure(EntityTypeBuilder<SmsdnHist> builder)
        {
            builder.HasKey(e => e.SmsdnHistId);

            builder.Property(e => e.SmsdnHistId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ErrorCode)
                .HasMaxLength(20);

            builder.Property(e => e.Status)
                .HasMaxLength(20);

            builder.Property(e => e.ErrorDesc)
                .HasMaxLength(100);

            builder.HasAlternateKey(e => e.MtTxId);

            builder.Property(e => e.SmsdnHistId)
                .IsRequired();
        }
    }
}
