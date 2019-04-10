using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.Data.ModelDtos.Import
{
    [XmlType("Procedure")]
    public class ImportProceduresDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Vet { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Animal { get; set; }

        [Required] public string DateTime { get; set; }

        [XmlArray("AnimalAids")] public ImportAnmalAidsDto[] AnimalAids { get; set; }
    }

    [XmlType("AnimalAid")]
    public class ImportAnmalAidsDto
    {
        public string Name { get; set; }
    }
}