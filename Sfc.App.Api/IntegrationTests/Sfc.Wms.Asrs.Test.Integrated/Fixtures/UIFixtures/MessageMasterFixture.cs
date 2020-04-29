using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries;
using Sfc.Wms.Configuration.MessageMaster.Contracts.Dtos;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class MessageMasterFixture:BaseFixture
    {

        DataTable MsgMasterDt = new DataTable();
        DataTable MsgMasterApiDt = new DataTable();
        public void GetMessageMasterRecordsRelatedToUIFromDb()         
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(UIApiQueries.MsgMasterSql, db);
                MsgMasterDt.Load(_command.ExecuteReader());
            }
            }

        public void CallMessageMasterApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<MessageDetailDto>>>(response.Content).Payload;
            MsgMasterApiDt = ToDataTable(payload);
        }
       public void  VerifyMessageMasterApiOutputAgainstDbOutput()
        {
            VerifyApiOutputAgainstDbOutput(MsgMasterDt,MsgMasterApiDt); 
        }
    }
}
