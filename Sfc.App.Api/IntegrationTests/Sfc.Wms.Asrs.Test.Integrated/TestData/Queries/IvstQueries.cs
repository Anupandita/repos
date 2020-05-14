using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class IvstQueries
    {       
        public static string IvstQueryForPickLocnQtyGreaterThanIvstQty = $"select distinct swm_to_mhe.container_id,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,case_dtl.actl_qty,swm_to_mhe.qty from swm_to_mhe " +
            $"inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id " +
            $"inner join pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id " +
            $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr " +
            $"inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr " +
            $"and swm_to_mhe.source_msg_status = :ready " +
            $"and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode and pick_locn_dtl.actl_invn_qty > 2 and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId) " +
            $"order by swm_to_mhe.created_date_time desc";

        public static string IvstQueryForTiNegativePickAlreadyExists = $"select distinct swm_to_mhe.container_id,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,case_dtl.actl_qty,swm_to_mhe.qty from swm_to_mhe inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id inner join  " +
         $"pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id " +
         $"inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr " +
         $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = :ready and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode and pick_locn_dtl.actl_invn_qty > 0 and trans_invn.trans_invn_type = (select code_id from sys_code where rec_type='B' and code_type='052' and code_desc='Negative Pick')" +
         $"and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId) order by swm_to_mhe.created_date_time desc";

        public static string IvstQueryForTiNegativePickNotExists = $"select distinct swm_to_mhe.container_id,case_dtl.actl_qty,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id " +
            $"inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id " +
            $"inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr " +
            $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = :ready and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode and" +
            $" pick_locn_dtl.actl_invn_qty > 0 and trans_invn.trans_invn_type not in (select code_id from sys_code where rec_type='B' and code_type='052' and code_desc=' Negative Pick') and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId) order by swm_to_mhe.created_date_time desc";

        public static string CaseHdrStatCodeLesserThan90AndCaseDtlActlQtyGreaterThanZero = 
            $"select distinct swm_to_mhe.container_id,case_dtl.actl_qty,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe " +
            $"inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id " +
            $"inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id   " +
            $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr " +      
            $"inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr " +
            $"and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code < 90 and case_dtl.actl_qty>0 and  pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = 18)  order by swm_to_mhe.created_date_time desc";
        public static string CaseHdrStatCodeLesserThan90AndCaseDtlActlQtyEqualToZero = $"select distinct swm_to_mhe.container_id,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,case_dtl.actl_qty,swm_to_mhe.qty from swm_to_mhe " +
            $"inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id " +
            $"inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id   " +
            $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr " +
            $"inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr " +
            $"and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96 and case_dtl.actl_qty = 0 " +
            $"and  pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = 18)  order by swm_to_mhe.created_date_time desc";
   
        public static string IvstQueryForPickLocnQtyLesserThanOrEqualToZero =
            $"select distinct swm_to_mhe.container_id, swm_to_mhe.created_date_time, swm_to_mhe.sku_id, pick_locn_dtl.locn_id, case_dtl.actl_qty,swm_to_mhe.qty from swm_to_mhe inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id " +
            $"inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id " +
            $"inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr " +
            $" inner join case_hdr on " +
            $"swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = :ready and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode and pick_locn_dtl.actl_invn_qty <= 0 and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join " +
            $"sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId) order by swm_to_mhe.created_date_time desc";

        public static string IvstQueryForTiNegativePickAlreadyExistsForStatus96 = $"select distinct swm_to_mhe.container_id,case_dtl.actl_qty,trans_invn.trans_invn_type,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id inner join case_dtl on swm_to_mhe.container_id = case_dtl.case_nbr inner join pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = :ready and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode and pick_locn_dtl.actl_invn_qty > 0 and trans_invn.trans_invn_type = (select code_id from sys_code where rec_type='B' and code_type='052' and code_desc='Negative Pick')  and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId)  order by swm_to_mhe.created_date_time desc";

        public static string IvstQueryForTiNegativePickNotExistsForStatus96 = $"select distinct swm_to_mhe.container_id,case_dtl.actl_qty,swm_to_mhe.created_date_time,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr " +
            $"inner join case_dtl on swm_to_mhe.sku_id = case_dtl.sku_id " +
            $"and swm_to_mhe.source_msg_status = :ready and swm_to_mhe.qty!= 0 and case_hdr.stat_code = :statCode and" +           
            $" pick_locn_dtl.actl_invn_qty > 0 and trans_invn.trans_invn_type not in (select code_id from sys_code where rec_type='B' and code_type='052' and code_desc=' Negative Pick') and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :sysCodeId) order by swm_to_mhe.created_date_time desc";

        public static string Pb2CorbaHdrAndDtl = $"select * from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id=pd.id where " +
            $"func_name like '%printCaseLabelPB' and workstation_id = (select TRIM(SUBSTR(MISC_FLAGS,13,15)) from whse_sys_code where code_type = '309' and rec_type = 'C'  and code_id = 'Freezer') order by ph.id desc";
        public static string MaxIdForPb2CorbaHdr = $"select max(Id) from pb2_corba_hdr  where workstation_id = (select TRIM(SUBSTR(MISC_FLAGS,13,15))from whse_sys_code where code_type = '309' and rec_type = 'C' and code_id = 'Freezer')";
        public static string Pb2CorbaHdrDtl = $"select * from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id=pd.id where func_name like '%printCaseLabelPB' and workstation_id = (select TRIM(SUBSTR(MISC_FLAGS,13,15)) from whse_sys_code where code_type = '309' and rec_type = 'C' and code_id = 'Dry') and ph.id = '10865079' order by parm_name asc";

    }
}
