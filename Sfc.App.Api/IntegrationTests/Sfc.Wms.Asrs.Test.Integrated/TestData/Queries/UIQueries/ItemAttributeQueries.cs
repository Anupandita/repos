using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    public static class ItemAttributeQueries
    {
        public const string FetchItemNumberInItemAttributeSql = "SELECT * FROM(select distinct im.sku_id from item_master im inner join asn_dtl ad on ad.sku_id= im.sku_id inner join PICK_LOCN_DTL pld  on pld.sku_id=im.sku_id where ad.vendor_item_nbr is not null ORDER BY dbms_random.value) where rownum=1";
        public const string FetchItemDescriptionSql = "SELECT * FROM(select distinct sku_desc from item_master group by sku_desc ORDER BY dbms_random.value) where rownum=1";
        public const string FetchVendorItemNumberSql = "SELECT * FROM(select distinct vendor_item_nbr,count(*) from item_master group by vendor_item_nbr ORDER BY dbms_random.value) where rownum=1";
        public const string FetchTempZoneSql = "SELECT * FROM(select distinct itma.temp_zone,count(*) from item_master itma inner join ITEM_WHSE_MASTER iwm  on itma.sku_id = iwm.sku_id where temp_zone is not null group by itma.temp_zone ORDER BY dbms_random.value) where rownum=1";
        public static string FetchItemPageGridDtSql()
        {
            return $@"SELECT itma.sku_id  item, itma.sku_desc description, get_sc_desc ('B','722',itma.stat_code,NULL) status,
                     get_sc_desc ('B','332',itma.temp_zone,NULL) standardTempZone,itma.spl_instr_code_5 childParent,iwm.case_size_type lpnSize,
                     ltrim(to_char(itma.unit_price,'{UIConstants.DecimalFormat}')) price,itma.sku_brcd barcode,iwm.carton_per_tier ti,iwm.tier_per_plt hi,
                    itma.purch_uom PurchaseUintOfMeasure,DECODE(itma.catch_wt,'2','Y',itma.catch_wt) catchWeight,ltrim(to_char(itma.nest_vol,'{UIConstants.HeightFormat}')) || '/' || rtrim(itma.dflt_cons_date) || ' ' ||itma.purch_uom AS packSize,
                     itma.PROD_LIFE_IN_DAY shelfLife,itma.MAX_RECV_TO_XPIRE_DAYS requiredShelfLife,iwm.VIOLATE_FIFO_ALLOC_QTY_MATCH violateFifoFullPalletPull,
                     itma.STD_UOM allowFullPalletPull,itma.PKG_TYPE replenishPartialLpnQuantity,itma.SPL_INSTR_CODE_4 crossDock,itma.SPL_INSTR_CODE_8 asrs,itma.PROD_TYPE conveyable,
                     itma.SPL_INSTR_CODE_3 totable,itma.SPL_INSTR_CODE_2 jit,get_sc_desc ('B','322',itma.LOAD_ATTR,NULL) loadType,itma.SPL_INSTR_CODE_7 specialOrder,itma.VOLTY_CODE velocityCode,
                     itma.PROD_GROUP stretchWrap ,cons_prty_date_code dataType,cons_prty_date_window dateWindow,cons_prty_date_window_incr dateWindowIncrement,
                    allow_rcpt_older_sku allowOlderSku,xpire_date_reqd promptExpiryDate,mfg_date_reqd promptManufacturingDate,ship_by_date_reqd promptShipmentByDate,
                    pick_wt_tol_amnt pickWeightTolerance,pick_wt_tol_type pickWeightToleranceType,mhe_wt_tol_amnt mheWeightTolerance,mhe_wt_tol_type mheWeightToleranceType,
                    get_sc_desc('B', '669', itma.PROD_LINE, NULL) pickLocationType,get_sc_desc('B', '667', iwm.PUTWY_TYPE, NULL) putWayType,
                    get_sc_desc('B', '325', iwm.alloc_type, NULL) allocationType,get_sc_desc('C', '144', itma.SPL_INSTR_CODE_10, NULL) climateZone,
                    get_sc_desc('B', '332', itma.trlr_temp_zone, NULL) loadTempZone,SPL_INSTR_CODE_6 iceCream,avg_dly_dmnd averageDailyDemand,volty_code velocityCode,
                     ltrim(to_char(itma.std_case_qty,'{UIConstants.HeightFormat}')) lpnQuantity,ltrim(to_char(itma.unit_vol,'{UIConstants.VolumeDecimalFormat}')) volume,
                     ltrim(to_char(itma.critcl_dim_3,'{UIConstants.DecimalFormat}')) height,ltrim(to_char(itma.critcl_dim_1,'{UIConstants.DecimalFormat}')) length,
                     ltrim(to_char(itma.critcl_dim_2,'{UIConstants.DecimalFormat}')) width,ltrim(to_char(itma.unit_wt,'{UIConstants.HeightFormat}')) weight
                    FROM ITEM_MASTER itma inner join ITEM_WHSE_MASTER iwm  on itma.sku_id = iwm.sku_id WHERE itma.sku_id='{UIConstants.ItemNumber}'";
        }
        public static string FetchVendorDtSql()
        {
            return $@"SELECT VENDOR_MASTER.VENDOR_NAME || ' (' || ASN_DTL.VENDOR_ITEM_NBR || ')' FROM ASN_DTL inner join
                    VENDOR_MASTER on VENDOR_MASTER.VENDOR_ID = ASN_DTL.VENDOR_ID WHERE ASN_DTL.SKU_ID = '{UIConstants.ItemNumber}'
                    ORDER BY VENDOR_MASTER.VENDOR_NAME";
        }
        public static string FetchActiveLocnDtSql()
        {
            return $@"SELECT LOCN_HDR.LOCN_BRCD FROM LOCN_HDR, PICK_LOCN_DTL WHERE LOCN_HDR.LOCN_ID = PICK_LOCN_DTL.LOCN_ID AND PICK_LOCN_DTL.SKU_ID = '{UIConstants.ItemNumber}'";
        }
    }
}
