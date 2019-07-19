using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Routes = Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Constants.Routes;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
{
    [RoutePrefix(Routes.DematicMessageComtPrefix)]
    public class ComtController : SfcBaseApiController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public ComtController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> CreateAsync([FromBody]ComtTriggerInput comtTriggerInput)
        {
            if (comtTriggerInput == null) return BadRequestHandler();

            var response = await _wmsToEmsMessageProcessorService.GetComtMessageAsync(comtTriggerInput)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}