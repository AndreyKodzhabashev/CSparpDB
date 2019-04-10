namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team
    {
        [Key] public int TeamId { get; set; }

        [Required] public string Name { get; set; }

        public string LogoUrl { get; set; }

        [Column(TypeName = "nchar(3)")] public string Initials { get; set; }

        public decimal Budget { get; set; }

        //navig
        public int PrimaryKitColorId { get; set; }

        [ForeignKey(nameof(PrimaryKitColorId))]
        [InverseProperty("PrimaryKitTeams")]
        public Color PrimaryKitColor { get; set; }

        public int SecondaryKitColorId { get; set; }

        [ForeignKey(nameof(SecondaryKitColorId))]
        [InverseProperty("SecondaryKitTeams")]
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        [InverseProperty("Teams")]
        public Town Town { get; set; }

        [InverseProperty("Team")] public ICollection<Player> Players { get; set; }

        [InverseProperty("HomeTeam")] public ICollection<Game> HomeGames { get; set; }

        [InverseProperty("AwayTeam")] public ICollection<Game> AwayGames { get; set; }

        public Team()
        {
            this.Players = new List<Player>();
            this.HomeGames = new List<Game>();
            this.AwayGames = new List<Game>();
        }
    }
}