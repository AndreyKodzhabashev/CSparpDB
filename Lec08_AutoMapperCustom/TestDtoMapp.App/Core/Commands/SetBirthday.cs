namespace TestDtoMapp.App.Core.Commands
{
    using System;
    using Data;
    using Contracts;

    public class SetBirthday : ICommand
    {
        private readonly TestDtoMappContext context;

        public SetBirthday(TestDtoMappContext context)
        {
            this.context = context;
        }

        //•	SetBirthday <employeeId> <date: "dd-MM-yyyy"> – sets the birthday of the employee to the given date
        public string Execute(string[] args)
        {
            var empId = int.Parse(args[0]);
            DateTime loadedDate = DateTime.ParseExact(args[1], "dd-MM-yyyy", null);

            var currEmplpoyee = context.Employees.Find(empId);

            if (currEmplpoyee == null)
            {
                throw new ArgumentException("No such person in the system!");
            }

            currEmplpoyee.Birthday = loadedDate;
            context.SaveChanges();
            return
                $"Birthday date {loadedDate} successfully added to person {currEmplpoyee.FirstName} {currEmplpoyee.LastName}";
        }
    }
}