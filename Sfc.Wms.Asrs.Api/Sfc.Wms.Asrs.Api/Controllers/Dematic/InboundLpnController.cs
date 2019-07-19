using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
{
    [RoutePrefix(Routes.InboundLpnsPrefix)]
    public class InboundLpnController : SfcBaseApiController
    {
        private readonly IInboundLpnService _inboundLpnService;

        public InboundLpnController(IInboundLpnService inboundLpnService)
        {
            _inboundLpnService = inboundLpnService;
        }

        [HttpPatch]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateCaseDtlQuantityAsync([FromBody]IvmtDto ivmtDtoPatch)
        {
            if (ivmtDtoPatch == null)
                return BadRequestHandler();

            var response = await _inboundLpnService.UpdateCaseDtlQuantityAsync(ivmtDtoPatch)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}