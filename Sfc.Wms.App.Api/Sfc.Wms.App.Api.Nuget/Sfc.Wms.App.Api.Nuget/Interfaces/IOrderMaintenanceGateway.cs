using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IOrderMaintenanceGateway
    {
        Task<BaseResult> CreateOrmtMessageByCartonNumberAsync(string cartonNumber, string actionCode);

        Task<BaseResult> CreateOrmtMessageByWaveNumberAsync(string waveNumber);
    }
}