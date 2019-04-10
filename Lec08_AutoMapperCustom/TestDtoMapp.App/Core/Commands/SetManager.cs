namespace TestDtoMapp.App.Core.Commands
{
    using System;
    using Contracts;
    using Data;

    public class SetManager : ICommand
    {
        private readonly TestDtoMappContext context;

        public SetManager(TestDtoMappContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            var empId = int.Parse(args[0]);
            var managerId = int.Parse(args[1]);

            var employee = context.Employees.Find(empId);
            var manager = context.Employees.Find(managerId);

            if (employee == null)
            {
                throw new ArgumentException("No such person in system.");
            }

            employee.Manager = manager;

            context.SaveChanges();

            return "Manager added";
        }
    }
}