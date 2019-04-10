

namespace TestDtoMapp.App.Core.Commands
{
    using AutoMapper;
    using Data;
    using Contracts;
    using Models.DTOs;
    using TestDtoMapp.Models.Models;


    public class AddEmployee : ICommand
    {
        private readonly TestDtoMappContext context;
        private readonly Mapper mapper;

        public AddEmployee(TestDtoMappContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            //•	AddEmployee <firstName> <lastName> <salary> –  adds a new Employee to the database

            string firstName = args[0];
            string lastName = args[1];
            decimal salary = decimal.Parse(args[2]);

            //TODO validate

            var emp = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            var empDto = mapper.CreateMappedObject<EmployeeDto>(emp);

            context.Employees.Add(emp);
            context.SaveChanges();

            return $"Registered {empDto.FirstName} {empDto.LastName} with Salary ${empDto.Salary}.";
        }
    }
}