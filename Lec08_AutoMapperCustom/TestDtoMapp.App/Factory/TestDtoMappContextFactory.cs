

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TestDtoMapp.Data;

namespace TestDtoMapp.App.Factory
{
    public class TestDtoMappContextFactory : IDesignTimeDbContextFactory<TestDtoMappContext>
    {

        public TestDtoMappContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDtoMappContext>();
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);

            return new TestDtoMappContext(optionsBuilder.Options);
        }

    }
}