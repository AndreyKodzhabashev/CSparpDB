using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using PetClinic.Models.Attributes;

namespace PetClinic.Data.ModelDtos.Import
{
    [XmlType("Vet")]
    public class ImportVetDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Profession { get; set; }


        [Required] [Range(22, 65)] public int Age { get; set; }

        [Required]
       // [PhoneNumber]
        [RegularExpression(@"^(\+359|0)[0-9]{9}$")]
        public string PhoneNumber { get; set; }
    }
}