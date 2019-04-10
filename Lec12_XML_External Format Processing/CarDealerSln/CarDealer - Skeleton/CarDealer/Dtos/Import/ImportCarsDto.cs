using System;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Car")]
    public class ImportCarsDto
    {
        [XmlElement("make")] public string Make { get; set; }
        [XmlElement("model")] public string Model { get; set; }
        [XmlElement("TraveledDistance")] public long TraveledDistance { get; set; }

        
        [XmlArray("parts")]
        public PartDto[] PartsDto { get; set; } = new PartDto[0];
    }

    [XmlType("partId")]
    public class PartDto
    {
        [XmlAttribute("id")] public int Id { get; set; }
    }
}