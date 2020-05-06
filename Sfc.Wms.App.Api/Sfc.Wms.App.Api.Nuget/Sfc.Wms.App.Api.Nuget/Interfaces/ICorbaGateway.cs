using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.Corba.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface ICorbaGateway
    {
        Task<BaseResult> ProcessSingleCorbaCall(string functionName, string isVector, CorbaDto corbaDto, string token);

        Task<BaseResult<CorbaResponseDto>> ProcessBatchCorbaCall(string functionName, string isVector, List<CorbaDto> corbaDtos, string token);
    }
}