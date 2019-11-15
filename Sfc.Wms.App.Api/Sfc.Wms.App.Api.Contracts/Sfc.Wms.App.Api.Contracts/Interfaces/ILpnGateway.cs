using System;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface ILpnGateway
    {
        Task<BaseResult<T>> GetLpnDetailsAsync<T>(LpnParamModel lpnParamModel, string token);

        Task<BaseResult<T>> GetLpnDetailsByLpnIdAsync<T>(String lpnId, string token);

        Task<BaseResult<T>> GetLpnCommentsByLpnIdAsync<T>(String lpnId, string token);

        Task<BaseResult<T>> GetLpnHistoryAsync<T>(String lpnId, string whse, string token);

        Task<BaseResult<T>> GetLpnLockUnlockByLpnIdkAsync<T>(String lpnId, string token);

        Task<BaseResult<T>> InsertLpnAisleTransAsync<T>(LpnAisleTransModel lpnAisleTransModel, string token);

        Task<BaseResult<T>> UpdateLpnDetailsAsync<T>(LpnDetailsUpdateModel lpnDetailsUpdateModel, string token);

        Task<BaseResult<T>> InsertLpnCommentsAsync<T>(LpnCommentsModel lpnCommentsModel, string token);

        Task<BaseResult<T>> DeleteLpnCommentsAsync<T>(LpnCommentsModel lpnCommentsModel, string token);

        Task<BaseResult<T>> GetLpnVendorsAsync<T>(string token);

        Task<BaseResult<T>> UpdateCaseLpnDetailsAsync<T>(LpnCaseDetailsUpdateModel lpnDetailsUpdateModel, string token);
    }
}