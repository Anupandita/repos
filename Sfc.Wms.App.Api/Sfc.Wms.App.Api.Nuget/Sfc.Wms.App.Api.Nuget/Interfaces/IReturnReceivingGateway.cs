using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IReturnReceivingGateway
    {
        Task<BaseResult<string>>
            GetReturnReceiving(ReturnReceivingSearchModel returnReceivingSearchModel, string token);

        Task<BaseResult<string>> InsertReturnReceiving(ReturnReceivingInsertModel returnReceivingInsertModel,
            string token);
    }
}