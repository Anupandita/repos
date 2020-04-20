using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
 
    public class SyndwithsameskuCombinationMessageFixture:DataBaseFixtureForSyndwithsameskuCombination
    {
        protected BaseResult Result;
        protected IRestResponse Response;
        protected string SynrUrl = ConfigurationManager.AppSettings["BaseUrl"] + TestData.Parameter.Synchronizationrequest;
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
        protected string SyncUrl;
        protected string SyndUrl;

        protected void TestInitializeForValidMessage()
        {
            GetDataBeforeTriggerSynr();
        }

        protected void SynrApiIsCalledCreatedIsReturnedWithValidUrlAndSyncIdIs(string url, long syncId)
        {
            Result = SynrResult(url);
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }
        protected BaseResult SynrResult(string url)
        {
            var response = ApiIsCalled(SynrUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }

        protected IRestResponse ApiIsCalled(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected void GetDataFromDataBaseForSynrScenarios()
        {
            GetDataAfterTrigger();
        }
        protected void VerifySynrMessageWasInsertedIntoSwmToMhe()
        {
            VerifySynrMessageWasInsertedIntoSwmToMhe(Synr, SwmToMheSynr);
        }

        protected void VerifySynrMessageWasInsertedIntoSwmToMhe(SynrDto synr, SwmToMheDto swmToMhe)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheSynr.SourceMessageStatus);
            Assert.AreEqual(0, SwmToMheSynr.SourceMessageResponseCode);
            Assert.AreEqual(TransactionCode.Synr, Synr.TransactionCode);
            Assert.AreEqual(MessageLength.Synr, Synr.MessageLength);
            //Assert.AreEqual(Convert.ToString(Nextupcnt)+1, Synr.SynchronizationId);
        }
        protected void VerifySynrMessageWasInsertedIntoWmsToEms()
        {
            VerifySynrMessageWasInsertedIntoWmsToEmsTable(WmsToEmsSynr);
        }
        protected void VerifySynrMessageWasInsertedIntoWmsToEmsTable(WmsToEmsDto wte1)
        {
            Assert.AreEqual(SwmToMheSynr.SourceMessageKey, wte1.MessageKey);
            Assert.AreEqual(SwmToMheSynr.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(SwmToMheSynr.SourceMessageText, wte1.MessageText);
            Assert.AreEqual(SwmToMheSynr.SourceMessageResponseCode, wte1.ResponseCode);
            Assert.AreEqual(TransactionCode.Synr, wte1.Transaction);
        }
        protected void VerifyCountPickLocationTableAndSnapshotTable()
        {
            Assert.AreEqual(Convert.ToInt32(BeoforeApiPickLocn), Pldsnapshot);
        }

        protected void TestInitializeForDupilcateSyndForaSameSkuWithQuantities()
        {
            SyndMessageForSameSkuWithQuantities();
        }
        protected void ValidSyndSameSkuWithDifferentQtyUrl()
        {
            SyndUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={SyncDataDuplicate.MsgKey}&{TestData.Parameter.MsgProcessor}={EmsToWmsParameters.Process}";
        }
        protected void SyndSameSkuWithDifferentQtyApiIsCalledWithValidMsgKey()
        {
            var result = SyndSameSkuWithDifferentQty();
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
        }
        protected BaseResult SyndSameSkuWithDifferentQty()
        {
            var response = ApiIsCalledSynd(SyndUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected IRestResponse ApiIsCalledSynd(string syndqurl)
        {
            var client = new RestClient(syndqurl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Contents.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected void GetDataFromDataBaseForSyndSameSkuWithDifferentQtyScenarios()
        {
            GetDataAfterTriggerForQtyDifference();
        }
        protected void VerifySyndMessageWasInsertedIntoSwmFromMhe()
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMheSynd.SourceMessageProcess);
            Assert.AreEqual(SyncDataDuplicate.MsgKey, SwmFromMheSynd.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMheSynd.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMheSynd.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMheSynd.SourceMessageText);
        }
      

        protected void VerifySyndMessageWasInsertedIntoSwmFromMheTableandSyndDataTable()
        {
            Assert.AreEqual(WmsSyndData.SynchronizationId, int.Parse(Synd.SynchronizationId));
            Assert.AreEqual(WmsSyndData.SkuId, Synd.Sku);
            //Assert.AreEqual(Convert.ToString(WmsSyndData.Quantity), Synd.Quantity);
            //Assert.AreEqual(WmsSyndData.LocationId, SwmFromMheSynd.LocationId);
        }


        public void SyncTestInitialize()
        {
            SyncGetValidData();
        }
        protected void SyncTestInitializeForValidMessage()
        {
            GetDataBeforeTriggerSync();
        }
        protected void ValidSyncUrlMsgKeyAndProcessorIs(string url, Int64 currentMsgKey, string currentMsgProcessor)
        {
            SyncUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={currentMsgKey}&{TestData.Parameter.MsgProcessor}={currentMsgProcessor}";
        }
        protected void SyncApiIsCalledWithValidMsgKey()
        {
            var result = SyncResult();
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
        }
        protected BaseResult SyncResult()
        {
            var response = ApiIsCalledSync(SyncUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected IRestResponse ApiIsCalledSync(string syncqurl)
        {
            var client = new RestClient(syncqurl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Contents.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected void GetValidDataAfterTriggerSync()
        {
            GetDataAfterTriggerSync();
        }
        protected void VerifySyncMessageWasInsertedIntoEmsToWms()
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMheSyncDto.SourceMessageProcess);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMheSyncDto.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMheSyncDto.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMheSyncDto.SourceMessageText);
        }
        protected void ValidateForQtyDifferenceShouldBeUpdatedInPickLocationTable()
        {
            Assert.AreEqual(SyndSum, PickLocnAfterApi.ActualInventoryQuantity);
        }

    }
}
