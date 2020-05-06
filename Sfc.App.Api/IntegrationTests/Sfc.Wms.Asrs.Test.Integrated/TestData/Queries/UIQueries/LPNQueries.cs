using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace FunctionalTestProject.SQLQueries
{
    public static class LpnQueries
    {
        public const string FetchLpnNumberSql = "SELECT distinct ch.case_nbr FROM case_hdr ch inner join case_dtl cd ON cd.case_nbr = ch.case_nbr left outer join " +
                   "locn_hdr lh ON lh.locn_id = ch.locn_id inner join case_cmnt cm on ch.case_nbr= cm.case_nbr " +
                    "WHERE cm.cmnt_type in('INV','LOT','GEN') and ch.stat_code='30'and lh.aisle like '0%' ORDER BY dbms_random.value";
        public const string FetchLpnNbrForUpdateShpmtNbr = "select distinct case_nbr from case_hdr where po_nbr is null and orig_shpmt_nbr is null and dc_ord_nbr is null and stat_code <='45' ORDER BY dbms_random.value";
        public const string FetchLpnNbrForLockUnlock = "SELECT distinct cl.case_nbr from case_lock cl inner join case_lock_cnt cct on cl.case_nbr= cct.case_nbr inner join case_hdr ch on cl.case_nbr= ch.case_nbr where invn_lock_code in ('IR', 'LC','LP','QH','QI','RC','RE','RT','RV','SV','XT')" +
                                                        "and lock_cnt >='2' and ch.stat_code in('10','15','30','45','90','91') ORDER BY dbms_random.value";
        public const string FetchLpnNumberForDeallocate = "SELECT distinct case_nbr from case_hdr where stat_code in('45','50') ORDER BY dbms_random.value";
        public const string FetchLpnNumberForItems = "SELECT distinct ch.case_nbr FROM case_hdr ch inner join case_dtl cd ON cd.case_nbr = ch.case_nbr where ch.stat_code in('45','50','90') ORDER BY dbms_random.value";
        public const string FetchItemNumberSql = "SELECT * FROM(SELECT distinct cd.sku_id,count(*) FROM case_hdr ch inner join case_dtl cd ON cd.case_nbr = ch.case_nbr left outer join locn_hdr lh " +
                    "ON lh.locn_id = ch.locn_id WHERE cd.sku_id IS NOT NULL group by cd.sku_id ORDER BY dbms_random.value) where rownum=1";
        public const string FetchPalletIdSql = "SELECT * FROM(SELECT distinct ch.plt_id, count(*) FROM case_hdr ch inner join case_dtl cd ON cd.case_nbr = ch.case_nbr left outer join locn_hdr lh ON " +
                    "lh.locn_id = ch.locn_id WHERE ch.plt_id IS NOT NULL group by ch.plt_id ORDER BY dbms_random.value) where rownum=1";        
        public static string FetchCreatedDateSql = $"SELECT * FROM(SELECT distinct to_char(ch.CREATE_DATE_TIME,'{UIConstants.EnterDateTimeFormat}'),count(*) FROM case_hdr ch inner join case_dtl" +
            $" cd ON cd.case_nbr = ch.case_nbr left outer join locn_hdr lh ON lh.locn_id = ch.locn_id WHERE ch.CREATE_DATE_TIME IS NOT NULL group by to_char(ch.CREATE_DATE_TIME,'{UIConstants.EnterDateTimeFormat}') ORDER BY dbms_random.value) where rownum=1";
        public const string FetchLpnNumberForConsumed = "select distinct ch.case_nbr from case_hdr ch inner join case_lock cl on ch.case_nbr= cl.case_nbr inner join case_cmnt cm on ch.case_nbr= cm.case_nbr where ch.stat_code >95 ORDER BY dbms_random.value";
        public const string FetchLpnNumberForHistory = "select distinct ach.case_nbr from sfc_audit_case_hdr ach INNER JOIN case_hdr ch on ch.case_nbr=ach.case_nbr ORDER BY dbms_random.value";
        public const string FetchSlotAisle = "SELECT distinct lh.ZONE,lh.AISLE,lh.BAY,lh.LVL FROM case_hdr ch inner join case_dtl cd ON cd.case_nbr = ch.case_nbr left outer join locn_hdr lh " +
                     "ON lh.locn_id = ch.locn_id WHERE ch.locn_id IS NOT NULL ORDER BY dbms_random.value";
        public static string FetchLpnPageGridDtSql() => $@"SELECT ch.CASE_NBR caseNumber,ch.RCVD_SHPMT_NBR receivedShipmentNumber ,DSP_LOCN displayLocation,cd.SKU_ID skuId,SKU_DESC skuDescription,DC_ORD_NBR distributionCenterOrderNumber,LOCK_CNT lockCount,
                   CONS_CASE_PRTY consumeCasePriority, CONS_PRTY_DATE consumePriorityDate,CONS_SEQ consumeSequence,
                   ch.MFG_DATE manufacturingOn,RCVD_DATE receivedOn,ch.XPIRE_DATE expiryDate,
                   ch.PROC_IMMD_NEEDS processImmediateNeeds,VOL volume,ltrim(to_char(EST_WT,'{UIConstants.DecimalFormat}')) estimateWeight,ltrim(to_char(ACTL_WT,'{UIConstants.DecimalFormat}')) actualWeight,ch.PO_NBR poNumber,get_sc_desc ('B','509',ch.stat_code,NULL) codeDescription ,
                   VENDOR_NAME vendorName,PHYS_ENTITY_CODE physicalEntityCode,PLT_ID plantId,SNGL_SKU_CASE singleSkuCase,ch.SPL_INSTR_CODE_1 specialInstructionCode1,ch.SPL_INSTR_CODE_2 specialInstructionCode2,
                   ch.SPL_INSTR_CODE_3 specialInstructionCode3,ch.SPL_INSTR_CODE_4 specialInstructionCode4,ch.SPL_INSTR_CODE_5 specialInstructionCode5,ltrim(to_char(TOTAL_ALLOC_QTY,'{UIConstants.QuantityFormat}')) totalAllocatedQuantity, ltrim(to_char(ORIG_QTY,'{UIConstants.QuantityFormat}')) originalQuantity,
                   ltrim(to_char(ACTL_QTY,'{UIConstants.QuantityFormat}')) actualQuantity,ltrim(to_char(SHPD_ASN_QTY,'{UIConstants.QuantityFormat}')) shippedAsnQuantity,ch.CREATE_DATE_TIME createdOn,ch.MOD_DATE_TIME updatedOn,
                   sw.First_name || ' ' || sw.last_name userName,ad.vendor_item_nbr vendorItemNumber, cd.cut_nbr cutNumber,ltrim(to_char(ad.units_rcvd-ad.units_shpd,'{UIConstants.QuantityFormat}'))  varianceQuantity FROM CASE_HDR ch INNER JOIN CASE_DTL cd ON cd.case_nbr = ch.case_nbr
                   INNER JOIN ITEM_MASTER im ON im.sku_id = cd.sku_id  INNER JOIN whse_master wm ON wm.whse = ch.whse left outer join LOCN_HDR  lh ON lh.locn_id = ch.locn_id LEFT OUTER JOIN CASE_LOCK_CNT
                   clc ON clc.case_nbr = ch.case_nbr LEFT OUTER JOIN VENDOR_MASTER vm ON vm.vendor_id = ch.vendor_id left outer join case_cmnt cm on cm.case_nbr = ch.case_nbr left outer join swm_user_master sw on sw.user_name = ch.user_id 
                   left outer join asn_dtl ad on ch.orig_shpmt_nbr= ad.shpmt_nbr and cd.sku_id = ad.sku_id WHERE ch.case_nbr = '{UIConstants.LpnNumber}'";
        public static string FetchGetLpnCount()
        {
            return $@"SELECT count(*) from case_hdr ch inner join case_dtl cd on ch.case_nbr = cd.case_nbr where ch.stat_code >={UIConstants.LpnFromStatus} and ch.stat_code <={UIConstants.LpnToStatus}";
        }

        public static string FetchHistoryGridDtSql()
        {
            return $@"SELECT audit_seq_nbr AuditSequenceNumber,event,audit_datetime auditDateTime,getlocation (whse, bf_locn_id) beforeLocationId,
                       getlocation(whse, bf_prev_locn_id) beforePreviousLocationId,ltrim(to_char(bf_actl_wt,'{UIConstants.DecimalFormat}')) beforeActualWeight,rtrim(get_sc_desc('B','509',bf_stat_code,NULL)) beforeStatusCode, bf_rsn_code BeforeReasonCode,
                       to_char(bf_create_date_time,'{UIConstants.FormatDateTime}') beforeCreateDateTime,to_char(bf_mod_date_time,'{UIConstants.FormatDateTime}') beforeModifiedDateTime,s1.first_name ||' '||s1.last_name beforeUser,getlocation(whse, af_locn_id) afterLocationId,getlocation(whse, af_prev_locn_id) afterPrevLocn,
                       getlocation (whse, af_prev_locn_id) afterPreviousLocationId,ltrim(to_char(af_actl_wt,'{UIConstants.DecimalFormat}')) afterActualWeight,rtrim(get_sc_desc('B','509',af_stat_code,NULL)) afterStatusCode,af_rsn_code AfterReasonCode,to_char(af_create_date_time,'{UIConstants.FormatDateTime}') afterCreateDateTime,to_char(af_mod_date_time,'{UIConstants.FormatDateTime}') afterModifiedDateTime,
                       s2.first_name ||' '||s2.last_name afterUser,af_plt_id afterPalletId,bf_rsn_code beforeReasonCode, af_rsn_code afterReasonCode,case_nbr,af_cons_case_prty afterConsCasePriority,af_db_user afterDbUser,to_char(af_cons_prty_date,'{UIConstants.FormatDateTime}') afterConsPriorityDate FROM sfc_audit_case_hdr sc left outer join swm_user_master s1 on s1.user_name=sc.bf_user_id left outer join swm_user_master s2 on s2.user_name=sc.af_user_id WHERE whse = '{UIConstants.Whse}'
                       AND case_nbr = '{UIConstants.LpnNumberForHistory}' order by audit_seq_nbr";
        }
        public static string FetchDrilldownTabDtSql()
        {
            return $@"Select cd.ACTL_QTY ""Current LPN Quantity"",ORIG_SHPMT_NBR ""Shipment Number"",PLT_ID ""Pallet Number"",(ch.CASE_TYPE || ' / ' || ch.CASE_SIZE_TYPE) ""LPN Type / Size"",
               ltrim(to_char(EST_WT,'99990.00')) ""Estimated Weight"" , ltrim(to_char(ACTL_WT,'{UIConstants.DecimalFormat}')) ""Weight"",ltrim(to_char(VOL,'{UIConstants.VolumeDecimalFormat}')) ""Volume"",getlocation (ch.whse, PREV_LOCN_ID) ""Previous Location"",
                getlocation (ch.whse, ch.LOCN_ID) ""Current Location"",getlocation (ch.whse, DEST_LOCN_ID) ""Destination Location"",
                RCVD_SHPMT_NBR ""Receipt"",ORIG_SHPMT_NBR ""Original Receipt"",PO_NBR ""PO Number"",vm.VENDOR_Name ""Vendor"",
               to_char(RCVD_DATE,'{UIConstants.FormatDateTime}') ""Received Date"",SHIP_VIA ""Ship Via"",
                TRLR_NBR ""Trailer"",MANIF_TYPE ""Manfest Type"",MANIF_NBR ""Manifest Number"",MFG_PLNT ""Manufacturing Plant"",to_char(MFG_DATE,'{UIConstants.FormatDateTime}') ""Manufactured Date"",
                to_char(XPIRE_DATE,'{UIConstants.FormatDateTime}') ""Expiration Date"",DC_ORD_NBR ""DC Order Nbr"",WORK_ORD_NBR ""Work Order Nbr"",REWRK_CODE ""Rework Code"",
                CONS_CASE_PRTY ""Consume Priority"",to_char(CONS_PRTY_DATE,'{UIConstants.FormatDateTime}') ""Consume Date"",CONS_SEQ ""Consume Sequence"",PHYS_ENTITY_CODE ""Physical Entity"",
                RSN_CODE ""Reason Code"",CASE_DIVRT_CODE ""Divert Code"",
                DECODE(ch.SPL_INSTR_CODE_1,'','-',ch.SPL_INSTR_CODE_1)|| '' || DECODE(ch.SPL_INSTR_CODE_2,'','-',ch.SPL_INSTR_CODE_2) ||''||DECODE(ch.SPL_INSTR_CODE_3,'','-',ch.SPL_INSTR_CODE_3)||''||DECODE(ch.SPL_INSTR_CODE_4,'','-',ch.SPL_INSTR_CODE_4)||''||DECODE(ch.SPL_INSTR_CODE_5,'','-',ch.SPL_INSTR_CODE_5) ""Special Instructions""
                FROM CASE_HDR ch
                INNER JOIN CASE_DTL cd ON cd.case_nbr = ch.case_nbr
                LEFT OUTER JOIN VENDOR_MASTER vm ON vm.vendor_id = ch.vendor_id WHERE ch.case_nbr = '{UIConstants.LpnNumber}'";
        }
        public static string FetchCaseLockDtSql()
        {
            return $@"Select cl.invn_lock_code InventoryLockId, get_sc_desc('B','527',cl.invn_lock_code,NULL) inventoryLockCode, cm.cmnt lockComment, cl.create_date_time lockDateTime,cm.cmnt_seq_nbr CommentSequenceNumber from case_lock cl 
                left outer join case_cmnt cm on cm.case_nbr=cl.case_nbr and cm.cmnt_code=cl.invn_lock_code where cl.case_nbr = '{UIConstants.LpnNbrForLockUnlock}' order by cm.create_date_time desc";
        }
        public static string FetchCaseCommentsDtSql() => $@"select cm.cmnt_type CommentType,get_sc_desc ('B','346',cm.cmnt_type,NULL) SystemCodeCommentType ,cm.cmnt_code CommentCode,get_sc_desc ('B','347',cm.cmnt_code,NULL) SystemCodeCommentCode , cm.cmnt CommentText,cm.cmnt_seq_nbr CommentSequenceNumber from case_cmnt cm 
                                     where cm.case_nbr = '{UIConstants.LpnNumber}' order by cm.create_date_time asc";
        public static string FetchItemsDtSql() => $@"SELECT SKU_ID skuId,CUT_NBR cutNumber, ASSORT_NBR assortmentNumber,SHPD_ASN_QTY shippedAsnQuantity,ORIG_QTY originalQuantity,ACTL_QTY actualQuantity,TOTAL_ALLOC_QTY totalAllocatedQuantity,to_char(CREATE_DATE_TIME,'{UIConstants.FormatDateTime}') createdOn from case_dtl where case_nbr = '{UIConstants.LpnNumberForItems}'";
    }
}
