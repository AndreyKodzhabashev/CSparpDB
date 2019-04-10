namespace TestDtoMapp.App.Core.Commands
{
    using Contracts;
    using System;
    using AutoMapper;
    using Data;
    using Models.DTOs;

    public class SetAddress : ICommand
    {
        private readonly TestDtoMappContext context;
        private readonly Mapper mapper;

        public SetAddress(TestDtoMappContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] args)
        {
            //•	SetAddress <employeeId> <address> –  sets the address of the employee to the given string
            var empId = int.Parse(args[0]);
            var address = args[1];

            var currEmplpoyee = context.Employees.Find(empId);

            if (currEmplpoyee == null)
            {
                throw new ArgumentException("No such person in the system!");
            }

            currEmplpoyee.Address = address;
            if (context.SaveChanges() == 0)
            {
                throw new OperationCanceledException("No changes has been made");
            }

            var empDto = mapper.CreateMappedObject<SetAddressDto>(currEmplpoyee);

            return $"Address {address} successfully added to person {empDto.FirstName} {empDto.LastName}";
        }
    }
}