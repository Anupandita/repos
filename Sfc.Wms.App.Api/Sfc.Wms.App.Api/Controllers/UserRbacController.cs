using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Core.BaseApiController;
using JwtConstants = Sfc.Wms.Framework.Security.Token.Jwt.Jwt.Constants;
using Sfc.Wms.App.Api.Interfaces;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.User)]
    public class UserRbacController : SfcBaseController
    {
        private readonly IRbacService _rbacService;

        public UserRbacController(IRbacService rbacService)
        {
            _rbacService = rbacService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.Paths.UserLogin)]
        [ResponseType(typeof(BaseResult<UserInfoDto>))]
        public async Task<IHttpActionResult> SignInAsync(LoginCredentials loginCredentials)
        {
            if (loginCredentials == null)
                return ResponseHandler(BadRequestBaseResult);
            var resultResponse = await _rbacService.SignInAsync(loginCredentials).ConfigureAwait(false);
            if (resultResponse.Payload != null && resultResponse.ResultType == ResultTypes.Ok)
            {
                HttpContext.Current?.Response.Headers.Add(Constants.Token,
                    $"{JwtConstants.Bearer} {resultResponse.Payload.Token}");
                resultResponse = await _rbacService.GetPrinterValuesAsyc(resultResponse.Payload);
                resultResponse.Payload.Token = string.Empty;
            }

            return ResponseHandler(resultResponse);
        }
    }
}