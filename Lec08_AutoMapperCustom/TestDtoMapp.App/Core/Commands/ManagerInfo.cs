namespace TestDtoMapp.App.Core.Commands
{
    using System.Text;
    using System.Linq;
    using AutoMapper;
    using Contracts;
    using Data;
    using Models.DTOs;
    using Microsoft.EntityFrameworkCore;


    public class ManagerInfo : ICommand
    {
        private readonly TestDtoMappContext context;
        private readonly Mapper mapper;

        public ManagerInfo(TestDtoMappContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            var managerId = int.Parse(args[0]);

            var manager = context.Employees.Include(x => x.ManagedEmployees)
                .FirstOrDefault(x => x.EmployeeId == managerId);

            var managerDto = mapper.CreateMappedObject<ManagerDto>(manager);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(
                $"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.ManagedEmployees.Count}");
            foreach (var emp in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"    - {emp.FirstName} {emp.LastName} - ${emp.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}