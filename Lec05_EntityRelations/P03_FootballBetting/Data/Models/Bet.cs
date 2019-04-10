namespace P03_FootballBetting.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Bet
    {
        [Key] public int BetId { get; set; }

        [Required] public decimal Amount { get; set; }

        [Required] public PredictionType Prediction { get; set; }

        [Required] public DateTime DateTime { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Bets")]
        public User User { get; set; }


        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("Bets")]
        public Game Game { get; set; }
    }
}