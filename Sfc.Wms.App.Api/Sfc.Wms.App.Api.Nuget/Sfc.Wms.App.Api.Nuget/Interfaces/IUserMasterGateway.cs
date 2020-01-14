using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.UserMaster.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IUserMasterGateway
    {
        Task<BaseResult> CreateUserPreferences(SwmUserSettingDto swmUserSettingDto, string token);
        Task<BaseResult> UpdateUserPreferences(UserPreferencesModel userPreferencesModel, string token);
    }
}
