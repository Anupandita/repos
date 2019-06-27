using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using Sfc.Wms.Result;

namespace Sfc.App.Api.Controllers
{
    public class SfcBaseApiController : ApiController
    {
        private int _userId;

        private ClaimsIdentity UserClaimsIdentity => (ClaimsIdentity) RequestContext.Principal.Identity;

        protected int UserId
        {
            get
            {
                if (_userId == 0)
                    int.TryParse(
                        UserClaimsIdentity.Claims.SingleOrDefault(el => el.Type == ClaimTypes.NameIdentifier)?.Value,
                        out _userId);

                return _userId;
            }
        }

        public IHttpActionResult ResponseHandler<T>(BaseResult<T> result) where T : class
        {
            switch (result.ResultType)
            {
                //  case ResultTypes.Authorized: return Ok(result);

                case ResultTypes.Ok: return Ok(result);

                case ResultTypes.BadRequest: return BadRequestHandler(result.Payload);

                case ResultTypes.Unauthorized: return Content(HttpStatusCode.Unauthorized, result);

                default: return NotFound();
            }
        }


        public IHttpActionResult BadRequestHandler<T>(T result) where T : class
        {
            return Content(HttpStatusCode.BadRequest, new BaseResult<T>
            {
                Payload = result,
                ResultType = ResultTypes.BadRequest,
                ValidationMessages = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(el => new ValidationMessage(el.Exception.Message,
                        el.Exception.Source)
                    ).ToList()
            });
        }

        public IHttpActionResult ResponseHandler(BaseResult result)
        {
            switch (result.ResultType)
            {
                case ResultTypes.Ok: return Ok(result);
                case ResultTypes.BadRequest: return BadRequestHandler();
                case ResultTypes.Conflict: return Content(HttpStatusCode.Conflict, result);
                case ResultTypes.InternalServerError: return Content(HttpStatusCode.InternalServerError, result);
                case ResultTypes.ServiceUnavailable: return Content(HttpStatusCode.ServiceUnavailable, result);
                case ResultTypes.Unauthorized: return Content(HttpStatusCode.Unauthorized, result);
                case ResultTypes.Forbidden: return Content(HttpStatusCode.Forbidden, result);
                case ResultTypes.NotCompleted: return Content(HttpStatusCode.NotModified, result);
                case ResultTypes.ExpectationFailed: return Content(HttpStatusCode.ExpectationFailed, result);
                default: return Content(HttpStatusCode.NotFound, result);
            }
        }

        public IHttpActionResult BadRequestHandler()
        {
            return Content(HttpStatusCode.BadRequest, new BaseResult
            {
                ResultType = ResultTypes.BadRequest,
                ValidationMessages = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(el => new ValidationMessage(el.Exception.Message,
                        el.Exception.Source)).ToList()
            });
        }
    }
}