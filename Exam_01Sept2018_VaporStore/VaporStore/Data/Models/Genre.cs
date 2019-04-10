using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VaporStore.Data.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; } // – integer, Primary Key
        [Required]
        public string Name { get; set; } //– text(required)
        public virtual ICollection<Game> Games { get; set; } = new List<Game>(); //- collection of type ExportGame
    }
}