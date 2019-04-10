using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        [Key] public int StoreId { get; set; }
        [Column(TypeName = "nvarchar(80)")] public string Name { get; set; } //(up to 80 characters, unicode)

       ICollection<Sale> Sales { get; set; }
    }
}