using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    class GenReportStatusConfiguration : IEntityTypeConfiguration<GenReportStatus>
    {
        public void Configure(EntityTypeBuilder<GenReportStatus> builder)
        {
            builder.HasKey(e => e.StatusId);

            builder.Property(e => e.StatusId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Status)
                .IsRequired();
        }
    }
}
