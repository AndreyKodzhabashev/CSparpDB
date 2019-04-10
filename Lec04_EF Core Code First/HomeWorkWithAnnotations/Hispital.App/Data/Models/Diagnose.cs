using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Diagnoses")]
    public class Diagnose
    {
        [Key] [Column("DiagnoseId")] public int DiagnoseId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } //(up to 50 characters, unicode)

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Comments { get; set; } //(up to 250 characters, unicode)

        //navigation property
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        [InverseProperty("Diagnoses")]
        public Patient Patient { get; set; }
    }
}