using Sfc.Wms.Asrs.Api;
using Swashbuckle.Application;
using System.Web;
using System.Web.Http;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Sfc.Wms.Asrs.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            SwaggerWindowsAuthConfig.RegisterAuth();
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "WMS API Services");
                        c.UseFullTypeNameInSchemaIds();
                    })
                .EnableSwaggerUi();
        }
    }
}