using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
   
    public class CommonQueries
    {

        public static string WmsToEms = $"select * from WMSTOEMS where TRX = :transCode and MSGKEY = :msgKey";
        public static string TransInventory = $"Select * from trans_invn where sku_id = :skuId and  trans_invn_type = :transInventoryType";
        public static string ItemMaster = $"select unit_wt from item_master where sku_id = :skuId";
        public static string TempZone = $"select TEMP_ZONE  from item_master where sku_id= :skuId";
        public static string SwmFromMhe = $"select * from swm_from_mhe where Source_MSg_Key = :messageKey and source_msg_trans_code = :transCode order by created_date_time desc";
        public static string SeqNbrEmsToWms = $"select EMSTOWMS_MSGKEY_SEQ.nextval from dual";
        public static string PickLocnDtl = $"select * from pick_locn_dtl where sku_id = :skuId and locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId ) order by mod_date_time desc";
        public static string PickLocnDtlExt = $"select Active_Ormt_Count from pick_locn_dtl_ext WHERE  SKU_ID= :skuId and locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId ) order by updated_date_time desc,created_date_time asc";
        public static string CartonHdr = $"Select * from carton_hdr where carton_nbr = :cartonNumber";
        public static string TaskHdr = $"select * from task_hdr where sku_id = :skuId order by mod_date_time desc";
        public static string EmsToWms = $"select * from emstowms where msgKey = :msgKey";
    }
}
