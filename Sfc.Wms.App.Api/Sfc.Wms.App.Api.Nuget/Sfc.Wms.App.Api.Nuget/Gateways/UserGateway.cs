using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class UserGateway : SfcBaseGateway, IUserGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public UserGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.User;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> ChangePassword(ChangePasswordModel changePasswordModel, string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = PostChangePasswordRequest(changePasswordModel, token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> CheckSession(string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = CheckSessionRequest(token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetAllAsync(string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetAllRequest(token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetRolesByUsernameAsync(string token, string userName)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetRolesByUsernameRequest(token, userName);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetUserMenus(string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetUserMenusRequest(token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetUserPermissions(string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetUserPermissionsRequest(token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> Logout()
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = GetLogoutRequest();
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> SignInAsync(LoginCredentials loginCredentials)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var cRequest = SignInRequest(loginCredentials, ConfigurationManager.AppSettings["ServiceUrl"]);
                var cResponse = await _restClient.ExecuteTaskAsync<UserInfoDto>(cRequest).ConfigureAwait(false);
                var nodeRequest = SignInRequest(loginCredentials, _restClient.BaseUrl.ToString());
                var nodeResponse = await _restClient.ExecuteTaskAsync<UserInfoDto>(nodeRequest).ConfigureAwait(false);
                var tokenHeaderParameter = new Parameter(Constants.NodeToken, nodeResponse.Data.Token, ParameterType.HttpHeader);
                cResponse.Headers.Add(tokenHeaderParameter);
                return _responseBuilder.GetCResponseData<UserInfoDto>(cResponse);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateUserRoles(UserRoleModel userRoleModel, string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = UpdateUserRolesRequest(userRoleModel, token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UserPreferences(UserPreferencesModel userPreferencesModel, string token)
        {
            return await Proxy().ExecuteAsync(async () =>
            {
                var request = UpdateUserPreferencesRequest(userPreferencesModel, token);
                var response = await _restClient.ExecuteTaskAsync<List<object>>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<List<object>>(response);
            }).ConfigureAwait(false);
        }

        private string BuildUriString(string path, string restUrl)
        {
            var urlBuilder = new UriBuilder(restUrl);
            urlBuilder.Path += $"{_endPoint}/{path}";
            return urlBuilder.Uri.ToString();
        }

        private RestRequest CheckSessionRequest(string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.CheckSession}";
            return GetRequest(token, resource);
        }

        private RestRequest GetAllRequest(string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.AllUsers}";
            return GetRequest(token, resource);
        }

        private RestRequest GetLogoutRequest()
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.Logout}";
            return GetRequest(null, resource);
        }

        private RestRequest GetRolesByUsernameRequest(string token, string userName)
        {
            var resource = $"{_endPoint}/{Routes.Paths.UserRoles}/{userName}";
            return GetRequest(token, resource);
        }

        private RestRequest GetUserMenusRequest(string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.Menu}";
            return GetRequest(token, resource);
        }

        private RestRequest GetUserPermissionsRequest(string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.UserRolePermissions}";
            return GetRequest(token, resource);
        }

        private RestRequest PostChangePasswordRequest(ChangePasswordModel changePasswordModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.ChangePassword}";
            return PostRequest(token, changePasswordModel, resource);
        }

        private RestRequest SignInRequest(LoginCredentials loginCredentials, string restUrl)
        {
            var resource = BuildUriString(Routes.Paths.UserLogin, restUrl);
            var body = new
            {
                userName = loginCredentials.UserName,
                password = loginCredentials.Password
            };
            return PostRequest(resource, body, null);
        }

        private RestRequest UpdateUserPreferencesRequest(UserPreferencesModel userPreferencesModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.UserPreferences}";
            return PutRequest(resource, userPreferencesModel, token);
        }

        private RestRequest UpdateUserRolesRequest(UserRoleModel userRoleModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Prefixes.UserRoles}";
            return PostRequest(token, userRoleModel, resource);
        }
    }
}