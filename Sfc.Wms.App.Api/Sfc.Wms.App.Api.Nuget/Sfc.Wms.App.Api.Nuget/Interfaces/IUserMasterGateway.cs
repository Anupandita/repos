using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IUserMasterGateway
    {
        Task<BaseResult<IEnumerable<PreferencesDto>>> UpdateUserPreferences(IEnumerable<PreferencesDto> preferencesDto, string token);
    }
}
