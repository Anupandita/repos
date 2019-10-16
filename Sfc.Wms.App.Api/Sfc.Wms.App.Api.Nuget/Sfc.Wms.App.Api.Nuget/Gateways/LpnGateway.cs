using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using Sfc.Wms.App.Api.Contracts.Result;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Nuget.Builders;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class LpnGateway : SfcBaseGateway, ILpnGateway
    {

        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;


        public LpnGateway(IResponseBuilder responseBuilders,IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Lpn;
            _serviceBaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetLpnDetailsAsync(LpnParamModel lpnParamModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnDetailsRequest(lpnParamModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
            
        }

        public async Task<BaseResult<string>> GetLpnDetailsByLpnIdAsync(String lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnDetailsByLpnIdRequest(lpnId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLpnCommentsByLpnIdAsync(String lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnCommentsByLpnIdRequest(lpnId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLpnHistoryAsync(String lpnId, string whse, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnHistoryRequest(lpnId, whse, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLpnLockUnlocByLpnIdkAsync(String lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnLockUnlocByLpnIdRequest(lpnId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> InsertLpnAisleTransAsync(LpnAisleTransModel lpnAisleTransModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = InsertLpnAisleTransRequest(lpnAisleTransModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateLpnDetailsAsync(LpnDetailsUpdateModel lpnDetailsUpdateModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateLpnDetailsRequest(lpnDetailsUpdateModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> InsertLpnCommentsAsync(LpnCommentsModel lpnCommentsModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = InsertLpnCommentsRequest(lpnCommentsModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> DeleteLpnCommentsAsync(LpnCommentsModel lpnCommentsModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = DeleteLpnCommentsRequest(lpnCommentsModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLpnVendorsAsync(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnVendorsRequest( token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetLpnDetailsRequest(LpnParamModel lpnParamModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.Find}{Routes.Paths.QueryParamSymbol}pageNo={lpnParamModel.pageNo}{Routes.Paths.QueryParamAnd}rowsPerPage={lpnParamModel.rowsPerPage}{Routes.Paths.QueryParamAnd}totalRows={lpnParamModel.totalRows}";

            resource = QueryStringBuilder.BuildQuery("lpnNumber=", lpnParamModel.lpnNumber, resource, false);
            resource = QueryStringBuilder.BuildQuery("asnId=", lpnParamModel.asnId, resource, false);
            resource = QueryStringBuilder.BuildQuery("palletId=", lpnParamModel.palletId, resource, false);
            resource = QueryStringBuilder.BuildQuery("skuId=", lpnParamModel.skuId, resource, false);
            resource = QueryStringBuilder.BuildQuery("statusFrom=", lpnParamModel.statusFrom, resource, false);
            resource = QueryStringBuilder.BuildQuery("statusTo=", lpnParamModel.statusTo, resource, false);
            resource = QueryStringBuilder.BuildQuery("zone=", lpnParamModel.zone, resource, false);
            resource = QueryStringBuilder.BuildQuery("aisle=", lpnParamModel.aisle, resource, false);
            resource = QueryStringBuilder.BuildQuery("slot=", lpnParamModel.slot, resource, false);
            resource = QueryStringBuilder.BuildQuery("createdDate=", lpnParamModel.createdDate, resource, false);

            return GetRequest(token, resource);
        }

        private RestRequest GetLpnDetailsByLpnIdRequest(String lpnId, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnDetails}/{lpnId}";
            return GetRequest(token, resource);
        }

        private RestRequest GetLpnCommentsByLpnIdRequest(String lpnId, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnComments}/{lpnId}";
            return GetRequest(token, resource);
        }

        private RestRequest GetLpnHistoryRequest(String lpnId, string whse, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnHistory}/{lpnId}/{whse}";
            return GetRequest(token, resource);
        }

        private RestRequest GetLpnLockUnlocByLpnIdRequest(String lpnId, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnLockUnlock}/{lpnId}";
            return GetRequest(token, resource);
        }

        private RestRequest InsertLpnAisleTransRequest(LpnAisleTransModel lpnAisleTransModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnAisleTrans}";
            return PostRequest(resource, lpnAisleTransModel, token);
        }

        private RestRequest UpdateLpnDetailsRequest(LpnDetailsUpdateModel lpnDetailsUpdateModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnDetails}";

            return PutRequest(resource, lpnDetailsUpdateModel, token);
        }

        private RestRequest InsertLpnCommentsRequest(LpnCommentsModel lpnCommentsModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnComments}";
            return PostRequest(resource, lpnCommentsModel, token);
        }

        private RestRequest DeleteLpnCommentsRequest(LpnCommentsModel lpnCommentsModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnComments}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery("caseNbr=", lpnCommentsModel.caseNbr, resource, true);
            resource = QueryStringBuilder.BuildQuery("seqNbr=", lpnCommentsModel.seqNbr, resource, false);
            return DeleteRequest(resource,token);
        }

        private RestRequest GetLpnVendorsRequest(string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnVendors}";
            return GetRequest(token, resource);
        }
    }
}
