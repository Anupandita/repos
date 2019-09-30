using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface ILocationsGateway
    {
        Task<BaseResult<string>> GetLocationGroupsByGroupType(string locationGroupId, string token);
        Task<BaseResult<string>> GetActiveLocationSearch(ActiveLocationModel activeLocationModel, string token);
        Task<BaseResult<string>> GetReserveLocationSearch(ReserveLocationModel reserveLocationModel, string token);

        Task<BaseResult<string>> GetWorkAreaMaster(string token);
        Task<BaseResult<string>> UpdateActiveLocationsDrilldown(ActiveLocationsDrillDownModel activeLocationsDrillDownModel, string token);
        Task<BaseResult<string>> GetActionItem(ActiveItemModel activeItemModel, string token);
        Task<BaseResult<string>> UpdateActionItem(ActiveItemUpdateModel activeItemUpdateModel, string token);

        Task<BaseResult<string>> GetLocationGroupById(string grid_locn_id, string token);
        Task<BaseResult<string>> GetLocationGroupTypesAll(string token);
        Task<BaseResult<string>> UpdateLocationGroupById(ActiveLocationGroupModel activeLocationGroupModel, string token);

        Task<BaseResult<string>> GetLocationLPNS(string grid_locn_id, string token);
        Task<BaseResult<string>> UpdateLocationLPNS(ActiveLocationLpnModel activeLocationLpnModel, string token);

        Task<BaseResult<string>> UpdateLockUnlock(LockUnlockModel lockUnlockModel, string token);
        Task<BaseResult<string>> UpdateAdjInv(AdjInvModel adjInvModel, string token);

        Task<BaseResult<string>> GetReserveLocnDrillDown(string token);
        Task<BaseResult<string>> UpdateReserveLocationnDrillDown(ReserveLocationDrillDownModel reserveLocationDrillDownModel, string token);

        Task<BaseResult<string>> DeleteFromLocationGroup(LocationDeleteModel locationDeleteModel, string token);
        Task<BaseResult<string>> AddToLocationGroup(AddActiveLocationGroupModel addActiveLocationGroupModel, string token);

    }
}
