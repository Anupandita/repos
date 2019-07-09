using System.Threading.Tasks;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;

namespace Sfc.App.App.Interfaces
{
    public interface IRbacGateway
    {
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);
    }
}