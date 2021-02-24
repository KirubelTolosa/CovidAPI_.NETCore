using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Covid.Ingestion
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<EntryPoint>().Run(args);
        }
    }
}
