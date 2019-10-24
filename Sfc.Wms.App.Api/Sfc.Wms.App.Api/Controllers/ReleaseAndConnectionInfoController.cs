using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Dto;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.ConnectionInformation)]
    public class ReleaseAndConnectionInfoController : SfcBaseController
    {
        [HttpGet]
        [Route]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            var releaseAndConnectionInfo = new ConnectionInfo()
            {
                Database = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString,
            };
            return Json(releaseAndConnectionInfo);
        }
    }
}