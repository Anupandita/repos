using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
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