using SoftJail.Data.Models;

namespace SoftJail.Data
{
    using Microsoft.EntityFrameworkCore;

    public class SoftJailDbContext : DbContext
    {
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }
        public DbSet<Prisoner> Prisoners { get; set; }

        public SoftJailDbContext()
        {
        }

        public SoftJailDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OfficerPrisoner>().HasKey(k => new { k.OfficerId, k.PrisonerId });
            builder.Entity<OfficerPrisoner>(entity =>
                entity
                    .HasOne(x => x.Prisoner)
                    .WithMany(p => p.PrisonerOfficers));
                    //.OnDelete(DeleteBehavior.Restrict));

            builder.Entity<OfficerPrisoner>(entity =>
                entity
                    .HasOne(x => x.Officer)
                    .WithMany(p => p.OfficerPrisoners));
            //.OnDelete(DeleteBehavior.Restrict));


        }
    }
}