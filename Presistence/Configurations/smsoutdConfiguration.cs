﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.SMS;

namespace Presistence.Configurations
{
    public class smsoutdConfiguration : IEntityTypeConfiguration<SmsoutD>
    {
        public void Configure(EntityTypeBuilder<SmsoutD> builder)
        {
            builder.HasKey(e => e.SmsoutDId);

            builder.Property(e => e.SmsoutDId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Msisdn)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Mt_Message)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Trx_Status)
                .HasMaxLength(20);

            builder.Property(e => e.Sparam)
                .HasMaxLength(50);

            builder.Property(e => e.MtTxId)
                .HasMaxLength(200);

            builder.Property(e => e.OperatorId)
                .IsRequired();

            builder.Property(e => e.Shortcode)
                .IsRequired();

            builder.Property(e => e.MessageId)
                .IsRequired();

            builder.HasOne(e => e.SmsdnD)
                .WithOne(e => e.SmsoutD)
                .HasPrincipalKey<SmsdnD>(e => e.MtTxId);
        }
    }
}