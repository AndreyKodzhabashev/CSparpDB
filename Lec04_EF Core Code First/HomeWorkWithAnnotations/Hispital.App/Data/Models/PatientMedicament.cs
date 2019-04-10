using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("PatientsMedicaments")]
    public class PatientMedicament
    {
        [Column("PatientID")]
        public int PatientId { get; set; }

       [Column("MedicamentID")]
       public int MedicamentId { get; set; }

       [ForeignKey("PatientId")]
       [InverseProperty("Prescriptions")]
       public Patient Patient { get; set; }
        [ForeignKey("MedicamentId")]
       [InverseProperty("Prescriptions")]
        public Medicament Medicament { get; set; }

    }
}