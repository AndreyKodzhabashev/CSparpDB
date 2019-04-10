namespace P01_HospitalDatabase.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class HospitalContext : DbContext
    {
        private const string ConnectionString =
            @"Server=DESKTOP-CVEQJBP\SQLEXPRESS;Database=HospitalDB;Trusted_Connection=True;";

        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions<HospitalContext> options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (builder.IsConfigured == false)
            {
                builder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigurePatientEntity(builder);

            ConfigureVisitationEntity(builder);

            ConfigureDiagnoseEntity(builder);

            ConfigureMedicamentEntity(builder);

            ConfigurePatientMedicamentEntity(builder);
        }

        private void ConfigurePatientMedicamentEntity(ModelBuilder builder)
        {
            builder.Entity<PatientMedicament>().HasKey(pm => new {pm.PatientId, pm.MedicamentId});

            builder.Entity<PatientMedicament>().HasOne(p => p.Patient).WithMany(p => p.Prescriptions);
            builder.Entity<PatientMedicament>().HasOne(m => m.Medicament).WithMany(p => p.Prescriptions);
        }

        private void ConfigureMedicamentEntity(ModelBuilder builder)
        {
            builder.Entity<Medicament>().HasKey(m => m.MedicamentId);

            builder.Entity<Medicament>().Property(m => m.Name).HasMaxLength(50).IsUnicode(true);
        }


        private void ConfigureDiagnoseEntity(ModelBuilder builder)
        {
            builder.Entity<Diagnose>().HasKey(d => d.DiagnoseId);
            builder.Entity<Diagnose>().Property(d => d.Name).HasMaxLength(50).IsUnicode(true);
            builder.Entity<Diagnose>().Property(d => d.Comments).HasMaxLength(250).IsUnicode(true);
            builder.Entity<Diagnose>().HasOne(p => p.Patient).WithMany(d => d.Diagnoses)
                .HasForeignKey(d => d.PatientId);
        }

        private void ConfigureVisitationEntity(ModelBuilder builder)
        {
            builder.Entity<Visitation>().HasKey(v => v.VisitationId);

            builder.Entity<Visitation>().Property(v => v.Comments).HasMaxLength(250).IsUnicode(true);

            builder.Entity<Visitation>().HasOne(p => p.Patient).WithMany(v => v.Visitations)
                .HasForeignKey(v => v.PatientId);
        }

        private void ConfigurePatientEntity(ModelBuilder builder)
        {
            builder.Entity<Patient>().HasKey(p => p.PatientId);

            builder.Entity<Patient>().Property(p => p.FirstName).HasMaxLength(50).IsRequired(true).IsUnicode(true);

            builder.Entity<Patient>().Property(p => p.LastName).HasMaxLength(50).IsRequired(true).IsUnicode(true);

            builder.Entity<Patient>().Property(p => p.Address).HasMaxLength(250).IsUnicode(true);

            builder.Entity<Patient>().Property(p => p.Email).HasMaxLength(80).IsUnicode(true);

            builder.Entity<Patient>().HasMany(p => p.Visitations).WithOne(v => v.Patient)
                .HasForeignKey(p => p.PatientId);
        }
    }
}