using System.Web.Http;
using WebActivatorEx;
using Sfc.App.Api;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Sfc.App.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Sfc.App.Api");
                        c.ApiKey("Token")
                            .Description("Filling bearer token here")
                            .Name("Authorization")
                            .In("header");
                    })
                .EnableSwaggerUi(c =>
                {
                    c.EnableApiKeySupport("Authorization", "header");
                });
        }
    }
}
