using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Medicaments")]
    public class Medicament
    {
        [Key] [Column("MedicamentId")] public int MedicamentId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } //(up to 50 characters, unicode)

        [InverseProperty("Medicament")]
        public ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}