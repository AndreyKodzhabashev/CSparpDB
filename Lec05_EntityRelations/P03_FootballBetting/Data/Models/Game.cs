using System;
using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Game
    {
        [Key] public int GameId { get; set; }

        public int HomeTeamId { get; set; }

        [ForeignKey(nameof(HomeTeamId))]
        [InverseProperty("HomeGames")]
        public Team HomeTeam { get; set; }


        public int AwayTeamId { get; set; }

        [ForeignKey(nameof(AwayTeamId))]
        [InverseProperty("AwayGames")]
        public Team AwayTeam { get; set; }


        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public DateTime DateTime { get; set; }

        public double HomeTeamBetRate { get; set; }

        public double AwayTeamBetRate { get; set; }

        public double DrawBetRate { get; set; }

        public string Result { get; private set; }

        [InverseProperty("Game")] public ICollection<PlayerStatistic> PlayerStatistics { get; set; }

        [InverseProperty("Game")] public ICollection<Bet> Bets { get; set; }

        public Game()
        {
            this.PlayerStatistics = new List<PlayerStatistic>();
            this.Bets = new List<Bet>();
            this.Result = $"{this.HomeTeamGoals}:{this.AwayTeamGoals}";
        }
    }
}