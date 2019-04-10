using PetClinic.Models;

namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;

    public class PetClinicContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalAid> AnimalAids { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }
        public DbSet<Vet> Vets { get; set; }

        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            :base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AnimalAid>(ent =>
            {
                ent.HasIndex(a => a.Name)
                    .IsUnique();
            });

            builder.Entity<ProcedureAnimalAid>(ent =>
            {
                ent.HasKey(k => new {k.AnimalAidId, k.ProcedureId});

                ent.HasOne(x => x.Procedure)
                    .WithMany(z => z.ProcedureAnimalAids)
                    .HasForeignKey(k => k.ProcedureId);

                ent.HasOne(x => x.AnimalAid)
                    .WithMany(z => z.AnimalAidProcedures)
                    .HasForeignKey(k => k.AnimalAidId);
            });

            builder.Entity<Animal>(ent =>
            {
                ent.HasOne(p => p.Passport)
                    .WithOne(a => a.Animal);
            });

            builder.Entity<Passport>(ent =>
            {
                ent.HasKey(k => k.SerialNumber);
                ent.Property(p => p.SerialNumber)
                    .ValueGeneratedNever();
                    
                ent.HasOne(a => a.Animal)
                    .WithOne(p => p.Passport);
            });
        }
    }
}
