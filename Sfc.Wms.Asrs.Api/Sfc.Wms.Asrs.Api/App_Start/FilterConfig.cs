using System.Web.Http.Filters;
using System.Web.Mvc;

//using WMSAop.Logging;

namespace Sfc.Wms.Asrs.Api.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            //filters.Add(new LogActionWebApiFilter());
            //filters.Add(new LogExceptionWebApiFilter());
        }
    }
}