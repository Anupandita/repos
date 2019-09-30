using System;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
{
    public interface IReturnReceivingGateway
    {
        Task<BaseResult<string>> GetReturnReceiving(ReturnReceivingSearchModel returnReceivingSearchModel, string token);
        Task<BaseResult<string>> InsertReturnReceiving(ReturnReceivingInsertModel returnReceivingInsertModel, string token);
    }
}
