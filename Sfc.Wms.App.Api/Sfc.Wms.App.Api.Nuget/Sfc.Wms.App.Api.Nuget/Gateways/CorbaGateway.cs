using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.Corba.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CorbaGateway : SfcBaseGateway, ICorbaGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestCsharpClient _restCsharpClient;

        public CorbaGateway(IResponseBuilder responseBuilders, IRestCsharpClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.Corba;
            _responseBuilder = responseBuilders;
            _restCsharpClient = restClient;

        }

        public async Task<BaseResult<CorbaResponseDto>> ProcessBatchCorbaCall(string functionName, string isVector, List<CorbaDto> corbaDtos, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{"batch"}{Routes.Paths.QueryParamSeperator}{functionName}{Routes.Paths.QueryParamSeperator}{isVector}";
                var request = PostRequest(resource, corbaDtos, token, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<CorbaResponseDto>>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<CorbaResponseDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> ProcessSingleCorbaCall(string functionName, string isVector, CorbaDto corbaDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{"single"}{Routes.Paths.QueryParamSeperator}{functionName}{Routes.Paths.QueryParamSeperator}{isVector}";
                var request = PostRequest(resource, corbaDto, token, Constants.Authorization);

                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<BaseResult>(response);
            }).ConfigureAwait(false);
        }
    }
}