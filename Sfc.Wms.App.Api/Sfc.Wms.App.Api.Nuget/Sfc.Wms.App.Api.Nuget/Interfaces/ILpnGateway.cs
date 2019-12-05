using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface ILpnGateway
    {
        Task<BaseResult<T>> GetLpnDetailsAsync<T>(LpnParameterDto lpnParamModel, string token);

        //Task<BaseResult<T>> GetLpnDetailsByLpnIdAsync<T>(string lpnId, string token);

        //Task<BaseResult<T>> GetLpnCommentsByLpnIdAsync<T>(string lpnId, string token);

        //Task<BaseResult<T>> GetLpnHistoryAsync<T>(string lpnId, string whse, string token);

        //    Task<BaseResult<T>> GetLpnLockUnlockByLpnIdkAsync<T>(string lpnId, string token);

        // //   Task<BaseResult<T>> InsertLpnAisleTransAsync<T>(LpnAisleTransModel lpnAisleTransModel, string token);

        //    Task<BaseResult<T>> UpdateLpnDetailsAsync<T>(LpnHeaderUpdateDto lpnDetailsUpdateModel, string token);

        //    Task<BaseResult<T>> InsertLpnCommentsAsync<T>(CaseCommentDto lpnCommentsModel, string token);

        //    Task<BaseResult<T>> DeleteLpnCommentsAsync<T>(CaseCommentDto lpnCommentsModel, string token);

        //    Task<BaseResult<T>> GetLpnVendorsAsync<T>(string token);

        //    Task<BaseResult<T>> UpdateCaseLpnDetailsAsync<T>(LpnDetailsUpdateDto lpnDetailsUpdateModel, string token);
    }
}