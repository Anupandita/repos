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

    public class SynrQtyDefferenceMessageFixture : DataBaseFixtureForSynrQtyDefference
    {
        protected BaseResult Result;
        protected new IRestResponse Response;
        protected string SynrUrl = ConfigurationManager.AppSettings["BaseUrl"] + TestData.Parameter.Synchronizationrequest;
        protected string SyncUrl;
        


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

        protected new IRestResponse ApiIsCalled(string url)
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


        protected void TestInitialize()
        {
            GetValidData();
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

        protected void  ValidateForQtyShouldBeUpdatedInPickLocationTable()
        {
           Assert.AreEqual(PickLocnBeforeApi.ActualInventoryQuantity -(PldSnapQtyDefference.ActualInventoryQuantity - SyndDataQtyDefference.Quantity), PickLocnAfterApi.ActualInventoryQuantity);
        }



        protected void ValidateForPixTran()
        {
            Assert.AreEqual(SyndDataQtyDefference.SkuId,PixTran.Style);
            Assert.AreEqual(Math.Abs(Convert.ToInt32(PldSnapQtyDefference.ActualInventoryQuantity -SyndDataQtyDefference.Quantity)),PixTran.InventoryAdjustmentQuantity);
            Assert.AreEqual("S", PixTran.InventoryAdjustmentType);
            Assert.AreEqual(SyndDataQtyDefference.LocationId,PixTran.ReferenceField1);
            Assert.AreEqual(SyndDataQtyDefference.SynchronizationId,int.Parse(PixTran.ReferenceField2));
            Assert.AreEqual(SyndDataQtyDefference.MessageKey, Convert.ToInt32(PixTran.ReferenceField4));
        }

    }
}
