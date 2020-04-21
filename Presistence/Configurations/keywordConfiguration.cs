using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class keywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.HasKey(e => e.KeywordId);

            builder.Property(e => e.KeywordId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.KeyWord)
                .HasMaxLength(50);

            builder.HasOne(e => e.Service)
                .WithMany(e => e.Keywords)
                .HasForeignKey(e => e.ServiceId);
        }
    }
}
