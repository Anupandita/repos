using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.MessageMaster.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IMessageMasterGateway
    {
        Task<BaseResult<IEnumerable<MessageDetailDto>>> GetMessageDetailsAsync(string token);
    }
}