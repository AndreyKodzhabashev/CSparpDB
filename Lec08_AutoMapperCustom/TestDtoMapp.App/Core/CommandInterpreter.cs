using System;
using System.Linq;
using System.Reflection;
using TestDtoMapp.App.Core.Commands.Contracts;


namespace TestDtoMapp.App.Core
{
    using Contracts;

    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Suffix = "Command";
        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider provider)
        {
            this.serviceProvider = provider;
        }

        public string Read(string[] args)
        {
            //type
            //ctor
            //ctorParams
            //with the provider create for every parameter in the ctorParams with Select we apply GetService - to create na instance of the necessary Class
            //invoke the ctor ( or Activator.CreateInstance) = creates an instance of the Command class
            //execute
            //return result
            var command = args[0];
            string[] parameters = args.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes().FirstOrDefault(x => x.Name == command);


            var constructor = type.GetConstructors().FirstOrDefault();

            var ctorParams = constructor.GetParameters().Select(x => x.ParameterType).ToArray();

            var services = ctorParams.Select(this.serviceProvider.GetService).ToArray();

            var comm = (ICommand) constructor.Invoke(services);


            var result = comm.Execute(parameters);

            return result;
        }
    }
}