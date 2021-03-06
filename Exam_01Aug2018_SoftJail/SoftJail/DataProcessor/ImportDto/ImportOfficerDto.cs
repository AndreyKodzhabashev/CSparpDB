﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using SoftJail.Data.Models.Enums;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name  { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Money { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Weapon { get; set; }

        [Required]
        public int  DepartmentId { get; set; }
        [XmlArray("Prisoners")]
        public ImportPrisonerXmlDto[] Prisoners { get; set; }
    }

    [XmlType("Prisoner")]
    public class ImportPrisonerXmlDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}