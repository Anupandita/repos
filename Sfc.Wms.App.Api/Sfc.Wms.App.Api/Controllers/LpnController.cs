﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize]
    [RoutePrefix(Routes.Prefixes.Lpn)]
    public class LpnController : SfcBaseController
    {
        private readonly ILpnService _lpnService;

        public LpnController(ILpnService lpnService)
        {
            _lpnService = lpnService;
        }

        [HttpPost]
        [Route(Routes.Paths.LpnCommentsAdd)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> AddLpnCommentAsync(LpnCommentDto lpnCommentDto)
        {
            var response = await _lpnService.AddLpnCommentAsync(lpnCommentDto).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpDelete]
        [Route(Routes.Paths.LpnDeleteComments)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> DeleteLpnCommentAsync(string caseNumber, int commentSequenceNumber)
        {
            var response = await _lpnService.DeleteLpnCommentAsync(caseNumber, commentSequenceNumber).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.Find)]
        [ResponseType(typeof(BaseResult<FindLpnDto>))]
        public async Task<IHttpActionResult> FindLpnAsync([FromUri]LpnParamModel lpnParamModel)
        {
            var response = await _lpnService.FindLpnAsync(lpnParamModel).ConfigureAwait(false);
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
        [Route(Routes.Paths.LpnComments)]
        [ResponseType(typeof(BaseResult<List<CaseCommentDto>>))]
        public async Task<IHttpActionResult> GetLpnCommentsAsync(string lpnId)
        {
            var response = await _lpnService.GetLpnCommentsAsync(lpnId).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [Route(Routes.Paths.LpnDetails)]
        [ResponseType(typeof(BaseResult<List<CaseDetailDto>>))]
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
        [ResponseType(typeof(BaseResult<List<CaseDetailDto>>))]
        public async Task<IHttpActionResult> GetLpnLockUnlockAsync(string lpnId)
        {
            var response = await _lpnService.GetLpnLockUnlockAsync(lpnId).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPut]
        [Route(Routes.Paths.LpnUpdateDetails)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateLpnAsync(LpnUpdateDto lpnUpdate)
        {
            var response = await _lpnService.UpdateLpnDetails(lpnUpdate).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPut]
        [Route(Routes.Paths.LpnCaseDetails)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> UpdateLpnCaseDetailsAsync(LpnCaseDetailsUpdateDto lpnCaseDetailsUpdate)
        {
            var response = await _lpnService.UpdateLpnCaseDetails(lpnCaseDetailsUpdate).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}