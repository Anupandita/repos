using System;
using System.Collections.Generic;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Dtos;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public  class MessageLoggerFixture : BaseFixture
    {

        MessageLogDto messageLogDto;
        protected DataTable MessageLoggerDt = new DataTable();
       
        public void CreateInputDtoForMessageLoggerApi()
        {
            UIConstants.Message= Guid.NewGuid().ToString("n").Substring(0, 8);
            UIConstants.LogDate = System.DateTime.Today;
            messageLogDto = new MessageLogDto()
            { Module = UIConstants.Module,
              MessageId = UIConstants.MessageId,
              LogDateTime = UIConstants.LogDate,                
              Message = UIConstants.Message,
              ReferenceCode1= UIConstants.RefCode,
                ReferenceCode2 = UIConstants.RefCode,
              ReferenceCode3 = UIConstants.RefCode,
                ReferenceCode4 = UIConstants.RefCode,
                ReferenceCode5 = UIConstants.RefCode,
                ReferenceValue1 = UIConstants.RefValue,
                ReferenceValue2 = UIConstants.RefValue,
                ReferenceValue3 = UIConstants.RefValue,
                ReferenceValue4 = UIConstants.RefValue,
                ReferenceValue5 = UIConstants.RefValue
            };

        }

        public void CallMessageLoggerApiWithUrl(string url)
        {            
            var request = CallPostApi();           
            request.AddJsonBody(new List<MessageLogDto> { messageLogDto });
            var response = ExecuteRequest(url, request);
            VerifyCreatedResultAndStoreBearerToken(response);
        }

        public void VerifyLogInsteredInMessageLogTableInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(UIApiQueries.FetchMessageLoggerDtSql, db);
                var tempdt = new DataTable();
                tempdt.Load(_command.ExecuteReader());
                Assert.AreEqual(UIConstants.Module,tempdt.Rows[0][0]);
                Assert.AreEqual(UIConstants.MessageId, tempdt.Rows[0][1]);               
                Assert.AreEqual(UIConstants.Message, tempdt.Rows[0][3]);
                Assert.AreEqual(UIConstants.RefCode, tempdt.Rows[0][4]);
                Assert.AreEqual(UIConstants.RefCode, tempdt.Rows[0][5]);
                Assert.AreEqual(UIConstants.RefCode, tempdt.Rows[0][6]);
                Assert.AreEqual(UIConstants.RefCode, tempdt.Rows[0][7]);
                Assert.AreEqual(UIConstants.RefCode, tempdt.Rows[0][8]);
                Assert.AreEqual(UIConstants.RefValue, tempdt.Rows[0][9]);
                Assert.AreEqual(UIConstants.RefValue, tempdt.Rows[0][10]);
                Assert.AreEqual(UIConstants.RefValue, tempdt.Rows[0][11]);
                Assert.AreEqual(UIConstants.RefValue, tempdt.Rows[0][12]);
                Assert.AreEqual(UIConstants.RefValue, tempdt.Rows[0][13]);
            }
            }
    }
}
