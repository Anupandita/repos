using System;
using System.Net;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Core.BaseApiController;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Routes = Sfc.Wms.Interface.Asrs.Constants.Routes;

namespace Sfc.App.Api.Controllers
{
    [RoutePrefix(Routes.DematicMessageIvmtPrefix)]
    public class InventoryMaintenanceController : SfcBaseController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public InventoryMaintenanceController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync([FromBody]IvmtTriggerInputDto ivmtTriggerInput)
        {
           

            var result = await _wmsToEmsMessageProcessorService.GetIvmtMessageAsync(ivmtTriggerInput)
                .ConfigureAwait(false);
            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }
    }
}