using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        //    •	Id – integer, Primary Key
        [Key] public int Id { get; set; }

        //•	Description– text(required)
        [Required] public string Description { get; set; }

        //•	Sender – text(required)
        [Required] public string Sender { get; set; }

        //•	Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required) (Example: “62 Muir Hill str.“)
        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s]+ str.$")]
        public string Address { get; set; }

        //•	PrisonerId - integer, foreign key
        public int PrisonerId { get; set; }

        //•	Prisoner – the mail's Prisoner (required)
        [Required] public Prisoner Prisoner { get; set; }
    }
}