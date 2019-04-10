using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Hall : BaseModel   
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public bool Is4Dx { get; set; }

        [Required]
        public bool Is3D { get; set; }


        public ICollection<Projection> Projections { get; set; } = new List<Projection>();

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}