using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class UserRbacGateway : SfcBaseGateway, IUserRbacGateway
    {
        public UserRbacGateway(IRestClient restClient) : base(restClient, Routes.Prefixes.User)
        {
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var request = new RestRequest(Routes.Paths.UserLogin, Method.POST);
            request.AddJsonBody(loginCredentials);
            var result = await RestClient
                .ExecuteTaskAsync<BaseResult<UserInfoDto>>(request)
                .ConfigureAwait(false);
            return GetBaseResult<UserInfoDto>(result);
        }
    }
}