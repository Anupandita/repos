using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class ReceiptGateway : SfcBaseGateway, IReceiptGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public ReceiptGateway(IResponseBuilder responseBuilders, IRestClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.Receipt;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetReceiptAsync(string token, string resource)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetReceiptRequest(token, resource);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetShipmentDetailsAsync(string token, string shipmentNumber)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetShipmentDetailsRequest(token, shipmentNumber);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetQvDetailsAsync(string token, string shipmentNumber)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetQvDetailsRequest(token, shipmentNumber);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetAsnDetailsAsync(string token, string shipmentNumber)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetAsnDetailsRequest(token, shipmentNumber);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetDrillAsnDetailsAsync(string token, string whse, string shipmentNumber,
            string skuId)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetDrillAsnDetails(token, whse, shipmentNumber, skuId);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetAppoitmentScheduleAsync(string token, string shipmentNumber)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetAppoitmentSchedule(token, shipmentNumber);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetASNCommentsAsync(string token, string shipmentNumber)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetAsnComments(token, shipmentNumber);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateQVDetails(string token, QVDetails qvDetails)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = UpdateQvDetailsRequest(token, qvDetails);
                var response = await _restClient.ExecuteTaskAsync<object>(request, Method.PUT).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetShipmentDetailsRequest(string token, string shipmentNumber)
        {
            var resource = $"{_endPoint}/{Routes.Paths.ShipmentDetails}/{shipmentNumber}";
            return GetRequest(token, resource);
        }

        private RestRequest GetQvDetailsRequest(string token, string shipmentNumber)
        {
            var resource = $"{_endPoint}/{Routes.Paths.QvDetails}?{Routes.Params.ShipmentNumber}={shipmentNumber}";
            return GetRequest(token, resource);
        }

        private RestRequest GetAsnDetailsRequest(string token, string shipmentNumber)
        {
            var resource = $"{_endPoint}/{Routes.Paths.AsnDetails}/{shipmentNumber}";
            return GetRequest(token, resource);
        }

        private RestRequest GetDrillAsnDetails(string token, string whse, string shipmentNumber, string skuId)
        {
            var resource = $"{_endPoint}/{Routes.Paths.DrillASNDetails}/{whse}/{shipmentNumber}/{skuId}";
            return GetRequest(token, resource);
        }

        private RestRequest GetAppoitmentSchedule(string token, string shipmentNumber)
        {
            var resource =
                $"{_endPoint}/{Routes.Paths.ApprointmentSchedule}?{Routes.Params.ShipmentNumber}={shipmentNumber}";
            return GetRequest(token, resource);
        }

        private RestRequest GetAsnComments(string token, string shipmentNumber)
        {
            var resource = $"{_endPoint}/{Routes.Paths.AsnComments}?{Routes.Params.ShipmentNumber}={shipmentNumber}";
            return GetRequest(token, resource);
        }

        private RestRequest UpdateQvDetailsRequest(string token, QVDetails qvDetails)
        {
            var resource = $"{_endPoint}/{Routes.Paths.QvDetails}";
            return PutRequest(resource, qvDetails, token);
        }

        private RestRequest GetReceiptRequest(string token, string resourceParams)
        {
            return GetRequest(token, resourceParams);
        }
    }
}