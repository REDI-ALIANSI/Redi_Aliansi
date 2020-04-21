using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class contentConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.HasKey(e => e.ContentId);

            builder.Property(e => e.ContentId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ContentText)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.ContentPath)
                .HasMaxLength(500);

            builder.Property(e => e.ContentTypeId)
                .IsRequired();

            builder.Property(e => e.MessageId)
                .IsRequired();
        }
    }
}
