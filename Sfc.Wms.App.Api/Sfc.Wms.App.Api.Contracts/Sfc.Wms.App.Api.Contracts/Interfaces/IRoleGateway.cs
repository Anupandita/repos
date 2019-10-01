using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
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
