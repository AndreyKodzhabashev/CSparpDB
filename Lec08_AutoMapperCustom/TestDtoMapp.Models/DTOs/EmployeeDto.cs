namespace TestDtoMapp.Models.DTOs
{
    public class EmployeeDto
    {
        //only id, first name, last name and salary).
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }
    }
}