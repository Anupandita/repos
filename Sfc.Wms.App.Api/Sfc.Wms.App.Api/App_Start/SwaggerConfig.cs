using Swashbuckle.Application;
using System.Web.Http;
using WebActivatorEx;
using Wms.App.Api;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Wms.App.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Wms.App.Api");
                })
                .EnableSwaggerUi(c =>
                {
                    c.EnableApiKeySupport("Authorization", "header");
                });
        }
    }
}