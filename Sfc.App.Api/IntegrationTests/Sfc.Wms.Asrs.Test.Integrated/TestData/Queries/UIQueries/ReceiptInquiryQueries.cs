﻿using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace FunctionalTestProject.SQLQueries
{
    public static class ReceiptInquiryQueries
    {
        public const string FetchPoNbrSql = "select distinct ad.po_nbr from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join asn_lot_trkg_tbl al on ad.shpmt_nbr=al.shpmt_nbr ORDER BY dbms_random.value";
        public const string FetchAsnNbrSql = "Select * from(select distinct ah.SHPMT_NBR,count(*) from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join asn_lot_trkg_tbl al on ad.shpmt_nbr=al.shpmt_nbr " +
            "group by ah.shpmt_nbr ORDER BY dbms_random.value) where rownum=1";
        public const string FetchVendorName = "Select * from(select distinct vendor_name,count(*) from asn_dtl ad inner join vendor_master vm on vm.vendor_id=ad.vendor_id group by vendor_name  ORDER BY dbms_random.value) where rownum=1";
        public static string FetchVerfDateSql = "Select * from(select to_char(verf_date_time-10,'MM/dd/yyyy'),to_char(verf_date_time+10,'MM/dd/yyyy') from asn_hdr where verf_date_time is not null ORDER BY dbms_random.value) where rownum=1";

        public const string FetchUnAnsQvShipmentNbrSql = "select distinct ad.shpmt_nbr,ques_ans_id from ques_master qm inner join ques_ans qa on qm.ques_id=qa.ques_id inner join asn_dtl ad " +
                    "on ad.shpmt_nbr=qa.shpmt_nbr inner join asn_hdr ah on ah.shpmt_nbr=ad.shpmt_nbr where ans_mandt_flag='Y' and ans_text is null ORDER BY dbms_random.value";
        public const string FetchQvShipmentNbrSql = "select distinct shpmt_nbr from ques_master qm inner join ques_ans qa on qm.ques_id=qa.ques_id inner join asn_dtl ad" +
                    " on ad.shpmt_nbr=qa.shpmt_nbr  where ans_text is null ORDER BY dbms_random.value";

        public static string GetReceivingSearchByStatusCount()
        {
            return $@"SELECT count(*) from asn_hdr where stat_code >={UIConstants.FromStatus} and stat_code <= {UIConstants.ToStatus}";
        }

        public static string GetReceivingSearchByVerifiedDateCount()
        {
            return $@"SELECT count(*) from asn_hdr where verf_date_time >={UIConstants.VerifiedFrom} and verf_date_time <= {UIConstants.VerifiedTo}";
        }
        public static string FetchQvGridDataSql()
        {
            if (UIConstants.QvPoNbr != null)
                return $@"SELECT qa.QUES_ANS_ID,qa.ANS_TEXT,DECODE(qa.PROMPT_LVL,'I','Item','C','Case','S','Shipment',qa.PROMPT_LVL) PROMPT_LEVEL,qa.SHPMT_NBR,qa.CASE_NBR,qa.SKU_ID,
                           qa.SEQ_NBR,DECODE(qa.STAT_CODE,'0','Pending','90','Completed','91','Skipped',qa.STAT_CODE) STATUS_CODE,to_char(qa.CREATE_DATE_TIME,'{UIConstants.DateTimeFormat}') CREATE_DATE_TIME,to_char(qa.MOD_DATE_TIME,'{UIConstants.DateTimeFormat}') MOD_DATE_TIME,qa.USER_ID, qm.ques_txt,
                            wm.whse_name from QUES_ANS qa inner join QUES_MASTER qm on qm.ques_id = qa.ques_id inner join WHSE_MASTER wm
                           on wm.whse = qa.whse inner join asn_dtl ad on ad.shpmt_nbr = qa.shpmt_nbr where ad.po_nbr = '{UIConstants.QvShipmentNbr}'";
            else return "";
        }
        public static string VerifyAnswerUpdatedSql()
        {
            if (UIConstants.QvPoNbr != null)
                return $@"SELECT qa.ANS_TEXT from QUES_ANS qa where qa.shpmt_nbr='{UIConstants.UnAnsQvShipmentNbr}' and qa.ques_ans_id = '{UIConstants.QuesAnswerId}'";
            else return "";
        }
        public static string FetchMainPageGridDtSql()
        {
                return $@"select asn_hdr.shpmt_nbr ShipmentNumber,wm.whse_name ,asn_hdr.MANIF_NBR,
                        asn_hdr.MANIF_TYPE,asn_hdr.BOL,asn_dtl.po_nbr PoNumber, 
                        asn_hdr.SHIP_VIA,asn_hdr.TRLR_NBR,asn_hdr.WORK_ORD_NBR,asn_hdr.CUT_NBR,asn_hdr.MFG_PLNT,
                        DECODE(asn_dtl_type,'S','Item level','C','Case Level',asn_dtl_type) ASN_DTL_TYPE,asn_hdr.ASN_ORGN_TYPE,
                        asn_hdr.RECV_AGAINST_ASN,asn_hdr.BUYER_CODE,asn_hdr.REP_NAME,asn_hdr.PRO_NBR,
                        asn_hdr.DC_ORD_NBR,asn_hdr.CONTRAC_LOCN,asn_hdr.WHSE_XFER_FLAG,
                        asn_dtl.QUAL_CHK_HOLD_UPON_RCPT,asn_dtl.QUAL_AUDIT_PCNT,asn_hdr.CASES_SHPD,asn_hdr.UNITS_SHPD,
                        asn_hdr.CASES_RCVD,asn_hdr.UNITS_RCVD,
                        to_char(SHPD_DATE_TIME,'{UIConstants.DateTimeFormat}') ,
                        to_char(asn_hdr.ARRIVAL_DATE_TIME,'{UIConstants.DateTimeFormat}'),
                        to_char(asn_hdr.FIRST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}') ,
                        to_char(asn_hdr.LAST_RCPT_DATE_TIME,'{UIConstants.DateTimeFormat}'),
                        to_char(asn_hdr.VERF_DATE_TIME,'{UIConstants.DateTimeFormat}'),
                        ltrim(to_char(asn_hdr.VOL,'{UIConstants.VolumeDecimalFormat}')) ,
                        ltrim(to_char(asn_hdr.TOTAL_WT,'{UIConstants.DecimalFormat}')) ,
                        asn_hdr.AUDIT_STAT_CODE,asn_hdr.REF_CODE_1,asn_hdr.REF_FIELD_1,get_sc_desc ('B','564',asn_hdr.stat_code,NULL) STATUS_DESC,
                        asn_hdr.REF_CODE_2,
                        asn_hdr.REF_FIELD_2,asn_hdr.REF_CODE_3,
                        asn_hdr.REF_FIELD_3,asn_hdr.MISC_INSTR_CODE_1,asn_hdr.MISC_INSTR_CODE_2,asn_hdr.LABEL_PRT,
                        to_char(asn_hdr.create_date_time,'{UIConstants.DateTimeFormat}'),
                        to_char(asn_hdr.mod_date_time,'{UIConstants.DateTimeFormat}'),asn_hdr.user_id
                        ,vendor_master.VENDOR_NAME from asn_hdr inner join asn_dtl on  asn_dtl.shpmt_nbr=asn_hdr.shpmt_nbr inner join 
                        VENDOR_MASTER on vendor_master.vendor_id=asn_dtl.vendor_id inner join asn_lot_trkg_tbl al on asn_dtl.shpmt_nbr=al.shpmt_nbr 
                        inner join WHSE_MASTER wm on wm.whse = asn_hdr.TO_WHSE where asn_dtl.po_nbr='{UIConstants.PoNumber}'";
        }
        public static string FetchDetailsGridDtSql()
        {
            return $@"select asn_dtl.shpmt_nbr,asn_dtl.shpmt_seq_NBR,asn_dtl.SKU_ID,asn_dtl.INVN_TYPE,asn_dtl.PROD_STAT,asn_dtl.BATCH_NBR,
                    asn_dtl.SKU_ATTR_1,asn_dtl.SKU_ATTR_2, asn_dtl.SKU_ATTR_3,asn_dtl.SKU_ATTR_4,asn_dtl.SKU_ATTR_5,asn_dtl.PPACK_GRP_CODE,
                    asn_dtl.ASSORT_NBR,asn_dtl.CASES_SHPD,asn_dtl.CASES_RCVD,asn_dtl.UNITS_RCVD,asn_dtl.UNITS_SHPD,UNITS_PRE_RCV,asn_dtl.PROC_IMMD_NEEDS,
                    asn_dtl.MFG_PLNT,to_char(asn_dtl.MFG_DATE,'{UIConstants.DateTimeFormat}')MFG_DATE,asn_dtl.RCVR_NBR,asn_dtl.QUAL_CHK_HOLD_UPON_RCPT,
                    asn_dtl.QUAL_AUDIT_PCNT,asn_dtl.STOP_RCV_FLAG,asn_dtl.START_SHIP_FLAG,asn_dtl.ACTN_CODE,asn_dtl.CASE_DIVRT_CODE,asn_dtl.PURCH_UOM,
                    asn_dtl.PO_NBR,asn_dtl.PO_LINE_NBR,asn_dtl.CUT_NBR,asn_dtl.VENDOR_ITEM_NBR,asn_dtl.REF_FIELD_1,asn_dtl.REF_FIELD_2,
                    asn_dtl.REF_FIELD_3,asn_dtl.MISC_INSTR_CODE_1,asn_dtl.MISC_INSTR_CODE_2,to_char(asn_dtl.XPIRE_DATE,'{UIConstants.DateTimeFormat}')XPIRE_DATE,
                    to_char(asn_dtl.SHIP_BY_DATE,'{UIConstants.DateTimeFormat}')SHIP_BY_DATE,asn_dtl.TOTAL_CATCH_WT,asn_dtl.NBR_OF_PACK_FOR_CATCH_WT,asn_dtl.CUSTOM_PROC,
                    asn_dtl.CNTRY_OF_ORGN,asn_dtl.STD_SUB_PACK_QTY,asn_dtl.STD_PACK_QTY,asn_dtl.STD_CASE_QTY,asn_dtl.CARTON_PER_TIER,asn_dtl.TIER_PER_PLT,
                    asn_dtl.WT_RCVD,to_char(asn_dtl.CREATE_DATE_TIME,'{UIConstants.DateTimeFormat}')CREATE_DATE_TIME,
                    to_char(asn_dtl.MOD_DATE_TIME,'{UIConstants.DateTimeFormat}')MOD_DATE_TIME,asn_dtl.USER_ID from asn_dtl
                    inner join asn_hdr on asn_hdr.shpmt_nbr=asn_dtl.shpmt_nbr inner join item_master on item_master.sku_id = asn_dtl.sku_id inner join item_whse_master
                    on item_whse_master.whse = asn_hdr.to_whse and asn_dtl.sku_id=item_whse_master.sku_id  where asn_dtl.shpmt_nbr='{UIConstants.ShipmentNbr}'";
        }
        
        public static string FetchDetailsDrilldownGridDataSql()
        {
            return $@"SELECT whse,al.shpmt_nbr,al.sku_id,ad.po_nbr,lot_trkg_data,lot_trkg_type,to_char(al.create_date_time,'{UIConstants.DateTimeFormat}'),to_char(al.mod_date_time,'{UIConstants.DateTimeFormat}'),al.user_id FROM asn_lot_trkg_tbl al
                    inner join asn_dtl ad on ad.shpmt_nbr=al.shpmt_nbr where ad.shpmt_nbr='{UIConstants.ShipmentNbr}' and ad.sku_id='{UIConstants.ItemNumber}'";
        }


    }
}
