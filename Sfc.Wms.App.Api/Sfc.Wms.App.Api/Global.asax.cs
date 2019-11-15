using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Sfc.Wms.App.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
#pragma warning disable CA1822 // Mark members as static
        protected void Application_Start()
#pragma warning restore CA1822 // Mark members as static
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}