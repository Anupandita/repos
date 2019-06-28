using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.App.Contracts.Constants;
using Sfc.App.Interfaces;
using Sfc.Wms.BaseController;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Api.Controllers
{
    [RoutePrefix(Route.User)]
    public class UserRbacController : SfcBaseController
    {
        private readonly IRbacGateway _rabcGateway;

        public UserRbacController(IRbacGateway rabcGateway)
        {
            _rabcGateway = rabcGateway;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Route.Paths.UserLogin)]
        [ResponseType(typeof(BaseResult<UserDetailsDto>))]
        public async Task<IHttpActionResult> SignInAsync( LoginCredentials loginCredentials)
        {
            var response = await _rabcGateway.SignInAsync(loginCredentials).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}