using System.Threading.Tasks;
using Wms.App.Contracts.Entities;
using Wms.App.Contracts.Result;

namespace Wms.App.Contracts.Interfaces
{
    public interface IReceiptService
    {
        Task<BaseResult<string>> GetShipmentDetailsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetQvDetailsAsync(string token, string shipmentNumber);

    }
}