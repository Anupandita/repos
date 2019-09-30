using System.Threading.Tasks;
using Wms.App.Contracts.Entities;
using Wms.App.Contracts.Result;

namespace Wms.App.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<BaseResult<string>> SignInAsync(LoginCredentials loginCredentials);

        Task<BaseResult<string>> GetAllAsync(string token);

        Task<BaseResult<string>> GetRolesByUsernameAsync(string token, string userName);
    }
}