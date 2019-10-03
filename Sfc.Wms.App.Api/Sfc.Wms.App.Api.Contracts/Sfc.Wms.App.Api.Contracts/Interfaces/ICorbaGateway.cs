using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Result;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface ICorbaGateway
    {
        Task<BaseResult<string>> ProcessSingleCorbaCall(string functionName, string className, string isVector, string token, 
            params CorbaModel[] corbaModel);

        Task<BaseResult<string>> ProcessBatchCorbaCall(string functionName, string className, string isVector, string token,
           params CorbaModel[] corbaModel);
    }
}
