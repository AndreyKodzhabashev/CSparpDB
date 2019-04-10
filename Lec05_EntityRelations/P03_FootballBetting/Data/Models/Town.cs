namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Town
    {
        [Key] public int TownId { get; set; }

        [Required] public string Name { get; set; }

        [InverseProperty("Town")] public ICollection<Team> Teams { get; set; }

        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty("Towns")]
        public Country Country { get; set; }

        public Town()
        {
            this.Teams = new List<Team>();
        }
    }
}