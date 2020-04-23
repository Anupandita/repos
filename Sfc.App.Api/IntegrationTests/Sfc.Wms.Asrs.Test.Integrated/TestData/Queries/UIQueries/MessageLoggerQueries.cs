using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries
{
    class MessageLoggerQueries
    {
        public static string FetchMessageLoggerDtSql = "select module,msg_id,log_date_time,msg,ref_code_1,ref_code_2,ref_code_3," +
            "ref_code_4,ref_code_5,ref_value_1,ref_value_2,ref_value_3,ref_value_4,ref_value_5 from msg_log where module " +
            "='" + UIConstants.Module+"'and msg_id ='"+UIConstants.MessageId+"' and msg='"+UIConstants.Message+"'";
       
    }
}
