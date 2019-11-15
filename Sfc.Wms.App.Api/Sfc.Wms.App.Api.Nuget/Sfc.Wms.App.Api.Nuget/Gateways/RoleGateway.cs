using RestSharp;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class RoleGateway : SfcBaseGateway, IRoleGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public RoleGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Roles;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetRoleMenuMappings(int roleId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetRoleMenuMappingsRequest(roleId,token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); 
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }


        public async Task<BaseResult<string>> GetRolesForUser(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetRolesForUserRequest(token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); 
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateRoleMenus(RoleMenuModel roleMenu,string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateRoleMenusRequest(roleMenu, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }
        
        public async Task<BaseResult<string>> GetRolePermissions(int roleId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetRolePermissionsRequest(roleId,token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); 
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }


        public async Task<BaseResult<string>> UpdateRolePermissions(RolePermissionModel rolePermissions, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateRolePermissionsRequest(rolePermissions, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); 
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }


        private RestRequest GetRoleMenuMappingsRequest(int roleId, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.RoleMenus}{Routes.Paths.QueryParamSeperator}{roleId}";
            return GetRequest(token, resource);
        }

         private RestRequest GetRolePermissionsRequest(int roleId, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.RolePermissions}{Routes.Paths.QueryParamSeperator}{roleId}";
            return GetRequest(token, resource);
        }

        private RestRequest UpdateRoleMenusRequest(RoleMenuModel roleMenu, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.RoleMenus}";
            return PutRequest(resource, roleMenu, token);
        }

        private RestRequest UpdateRolePermissionsRequest(RolePermissionModel rolePermissions, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.RolePermissions}";
            return PutRequest(resource, rolePermissions, token);
        }

        private RestRequest GetRolesForUserRequest(string token)
        {
            var resource = $"{_endPoint}";
            return GetRequest(token, resource);
        }
    }
}