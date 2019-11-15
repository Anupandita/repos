using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IUserGateway
    {
        Task<BaseResult<string>> SignInAsync(LoginCredentials loginCredentials);

        Task<BaseResult<string>> GetAllAsync(string token);

        Task<BaseResult<string>> GetRolesByUsernameAsync(string token, string userName);
        Task<BaseResult<string>> CheckSession(string token);
        Task<BaseResult<string>> GetUserPermissions(string token);
        Task<BaseResult<string>> GetUserMenus(string token);
        Task<BaseResult<string>> Logout();
        Task<BaseResult<string>> ChangePassword(ChangePasswordModel changePasswordModel, string token);
        Task<BaseResult<string>> UpdateUserRoles(UserRoleModel userRoleModel, string token);
        Task<BaseResult<string>> UserPreferences(UserPreferencesModel userPreferencesModel, string token);
    }
}