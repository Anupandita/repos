using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.App.Contracts.Entities
{
    public class RolePermissionModel
    {
        public List <PermissionModel> permissions { set; get; }
        public string roleId { set; get; }
    }
}