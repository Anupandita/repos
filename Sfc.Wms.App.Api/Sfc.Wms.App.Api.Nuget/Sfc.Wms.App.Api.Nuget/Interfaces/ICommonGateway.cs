using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Dto;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface ICommonGateway
    {
        Task<BaseResult<IEnumerable<SysCodeDto>>> GetCodeIdsAsync(SystemCodeInputDto systemCodeDto, string token);
    }
}