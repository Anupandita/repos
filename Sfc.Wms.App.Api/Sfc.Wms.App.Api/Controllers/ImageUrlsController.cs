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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Http;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.ImageUrls)]
    public class ImageUrlsController : SfcBaseController
    {
        private readonly ISfcImageService _sfcImageService;
        public ImageUrlsController(ISfcImageService sfcImageService)
        {
            _sfcImageService = sfcImageService;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.Paths.ImageUrlsParams)]
        //[ResponseType(typeof(Image))]
        public async Task<HttpResponseMessage> Get([FromUri] string sku, string gtin = "")
        {
            HttpResponseMessage  httpResponseMessage = null;
            var response = await  _sfcImageService.GetItemImageAsync(sku, gtin).ConfigureAwait(false);
                     
            if (response.ResultType == ResultTypes.Ok)
                httpResponseMessage = ByteArrayToImage(response.Payload.ImageBlob);
            if (response.ResultType == ResultTypes.BadRequest)
                httpResponseMessage = null;
            if (response.ResultType == ResultTypes.NotFound)
                httpResponseMessage = null;

            return httpResponseMessage;
        }

        private HttpResponseMessage ByteArrayToImage(byte[] data)
        {
            //MemoryStream ms = new MemoryStream(data);
            //Image returnImage = Image.FromStream(ms);
            //return returnImage;

            MemoryStream ms = new MemoryStream(data);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");

            return response;
        }

    }
}