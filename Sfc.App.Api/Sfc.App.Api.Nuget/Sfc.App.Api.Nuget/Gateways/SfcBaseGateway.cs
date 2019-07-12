using System;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.RestResponse;
using Sfc.Wms.Result;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class SfcBaseGateway : ResponseBuilder
    {
        protected readonly IRestClient RestClient;

        protected SfcBaseGateway(IRestClient client)
        {
            RestClient = client;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            RestClient.BaseUrl =new Uri($"{baseUrl}");
        }

        protected string GetQueryString(params string[] parameters)
        {
            return string.Join("/", parameters.Where(p => !string.IsNullOrEmpty(p)));
        }

        protected BaseResult ToBaseResult(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<BaseResult>(response.Content);
        }
    }
}