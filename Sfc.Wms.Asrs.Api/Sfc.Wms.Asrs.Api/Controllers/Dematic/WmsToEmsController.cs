using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
{
    [RoutePrefix(Routes.WmsToEmsPrefix)]
    public class WmsToEmsController : SfcBaseApiController
    {
        private readonly IWmsToEmsService _wmsToEmsService;

        public WmsToEmsController(IWmsToEmsService wmsToEmsService)
        {
            _wmsToEmsService = wmsToEmsService;
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<IEnumerable<WmsToEmsDto>>))]
        public async Task<IHttpActionResult> GetAsync()
        {
            var response = await _wmsToEmsService.GetAsync().ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.ByPrcAndMsgKey, Name = Routes.WmsToEms)]
        [ResponseType(typeof(BaseResult<WmsToEmsDto>))]
        public async Task<IHttpActionResult> GetAsync(string prc, long msgKey)
        {
            var response = await _wmsToEmsService
                .GetAsync(_ => _.Process == prc && _.MessageKey == msgKey).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.ByStatus)]
        [ResponseType(typeof(BaseResult<WmsToEmsDto>))]
        public async Task<IHttpActionResult> GetAsync(RecordStatus status)
        {
            var response = await _wmsToEmsService
                .GetAsync(_ => _.Status == status.ToString()).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> InsertAsync(WmsToEmsDto wmsToEmsDto)
        {
            if (wmsToEmsDto == null)
                return ResponseHandler(BadRequestBaseResult);

            var response = await _wmsToEmsService
                .InsertAsync(wmsToEmsDto, _ => _.Process == wmsToEmsDto.Process && _.MessageKey == wmsToEmsDto.MessageKey).ConfigureAwait(false);

            if (response.ResultType != ResultTypes.Created) return ResponseHandler(response);

            RouteName = Routes.EmsToWms;
            RouteValues = new { prc = wmsToEmsDto.Process, msgKey = wmsToEmsDto.MessageKey };
            return ResponseHandler(response);
        }

        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateAsync(WmsToEmsDto wmsToEmsDto)
        {
            if (wmsToEmsDto == null)
                return ResponseHandler(BadRequestBaseResult);

            var response = await _wmsToEmsService
                .UpdateAsync(wmsToEmsDto, _ => _.Process == wmsToEmsDto.Process && _.MessageKey == wmsToEmsDto.MessageKey).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpDelete]
        [Route(Routes.ByPrcAndMsgKey)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> DeleteAsync(string prc, long msgKey)
        {
            if (string.IsNullOrEmpty(prc) || string.IsNullOrWhiteSpace(prc) || msgKey == 0)
                return ResponseHandler(BadRequestBaseResult);

            var response = await _wmsToEmsService
                .DeleteAsync(_ => _.Process == prc && _.MessageKey == msgKey).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}