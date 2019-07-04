using System.Web.Http;
using Sfc.App.Api.App_Start;

namespace Sfc.App.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
            DependencyConfig.Register();
            config.Routes.MapHttpRoute("DefaultApi","api/{controller}/{id}");
        }
    }
}