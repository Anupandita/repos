using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IWmsAppGateway<TEntity> where TEntity : class
    {
        Task<BaseResult<string>> UpdateAsync(string token, TEntity tEntity);

        Task<BaseResult<string>> CreateAsync(string token, TEntity tEntity);

        Task<BaseResult<string>> GetAsync(string token, int id);

        Task<BaseResult<string>> DeleteAsync(string token, int id);

        Task<BaseResult<string>> GetAllAsync(string token);
    }
}