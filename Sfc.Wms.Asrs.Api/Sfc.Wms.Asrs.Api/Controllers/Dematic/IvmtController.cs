using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Routes = Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Constants.Routes;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
{
    [RoutePrefix(Routes.DematicMessageIvmtPrefix)]
    public class IvmtController : SfcBaseApiController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public IvmtController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> CreateAsync([FromBody]IvmtTriggerInputDto ivmtTriggerInput)
        {
            if (ivmtTriggerInput == null)
                return BadRequestHandler();

            var response = await _wmsToEmsMessageProcessorService.GetIvmtMessageAsync(ivmtTriggerInput)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}