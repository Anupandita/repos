using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IWmsAppService<in TEntity> where TEntity : class

    {
        Task<BaseResult<string>> CreateAsync(string token, TEntity createRole);

        Task<BaseResult<string>> UpdateAsync(string token, TEntity updateRole);

        Task<BaseResult<string>> GetAsync(string token, int id);

        Task<BaseResult<string>> DeleteAsync(string token, int id);

        Task<BaseResult<string>> GetAllAsync(string token);
    }
}