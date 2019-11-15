using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IEmsToWmsMessageGateway
    {
        Task<BaseResult> CreateAsync(long msgKey);
    }
}