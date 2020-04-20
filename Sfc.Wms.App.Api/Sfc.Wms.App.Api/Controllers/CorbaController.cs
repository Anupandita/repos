using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Foundation.Corba.Contracts.Dtos;
using Sfc.Wms.Foundation.Corba.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize]
    [RoutePrefix(Routes.Prefixes.Corba)]
    public class CorbaController : SfcBaseController
    {
        private readonly ICorbaService _corbaService;

        public CorbaController(ICorbaService corbaService)
        {
            _corbaService = corbaService;
        }

        [HttpPost]
        [Route(Routes.Paths.Batch)]
        [ResponseType(typeof(BaseResult<CorbaResponseDto>))]
        public async Task<IHttpActionResult> BatchCorbaAsync(string functionName, string isVector, List<CorbaDto> corbaDtos)
        {
            var response = await _corbaService.BatchCorbaAsync(functionName, isVector, corbaDtos).ConfigureAwait(false);
            return ResponseHandler(response);
        }

        [HttpPost]
        [Route(Routes.Paths.Single)]
        [ResponseType(typeof(BaseResult<CorbaResponseDto>))]
        public async Task<IHttpActionResult> SingleCorbaAsync(string functionName, string isVector, CorbaDto corbaDto)
        {
            var response = await _corbaService.SingleCorbaAsync(functionName, isVector, corbaDto).ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}