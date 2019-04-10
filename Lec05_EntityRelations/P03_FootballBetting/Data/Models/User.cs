using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class User
    {
        [Key] public int UserId { get; set; }

        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }

        [Required] public string Email { get; set; }

        [Required] public string Name { get; set; }

        public decimal Balance { get; set; }

        //navig

        [InverseProperty("User")] public ICollection<Bet> Bets { get; set; }

        public User()
        {
            this.Bets = new List<Bet>();
        }
    }
}