using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Dto
{
   public class Menu
    {
        public string[] ChildMenus { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public int MenuId { get; set; }
    }
}
