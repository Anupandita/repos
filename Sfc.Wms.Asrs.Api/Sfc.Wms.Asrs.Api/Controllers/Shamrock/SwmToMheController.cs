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
    [RoutePrefix(Routes.SwmToMhe.Prefix)]
    public class SwmToMheController : SfcBaseApiController
    {
        private readonly ISwmToMheService _swmToMheService;

        public SwmToMheController(ISwmToMheService swmToMheService)
        {
            _swmToMheService = swmToMheService;
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<IEnumerable<SwmToMhe>>))]
        public async Task<IHttpActionResult> GetAsync()
        {
            var response = await _swmToMheService.GetAsync().ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<SwmToMhe>))]
        [Route(Routes.SwmToMhe.ByPrcAndMsgKey, Name = Routes.SwmToMhe.GetByKeyRouteName)]
        public async Task<IHttpActionResult> GetAsync(string process, int key)
        {
            var response = await _swmToMheService
                .GetAsync(_ => _.SourceMessageProcess == process && _.SourceMessageKey == key)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult<IEnumerable<SwmToMhe>>))]
        [Route(Routes.SwmToMhe.ByStatus)]
        public async Task<IHttpActionResult> GetAsync(RecordStatus status)
        {
            var statusValue = (int)status;
            var response = await _swmToMheService.GetAsync(_ => _.MessageStatus == statusValue)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> InsertAsync(SwmToMheDto swmToMheDto)
        {
            if (swmToMheDto == null || !ModelState.IsValid)
                return BadRequestHandler();

            var response = await _swmToMheService.InsertAsync(swmToMheDto,
                _ => _.SourceMessageProcess == swmToMheDto.SourceMessageProcess &&
                   _.SourceMessageKey == swmToMheDto.SourceMessageKey).ConfigureAwait(false);

            if (response.ResultType == ResultTypes.Created)
                return CreatedResponseHandler(response, Routes.SwmFromMhe.GetByKeyRouteName,
                    new { process = swmToMheDto.SourceMessageProcess, key = swmToMheDto.SourceMessageKey });
            return ResponseHandler(response);
        }

        [HttpPut]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateAsync(SwmToMheDto swmToMheDto)
        {
            if (swmToMheDto == null || !ModelState.IsValid)
                return BadRequestHandler();

            var response = await _swmToMheService.UpdateAsync(swmToMheDto,
                _ => _.SourceMessageProcess == swmToMheDto.SourceMessageProcess &&
                          _.SourceMessageKey == swmToMheDto.SourceMessageKey).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpDelete]
        [ResponseType(typeof(BaseResult))]
        [Route(Routes.SwmToMhe.ByPrcAndMsgKey)]
        public async Task<IHttpActionResult> DeleteAsync(string process, int key)
        {
            if (string.IsNullOrEmpty(process) || string.IsNullOrWhiteSpace(process) || key == 0)
                return BadRequestHandler();

            var response = await _swmToMheService
                .DeleteAsync(_ => _.SourceMessageProcess == process && _.SourceMessageKey == key)
                .ConfigureAwait(false);

            return ResponseHandler(response);
        }
    }
}