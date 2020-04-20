using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace FunctionalTestProject.SQLQueries
{
    public static class ReserveLocationQueries
    {
        public const string FetchReserveLocnSql = @"select distinct lh.zone,lh.aisle,lh.bay,lh.lvl from locn_hdr lh inner join whse_sys_code ws on ws.whse=lh.whse where lh.locn_class='R'and rownum='1' ORDER BY dbms_random.value";
        public const string FetchReserveLocnGrpSql = @"select distinct get_sc_desc('S','740',lg.GRP_TYPE,NULL) ""GRP_TYPE"",grp_attr
                     from locn_grp lg inner join locn_hdr lh on lh.locn_id = lg.locn_id where locn_class = 'R' and rownum = '1' ORDER BY dbms_random.value";
        public const string FetchPutawayZoneSql = "select distinct code_desc from locn_hdr lh inner join WHSE_SYS_CODE wsc on " +
                    "lh.putwy_zone = wsc.code_id where wsc.code_type = '599' AND wsc.rec_type = 'B' ORDER BY dbms_random.value";
        public static string FetchMainPageGridDtSql() { return $@"SELECT lh.DSP_LOCN, RESV_LOCN_HDR.MAX_UOM_QTY,RESV_LOCN_HDR.CURR_UOM_QTY,
                    RESV_LOCN_HDR.DIRCT_UOM_QTY,wsc.code_desc ""PUTAWAY_ZONE_DESC"" FROM  WHSE_SYS_CODE wsc inner join
                    LOCN_HDR lh on lh.putwy_zone = wsc.code_id  inner join RESV_LOCN_HDR
                    on RESV_LOCN_HDR.LOCN_ID = lh.LOCN_ID where lh.dsp_locn LIKE '{UIConstants.DisplayLocation}F' and rownum='1' order by lh.dsp_locn asc"; }
        public static string FetchDrillDownHeaderDtSql() { return $@"select lh.locn_class ""Location Category"",DECODE(sku_dedctn_type,'T','Temporary','P','Permanent',sku_dedctn_type) ""Item Dedication"",
                     lh.work_grp || '/' || lh.work_area ""Work Group/ Area"" from locn_hdr lh  where lh.dsp_locn = '{UIConstants.DisplayLocation}F' ORDER BY dbms_random.value"; }
        public static string FetchLocationGroupHeaderDtSql() { return $@"select dsp_locn ""Location"" from locn_hdr where dsp_locn='{UIConstants.DisplayLocation}' ORDER BY dbms_random.value"; }
        public static string FetchLocationGrpDtSql() { return $@"select get_sc_desc('S','740',lg.GRP_TYPE,NULL) ""GRP_TYPE"",lg.GRP_ATTR from locn_grp lg
                    inner join locn_hdr lh on lh.locn_id=lg.locn_id where lh.dsp_locn='{UIConstants.DisplayLocation}' ORDER BY dbms_random.value"; }
        public static string FetchReserveDrilldowntabDtSql() { return $@"SELECT pz.pull_locn_desc ""Pull Zone"",lst.LOCN_desc ""Location Size Type"",get_sc_desc('B','354',rlh.locn_putaway_lock,NULL) ""Putaway Lock"",
                    get_sc_desc('B','527',rlh.invn_lock_code,NULL) ""Inventory Lock"",lh.zone ""Zone"",lh.aisle ""Aisle"",lh.bay ""Slot"",
                    lh.lvl ""Level"",lh.time_to_exit_point ""Time to Exit Point(sec)"",lh.exit_point ""Exit Point"",lh.x_coord ""X"",lh.y_coord ""Y"",
                    lh.z_coord ""Z"",lh.len ""Length"",lh.width ""Width"",lh.ht ""Height"",lh.locn_brcd ""Barcode"",lh.locn_pick_seq ""Location Pick Seq"",
                    lh.cycle_cnt_rsn_code ""Cycle Count Reason"",lh.last_frozn_date_time ""Last Frozen Date"",lh.last_cnt_date_time ""Last Count Date"",
                    wsc.code_desc as ""Putaway Zone"" FROM WHSE_SYS_CODE wsc inner join LOCN_HDR lh on lh.putwy_zone = wsc.code_id
                    inner join RESV_LOCN_HDR rlh on rlh.locn_id = lh.locn_id inner join pull_zone pz on pz.pull_zone=rlh.pull_Zone
                    inner join locn_size_type lst on lst.locn_size_type=rlh.locn_size_type WHERE lh.locn_class = 'R' AND wsc.code_type  = '599' AND wsc.rec_type  = 'B'
                    and  dsp_locn='{UIConstants.DisplayLocation}' ORDER BY dbms_random.value"; }
    }
}
