using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CorbaGateway : SfcBaseGateway, ICorbaGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;
        private readonly IRestClient _restCsharpClient;

        public CorbaGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Corba;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
            _restCsharpClient =
                new RestClient(ServiceUrl); //TODO: This variable will be removed after all endpoints were moved to C#.
        }

        public async Task<BaseResult<T>> ProcessBatchCorbaCall<T>(string functionName, string className, string isVector, string token, params CorbaModel[] corbaModel)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetBatchCorbaCallRequest(functionName, className, isVector, token, corbaModel);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> ProcessSingleCorbaCall<T>(string functionName, string className, string isVector, string token, params CorbaModel[] corbaModel)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetSingleCorbaCallRequest(functionName, className, isVector, token, corbaModel);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetBatchCorbaCallRequest(string functionName, string className, string isVector, string token, CorbaModel[] corbaModel)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{"batch"}{Routes.Paths.QueryParamSeperator}{functionName}{Routes.Paths.QueryParamSeperator}{className}{Routes.Paths.QueryParamSeperator}{isVector}";
            return PostRequest(resource, corbaModel, token, Constants.Authorization);
        }

        private RestRequest GetSingleCorbaCallRequest(string functionName, string className, string isVector, string token, CorbaModel[] corbaModel)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{"single"}{Routes.Paths.QueryParamSeperator}{functionName}{Routes.Paths.QueryParamSeperator}{className}{Routes.Paths.QueryParamSeperator}{isVector}";
            return PostRequest(resource, corbaModel[0], token, Constants.Authorization);
        }
    }
}