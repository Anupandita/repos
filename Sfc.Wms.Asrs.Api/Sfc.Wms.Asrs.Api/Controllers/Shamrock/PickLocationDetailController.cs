using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;

namespace Sfc.Wms.Asrs.Api.Controllers.Shamrock
{
    [RoutePrefix(Routes.PickLocationDtlPrefix)]
    public class PickLocationDetailController : SfcBaseApiController
    {
        private readonly IPickLocationService _pickLocationService;

        public PickLocationDetailController(IPickLocationService pickLocationService)
        {
            _pickLocationService = pickLocationService;
        }

        [HttpPatch]
        [Route(Routes.PickLocationDtlByLocationId)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateAsync([FromBody]CostDto costDto)
        {
            if (costDto == null)
                return BadRequestHandler();

            var response = await _pickLocationService.UpdateQuantityAsync(costDto)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}