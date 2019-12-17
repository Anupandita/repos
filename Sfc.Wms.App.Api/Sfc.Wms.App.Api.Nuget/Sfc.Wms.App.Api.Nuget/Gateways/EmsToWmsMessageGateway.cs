using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class EmsToWmsMessageGateway : SfcBaseGateway, IEmsToWmsMessageGateway
    {
        private readonly ResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public EmsToWmsMessageGateway(IRestClient restClient, ResponseBuilder responseBuilder) : base(restClient)
        {
            _restClient = restClient;
            _responseBuilder = responseBuilder;
        }

        public async Task<BaseResult> CreateAsync(long msgKey)
        {
            var request = new RestRequest(Routes.Prefixes.EmsToWmsMessage, Method.POST).AddJsonBody(msgKey);
            var result = await _restClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return _responseBuilder.GetBaseResult(result);
        }
    }
}