using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class UserRbacGateway : SfcBaseGateway, IUserRbacGateway
    {
        private readonly IResponseBuilder _responseBuilder;
        private readonly string _endPoint;
        private readonly IRestClient _restCsharpClient;
        private readonly string Authorization = "Authorization";


        public UserRbacGateway(IRestClient restClient, IResponseBuilder responseBuilder) : base(restClient)
        {
            _responseBuilder = responseBuilder;
            _endPoint = Routes.Prefixes.User;
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
            var resource = $"{_endPoint}/{Routes.Paths.UserLogin}";
            var request = PostRequest(resource, loginCredentials);
            var result = await _restCsharpClient
                .ExecuteTaskAsync<BaseResult<UserInfoDto>>(request)
                .ConfigureAwait(false);
            return _responseBuilder.GetBaseResult<UserInfoDto>(result);
        }
    }
}