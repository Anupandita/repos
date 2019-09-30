using System;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
{
    public interface IVelocityMasterGateway
    {
        Task<BaseResult<string>> GetVelocityMasterDetailsAsync(VelocityMasterModel velocityMasterModel, string token);
     }
}