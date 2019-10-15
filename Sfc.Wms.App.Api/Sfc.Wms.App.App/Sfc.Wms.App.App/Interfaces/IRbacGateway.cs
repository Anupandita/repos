using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;

namespace Sfc.Wms.App.App.Interfaces
{
    public interface IRbacGateway
    {
        Task<BaseResult<IEnumerable<SysCodeDto>>> GetPrinterValuesAsyc();
        Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials);
    }
}