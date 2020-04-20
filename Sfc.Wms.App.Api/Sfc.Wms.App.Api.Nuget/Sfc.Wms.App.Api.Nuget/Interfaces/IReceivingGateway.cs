using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IReceivingGateway
    {
        Task<BaseResult<SearchResultDto>> SearchAsync(ReceiptInquiryDto receiptInquiryDto, string token);

        Task<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>> GetAsnDetailsAsync(string shipmentNumber, string token);

        Task<BaseResult<IEnumerable<QvDetailsDto>>> GetQvDetailsAsync(string shipmentNumber, string token);

        Task<BaseResult<IEnumerable<AsnLotTrackingDto>>> GetAsnLotTrackingDetailsAsync(string shipmentNumber, string skuId, string token);

        Task<BaseResult> UpdateAnswerTextAsync(AnswerTextDto asnAnswerTextDto, string token);

        Task<BaseResult<ShipmentDetailsDto>> GetShipmentDetailsAsync(string shipmentNumber, string token);

    }
}