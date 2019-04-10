using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Patients")]
    public class Patient
    {
        [Key] [Column("PatientId")] public int PatientId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } //(up to 50 characters, unicode)

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } //   (up to 50 characters, unicode)

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Address { get; set; } //(up to 250 characters, unicode)

        [Required]
        [Column(TypeName = "nvarchar(80)")]
        public string Email { get; set; } //(up to 80 characters, not unicode)

        public bool HasInsurance { get; set; }

        //relation property
        [InverseProperty("Patient")]
        public ICollection<Visitation> Visitations { get; set; }
        [InverseProperty("Patient")]
        public ICollection<Diagnose> Diagnoses { get; set; }

        [InverseProperty("Patient")]
        public ICollection<PatientMedicament> Prescriptions { get; set; }


    }
}