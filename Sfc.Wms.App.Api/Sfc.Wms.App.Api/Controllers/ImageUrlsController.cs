using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using Sfc.Wms.Foundation.Edm.Contracts.Interfaces;
using System.IO;
using RestSharp.Extensions;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.ImageUrls)]
    public class ImageUrlsController : SfcBaseController
    {
        private readonly ISfcImageService _sfcImageService;
        public ImageUrlsController(ISfcImageService sfcImageService)
        {
            _sfcImageService = sfcImageService;

            //var context = new MediaDbContext("MediaDbContext");
            //var _imageRepository = new ImageRepository(context);
            //_sfcImageService = new SfcImageService(_imageRepository);
        }
        

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.Paths.ImageUrlsParams)]
        //[ResponseType(typeof(Image))]
        public async Task<HttpResponseMessage> Get([FromUri] string sku, string gtin = "")
        {
            HttpResponseMessage  httpResponseMessage = null;
            var response = await  _sfcImageService.GetItemImageAsync(sku, gtin, 0, "T").ConfigureAwait(false);

            if (response.ResultType == ResultTypes.Ok)
                httpResponseMessage = ByteArrayToImage(response.Payload.ImageBlob);
            else
                return new HttpResponseMessage((HttpStatusCode)response.ResultType) { Content = new StringContent($"Result={response.ResultType};  Sku={response.Payload.Sku}") };

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