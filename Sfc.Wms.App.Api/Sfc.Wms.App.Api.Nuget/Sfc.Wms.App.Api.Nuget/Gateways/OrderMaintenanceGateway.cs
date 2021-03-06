using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class OrderMaintenanceGateway : SfcBaseGateway, IOrderMaintenanceGateway
    {
        private readonly ResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public OrderMaintenanceGateway(IRestClient restClient, ResponseBuilder responseBuilder) : base(restClient)
        {
            _restClient = restClient;
            _responseBuilder = responseBuilder;
        }

        public async Task<BaseResult> CreateOrmtMessageByCartonNumberAsync(string cartonNumber, string actionCode)
        {
            var request = new RestRequest($"{Routes.Prefixes.DematicMessageOrmt}/{Routes.Paths.OrmtByCartonNumber}",
                Method.POST);
            request.AddParameter(nameof(cartonNumber), cartonNumber);
            request.AddParameter(nameof(actionCode), actionCode);
            var result = await _restClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return _responseBuilder.GetBaseResult(result);
        }

        public async Task<BaseResult> CreateOrmtMessageByWaveNumberAsync(string waveNumber)
        {
            var request = new RestRequest($"{Routes.Prefixes.DematicMessageOrmt}/{Routes.Paths.OrmtByWaveNumber}",
                Method.POST);
            request.AddParameter(nameof(waveNumber), waveNumber);
            var result = await _restClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return _responseBuilder.GetBaseResult(result);
        }
    }
}