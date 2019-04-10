namespace BillsPaymentSystem.App.Core
{
    using System;
    using Contracts;

    public class Engine : IEngine
    {
        private readonly ICommandInterpreter interpreter;
        private readonly IDisposable context;

        public Engine(ICommandInterpreter interpreter, IDisposable context)
        {
            this.interpreter = interpreter;
            this.context = context;
        }

        public void Run()
        {
            while (true)
            {
                string[] inputParams = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);
                using (context)
                {
                    string result = interpreter.ReadCommand(inputParams, context);
                    Console.WriteLine(result);
                    //try
                    //{

                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e.Message);
                    //}
                }
            }
        }
    }
}