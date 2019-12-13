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
        private readonly ResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public UserRbacGateway(IRestClient restClient, ResponseBuilder responseBuilder) : base(restClient)
        {
            _restClient = restClient;
            _responseBuilder = responseBuilder;
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