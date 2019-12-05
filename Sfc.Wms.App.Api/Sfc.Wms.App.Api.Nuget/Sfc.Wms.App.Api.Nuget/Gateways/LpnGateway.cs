using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Builders;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Entities;

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

        public async Task<BaseResult<T>> DeleteLpnCommentsAsync<T>(CaseCommentDto caseCommentDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = DeleteLpnCommentsRequest(caseCommentDto, token);
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

        public async Task<BaseResult<T>> GetLpnDetailsAsync<T>(LpnParameterDto lpnParameterDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLpnDetailsRequest(lpnParameterDto, token);
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

        public async Task<BaseResult<T>> InsertLpnCommentsAsync<T>(CaseCommentDto lpnCommentsModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = InsertLpnCommentsRequest(lpnCommentsModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> UpdateCaseLpnDetailsAsync<T>(LpnDetailsUpdateDto lpnCaseDetailsUpdateModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateCaseLpnDetailsRequest(lpnCaseDetailsUpdateModel, token);
                var response = await _restCsharpClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<T>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<T>> UpdateLpnDetailsAsync<T>(LpnHeaderUpdateDto lpnDetailsUpdateModel,
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

        private RestRequest DeleteLpnCommentsRequest(CaseCommentDto lpnCommentsModel, string token)
        {
            var resource = $"{_endPoint}/{"lpn-comments"}/{lpnCommentsModel.CaseNumber}/{lpnCommentsModel.CommentSequenceNumber}";
            return DeleteRequest(resource, token, Authorization);
        }

        private RestRequest GetLpnCommentsByLpnIdRequest(string lpnId, string token)
        {
            var resource = $"{_endPoint}/{"lpn-comments"}/{lpnId}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest GetLpnDetailsByLpnIdRequest(string lpnId, string token)
        {
            var resource = $"{_endPoint}/{"lpn-details"}/{lpnId}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest GetLpnDetailsRequest(LpnParameterDto lpnParameterDto, string token)
        {
            var resource =
                $"{_endPoint}/{Routes.Paths.Find}{Routes.Paths.QueryParamSymbol}pageNo={lpnParameterDto.PageNumber}{Routes.Paths.QueryParamAnd}rowsPerPage={lpnParameterDto.PageSize}{Routes.Paths.QueryParamAnd}totalRows={lpnParameterDto.TotalRecords}";

            resource = QueryStringBuilder.BuildQuery("lpnNumber=", lpnParameterDto.LpnNumber, resource, false);
            resource = QueryStringBuilder.BuildQuery("asnId=", lpnParameterDto.AsnId, resource, false);
            resource = QueryStringBuilder.BuildQuery("palletId=", lpnParameterDto.PalletId, resource, false);
            resource = QueryStringBuilder.BuildQuery("skuId=", lpnParameterDto.SkuId, resource, false);
            resource = QueryStringBuilder.BuildQuery("statusFrom=", lpnParameterDto.StatusFrom, resource, false);
            resource = QueryStringBuilder.BuildQuery("statusTo=", lpnParameterDto.StatusTo, resource, false);
            resource = QueryStringBuilder.BuildQuery("zone=", lpnParameterDto.Zone, resource, false);
            resource = QueryStringBuilder.BuildQuery("aisle=", lpnParameterDto.Aisle, resource, false);
            resource = QueryStringBuilder.BuildQuery("slot=", lpnParameterDto.Slot, resource, false);
            resource = QueryStringBuilder.BuildQuery("createdDate=", lpnParameterDto.CreatedDate, resource, false);
            resource = QueryStringBuilder.BuildQuery("level=", lpnParameterDto.Level, resource, false);

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

        private RestRequest GetLpnVendorsRequest(string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnVendors}";
            return GetRequest(token, resource, Authorization);
        }

        private RestRequest InsertLpnAisleTransRequest(LpnAisleTransModel lpnAisleTransModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnAisleTrans}";
            return PostRequest(resource, lpnAisleTransModel, token, Authorization);
        }

        private RestRequest InsertLpnCommentsRequest(CaseCommentDto caseCommentDto, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnCommentsAdd}";
            return PostRequest(resource, caseCommentDto, token, Authorization);
        }

        private RestRequest UpdateCaseLpnDetailsRequest(LpnDetailsUpdateDto lpnCaseDetailsUpdateModel,
            string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnCaseDetails}";

            return PutRequest(resource, lpnCaseDetailsUpdateModel, token, Authorization);
        }

        private RestRequest UpdateLpnDetailsRequest(LpnHeaderUpdateDto lpnDetailsUpdateModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.LpnUpdateDetails}";

            return PutRequest(resource, lpnDetailsUpdateModel, token, Authorization);
        }
    }
}