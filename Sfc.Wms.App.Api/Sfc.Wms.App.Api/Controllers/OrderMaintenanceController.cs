using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.DematicMessageOrmt)]
    public class OrderMaintenanceController : SfcBaseController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public OrderMaintenanceController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route(Routes.Paths.OrmtByCartonNumber)]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateOrmtMessageByCartonNumberAsync(string cartonNumber, string actionCode)
        {
            var result = await _wmsToEmsMessageProcessorService
                .GetOrmtMessageByCartonNumberAsync(cartonNumber, actionCode)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }

        [HttpPost]
        [Route(Routes.Paths.OrmtByWaveNumber)]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateOrmtMessageByWaveNumberAsync(string waveNumber)
        {
            var path = HttpContext.Current.Server.MapPath("~/");
            var result = await _wmsToEmsMessageProcessorService.GetOrmtMessageByWaveNumberAsync(waveNumber)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }
    }
}