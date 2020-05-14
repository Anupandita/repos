using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace FunctionalTestProject.SQLQueries
{
    public static class ActiveLocationQueries
    {
        public const string FetchItemNumberInActivelocationSql = "select distinct sku_id from locn_hdr lh inner join pick_locn_dtl pd on lh.locn_id=pd.locn_id where lh.locn_class='A'";
        public const string FetchLocn = @"select distinct lh.zone,lh.aisle,lh.bay,lh.lvl
                    from locn_hdr lh inner join whse_sys_code ws on ws.whse=lh.whse
                    where lh.locn_class='A' and SUBSTR(ws.misc_flags,1,1)!='N' and rownum='1'";
        public const string FetchLocns = @"select distinct lh.zone,lh.aisle,lh.bay,lh.lvl
                    from locn_hdr lh inner join whse_sys_code ws on ws.whse=lh.whse
                    where lh.locn_class='A' and SUBSTR(ws.misc_flags,1,1)='N' and rownum='1'";
        public const string FetchGrp = @"select distinct get_sc_desc('S','740',lg.GRP_TYPE,NULL) ""GRP_TYPE"",grp_attr
                from locn_grp lg inner join locn_hdr lh on lh.locn_id = lg.locn_id where locn_class = 'A' and rownum = '1'";
        public static string FetchMainPageActiveLoctnGridDtSql()
        {
            return $"SELECT lh.dsp_locn,pld.locn_seq_nbr,pld.sku_id,pld.actl_invn_qty,pld.max_invn_qty,pld.min_invn_qty,pld.invn_type,pld.prod_stat,pld.sku_attr_1 || ' ' || pld.sku_attr_2 || ' ' || pld.sku_attr_3 || ' '	|| pld.sku_attr_4 || ' ' || pld.sku_attr_5 AS id_codes,pld.actl_invn_cases,pld.min_invn_cases,pld.max_invn_cases,pld.pikng_lock_code FROM LOCN_HDR lh inner join PICK_LOCN_DTL pld on pld.locn_id = lh.locn_id WHERE lh.locn_class = 'A' AND lh.zone='{UIConstants.Zone}'AND lh.aisle='{UIConstants.Aisle}' AND lh.bay='{UIConstants.Slot}' AND lh.lvl = '{UIConstants.Level}'";
        }
        public static string FetchDrillDownHeaderDtSql()
        {
            return $@"select lh.locn_class ""Location Category"",sku_dedctn_type ""Item Dedication"",wam.work_grp||'/'||wam.work_area ""Work Group/Area"",
                    plh.max_nbr_of_sku ""Max Nbr of Item""
                    from locn_hdr lh inner join pick_locn_hdr plh on plh.locn_id=lh.locn_id inner join work_area_master wam on wam.work_grp=lh.work_grp and
                    wam.work_area=lh.work_area where lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
        public static string FetchItemHeaderDtSql()
        {
            return $@"select locn_seq_nbr ""Location Seq Nbr"",get_sc_desc('B','353',pikng_lock_code,NULL) ""Picking Lock Code""
                        from pick_locn_dtl pld inner join locn_hdr lh on lh.locn_id=pld.Locn_id  where lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
        public static string FetchLocationGroupHeaderDtSql()
        {
            return $@"select lh.dsp_locn ""Location"" from locn_hdr lh where lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
        public static string FetchLocationGrpDtSql()
        {
            return $@"select get_sc_desc('S','740',lg.GRP_TYPE,NULL) ""GRP_TYPE"",lg.GRP_ATTR
                    from locn_grp lg inner join locn_hdr lh on lh.locn_id=lg.locn_id where lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
        public static string FetchLpnGridDtSql()
        {
            return $@"select cd.case_nbr,cd.sku_id,cd.ACTL_QTY from case_dtl cd inner join case_hdr ch on ch.case_nbr=cd.case_nbr
                      inner join locn_hdr lh on lh.locn_id=ch.locn_id  where lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
        public static string FetchAdjInvGridDtSql()
        {
            return $@"select pld.locn_seq_nbr,pld.sku_id,pld.actl_invn_qty from pick_locn_dtl pld inner join locn_hdr lh on lh.locn_id=pld.locn_id
                      where lh.dsp_locn='{UIConstants.AdjacentLocation}'";
        }
        public static string FetchActiveLocnDrillDownDtSql()
        {
            return $@"SELECT lh.zone ""Zone"",lh.aisle ""Aisle"",lh.bay ""Slot"",lh.lvl ""Level"",lh.x_coord ""X"",lh.y_coord ""Y"",lh.z_coord ""Z"",
                    lh.time_to_exit_point ""Time toExit Point(sec)"",lh.locn_pick_seq ""Locn Pick Seq"",lh.len ""Length"",lh.width ""Width"",lh.ht ""Height""
                    ,lh.last_frozn_date_time ""Last Frozen Date"",lh.last_cnt_date_time ""Last Count Date"",
                    lh.exit_point ""Exit Point"", lh.locn_brcd ""Barcode"",plh.repl_locn_brcd ""Replenish Barcode"",plh.putwy_type ""Putaway Type"",
                    plh.pick_detrm_zone ""Pick Determination Zone"",plh.pick_locn_assign_zone ""Pick Assign Zone"" FROM LOCN_HDR lh
                    inner join PICK_LOCN_HDR plh on plh.locn_id = lh.locn_id WHERE lh.locn_class = 'A' AND lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
        public static string FetchActiveLocnItemDtSql()
        {
            return $@"SELECT pl.sku_id ""Item"",pl.SKU_ATTR_1 || ' ' || pl.SKU_ATTR_2 || ' ' || pl.SKU_ATTR_3 || ' ' || pl.SKU_ATTR_4 || ' ' || pl.SKU_ATTR_5
                    AS ""ID Codes"",pl.invn_type ""Inventory Type"",pl.prod_stat ""Product Status"",pl.batch_nbr ""Batch"",pl.actl_invn_qty ""Actual"",pl.max_invn_qty ""Maximum"",
                    pl.min_invn_qty ""Minimum"",pl.to_be_pikd_qty ""To Be Picked"",pl.to_be_filld_qty ""To Be Filled"",pl.actl_invn_cases ""Actual"",pl.min_invn_cases ""Minimum"",
                    pl.max_invn_cases ""Maximum"",pl.first_wave_nbr ""First Wave"",pl.last_wave_nbr ""Last Wave"",pl.cntry_of_orgn ""Country of  Origin"",
                    pl.ltst_pick_assign_date_time ""Latest Pick Assign Date"",pl.to_be_filld_cases ""To Be Filled LPNs"" FROM PICK_LOCN_DTL
                    pl inner join locn_hdr lh on  pl.locn_id = lh.locn_id where lh.dsp_locn='{UIConstants.DisplayLocation}'";
        }
    }
}
