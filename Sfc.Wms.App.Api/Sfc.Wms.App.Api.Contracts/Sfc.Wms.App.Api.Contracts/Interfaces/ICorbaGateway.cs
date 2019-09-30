using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wms.App.Contracts.Entities;
using Wms.App.Contracts.Result;

namespace Wms.App.Contracts.Interfaces
{
    public interface ICorbaGateway
    {
        Task<BaseResult<string>> ProcessSingleCorbaCall(string functionName, string className, string isVector, string token, 
            params CorbaModel[] corbaModel);

        Task<BaseResult<string>> ProcessBatchCorbaCall(string functionName, string className, string isVector, string token,
           params CorbaModel[] corbaModel);
    }
}
