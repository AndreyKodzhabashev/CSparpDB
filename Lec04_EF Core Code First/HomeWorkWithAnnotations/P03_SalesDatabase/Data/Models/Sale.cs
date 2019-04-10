using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    public class Sale
    {
        [Key] public int SaleId { get; set; }
        public DateTime Date { get; set; }

        //navigation for Product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        //navigation for Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        //navigation for Store
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}