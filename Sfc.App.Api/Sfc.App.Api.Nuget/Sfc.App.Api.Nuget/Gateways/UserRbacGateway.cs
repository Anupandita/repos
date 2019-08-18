using System.Threading.Tasks;
using System.Web.Routing;
using RestSharp;
using Sfc.App.Api.Nuget.Interfaces;
using Sfc.App.Api.Contracts.Constants;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Dtos.Ui;
using Route = Sfc.App.Api.Contracts.Constants.Route;

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