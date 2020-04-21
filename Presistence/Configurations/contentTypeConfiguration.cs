using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;


namespace Presistence.Configurations
{
    class contentTypeConfiguration : IEntityTypeConfiguration<ContentType>
    {
        public void Configure(EntityTypeBuilder<ContentType> builder)
        {
            builder.HasKey(e => e.ContentTypeId);

            builder.Property(e => e.Name)
                .HasMaxLength(150);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.HasData
                (
                    new ContentType
                    {
                        ContentTypeId = 1,
                        Name = "TEXT",
                        Description = "PLAIN CONTENT"
                    },
                    new ContentType
                    {
                        ContentTypeId = 2,
                        Name = "PICTURE",
                        Description = "RICH PICTURE CONTENT"
                    },
                    new ContentType
                    {
                        ContentTypeId = 3,
                        Name = "VIDEO",
                        Description = "RICH VIDEO CONTENT"
                    }
                );

        }
    }
}
