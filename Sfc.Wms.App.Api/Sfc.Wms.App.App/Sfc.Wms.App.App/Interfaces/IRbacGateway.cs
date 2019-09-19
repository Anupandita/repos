using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.Wms.App.App.Interfaces
{
    public interface IRbacGateway
    {
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);
    }
}