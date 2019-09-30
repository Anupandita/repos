using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IReceiptService
    {
        Task<BaseResult<string>> GetShipmentDetailsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetQvDetailsAsync(string token, string shipmentNumber);

    }
}