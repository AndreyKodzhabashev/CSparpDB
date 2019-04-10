using System.ComponentModel.DataAnnotations;

namespace PetClinic.Data.ModelDtos.Import
{
    public class ImportAnimalDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Type { get; set; }

        [Required]
        [Range(1, 2147000000)]
        public int Age { get; set; }

        public ImportPassportDto Passport { get; set; }
    }

    public class ImportPassportDto
    {

        [RegularExpression(@"^[A-Za-z]{7}[0-9]{3}$")]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string OwnerName { get; set; }

        [Required]
        //[PhoneNumber]
        [RegularExpression(@"^0[0-9]{9}$|^\+359[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
}