using Sfc.Core.BaseApiController;
using Sfc.Wms.Interface.Asrs.Dtos;
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
    [RoutePrefix(Routes.DematicMessageComtPrefix)]
    public class ContainerMaintenanceController : SfcBaseController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public ContainerMaintenanceController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync([FromBody]ComtTriggerInputDto comtTriggerInput)
        {
            var result = await _wmsToEmsMessageProcessorService.GetComtMessageAsync(comtTriggerInput)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }
    }
}