using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using System;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Builders;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class LpnGateway : SfcBaseGateway, ILpnGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;
        private readonly IRestClient _restCsharpClient;
        private readonly string Authorization = "Authorization";

        public LpnGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Lpn;

            _responseBuilder = responseBuilders;
            _restClient = restClient;
            _restCsharpClient =
                new RestClient(ServiceUrl); //TODO: This variable will be removed after all endpoints were moved to C#.
        }

        public async Task<BaseResult<T>> GetLpnDetailsAsync<T>(LpnParamModel lpnParamModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnDetailsRequest(lpnParamModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> GetLpnDetailsByLpnIdAsync<T>(string lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnDetailsByLpnIdRequest(lpnId, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> GetLpnCommentsByLpnIdAsync<T>(string lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnCommentsByLpnIdRequest(lpnId, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> GetLpnHistoryAsync<T>(string lpnId, string whse, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnHistoryRequest(lpnId, whse, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> GetLpnLockUnlockByLpnIdkAsync<T>(string lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnLockUnlockByLpnIdRequest(lpnId, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> InsertLpnAisleTransAsync<T>(LpnAisleTransModel lpnAisleTransModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = InsertLpnAisleTransRequest(lpnAisleTransModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> UpdateLpnDetailsAsync<T>(LpnDetailsUpdateModel lpnDetailsUpdateModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateLpnDetailsRequest(lpnDetailsUpdateModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> UpdateCaseLpnDetailsAsync<T>(
            LpnCaseDetailsUpdateModel lpnCaseDetailsUpdateModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateCaseLpnDetailsRequest(lpnCaseDetailsUpdateModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> InsertLpnCommentsAsync<T>(LpnCommentsModel lpnCommentsModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = InsertLpnCommentsRequest(lpnCommentsModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> DeleteLpnCommentsAsync<T>(LpnCommentsModel lpnCommentsModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = DeleteLpnCommentsRequest(lpnCommentsModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> GetLpnVendorsAsync<T>(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnVendorsRequest(token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetLpnDetailsRequest(LpnParamModel lpnParamModel, string token)
        {
            var resource =
                $"{_endPoint}/{Routes.Paths.Find}{Routes.Paths.QueryParamSymbol}pageNo={lpnParamModel.PageNo}{Routes.Paths.QueryParamAnd}rowsPerPage={lpnParamModel.RowsPerPage}{Routes.Paths.QueryParamAnd}totalRows={lpnParamModel.TotalRows}";

            resource = QueryStringBuilder.BuildQuery("lpnNumber=", lpnParamModel.LpnNumber, resource, false);
            resource = QueryStringBuilder.BuildQuery("asnId=", lpnParamModel.AsnId, resource, false);
            resource = QueryStringBuilder.BuildQuery("palletId=", lpnParamModel.PalletId, resource, false);
            resource = QueryStringBuilder.BuildQuery("skuId=", lpnParamModel.SkuId, resource, false);
            resource = QueryStringBuilder.BuildQuery("statusFrom=", lpnParamModel.StatusFrom, resource, false);
            resource = QueryStringBuilder.BuildQuery("statusTo=", lpnParamModel.StatusTo, resource, false);
            resource = QueryStringBuilder.BuildQuery("zone=", lpnParamModel.Zone, resource, false);
            resource = QueryStringBuilder.BuildQuery("aisle=", lpnParamModel.Aisle, resource, false);
            resource = QueryStringBuilder.BuildQuery("slot=", lpnParamModel.Slot, resource, false);
            resource = QueryStringBuilder.BuildQuery("createdDate=", lpnParamModel.CreatedDate, resource, false);
            resource = QueryStringBuilder.BuildQuery("level=", lpnParamModel.Level, resource, false);

            return GetRequest(token, resource, Authorization);
        }

        private RestRequest GetLpnDetailsByLpnIdRequest(string lpnId, string token)
        {
            var resource = $"{_endPoint}/{"lpn-details"}/{lpnId}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest GetLpnCommentsByLpnIdRequest(string lpnId, string token)
        {
            var resource = $"{_endPoint}/{"lpn-comments"}/{lpnId}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest GetLpnHistoryRequest(string lpnId, string whse, string token)
        {            
            var resource = $"{_endPoint}/{"lpn-history"}/{lpnId}/{whse}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest GetLpnLockUnlockByLpnIdRequest(string lpnId, string token)
        {
            var resource = $"{_endPoint}/{"lpn-lock-unlock"}/{lpnId}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest InsertLpnAisleTransRequest(LpnAisleTransModel lpnAisleTransModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnAisleTrans}";
            return PostRequest(resource, lpnAisleTransModel, token, Authorization);
        }

        private RestRequest UpdateLpnDetailsRequest(LpnDetailsUpdateModel lpnDetailsUpdateModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnUpdateDetails}";

            return PutRequest(resource, lpnDetailsUpdateModel, token, Authorization);
        }

        private RestRequest UpdateCaseLpnDetailsRequest(LpnCaseDetailsUpdateModel lpnCaseDetailsUpdateModel,
            string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnCaseDetails}";

            return PutRequest(resource, lpnCaseDetailsUpdateModel, token, Authorization);
        }

        private RestRequest InsertLpnCommentsRequest(LpnCommentsModel lpnCommentsModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnComments}";
            return PostRequest(resource, lpnCommentsModel, token, Authorization);
        }

        private RestRequest DeleteLpnCommentsRequest(LpnCommentsModel lpnCommentsModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnComments}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery("caseNbr=", lpnCommentsModel.caseNbr, resource, true);
            resource = QueryStringBuilder.BuildQuery("seqNbr=", lpnCommentsModel.seqNbr, resource, false);
            return DeleteRequest(resource,token, Authorization);
        }

        private RestRequest GetLpnVendorsRequest(string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnVendors}";
            return GetRequest(token, resource, Authorization);
        }
    }
}