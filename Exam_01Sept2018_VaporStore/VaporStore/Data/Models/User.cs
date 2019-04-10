using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class User
    {
        [Key] public int Id { get; set; } //– integer, Primary Key

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; } // – text with length[3, 20] (required)

        [Required]
        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; } //– text, which has two words, consisting of Latin letters.
        //Both start with an upper letter and are separated by a single space(ex. "John Smith") (required)

        [Required] public string Email { get; set; } // – text(required)

        [Required] [Range(3, 103)] public int Age { get; set; } //– integer in the range[3, 103] (required)
        public virtual ICollection<Card> Cards { get; set; } // – collection of type Card
    }
}