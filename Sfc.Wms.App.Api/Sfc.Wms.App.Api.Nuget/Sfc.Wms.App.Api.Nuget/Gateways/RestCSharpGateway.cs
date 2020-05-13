using RestSharp;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class RestCSharpGateway : RestClient
    {
        public RestCSharpGateway(string baseUrl) : base(baseUrl)
        {

        }
    }
}
