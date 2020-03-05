namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
   
    public class OrstQueries
    {

        public static string ValidDataForInsertingOrstMessage = $"select sm.order_id,sm.created_date_time,sm.sku_id,sm.qty,sm.msg_json,ch.curr_locn_id,pl.locn_id,ph.ship_w_ctrl_nbr,ch.dest_locn_id from swm_to_mhe sm " +
                "inner join carton_hdr ch ON sm.order_id = ch.carton_nbr " +
                "inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                "inner join alloc_invn_dtl al ON al.task_cmpl_ref_nbr = ch.carton_nbr " +
                "inner join pick_locn_dtl pl ON pl.sku_id = sm.sku_id " +
                "inner join pkt_dtl pd ON pd.pkt_ctrl_nbr =ch.pkt_ctrl_nbr";
        public static string PickTktSeqNbrIsLessThan1 = $"select sm.order_id,sm.sku_id,sm.qty,sm.msg_json,ch.curr_locn_id,pl.locn_id,ph.ship_w_ctrl_nbr,ch.dest_locn_id from swm_to_mhe sm " +
                "inner join carton_hdr ch ON sm.order_id = ch.carton_nbr " +
                "inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                "inner join alloc_invn_dtl al ON al.task_cmpl_ref_nbr = ch.carton_nbr " +
                "inner join pick_locn_dtl pl ON pl.sku_id = sm.sku_id " +
                "inner join pkt_dtl pd ON pd.pkt_ctrl_nbr =ch.pkt_ctrl_nbr" +
                " where sm.source_msg_status = :status and ch.stat_code = :cartonStatusCode and ph.pkt_stat_code < :pktStatusCode and pd.pkt_seq_nbr <= 0";
        public static string MsgToSortView = $"select * from msg_to_sv where message_type= :msgType and PTN= :cartonNo";
        public static string CartonHeader = $"select CURR_LOCN_ID, DEST_LOCN_ID,MOD_DATE_TIME, USER_ID, STAT_CODE from CARTON_HDR where CARTON_NBR= :cartonNumber";
        public static string CartonDetail = $"select * from carton_dtl where carton_nbr = :cartonNumber";
        public static string PickTktHeader = $"select PKT_HDR.PKT_STAT_CODE,PKT_HDR.MOD_DATE_TIME,PKT_HDR.USER_ID from PKT_HDR " +
                "WHERE PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR = :cartonNumber)";
        public static string PickTktDtl = $"select PKT_DTL.UNITS_PAKD,PKT_DTL.MOD_DATE_TIME,PKT_DTL.USER_ID ,PKT_DTL.VERF_AS_PAKD,PKT_DTL.PKT_CTRL_NBR from PKT_DTL " +
                "where PKT_DTL.PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR= :cartonNbr) AND PKT_SEQ_NBR= :pktSeqNbr";
        public static string AllocInventory = $"select * from ALLOC_INVN_DTL WHERE task_cmpl_ref_nbr= :cntrNbr order by mod_date_time desc";
        
    }
}
