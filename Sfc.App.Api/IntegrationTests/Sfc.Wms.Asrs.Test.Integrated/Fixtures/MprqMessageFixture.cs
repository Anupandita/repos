using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Newtonsoft.Json;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class MprqMessageFixture : DataBaseFixtureForMprq
    {
        protected Int64 CurrentMsgKey;
        protected string CurrentMsgProcessor;
        protected IRestResponse Response;
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
        protected string MprqUrl;

        protected void TestInitializeForValidMessage()
        {
            GetDataBeforeTrigger();
        }
        
        protected void AValidMprqUrl(string url,Int64 currentMsgKey,string currentMsgProcessor)
        {
            MprqUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={currentMsgKey}&{TestData.Parameter.MsgProcessor}={currentMsgProcessor}";
        }
        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(MprqUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);          
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }

        protected BaseResult MprqResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }

        protected void MprqApiIsCalledWithValidMsgKey()
        {
            var result = MprqResult();
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
        }
        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }

        protected void VerifyMprqMessageWasInsertedIntoSwmFromMhe()
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(MprqData.MsgKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMhe.SourceMessageText);
        }

        protected void VerifyMpidMessageWasInsertedIntoswmToMhe()
        {
            Assert.AreEqual(NextUpCounter.PrefixField + ((NextUpCounter.CurrentNumber)+1).ToString("000000000"), Mpid.MasterPackId);
        }

        protected void VerifyLocationMpid()
        {
            Assert.AreEqual(Mprq.LocationId, Mpid.LocationId);
        }
    }  
}
