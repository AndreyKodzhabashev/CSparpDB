using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; } //– integer, Primary Key
        [Required]
        public string Name { get; set; } // – text(required)
        [Range(typeof(decimal),"0", "79228162514264337593543950335")]
        public decimal Price { get; set; } //– decimal (non-negative, minimum value: 0) (required)
        [Required]
        public DateTime ReleaseDate { get; set; } // – Date(required)

        
        public virtual int DeveloperId { get; set; } // – integer, foreign key(required)
        [Required]
        public virtual Developer Developer { get; set; } //– the game’s developer(required)

        public virtual int GenreId { get; set; } // – integer, foreign key(required)
        [Required]
        public virtual Genre Genre { get; set; } // – the game’s genre(required)

        public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>(); // - collection of type Purchase

        [Required]
        [MinLength(1)]
        public virtual ICollection<GameTag> GameTags { get; set; } = new List<GameTag>(); // - collection of type GameTag.Each game must have at least one tag.
    }
}