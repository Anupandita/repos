﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ActiveLocationsDrillDownModel : PaginationModel
    {
        public string exit_point { get; set; }
        public string sku_dedctn_type { get; set; }
        public string zone { get; set; }
        public string locn_brcd { get; set; }
        public string locn_pick_seq { get; set; }
        public string time_to_exit_point { get; set; }
        public string len { get; set; }
        public string width { get; set; }
        public string ht { get; set; }
        public string x_coord { get; set; }
        public string y_coord { get; set; }
        public string z_coord { get; set; }
        public string work_area_group { get; set; }
        public string locn_id { get; set; }
        public string pick_determ_zone { get; set; }
        public string putwy_type { get; set; }
        public string locn_assign_zone { get; set; }
        public int rnum { get; set; }
        public int locn_seq_nbr { get; set; }
        public string sku_id { get; set; }
        public string invn_type { get; set; }
        public string prod_stat { get; set; }
        public int actl_invn_qty { get; set; }
        public int max_invn_qty { get; set; }
        public int min_invn_qty { get; set; }
        public string sku_attr_1 { get; set; }
        public string sku_attr_2 { get; set; }
        public string sku_attr_3 { get; set; }
        public string sku_attr_4 { get; set; }
        public string sku_attr_5 { get; set; }
        public string id_codes { get; set; }
        public int max_nbr_of_sku { get; set; }
        public string season { get; set; }
        public string season_yr { get; set; }
        public string style { get; set; }
        public string style_sfx { get; set; }
        public string color { get; set; }
        public string color_sfx { get; set; }
        public int sec_dim { get; set; }
        public string qual { get; set; }
        public string size_desc { get; set; }
        public string dsp_sku { get; set; }
        public string bay { get; set; }
        public string aisle { get; set; }
        public string lvl { get; set; }
        public string posn { get; set; }
        public string cycle_cnt_rsn_code { get; set; }
        public string last_frozn_date_time { get; set; }
        public string last_cnt_date_time { get; set; }
        public string workgrouparea { get; set; }
        public string area_septr { get; set; }
        public string zone_septr { get; set; }
        public string aisle_septr { get; set; }
        public string bay_septr { get; set; }
        public string lvl_septr { get; set; }
        public string posn_septr { get; set; }
        public string locn_class { get; set; }
        public string area { get; set; }
        public string dsp_locn { get; set; }
        public string pikng_lock_code { get; set; }
        public string repl_locn_brcd { get; set; }
        public string repl_flag { get; set; }
        public string pick_detrm_zone { get; set; }
        public string pick_locn_assign_zone { get; set; }
        public string create_date_time { get; set; }
        public string mod_date_time { get; set; }
        public string user_id { get; set; }
        public string actl_invn_cases { get; set; }
        public string sku_desc { get; set; }
        public string ltst_sku_assign { get; set; }
        public string cs_trackcase { get; set; }
        public string cs_uom { get; set; }
        public string selectedrow { get; set; }
        public int min_invn_cases { get; set; }
        public int max_invn_cases { get; set; }
        public double unit_wt { get; set; }

    }
}