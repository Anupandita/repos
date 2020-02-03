namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    public class OrmtQueries
    {

        public static string CommonSelect = $"select distinct eg.CARTON_NBR,eg.CREATED_DATE_TIME,ch.wave_nbr,ch.sku_id,pl.locn_id,ch.MISC_NUM_1," +
                                        " ch.total_qty ,ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse," +
                                        "ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5";
        public static string CommonJoin = $"inner join carton_hdr ch on ch.Carton_Nbr = eg.Carton_Nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                                        "inner join Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id inner join ITEM_MASTER im ON ch.sku_id = im.sku_id  " +
                                        "inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id  " +
                                        "inner join sys_code sc ON sc.code_id = lg.grp_type inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                                        "inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id " +
                                        "left  join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id ";
        public static string CommonWhereCondition = $"where  ch.misc_instr_code_5 is null and misc_num_1 = :miscNum1 and sc.code_type = :sysType " +
                                                "and sc.code_id = :sysCodeId and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') " +
                                                " and eg.status = :status and ch.stat_code = 5";

        public static string ValidDataForOnProcessingCostMessage = $"select distinct soc.CARTON_NBR,soc.CREATED_DATE_TIME,ch.wave_nbr,ch.sku_id,pl.locn_id,ch.MISC_NUM_1, " +
                "ch.total_qty ,ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, " +
                "im.spl_instr_code_1, im.spl_instr_code_5 from swm_from_mhe sfm inner join carton_hdr ch on sfm.SKU_ID = ch.SKU_ID " +
                "inner join SWM_ELGBL_ORMT_CARTONS soc on soc.carton_nbr = ch.carton_nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = " +
                "ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id inner join ITEM_MASTER im ON ch.sku_id = im.sku_id " +
                "inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                "inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id " +
                "inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id " +
                "inner join sys_code sc ON sc.code_id = lg.grp_type where ch.misc_instr_code_5 is null and misc_num_1 = 0 and " +
                "sc.code_type = :sysType and code_id = :sysCodeId and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and " +
                "ch.misc_instr_code_5 is null and misc_num_1 = :miscNum1 and soc.STATUS = 50 and sfm.source_msg_trans_code = 'COST' " +
                "ORDER BY soc.CARTON_NBR,soc.CREATED_DATE_TIME";

        public static string UpdatePickStatCode = $"update pkt_hdr set pkt_stat_code = :pktStatCode where pkt_ctrl_nbr = :pktCtrlNbr";
        public static string ActiveOrmtNotFound = $"select ch.carton_nbr,ch.sku_id,ch.wave_nbr,ch.MISC_NUM_1, ch.total_qty, ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR," +
                "ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from CARTON_HDR ch " +
                "inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                "inner join Pick_Locn_dtl pl ON  pl.locn_id = ch.pick_locn_id " +
                "inner join ITEM_MASTER im ON pl.sku_id = im.sku_id inner join locn_hdr lh ON pl.locn_id = lh.locn_id " +
                "inner join locn_grp lg ON lg.locn_id = lh.locn_id inner join pick_locn_dtl_ext ple ON ple.sku_id = ch.sku_id " +
                "inner join sys_code sc ON sc.code_id = lg.grp_type " +
                "where sc.code_type = :sysType and sc.code_id = :sysCodeId and " +
                "lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and pl.actl_invn_qty <= 0";

        public static string PickLocnNotFound = $"select * from carton_hdr ch join pick_locn_dtl pl  ON pl.sku_id = ch.sku_id  where ch.sku_id not in " +
                "(select sku_id from pick_locn_dtl where locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on " +
                "lg.locn_id=lh.locn_id inner join sys_code sc on sc.code_id=lg.grp_type and sc.code_type= :sysType and sc.code_id= :sysCodeId)) " +
                "and stat_code = 5 and ch.misc_instr_code_5 is null and misc_num_1 = 0 and pl.actl_invn_qty > 0";

        public static string CartonHeader = $"Select * from carton_hdr where carton_nbr = :cartonNbr";
        public static string SwmElgblOrmtCount = $"Select * from SWM_ELGBL_ORMT_CARTONS where carton_nbr = :cartonNbr order by updated_date_time desc";

        public static string ActiveLocnNotFound = $"select ch.carton_nbr,ch.sku_id,pl.locn_id,ch.wave_nbr from carton_hdr ch inner join " +
                "Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id where locn_id not in (select lh.locn_id from locn_hdr lh " +
                "inner join locn_grp lg on lg.locn_id = lh.locn_id and lg.grp_attr in ('Freezer', 'Dry') " +
                "inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId)";

        public static string WaveRelease = $"select  distinct eg.wave_nbr,ch.carton_nbr,ch.sku_id,ch.MISC_NUM_1,ch.total_qty, " +
            "ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, " +
            "im.spl_instr_code_1, im.spl_instr_code_5 from SWM_ELGBL_ORMT_CARTONS eg inner join carton_hdr ch on" +
            " ch.Carton_Nbr = eg.Carton_Nbr  inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr  " +
            "inner join Pick_Locn_dtl pl ON  pl.sku_id = ch.sku_id inner join ITEM_MASTER im ON ch.sku_id = im.sku_id  " +
            "inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id " +
            "inner join sys_code sc ON sc.code_id = lg.grp_type inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr  " +
            "inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id " +
            "left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id where  ch.misc_instr_code_5 is null and misc_num_1 = 0 and sc.code_type = :sysType " +
            "and code_id = :sysCodeId and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and ch.misc_instr_code_5 is null and misc_num_1 = 0 and " +
            "eg.wave_nbr = '{waveNbr}' and eg.status = :status and ch.stat_code = 5";

        public static string ValidCartons = $"select distinct ch.carton_nbr,ch.create_date_time,ch.wave_nbr,ch.sku_id,pl.locn_id,ch.MISC_NUM_1, ch.total_qty," +
            $" ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse, ph.CO,ph.DIV, im.spl_instr_code_1," +
            $" im.spl_instr_code_5 from SWM_ELGBL_ORMT_CARTONS eg  inner join carton_hdr ch on ch.Carton_Nbr = eg.Carton_Nbr " +
            $"inner join PKT_HDR ph ON  ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id " +
            $"inner join ITEM_MASTER im ON ch.sku_id = im.sku_id  inner join locn_hdr lh ON  pl.locn_id = lh.locn_id" +
            $" inner join locn_grp lg ON lg.locn_id = lh.locn_id   inner join sys_code sc ON sc.code_id = lg.grp_type  " +
            $" inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr  inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
            $" inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id " +
            $" where ch.misc_instr_code_5 is null and misc_num_1 = 0 and sc.code_type = '740' and code_id = '18' and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and  ch.misc_instr_code_5 is null and misc_num_1 = 0  and eg.status = 10 ";
    }
}
