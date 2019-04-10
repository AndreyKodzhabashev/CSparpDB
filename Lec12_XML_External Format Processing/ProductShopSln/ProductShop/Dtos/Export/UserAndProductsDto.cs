using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class UserAndProductsDto
    {
        [XmlElement("count")]
        public int Count  { get; set; }

        [XmlArray("users")]
        public UserProductsDto[] UsersProducts { get; set; }
    }
    [XmlType("User")]
    public class UserProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }
        
        public SoldProdDto SoldProducts { get; set; }
    }
    
    public class SoldProdDto
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("products")]
        public ProdsDto[] Products { get; set; }
    }
    [XmlType("Product")]
    public class ProdsDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }  
    }
}