using Sfc.Core.BaseApiController;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Sfc.Wms.Result;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Routes = Sfc.Wms.Interface.Asrs.Constants.Routes;

namespace Sfc.App.Api.Controllers
{
    public class EmsToWmsMessageController : SfcBaseController
    {
        private readonly IEmsToWmsMessageProcessorSevice _emsToWmsMessageProcessorService;

        public EmsToWmsMessageController(IEmsToWmsMessageProcessorSevice wmsToEmsMessageProcessorService)
        {
            _emsToWmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route(Routes.EmsToWmsMessagePrefix)]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync([FromBody]long msgKey)
        {
            var result = await _emsToWmsMessageProcessorService.GetMessageAsync(msgKey)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }
    }
}