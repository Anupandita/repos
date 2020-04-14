using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.Api.Contracts.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SFC.Core.Extensions;
using Sfc.Core.OnPrem.Pagination;
using Sfc.Wms.Configuration.SystemCode.Contracts.UoW.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.UoW.Enumerations;
using Sfc.Wms.Configuration.SystemCode.Contracts.UoW.Interfaces;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize, RoutePrefix("system-code")]
    public class SystemCodeController : SfcBaseController
    {
        private readonly ISystemCodeService _systemCodeService;

        public SystemCodeController(ISystemCodeService systemCodeService)
        {
            _systemCodeService = systemCodeService;
        }

        [HttpGet, Route("get")]
        [ResponseType(typeof(BaseResult<IEnumerable<SysCodeDto>>))]
        public async Task<IHttpActionResult> GetSystemCode(string warehouse, string rectType,
            string codeType, string codeId)
        {
            var actualResult = _systemCodeService.GetWhseSystemCodeAsync("008",
                RectType.BaseType.ToString().Substring(0, 1), BaseType.EquipmentClasses.GetDescription(), "S20", new SortOption()).Result;
            var result = await _systemCodeService.GetWhseSystemCodeAsync(warehouse, rectType, codeType, codeId, new SortOption());
            return ResponseHandler(actualResult);
        }
    }
}