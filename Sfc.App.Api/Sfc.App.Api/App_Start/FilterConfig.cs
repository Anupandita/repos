using System.Web.Http.Filters;
using System.Web.Mvc;
using Sfc.Wms.Aop.Logging;
using Sfc.Wms.Security.Token.Filters;

namespace Sfc.App.Api.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new IdentityJwtAuthenticationAttribute());
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new LogActionWebApiFilter());
            filters.Add(new LogExceptionWebApiFilter());
        }
    }
}