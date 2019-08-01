using System.Threading.Tasks;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.Result;

namespace Sfc.App.Api.Nuget.Interfaces
{
    public interface IUserRbacGateway
    {
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);
    }
}