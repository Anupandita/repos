namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
   
    public class ComtQueries
    {
        public static string CommonJoin = $"inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join PICK_LOCN_DTL pl on CASE_DTL.SKU_ID = pl.SKU_ID inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id inner join locn_hdr lh ON CASE_HDR.locn_id = lh.locn_id  inner join locn_grp lg ON lg.locn_id = lh.locn_id inner join sys_code sc ON sc.code_id = lg.grp_type ";
        public static string ReceivedCaseFromReturns = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from CASE_HDR "+ CommonJoin +"  " +
            "where pl.LOCN_ID in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and  " +
            "sc.code_id = :sysCodeId) and CASE_DTL.actl_qty >= :minValue  and CASE_DTL.CASE_SEQ_NBR = :seqNbr  and CASE_DTL.actl_qty <= pl.MAX_INVN_QTY - (pl.ACTL_INVN_QTY + pl.TO_BE_FILLD_QTY - pl.TO_BE_PIKD_QTY)  " +
            "and sc.code_type = :sysCodeType and code_id = :codeIdForDropZone and im.temp_zone in (:dry, :freezer) and CASE_HDR.STAT_CODE = :statCode  and pl.LTST_SKU_ASSIGN = :listSkuAssign";
        public static string ReceivedCaseFromVendors = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR " +
            "inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR " +
            "inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id " +
            "inner join locn_hdr lh ON CASE_HDR.locn_id = lh.locn_id " +
            "inner join locn_grp lg ON lg.locn_id= lh.locn_id " +
            "inner join sys_code sc ON sc.code_id= lg.grp_type " +
            "inner join pick_locn_dtl pl ON pl.sku_id = case_dtl.sku_id " +
            "where CASE_DTL.total_alloc_qty >= :minValue and CASE_DTL.actl_qty>= :minValue and CASE_DTL.CASE_SEQ_NBR = :seqNbr and " +
            "CASE_HDR.stat_code = :statCode and sc.code_type= :sysCodeType and code_id = :codeIdForDropZone and im.temp_zone in (:dry, :freezer) " +
            "and pl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id= lh.locn_id inner join sys_code sc on sc.code_id= lg.grp_type and sc.code_type= :sysCodeType and sc.code_id= :sysCodeId)";
        public static string CaseHdrDtlJoin = $"select case_hdr.stat_code,case_dtl.total_alloc_qty,case_dtl.actl_qty from case_hdr " +
                "inner join case_dtl on case_hdr.case_nbr = case_dtl.case_nbr and case_hdr.case_nbr = :caseNumber";
        public static string CaseHdrDtlTransInvnJoin = $"select TRANS_INVN.MOD_DATE_TIME, CASE_HDR.STAT_CODE,CASE_DTL.ACTL_QTY,CASE_DTL.TOTAL_ALLOC_QTY,TRANS_INVN.ACTL_INVN_UNITS,TRANS_INVN.ACTL_WT from CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  " +
                "inner join TRANS_INVN on CASE_DTL.SKU_ID = TRANS_INVN.SKU_ID  and TRANS_INVN.TRANS_INVN_TYPE = :transInvnType  and CASE_HDR.CASE_NBR = :caseNumber order by TRANS_INVN.MOD_DATE_TIME desc";
        public static string TransInvnForMultiSku = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from " +
                    "trans_invn where sku_id= :skuId  and trans_invn_type = :transInvnType";
        public static string NotEnoughInventory = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.total_alloc_qty <= 0  and CASE_DTL.CASE_SEQ_NBR = 1 and stat_code = 96";
        public static string CasesFromVendorsWithTriggerEnabled = $"select CASE_HDR.CASE_NBR,CASE_HDR.create_date_time,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR   inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  inner join pick_locn_dtl pl ON pl.sku_id = case_dtl.sku_id  where CASE_DTL.total_alloc_qty >= 1 and CASE_DTL.actl_qty >= 1 and CASE_DTL.CASE_SEQ_NBR = 1 and  CASE_HDR.stat_code = 50 and CASE_HDR.locn_id is null  and pl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')  order by create_date_time desc";
        public static string CasesFromReturnsWithTriggerEnabled = $"select CASE_HDR.CASE_NBR,CASE_HDR.create_date_time,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR   inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join pick_locn_dtl pl ON pl.sku_id = case_dtl.sku_id  where CASE_DTL.actl_qty >= 1 and CASE_DTL.CASE_SEQ_NBR = 1 and CASE_HDR.stat_code = 15 and CASE_HDR.locn_id is null and pl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')  order by create_date_time desc";
    }
}
