using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Builders;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class ReturnReceivingGateway : SfcBaseGateway, IReturnReceivingGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public ReturnReceivingGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.ReturnsReceiving;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetReturnReceiving(ReturnReceivingSearchModel returnReceivingSearchModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetReturnReceivingRequest(returnReceivingSearchModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);

        }

        public async Task<BaseResult<string>> InsertReturnReceiving(ReturnReceivingInsertModel returnReceivingInsertModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = InsertReturnReceivingRequest(returnReceivingInsertModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetReturnReceivingRequest(ReturnReceivingSearchModel returnReceivingSearchModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSymbol}pageNo={returnReceivingSearchModel.pageNo}{Routes.Paths.QueryParamAnd}rowsPerPage={returnReceivingSearchModel.rowsPerPage}{Routes.Paths.QueryParamAnd}totalRows={returnReceivingSearchModel.totalRows}";
            
            resource = QueryStringBuilder.BuildQuery($"{nameof(returnReceivingSearchModel.item)}=", returnReceivingSearchModel.item, resource, false);
            resource = QueryStringBuilder.BuildQuery($"{nameof(returnReceivingSearchModel.asn)}=", returnReceivingSearchModel.asn, resource, false);
            resource = QueryStringBuilder.BuildQuery($"{nameof(returnReceivingSearchModel.userRoute)}=", returnReceivingSearchModel.userRoute, resource, false);
            resource = QueryStringBuilder.BuildQuery($"{nameof(returnReceivingSearchModel.fromDate)}=", returnReceivingSearchModel.fromDate, resource, false);
            resource = QueryStringBuilder.BuildQuery($"{nameof(returnReceivingSearchModel.toDate)}=", returnReceivingSearchModel.toDate, resource, false);

            return GetRequest(token, resource);
        }

        private RestRequest InsertReturnReceivingRequest(ReturnReceivingInsertModel returnReceivingInsertModel, string token)
        {
            var resource = $"{_endPoint}";
            return PostRequest(resource, returnReceivingInsertModel, token);
        }
    }
}
