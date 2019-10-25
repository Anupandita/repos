using System;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IPixTransactionsGateway
    {
       
        Task<BaseResult<string>> GetPixTransactionsDetailsAsync(PixTranactionsModel pixTranactionsModel, string token);

    }
}