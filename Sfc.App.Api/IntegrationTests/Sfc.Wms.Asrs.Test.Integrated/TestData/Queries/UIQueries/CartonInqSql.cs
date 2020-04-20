using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace FunctionalTestProject.SQLQueries
{
    public static class CartonInqSql
    {
        public const string FetchCartonSql = "select distinct ch.CARTON_NBR from CARTON_HDR ch inner join carton_cmnt cm on ch.carton_nbr=cm.carton_nbr " +
             "inner join carton_dtl cd on ch.carton_nbr = cd.carton_nbr inner join pkt_dtl pd on pd.pkt_ctrl_nbr = cd.pkt_ctrl_nbr " +
             "inner join msg_to_sv mts on mts.ptn = ch.carton_nbr inner join msg_from_sv mfs on mfs.ptn = ch.carton_nbr " +
             "inner join hosp_log hl on hl.ptn = ch.carton_nbr where ch.WHSE = '008'";
        public const string FetchCartonPickLocnSql = "select distinct ACTV.ZONE,ACTV.AISLE,ACTV.BAY,ACTV.LVL from " +
                    "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                    "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                    "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                    "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                    "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                    "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchsCurrentLocnSql = "select distinct LH.ZONE,LH.AISLE,LH.BAY,LH.LVL from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchPktCtrlNbrSql = "select distinct CH.PKT_CTRL_NBR from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchWaveNbrSql = "select distinct CH.WAVE_NBR from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchPnhCtrlNbrSql = "select distinct PH.PNH_CTRL_NBR from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchShpmtNbrSql = "select distinct PH.SHPMT_NBR from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchMasterPackIdSql = "select distinct CH.MASTER_PACK_ID from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchPalletIdSql = "select distinct CH.PLT_ID from " +
                  "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                  "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                  "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                  "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                  "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                  "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public const string FetchCartonSkuSql = "select distinct IM.SKU_ID from " +
                 "CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
                 "INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
                 "INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
                 "INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
                 "INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
                 "where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008'";
        public static string FetchCartonPageGridDtsql()
        {
            return "select CH.CARTON_NBR,CH.PKT_CTRL_NBR,CH.WAVE_NBR," +
"IM.SKU_ID,IM.SKU_DESC,CH.SNGL_SKU_FLAG,CH.TOTAL_QTY," +
"get_sc_desc('B', '502', CH.STAT_CODE, NULL) CODE_DESC,CH.CURR_LOCN_ID,PH.TRLR_STOP_SEQ_NBR FROM " +
"CARTON_HDR CH INNER JOIN  LOCN_HDR ACTV ON CH.PICK_LOCN_ID = ACTV.LOCN_ID " +
"INNER JOIN LOCN_HDR LH ON CH.curr_locn_id = LH.locn_id " +
"INNER JOIN WHSE_LOCN_MASK WLM ON LH.locn_class = WLM.locn_class " +
"INNER JOIN ITEM_MASTER IM ON CH.SKU_ID = IM.SKU_ID " +
"INNER JOIN PKT_HDR PH ON CH.PKT_CTRL_NBR = PH.PKT_CTRL_NBR " +
$"where CH.WHSE = '008' AND LH.WHSE = '008' AND WLM.WHSE = '008' AND CH.CARTON_NBR = '{UIConstants.CartonNbr}'";
        }
        public static string FetchCartonHeaderDtsql()
        {
            return $@"select CARTON_HDR.CARTON_NBR ""Carton Nbr"",get_sc_desc('B','502',CARTON_HDR.STAT_CODE,NULL) ""Status"",
                    CARTON_HDR.CARTON_NBR_X_OF_Y ""X of Y"",CARTON_HDR.CARTON_SIZE ""Carton Size"",CARTON_HDR.CARTON_TYPE ""Carton Type"",
                    get_sc_desc('B','731',CARTON_HDR.CARTON_CREATION_CODE,NULL) ""Creation Code"",CARTON_HDR.PKT_CTRL_NBR ""Pickticket Control Nbr""
                    from CARTON_HDR WHERE CARTON_HDR.CARTON_NBR='{UIConstants.CartonNbr}'";
        }
        public static string FetchCartonDrillTabDtsql() { return $@"select getlocation(CARTON_HDR.whse,CARTON_HDR.PICK_LOCN_ID) ""Pick Location"",getlocation(CARTON_HDR.whse,CARTON_HDR.CURR_LOCN_ID) ""Current Location"",
                      getlocation(CARTON_HDR.whse,CARTON_HDR.PREV_LOCN_ID) ""Previous Location"",CARTON_HDR.DEST_LOCN_ID ""Destination Location"",CARTON_HDR.CARTON_DIVRT_CODE ""Carton Divert Code"",
                      CARTON_HDR.PIKR ""Picker"",CARTON_HDR.PAKR ""Packer"",CARTON_HDR.WAVE_NBR ""Wave Nbr"",CARTON_HDR.WAVE_SEQ_NBR ""Wave Seq Nbr"",
                      get_sc_desc('B','595',CARTON_HDR.WAVE_STAT_CODE,NULL) ""Wave Status"",CARTON_HDR.TRLR_NBR ""Nbr"",CARTON_HDR.TRLR_ZONE_INDIC ""Zone Indicator"",
                      CARTON_HDR.TRLR_POSN_X_COORD ""X Coordinate"",CARTON_HDR.TRLR_POSN_Y_COORD ""Y Coordinate"",CARTON_HDR.PICK_DLVRY_DURTN ""Pick Delivery Duration"",
                      CARTON_HDR.IDEAL_PICK_DATE_TIME ""Ideal Pick Date Time"",CARTON_HDR.BPI ""BPI"",CARTON_HDR.PROD_VALUE ""Product Value"",CARTON_HDR.FRT_CHRG ""Freight Charge"",
                      CARTON_HDR.BOL ""BOL"",CARTON_HDR.TRKG_NBR ""Tracking Nbr"",ltrim(to_char(CARTON_HDR.EST_WT,UIConstants.DecimalFormat)) ""Estimated Weight"",ltrim(to_char(CARTON_HDR.ACTL_WT,UIConstants.DecimalFormat)) ""Actual Weight"",ltrim(to_char(CARTON_HDR.EST_VOL,UIConstants.VolumeDecimalFormat)) ""Estimated Volume"",
                      CARTON_HDR.USER_ID ""User ID"",CARTON_HDR.FIRST_ZONE ""First Zone"",CARTON_HDR.LAST_ZONE ""Last Zone"",CARTON_HDR.NBR_OF_ZONES ""Nbr Of Zones"",
                      CARTON_HDR.STAGE_INDIC ""Stage Indicator"",CARTON_HDR.LOAD_SEQ ""Load Seq"",CARTON_HDR.MANIF_NBR ""Manifest Nbr"",CARTON_HDR.SPUR_LANE ""Spur Lane"",CARTON_HDR.SPUR_POSN ""Spur Position"",
                      CARTON_HDR.PLT_ID ""Pallet ID"",CARTON_HDR.PLT_TYPE ""Pallet Type"",CARTON_HDR.PLT_SIZE ""Pallet Size"",CARTON_HDR.PLT_SIZE_TYPE ""Pallet Size Type"",CARTON_HDR.PLT_NBR_X_OF_Y ""X Of Y"",
                      CARTON_HDR.MASTER_PACK_ID ""Master Pack Id"",CARTON_HDR.TRANS_INVN_TYPE ""Trans Inventory Type"",CARTON_HDR.REF_CASE_NBR ""Ref LPN Nbr"",CARTON_HDR.SKU_ID ""Item"",CARTON_HDR.TOTAL_QTY ""Total Qty"",
                      get_sc_desc('B','502',CARTON_HDR.SKU_STAT_CODE,NULL) ""Item Status Code"",CARTON_HDR.SNGL_SKU_FLAG ""Single Item Flag [checkbox]"",CARTON_HDR.SEQ_RULE_PRTY ""Sequence Rule Prty"",
                      CARTON_HDR.OVRSZ_LEN ""Oversize Length"",CARTON_HDR.LAST_FROZN_DATE_TIME ""Last Frozen Date Time"",CARTON_HDR.REPRT_CNT ""Reprint Count"",CARTON_HDR.ERROR_ID ""Error ID"",
                      CARTON_HDR.ERROR_MODULE ""Error Module"",CARTON_HDR.MISC_INSTR_CODE_1||' '||CARTON_HDR.MISC_INSTR_CODE_2||' '||CARTON_HDR.MISC_INSTR_CODE_3||' '||CARTON_HDR.MISC_INSTR_CODE_4||' '||CARTON_HDR.MISC_INSTR_CODE_5 ""Miscellaneous"",
                      CARTON_HDR.MISC_NUM_1 ""Misc Nbr 1"",CARTON_HDR.MISC_NUM_2 ""Misc Nbr 2"" from CARTON_HDR where CARTON_HDR.CARTON_NBR='{UIConstants.CartonNbr}'"; }
        public static string FetchCommentsGridDtsql() { return $"SELECT carton_cmnt.cmnt_type, carton_cmnt.cmnt_code, carton_cmnt.cmnt from carton_cmnt where carton_cmnt.CARTON_NBR='{UIConstants.CartonNbr}'"; }
        public static string FetchDetailsGridDtsql()
        {
            return "SELECT item_master.dsp_sku,pkt_dtl.invn_type,pkt_dtl.prod_stat,pkt_dtl.sku_attr_1,pkt_dtl.sku_attr_2,pkt_dtl.sku_attr_3,pkt_dtl.sku_attr_4," +
            "pkt_dtl.sku_attr_5,pkt_dtl.batch_nbr,pkt_dtl.cntry_of_orgn,carton_dtl.pack_code,carton_dtl.to_be_pakd_units,carton_dtl.units_pakd,carton_dtl.line_item_stat," +
            "carton_dtl.carton_nbr,carton_dtl.carton_seq_nbr,item_master.dsp_uom,item_master.stk_uom FROM item_master, pkt_dtl,carton_dtl " +
            "WHERE pkt_dtl.pkt_ctrl_nbr = carton_dtl.pkt_ctrl_nbr AND pkt_dtl.pkt_seq_nbr = carton_dtl.pkt_seq_nbr AND item_master.sku_id = pkt_dtl.sku_id " +
            $"AND CARTON_DTL.CARTON_NBR = '{UIConstants.CartonNbr}'";
        }
        public static string FetchSerialNumbersGridDtsql()
        {
            return "SELECT item_master.dsp_sku,pkt_dtl.invn_type,pkt_dtl.prod_stat,pkt_dtl.sku_attr_1,pkt_dtl.sku_attr_2,pkt_dtl.sku_attr_3," +
        "pkt_dtl.sku_attr_4,pkt_dtl.sku_attr_5,pkt_dtl.batch_nbr,pkt_dtl.cntry_of_orgn,carton_sku_srl_nbr.srl_nbr,carton_sku_srl_nbr.units " +
        "FROM item_master,pkt_dtl,carton_dtl,carton_sku_srl_nbr WHERE carton_dtl.pkt_seq_nbr = pkt_dtl.pkt_seq_nbr AND " +
        "carton_dtl.pkt_ctrl_nbr = pkt_dtl.pkt_ctrl_nbr AND item_master.sku_id = pkt_dtl.sku_id AND " +
        "carton_dtl.carton_nbr = carton_sku_srl_nbr.carton_nbr AND carton_dtl.carton_seq_nbr = carton_sku_srl_nbr.carton_seq_nbr " +
        $"AND carton_sku_srl_nbr.carton_nbr = '{UIConstants.CartonNbr}'";
        }
        public static string FetchToSorationGridDtsql()
        {
            return "SELECT msg_to_sv.message_date,msg_to_sv.message_type,msg_to_sv.ptn,msg_to_sv.sort_lane_id,msg_to_sv.stat_code,msg_to_sv.wavenum " +
                    $"FROM msg_to_sv WHERE  msg_to_sv.ptn = '{UIConstants.CartonNbr}'";
        }
        public static string FetchFromSorationGridDtsql()
        {
            return "SELECT message_id,message_date,message_type,ptn,sort_lane_id,reject_message_id,reject_reason_code,reject_reason_text,stat_code," +
                $"pkms_error_id,create_date_time,mod_date_time FROM msg_from_sv WHERE msg_from_sv.ptn ='{UIConstants.CartonNbr}'";
        }
        public static string FetchHospitalLogGridDtsql()
        {
            return "SELECT ptn_sku_id item,actl_sku_id actual_item,get_sc_desc ('B','502',intl_ptn_stat,NULL) intl_ptn_status,error_id,orig_tote,tote,pikr," +
                "pakr,correct_sku_flag correct_item,get_sc_desc ('B', '761', hndl_code, NULL) hndl_code,rmv_frm_tote,ltrim(to_char(updt_ptn_wt,UIConstants.DecimalFormat)),new_lpn,pix,epick_gen," +
                $"user_id,create_date_time,ltrim(to_char(wt_adjmt,UIConstants.DecimalFormat)) FROM hosp_log WHERE ptn = '{UIConstants.CartonNbr}'";
        }
    }
}
