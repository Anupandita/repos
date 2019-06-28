using System.Threading.Tasks;
using RestSharp;
using Sfc.App.Api.Nuget.Interfaces;
using Sfc.App.Contracts.Constants;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class UserRbacGateway : SfcBaseGateway, IUserRbacGateway
    {
        public UserRbacGateway(IRestClient restClient) : base(restClient, Route.User)
        {
        }

        public async Task<BaseResult<UserDetailsDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var request = new RestRequest(Route.Paths.UserLogin, Method.POST);
            request.AddJsonBody(loginCredentials);
            var result = await RestClient
                .ExecuteTaskAsync<BaseResult<UserDetailsDto>>(request)
                .ConfigureAwait(false);
            return GetBaseResult<UserDetailsDto>(result);
        }
    }
}