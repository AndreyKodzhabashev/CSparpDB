using System;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        [Key] public int Id { get; set; } //– integer, Primary Key

        [Required] public PurchaseType Type { get; set; }// – enumeration of type PurchaseType, with possible values(“Retail”, “Digital”) (required) 

        [Required]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string ProductKey { get; set; }//– text, which consists of 3 pairs of 4 uppercase Latin letters and digits, separated by dashes(ex. “ABCD-EFGH-1J3L”) (required)

        [Required] public DateTime Date { get; set; } //– Date(required)
        [Required] public virtual int CardId { get; set; } // – integer, foreign key(required)


        [Required] public virtual Card Card { get; set; } // – the purchase’s card(required)

        [Required] public virtual int GameId { get; set; } // – integer, foreign key(required)
        public virtual Game Game { get; set; } //– the purchase’s game(required)
    }
}