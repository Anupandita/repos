using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Data.Entities;

namespace Sfc.Wms.Asrs.Api.Controllers.Dematic
{
    [RoutePrefix(Routes.EmsToWmsPrefix)]
    public class EmsToWmsController : SfcBaseApiController
    {
        private readonly IEmsToWmsService _emsToWmsService;

        public EmsToWmsController(IEmsToWmsService emsToWmsService)
        {
            _emsToWmsService = emsToWmsService;
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<IEnumerable<EmsToWmsDto>>))]
        public async Task<IHttpActionResult> GetAsync()
        {
            var response = await _emsToWmsService.GetAsync().ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<EmsToWmsDto>))]
        [Route(Routes.ByPrcAndMsgKey, Name = Routes.EmsToWms)]
        public async Task<IHttpActionResult> GetAsync(string prc, long msgKey)
        {
            var response = await _emsToWmsService.GetAsync(_ => _.Process == prc && _.MessageKey == msgKey)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.ByStatus)]
        [ResponseType(typeof(BaseResult<EmsToWmsDto>))]
        public async Task<IHttpActionResult> GetAsync(RecordStatus status)
        {
            var response = await _emsToWmsService.GetAsync(_ => _.Status == status.ToString())
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> InsertAsync(EmsToWmsDto emsToWmsDto)
        {
            if (emsToWmsDto == null)
                return ResponseHandler(BadRequestBaseResult);

            var response = await _emsToWmsService
                .InsertAsync(emsToWmsDto, _ => _.Process == emsToWmsDto.Process && _.MessageKey == emsToWmsDto.MessageKey)
                .ConfigureAwait(false);

            if (response.ResultType != ResultTypes.Created) return ResponseHandler(response);

            RouteName = Routes.EmsToWms;
            RouteValues = new { prc = emsToWmsDto.Process, msgKey = emsToWmsDto.MessageKey };
            return ResponseHandler(response);
        }

        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateAsync(EmsToWmsDto emsToWmsDto)
        {
            if (emsToWmsDto == null)
                return ResponseHandler(BadRequestBaseResult);

            var response = await _emsToWmsService
                .UpdateAsync(emsToWmsDto, _ => _.Process == emsToWmsDto.Process && _.MessageKey == emsToWmsDto.MessageKey)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpDelete]
        [Route(Routes.ByPrcAndMsgKey)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> DeleteAsync(string prc, long msgKey)
        {
            if (string.IsNullOrWhiteSpace(prc) || msgKey == 0)
                return ResponseHandler(BadRequestBaseResult);

            var response = await _emsToWmsService.DeleteAsync(_ => _.Process == prc && _.MessageKey == msgKey).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}