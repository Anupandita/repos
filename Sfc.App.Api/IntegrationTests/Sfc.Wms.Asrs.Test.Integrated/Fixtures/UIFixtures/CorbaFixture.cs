using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Sfc.Wms.Foundation.Corba.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class CorbaFixture : BaseFixture
    {
        CorbaDto corbaDto;

        protected void CreateCorbaInputParamAndUrl(string type, string functionName)
        {

            UIConstants.CorbaUrl = UIConstants.CorbaUrl + type + functionName + "/" + UIConstants.Default;

            corbaDto = new CorbaDto()
            {
                WorkStationId = UIConstants.WorkStation,
                Inputs = new List<string> { "00100283000842762775" }
            };
           
        }
        protected void CallCorbaApi(string corbaUrl)
        {
            var request = CallPostApi();
            request.AddJsonBody(corbaDto);
            var response=ExecuteRequest(corbaUrl, request);
            VerifyOkResultAndStoreBearerToken(response);
        }
        protected void VerifyCorbaApiOutputInDb() 
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(UIApiQueries.CorbaSql, db);
                var tempDt = new DataTable();
                tempDt.Load(_command.ExecuteReader());
                var count=tempDt.Rows.Count;
                Assert.IsTrue(count > 0);
            }
        }
    }   
}
    

