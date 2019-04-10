using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        [Key] public int CustomerId { get; set; }
        [Column(TypeName = "nvarchar(100)")] public string Name { get; set; } //(up to 100 characters, unicode)
        [Column(TypeName = "varchar(80)")] public string Email { get; set; } //(up to 80 characters, not unicode)
        public string CreditCardNumber { get; set; } //(string)

       public ICollection<Sale> Sales { get; set; }
    }
}