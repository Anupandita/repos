using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Wms.Foundation.Edm.Contracts.Interfaces;
using Sfc.Wms.Foundation.Edm.Contracts.Dtos;
using Sfc.Wms.App.Api.Contracts.Dto;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize]
    [RoutePrefix(Routes.Prefixes.ImageUrls)]
    public class ImageUrlsController : SfcBaseController
    {
        private readonly ISfcImageService _sfcImageService;
        public ImageUrlsController(ISfcImageService sfcImageService)
        {
            _sfcImageService = sfcImageService;
        }

        [HttpGet]
        [Route(Routes.Paths.ImageUrlsParams)]
        [ResponseType(typeof(BaseResult<ImageDto>))]
        public async Task<IHttpActionResult> GetImageAsync([FromUri] ImageUrlDto imageUrlDto)
        {
            var response = await _sfcImageService.GetItemImageAsync(imageUrlDto.sku, imageUrlDto?.gtin).ConfigureAwait(false);
            return ResponseHandler(response);
        }

    }
}