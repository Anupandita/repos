using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.Configuration.UserMaster.Contracts.Dtos;
using Sfc.Wms.Configuration.UserMaster.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize, RoutePrefix(Routes.Prefixes.User)]
    public class UserMasterController : SfcBaseController
    {
        private readonly ISwmUserSettingService _swmUserSettingService;

        public UserMasterController(ISwmUserSettingService swmUserSettingService)
        {
            _swmUserSettingService = swmUserSettingService;
        }

        [HttpPost, Route(Routes.Paths.Preferences)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateUserPreferences(UserPreferencesModel userPreferenceModel)
        {
            var response = await _swmUserSettingService.UpsertUserPreferences(userPreferenceModel.data);
            return ResponseHandler(response);
        }
    }
}