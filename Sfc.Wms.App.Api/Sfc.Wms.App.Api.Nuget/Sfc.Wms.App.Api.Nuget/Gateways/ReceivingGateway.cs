using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;

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
            _endPoint = Routes.Prefixes.Receiving;
            _responseBuilder = responseBuilders;
            restClient.BaseUrl = new Uri(ServiceUrl);
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult<SearchResultDto>> SearchAsync(ReceiptInquiryDto receiptInquiryDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetRequest(_endPoint, receiptInquiryDto, token, Authorization);
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
                var resource = $"{_endPoint}/{Routes.Paths.AsnDetails}/{shipmentNumber}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient
                    .ExecuteTaskAsync<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<AsnDrillDownDetailsDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<IEnumerable<QvDetailsDto>>> GetQvDetailsAsync(string shipmentNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.QuestionsAnswers}/{shipmentNumber}";
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
                var resource = $"{_endPoint}/{Routes.Paths.AsnLotTracking}/{shipmentNumber}/{skuId}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient
                    .ExecuteTaskAsync<BaseResult<IEnumerable<AsnLotTrackingDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<AsnLotTrackingDto>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateAnswerTextAsync(AnswerTextDto asnAnswerTextDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.QuestionsAnswers}";
                var request = PostRequest(resource, asnAnswerTextDto, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<ShipmentDetailsDto>> GetShipmentDetailsAsync(string shipmentNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.ShipmentDetails}/{shipmentNumber}";
                var request = GetRequest(resource, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<ShipmentDetailsDto>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<ShipmentDetailsDto>(response);
            }).ConfigureAwait(false);
        }
    }
}