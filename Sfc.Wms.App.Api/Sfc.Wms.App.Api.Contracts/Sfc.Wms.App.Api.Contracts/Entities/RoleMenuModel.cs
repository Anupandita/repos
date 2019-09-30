using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.App.Contracts.Entities
{
    public class RoleMenuModel
    {
        public List<MenuModel> menus { set; get; }
        public string roleId { set; get; }
    }
}