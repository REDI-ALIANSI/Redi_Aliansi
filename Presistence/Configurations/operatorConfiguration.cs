using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Presistence.Configurations
{
    public class operatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.HasKey(e => e.OperatorId);

            builder.Property(e => e.OperatorName)
                .HasMaxLength(50);

            builder.HasData
                (
                    new Operator
                    {
                        OperatorId = 51010,
                        OperatorName = "TELKOMSEL"
                    },
                    new Operator
                    {
                        OperatorId = 51011,
                        OperatorName = "EXCELCOM"
                    },
                    new Operator
                    {
                        OperatorId = 51021,
                        OperatorName = "INDOSAT"
                    }
                );
        }
    }
}
