namespace BillsPaymentSystem.Models
{
    using Attributes;
    using Enums;

    public class PaymentMethod
    {
        public int Id { get; set; } // - PK
        public PaymentType PaymentType { get; set; } //enum (BankAccount, CreditCard)

        //navig
        public int UserId { get; set; }
        public User User { get; set; }

        //navig
        [Xor(nameof(CreditCardId))] public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        //navig
        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}