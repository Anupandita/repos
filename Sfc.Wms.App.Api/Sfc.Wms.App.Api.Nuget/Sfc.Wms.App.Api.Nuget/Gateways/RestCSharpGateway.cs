using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class RestCSharpGateway:RestClient,IRestCsharpClient
    {
        public RestCSharpGateway(string baseUrl):base(baseUrl)
        {

        }
    }
}
