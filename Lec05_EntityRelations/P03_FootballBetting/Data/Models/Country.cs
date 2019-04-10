using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        [Key] public int CountryId { get; set; }

        [Required] public string Name { get; set; }

        [InverseProperty("Country")] public ICollection<Town> Towns { get; set; }
    }
}