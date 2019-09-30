using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
{
    public interface IRoleGateway
    {
        Task<BaseResult<string>> GetRoleMenuMappings(int roleId, string token);
        Task<BaseResult<string>> GetRolesForUser(string token);
        Task<BaseResult<string>> UpdateRoleMenus(RoleMenuModel roleMenu, string token);
        Task<BaseResult<string>> GetRolePermissions(int roleId, string token);
        Task<BaseResult<string>> UpdateRolePermissions(RolePermissionModel rolePermissions, string token);
    }
}
