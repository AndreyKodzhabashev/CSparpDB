using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("User")]
    public class ImportUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlElement("age")]
        public int Age { get; set; }
    //    <User>
    //<firstName>Etty</firstName>
    //<lastName>Haill</lastName>
    //<age>31</age>
    //</User>
    }
}