using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IPixTransactionsGateway
    {
        Task<BaseResult<string>> GetPixTransactionsDetailsAsync(PixTranactionsModel pixTranactionsModel, string token);
    }
}