using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CommonGateway : SfcBaseGateway, ICommonGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public CommonGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Common;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> CodeIds(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetCommonCodeRequest(isWhseSysCode, recType, codeType,isNumber,orderByColumn, token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetCommonCodeRequest(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.CodeIds}{Routes.Paths.QueryParamSymbol}recType={recType}{Routes.Paths.QueryParamAnd}codeType={codeType}{Routes.Paths.QueryParamAnd}isNumber={isNumber}{Routes.Paths.QueryParamAnd}orderByColumn={orderByColumn}{Routes.Paths.QueryParamAnd}isWhseSysCode={isWhseSysCode}";
            return GetRequest(token, resource);
        }
    }
}
