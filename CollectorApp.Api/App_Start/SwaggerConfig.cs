using System;
using System.Web.Http;
using CollectorApp.Api;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace CollectorApp.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "CollectorApp.Api");

                    c.RootUrl(req => req.RequestUri.GetLeftPart(UriPartial.Authority));
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("CollectorApp API - Swagger UI");
                });
        }
    }
}