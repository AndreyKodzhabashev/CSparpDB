using System.Xml.Serialization;
using ProductShop.Models;

namespace ProductShop.Dtos.Export
{
    [XmlType("Product")]
    public class ExportProductsDto
    {
        [XmlElement("name")]
        public string Name{ get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
        [XmlElement("buyer")]
        public string Buyer { get; set; }
    }
}