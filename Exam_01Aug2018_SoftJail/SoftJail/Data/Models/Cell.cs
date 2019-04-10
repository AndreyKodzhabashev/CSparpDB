using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Cell
    {
    //    •	Id – integer, Primary Key
    [Key]
    public int Id { get; set; }
    //•	CellNumber – integer in the range[1, 1000] (required)
    [Required]
    [Range(1,1000)]
    public int CellNumber { get; set; }
    //•	HasWindow – bool (required)
    [Required]
    public bool HasWindow { get; set; }
    //•	DepartmentId - integer, foreign key
    public int DepartmentId { get; set; }
    //•	Department – the cell's department (required)
    
    public Department Department { get; set; }  

    //•	Prisoners - collection of type Prisoner
    public ICollection<Prisoner> Prisoners { get; set; } = new List<Prisoner>();
    }
}