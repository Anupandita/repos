using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IUserService
    {
        Task<BaseResult<string>> SignInAsync(LoginCredentials loginCredentials);

        Task<BaseResult<string>> GetAllAsync(string token);

        Task<BaseResult<string>> GetRolesByUsernameAsync(string token, string userName);
    }
}