using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.App.Contracts.Constants;
using Sfc.Wms.Result;
using Sfc.Wms.UserRbac.Contracts.Dtos;
using Sfc.Wms.UserRbac.Contracts.Dtos.UI;
using Sfc.Wms.UserRbac.Contracts.Interfaces;

namespace Sfc.App.Api.Controllers
{
    [RoutePrefix(Route.User)]
    public class UserRbacController : SfcBaseApiController
    {
        private readonly IUserRabcGateway _userRabcGateway;

        public UserRbacController(IUserRabcGateway userRabcGateway)
        {
            _userRabcGateway = userRabcGateway;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Route.Paths.UserLogin)]
        [ResponseType(typeof(BaseResult<UserDetailsDto>))]
        public async Task<IHttpActionResult> SignInAsync([FromBody] LoginCredentials loginCredentials)
        {
            var response = await _userRabcGateway.SignInAsync(loginCredentials).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}