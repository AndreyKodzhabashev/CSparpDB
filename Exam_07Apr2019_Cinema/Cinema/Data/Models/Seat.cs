using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Seat : BaseModel
    {
        public int HallId { get; set; }

        [Required] public Hall Hall { get; set; }
    }
}