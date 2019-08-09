using Sfc.App.App.Interfaces;
using Sfc.Core.BaseApiController;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;
using Sfc.Wms.Security.Contracts.Dtos.UI;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Route = Sfc.App.Api.Contracts.Constants.Route;

namespace Sfc.App.Api.Controllers
{
    [RoutePrefix(Route.User)]
    public class UserRbacController : SfcBaseController
    {
        private readonly IRbacGateway _rbacGateway;

        public UserRbacController(IRbacGateway rabcGateway)
        {
            _rbacGateway = rabcGateway;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Route.Paths.UserLogin)]
        [ResponseType(typeof(BaseResult<UserDetailsDto>))]
        public async Task<IHttpActionResult> SignInAsync(LoginCredentials loginCredentials)
        {
            var response = await _rbacGateway.SignInAsync(loginCredentials).ConfigureAwait(false);
            return ResponseHandler(response);
        }
        [HttpPost]
        [Route("12")]
        [ResponseType(typeof(BaseResult<UserDetailsDto>))]
        public async Task<IHttpActionResult> Test(LoginCredentials loginCredentials)
        {
            var response = new BaseResult<UserDetailsDto>();// await _rbacGateway.SignInAsync(loginCredentials).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}