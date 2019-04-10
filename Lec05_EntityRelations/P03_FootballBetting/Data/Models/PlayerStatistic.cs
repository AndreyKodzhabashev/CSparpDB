using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }

        public int PlayerId { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("PlayerStatistics")]
        public Game Game { get; set; }

        [InverseProperty("PlayerStatistics")]
        public Player Player { get; set; }

        public int ScoredGoals { get; set; }

        public int Assists { get; set; }

        public int MinutesPlayed { get; set; }
    }
}