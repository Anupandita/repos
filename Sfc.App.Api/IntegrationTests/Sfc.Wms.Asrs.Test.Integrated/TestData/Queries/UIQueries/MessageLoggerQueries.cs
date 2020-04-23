using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries
{
    class MessageLoggerQueries
    {
        public static string FetchMessageLoggerDtSql = "select module,msg_id,log_date_time,msg from msg_log where module ='"+UIConstants.Module+"'and msg_id ='"+UIConstants.MessageId+"' and msg='"+UIConstants.Message+"'";
       
    }
}
