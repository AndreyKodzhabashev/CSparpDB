using System.Xml.Serialization;
using PetClinic.Models;

namespace PetClinic.Data.ModelDtos.Export
{
    [XmlType("Procedure")]
    public class ExportProcedureDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public ExportAnimalAidDto[] AnimalAids { get; set; }

        public string TotalPrice { get; set; }
    }
    [XmlType("AnimalAid")]
    public class ExportAnimalAidDto 
    {
        public string Name { get; set; }

        public string Price { get; set; }

    }
}