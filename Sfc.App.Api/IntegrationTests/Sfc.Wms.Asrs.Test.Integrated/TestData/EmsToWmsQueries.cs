namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
   
    public class EmsToWmsQueries
    {
        public static string CostQuery= $"select swm_to_mhe.container_id,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                " on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                "inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = :ready and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode" +
                " and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id " +
                "inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :codeId) order by swm_to_mhe.created_date_time desc ";

        public static string CostFetchDataWithOutTransInvn = $"select cd.SKU_ID,ch.CASE_NBR,tn.ACTL_INVN_UNITS,ch.STAT_CODE,pick_locn_dtl.locn_id from CASE_HDR ch inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR inner join pick_locn_dtl on pick_locn_dtl.sku_id = cd.sku_id  left join trans_invn tn on tn.SKU_ID = cd.SKU_ID and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS > 1 and trans_invn_type = 18 and tn.SKU_ID = null";

        public static string CostPickLocnDoesNotExist = $"select tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join trans_invn tn on tn.SKU_ID = cd.SKU_ID  " +
                "left join pick_locn_dtl on pick_locn_dtl.sku_id = tn.sku_id  and ch.STAT_CODE = :statCode and tn.ACTL_INVN_UNITS > :qty and " +
                "trans_invn_type = :transInvnType ";
    }
}
