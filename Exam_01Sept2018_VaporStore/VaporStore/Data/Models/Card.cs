using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VaporStore.Data.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; } //– integer, Primary Key

        [Required]
        [RegularExpression("^[0-9]{4}\\s+[0-9]{4}\\s+[0-9]{4}\\s+[0-9]{4}$")]
        public string Number { get; set; }

        // – text, which consists of 4 pairs of 4 digits, separated by spaces(ex. “1234 5678 9012 3456”) (required)
        [Required]
        [RegularExpression("^[0-9]{3}$")]
        public string Cvc { get; set; } // – text, which consists of 3 digits(ex. “123”) (required)

        [Required]
        public CardType Type { get; set; }
        // – enumeration of type CardType, with possible values(“Debit”, “Credit”) (required)


        [Required]
        public virtual int UserId { get; set; } // – integer, foreign key(required)
        [Required]
        public virtual User User { get; set; } // – the card’s user(required)

        public virtual ICollection<Purchase> Purchases { get; set; } // – collection of type Purchase
    }
}