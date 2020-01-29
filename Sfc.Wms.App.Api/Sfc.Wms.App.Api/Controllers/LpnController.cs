using System;
using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize]
    [RoutePrefix(Routes.Prefixes.Lpn)]
    public class LpnController : SfcBaseController
    {
        private readonly ILpnService _lpnService;
        private readonly ICaseCommentService _caseCommentService;
        private readonly ICaseDetailService _caseDetailService;
        private readonly ICaseLockService _caseLockService;

        public LpnController(ILpnService lpnService, ICaseCommentService caseCommentService,
            ICaseDetailService caseDetailService, ICaseLockService caseLockService)
        {
            _lpnService = lpnService;
            _caseCommentService = caseCommentService;
            _caseDetailService = caseDetailService;
            _caseLockService = caseLockService;
        }

        [HttpPost]
        [Route(Routes.Paths.LpnCommentsAdd)]
        [ResponseType(typeof(BaseResult<CaseCommentDto>))]
        public async Task<IHttpActionResult> AddLpnCommentAsync(CaseCommentDto caseCommentDto)
        {
            var response = await _caseCommentService.InsertAsync(caseCommentDto)
                .ConfigureAwait(false);
            return response.ResultType == ResultTypes.Created ?
                CreatedAtRoute(nameof(CaseCommentDto), new { lpnId = caseCommentDto.CaseNumber }, response) : ResponseHandler(response);
        }

        [HttpDelete]
        [Route(Routes.Paths.LpnDeleteComments)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> DeleteLpnCommentAsync(string caseNumber, int commentSequenceNumber)
        {
            var response = await _caseCommentService.DeleteAsync(caseNumber, commentSequenceNumber)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.Find)]
        [ResponseType(typeof(BaseResult<LpnSearchResultsDto>))]
        public async Task<IHttpActionResult> LpnSearchAsync([FromUri]LpnParameterDto lpnParamDto)
        {
            var response = await _lpnService.LpnSearchAsync(lpnParamDto).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [Route(Routes.Paths.LpnAisleTrans)]
        [ResponseType(typeof(BaseResult<AisleTransactionDto>))]
        public async Task<IHttpActionResult> GetAisleTransactionAsync(string lpnId, string faceLocationId)
        {
            var response = await _lpnService.GetAisleTransactionAsync(lpnId, faceLocationId).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.LpnComments, Name = nameof(CaseCommentDto))]
        [ResponseType(typeof(BaseResult<List<CaseCommentDto>>))]
        public async Task<IHttpActionResult> GetLpnCommentsAsync(string lpnId)
        {
            var response = await _caseCommentService.GetLpnCommentsWithCodeDescriptionAsync(lpnId).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.LpnDetails)]
        [ResponseType(typeof(BaseResult<LpnDetailsDto>))]
        public async Task<IHttpActionResult> GetLpnDetailsAsync(string lpnId)
        {
            var response = await _lpnService.GetLpnDetailsAsync(lpnId).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.LpnHistory)]
        [ResponseType(typeof(BaseResult<List<LpnHistoryDto>>))]
        public async Task<IHttpActionResult> GetLpnHistoryAsync(string lpnNumber, string warehouse)
        {
            var response = await _lpnService.GetLpnHistoryAsync(warehouse, lpnNumber).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.LpnLockUnlock)]
        [ResponseType(typeof(BaseResult<List<CaseLockUnlockDto>>))]
        public async Task<IHttpActionResult> GetLpnLockUnlockAsync(string lpnId)
        {
            var response = await _caseLockService.GetCaseLockUnlockAsync(lpnId).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.CaseUnlock)]
        [ResponseType(typeof(BaseResult<List<CaseLockDto>>))]
        public async Task<IHttpActionResult> GetCaseUnLockDetailsAsync([FromUri]IEnumerable<string> lpnIds)
        {
            var response = await _caseLockService.GetCaseUnLockDetailsAsync(lpnIds).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPut]
        [Route(Routes.Paths.LpnUpdateDetails)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateLpnHeaderAsync(LpnHeaderUpdateDto lpnUpdate)
        {
            var response = await _lpnService.UpdateLpnDetailsAsync(lpnUpdate).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPut]
        [Route(Routes.Paths.LpnCommentsAdd)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateLpnCaseDetailsAsync(LpnDetailsUpdateDto lpnCaseDetailsUpdate)
        {
            var response = await _caseDetailService.UpdateCaseDetailAssortAndCutNumberAsync(lpnCaseDetailsUpdate).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPut]
        [Route(Routes.Paths.LpnCommentUpdate)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateLpnCommentAsync(CaseCommentDto caseCommentDto)
        {
            var response = await _caseCommentService.UpdateAsync(caseCommentDto).ConfigureAwait(false);
            return ResponseHandler((BaseResult)response);
        }

        [HttpPost]
        [Route(Routes.Paths.LpnMultipleUnlock)]
        [ResponseType(typeof(BaseResult<LpnMultipleUnlockResultDto>))]
        public async Task<IHttpActionResult> UnlockCommentWithBatchCorbaAsync(List<LpnMultipleUnlockDto> lpnMultipleUnlockDto)
        {
            var response = await _caseCommentService.UnlockCommentWithBatchCorbaAsync(lpnMultipleUnlockDto).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [Route(Routes.Paths.LpnMultiplelock)]
        [ResponseType(typeof(BaseResult<LpnMultipleUnlockResultDto>))]
        public async Task<IHttpActionResult> CaseLockCommentWithBatchCorbaAsync([FromBody]CaseLockCommentDto caseLockComment)
        {
            var response = await _caseCommentService.AddCaseLockCommentWithBatchCorbaAsync(caseLockComment).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}