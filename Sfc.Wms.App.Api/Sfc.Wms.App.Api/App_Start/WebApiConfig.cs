using Newtonsoft.Json.Serialization;
using Sfc.Wms.App.Api.DelegatingHandlers;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using System.Web.Http;
using Sfc.Core.Aop.WebApi.Logging;

namespace Sfc.Wms.App.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new SlidingExpirationHandler());
            var container = DependencyConfig.Register();
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters, container.GetInstance<SfcLogger>());
            FilterConfig.RegisterDbInterceptor(container.GetInstance<QueryLogger>());
            SfcMapper.Initialize();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}