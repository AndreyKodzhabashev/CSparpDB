namespace TestDtoMapp.App.Core.Commands
{
    using System;
    using AutoMapper;
    using Contracts;
    using Data;
    using Models.DTOs;
    using System.Text;

    public class EmployeePersonalInfo : ICommand
    {
        private readonly TestDtoMappContext context;
        private readonly Mapper mapper;

        public EmployeePersonalInfo(TestDtoMappContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //    •	EmployeePersonalInfo<employeeId> – prints all the information for an employee in the following format:
        //ID: 1 - Pesho Ivanov - $1000.00
        //Birthday: 15-04-1976
        //Address: Sofia, ul.Vitosha 15
        public string Execute(string[] args)
        {
            var empId = int.Parse(args[0]);

            var currEmployee = context.Employees.Find(empId);

            if (currEmployee == null)
            {
                throw new ArgumentException("No such person in system.");
            }

            var empDto = mapper.CreateMappedObject<EmployeePersonalInfoDto>(currEmployee);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"ID: {empDto.EmployeeId} - {empDto.FirstName} {empDto.LastName} - ${empDto.Salary}");
            sb.AppendLine($"Birthday: {empDto.Birthday}");
            sb.AppendLine($"Address: {empDto.Address}");

            return sb.ToString().TrimEnd();
        }
    }
}