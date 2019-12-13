using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class InventoryMaintenanceGateway : SfcBaseGateway, IInventoryMaintenanceGateway
    {
        private readonly ResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public InventoryMaintenanceGateway(IRestClient restClient, ResponseBuilder responseBuilder) : base(restClient)
        {
            _restClient = restClient;
            _responseBuilder = responseBuilder;
        }

        public async Task<BaseResult> CreateAsync(IvmtTriggerInputDto ivmtTriggerInput)
        {
            var request = new RestRequest(Routes.Prefixes.DematicMessageIvmt, Method.POST)
                .AddJsonBody(ivmtTriggerInput);
            var result = await _restClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return _responseBuilder.GetBaseResult(result);
        }
    }
}