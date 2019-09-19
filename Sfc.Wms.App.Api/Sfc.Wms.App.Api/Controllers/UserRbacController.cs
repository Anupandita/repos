using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.App.Api.Contracts.Constants;
using Sfc.App.App.Interfaces;
using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.Security.Token.Jwt.Jwt;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Route.User)]
    public class UserRbacController : SfcBaseController
    {
        private readonly IRbacGateway _rbacGateway;

        public UserRbacController(IRbacGateway rbacGateway)
        {
            _rbacGateway = rbacGateway;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Route.Paths.UserLogin)]
        [ResponseType(typeof(BaseResult<UserInfoDto>))]
        public async Task<IHttpActionResult> SignInAsync(LoginCredentials loginCredentials)
        {
            var response = await _rbacGateway.SignInAsync(loginCredentials).ConfigureAwait(false);
            if (response.ResultType != ResultTypes.Ok) return ResponseHandler(response);
            HttpContext.Current?.Response.Headers.Add(Constants.Authorization,
                $"{Constants.Bearer} {response.Payload.Token}");
            response.Payload.Token = string.Empty;
            return ResponseHandler(response);
        }
    }
}