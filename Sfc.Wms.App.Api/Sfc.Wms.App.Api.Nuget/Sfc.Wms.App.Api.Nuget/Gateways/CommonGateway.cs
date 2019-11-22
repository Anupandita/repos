using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CommonGateway : SfcBaseGateway, ICommonGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;
        private readonly IRestClient _restCsharpClient;

        public CommonGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Common;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
            _restCsharpClient =
                new RestClient(ServiceUrl); //TODO: This variable will be removed after all endpoints were moved to C#.
        }

        public async Task<BaseResult<T>> CodeIds<T>(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCommonCodeRequest(isWhseSysCode, recType, codeType, isNumber, orderByColumn, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetCommonCodeRequest(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.CodeIds}{Routes.Paths.QueryParamSymbol}RecType={recType}{Routes.Paths.QueryParamAnd}CodeType={codeType}{Routes.Paths.QueryParamAnd}IsNumber={isNumber}{Routes.Paths.QueryParamAnd}OrderByColumn={orderByColumn}{Routes.Paths.QueryParamAnd}IsWhseSysCode={isWhseSysCode}";
            return GetRequest(token, resource, Constants.Authorization);
        }
    }
}
