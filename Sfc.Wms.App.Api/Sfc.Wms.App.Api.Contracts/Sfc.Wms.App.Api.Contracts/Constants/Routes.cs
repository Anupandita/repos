// ReSharper disable InconsistentNaming



namespace Sfc.Wms.App.Api.Contracts.Constants
{
    public static class Routes
    {
        public static class Params
        {
            public const string ShipmentNumber = "shipment_nbr";
            public const string SkuId = "skuId";
            public const string WHSE = "whse";

            public const string ByShipmentNumber = "{shipmentNumber}";
            public const string BySkuId = "{skuId}";
        }

        public static class Paths
        {
            public const string AllUsers = "all-users";
            public const string ApprointmentSchedule = "appointment-schedule";
            public const string AsnComments = "asn-comments";
            public const string AsnDetails = "asn-details";
            public const string Batch = "batch/" + CorbaParams;
            public const string CodeIds = "code-ids";
            public const string Comments = "comments";
            public const string CorbaParams = "{functionName}/{isVector}";
            public const string Details = "details";
            public const string DrillASNDetails = "drill-asn-details";
            public const string DrillDownItemAttribute = "{itemId}";
            public const string Find = "find";

            public const string FindByShipmentDetails =
                "find?shipmtNumber={shipmtNumber}&statusFrom={statusFrom}&statusTo={statusTo}&poNumber={poNumber}&vendorName={vendorName}&shippedDate={shippedDate}&expectedDate={expectedDate}&scheduledDate={scheduledDate}";

            public const string FromSortation = "from-sortation";
            public const string GetStatusCodes = "GetStatusCodes";
            public const string GridGroupTypes = "grid_grp_types=";
            public const string GridLocationId = "grid_locn_id=";
            public const string GridSequenceNumber = "grid_seq_nbr=";
            public const string HospitalLog = "hospital-log";
            public const string LpnAisleTrans = "aisle-trans/{lpnId}/{faceLocationId}";
            public const string LpnCaseDetails = "lpn-case-details";
            public const string LpnComments = "lpn-comments/{lpnId}";
            public const string LpnCommentsAdd = "lpn-comments";
            public const string LpnDeleteComments = "lpn-comments/{caseNumber}/{commentSequenceNumber}";
            public const string LpnDetails = "lpn-details/{lpnId}";
            public const string LpnHistory = "lpn-history/{lpnNumber}/{warehouse}";
            public const string LpnLockUnlock = "lpn-lock-unlock/{lpnId}";
            public const string LpnMultiplelock = "multiple-lock";
            public const string LpnCaseUnLockDetails = "{lpnIds}/unlocks";
            public const string LpnUpdateDetails = "lpn-details";
            public const string MultipleLpnUpdate = "multiple-lpn-details";
            public const string MultipleLpnCommentsAddition = "multiple-lpn-comments";
            public const string LpnVendors = "vendors";
            public const string Menu = "menu";
            public const string MenusById = "menus/{menuId}";
            public const string OrmtByCartonNumber = "carton-number";
            public const string OrmtByWaveNumber = "wave-number";
            public const string OrmtByWaveNumberUsingParallel = "wave-number-parallel";
            public const string PageNo = "pageNo=";
            public const string QueryParamAnd = "&";
            public const string QueryParamSeperator = "/";
            public const string QueryParamSymbol = "?";
            public const string QvDetails = "qv-details";
            public const string QvDetailsByShpmtNumber = "QvDetails";
            public const string Role = "role";
            public const string RoleMenus = "role-menus";
            public const string RolePermissions = "role-permissions";
            public const string RolesById = "roles/{roleId}";
            public const string RolesByUsername = "roles/{userName}";
            public const string RowsPerPage = "rowsPerPage=";
            public const string Search = "search";
            public const string SearchItemAttribute = "search";
            public const string SearchPixTransactions = "search";
            public const string SearchVelocityMaster = "verlocity-master-search";
            public const string SerialNumbers = "serial-numbers";
            public const string ShipmentDetails = "shipment-details";
            public const string ShpmtDetailsByShpmtNumber = "shipmentDetails/{shipmentNumber}";
            public const string Single = "single/" + CorbaParams;
            public const string SkmtWrapper = "wrapper";
            public const string StatusCodes = "get_status_codes";
            public const string ToSortation = "to-sortation";
            public const string TotalRows = "totalRows=";
            public const string UserInputAisle = "user_inpt_aisle=";
            public const string UserInputGroupTypes = "user_inpt_grp_type=";
            public const string UserInputLocationGroup = "user_inpt_locn_grp=";
            public const string UserInputLVL = "user_inpt_lvl=";
            public const string UserInputPutwayZone = "user_inpt_putwy_zone=";
            public const string UserInputSKU = "user_inpt_sku=";
            public const string UserInputSlot = "user_inpt_slot=";
            public const string UserInputZone = "user_inpt_zone=";
            public const string UserLocationClass = "user_inpt_locn_cls=";
            public const string UserLogin = "login";
            public const string Username = "username";
            public const string UserRoles = "user-roles";
            public const string Preferences = "preferences";
            public const string GetUserPrefereces = "{userId}/" + Preferences;
            public const string LpnMultipleUnlock = "multiple-unlock";
            public const string CaseUnlock = "case-unlock";
            public const string RefreshToken = "refresh-token";
            public const string UiSpecificMessages = "ui-messages";


