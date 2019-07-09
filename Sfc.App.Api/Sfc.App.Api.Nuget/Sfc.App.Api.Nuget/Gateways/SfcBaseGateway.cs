using System;
using System.Configuration;
using System.Linq;
using RestSharp;
using Sfc.Core.RestResponse;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class SfcBaseGateway : ResponseBuilder
    {
        protected readonly IRestClient RestClient;

        protected SfcBaseGateway(IRestClient restClient, string path)
        {
            RestClient = restClient;
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            RestClient.BaseUrl =new Uri($"{baseUrl}/{path}");
        }

        protected string GetQueryString(params string[] parameters)
        {
            return string.Join("/", parameters.Where(p => !string.IsNullOrEmpty(p)));
        }
    }
}