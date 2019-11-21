using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface ICommonGateway
    {
        Task<BaseResult<T>> CodeIds<T>(string isWhseSysCode, string recType, string codeType, bool isNumber, string orderByColumn, string token);
    }
}
