using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class ExportCustomerDto
    {
        [XmlAttribute] public string FirstName { get; set; }
        [XmlAttribute] public string LastName { get; set; }

        public string SpentMoney { get; set; }


        public string SpentTime { get; set; }

    }
}