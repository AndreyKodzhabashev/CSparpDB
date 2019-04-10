namespace TestDtoMapp.App.Core.Commands
{
    using Contracts;
    using System;

    public class Exit : ICommand
    {
        public string Execute(string[] args)
        {
            Console.WriteLine("App exit.Thanks for using.");
            
            Environment.Exit(0);

            return string.Empty;
        }
    }
}