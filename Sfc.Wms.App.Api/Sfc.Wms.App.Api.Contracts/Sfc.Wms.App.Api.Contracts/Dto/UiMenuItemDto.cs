using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Dto
{
   public class UiMenuItemDto
    {
        public string DisplayName { get; set; }
        public MenuRouteDto RouteTo { get; set; }
    }
}
