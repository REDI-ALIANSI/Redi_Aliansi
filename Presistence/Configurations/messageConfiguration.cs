using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class messageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(e => e.MessageId);

            builder.Property(e => e.MessageId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.MessageTxt)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.Billing1)
                .HasMaxLength(100);

            builder.Property(e => e.Billing2)
                .HasMaxLength(100);

            builder.Property(e => e.Billing3)
                .HasMaxLength(100);

            builder.Property(e => e.MessageType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Sparam)
                .HasMaxLength(500);

            builder.Property(e => e.SidBilling)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.HasOne(e => e.Service)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.ServiceId);

            builder.HasOne(e => e.Sid)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.SidBilling);

            builder.HasOne(e => e.Operator)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.OperatorId);
        }
    }
}
