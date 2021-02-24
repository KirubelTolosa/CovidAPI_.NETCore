using Autofac;
using Covid.DAL.Service;
namespace Covid.BLL.Service
{ 
    public static class BLDIModule
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ICovidBLService>().As<CovidBLService>();
            builder.RegisterType<ICovidDataRepository>().As<CovidDataRepository>();
            DALDIModule.RegisterServices(builder);            
        }
    }
}
