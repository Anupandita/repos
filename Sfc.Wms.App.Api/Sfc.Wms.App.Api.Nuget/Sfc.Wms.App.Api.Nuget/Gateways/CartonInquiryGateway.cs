using RestSharp;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using Sfc.Wms.App.Api.Nuget.Builders;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CartonInquiryGateway : SfcBaseGateway, ICartonInquiryGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public CartonInquiryGateway(IResponseBuilder responseBuilder , IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Carton;
            _responseBuilder = responseBuilder;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> CartonComments(string cartonNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCartonRequest(Routes.Paths.Comments,cartonNumber, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        

        public async Task<BaseResult<string>> CartonDetails(string cartonNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCartonRequest(Routes.Paths.Details,cartonNumber, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> CartonFromSortation(string cartonNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCartonRequest(Routes.Paths.FromSortation, cartonNumber, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> CartonHospitalLog(string cartonNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCartonRequest(Routes.Paths.HospitalLog, cartonNumber, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> CartonSerialNumbers(string cartonNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCartonRequest(Routes.Paths.SerialNumbers, cartonNumber, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> CartonToSortation(string cartonNumber, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetCartonRequest(Routes.Paths.ToSortation, cartonNumber, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> SearchCartonInquiry(CartonInquiryModel cartonInquiryModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetSearchCartonInquiryRequest(cartonInquiryModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetCartonRequest(string paramName , string cartonNumber, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{paramName}{Routes.Paths.QueryParamSeperator}{cartonNumber}";
            return GetRequest(token, resource);
        }

        private RestRequest GetSearchCartonInquiryRequest(CartonInquiryModel cartonInquiryModel , string token)
        {
            var url = $"{_endPoint}/{Routes.Paths.Search}?";
            url = QueryStringBuilder.BuildQuery("inpt_exact_carton_nbr=", cartonInquiryModel.inpt_exact_carton_nbr, url, true);
            url = QueryStringBuilder.BuildQuery("inpt_exact_pktctrl_nbr=", cartonInquiryModel.inpt_exact_pktctrl_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_shpmt_nbr=", cartonInquiryModel.inpt_shpmt_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_from_stat=", cartonInquiryModel.inpt_from_stat, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_to_stat=", cartonInquiryModel.inpt_to_stat, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_sku_id=", cartonInquiryModel.inpt_sku_id, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_wave_nbr=", cartonInquiryModel.inpt_wave_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_pallet_id=", cartonInquiryModel.inpt_pallet_id, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_from_carton_nbr=", cartonInquiryModel.inpt_from_carton_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_to_carton_nbr=", cartonInquiryModel.inpt_to_carton_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_masterpack_id=", cartonInquiryModel.inpt_masterpack_id, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_to_pkt_control_nbr=", cartonInquiryModel.inpt_to_pkt_control_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_from_pkt_control_nbr=", cartonInquiryModel.inpt_from_pkt_control_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_pnh_ctrl_nbr=", cartonInquiryModel.inpt_pnh_ctrl_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_from_wave_nbr=", cartonInquiryModel.inpt_from_wave_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_to_wave_nbr=", cartonInquiryModel.inpt_to_wave_nbr, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_shortage_type=", cartonInquiryModel.inpt_shortage_type, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_curr_zone=", cartonInquiryModel.inpt_curr_zone, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_curr_aisle=", cartonInquiryModel.inpt_curr_aisle, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_curr_bay=", cartonInquiryModel.inpt_curr_bay, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_curr_lvl=", cartonInquiryModel.inpt_curr_lvl, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_pick_zone=", cartonInquiryModel.inpt_pick_zone, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_pick_aisle=", cartonInquiryModel.inpt_pick_aisle, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_pick_bay=", cartonInquiryModel.inpt_pick_bay, url, false);
            url = QueryStringBuilder.BuildQuery("inpt_pick_lvl=", cartonInquiryModel.inpt_pick_lvl, url, false);
            url = QueryStringBuilder.BuildQuery("pageNo=", cartonInquiryModel.pageNo, url, false);
            url = QueryStringBuilder.BuildQuery("rowsPerPage=", cartonInquiryModel.rowsPerPage, url, false);
            url = QueryStringBuilder.BuildQuery("totalRows=", cartonInquiryModel.totalRows, url, false);
            return GetRequest(token, url);
        }
    }
}
