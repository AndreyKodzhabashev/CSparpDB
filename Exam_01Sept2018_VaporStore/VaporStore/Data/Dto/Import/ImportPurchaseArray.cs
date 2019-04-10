namespace VaporStore.Data.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("Purchases")]
    public class ImportPurchaseArray
    {
        public ImportPurchaseDto[] Purchases { get; set; }
    }
}