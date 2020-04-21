using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;
using System;

namespace Presistence.Configurations
{
    public class shortCodeConfiguration : IEntityTypeConfiguration<ShortCode>
    {
        public void Configure(EntityTypeBuilder<ShortCode> builder)
        {
            builder.HasKey(e => e.Shortcode);

            builder.Property(e => e.Description)
                .HasMaxLength(300);

            builder.HasData
                (
                    new ShortCode
                    {
                        Created = DateTime.Now,
                        CreatedBy = "InitialMigration",
                        Shortcode = 93450,
                        Description = "HM Short Code"
                    }
                ) ;
        }
    }
}
