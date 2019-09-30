using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;

namespace Wms.App.Contracts.Interfaces
{
    public interface ICommonGateway
    {
        Task<BaseResult<string>> codeIds(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token);
    }
}
