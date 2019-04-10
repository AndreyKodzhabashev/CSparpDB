namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Position
    {
        [Key] public int PositionId { get; set; }

        [Required] public string Name { get; set; }

        [InverseProperty("Position")] public ICollection<Player> Players { get; set; }

        public Position()
        {
            this.Players = new List<Player>();
        }
    }
}