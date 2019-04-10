namespace TestDtoMapp.App.Core
{
    using Contracts;
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            while (true)
            {
                var interpreter = this.serviceProvider.GetService<ICommandInterpreter>();

                string[] input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
               
                try
                {
                    var result = interpreter.Read(input);
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}