namespace TestDtoMapp.App.Core.Commands
{
    using System;
    using Contracts;
    using AutoMapper;
    using Data;
    using Models.DTOs;

    public class EmployeeInfo : ICommand
    {
        private readonly TestDtoMappContext context;
        private readonly Mapper mapper;

        public EmployeeInfo(TestDtoMappContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            var empId = int.Parse(args[0]);

            var emp = context.Employees.Find(empId);

            if (emp == null)
            {
                throw new ArgumentNullException("No such employee in the system!");
            }

            var empDto = mapper.CreateMappedObject<EmployeeDto>(emp);
            //•	EmployeeInfo <employeeId> – prints on the console the information for an employee in the format "ID: {employeeId} - {firstName} {lastName} -  ${salary:f2}"
            return $"{empDto.EmployeeId} - {empDto.FirstName} - {empDto.LastName} - ${empDto.Salary}";
        }
    }
}