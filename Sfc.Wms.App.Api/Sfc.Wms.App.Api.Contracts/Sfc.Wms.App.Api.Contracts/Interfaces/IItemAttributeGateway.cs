using System;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
{
    public interface IItemAttributeGateway
    {
        Task<BaseResult<string>> GetItemAttributeDetailsAsync(ItemAttributeParamModel itemAttributeParamModel, string token);

        Task<BaseResult<string>> GetDrillDownItemAttributeAsync(String item, string token);

     }
}