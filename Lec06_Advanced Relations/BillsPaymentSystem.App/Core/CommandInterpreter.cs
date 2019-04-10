namespace BillsPaymentSystem.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using BillsPaymentSystem.App.Core.Commands.Contracts;

    using Contracts;
    using Data;

    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Sufix = "Command";

        public string ReadCommand(string[] args, IDisposable context)
        {
            string command = args[0];
            string[] commandArgs = args.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes().FirstOrDefault(x => x.Name == command + Sufix);

            if (type == null)
            {
                throw new ArgumentNullException("Command not found");
            }

            var typeInstance = Activator.CreateInstance(type, context);

            var result = ((ICommand) typeInstance).Execute(commandArgs);

            return result;
        }
    }
}