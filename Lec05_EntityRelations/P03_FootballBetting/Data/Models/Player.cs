namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Player
    {
        [Key] public int PlayerId { get; set; }

        [Required] public string Name { get; set; }

        public int SquadNumber { get; set; }

        public bool IsInjured { get; set; }


        //navig
        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        [InverseProperty("Players")]
        public Team Team { get; set; }

        public int PositionId { get; set; }
        [ForeignKey(nameof(PositionId))]
        [InverseProperty("Players")]
        public Position Position { get; set; }

        [InverseProperty("Player")]
        public ICollection<PlayerStatistic> PlayerStatistics { get; set; }

        public Player()
        {
            this.PlayerStatistics = new List<PlayerStatistic>();
        }
    }
}