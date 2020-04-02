using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface ILpnGateway
    {
        Task<BaseResult<LpnSearchResultsDto>> LpnSearchAsync(LpnParameterDto lpnParamModel, string token);

        Task<BaseResult<LpnDetailsDto>> GetLpnDetailsByLpnIdAsync(string lpnId, string token);

        Task<BaseResult<List<CaseCommentDto>>> GetLpnCommentsByLpnIdAsync(string lpnId, string token);

        Task<BaseResult<List<LpnHistoryDto>>> GetLpnHistoryByLpnIdAndWhseAsync(string lpnId, string whse, string token);

        Task<BaseResult<List<CaseLockUnlockDto>>> GetLpnLockUnlockByLpnIdAsync(string lpnId, string token);

        //TODO:  Needs to be validated at the time of aisle implementation
        Task<BaseResult<AisleTransactionDto>> InsertLpnAisleTransAsync(LpnAisleTransModel lpnAisleTransModel, string token);

        Task<BaseResult> UpdateLpnHeaderAsync(LpnHeaderUpdateDto lpnDetailsUpdateModel, string token);

        Task<BaseResult<CaseCommentDto>> InsertLpnCommentsAsync(CaseCommentDto lpnCommentsModel, string token);

        Task<BaseResult> DeleteLpnCommentsAsync(string caseNumber, int commentSequenceNumber, string token);

        Task<BaseResult> UpdateLpnCommentAsync(CaseCommentDto caseCommentDto, string token);

        Task<BaseResult> UpdateLpnDetailsAsync(LpnDetailsUpdateDto lpnDetailsUpdateModel, string token);

        Task<BaseResult<List<CaseLockDto>>> GetCaseUnLockDetailsAsync(IEnumerable<string> lpnIds, string token);

        Task<BaseResult<LpnMultipleUnlockResultDto>> LpnMultipleUnlockAsync(List<LpnMultipleUnlockDto> lpnMultipleUnlockDto, string token);

        Task<BaseResult<LpnMultipleUnlockResultDto>> CaseLockCommentWithBatchCorbaAsync(CaseLockCommentDto caseLockComment, string token);

        Task<BaseResult> MultipleLpnsCommentsAddAsync(IEnumerable<CaseCommentDto> caseCommentDtos, string token);

        Task<BaseResult> MultipleLpnsUpdateAsync(LpnBatchUpdateDto lpnBatchUpdateDto, string token);
    }
}