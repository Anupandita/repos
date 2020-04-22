using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries
{
    class MessageLoggerQueries
    {
        public const string FetchMessageLoggerDtSql = "select module,msg_id,log_date_time,msg from msg_log where module ='ARCHLAYER'and msg_id ='MA-0234' and log_date_time like sysdate and msg='API Testing Message'";
       
    }
}
