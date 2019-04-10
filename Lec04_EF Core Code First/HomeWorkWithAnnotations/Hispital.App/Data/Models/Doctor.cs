using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        //properties for the table
        [Key] [Column("DoctorId")] public int DoctorId { get; set; }
        [Required] [Column(TypeName = "nvarchar(100)")]public string Name { get; set; }
        [Required] [Column(TypeName = "nvarchar(100)")]public string Specialty { get; set; }

        //navigation property
        //public int VisitationId { get; set; }
        //[ForeignKey("VisitationId")] public Visitation Visitation { get; set; }
      [InverseProperty("Doctor")]
        public ICollection<Visitation> Visitations { get; set; }
    }
}