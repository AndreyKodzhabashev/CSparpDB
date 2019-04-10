using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class BaseModel
    {
        [Key] public int Id { get; set; }
    }
}