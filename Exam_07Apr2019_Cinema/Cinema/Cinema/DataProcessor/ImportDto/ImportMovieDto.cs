using System;
using System.ComponentModel.DataAnnotations;
using Cinema.Data.Models.Enums;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportMovieDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string Title { get; set; }

        [Required] public string Genre { get; set; }

        [Required] public TimeSpan Duration { get; set; }

        [Required] [Range(1, 10)] public double Rating { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Director { get; set; }
    }
}