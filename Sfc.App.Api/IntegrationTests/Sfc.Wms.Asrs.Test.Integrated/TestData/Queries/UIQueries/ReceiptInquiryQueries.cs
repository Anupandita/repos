using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace FunctionalTestProject.SQLQueries
{
    public static class ReceiptInquiryQueries
    {
        public const string FetchPoNbrSql = "select distinct ad.po_nbr from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join asn_lot_trkg_tbl al on ad.shpmt_nbr=al.shpmt_nbr ORDER BY dbms_random.value";
        public const string FetchAsnNbrSql = "Select * from(select distinct ah.SHPMT_NBR,count(*) from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join asn_lot_trkg_tbl al on ad.shpmt_nbr=al.shpmt_nbr " +
            "group by ah.shpmt_nbr ORDER BY dbms_random.value) where rownum=1";
        public const string FetchVendorName = "Select * from(select distinct vendor_name,count(*) from asn_dtl ad inner join vendor_master vm on vm.vendor_id=ad.vendor_id group by vendor_name  ORDER BY dbms_random.value) where rownum=1";
        public static string FetchVerfDateSql = "Select * from(select to_char(verf_date_time-10,'MM/dd/yyyy'),to_char(verf_date_time+10,'MM/dd/yyyy') from asn_hdr where verf_date_time is not null ORDER BY dbms_random.value) where rownum=1";

        public const string FetchUnAnsQvShipmentNbrSql = "select distinct ad.shpmt_nbr,ques_ans_id from ques_master qm inner join ques_ans qa on qm.ques_id=qa.ques_id inner join asn_dtl ad " +
                    "on ad.shpmt_nbr=qa.shpmt_nbr inner join asn_hdr ah on ah.shpmt_nbr=ad.shpmt_nbr where ans_mandt_flag='Y' and ans_text is null ORDER BY dbms_random.value";
        public const string FetchQvShipmentNbrSql = "select distinct shpmt_nbr from ques_master qm inner join ques_ans qa on qm.ques_id=qa.ques_id inner join asn_dtl ad" +
                    " on ad.shpmt_nbr=qa.shpmt_nbr  where ans_text is null ORDER BY dbms_random.value";

        public static string GetReceivingSearchByStatusCount()
        {
            return $@"SELECT count(*) from asn_hdr where stat_code >={UIConstants.FromStatus} and stat_code <= {UIConstants.ToStatus}";
        }

        public static string GetReceivingSearchByVerifiedDateCount()
        {
            return $@"SELECT count(*) from asn_hdr where verf_date_time >={UIConstants.VerifiedFrom} and verf_date_time <= {UIConstants.VerifiedTo}";
        }
        public static string FetchQvGridDataSql()
        {
            if (UIConstants.QvShipmentNbr != null)
                return $@"SELECT qa.QUES_ANS_ID QuestionAnswerId,
                           qa.ANS_TEXT AnswerText,DECODE(qa.PROMPT_LVL,'I','Item','C','Case','S','Shipment',qa.PROMPT_LVL) PromptLevel,
                           qa.SHPMT_NBR ShipmentNumber,qa.CASE_NBR CaseNumber,qa.SKU_ID SkuId,
                           qa.SEQ_NBR SequenceNumber,DECODE(qa.STAT_CODE,'0','Pending','90','Completed','91','Skipped',qa.STAT_CODE) StatusCode,
                           to_char(qa.CREATE_DATE_TIME,'{UIConstants.DateTimeFormat}') CreatedOn,to_char(qa.MOD_DATE_TIME,'{UIConstants.DateTimeFormat}') UpdatedOn,
                           qa.USER_ID UpdatedBy, qm.ques_txt QuestionText,
                           wm.whse_name Whse from QUES_ANS qa inner join QUES_MASTER qm on qm.ques_id = qa.ques_id inner join WHSE_MASTER wm
                           on wm.whse = qa.whse inner join asn_dtl ad on ad.shpmt_nbr = qa.shpmt_nbr where ad.po_nbr = '{UIConstants.QvShipmentNbr}'";
            else return "";
        }
        public static string VerifyAnswerUpdatedSql()
        {
            if (UIConstants.UnAnsQvShipmentNbr != null)
                return $@"SELECT qa.ANS_TEXT from QUES_ANS qa where qa.shpmt_nbr='{UIConstants.UnAnsQvShipmentNbr}' and qa.ques_ans_id = '{UIConstants.QuesAnswerId}'";
            else return "";
        }
        public static string FetchMainPageGridDtSql()
        {
                return $@"select asn_hdr.shpmt_nbr ShipmentNumber,wm.whse_name WarehouseName,asn_hdr.MANIF_NBR ManifestNumber,
                        asn_hdr.MANIF_TYPE ManifestType,asn_hdr.BOL BillOfLading,asn_dtl.po_nbr PoNumber, 
                        asn_hdr.SHIP_VIA ShipVia,asn_hdr.TRLR_NBR TrailerNumber,asn_hdr.WORK_ORD_NBR WorkOrdNumber,asn_hdr.CUT_NBR CutNumber,asn_hdr.MFG_PLNT ManufacturePlant,
                        DECODE(asn_dtl_type,'S','Item level','C','Case Level',asn_dtl_type) ASN_DTL_TYPE AsnDetailType,asn_hdr.ASN_ORGN_TYPE AsnOriginType,
                        asn_hdr.RECV_AGAINST_ASN ReceivingAgainstAsn,asn_hdr.BUYER_CODE BuyerCode,asn_hdr.REP_NAME RepresentativeName,asn_hdr.PRO_NBR ProNumber,
                        asn_hdr.DC_ORD_NBR DcOrderNumber,asn_hdr.CONTRAC_LOCN ContractLocation,asn_hdr.WHSE_XFER_FLAG WarehouseTransferFlag,
                        asn_dtl.QUAL_CHK_HOLD_UPON_RCPT QualityCheckHoldUponReceipt,asn_dtl.QUAL_AUDIT_PCNT QualityAuditPercent,asn_hdr.CASES_SHPD CasesShipped,asn_hdr.UNITS_SHPD UnitsShipped,
                        asn_hdr.CASES_RCVD CasesReceived,asn_hdr.UNITS_RCVD UnitsReceived,
                        to_char(SHPD_DATE_TIME,'{UIConstants.DateTimeFormat}') ShippedOn,
                        to_char(asn_hdr.ARRIVAL_DATE_TIME,'{UIConstants.DateTimeFormat}') ArrivalOn,
                        to_char(asn_hdr.FIRST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') FirstReceiptOn,
                        to_char(asn_hdr.LAST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') LastReceiptOn,
                        to_char(asn_hdr.VERF_DATE_TIME,'{UIConstants.DateTimeFormat}') VerificationDateTime,
                        ltrim(to_char(asn_hdr.VOL,'{UIConstants.VolumeDecimalFormat}')) Volume,
                        ltrim(to_char(asn_hdr.TOTAL_WT,'{UIConstants.DecimalFormat}')) TotalWeight,
                        asn_hdr.AUDIT_STAT_CODE AuditStatusCode,asn_hdr.REF_CODE_1 ReferenceCode1,asn_hdr.REF_FIELD_1 ReferenceField1,
                        get_sc_desc ('B','564',asn_hdr.stat_code,NULL) StatusDescription,
                        asn_hdr.REF_CODE_2 ReferenceCode2,asn_hdr.REF_FIELD_2 ReferenceField2,asn_hdr.REF_CODE_3 ReferenceCode3,
                        asn_hdr.REF_FIELD_3 ReferenceField3,asn_hdr.MISC_INSTR_CODE_1 MiscellaneousInstructionCode1,asn_hdr.MISC_INSTR_CODE_2 MiscellaneousInstructionCode2,
                        asn_hdr.LABEL_PRT LabelPrinter,
                        to_char(asn_hdr.create_date_time,'{UIConstants.DateTimeFormat}') CreatedOn,
                        to_char(asn_hdr.mod_date_time,'{UIConstants.DateTimeFormat}') UpdatedOn,asn_hdr.user_id UpdatedBy
                        ,vendor_master.VENDOR_NAME VendorName from asn_hdr inner join asn_dtl on  asn_dtl.shpmt_nbr=asn_hdr.shpmt_nbr inner join 
                        VENDOR_MASTER on vendor_master.vendor_id=asn_dtl.vendor_id inner join asn_lot_trkg_tbl al on asn_dtl.shpmt_nbr=al.shpmt_nbr 
                        inner join WHSE_MASTER wm on wm.whse = asn_hdr.TO_WHSE where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchDetailsGridDtSql()
        {
            return $@"select asn_dtl.shpmt_nbr ShipmentNumber,asn_dtl.shpmt_seq_NBR ShipmentSequenceNumber,asn_dtl.SKU_ID SkuId,asn_dtl.INVN_TYPE InventoryType,
                    asn_dtl.PROD_STAT ProductState,asn_dtl.BATCH_NBR BatchNumber,
                    asn_dtl.SKU_ATTR_1 SkuAttribute1,asn_dtl.SKU_ATTR_2 SkuAttribute2, asn_dtl.SKU_ATTR_3 SkuAttribute3,
                    asn_dtl.SKU_ATTR_4 SkuAttribute4,asn_dtl.SKU_ATTR_5 SkuAttribute5,
                    asn_dtl.PPACK_GRP_CODE PpackGroupCode,asn_dtl.ASSORT_NBR AssortNumber,asn_dtl.RCVR_NBR ReceiverNumber,
                    asn_dtl.CASES_SHPD CasesShipped,asn_dtl.CASES_RCVD  CasesReceived,asn_dtl.UNITS_RCVD UnitsReceived,asn_dtl.UNITS_SHPD UnitsShipped,
                    UNITS_PRE_RCV UnitsPreReceived,asn_dtl.PROC_IMMD_NEEDS ProcessImmediateNeeds,
                    asn_dtl.MFG_PLNT ManufacturePlant,to_char(asn_dtl.MFG_DATE,'{UIConstants.DateTimeFormat}') ManufactureDate,
                    asn_dtl.QUAL_CHK_HOLD_UPON_RCPT QualityCheckHoldUponReceipt,
                    asn_dtl.QUAL_AUDIT_PCNT QualityAuditPercent,asn_dtl.STOP_RCV_FLAG StopReceiveFlag,asn_dtl.START_SHIP_FLAG StartShippedFlag,
                    asn_dtl.ACTN_CODE ActionCode,asn_dtl.CASE_DIVRT_CODE CaseDivertCode,asn_dtl.PURCH_UOM PurchaseUnitOfMeasure,
                    asn_dtl.PO_NBR PoNumber,asn_dtl.PO_LINE_NBR PoLineNumber,asn_dtl.CUT_NBR CutNumber,asn_dtl.VENDOR_ITEM_NBR VendorItemNumber,
                    asn_dtl.REF_FIELD_1 ReferenceField1,asn_dtl.REF_FIELD_2 ReferenceField2,
                    asn_dtl.REF_FIELD_3 ReferenceField3,asn_dtl.MISC_INSTR_CODE_1 MiscellaneousInstructionCode1,asn_dtl.MISC_INSTR_CODE_2 MiscellaneousInstructionCode2,
                    to_char(asn_dtl.XPIRE_DATE,'{UIConstants.DateTimeFormat}')XPIRE_DATE ExpireDate,
                    to_char(asn_dtl.SHIP_BY_DATE,'{UIConstants.DateTimeFormat}')SHIP_BY_DATE ShipByDate,asn_dtl.TOTAL_CATCH_WT TotalCatchWeight,
                    asn_dtl.NBR_OF_PACK_FOR_CATCH_WT NumberOfPackForCatchWeight,asn_dtl.CUSTOM_PROC CustomProcess,
                    asn_dtl.CNTRY_OF_ORGN CountryOfOrigin,asn_dtl.STD_SUB_PACK_QTY StandardSubPackQuantity,asn_dtl.STD_PACK_QTY StandardPackQuantity,
                    asn_dtl.STD_CASE_QTY StandardCaseQuantity,asn_dtl.CARTON_PER_TIER CartonPerTier,asn_dtl.TIER_PER_PLT TierPerPallet,
                    asn_dtl.WT_RCVD WeightReceived,to_char(asn_dtl.CREATE_DATE_TIME,'{UIConstants.DateTimeFormat}') CreatedOn,
                    to_char(asn_dtl.MOD_DATE_TIME,'{UIConstants.DateTimeFormat}') UpdatedOn,asn_dtl.USER_ID UpdatedBy from asn_dtl
                    inner join asn_hdr on asn_hdr.shpmt_nbr=asn_dtl.shpmt_nbr inner join item_master on item_master.sku_id = asn_dtl.sku_id inner join item_whse_master
                    on item_whse_master.whse = asn_hdr.to_whse and asn_dtl.sku_id=item_whse_master.sku_id  where asn_dtl.shpmt_nbr='{UIConstants.ShipmentNbr}'";
        }
        
        public static string FetchDetailsDrilldownGridDataSql()
        {
            return $@"SELECT whse Warehouse,al.shpmt_nbr ShipmentNumber,al.sku_id SkuId,
                    lot_trkg_data LotTrackingData,lot_trkg_type LotTrackingType,
                    to_char(al.create_date_time,'{UIConstants.DateTimeFormat}') CreatedOn,
                    to_char(al.mod_date_time,'{UIConstants.DateTimeFormat}') UpdatedOn,al.user_id UpdatedBy FROM asn_lot_trkg_tbl al
                    inner join asn_dtl ad on ad.shpmt_nbr=al.shpmt_nbr where ad.shpmt_nbr='{UIConstants.ShipmentNbr}' and ad.sku_id='{UIConstants.ItemNumber}'";
        }


    }
}
