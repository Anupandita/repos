using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Core.BaseApiController;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Routes = Sfc.Wms.Interface.Asrs.Constants.Routes;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
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
            var response = await _emsToWmsMessageProcessorService.GetMessageAsync(msgKey)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}