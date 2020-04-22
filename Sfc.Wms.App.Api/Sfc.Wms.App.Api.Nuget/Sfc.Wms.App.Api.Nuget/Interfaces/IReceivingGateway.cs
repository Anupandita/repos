using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IReceivingGateway
    {
        Task<BaseResult<SearchResultDto>> SearchAsync(ReceiptInquiryDto receiptInquiryDto, string token);

        Task<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>> GetAsnDetailsAsync(string shipmentNumber, string token);

        Task<BaseResult<IEnumerable<QvDetailsDto>>> GetQualityVerificationsDetailsAsync(string shipmentNumber, string token);

        Task<BaseResult<IEnumerable<AsnLotTrackingDto>>> GetAsnLotTrackingDetailsAsync(string shipmentNumber, string skuId, string token);

        Task<BaseResult> UpdateQualityVerificationsAsync(AnswerTextDto asnAnswerTextDto, string shipmentNumber, string token);
    }
}