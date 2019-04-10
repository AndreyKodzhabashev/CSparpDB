namespace TestDtoMapp.Models.Models
{
    using System;
    using System.Collections.Generic;

    public class Employee
    {
        //: first name, last name, salary, birthday and address. Only first name, last name and salary are required.
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        //nav
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public List<Employee> ManagedEmployees { get; set; } = new List<Employee>();
    }
}