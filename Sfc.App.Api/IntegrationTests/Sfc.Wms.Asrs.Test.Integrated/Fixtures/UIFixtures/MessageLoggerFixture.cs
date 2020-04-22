using Newtonsoft.Json;
using Sfc.Core.OnPrem.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Dtos;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
  public  class MessageLoggerFixture : BaseFixture
    {

        MessageLogDto messageLogDto;
        protected DataTable MessageLoggerDt = new DataTable();
        public void CreateInputDtoForMessageLoggerApi()
        {
            messageLogDto = new MessageLogDto()
            { Module = UIConstants.Module,
              MessageId = UIConstants.MessageId,
              LogDateTime = System.DateTime.Today,
              Message= UIConstants.Message,
            };

        }

        public void CallMessageLoggerApiWithUrl(string url)
        {
            //var input = JsonConvert.SerializeObject(messageLogDto);
            //var response = CallPostApi(url, input);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            // request.AddHeader("content-type",content.);
            request.AddHeader("Authorization", UIConstants.BearerToken);
            request.AddJsonBody(new List<MessageLogDto> { messageLogDto });
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
            VerifyCreatedResultAndStoreBearerToken(response);
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
                Assert.AreEqual(System.DateTime.Today, tempdt.Rows[0][2]);
                Assert.AreEqual(UIConstants.Message, tempdt.Rows[0][3]);
            }
            }
    }
}
