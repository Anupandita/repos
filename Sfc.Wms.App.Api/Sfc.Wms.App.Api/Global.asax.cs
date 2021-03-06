using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Sfc.Wms.App.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()

        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}