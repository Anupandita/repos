using System;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant
{
    public class UIConstants
    {
        public const string LoginUrl = "http://dev.az.app.api.wms.shamrockfoods.com/user/login";
        public const string ItemAttributes = "item-attributes/";
        public const string SearchInputItemId = "attributeSearchInputDto.itemId=";
        public const string SearchInputItemDescription = "attributeSearchInputDto.itemDescription=";
        public const string SearchInputTempZone = "attributeSearchInputDto.tempZone=";
        public const string SearchInputVendorItemNumber = "attributeSearchInputDto.vendorItemNumber=";
        public const string SearchInputTotalRecords = "attributeSearchInputDto.totalRecords=";
        public const string SearchInputLastPageNumber = "attributeSearchInputDto.lastPageNumber=";
        public const string SearchInputPageNumber = "attributeSearchInputDto.pageNumber=";
        public const string SearchInputPageSize = "attributeSearchInputDto.pageSize=";
        public const string SearchInputSortOptions = "attributeSearchInputDto.sortOptions=";
        public const string DateTimeFormat = "mm-dd-yy hh24:mi:ss";
        public const string FormatDateTime = "mm/dd/yyyy hh24:mi:ss";
        public const string EnterDateTimeFormat = "MM/dd/yyyy";
        public const string VolumeDecimalFormat = "99990.0000";
        public const string DecimalFormat = "99990.00";
        public const string HeightFormat = "99990.09";
        public const string CommentCode = "LOT";
        public const string SystemCodeCommentCode = "Lot Data";
        public const string SystemCodeCommentType = "Lot Tracking";
        public static string ItemNumber;
        public static string CartonNbr;
        public static string PoNumber;
        public static string LpnNumber;
        public static string DisplayLocation;
        public static string Aisle;
        public static string Zone;
        public static string Level;
        public static string Slot;
        public const string FromStatus = "0";
        public const string ToStatus = "0";
        public const string Whse = "008";
        public static string QvPoNbr;
        public const string LpnToStatus = "50";
        public const string LpnFromStatus = "45";
        public static string AdjacentLocation;
        public static string LpnNbrForLockUnlock;
        public static string LpnNumberForItems;
        public static string LpnNumberForHistory;
        public static string BearerToken;
        public static string ItemDescription;
        public static string VendorItemNumber;
        public static string TempZone;
        public static string VendorItemNumberCount;
        public static string TempZoneCount;
        public static string ItemAttributeSearchUrl;
        public static string ItemAttributeDetailsUrl;
        public static string MessageLoggerUrl = ConfigurationManager.AppSettings["BaseUrl"] + "message-logs";
        public static string MessageMasterUrl = ConfigurationManager.AppSettings["BaseUrl"] + "message-masters/ui-messages";
        public const string Module = "ARCHLAYER";
        public const string MessageId = "MA-0234";
        public static string Message = "API Testing Message";
        public const string RefCode = "RN";
        public const string RefValue = "Test";
        public const string RecType = "B";
        public const string CodeType = "714";
        public const string Sort = "0";
        public static string CorbaUrl = "http://dev.az.app.api.wms.shamrockfoods.com/corba/";
        public const string Single = "single/";
        public const string Batch = "batch/";
        public const string Default = "default";
        public static string CorbaFunctionName = "printCaseLabelPB";
        public const string WorkStation = "ASRS 2";
        public static string SysCodeUrl = "http://dev.az.app.api.wms.shamrockfoods.com/common/code-ids?";
        public static string SysCodeInputSort = "systemCodeInputDto.sortOption.orderBy=";
        public static string SysCodeInputCodeType = "systemCodeInputDto.codeType=";
        public static string SysCodeInputRecType = "systemCodeInputDto.recType=";
        public const string UserMasterUrl = "http://dev.az.app.api.wms.shamrockfoods.com/user/preferences";
        public const string UnconstrainedValue = "[{\"colId\":\"actions\",\"hide\":false,\"aggFunc\":null,\"width\":150,\"pivotIndex\":null,\"pinned\":\"left\",\"rowGroupIndex\":null},{\"colId\":\"caseNumber\",\"hide\":false,\"aggFunc\":null,\"width\":182,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"displayLocation\",\"hide\":false,\"aggFunc\":null,\"width\":109,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"skuId\",\"hide\":false,\"aggFunc\":null,\"width\":93,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"skuDescription\",\"hide\":false,\"aggFunc\":null,\"width\":255,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"receivedShipmentNumber\",\"hide\":false,\"aggFunc\":null,\"width\":100,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"distributionCenterOrderNumber\",\"hide\":false,\"aggFunc\":null,\"width\":139,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"lockCount\",\"hide\":false,\"aggFunc\":null,\"width\":109,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"lockCount_1\",\"hide\":false,\"aggFunc\":null,\"width\":106,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"consumeCasePriority\",\"hide\":false,\"aggFunc\":null,\"width\":140,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"consumePriorityDate\",\"hide\":false,\"aggFunc\":null,\"width\":144,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"consumeSequence\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"manufacturingOn\",\"hide\":false,\"aggFunc\":null,\"width\":150,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"receivedOn\",\"hide\":false,\"aggFunc\":null,\"width\":150,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"expiryDate\",\"hide\":false,\"aggFunc\":null,\"width\":150,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"processImmediateNeeds\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"volume\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"estimateWeight\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"actualWeight\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"poNumber\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"codeDescription\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"vendorName\",\"hide\":false,\"aggFunc\":null,\"width\":120,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"physicalEntityCode\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"plantId\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"singleSkuCase\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"specialInstructionCode1\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"specialInstructionCode2\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"specialInstructionCode3\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"specialInstructionCode4\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"specialInstructionCode5\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"shippedAsnQuantity\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"originalQuantity\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"actualQuantity\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"totalAllocatedQuantity\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"varianceQuantity\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"cutNumber\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"createdOn\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"updatedOn\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"userName\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"comment\",\"hide\":false,\"aggFunc\":null,\"width\":90,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null},{\"colId\":\"vendorItemNumber\",\"hide\":false,\"aggFunc\":null,\"width\":200,\"pivotIndex\":null,\"pinned\":null,\"rowGroupIndex\":null}]";
        public static int PreferenceId;
        public static string VendorNameCount;
        public static string ReceivingStatusCount;
        public static string VerifiedDateRangeCount;
        public static string ShipmentNbrCount;
        public static string ShipmentNbr;
        public static string VendorName;
        public static string VerifiedFrom;
        public static string VerifiedTo;
        public static string QvShipmentNbr;
        public static string UnAnsQvShipmentNbr;
        public static string QuesAnswerId;
        public const string LpnLockUnlock = "lpn-lock-unlock/";
        public const string Find = "find?";
        public const string slash = "/";
        public static string PalletId;
        public static string PalletIdCount;
        public static string CreatedDateCount;
        public static string CreatedDate;
        public static string StatusCount;
        public static DateTime LogDate;

        public const string Lpn = "http://dev.az.app.api.wms.shamrockfoods.com/lpn/";
        public const string Search = "search?";
        public const string LpnComments = "lpn-comments";
        public const string LpnDetails = "lpn-details";        
        public const string LpnHistory = "lpn-history/";
        public const string LpnCaseUnlock = "case-unlock?";
        public const string LpnCaseDetails = "lpn-case-details";
        public const string LpnIds = "lpnIds=";
        public const string LpnMultiUnlock = "multiple-unlock";
        public const string LpnMultiLock = "multiple-lock";
        public const string LpnMultiComments = "multiple-lpn-comments";
        public const string LpnMultiEdit = "multiple-lpn-details";
        public static string LpnSearchUrl;
        public static string LpnCommentsUrl;
        public static string LpnHistoryUrl;
        public static string LpnLockUnlockUrl;
        public static string LpnCaseUnlockUrl;
        public static string LpnDetailsUrl;
        public static string LpnAddCommentsUrl;
        public static string LpnEditCommentsUrl;
        public static string LpnDeleteCommentsUrl;
        public static string LpnUpdateUrl;
        public static string LpnItemsUrl;
        public static string LpnMultiUnlockUrl;
        public static string LpnMultiLockUrl;
        public static string LpnMultiCommentsUrl;
        public static string LpnMultiEditUrl;
        public static string ItemNumberCount;
        public const string QuantityFormat = "99990.0";
        public const string SearchInputSkuId = "lpnParamDto.skuId=";
        public const string SearchInputPalletId = "lpnParamDto.palletId=";
        public const string SearchInputZone = "lpnParamDto.zone=";
        public const string SearchInputAisle = "lpnParamDto.aisle=";
        public const string SearchInputSlot = "lpnParamDto.slot=";
        public const string SearchInputLevel = "lpnParamDto.level=";
        public const string SearchInputCreatedDate = "lpnParamDto.createdDate=";
        public const string SearchInputFromDate = "lpnParamDto.statusFrom=";
        public const string SearchInputToDate = "&lpnParamDto.statusTo=";
        public const string SearchInputLpnNumber = "lpnParamDto.lpnNumber=";
        public static string ReceivingSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + "receipts?";

        // http://qa.api.wms.shamrockfoods.com/api/receipts?statusFrom=10&statusTo=50&totalRows=0&pageNo=1&rowsPerPage=50
   
        public static string ReceivingDetailsUrl = ConfigurationManager.AppSettings["BaseUrl"] + "advanced-shipment-notices/";
       // http://qa.api.wms.shamrockfoods.com/api/advanced-shipment-notices/12335721
        public static string ReceivingDetailsDrilldownUrl = "/sku/";
        // http://qa.api.wms.shamrockfoods.com/api/advanced-shipment-notices/12335721/sku/1061561 
        public static string ReceivingQvDetailsUrl = "/quality-verifications";
        public static string LockCount;

        public static DateTime? ManufacturingDate;
        public static DateTime? ExpireDate;
        public static string ConsumePriority;

        public static DateTime? ConsumePriorityDate { get; internal set; }
        public static string SpclInstCode2 { get; internal set; }
        public static string SpclInstCode4 { get; internal set; }
        public static string SpclInstCode3 { get; internal set; }
        public static string SpclInstCode5 { get; internal set; }
        public static decimal Volume { get; internal set; }
        public static string DcOrderNbr { get; internal set; }
        public const string VendorId = "0011133";
        public static string SpclInstCode1 { get; internal set; }
        public static decimal ActlWt { get; internal set; }
        public static decimal EstWt { get; internal set; }
        public static string ConsumeSequence { get; internal set; }
        public static string AssortNumber { get; internal set; }
        public static string CutNumber { get; internal set; }
        public static string LpnNbrForLockUnlock1 { get; internal set; }
        public static string LockCount1 { get; internal set; }
        public static int CommentSequenceNumber { get; internal set; }
        public static string SkuId { get; internal set; }
        // http://qa.api.wms.shamrockfoods.com/api/advanced-shipment-notices/12335721/quality-verifications

    }
    public class FuncName
    {            
        public const string PrintShipment= "printShipmentPB";
        public const string VerifyShipment = "verifyShipmentPB";
        public const string MultiUnLock = "unlockPB";
        public const string MultiLock = "lockPB";
        public const string Items = "updateCaseDtl";

    }
}
