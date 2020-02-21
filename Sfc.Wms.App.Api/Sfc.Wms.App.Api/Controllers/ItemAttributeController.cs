using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Interface;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.ItemAttribute)]
    [Authorize]
    public class ItemAttributeController : SfcBaseController
    {
        private readonly IItemAttributeService _itemAttributeService;

        public ItemAttributeController(IItemAttributeService itemAttributeService)
        {
            _itemAttributeService = itemAttributeService;
        }

        [HttpGet]
        [Route(Routes.Paths.Search)]
        [ResponseType(typeof(BaseResult<ItemAttributeSearchResultDto>))]
        public async Task<IHttpActionResult> SearchAsync([FromUri]ItemAttributeSearchInputDto attributeSearchInputDto)
        {
            var response = await _itemAttributeService.AttributeSearchAsync(attributeSearchInputDto)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.DrillDownItemAttribute)]
        [ResponseType(typeof(BaseResult<ItemAttributeDetailsDto>))]
        public async Task<IHttpActionResult> AttributeDrillDownAsync(string itemId)
        {
            var response = await _itemAttributeService.AttributeDrillDownAsync(itemId)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}
