using Microsoft.Extensions.Logging;

namespace BillsPaymentSystem.App
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    using Data;
    using Core;
    using Core.Contracts;


    public class StartUp
    {
        public static void Main()
        {
            using (var context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                DbInitializer.Seed(context);

                var test = ConfigureServices();
                IEngine engine = ConfigureServices().GetService<IEngine>();
                engine.Run();
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IEngine, Engine>()
                .AddTransient<ICommandInterpreter, CommandInterpreter>()
                .AddDbContext<IDisposable, BillsPaymentSystemContext>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}