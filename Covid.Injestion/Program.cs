using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Covid.BLL.Service;
using Covid.DAL.Service;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Covid.BLL.Service.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Covid.Injestion
{
    class Program
    {
       private static IServiceProvider _serviceProvider;
        
        
        #region
        private static void RegisterServices()
        {
            var path = Directory.GetCurrentDirectory();
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("path/appsettings.json").Build();
            var services = new ServiceCollection();
            services.AddSingleton<ICovidBLService, CovidBLService>();
            services.AddSingleton<ICovidDataRepository, CovidDataRepository>();
            
            services.AddSingleton<Application>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
        static void Main(string[] args)
        {
            RegisterServices();

            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<Application>().Run();

            DisposeServices();

        }
        #endregion
    }
}
