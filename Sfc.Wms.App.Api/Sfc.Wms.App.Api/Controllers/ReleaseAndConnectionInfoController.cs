using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Dto;
using System;
using System.Configuration;
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
        public IHttpActionResult Get()
        {
            var releaseAndConnectionInfo = new BaseResult<ConnectionInfo>()
            {
                ResultType = ResultTypes.Ok,
                Payload = new ConnectionInfo()
                {
                    Database = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString,
                    Environment = ConfigurationManager.AppSettings.Get(nameof(Environment))
                }
            };
            return Json(releaseAndConnectionInfo);
        }
    }
}