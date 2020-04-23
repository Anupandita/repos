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
            var builder = new System.Data.Common.DbConnectionStringBuilder
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString
            };
            var version = ConfigurationManager.AppSettings["Version"] as string;
            if (!string.IsNullOrEmpty(version)) version = $"-{version}";
            var server = builder["Data Source"] as string;
            var releaseAndConnectionInfo = new BaseResult<ConnectionInfo>()
            {
                ResultType = ResultTypes.Ok,
                Payload = new ConnectionInfo()
                {
                    Database = server?.Split('.')[0],
                    Environment = ConfigurationManager.AppSettings.Get(nameof(Environment)),
                    Version = $"{GetType().Assembly.GetName().Version}{version}"
                }
            };
            return Json(releaseAndConnectionInfo);
        }
    }
}
