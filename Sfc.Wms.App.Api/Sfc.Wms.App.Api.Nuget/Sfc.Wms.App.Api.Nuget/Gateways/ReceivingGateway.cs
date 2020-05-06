using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class ReceivingGateway : SfcBaseGateway, IReceivingGateway
    {

        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestCsharpClient _restCsharpClient;
        private const string Authorization = "Authorization";

        public ReceivingGateway(IResponseBuilder responseBuilders, IRestCsharpClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.Api;
            _responseBuilder = responseBuilders;
            restClient.BaseUrl = new Uri(ServiceUrl);
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult<SearchResultDto>> SearchAsync(ReceiptInquiryDto receiptInquiryDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetRequest($"{_endPoint}/{Routes.Paths.Receipt}", receiptInquiryDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<SearchResultDto>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<SearchResultDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>> GetAsnDetailsAsync(string shipmentNumber,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.AdvanceShipmentNotices}/{shipmentNumber}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient
                    .ExecuteTaskAsync<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<AsnDrillDownDetailsDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<IEnumerable<QvDetailsDto>>> GetQualityVerificationsDetailsAsync(string shipmentNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.AdvanceShipmentNotices}/{shipmentNumber}/{Routes.Paths.QualityVerifications}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<IEnumerable<QvDetailsDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<QvDetailsDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<IEnumerable<AsnLotTrackingDto>>> GetAsnLotTrackingDetailsAsync(
            string shipmentNumber, string skuId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.AdvanceShipmentNotices}/{shipmentNumber}/{Routes.Paths.Skus}/{skuId}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient
                    .ExecuteTaskAsync<BaseResult<IEnumerable<AsnLotTrackingDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<AsnLotTrackingDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateQualityVerificationsAsync(AnswerTextDto asnAnswerTextDto, string shipmentNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.AdvanceShipmentNotices}/{shipmentNumber}/{Routes.Paths.QualityVerifications}";
                var request = PutRequest(resource, asnAnswerTextDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateAdvanceShipmentNoticesDetailsAsync(UpdateAsnDto updateAsnDto, string shipmentNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.AdvanceShipmentNotices}/{shipmentNumber}";
                var request = PutRequest(resource, updateAsnDto, token, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);

        }
    }
}