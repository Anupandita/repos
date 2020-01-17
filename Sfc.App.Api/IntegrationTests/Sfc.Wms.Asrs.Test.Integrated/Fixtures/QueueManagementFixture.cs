using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class QueueManagementFixture: CommonFunction
    {
        protected string SqlStatements = "";
        protected OrmtParams OrmtParameters;
        protected List<SwmEligibleOrmtCarton> SwmEligibleOrmt = new List<SwmEligibleOrmtCarton>();


        public List<SwmEligibleOrmtCarton> GetValidCartonsFromSwmEligibleOrmtCarton(OracleConnection db)
        {
            var swmEligibleOrmtCarton = new List<SwmEligibleOrmtCarton>();
            SqlStatements = $"select * from SWM_ELGBL_ORMT_CARTONS where status = 10 order by created_date_time desc";
            Command = new OracleCommand(SqlStatements, db);
            var validData = Command.ExecuteReader();
            while (validData.Read())
            {
                var set = new SwmEligibleOrmtCarton
                {
                    CartonNumber = validData["CARTON_NBR"].ToString(),
                    WaveNumber = validData["WAVE_NBR"].ToString()
                };
                swmEligibleOrmtCarton.Add(set);

            }
            return swmEligibleOrmtCarton;
        }

        
        public void GetValidCartonAndWaveNumberFromSwmEligibleOrmtCarton()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                SwmEligibleOrmt = GetValidCartonsFromSwmEligibleOrmtCarton(db);
                for (var i = 0; i < SwmEligibleOrmt.Count; i++)
                {
                    OrmtParameters = new OrmtParams
                    {
                        ActionCode = OrmtActionCode.AddRelease,
                        CartonNumber = SwmEligibleOrmt[i].CartonNumber,
                        WaveNumber = SwmEligibleOrmt[i].WaveNumber
                    };
                    var json = new JavaScriptSerializer().Serialize(OrmtParameters);
                    Transaction = db.BeginTransaction();
                    var msgKey = GetSeqNbrEmsToWms(db);
                    var insertQuery = $"insert into swm_msg_queue values('{msgKey}','ORMT','{json}','1','Sequential','{DateTime.Now.ToString("dd-MMM-yy")}','TestUser','{DateTime.Now.ToString("dd-MMM-yy")}','TestUser')";
                    Command = new OracleCommand(insertQuery, db);
                    Command.ExecuteNonQuery();
                    Transaction.Commit();
                }
            }
        }

        public void GetValidCaseNumbersFromCartonHeader()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();


            }
        }

    }
}
