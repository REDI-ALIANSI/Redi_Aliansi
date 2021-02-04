using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class blackListConfiguration : IEntityTypeConfiguration<BlackList>
    {
        public void Configure(EntityTypeBuilder<BlackList> builder)
        {
            builder.HasKey(e => e.BlackListId);

            builder.Property(e => e.BlackListId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Msisdn)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.BlackLists)
                .HasForeignKey(e => e.OperatorId)
                .IsRequired();
        }
    }
}
