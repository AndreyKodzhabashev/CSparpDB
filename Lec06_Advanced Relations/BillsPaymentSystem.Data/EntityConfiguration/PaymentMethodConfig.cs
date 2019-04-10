using System;
using BillsPaymentSystem.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PaymentType)
                .HasConversion(new EnumToStringConverter<PaymentType>());
                    //v => v.ToString(),
                    //v => (PaymentType) Enum.Parse(typeof(PaymentType), v));

            builder.HasOne(e => e.BankAccount)
                .WithOne(pm => pm.PaymentMethod);

            builder.HasOne(e => e.CreditCard)
                .WithOne(c => c.PaymentMethod);

            builder.HasOne(e => e.User)
                .WithMany(u => u.PaymentMethods)
                .HasForeignKey(e => e.UserId);
        }
    }
}