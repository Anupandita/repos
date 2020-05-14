// ReSharper disable InconsistentNaming

namespace Sfc.Wms.App.Api.Contracts.Constants
{
    public static class Routes
    {
        public static class Params
        {
            public const string ByShipmentNumber = "{shipmentNumber}";
            public const string BySkuId = "{skuId}";
        }

        public static class Paths
        {
            public const string UserPreferences = "userPreferences";
            public const string Batch = "batch/" + CorbaParams;
            public const string CodeIds = "code-ids";
            public const string CorbaParams = "{functionName}/{isVector}";
            public const string DrillDownItemAttribute = "{itemId}";
            public const string Find = "find";
            public const string LpnAisleTrans = "aisle-trans/{lpnId}/{faceLocationId}";
            public const string LpnCaseDetails = "lpn-case-details";
            public const string GetLpnComments = "lpn-comments/{lpnId}";
            public const string LpnComments = "lpn-comments";
            public const string LpnDeleteComments = "lpn-comments/{caseNumber}/{commentSequenceNumber}";
            public const string LpnDetails = "lpn-details/{lpnId}";
            public const string LpnHistory = "lpn-history/{lpnNumber}/{warehouse}";
            public const string LpnLockUnlock = "lpn-lock-unlock/{lpnId}";
            public const string LpnMultiplelock = "multiple-lock";
            public const string LpnUpdateDetails = "lpn-details";
            public const string MultipleLpnUpdate = "multiple-lpn-details";
            public const string MultipleLpnCommentsAddition = "multiple-lpn-comments";
            public const string OrmtByCartonNumber = "carton-number";
            public const string OrmtByWaveNumber = "wave-number";
            public const string QueryParamAnd = "&";
            public const string QueryParamSeperator = "/";
            public const string Search = "search";
            public const string Single = "single/" + CorbaParams;
            public const string SkmtWrapper = "wrapper";
            public const string UserLogin = "login";
            public const string Preferences = "preferences";
            public const string LpnMultipleUnlock = "multiple-unlock";
            public const string CaseUnlock = "case-unlock";
            public const string RefreshToken = "refresh-token";
            public const string UiSpecificMessages = "ui-messages";
            public const string Skus = "skus";
            public const string AdvanceShipmentNotices = "advanced-shipment-notices";
            public const string QualityVerifications = "quality-verifications";
            public const string GetAsnDetails = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber;
            public const string UpdateAsnDetails = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber;
            public const string GetAsnLotTrackingDetails = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber
                                                           + QueryParamSeperator + Skus + QueryParamSeperator
                                                           + Params.BySkuId;
            public const string GetQualityVerifications = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber
                                                          + QueryParamSeperator + QualityVerifications;

            public const string UpdateQualityVerifications = AdvanceShipmentNotices + QueryParamSeperator + Params.ByShipmentNumber
                                                             + QueryParamSeperator + QualityVerifications;
            public const string ImageUrlsParams = "{sku}/{gtin?}";
        }

        public static class Prefixes
        {
            public const string ApiCorba = "api/corba";
            public const string ApiLpn = "api/lpn";
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
            public const string Lpn = "lpn";
            public const string User = "user";
            public const string MessageLogger = "api/message-logs";
            public const string MessageMaster = "api/message-masters";
            public const string Api = "api";
            public const string ImageUrls = "api/image-urls";
        }
    }
}