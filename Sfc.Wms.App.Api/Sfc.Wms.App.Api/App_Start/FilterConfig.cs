using System.Web.Http.Filters;
using Sfc.Wms.Aop.Logging;
using Sfc.Wms.Security.Token.Jwt.Filters;

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