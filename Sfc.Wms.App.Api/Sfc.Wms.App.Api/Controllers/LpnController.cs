using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.Lpn)]
    public class LpnController : SfcBaseController
    {
        private readonly IFindLpnService _findLpnService;
        private readonly ILpnHistoryService _lpnHistoryService;
        public LpnController(IFindLpnService findLpnService, ILpnHistoryService lpnHistoryService)
        {
            _findLpnService = findLpnService;
            _lpnHistoryService = lpnHistoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.Paths.Find)]
        [ResponseType(typeof(BaseResult<List<FindLpnDto>>))]
        public async Task<IHttpActionResult> FindLpnAsync([FromUri]LpnParamModel lpnParamModel)
        {
            var response = await _findLpnService.FindLpnAsync(lpnParamModel).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.Paths.LpnHistory)]
        [ResponseType(typeof(BaseResult<List<LpnHistoryDto>>))]
        public async Task<IHttpActionResult> GetLpnHistoryAsync(string warehouse, string lpnNumber)
        {
            var response = await _lpnHistoryService.GetLpnHistoryAsync(warehouse, lpnNumber).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}