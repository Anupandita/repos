using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using System;
using System.Configuration;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Core.OnPrem.Result;
using System.Diagnostics;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class OrstMessageFixture : DataBaseFixtureForOrst
    {
        protected Int64 CurrentMsgKey;
        protected string CurrentMsgProcess;
        protected string BaseUrl = @ConfigurationManager.AppSettings["EmsToWmsUrl"];
        protected string OrstUrl;

        protected IRestResponse Response;

        protected void InitializeTestData()
        {
            GetDataBeforeTriggerOrst();
        }

        protected void TestDataForActionCodeComplete()
        {
            GetDataBeforeCallingApiForActionCodeComplete();
        }

        protected void TestDataForActionCodeDeAllocate()
        {
            GetDataBeforeCallingApiForActionCodeDeAllocate();
        }

        protected void TestDataForActionCodeCancel()
        {
            GetDataBeforeCallingApiForActionCodeCancel();
        }

        protected void ReadDataAfterApiForActionCodeAllocated()
        {
            GetDataAfterTriggerOrstCase1();
        }

        protected void ReadDataAfterApiForActionCodeComplete()
        {
            GetDataAfterTriggerOrstCase2();
        }

        protected void ReadDataAfterApiForActionCodeDeAllocate()
        {
            GetDataAfterTriggerOrstCase3();
        }

        protected void ReadDataAfterApiForActionCodeCancel()
        {
            GetDataAfterTriggerOrstCase4();
        }
        
        protected void ReadDataBeforeCallingApiForActionCodeCompleteWithBitsEnabled()
        {
            GetDataBeforeApiForActionCodeCompletedWithBitsEnabled();
        }

        protected void ReadDataAfterCallingApiForActionCodeCompleteWithBitsEnabled()
        {
            GetDataAfterApiForActionCodeCompleteWithBitsEnabled();
        }

        protected void ReadDataBeforeApiForNegativeCaseWherePickTicketSeqNumberIsLessThan1()
        {
            GetDataBeforeApiForActionCodeCompleteWithPickTicketSeqNumberLessThan1();
        }
        protected void MsgKeyForCase1()
        {
            CurrentMsgKey = MsgKeyForAllocated.MsgKey;
            CurrentMsgProcess = EmsToWmsAllocated.Process;
        }

        protected void MsgKeyForCase2()
        {
            CurrentMsgKey = MsgKeyForCompleted.MsgKey;
            CurrentMsgProcess = EmsToWmsCompleted.Process;
        }

        protected void MsgKeyForCase3()
        {
            CurrentMsgKey = MsgKeyForDeallocated.MsgKey;
            CurrentMsgProcess = EmsToWmsDeallocated.Process;
        }

        protected void MsgKeyForCase4()
        {
            CurrentMsgKey = MsgKeyForCanceled.MsgKey;
            CurrentMsgProcess = EmsToWmsCanceled.Process;
        }

        protected void MsgKeyForCase5()
        {
            CurrentMsgKey = MsgKeysForCase5.MsgKey;
            CurrentMsgProcess = EmsToWmsCompleted.Process;
        }

        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(OrstUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);          
            return Response;
        }

        protected void ValidOrstUrl()
        {
            OrstUrl = $"{BaseUrl}?{"msgKey"}={CurrentMsgKey}&{"msgProcessor"}={CurrentMsgProcess}";
        }

        protected BaseResult OrstResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }

        protected void OrstApiIsCalledCreatedIsReturned()
        {
            var result = OrstResult();
            Debug.Print(result.ResultType.ToString());
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
        }


        protected void OrstApiIsCalledForNegativeCase()
        {
            var result = OrstResult();
            Assert.AreEqual("1", result.ValidationMessages.Count.ToString());
        }
        protected void GetDataAfterTriggerForAllocatedActionCode()
        {
            GetDataAfterTriggerOrstCase1();
        }
        
        protected void GetDataAfterTriggerForCompletedActionCode()
        {
            GetDataAfterTriggerOrstCase2();
        }

        protected void GetDataAfterTriggerForDeAllocateActionCode()
        {
            GetDataAfterTriggerOrstCase3();
        }

        protected void GetDataAfterTriggerForCancelActionCode()
        {
            GetDataAfterTriggerOrstCase4();
        }
        protected void ValidateForOrmtMessagetosortViewtable()
        {
            Assert.AreEqual(MessageToSort.Ptn, CancelOrder.CartonNbr);
        }
        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeAllocated()
        {
            Assert.AreEqual(EmsToWmsAllocated.Process, SwmFromMheAllocated.SourceMessageProcess);
            Assert.AreEqual(EmsToWmsAllocated.MessageKey, SwmFromMheAllocated.SourceMessageKey);
            Assert.AreEqual(EmsToWmsAllocated.Status, SwmFromMheAllocated.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsAllocated.Transaction, SwmFromMheAllocated.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsAllocated.ResponseCode, SwmFromMheAllocated.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsAllocated.MessageText, SwmFromMheAllocated.SourceMessageText);            
            Assert.AreEqual("ORST", OrstAllocated.TransactionCode);
            Assert.AreEqual("00255", OrstAllocated.MessageLength);
            Assert.AreEqual("Allocated", OrstAllocated.ActionCode);        
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete()
        {
            Assert.AreEqual(EmsToWmsCompleted.Process, SwmFromMheComplete.SourceMessageProcess);
            Assert.AreEqual(EmsToWmsCompleted.MessageKey, SwmFromMheComplete.SourceMessageKey);
            Assert.AreEqual(EmsToWmsCompleted.Status, SwmFromMheComplete.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsCompleted.Transaction, SwmFromMheComplete.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsCompleted.ResponseCode, SwmFromMheComplete.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsCompleted.MessageText, SwmFromMheComplete.SourceMessageText);
            Assert.AreEqual("ORST", OrstCompleted.TransactionCode);
            Assert.AreEqual("00255", OrstCompleted.MessageLength);
            //Assert.AreEqual("Complete", orstCase2.ActionCode);
        }
    
        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeCancel()
        {
            Assert.AreEqual(EmsToWmsCanceled.Process, SwmFromMheCancel.SourceMessageProcess);
            Assert.AreEqual(EmsToWmsCanceled.MessageKey, SwmFromMheCancel.SourceMessageKey);
            Assert.AreEqual(EmsToWmsCanceled.Status, SwmFromMheCancel.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsCanceled.Transaction, SwmFromMheCancel.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsCanceled.ResponseCode, SwmFromMheCancel.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsCanceled.MessageText, SwmFromMheCancel.SourceMessageText);         
            Assert.AreEqual(TransactionCode.Orst, OrstCanceled.TransactionCode);
            Assert.AreEqual("00255", OrstCanceled.MessageLength);
            Assert.AreEqual("Canceled", OrstCanceled.ActionCode);
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeDeAllocate()
        {
            Assert.AreEqual(EmsToWmsDeallocated.Process, SwmFromMheDeallocate.SourceMessageProcess);
            Assert.AreEqual(EmsToWmsDeallocated.MessageKey, SwmFromMheDeallocate.SourceMessageKey);
            Assert.AreEqual(EmsToWmsDeallocated.Status, SwmFromMheDeallocate.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsDeallocated.Transaction, SwmFromMheDeallocate.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsDeallocated.ResponseCode, SwmFromMheDeallocate.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsDeallocated.MessageText, SwmFromMheDeallocate.SourceMessageText);   
            Assert.AreEqual(TransactionCode.Orst, OrstDeallocate.TransactionCode);
            Assert.AreEqual("00255", OrstDeallocate.MessageLength);
            Assert.AreEqual("Deallocate", OrstDeallocate.ActionCode);
        }

        protected void VerifyPickTicketStatusHasChangedToInPickingForActionCodeAllocated()
        {
            Assert.AreEqual(35, PickTktHdrAllocated.PickTicketStatusCode);
        }

        protected void VerifyCartonStatusHasChangedToInPackingForActionCodeAllocated()
        {
            Assert.AreEqual(15, CartonHdrAllocated.StatusCode);
            Assert.AreEqual(Allocated.CurrentLocationId ,OrstAllocated.CurrentLocationId);
            Assert.AreEqual(Allocated.DestLocnId , OrstAllocated.DestinationLocationId);
        }


        protected void VerifyCartonStatusHasChangedToPickedForActionCodeComplete()
        {
            Assert.AreEqual(30,CartonHdrCompleted.StatusCode);         
        }

        protected void VerifyCartonStatusHasChangedTo5ForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(5, CartonHdrCompleted.StatusCode);
        }
        
        protected void VerifyForQuantitiesInToPickLocationTableForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(PickLcnCase2BeforeApi.ToBePickedQty - Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickLcnCase2.ToBePickedQty);
        }

        protected void VerifyForOrmtCountForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(PickLcnExtCase2BeforeApi.ActiveOrmtCount - 1, PickLcnExtCase2.ActiveOrmtCount);
        }

        protected  void ValidateForQuantitiesInTocartonDetailTableForActionCodeComplete()
        {
            Assert.AreEqual(CartonDtlCase2BeforeApi.UnitsPacked + Convert.ToDecimal(OrstCompleted.QuantityDelivered), CartonDtlCase2AfterApi.UnitsPacked);
            Assert.AreEqual(CartonDtlCase2BeforeApi.ToBePackedUnits - Convert.ToDecimal(OrstCompleted.QuantityDelivered), CartonDtlCase2AfterApi.ToBePackedUnits);
        }

        protected void ValidateForQuantitiesInToPickTicketDetailTableForActionCodeComplete()
        {
            Assert.AreEqual(PickTktDtlCase2BeforeApi.UnitsPacked + Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickTktDtlCase2AfterApi.UnitsPacked);
            Assert.AreEqual(PickTktDtlCase2BeforeApi.VerifiedAsPacked + Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickTktDtlCase2AfterApi.VerifiedAsPacked);
        }

        protected void VerifyPickTicketStatusHasChangedToWeighedForStatusCodeComplete()
        {
            Assert.AreEqual(50, PickTktHdrCompleted.PickTicketStatusCode);
        }

        protected void VerifyAllocationStatusHasChangedToCompleteForActionCodeComplete()
        {
            Assert.AreEqual(Convert.ToDecimal(AllocInvnDtlCompletedBeforeApi.QtyPulled) + Convert.ToDecimal(OrstCompleted.QuantityDelivered), Convert.ToDecimal(AllocInvnDtlCompletedAfterApi.QtyPulled));
            Assert.AreEqual("90", AllocInvnDtlCompletedAfterApi.StatCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeComplete()
        {
            Assert.AreEqual(PickLcnCase2BeforeApi.ActualInventoryQuantity - Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickLcnCase2.ActualInventoryQuantity);
            Assert.AreEqual(PickLcnCase2BeforeApi.ToBePickedQty - Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickLcnCase2.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeComplete()
        {
           // Assert.AreEqual(pickLcnExtCase2BeforeApi.ActiveOrmtCount - 1, pickLcnExtCase2.ActiveOrmtCount);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeDeAllocate()
        {
            Assert.AreEqual("5", CartonHdrDeallocated.StatusCode);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeCancel()
        {
            Assert.AreEqual(5, CartonHdrCanceled.StatusCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeCancel()
        {
            Assert.AreEqual(PickLcnCase4BeforeApi.ToBePickedQty - Convert.ToDecimal(OrstCanceled.QuantityDelivered), PickLcnCase4.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeCancel()
        {
            Assert.AreEqual(PickLcnExtCase4BeforeApi.ActiveOrmtCount - 1, PickLcnExtCase4.ActiveOrmtCount);
        }


    }
}
