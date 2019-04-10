using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;


namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions<SalesContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //"Server=localhost\\sqlexpress;Database=SoftUni;Trusted_Connection=True;"
            if (builder.IsConfigured == false)
            {
                builder.UseSqlServer("Server=localhost\\sqlexpress;Database=SalesDB;Trusted_Connection=True;");
            }
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
                entity.Property(p => p.Date).HasDefaultValueSql("GetDate()"));

            modelBuilder.Entity<Product>(entity =>
                entity.Property(p => p.Description).HasDefaultValue("No description"));
        }
    }
}