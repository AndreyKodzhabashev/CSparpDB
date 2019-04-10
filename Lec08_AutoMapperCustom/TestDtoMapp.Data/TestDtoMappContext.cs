namespace TestDtoMapp.Data
{
    using Microsoft.EntityFrameworkCore;
    using TestDtoMapp.Models.Models;

    public class TestDtoMappContext : DbContext
    {

        public DbSet<Employee> Employees { get; set; }
      
        public TestDtoMappContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        {
        }
    }
}