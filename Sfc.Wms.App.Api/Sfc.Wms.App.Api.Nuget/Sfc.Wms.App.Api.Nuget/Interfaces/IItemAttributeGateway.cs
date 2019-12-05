using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IItemAttributeGateway
    {
        Task<BaseResult<string>> GetItemAttributeDetailsAsync(ItemAttributeParamModel itemAttributeParamModel,
            string token);

        Task<BaseResult<string>> GetDrillDownItemAttributeAsync(string item, string token);
    }
}