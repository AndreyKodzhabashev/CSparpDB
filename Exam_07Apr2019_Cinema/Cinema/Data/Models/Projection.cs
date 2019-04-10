using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Projection : BaseModel
    {
        public int MovieId { get; set; }

        [Required] public Movie Movie { get; set; }

        public int HallId { get; set; }

        [Required] public Hall Hall { get; set; }

        [Required] public DateTime DateTime { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}