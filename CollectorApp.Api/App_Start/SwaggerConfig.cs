using System.Web.Http;
using WebActivatorEx;
using CollectorApp.Api;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace CollectorApp.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var httpConfiguration = GlobalConfiguration.Configuration;

            httpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "CollectorApp API");
                    c.IncludeXmlComments(GetXmlCommentsPath());

                    c.ApiKey("apiKey")
                        .Description("Wpisz: Bearer {Twój_Token}")
                        .Name("Authorization")
                        .In("header");
                })
                .EnableSwaggerUi(c =>
                {
                    c.EnableApiKeySupport("Authorization", "header");
                });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\CollectorApp.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}