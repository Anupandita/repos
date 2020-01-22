using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IUserRbacGateway
    {
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);

        Task<BaseResult> RefreshAuthTokenAsync();
    }
}