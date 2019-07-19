using Sfc.Wms.Result;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Sfc.Wms.Asrs.Api.Controllers
{
    public class SfcBaseApiController : ApiController
    {
        protected string RouteName;
        protected object RouteValues;
        protected BaseResult BadRequestBaseResult => new BaseResult { ResultType = ResultTypes.BadRequest };

        protected IHttpActionResult ResponseHandler<T>(BaseResult<T> result)
        {
            switch (result.ResultType)
            {
                case ResultTypes.Ok: return Ok(result);
                case ResultTypes.Created: return Ok(result);
                case ResultTypes.BadRequest: return BadRequestHandler(result.Payload);
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

        private IHttpActionResult BadRequestHandler<T>(T response)
        {
            return Content(HttpStatusCode.BadRequest, new BaseResult<T>
            {
                Payload = response,
                ResultType = ResultTypes.BadRequest,
                ValidationMessages = ModelState.Values.SelectMany(v => v.Errors).Select(el => new ValidationMessage
                {
                    Message = el.Exception.Message,
                    FieldName = el.Exception.Source
                }).ToList()
            });
        }

        protected IHttpActionResult ResponseHandler(BaseResult result)
        {
            switch (result.ResultType)
            {
                case ResultTypes.Ok: return Ok(result);
                case ResultTypes.BadRequest: return Content(HttpStatusCode.BadRequest, result);
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

        protected IHttpActionResult CreatedResponseHandler(BaseResult result, string routeName,
            object routeValues)
        {
            return CreatedAtRoute(routeName, routeValues, result);
        }

        protected IHttpActionResult BadRequestHandler()
        {
            return Content(HttpStatusCode.BadRequest, new BaseResult
            {
                ResultType = ResultTypes.BadRequest,
                ValidationMessages = ModelState.Values.SelectMany(v => v.Errors).Select(el => new ValidationMessage
                {
                    Message = el.Exception.Message,
                    FieldName = el.Exception.Source
                }).ToList()
            });
        }
    }
}