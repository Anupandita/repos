using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Sfc.Wms.App.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new SlidingExpirationHandler());
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
            DependencyConfig.Register();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}