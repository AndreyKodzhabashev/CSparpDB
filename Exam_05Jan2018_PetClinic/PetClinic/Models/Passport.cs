using System;
using System.ComponentModel.DataAnnotations;
using PetClinic.Models.Attributes;

namespace PetClinic.Models
{
    public class Passport
    {
        [Key]
        [RegularExpression(@"^[A-Za-z]{7}[0-9]{3}$")]
        public string SerialNumber { get; set; }

        public Animal Animal { get; set; }

        [Required]
        //[PhoneNumber]
        [RegularExpression(@"^0[0-9]{9}$|^\+359[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string OwnerName { get; set; }

        [Required] public DateTime RegistrationDate { get; set; }
    }
}