using RestSharp;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class MenuGateway : SfcBaseGateway, IMenuGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

       
        public MenuGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.Menus;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> CreateMenu(MenuModel menuMainModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = CreateMenuRequest(menuMainModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> UpdateMenu(MenuModel menuMainModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = UpdateMenuRequest(menuMainModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> Menu(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetAllMenuRequest(token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); ;
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetById(string menuId, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetMenuByIdRequest(menuId, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); ;
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> DeleteById(string menuId,string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = DeleteMenuRequest(menuId,token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false); ;
                return _responseBuilder.GetResponseData<object>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest UpdateMenuRequest(MenuModel menuMainModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.Menu}";
            return PutRequest(resource, menuMainModel, token);
        }

        private RestRequest CreateMenuRequest(MenuModel menuMainModel, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.Menu}";
            return PostRequest(resource, menuMainModel, token);
        }

        private RestRequest DeleteMenuRequest(string menuId,string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.Menu}{Routes.Paths.QueryParamSeperator}{menuId}";
            return DeleteRequest(resource, token);
        }

        private RestRequest GetMenuByIdRequest(string menuId, string token)
        {
            var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.Menu}{Routes.Paths.QueryParamSeperator}{menuId}";
            return GetRequest(token, resource);
        }

        private RestRequest GetAllMenuRequest(string token)
        {
            var resource = $"{_endPoint}";
            return GetRequest(token, resource);
        }
    }
}