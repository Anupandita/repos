using RestSharp;
using Sfc.App.Api.Nuget.Interfaces;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Gateways;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class EmsToWmsMessageGateway : SfcBaseGateway, IEmsToWmsMessageGateway
    {
        private readonly IRestClient _restClient;
        private readonly ResponseBuilder _responseBuilder;

        public EmsToWmsMessageGateway(IRestClient restClient, ResponseBuilder responseBuilder) : base()
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