using System.Collections.Generic;
using Sfc.Wms.Configuration.UserMaster.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Contracts.Entities
{

    public class UserPreferencesModel
    {
        public IEnumerable<SwmUserSettingDto> data { get; set; }
    }
}