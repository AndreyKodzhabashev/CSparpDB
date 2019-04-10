using Newtonsoft.Json;

namespace VaporStore.Data.Dto.Import
{
    using System.Collections.Generic;
    using Models.Attributes;
    using System.ComponentModel.DataAnnotations;

    public class ImportUserDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        [Required] public string Email { get; set; }
        [Required] [Range(3, 103)] public int Age { get; set; }

        public ICollection<CardDto> Cards { get; set; } = new List<CardDto>();
    }
    public class CardDto
    {
        [Required]
        [CardNymberMatch]
        [RegularExpression("^[0-9]{4}\\s+[0-9]{4}\\s+[0-9]{4}\\s+[0-9]{4}$")]
        public string Number { get; set; }

        [JsonProperty(propertyName: "CVC")]
        [Required]
        [CvcValidation]
        [RegularExpression("^[0-9]{3}$")]
        public string Cvc { get; set; }

        [Required] public string Type { get; set; }
        //"Number": "1111 1111 1111 1111",
        //"CVC": "111",
        //"Type": "Debit"
    }
}