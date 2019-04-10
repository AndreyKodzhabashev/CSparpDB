using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        //    •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FullName – text with min length 3 and max length 20 (required)
        [Required]
     [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }
   
    //•	Nickname – text starting with "The " and a single word only of letters with an uppercase letter for beginning(example: The Prisoner) (required)
    [Required]
    [RegularExpression(@"^The [A-Z][a-z]+$")]
    public string Nickname { get; set; }
    //•	Age – integer in the range[18, 65] (required)
    [Required]
    [Range(18,65)]
    public int Age { get; set; }
    //•	IncarcerationDate ¬– Date(required)
    [Required]
    public DateTime IncarcerationDate { get; set; }
    //•	ReleaseDate– Date
    public DateTime? ReleaseDate { get; set; }
    //•	Bail– decimal (non-negative, minimum value: 0)
    [Range(typeof(decimal),"0", "79228162514264337593543950335")]
   public decimal? Bail { get; set; }

    //•	CellId - integer, foreign key
    public int? CellId { get; set; }

    //•	Cell – the prisoner's cell
    public Cell Cell { get; set; }

    //•	Mails - collection of type Mail
    public ICollection<Mail> Mails { get; set; } =new List<Mail>();
    //•	PrisonerOfficers - collection of type OfficerPrisoner
    public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } = new List<OfficerPrisoner>();

    }
}