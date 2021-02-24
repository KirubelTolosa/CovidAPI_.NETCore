using Autofac;

namespace Covid.DAL.Service
{
    public static class DALDIModule
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ICovidDataRepository>().As<CovidDataRepository>();            
        }
    }
}
