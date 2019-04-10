using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }

        public HospitalContext()
        {

        }
        public HospitalContext(DbContextOptions<HospitalContext> options)
               : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (builder.IsConfigured == false)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(k => new {k.PatientId, k.MedicamentId});

                entity.HasOne(p => p.Patient)
                    .WithMany(pm => pm.Prescriptions)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientsMedicaments_Patients");

                entity.HasOne<Medicament>(m => m.Medicament)
                    .WithMany(p => p.Prescriptions)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientsMedicaments_Medicaments");
            });

        }
    }
}
