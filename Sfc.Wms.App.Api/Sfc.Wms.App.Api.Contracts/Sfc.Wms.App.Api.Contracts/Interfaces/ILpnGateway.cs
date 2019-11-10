using System;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface ILpnGateway
    {
        Task<BaseResult<string>> GetLpnDetailsAsync(LpnParamModel lpnParamModel, string token);

        Task<BaseResult<string>> GetLpnDetailsByLpnIdAsync(String lpnId, string token);

        Task<BaseResult<string>> GetLpnCommentsByLpnIdAsync(String lpnId, string token);

        Task<BaseResult<string>> GetLpnHistoryAsync(String lpnId, string whse, string token);

        Task<BaseResult<string>> GetLpnLockUnlocByLpnIdkAsync(String lpnId, string token);

        Task<BaseResult<string>> InsertLpnAisleTransAsync(LpnAisleTransModel lpnAisleTransModel, string token);

        Task<BaseResult<string>> UpdateLpnDetailsAsync(LpnDetailsUpdateModel lpnDetailsUpdateModel, string token);

        Task<BaseResult<string>> InsertLpnCommentsAsync(LpnCommentsModel lpnCommentsModel, string token);

        Task<BaseResult<string>> DeleteLpnCommentsAsync(LpnCommentsModel lpnCommentsModel, string token);

        Task<BaseResult<string>> GetLpnVendorsAsync(string token);

        Task<BaseResult<string>> UpdateCaseLpnDetailsAsync(LpnCaseDetailsUpdateModel lpnDetailsUpdateModel, string token);


    }
}