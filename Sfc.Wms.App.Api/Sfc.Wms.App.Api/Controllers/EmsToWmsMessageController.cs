using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    public class EmsToWmsMessageController : SfcBaseController
    {
        private readonly IEmsToWmsMessageProcessorService _emsToWmsMessageProcessorService;

        public EmsToWmsMessageController(IEmsToWmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _emsToWmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route(Routes.Prefixes.EmsToWmsMessage)]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync(long msgKey, string msgProcessor)
        {
            var result = await _emsToWmsMessageProcessorService.GetMessageAsync(msgKey, msgProcessor)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }
    }
}