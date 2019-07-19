using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.Asrs.Api.Controllers.Shamrock
{
    [RoutePrefix(Routes.SwmFromMhe.Prefix)]
    public class SwmFromMheController : SfcBaseApiController
    {
        private readonly ISwmFromMheService _swmFromMheService;

        public SwmFromMheController(ISwmFromMheService swmFromMheService)
        {
            _swmFromMheService = swmFromMheService;
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<IEnumerable<SwmFromMhe>>))]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var response = await _swmFromMheService.GetAsync().ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<SwmFromMhe>))]
        [Route(Routes.SwmFromMhe.ByPrcAndMsgKey, Name = Routes.SwmFromMhe.GetByKeyRouteName)]
        public async Task<IHttpActionResult> GetAsync(string process, int key)
        {
            var response = await _swmFromMheService
                .GetAsync(_ => _.SourceMessageProcess == process && _.SourceMessageKey == key)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<IEnumerable<SwmFromMhe>>))]
        [Route(Routes.SwmFromMhe.ByStatus)]
        public async Task<IHttpActionResult> GetAsync(RecordStatus status)
        {
            var statusValues = (int)status;
            var response = await _swmFromMheService.GetAsync(_ => _.MessageStatus == statusValues)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        [Route("")]
        public async Task<IHttpActionResult> InsertAsync(SwmFromMheDto swmFromMheDto)
        {
            if (swmFromMheDto == null)
                return BadRequestHandler();

            var response = await _swmFromMheService
                .InsertAsync(swmFromMheDto, _ => _.SourceMessageProcess == swmFromMheDto.SourceMessageProcess
                                                 && _.SourceMessageKey == swmFromMheDto.SourceMessageKey)
                .ConfigureAwait(false);
            if (response.ResultType == ResultTypes.Created)
                return CreatedResponseHandler(response, Routes.SwmFromMhe.GetByKeyRouteName,
                    new { process = swmFromMheDto.SourceMessageProcess, key = swmFromMheDto.SourceMessageKey });
            return ResponseHandler(response);
        }

        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        [Route("")]
        public async Task<IHttpActionResult> UpdateAsync(SwmFromMheDto swmFromMheDto)
        {
            if (swmFromMheDto == null)
                return BadRequestHandler();

            var response = await _swmFromMheService.UpdateAsync(swmFromMheDto,
                _ => _.SourceMessageProcess == swmFromMheDto.SourceMessageProcess
                && _.SourceMessageKey == swmFromMheDto.SourceMessageKey).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpDelete]
        [ResponseType(typeof(BaseResult))]
        [Route(Routes.SwmFromMhe.ByPrcAndMsgKey)]
        public async Task<IHttpActionResult> DeleteAsync(string process, int key)
        {
            if (string.IsNullOrEmpty(process) || string.IsNullOrWhiteSpace(process) || key == 0)
                return BadRequestHandler();

            var response = await _swmFromMheService
                .DeleteAsync(_ => _.SourceMessageProcess == process && _.SourceMessageKey == key)
                .ConfigureAwait(false);

            return ResponseHandler(response);
        }
    }
}