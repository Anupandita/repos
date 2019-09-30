using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IMenuGateway
    {
        Task<BaseResult<string>> CreateMenu(MenuModel menuMainModel, string token);

        Task<BaseResult<string>> UpdateMenu(MenuModel menuMainModel, string token);

        Task<BaseResult<string>> Menu(string token);

        Task<BaseResult<string>> GetById(string menuId, string token);

        Task<BaseResult<string>> DeleteById(string menuId, string token);
    }
}
