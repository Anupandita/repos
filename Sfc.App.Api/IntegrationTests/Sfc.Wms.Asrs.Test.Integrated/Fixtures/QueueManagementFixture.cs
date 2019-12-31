using System;
using System.Data;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Framework.MessageQueue.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class QueueManagementFixture: CommonFunction
    {
        protected SwmMessageQueueDto  swmMessageQueue= new SwmMessageQueueDto();
        protected string SqlStatements = "";

        public SwmEligibleOrmtCarton GetValidCartonsFromSwmEligibleOrmtCarton(OracleConnection db)
        {
            var swmEligibleOrmtCarton = new SwmEligibleOrmtCarton();
            SqlStatements = $"select * from SWM_ELGBL_ORMT_CARTONS where status = 10 order by created_date_time desc";
            Command = new OracleCommand(SqlStatements, db);
            var validData = Command.ExecuteReader();
            if (validData.Read())
            {
                swmEligibleOrmtCarton.CartonNumber = validData["CARTON_NBR"].ToString();
            }
            return swmEligibleOrmtCarton;
        }

        public long InsertInToSwmMessageQueueTable(OracleConnection db, SwmMessageQueueDto swmMessageQueue )
        {
            Transaction = db.BeginTransaction();
            var msgKey = GetSeqNbrEmsToWms(db);
            // var msgJson = "{"CartonNumber":"00000999994116036839","WaveNumber":"","ActionCode":"AddRelease"}";
            var insertQuery = $"insert into swm_msg_queue values('{msgKey}','ORMT','msgJson','1','Sequential','{DateTime.Now.ToString("dd-MMM-yy")}','TestUser','{DateTime.Now.ToString("dd-MMM-yy")}','TestUser')";
            Command = new OracleCommand(insertQuery, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
            return msgKey;
        }












    }
}
