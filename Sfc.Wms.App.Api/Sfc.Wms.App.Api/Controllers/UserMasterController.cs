using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.Api.Contracts.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize, RoutePrefix(Routes.Prefixes.User)]
    public class UserMasterController : SfcBaseController
    {
        private readonly IUserRbacService _userRbacService;

        public UserMasterController(IUserRbacService userRbacService)
        {
            _userRbacService = userRbacService;
        }

        [HttpPost, Route(Routes.Paths.Preferences)]
        [ResponseType(typeof(BaseResult<IEnumerable<PreferencesDto>>))]
        public async Task<IHttpActionResult> UpdateUserPreferences(IEnumerable<PreferencesDto> userPreferenceModel)
        {
            var response = await _userRbacService.UpsertUserPreferences(userPreferenceModel).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}