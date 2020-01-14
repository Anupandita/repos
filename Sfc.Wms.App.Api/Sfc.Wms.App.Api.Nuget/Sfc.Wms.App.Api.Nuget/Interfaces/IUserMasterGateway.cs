using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.UserMaster.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IUserMasterGateway
    {
        Task<BaseResult> CreateUserPreferences(SwmUserSettingDto swmUserSettingDto, string token);
        Task<BaseResult> UpdateUserPreferences(SwmUserSettingDto swmUserSettingDto, string token);
    }
}
