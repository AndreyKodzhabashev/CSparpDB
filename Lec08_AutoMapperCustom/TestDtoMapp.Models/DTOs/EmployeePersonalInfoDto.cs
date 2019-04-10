namespace TestDtoMapp.Models.DTOs
{
    using System;

    public class EmployeePersonalInfoDto
    {
        //only id, first name, last name and salary).
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }
    }
}