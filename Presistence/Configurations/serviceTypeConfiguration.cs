using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class serviceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            builder.HasKey(e => e.ServiceTypeId);

            builder.Property(e => e.Type)
                .HasMaxLength(150);

            builder.HasData
                (
                    new ServiceType
                    {
                        ServiceTypeId = 1,
                        Type = "Entertaiment"
                    },
                    new ServiceType
                    {
                        ServiceTypeId = 2,
                        Type = "Sport"
                    },
                    new ServiceType
                    {
                        ServiceTypeId = 3,
                        Type = "Games"
                    },
                    new ServiceType
                    {
                        ServiceTypeId = 4,
                        Type = "Religion"
                    }
                );
        }
    }
}
