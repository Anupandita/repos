using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
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
