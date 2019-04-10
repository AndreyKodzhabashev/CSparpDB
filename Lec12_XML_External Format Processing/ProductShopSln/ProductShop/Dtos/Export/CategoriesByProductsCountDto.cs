using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Category")]
    public class CategoriesByProductsCountDto
    {
        [XmlElement("name")]
        public string Category { get; set; }

        [XmlElement("count")]
        public int ProductsCount { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }
        [XmlElement("totalRevenue")]
        public decimal TotalRevenue  { get; set; }
    }
}