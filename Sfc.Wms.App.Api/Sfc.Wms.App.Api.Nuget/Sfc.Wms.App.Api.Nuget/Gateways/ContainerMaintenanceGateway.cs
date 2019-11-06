using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Interfaces.Asrs.Contracts.Dtos;
using System.Threading.Tasks;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class ContainerMaintenanceGateway : SfcBaseGateway, IContainerMaintenanceGateway
    {
        private readonly IRestClient _restClient;
        private readonly ResponseBuilder _responseBuilder;

        public ContainerMaintenanceGateway(IRestClient restClient, ResponseBuilder responseBuilder) 
        {
            _restClient = restClient;
            _responseBuilder = responseBuilder;
        }
        public async Task<BaseResult> CreateAsync(ComtTriggerInputDto comtTriggerInput)
        {
            var request = new RestRequest($"{Routes.Prefixes.DematicMessageComt}", Method.POST)
                .AddJsonBody(comtTriggerInput);
            var result = await _restClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return _responseBuilder.GetBaseResult(result);
        }
    }
}