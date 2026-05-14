using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CollectorApp.Api.Infrastructure;
using CollectorApp.Api.Interfaces;
using CollectorApp.Api.Mappings;
using CollectorApp.Api.Services;
using Serilog;

namespace CollectorApp.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();


            // 1. Rejestracja kontrolerów Web API
            SerilogConfig.Configure();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // 2. Rejestracja naszych serwisów
            builder.RegisterType<BarcodeService>().As<IBarcodeService>().InstancePerRequest();
            builder.RegisterType<SubiektGTService>().AsSelf().InstancePerRequest();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerRequest();
            // 3. Konfiguracja AutoMappera
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            builder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>().SingleInstance();

            // 4. Budowanie kontenera
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Standardowe konfiguracje Web API
            GlobalConfiguration.Configure(WebApiConfig.Register);

            builder.RegisterType<SubiektGTService>().AsSelf().InstancePerRequest();
        }

        protected void Application_End()
        {
            Log.CloseAndFlush();
        }
    }
}