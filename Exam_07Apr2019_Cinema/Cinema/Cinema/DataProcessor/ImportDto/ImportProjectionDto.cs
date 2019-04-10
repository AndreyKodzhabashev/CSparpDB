using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ImportProjectionDto
    {
        [Required] [Range(1, int.MaxValue)] public int MovieId { get; set; }

        [Required] [Range(1, int.MaxValue)] public int HallId { get; set; }

        [Required] public string DateTime { get; set; }
    }
}