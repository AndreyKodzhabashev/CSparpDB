using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetClinic.Models
{
    public class Procedure
    {
        [Key] public int Id { get; set; }

        public int AnimalId { get; set; }

        public Animal Animal { get; set; }

        public int VetId { get; set; }
        public Vet Vet { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; } = new List<ProcedureAnimalAid>();

        public decimal Cost => ProcedureAnimalAids.Sum(x => x.AnimalAid.Price);

        [Required] public DateTime DateTime { get; set; }
    }
}