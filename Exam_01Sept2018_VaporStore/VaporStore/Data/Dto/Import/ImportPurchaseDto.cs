namespace VaporStore.Data.Dto.Import
{
    using Models.Attributes;
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;

    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")] public string Title { get; set; }
        [XmlElement("Type")] public string Type { get; set; }

        [XmlElement("Key")]
        [Required]
        [PurchaseKeyMatch]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string Key { get; set; }

        //which consists of 3 pairs of 4 uppercase Latin letters and digits, separated by dashes (ex. “ABCD-EFGH-1J3L”) (required)
        [XmlElement("Card")] public string Card { get; set; }
        [XmlElement("Date")] public string Date { get; set; }
    }
}