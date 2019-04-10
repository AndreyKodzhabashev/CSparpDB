using TestDtoMapp.Models.Models;

namespace TestDtoMapp.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //Only first name, last name and salary are required.
            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.LastName)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.Salary)
                .IsRequired();
        }
    }
}