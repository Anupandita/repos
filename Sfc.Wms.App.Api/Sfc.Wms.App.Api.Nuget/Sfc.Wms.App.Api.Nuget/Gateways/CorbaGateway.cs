using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using Sfc.Wms.App.Api.Contracts.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CorbaGateway : SfcBaseGateway, ICorbaGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public CorbaGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Corba;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }


        public async Task<BaseResult<string>> ProcessSingleCorbaCall(string functionName, string className, string isVector, string token, params CorbaModel[] corbaModel)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetSingleCorbaCallRequest(functionName, className,isVector , token,corbaModel);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetSingleCorbaCallRequest(string functionName, string className, string isVector, string token, CorbaModel[] corbaModel)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.Single}{Routes.Paths.QueryParamSeperator}{className}{Routes.Paths.QueryParamSeperator}{functionName}{Routes.Paths.QueryParamSeperator}{isVector}";
            return PostRequest(resource, corbaModel[0], token);
        }

        public async Task<BaseResult<string>> ProcessBatchCorbaCall(string functionName, string className, string isVector, string token, params CorbaModel[] corbaModel)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetBatchCorbaCallRequest(functionName, className, isVector, token, corbaModel);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetBatchCorbaCallRequest(string functionName, string className, string isVector, string token, CorbaModel[] corbaModel)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.Batch}{Routes.Paths.QueryParamSeperator}{className}{Routes.Paths.QueryParamSeperator}{functionName}{Routes.Paths.QueryParamSeperator}{isVector}";
            return PostRequest(resource, corbaModel, token);
        }
    }
}
