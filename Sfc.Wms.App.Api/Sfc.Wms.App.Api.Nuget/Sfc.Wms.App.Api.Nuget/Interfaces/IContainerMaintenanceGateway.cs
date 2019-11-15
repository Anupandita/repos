using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.Asrs.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IContainerMaintenanceGateway
    {
        Task<BaseResult> CreateAsync(ComtTriggerInputDto comtTriggerInput);
    }
}