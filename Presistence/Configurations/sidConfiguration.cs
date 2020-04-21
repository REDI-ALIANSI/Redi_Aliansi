using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class sidConfiguration : IEntityTypeConfiguration<Sid>
    {
        public void Configure(EntityTypeBuilder<Sid> builder)
        {
            builder.HasKey(e => e.SidBilling);

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.SidBilling)
                .IsRequired();

            builder.HasMany(e => e.Messages)
                .WithOne(e => e.Sid)
                .HasForeignKey(e => e.SidBilling);
        }
    }
}
