using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Dto;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Core.OnPrem.Pagination;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.Common)]
    public class CommonController : SfcBaseController
    {
        private readonly ISystemCodeService _systemCodeService;

        public CommonController(ISystemCodeService systemCodeService)
        {
            _systemCodeService = systemCodeService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.Paths.CodeIds)]
        [ResponseType(typeof(BaseResult<IEnumerable<SysCodeDto>>))]
        public async Task<IHttpActionResult> GetSystemCodesAsync([FromUri] SystemCodeDto sysCodeDto)
        {
            //TODO: Need to update SystemCodeDto
            var resultResponse = await _systemCodeService.GetSystemCodeAsync(sysCodeDto.RecType, sysCodeDto.CodeType,
                sysCodeDto.CodeId, 
                new SortOption
                {
                    PropertyName = sysCodeDto.OrderByColumn,
                    OrderBy = sysCodeDto.OrderBy == "asc" ? ListSortDirection.Ascending : ListSortDirection.Descending
                });
              //  sysCodeDto.OrderByColumn, sysCodeDto.OrderBy);
            return ResponseHandler(resultResponse);
        }
    }
}