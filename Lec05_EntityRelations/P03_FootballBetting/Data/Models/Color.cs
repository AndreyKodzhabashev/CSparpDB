namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class Color
    {
        [Key] public int ColorId { get; set; }

        [Required] public string Name { get; set; }

        [InverseProperty("PrimaryKitColor")] public ICollection<Team> PrimaryKitTeams { get; set; }

        [InverseProperty("SecondaryKitColor")] public ICollection<Team> SecondaryKitTeams { get; set; }

        public Color()
        {
            this.PrimaryKitTeams = new List<Team>();
            this.SecondaryKitTeams = new List<Team>();
        }
    }
}