using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class LpnGateway : SfcBaseGateway, ILpnGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restCsharpClient;
        private const string Authorization = "Authorization";

        public LpnGateway(IResponseBuilder responseBuilders, IRestClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.Lpn;
            _responseBuilder = responseBuilders;
            restClient.BaseUrl = new Uri(ServiceUrl);
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult> DeleteLpnCommentsAsync(string caseNumber, int commentSequenceNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnComments}/{caseNumber}/{commentSequenceNumber}";
                var request = DeleteRequest(resource, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<List<CaseCommentDto>>> GetLpnCommentsByLpnIdAsync(string lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnComments}/{lpnId}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<List<CaseCommentDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<List<CaseCommentDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<LpnSearchResultsDto>> LpnSearchAsync(LpnParameterDto lpnParameterDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.Find}";
                var request = GetRequest(resource, lpnParameterDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<LpnSearchResultsDto>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<LpnSearchResultsDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<LpnDetailsDto>> GetLpnDetailsByLpnIdAsync(string lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{"lpn-details"}/{lpnId}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<LpnDetailsDto>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<LpnDetailsDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<List<LpnHistoryDto>>> GetLpnHistoryByLpnIdAndWhseAsync(string lpnId, string whse,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{"lpn-history"}/{lpnId}/{whse}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<List<LpnHistoryDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<List<LpnHistoryDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<List<CaseLockUnlockDto>>> GetLpnLockUnlockByLpnIdAsync(string lpnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{"lpn-lock-unlock"}/{lpnId}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<List<CaseLockUnlockDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<List<CaseLockUnlockDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<CaseCommentDto>> InsertLpnCommentsAsync(CaseCommentDto caseCommentDto,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnComments}";
                var request = PostRequest(resource, caseCommentDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<CaseCommentDto>>(request)
                    .ConfigureAwait(false);

                return _responseBuilder.GetBaseResult<CaseCommentDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateLpnDetailsAsync(LpnDetailsUpdateDto lpnCaseDetailsUpdateModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnCaseDetails}";
                var request = PutRequest(resource, lpnCaseDetailsUpdateModel, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateLpnCommentAsync(CaseCommentDto caseCommentDto,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnComments}";
                var request = PutRequest(resource, caseCommentDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateLpnHeaderAsync(LpnHeaderUpdateDto lpnDetailsUpdateModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnUpdateDetails}";
                var request = PutRequest(resource, lpnDetailsUpdateModel, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<List<CaseLockDto>>> GetCaseUnLockDetailsAsync(IEnumerable<string> lpnIds, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.CaseUnlock}";
                var queryObject = new { lpnIds };
                var request = GetRequest(resource, queryObject, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<List<CaseLockDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<List<CaseLockDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<LpnMultipleUnlockResultDto>> LpnMultipleUnlockAsync(List<LpnMultipleUnlockDto> lpnMultipleUnlockDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnMultipleUnlock}";
                var request = PostRequest(resource, lpnMultipleUnlockDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<LpnMultipleUnlockResultDto>>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<LpnMultipleUnlockResultDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<LpnMultipleUnlockResultDto>> CaseLockCommentWithBatchCorbaAsync(CaseLockCommentDto caseLockComment, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.LpnMultiplelock}";
                var request = PostRequest(resource, caseLockComment, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<LpnMultipleUnlockResultDto>>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<LpnMultipleUnlockResultDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> MultipleLpnsCommentsAddAsync(IEnumerable<CaseCommentDto> caseCommentDtos, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.MultipleLpnCommentsAddition}";
                var request = PostRequest(resource, caseCommentDtos, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> MultipleLpnsUpdateAsync(LpnBatchUpdateDto lpnBatchUpdateDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.MultipleLpnUpdate}";
                var request = PutRequest(resource, lpnBatchUpdateDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }
    }
}