using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IReceiptService
    {
        Task<BaseResult<string>> GetShipmentDetailsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetQvDetailsAsync(string token, string shipmentNumber);
    }
}