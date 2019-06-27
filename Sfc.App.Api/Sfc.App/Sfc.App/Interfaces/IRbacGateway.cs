using System.Threading.Tasks;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Interfaces
{
    public interface IRbacGateway
    {
        Task<BaseResult<UserDetailsDto>> SignInAsync(LoginCredentials loginCredentials);
    }
}