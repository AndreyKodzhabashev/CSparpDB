﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using PetClinic.Models.Attributes;

namespace PetClinic.Models
{
    public class Vet
    {
        [Key] public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Profession { get; set; }


        [Required] [Range(22, 65)] public int Age { get; set; }

        [Required]
       // [PhoneNumber]
        [RegularExpression(@"^0[0-9]{9}$|^\+359[0-9]{9}$")]
        public string PhoneNumber { get; set; }

        public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
    }
}