using System.Collections.Generic;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class RolePermissionModel
    {
        public List<PermissionModel> permissions { set; get; }
        public string roleId { set; get; }
    }
}