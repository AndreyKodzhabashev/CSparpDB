using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        [Key] public int ProductId { get; set; }

        [Column(TypeName = "nvarchar(50)")] public string Name { get; set; } //(up to 50 characters, unicode)

        public double Quantity { get; set; } //(real number)
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(250)")] //, up to 250 symbols, Unicode
        public string Description { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}