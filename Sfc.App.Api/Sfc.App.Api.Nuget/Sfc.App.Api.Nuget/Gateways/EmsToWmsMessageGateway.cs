using RestSharp;
using Sfc.App.Api.Nuget.Interfaces;
using Sfc.Wms.Interface.Asrs.Constants;
using Sfc.Wms.Result;
using System.Threading.Tasks;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class EmsToWmsMessageGateway : SfcBaseGateway, IEmsToWmsMessageGateway
    {
        public EmsToWmsMessageGateway(IRestClient restClient) : base(restClient)
        {
        }

        public async Task<BaseResult> CreateAsync(long msgKey)
        {
            var request = new RestRequest($"{Routes.EmsToWmsMessagePrefix}", Method.POST).AddJsonBody(msgKey);
            var result = await RestClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return ToBaseResult(result);
        }
    }
}