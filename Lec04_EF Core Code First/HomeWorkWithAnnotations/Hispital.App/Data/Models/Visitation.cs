using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Visitations")]
    public class Visitation
    {
        [Key] [Column("VisitationId")] public int VisitationId { get; set; }

        [Required] public DateTime Date { get; set; }

        [Required] [StringLength(250)] public string Comments { get; set; } //(up to 250 characters, unicode)


        //navigation property to Doctors
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        [InverseProperty("Visitations")]
        public Doctor Doctor { get; set; }

        //navigation property to Patients
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        [InverseProperty("Visitations")]
        public Patient Patient { get; set; }
    }
}