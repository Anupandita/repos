using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace FunctionalTestProject.SQLQueries
{
    public static class ReceiptInquiryQueries
    {
        public const string FetchPoNbrSql = "select distinct ad.po_nbr from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join asn_lot_trkg_tbl al on ad.shpmt_nbr=al.shpmt_nbr ORDER BY dbms_random.value";
        public const string FetchAsnNbrSql = "Select * from(select distinct ah.SHPMT_NBR,count(*) from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join asn_lot_trkg_tbl al on ad.shpmt_nbr=al.shpmt_nbr " +
            "group by ah.shpmt_nbr ORDER BY dbms_random.value) where rownum=1";
        public const string FetchUnAnsQvPoNbrSql = "select distinct ad.po_nbr from ques_master qm inner join ques_ans qa on qm.ques_id=qa.ques_id inner join asn_dtl ad " +
                    "on ad.shpmt_nbr=qa.shpmt_nbr inner join asn_hdr ah on ah.shpmt_nbr=ad.shpmt_nbr where ans_mandt_flag='Y' and ans_text is null ORDER BY dbms_random.value";
        public const string FetchQvPoNbrSql = "select distinct po_nbr from ques_master qm inner join ques_ans qa on qm.ques_id=qa.ques_id inner join asn_dtl ad" +
                    " on ad.shpmt_nbr=qa.shpmt_nbr  where ans_text is null ORDER BY dbms_random.value";
        public static string FetchVerfDateSql = "select verf_date_time from asn_hdr where verf_date_time is not null ORDER BY dbms_random.value";
        public static string FetchQvGridDataSql()
        {
            if (UIConstants.QvPoNbr != null)
                return $@"SELECT qa.QUES_ANS_ID,qa.ANS_TEXT,DECODE(qa.PROMPT_LVL,'I','Item','C','Case','S','Shipment',qa.PROMPT_LVL) PROMPT_LEVEL,qa.SHPMT_NBR,qa.CASE_NBR,qa.SKU_ID,
                           qa.SEQ_NBR,DECODE(qa.STAT_CODE,'0','Pending','90','Completed','91','Skipped',qa.STAT_CODE) STATUS_CODE,to_char(qa.CREATE_DATE_TIME,'{UIConstants.DateTimeFormat}') CREATE_DATE_TIME,to_char(qa.MOD_DATE_TIME,'{UIConstants.DateTimeFormat}') MOD_DATE_TIME,qa.USER_ID, qm.ques_txt,
                            wm.whse_name from QUES_ANS qa inner join QUES_MASTER qm on qm.ques_id = qa.ques_id inner join WHSE_MASTER wm
                           on wm.whse = qa.whse inner join asn_dtl ad on ad.shpmt_nbr = qa.shpmt_nbr where ad.po_nbr = '{UIConstants.QvPoNbr}'";
            else return "";
        }
        public static string FetchDrillHeaderDtSql()
        {
            return $@"select asn_dtl.shpmt_nbr""Receipt Number"",DECODE(asn_dtl_type,'S','Item level','C','Case Level',asn_dtl_type) ""Detail Type"",get_sc_desc ('B','564',asn_hdr.stat_code,NULL) ""Status"" FROM
                    asn_hdr INNER JOIN asn_dtl ON asn_dtl.shpmt_nbr = asn_hdr.shpmt_nbr where asn_dtl.po_nbr = '{UIConstants.PoNumber}'";
        }
        public static string FetchDtSql()
        {
            return $@"Select asn_dtl.SHPMT_NBR ""ASN Nbr"",asn_dtl.PO_NBR ""PO Nbr"",get_sc_desc ('B','564',asn_hdr.stat_code,NULL) ""Status"",DECODE(asn_dtl_type,'S','Item level','C','Case Level',asn_dtl_type) ""Detail Type"",
                    DECODE(RECV_AGAINST_ASN,'I','Item','C','Lpn',RECV_AGAINST_ASN) ""Receive Against"",wm.whse_name ""To Whse"",to_char(SHPD_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Shipped on"",ltrim(to_char(VOL,'{UIConstants.VolumeDecimalFormat}')) ""Total Volume"",to_char(FIRST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Started Receiving on"",
                    ltrim(to_char(TOTAL_WT,'{UIConstants.DecimalFormat}')) ""Total Weight"",to_char(LAST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Finished Receiving on"" from asn_hdr INNER JOIN asn_dtl
                    ON asn_dtl.shpmt_nbr = asn_hdr.shpmt_nbr inner join WHSE_MASTER wm on wm.whse = asn_hdr.to_whse where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchDetailsGridDtSql()
        {
            return $@"select asn_dtl.PO_NBR,asn_dtl.SKU_ID,Item_master.sku_desc SKU_DESC,asn_dtl.UNITS_SHPD,UNITS_PRE_RCV,asn_dtl.UNITS_RCVD,
                    asn_dtl.units_rcvd-asn_dtl.units_shpd ""VARIANCEQTY"",to_char(asn_dtl.XPIRE_DATE,'{UIConstants.DateTimeFormat}')XPIRE_DATE,asn_dtl.WT_RCVD,asn_dtl.VENDOR_ITEM_NBR,asn_dtl.VENDOR_ID,asn_dtl.USER_ID,asn_dtl.TOTAL_CATCH_WT,asn_dtl.TIER_PER_PLT,asn_dtl.STOP_RCV_FLAG,
                    asn_dtl.STD_SUB_PACK_QTY,asn_dtl.STD_PACK_QTY,asn_dtl.STD_CASE_QTY,asn_dtl.START_SHIP_FLAG,asn_dtl.SKU_ATTR_5,asn_dtl.SKU_ATTR_4,asn_dtl.SKU_ATTR_3,asn_dtl.SKU_ATTR_2,asn_dtl.SKU_ATTR_1,to_char(asn_dtl.SHIP_BY_DATE,'{UIConstants.DateTimeFormat}')SHIP_BY_DATE,asn_dtl.REF_FIELD_3,asn_dtl.REF_FIELD_2,asn_dtl.REF_FIELD_1,asn_dtl.RCVR_NBR,asn_dtl.QUAL_CHK_HOLD_UPON_RCPT,
                    asn_dtl.QUAL_AUDIT_PCNT,asn_dtl.PURCH_UOM,asn_dtl.PROD_STAT,asn_dtl.PROC_IMMD_NEEDS,asn_dtl.PPACK_GRP_CODE,asn_dtl.PO_LINE_NBR,asn_dtl.NBR_OF_PACK_FOR_CATCH_WT,to_char(asn_dtl.MOD_DATE_TIME,'{UIConstants.DateTimeFormat}')MOD_DATE_TIME,asn_dtl.MISC_INSTR_CODE_2,asn_dtl.MISC_INSTR_CODE_1,asn_dtl.MFG_PLNT,to_char(asn_dtl.MFG_DATE,'{UIConstants.DateTimeFormat}')MFG_DATE,asn_dtl.INVN_TYPE,asn_dtl.CUT_NBR,asn_dtl.CUSTOM_PROC,to_char(asn_dtl.CREATE_DATE_TIME,'{UIConstants.DateTimeFormat}')CREATE_DATE_TIME,
                    asn_dtl.CNTRY_OF_ORGN,asn_dtl.CASES_SHPD,asn_dtl.CASES_RCVD,asn_dtl.CASE_DIVRT_CODE,asn_dtl.CARTON_PER_TIER,asn_dtl.BATCH_NBR,asn_dtl.ASSORT_NBR,asn_dtl.ACTN_CODE,asn_dtl.SHPMT_SEQ_NBR,asn_dtl.SHPMT_NBR,Item_whse_master.qv_item_grp QV_ITEM_GRP,DECODE(lot_trkg_pkg.is_lot_data_for_sku(asn_hdr.to_whse, '', asn_dtl.shpmt_nbr, asn_dtl.sku_id), 1, 'Y', 'N') LOT_TRKG_FLAG from asn_dtl
                    inner join asn_hdr on asn_hdr.shpmt_nbr=asn_dtl.shpmt_nbr inner join item_master on item_master.sku_id = asn_dtl.sku_id inner join item_whse_master
                    on item_whse_master.whse = asn_hdr.to_whse and asn_dtl.sku_id=item_whse_master.sku_id  where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchMainPageGridDtSql()
        {
            return $@"select asn_dtl.po_nbr ASN_DTL_PO_NBR,DECODE(asn_dtl_type,'S','Item level','C','Case Level',asn_dtl_type) ASN_DTL_TYPE,to_char(SHPD_DATE_TIME,'{UIConstants.DateTimeFormat}') ""SHPD_DATE_TIME"",get_sc_desc ('B','564',asn_hdr.stat_code,NULL) STATUS_DESC,asn_dtl.CASES_SHPD,
                    asn_dtl.CASES_RCVD,asn_dtl.UNITS_SHPD,asn_dtl.UNITS_RCVD,ltrim(to_char(asn_hdr.TOTAL_WT,'{UIConstants.DecimalFormat}')) ""TOTAL_WT"",ltrim(to_char(asn_hdr.VOL,'{UIConstants.VolumeDecimalFormat}')) ""VOL"",asn_hdr.SHIP_VIA,asn_hdr.PRO_NBR,vendor_master.VENDOR_NAME,to_char(asn_hdr.FIRST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ""FIRST_RCPT_DATE_TIME"",to_char(asn_hdr.LAST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ""LAST_RCPT_DATE_TIME"",wm.whse_name ""WHSE_NAME"",asn_hdr.MANIF_NBR,
                   asn_hdr.MANIF_TYPE,asn_hdr.BOL,asn_hdr.TRLR_NBR,asn_hdr.WORK_ORD_NBR,asn_hdr.CUT_NBR,asn_hdr.MFG_PLNT,asn_hdr.ASN_ORGN_TYPE,asn_hdr.RECV_AGAINST_ASN,asn_hdr.BUYER_CODE,asn_hdr.REP_NAME,asn_hdr.DC_ORD_NBR,asn_hdr.CONTRAC_LOCN,asn_hdr.WHSE_XFER_FLAG,asn_dtl.QUAL_CHK_HOLD_UPON_RCPT,asn_dtl.QUAL_AUDIT_PCNT,asn_hdr.CASES_SHPD""CASES_SHPD_1"",to_char(asn_hdr.ARRIVAL_DATE_TIME,'{UIConstants.DateTimeFormat}')""ARRIVAL_DATE_TIME"",to_char(asn_hdr.VERF_DATE_TIME,'{UIConstants.DateTimeFormat}')""VERF_DATE_TIME"",
                   asn_hdr.AUDIT_STAT_CODE,asn_hdr.REF_CODE_1,asn_hdr.REF_FIELD_1,asn_hdr.REF_CODE_2,asn_hdr.REF_FIELD_2,asn_hdr.REF_CODE_3,asn_hdr.REF_FIELD_3,asn_hdr.MISC_INSTR_CODE_1,asn_hdr.MISC_INSTR_CODE_2,to_char(al.create_date_time,'{UIConstants.DateTimeFormat}'),to_char(al.mod_date_time,'{UIConstants.DateTimeFormat}'),al.user_id,al.shpmt_nbr ""SHPMT_NBR"",asn_hdr.LABEL_PRT
                    from asn_hdr inner join asn_dtl on  asn_dtl.shpmt_nbr=asn_hdr.shpmt_nbr inner join VENDOR_MASTER on vendor_master.vendor_id=asn_dtl.vendor_id inner join asn_lot_trkg_tbl al on asn_dtl.shpmt_nbr=al.shpmt_nbr inner join WHSE_MASTER wm on wm.whse = asn_hdr.TO_WHSE where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchDetailDrillTabDataSql()
        {
            return $@" select asn_hdr.MFG_PLNT ""Manufacturing Plant"",to_char(MFG_DATE,'{UIConstants.DateTimeFormat}') ""Manufacturing Date"",CNTRY_OF_ORGN ""Country Of Origin"",to_char(XPIRE_DATE,'{UIConstants.DateTimeFormat}') ""Expire Date"",to_char(SHIP_BY_DATE,'{UIConstants.DateTimeFormat}') ""Ship By Date"" ,
            asn_dtl.QUAL_CHK_HOLD_UPON_RCPT ""Quality Chk Hold Upon Rcpt?"",asn_dtl.QUAL_AUDIT_PCNT ""Quality Audit Percent"",STOP_RCV_FLAG ""Stop Receive Flag"",START_SHIP_FLAG ""Start Ship Flag"",
            ACTN_CODE ""Action Code"",CASE_DIVRT_CODE ""LPN Divert Code"",RCVR_NBR ""Receiver Nbr"",TOTAL_CATCH_WT ""Total Catch Weight"",NBR_OF_PACK_FOR_CATCH_WT ""Nbr of Pack for Catch Wt"",
            PROC_IMMD_NEEDS ""Process Immediate Needs?"",PPACK_GRP_CODE ""Prepack Group Code"",ASSORT_NBR ""Assortment Nbr"",CUSTOM_PROC ""Custom Proc"",asn_dtl.MISC_INSTR_CODE_1 ""Dtl Misc Instr 1"",
            asn_dtl.MISC_INSTR_CODE_2 ""Dtl Misc Instr 2"",asn_dtl.REF_FIELD_1 ""ASN Detail Ref 1"",asn_dtl.REF_FIELD_2 ""ASN Detail Ref 2"",asn_dtl.REF_FIELD_3 ""ASN Detail Ref 3""
            from asn_hdr inner join asn_dtl on  asn_dtl.shpmt_nbr=asn_hdr.shpmt_nbr where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchTabDataSql()
        {
            return $@"select asn_dtl.PO_NBR ""PO Nbr"",asn_dtl.CASES_SHPD ""LPNs"",asn_dtl.UNITS_SHPD ""Units"",ltrim(to_char(TOTAL_WT,'{UIConstants.DecimalFormat}')) ""Total Weight"",
                    ltrim(to_char(VOL,'{UIConstants.VolumeDecimalFormat}')) ""Volume"",to_char(SHPD_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Scheduled to Ship On"",to_char(FIRST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Started Receiving On"",to_char(LAST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Finished Receiving On"",
                    DECODE(RECV_AGAINST_ASN,'I','Item','C','LPN',RECV_AGAINST_ASN) ""Receive Against"" ,MANIF_NBR ""Manifest Nbr"",MANIF_TYPE ""Manifest Type"",TRLR_NBR ""Trailer Nbr"",SHIP_VIA ""Ship Via"",BOL ""Bill of Lading"",
                    PRO_NBR ""Pro Nbr"" ,WORK_ORD_NBR ""Work Order Nbr"",DC_ORD_NBR ""DC Order Nbr"",CONTRAC_LOCN ""Contract Location"",asn_dtl.CUT_NBR ""Cut Nbr"",REP_NAME ""Rep Name"",
                    asn_dtl.MFG_PLNT ""Manf Plant"",BUYER_CODE ""Buyer Code"",WHSE_XFER_FLAG ""Whse Xfer"", DECODE(ASN_ORGN_TYPE,'M','Manufacturer','R','Returns','G' ,'Goods',ASN_ORGN_TYPE) ""Origin"",wm.whse_name ""To Warehouse"",to_char(VERF_DATE_TIME,'{UIConstants.DateTimeFormat}') ""Verified On"" ,
                    asn_hdr.REF_FIELD_1 ""ASN Ref 1"",asn_hdr.REF_FIELD_2 ""ASN Ref 2"",asn_hdr.REF_FIELD_3 ""ASN Ref 3"",asn_hdr.MISC_INSTR_CODE_1 ""ASN Inst 1"",
                    asn_hdr.MISC_INSTR_CODE_2 ""ASN Inst 2"" from asn_hdr inner join asn_dtl on  asn_dtl.shpmt_nbr=asn_hdr.shpmt_nbr inner join WHSE_MASTER wm 
                    on wm.whse = TO_WHSE where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchDetailDrillHeaderDtSql()
        {
            return $@"select VENDOR_NAME ""Vendor Name"",asn_dtl.Po_nbr""PO Nbr"",SKU_ATTR_1||''||SKU_ATTR_2||''||SKU_ATTR_3||''||SKU_ATTR_4||''||SKU_ATTR_5 ""ID Codes"",
                    SKU_ID ""Item"",PO_LINE_NBR ""Line Nbr"",ltrim(to_char(WT_RCVD,'{UIConstants.DecimalFormat}')) ""Weight"",PROD_STAT ""Product Status"",BATCH_NBR ""Batch Nbr"",INVN_TYPE ""Inventory Type"",asn_dtl.CASES_SHPD ""LPNs"",
                    asn_dtl.UNITS_SHPD ""Units"" from asn_hdr inner join asn_dtl on  asn_dtl.shpmt_nbr=asn_hdr.shpmt_nbr inner join VENDOR_MASTER on vendor_master.vendor_id=asn_dtl.vendor_id where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchDetailsDrilldownGridDataSql()
        {
            return $@"SELECT whse,al.shpmt_nbr,al.sku_id,ad.po_nbr,lot_trkg_data,lot_trkg_type,to_char(al.create_date_time,'{UIConstants.DateTimeFormat}'),to_char(al.mod_date_time,'{UIConstants.DateTimeFormat}'),al.user_id FROM asn_lot_trkg_tbl al
                    inner join asn_dtl ad on ad.shpmt_nbr=al.shpmt_nbr where ad.po_nbr='{UIConstants.PoNumber}'";
        }

        public static string FetchGetReceivingCount()
        {
            return $@"SELECT count(*) from asn_hdr where stat_code >={UIConstants.FromStatus} and stat_code <= {UIConstants.ToStatus}";
        }


    }
}
