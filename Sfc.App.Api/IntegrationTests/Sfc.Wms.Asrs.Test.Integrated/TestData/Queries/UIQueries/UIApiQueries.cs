using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries
{
    public class UIApiQueries
    {
        public const string MsgMasterSql = "select mm.module,mm.msg_id messageId,md.lang_id languageId,md.msg message,md.short_msg shortMessage from msg_master mm inner " +
                              "join msg_dtl md on md.module=mm.module and md.msg_id = mm.msg_id where mm.prt_indic='C'";

        public static string FetchMessageLoggerDtSql = "select module,msg_id,log_date_time,msg,ref_code_1,ref_code_2,ref_code_3," +
           "ref_code_4,ref_code_5,ref_value_1,ref_value_2,ref_value_3,ref_value_4,ref_value_5 from msg_log where module " +
           "='" + UIConstants.Module + "'and msg_id ='" + UIConstants.MessageId + "' and msg='" + UIConstants.Message + "'";

        public static string CorbaSql = "select ph.group_id from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id= pd.id where " +
            "func_name like '%"+UIConstants.CorbaFunctionName+"%' and " +
            "parm_name='return' and parm_value='PkValid' and crt_date like sysdate";

        public static string SysCodeSql = "select code_id CodeId,Code_desc CodeDesc,short_desc ShortDesc,misc_flags MiscFlag,rec_type RecType,code_type CodeType,cust_id CustId from sys_code " +
            "where rec_type='"+UIConstants.RecType+"' and code_type='"+UIConstants.CodeType+"' order by code_id asc";

        public static string FetchUserPrefIdSql = "Select Id from swm_user_setting where user_id=355 and setting_id=4";
    }
}
