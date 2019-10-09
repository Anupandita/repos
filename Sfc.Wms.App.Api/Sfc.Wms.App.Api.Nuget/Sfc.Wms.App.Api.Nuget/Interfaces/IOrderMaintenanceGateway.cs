using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.Asrs.Contracts.Dtos;
using System.Threading.Tasks;

namespace Sfc.App.Api.Nuget.Interfaces
{
    public interface IOrderMaintenanceGateway
    {
        Task<BaseResult> CreateOrmtMessageByCartonNumberAsync(string cartonNumber, string actionCode);

        Task<BaseResult> CreateOrmtMessageByWaveNumberAsync(string waveNumber);
    }
}