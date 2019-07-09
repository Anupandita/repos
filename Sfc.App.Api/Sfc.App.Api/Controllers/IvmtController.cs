using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Core.BaseApiController;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Routes = Sfc.Wms.Interface.Asrs.Constants.Routes;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
{
    [RoutePrefix(Routes.DematicMessageIvmtPrefix)]
    public class IvmtController : SfcBaseController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public IvmtController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync([FromBody]IvmtTriggerInputDto ivmtTriggerInput)
        {
           

            var response = await _wmsToEmsMessageProcessorService.GetIvmtMessageAsync(ivmtTriggerInput)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}