namespace BillsPaymentSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BankAccount
    {
        public int BankAccountId { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Balance { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string BankName { get; set; } //(up to 50 characters, unicode)

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string SWIFTCode { get; set; } //(up to 20 characters, non-unicode)

        //navig
        public PaymentMethod PaymentMethod { get; set; }
    }
}