using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IItemAttributeGateway
    {
        Task<BaseResult<ItemAttributeSearchResultDto>> SearchAsync(ItemAttributeSearchInputDto itemAttributeSearchInputDto,
            string token);

        Task<BaseResult<ItemAttributeDetailsDto>> AttributeDrillDownAsync(string item, string token);
    }
}