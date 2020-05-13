using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace FunctionalTestProject.SQLQueries
{
    public static class ReturnsReceivingSql
    {
        public const string FetchReturnRecvingSkusql = "select distinct ad.sku_id from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr where ah.asn_orgn_type='R' ORDER BY dbms_random.value";
        public const string FetchReturnRecvingRoutesql = "select distinct ph.shpmt_nbr from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr inner join pkt_hdr ph on ad.po_nbr=ph.pkt_ctrl_nbr where ah.asn_orgn_type='R' ORDER BY dbms_random.value";
        public const string FetchReturnRecvingShpmntNbrsql = "select distinct ah.shpmt_nbr from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr where ah.asn_orgn_type='R' ORDER BY dbms_random.value";
        public const string FetchReturnRecvingDatesql = "select distinct ah.create_date_time from asn_hdr ah inner join asn_dtl ad on ah.shpmt_nbr=ad.shpmt_nbr where ah.asn_orgn_type='R' ORDER BY dbms_random.value";
        public static string FetchReturnReceivingDtSql()
        {
            return "select  ASN_HDR.Create_date_time asn_created,ASN_HDR.SHPMT_NBR ASN," +
                        "ASN_DTL.PO_NBR PO,ASN_DTL.PO_LINE_NBR PO_Line, ASN_DTL.SKU_ID Item," +
                        "ITEM_MASTER.SKU_DESC Description,(ASN_DTL.UNITS_SHPD - ASN_DTL.UNITS_RCVD) VARIANCE," +
                        "SUBSTR(CASE_HDR.CASE_NBR, 9, 12) LPN,get_sc_desc('B', '509', CASE_HDR.STAT_CODE, NULL) LPN_DESC," +
                        "PKT_HDR.SHPMT_NBR FOR_ROUTE,asn_hdr.REF_FIELD_3,get_sc_desc('B', '564', ASN_HDR.STAT_CODE, NULL) ASN_DESC " +
                        "FROM ASN_HDR inner join ASN_DTL on ASN_HDR.SHPMT_NBR = ASN_DTL.SHPMT_NBR inner join CASE_HDR " +
                        "on CASE_HDR.ORIG_SHPMT_NBR = ASN_HDR.SHPMT_NBR inner join CASE_DTL " +
                        "on ASN_DTL.SKU_ID = CASE_DTL.SKU_ID AND CASE_HDR.DC_ORD_NBR = ASN_DTL.PO_LINE_NBR " +
                        "AND CASE_DTL.CASE_NBR = CASE_HDR.CASE_NBR inner join PKT_HDR " +
                        "on ASN_DTL.PO_NBR = PKT_HDR.PKT_CTRL_NBR inner join ITEM_MASTER " +
                        "on ITEM_MASTER.SKU_ID = ASN_DTL.SKU_ID WHERE ASN_HDR.ASN_ORGN_TYPE = 'R' " +
                        $"AND ITEM_MASTER.SKU_ID = '{UIConstants.ItemNumber}'";
        }
    }
}
