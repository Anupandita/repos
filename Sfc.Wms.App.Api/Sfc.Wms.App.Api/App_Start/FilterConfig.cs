using System.Data.Entity.Infrastructure.Interception;
using Sfc.Wms.Framework.Security.Token.Jwt.Filters;
using System.Web.Http.Filters;
using Sfc.Core.Aop.WebApi.Logging;

namespace Sfc.Wms.App.Api
{
    public class FilterConfig
    {
        public static void RegisterHttpFilters(HttpFilterCollection filters, SfcLogger sfcLogger)
        {
            filters.Add(new IdentityJwtAuthenticationAttribute());
            filters.Add(new LogActionWebApiFilter(sfcLogger));
            filters.Add(new LogExceptionWebApiFilter(sfcLogger));
        }

        public static void RegisterDbInterceptor(QueryLogger queryLogger)
        {
            DbInterception.Add(new CommandInterceptor());
        }
    }
}