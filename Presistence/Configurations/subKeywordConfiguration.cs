using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class subKeywordConfiguration : IEntityTypeConfiguration<SubKeyword>
    {
        public void Configure(EntityTypeBuilder<SubKeyword> builder)
        {
            builder.HasKey(e => e.SubKeywordId);

            builder.Property(e => e.SubKeywordId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.SubKeyWord)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.KeywordId)
                .IsRequired();

            builder.HasOne(e => e.Keyword)
                .WithMany(e => e.SubKeywords)
                .HasForeignKey(e => e.KeywordId);
        }
    }
}
