using Covid.BLL.Service;
using Covid.DAL.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covid.Ingestion
{
    public static class Startup
    {        
        public static IServiceCollection ConfigureServices()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();            
            var services = new ServiceCollection();
            services.AddSingleton<IConfigurationRoot>((ConfigurationRoot)configuration);
            services.AddSingleton<ICovidBLService, CovidBLService>();
            services.AddSingleton<ICovidDataRepository, CovidDataRepository>();
            services.AddSingleton<EntryPoint>();
            return services;
        }

    }
}
