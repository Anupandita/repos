using Sfc.Wms.Asrs.Api.App_Start;
using System.Web.Http;

namespace Sfc.Wms.Asrs.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            DependencyConfig.Register();
            SwaggerWindowsAuthConfig.RegisterAuth();
            config.MapHttpAttributeRoutes();
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}