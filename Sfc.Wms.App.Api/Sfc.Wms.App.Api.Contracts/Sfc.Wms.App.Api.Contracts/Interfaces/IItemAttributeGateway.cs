using System;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IItemAttributeGateway
    {
        Task<BaseResult<string>> GetItemAttributeDetailsAsync(ItemAttributeParamModel itemAttributeParamModel, string token);

        Task<BaseResult<string>> GetDrillDownItemAttributeAsync(String item, string token);

     }
}