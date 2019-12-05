using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IVelocityMasterGateway
    {
        Task<BaseResult<string>> GetVelocityMasterDetailsAsync(VelocityMasterModel velocityMasterModel, string token);
    }
}