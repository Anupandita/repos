
namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    
    public class SynrQueries
    {
        public const string NextupCountQuery = "select max(CURR_NBR) from nxt_up_cnt where rec_type_id =:RecTypeID";
        public const string PickLocnCountQuery = "select Count(*) from pick_locn_dtl p join item_master i on  p.sku_id = i.sku_id where  locn_id||(DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id||lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId)";
        public const string PLdSnapShotQuery = "select Count(*) from SWM_SYNR_PLD_SNAPSHOT pld join item_master i on  pld.sku_id = i.sku_id where pld.SYNC_ID = :syncId and locn_id|| (DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id || lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId)";
        public const string ListPldSkuQuery = "select * from pick_locn_dtl p join item_master i on  p.sku_id = i.sku_id where  locn_id||(DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id||lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
        public const string SyndDataQuery = "select * from SWM_SYND_DATA where SYNCHRONIZATION_ID=:SyncId and SKU=:Skuid";
        public const string SyndData = "select Count(*) from SWM_SYND_DATA where SYNCHRONIZATION_ID=:SyncId and status=:Status";


        public const string PldQtyQuery = "select SUM(pld.ACTL_INVN_QTY) from SWM_SYNR_PLD_SNAPSHOT pld join item_master i on  pld.sku_id = i.sku_id where pld.SYNC_ID = :SyncId and locn_id|| (DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id || lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id =:sysCodeId)";
        public const string SyndDataQty = "select SUM(SWM_SYND_DATA.Quantity) from SWM_SYND_DATA where SYNCHRONIZATION_ID=:SyncId and status=Status";

        public const string PldTableQuery = "select * from pick_locn_dtl p join item_master i on  p.sku_id = i.sku_id where p.ACTL_INVN_QTY! = 0 and  locn_id||(DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id||lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";

        public const string SyndQtydefferenceQuery = "select * from SWM_SYND_DATA where SYNCHRONIZATION_ID=:syncId and SKU=:Skuid";

        public const string SyndDataFilterQuery = "select * from SWM_SYND_DATA where status= 90 order by SYNCHRONIZATION_ID desc";
          public const string PldSnapTableQuery = "select * from SWM_SYNR_PLD_SNAPSHOT where sku_id=:Skuid and SYNC_ID=:syncId order by sync_id desc ";

    }
}
