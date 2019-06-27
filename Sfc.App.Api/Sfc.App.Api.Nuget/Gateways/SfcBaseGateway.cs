using System;
using System.Configuration;
using System.Linq;
using RestSharp;
using Sfc.Wms.RestResponse;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class SfcBaseGateway : ResponseBuilder
    {
        protected readonly IRestClient RestClient;

        protected SfcBaseGateway(IRestClient restClient, string path)
        {
            RestClient = restClient;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var uriBuilder = new UriBuilder
            {
                Host = baseUrl,
                Path = path
            };
#if DEBUG
            if (int.TryParse(ConfigurationManager.AppSettings["Port"], out var port))
                uriBuilder.Port = port;
#endif

            RestClient.BaseUrl = uriBuilder.Uri;
        }

        protected string GetQueryString(params string[] parameters)
        {
            return string.Join("/", parameters.Where(p => !string.IsNullOrEmpty(p)));
        }
    }
}