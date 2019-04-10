using System;

namespace BillsPaymentSystem.Data.EntityConfiguration
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(e => e.BankAccountId);

            builder.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            builder.Property(e => e.SWIFTCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.Property<DateTime?>("Created").ValueGeneratedOnAdd();
            builder.Property<DateTime?>("Updated").ValueGeneratedOnAddOrUpdate();

        }
    }
}