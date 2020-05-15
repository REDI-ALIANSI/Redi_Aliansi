using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class smsdndConfiguration : IEntityTypeConfiguration<SmsdnD>
    {
        public void Configure(EntityTypeBuilder<SmsdnD> builder)
        {
            builder.HasKey(e => e.SmsdnDId);

            builder.Property(e => e.SmsdnDId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ErrorCode)
                .HasMaxLength(20);

            builder.Property(e => e.Status)
                .HasMaxLength(20);

            builder.Property(e => e.ErrorDesc)
                .HasMaxLength(100);

            builder.HasOne(e => e.SmsoutD)
                .WithOne(o => o.SmsdnD)
                .HasForeignKey<SmsdnD>(e => e.MtTxId)
                .HasPrincipalKey<SmsoutD>(o => o.MtTxId)
                .IsRequired(false);
        }
    }
}
