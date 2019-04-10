using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        //    •	PrisonerId – integer, Primary Key
        public int PrisonerId { get; set; }

        //•	Prisoner – the officer’s prisoner(required)
        public Prisoner Prisoner { get; set; }


        //•	OfficerId – integer, Primary Key
         public int OfficerId { get; set; }

        //•	Officer – the prisoner’s officer(required)
        public Officer Officer { get; set; }
    }
}