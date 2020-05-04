﻿using System;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant
{
    public class UIConstants
    {
        public const string LoginUrl = "http://dev.az.app.api.wms.shamrockfoods.com/user/login";
        public const string ItemAttributes = "item-attributes/";
        public const string Lpn = "lpn/";
        public const string Search = "search?";
        public const string LpnComments = "lpn-comments";
        public const string LpnDetails = "lpn-details";
        public const string LpnHistory = "lpn-history";
        public const string Find = "find";
        public static DateTime LogDate;
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
    
        public static string ReceivingSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + "receipts?";

        // http://qa.api.wms.shamrockfoods.com/api/receipts?statusFrom=10&statusTo=50&totalRows=0&pageNo=1&rowsPerPage=50
        public static string ReceivingDrilldownUrl;
        public static string VendorVendorNumberCount;
        public static string ReceivingStatusCount;
        public static string VerifiedDateRangeCount;
        public static string ShipmentNbrCount;

        public static string ReceivingDetailsUrl = ConfigurationManager.AppSettings["BaseUrl"] + "advanced-shipment-notices/";
       // http://qa.api.wms.shamrockfoods.com/api/advanced-shipment-notices/12335721
        public static string ReceivingDetailsDrilldownUrl = "/sku/";
       // http://qa.api.wms.shamrockfoods.com/api/advanced-shipment-notices/12335721/sku/1061561 
        public static string ReceivingQvDetailsUrl = "/quality-verifications";

        public static string ShipmentNbr;
        // http://qa.api.wms.shamrockfoods.com/api/advanced-shipment-notices/12335721/quality-verifications

    }
}
