using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Interfaces
{
    public interface IRbacService
    {
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);
        Task<BaseResult<UserInfoDto>> GetPrinterValuesAsyc(UserInfoDto userInfoDto);
    }
}