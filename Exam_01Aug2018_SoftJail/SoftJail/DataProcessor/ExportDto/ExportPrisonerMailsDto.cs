using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerMailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")] public ExportMailsDto[] EncryptedMessages { get; set; }
    }

    [XmlType("Message")]
    public class ExportMailsDto
    {
        public string Description { get; set; }
    }
}