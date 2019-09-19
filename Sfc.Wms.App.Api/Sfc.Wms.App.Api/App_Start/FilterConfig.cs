using System.Web.Http.Filters;
using Sfc.Core.Aop.WebApi.Logging;
using Sfc.Wms.Framework.Security.Token.Jwt.Filters;

namespace Sfc.Wms.App.Api
{
    public class FilterConfig
    {
        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new IdentityJwtAuthenticationAttribute());
            filters.Add(new LogActionWebApiFilter());
            filters.Add(new LogExceptionWebApiFilter());
        }
    }
}