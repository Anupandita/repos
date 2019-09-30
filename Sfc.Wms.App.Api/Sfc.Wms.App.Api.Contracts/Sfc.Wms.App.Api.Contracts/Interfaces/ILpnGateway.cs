using System;
using System.Threading.Tasks;
using Wms.App.Contracts.Result;
using Wms.App.Contracts.Entities;

namespace Wms.App.Contracts.Interfaces
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


     }
}