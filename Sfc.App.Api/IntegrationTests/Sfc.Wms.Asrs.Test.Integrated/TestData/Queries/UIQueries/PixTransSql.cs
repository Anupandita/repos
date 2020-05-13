using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
namespace FunctionalTestProject.SQLQueries
{
    public static class PixTransSql
    {
        public const string FetchPixSkusql = "select distinct pt.sku_id from pix_tran pt ORDER BY dbms_random.value";
        public const string FetchPixDatesql = "select distinct pt.date_proc from pix_tran pt ORDER BY dbms_random.value";
        public static string FetchPixTransDtsql()
        {
            return $@"SELECT im.sku_id AS ""ITEM"",im.sku_desc AS ""ITEM_DESC"",pt.date_proc AS ""DATE_PROC"",
                    pt.tran_type || pt.tran_code || pt.actn_code || ' ' || sc1.code_desc AS ""PKMS_TRANSACTION"",pt.custom_ref AS ""ADAGE_TRANSLATION"",
                    'RSN (' || pt.rsn_code || ')' AS ""RSN_CODE"",DECODE(pt.invn_adjmt_type, 'S', pt.invn_adjmt_qty * (-1), pt.invn_adjmt_qty) AS ""QTY"",
                    DECODE(pt.wt_adjmt_type, 'S', pt.wt_adjmt_qty * (-1), pt.wt_adjmt_qty) AS ""WT"",pt.units_rcvd AS ""RECV"",pt.units_shpd AS ""SHPD"",
                    pt.case_nbr AS ""LPN"",pt.user_id,nvl(um.user_name, pt.user_id) AS ""NAME"" FROM USER_MASTER, PIX_TRAN, ITEM_MASTER, SYS_CODE WHERE pt.user_id = um.login_user_id(+)AND sc1.rec_type = 'B' AND sc1.code_type = '740'
                    AND sc1.code_id = pt.tran_type || pt.tran_code || pt.actn_code AND pt.tran_type || pt.tran_code <> '61501' AND im.sku_id = pt.sku_id
                    and pt.sku_id = '{UIConstants.ItemNumber}'";
        }
    }
}
