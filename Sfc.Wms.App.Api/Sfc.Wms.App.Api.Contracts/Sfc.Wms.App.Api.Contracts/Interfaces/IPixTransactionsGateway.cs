using System;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
{
    public interface IPixTransactionsGateway
    {
       
        Task<BaseResult<string>> GetPixTransactionsDetailsAsync(PixTranactionsModel pixTranactionsModel, string token);

    }
}