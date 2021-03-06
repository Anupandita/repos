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
    [RoutePrefix(Routes.Prefixes.DematicMessageSkmt)]
    public class SkuMaintenanceController : SfcBaseController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;
        private readonly IWmsToEmsParallelProcessService _wmsToEmsParallelProcessService;

        public SkuMaintenanceController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService,
            IWmsToEmsParallelProcessService wmsToEmsParallelProcessService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
            _wmsToEmsParallelProcessService = wmsToEmsParallelProcessService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync(string skuId, string actionCode)
        {
            var result = await _wmsToEmsMessageProcessorService.GetSkmtMessageAsync(skuId, actionCode)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }

        [HttpPost]
        [Route(Routes.Paths.SkmtWrapper)]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync(string actionCode)
        {
            var result = await _wmsToEmsParallelProcessService.StartSkmtWrapperAsync(actionCode)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }
    }
}