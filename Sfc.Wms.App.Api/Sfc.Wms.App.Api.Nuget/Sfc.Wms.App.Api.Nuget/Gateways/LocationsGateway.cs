using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Nuget.Builders;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class LocationsGateway : SfcBaseGateway, ILocationsGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public LocationsGateway(IResponseBuilder responseBuilders, IRestClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.Locations;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> AddToLocationGroup(
            AddActiveLocationGroupModel addActiveLocationGroupModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = AddToLocationGroupRequest(addActiveLocationGroupModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> DeleteFromLocationGroup(LocationDeleteModel locationDeleteModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = DeleteFromLocationGroupRequest(locationDeleteModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetActionItem(ActiveItemModel activeItemModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetActionItemRequest(activeItemModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetActiveLocationSearch(ActiveLocationModel activeLocationModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetActiveLocationRequest(activeLocationModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLocationGroupById(string gridLocnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetActiveLocationGroupByIdRequest(gridLocnId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLocationGroupsByGroupType(string locationGroupId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLocationGroupsByGroupTypeRequest(locationGroupId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }


        public async Task<BaseResult<string>> GetLocationGroupTypesAll(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLocationGroupTypesRequest(token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetLocationLPNS(string gridLocnId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetLocationLpnsRequest(gridLocnId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetReserveLocationSearch(ReserveLocationModel reserveLocationModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetReserveLocationSearchRequest(reserveLocationModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetReserveLocnDrillDown(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetReserveLocnDrillDownRequest(token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetWorkAreaMaster(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetWorkAreaMasterRequest(token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateActionItem(ActiveItemUpdateModel activeItemUpdateModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateActionItemRequest(activeItemUpdateModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateLocationLPNS(ActiveLocationLpnModel allm, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateLocationLpnsRequest(allm, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateActiveLocationsDrilldown(
            ActiveLocationsDrillDownModel activeLocationsDrillDownModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateActiveLocationsDrilldownRequest(activeLocationsDrillDownModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateAdjInv(AdjInvModel adjInvModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateAdjInvRequest(adjInvModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateLocationGroupById(ActiveLocationGroupModel activeLocationGroupModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateLocationGroupByIdRequest(activeLocationGroupModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateReserveLocationnDrillDown(
            ReserveLocationDrillDownModel reserveLocationDrillDownModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateReserveLocationDrillDownRequest(reserveLocationDrillDownModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetLocationGroupTypesRequest(string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationGroupTypesAll}";
            return GetRequest(token, resource);
        }

        private RestRequest GetLocationLpnsRequest(string gridLocnId, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationLPN}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.GridLocationId, gridLocnId, resource, true);
            return GetRequest(token, resource);
        }

        private RestRequest GetReserveLocationSearchRequest(ReserveLocationModel reserveLocationModel, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ReserveLocations}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputAisle, reserveLocationModel.user_inpt_aisle,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputGroupTypes,
                reserveLocationModel.user_inpt_grp_type, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserLocationClass,
                reserveLocationModel.user_inpt_locn_cls, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputLocationGroup,
                reserveLocationModel.user_inpt_locn_grp, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputLVL, reserveLocationModel.user_inpt_lvl,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputPutwayZone,
                reserveLocationModel.user_inpt_putwy_zone, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputSlot, reserveLocationModel.user_inpt_slot,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputZone, reserveLocationModel.user_inpt_zone,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.PageNo, reserveLocationModel.pageNo, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.RowsPerPage, reserveLocationModel.rowsPerPage,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.TotalRows, reserveLocationModel.totalRows, resource,
                false);
            return GetRequest(token, resource);
        }

        private RestRequest GetReserveLocnDrillDownRequest(string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ReserveLocationDrillDown}";
            return GetRequest(token, resource);
        }

        private RestRequest GetWorkAreaMasterRequest(string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.WorkAreaMaster}";
            return GetRequest(token, resource);
        }

        public async Task<BaseResult<string>> UpdateLockUnlock(LockUnlockModel lockUnlockModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateLockUnlockRequest(lockUnlockModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest UpdateLocationGroupByIdRequest(ActiveLocationGroupModel activeLocationGroupModel,
            string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationsGroupById}";
            return PutRequest(resource, activeLocationGroupModel, token);
        }

        private RestRequest UpdateAdjInvRequest(AdjInvModel adjInvModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationsAdjInv}";
            return PutRequest(resource, adjInvModel, token);
        }

        private RestRequest UpdateLockUnlockRequest(LockUnlockModel lockUnlockModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationsLockUnlock}";
            return PutRequest(resource, lockUnlockModel, token);
        }

        private RestRequest UpdateLocationLpnsRequest(ActiveLocationLpnModel activeLocationLpnModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationLPN}";
            return PutRequest(resource, activeLocationLpnModel, token);
        }

        private RestRequest UpdateReserveLocationDrillDownRequest(
            ReserveLocationDrillDownModel reserveLocationDrillDownModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ReserveLocationDrillDown}";
            return PutRequest(resource, reserveLocationDrillDownModel, token);
        }

        private RestRequest DeleteFromLocationGroupRequest(LocationDeleteModel locationDeleteModel, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationGroup}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery($"{nameof(locationDeleteModel.lcnId)}=", locationDeleteModel.lcnId,
                resource, true);
            resource = QueryStringBuilder.BuildQuery($"{nameof(locationDeleteModel.grpTypes)}=",
                locationDeleteModel.grpTypes, resource, false);
            return DeleteRequest(resource, token);
        }

        private RestRequest AddToLocationGroupRequest(AddActiveLocationGroupModel addActiveLocationGroupModel,
            string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.AddToLocnGroup}";
            return PostRequest(resource, addActiveLocationGroupModel, token);
        }

        private RestRequest GetActionItemRequest(ActiveItemModel activeItemModel, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ActiveItem}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.GridLocationId, activeItemModel.grid_locn_id,
                resource, true);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.GridSequenceNumber, activeItemModel.grid_seq_nbr,
                resource, false);
            return GetRequest(token, resource);
        }

        private RestRequest GetActiveLocationRequest(ActiveLocationModel activeLocationModel, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ActiveLocations}{Routes.Paths.QueryParamSymbol}";

            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputAisle, activeLocationModel.user_inpt_aisle,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputZone, activeLocationModel.user_inpt_zone,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputGroupTypes,
                activeLocationModel.user_inpt_grp_type, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputSlot, activeLocationModel.user_inpt_slot,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputLVL, activeLocationModel.user_inpt_lvl,
                resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputLocationGroup,
                activeLocationModel.user_inpt_locn_grp, resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.UserInputSKU, activeLocationModel.user_inpt_sku
                , resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.TotalRows, activeLocationModel.totalRows
                , resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.PageNo, activeLocationModel.pageNo
                , resource, false);
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.RowsPerPage, activeLocationModel.rowsPerPage
                , resource, false);
            return GetRequest(token, resource);
        }

        private RestRequest UpdateActiveLocationsDrilldownRequest(
            ActiveLocationsDrillDownModel activeLocationsDrillDownModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ActiveLocationsDrilldown}";
            return PutRequest(resource, activeLocationsDrillDownModel, token);
        }

        private RestRequest GetActiveLocationGroupByIdRequest(string gridLocnId, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationsGroupById}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery(Routes.Paths.GridLocationId, gridLocnId, resource, true);
            return GetRequest(token, resource);
        }

        private RestRequest GetLocationGroupsByGroupTypeRequest(string locationGroupId, string token)
        {
            var resource =
                $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.LocationGroup}{Routes.Paths.QueryParamSymbol}";
            resource = QueryStringBuilder.BuildQuery("grp_type=", locationGroupId, resource, true);
            return GetRequest(token, resource);
        }

        private RestRequest UpdateActionItemRequest(ActiveItemUpdateModel activeItemUpdateModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ActiveItem}";
            return PutRequest(resource, activeItemUpdateModel, token);
        }
    }
}