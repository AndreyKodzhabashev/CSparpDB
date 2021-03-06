﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;

    public class Color
    {
        //DONE
        public Color()
        {
            this.PrimaryKitTeams = new List<Team>();
            this.SecondaryKitTeams = new List<Team>();
        }
        
        public int ColorId { get; set; }

        public string Name { get; set; }

        public ICollection<Team> PrimaryKitTeams { get; set; }
        public ICollection<Team> SecondaryKitTeams { get; set; }
    }
}