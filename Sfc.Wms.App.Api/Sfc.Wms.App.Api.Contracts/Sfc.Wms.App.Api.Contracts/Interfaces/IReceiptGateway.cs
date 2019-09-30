﻿using System.Threading.Tasks;
using Wms.App.Contracts.Entities;
using Wms.App.Contracts.Result;

namespace Wms.App.Contracts.Interfaces
{
    public interface IReceiptGateway
    {
        Task<BaseResult<string>> GetShipmentDetailsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetQvDetailsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetAsnDetailsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetDrillAsnDetailsAsync(string token, string whse, string shipmentNumber, string skuId);

        Task<BaseResult<string>> GetAppoitmentScheduleAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> GetASNCommentsAsync(string token, string shipmentNumber);

        Task<BaseResult<string>> UpdateQVDetails(string token, QVDetails qvDetails);

        Task<BaseResult<string>> GetReceiptAsync(string token, string resource);
    }
}