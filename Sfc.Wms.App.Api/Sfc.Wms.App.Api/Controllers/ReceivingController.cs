using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Wms.App.Contracts.Entities;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize, RoutePrefix("receiving")]
    public class ReceivingController : SfcBaseController
    {
        private readonly IInboundReceivingService _receivingService;
        private readonly IAsnLotTrackingService _lotTrackingService;
        private readonly IQuestionAnswerService _answerService;

        public ReceivingController(IInboundReceivingService receivingService
            , IAsnLotTrackingService lotTrackingService, IQuestionAnswerService answerService)
        {
            _receivingService = receivingService;
            _lotTrackingService = lotTrackingService;
            _answerService = answerService;
        }

        [HttpGet, Route("")]
        [AllowAnonymous]
        [ResponseType(typeof(BaseResult<SearchResultDto>))]
        public async Task<IHttpActionResult> SearchAsync([FromUri]ReceiptInquiryDto receiptInquiryDto)
        {
            var result = await _receivingService.ReceivingSearchAsync(receiptInquiryDto).ConfigureAwait(false);

            return ResponseHandler(result);
        }

        [HttpGet, Route("asn-details")]
        [AllowAnonymous]
        [ResponseType(typeof(BaseResult<IEnumerable<AsnDrillDownDetailsDto>>))]
        public async Task<IHttpActionResult> GetAsnDetailsAsync(string shipmentNumber)
        {
            var result = await _receivingService.GetAsnDrillDownDetailsAsync(shipmentNumber).ConfigureAwait(false);

            return ResponseHandler(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateAnswerTextAsync(AnswerTextDto answerTextDto)
        {
            var result = await _answerService.UpdateAnswerTextAsync(answerTextDto).ConfigureAwait(false);

            return ResponseHandler(result);
        }

        [HttpGet, Route("questions")]
        [AllowAnonymous]
        [ResponseType(typeof(BaseResult<List<QVDetails>>))]
        public async Task<IHttpActionResult> GetQvDetails(string shipmentNumber)
        {
            var result = await _receivingService.GetQvDetailsAsync(shipmentNumber).ConfigureAwait(false);
            return ResponseHandler(result);
        }

        [HttpGet, Route("lot-tracking-details")]
        [AllowAnonymous]
        [ResponseType(typeof(BaseResult<IEnumerable<AsnLotTrackingDto>>))]
        public async Task<IHttpActionResult> GetAsnLotTrackByShipmentAndSkuIdAsync(string shipmentNumber, string skuId)
        {
            var result = await _lotTrackingService.GetByShipmentAndSkuIdAsync(shipmentNumber, skuId).ConfigureAwait(false);

            return ResponseHandler(result);
        }

    }
}