            public const string Skus = "skus";
            public const string AdvanceShipmentNotices = "advanced-shipment-notices";
            public const string QualityVerifications = "quality-verifications";
            public const string Receipt = "receipts";
            public const string GetAsnDetails = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber;
            public const string UpdateAsnDetails = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber;
            public const string GetAsnLotTrackingDetails = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber
                                                           + QueryParamSeperator + Skus + QueryParamSeperator
                                                           + Params.BySkuId;
            public const string GetQualityVerifications = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber
                                                          + QueryParamSeperator + QualityVerifications;

            public const string UpdateQualityVerifications = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber
                                                             + QueryParamSeperator + QualityVerifications;
            }

        public static class Prefixes
        {
            public const string ActiveItem = "active-item";
            public const string ActiveLocations = "active-locations";
            public const string ActiveLocationsDrilldown = "active-locations-drilldown";
            public const string AddToLocnGroup = "add-to-locn-grp";
            public const string Carton = "carton";
            public const string ChangePassword = "change-password";
            public const string CheckSession = "checkSession";
            public const string Common = "common";
            public const string ConnectionInformation = "api/release-connection-information";
            public const string Corba = "corba";
            public const string DematicMessageComt = "api/container-maintenance";
            public const string DematicMessageIvmt = "api/inventory-maintenance";
            public const string DematicMessageOrmt = "api/order-maintenance";
            public const string DematicMessageSkmt = "api/sku-maintenance";
            public const string DematicMessageSynr = "api/synchronization-request";
            public const string EmsToWmsMessage = "api/emstowms-message";
            public const string ItemAttribute = "api/item-attributes";
            public const string LocationGroup = "location-grp";
            public const string LocationGroupTypesAll = "location-group-types-all";
            public const string LocationLPN = "locations-lpn";
            public const string Locations = "location";
            public const string LocationsAdjInv = "location-adjinv";
            public const string LocationsGroupById = "location-group-by-id";
            public const string LocationsLockUnlock = "location-lock-unlock";
            public const string Logout = "logout";
            public const string Lpn = "lpn";
            public const string Menu = "menu";
            public const string Menus = "menus";
            public const string PixTransactions = "pix-trans";
            public const string Receipt = "receipts";
            public const string ReserveLocationDrillDown = "reserve-drilldown";
            public const string ReserveLocations = "reserve-locations";
            public const string ReturnsReceiving = "returns-receiving/";
            public const string Roles = "roles";
            public const string User = "user";
            public const string UserPreferences = "userPreferences";
            public const string UserRolePermissions = "all-permissions";
            public const string UserRoles = "user-roles";
            public const string VelocityMaster = "velocity";
            public const string WorkAreaMaster = "work-area-master";
            public const string MessageLogger = "api/message-logs";
            public const string MessageMaster = "api/message-masters";

            public const string Api = "api";
        }
    }
}