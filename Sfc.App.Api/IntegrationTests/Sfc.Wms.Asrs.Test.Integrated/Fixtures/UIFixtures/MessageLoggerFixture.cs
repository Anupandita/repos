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
            };

        }

        public void CallMessageLoggerApiWithUrl(string url)
        {            
            var request = CallPostApi();           
            request.AddJsonBody(new List<MessageLogDto> { messageLogDto });           
            VerifyCreatedResultAndStoreBearerToken(url,request);
        }

        public void VerifyLogInsteredInMessageLogTableInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(MessageLoggerQueries.FetchMessageLoggerDtSql, db);
                var tempdt = new DataTable();
                tempdt.Load(_command.ExecuteReader());
                Assert.AreEqual(UIConstants.Module,tempdt.Rows[0][0]);
                Assert.AreEqual(UIConstants.MessageId, tempdt.Rows[0][1]);               
                Assert.AreEqual(UIConstants.Message, tempdt.Rows[0][3]);
            }
            }
    }
}
