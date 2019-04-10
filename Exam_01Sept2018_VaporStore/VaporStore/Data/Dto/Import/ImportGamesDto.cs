namespace VaporStore.Data.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportGamesDto
    {
        //Name": "Dota 2",
        //"Price": 0,
        //"ReleaseDate": "2013-07-09",
        //"Developer": "Valve",
        //"Genre": "Action",
        //"Tags": [
        [Required] public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required] public string ReleaseDate { get; set; }
        [Required] public string Developer { get; set; }
        [Required] public string Genre { get; set; }
        [MinLength(1)] public string[] Tags { get; set; } = new string[0];
    }
}