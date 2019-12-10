using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface ICorbaGateway
    {
        Task<BaseResult<T>> ProcessBatchCorbaCall<T>(string functionName, string className, string isVector,
            string token,
            params CorbaModel[] corbaModel);

        Task<BaseResult<T>> ProcessSingleCorbaCall<T>(string functionName, string className, string isVector,
            string token,
            params CorbaModel[] corbaModel);
    }
}