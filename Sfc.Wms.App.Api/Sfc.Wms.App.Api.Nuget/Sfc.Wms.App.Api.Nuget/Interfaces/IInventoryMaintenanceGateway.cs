using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.Asrs.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IInventoryMaintenanceGateway
    {
        Task<BaseResult> CreateAsync(IvmtTriggerInputDto ivmtTriggerInput);
    }
}