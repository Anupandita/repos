using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface ICommonGateway
    {
        Task<BaseResult<string>> CodeIds(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token);
    }
}
