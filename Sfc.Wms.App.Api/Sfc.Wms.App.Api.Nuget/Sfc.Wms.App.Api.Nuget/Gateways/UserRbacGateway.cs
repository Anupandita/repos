using System;
using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class UserRbacGateway : SfcBaseGateway, IUserRbacGateway
    {
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;
        private readonly string _endPoint;
        private readonly IRestClient _restCsharpClient;
        private readonly string Authorization = "Authorization";


        public UserRbacGateway(IRestClient restClient, IResponseBuilder responseBuilder) : base(restClient)
        {
            _restClient = restClient;
            _responseBuilder = responseBuilder;
            _endPoint = Routes.Prefixes.User;
            restClient.BaseUrl = new Uri(ServiceUrl);
            _restCsharpClient = restClient;

        }

       public async Task<BaseResult> RefreshAuthTokenAsync(string token)
        {
            const string url = Routes.Paths.RefreshToken;
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{url}";
                var request = GetRequest(token, resource, Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var request = new RestRequest(Routes.Paths.UserLogin, Method.POST);
            request.AddJsonBody(loginCredentials);
            var result = await _restClient
                .ExecuteTaskAsync<BaseResult<UserInfoDto>>(request)
                .ConfigureAwait(false);
            return _responseBuilder.GetBaseResult<UserInfoDto>(result);
        }
    }
}