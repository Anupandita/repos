using System;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
{
    public interface ICartonInquiryGateway
    {
        Task<BaseResult<string>> SearchCartonInquiry(CartonInquiryModel cartonInquiryModel, string token);
        Task<BaseResult<string>> CartonComments(string cartonNumber, string token);
        Task<BaseResult<string>> CartonDetails(string cartonNumber, string token);
        Task<BaseResult<string>> CartonSerialNumbers(string cartonNumber, string token);
        Task<BaseResult<string>> CartonToSortation(string cartonNumber, string token);
        Task<BaseResult<string>> CartonFromSortation(string cartonNumber, string token);
        Task<BaseResult<string>> CartonHospitalLog(string cartonNumber, string token);
    }
}
