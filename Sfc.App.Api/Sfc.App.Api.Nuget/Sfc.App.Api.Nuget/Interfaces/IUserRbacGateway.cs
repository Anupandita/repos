using System.Threading.Tasks;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;

namespace Sfc.App.Api.Nuget.Interfaces
{
    public interface IUserRbacGateway
    {
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);
    }
}