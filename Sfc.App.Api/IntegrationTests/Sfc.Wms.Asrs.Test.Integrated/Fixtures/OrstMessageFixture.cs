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
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
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
        
        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(OrstUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);          
            return Response;
        }

        protected void ValidMsgKeyMsgProcessorAndOrstUrlIs(Int64 currentMsgKey,string currentMsgProcess,string url)
        {
            OrstUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={currentMsgKey}&{TestData.Parameter.MsgProcessor}={currentMsgProcess}";
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
            Assert.AreEqual(TransactionCode.Orst, OrstAllocated.TransactionCode);
            Assert.AreEqual(MessageLength.Orst, OrstAllocated.MessageLength);
            Assert.AreEqual(OrstActionCode.Allocated, OrstAllocated.ActionCode);        
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete()
        {
            Assert.AreEqual(EmsToWmsCompleted.Process, SwmFromMheComplete.SourceMessageProcess);
            Assert.AreEqual(EmsToWmsCompleted.MessageKey, SwmFromMheComplete.SourceMessageKey);
            Assert.AreEqual(EmsToWmsCompleted.Status, SwmFromMheComplete.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsCompleted.Transaction, SwmFromMheComplete.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsCompleted.ResponseCode, SwmFromMheComplete.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsCompleted.MessageText, SwmFromMheComplete.SourceMessageText);
            Assert.AreEqual(TransactionCode.Orst, OrstCompleted.TransactionCode);
            Assert.AreEqual(MessageLength.Orst, OrstCompleted.MessageLength);
            Assert.AreEqual(OrstActionCode.Complete, OrstCompleted.ActionCode);
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
            Assert.AreEqual(MessageLength.Orst, OrstCanceled.MessageLength);
            Assert.AreEqual(OrstActionCode.Canceled, OrstCanceled.ActionCode);
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
            Assert.AreEqual(MessageLength.Orst, OrstDeallocate.MessageLength);
            Assert.AreEqual(OrstActionCode.Deallocate, OrstDeallocate.ActionCode);
        }

        protected void VerifyPickTicketStatusHasChangedToInPickingForActionCodeAllocated()
        {
            Assert.AreEqual(Constants.PktStatusForInPacking, PickTktHdrAllocated.PickTicketStatusCode);
        }

        protected void VerifyCartonStatusHasChangedToInPackingForActionCodeAllocated()
        {
            Assert.AreEqual(Constants.CartonStatusForInPacking, CartonHdrAllocated.StatusCode);
            Assert.AreEqual(Allocated.CurrentLocationId ,OrstAllocated.CurrentLocationId);
            Assert.AreEqual(Allocated.DestLocnId , OrstAllocated.DestinationLocationId);
        }

        protected void VerifyCartonStatusHasChangedToPickedForActionCodeComplete()
        {
            Assert.AreEqual(Constants.CartonStatusForPicked,CartonHdrCompleted.StatusCode);         
        }

        protected void VerifyCartonStatusHasChangedTo5ForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(Constants.CartonStatusAllocated, CartonHdrCompleted.StatusCode);
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
            Assert.AreEqual(Constants.PktWeighed, PickTktHdrCompleted.PickTicketStatusCode);
        }

        protected void VerifyAllocationStatusHasChangedToCompleteForActionCodeComplete()
        {
            Assert.AreEqual(Convert.ToDecimal(AllocInvnDtlCompletedBeforeApi.QtyPulled) + Convert.ToDecimal(OrstCompleted.QuantityDelivered), Convert.ToDecimal(AllocInvnDtlCompletedAfterApi.QtyPulled));
            Assert.AreEqual(Constants.AllocationStatus, AllocInvnDtlCompletedAfterApi.StatCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeComplete()
        {
            Assert.AreEqual(PickLcnCase2BeforeApi.ActualInventoryQuantity - Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickLcnCase2.ActualInventoryQuantity);
            Assert.AreEqual(PickLcnCase2BeforeApi.ToBePickedQty - Convert.ToDecimal(OrstCompleted.QuantityDelivered), PickLcnCase2.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeComplete()
        {
            Assert.AreEqual(PickLcnExtCase2BeforeApi.ActiveOrmtCount - 1, PickLcnExtCase2.ActiveOrmtCount);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeCancel()
        {
            Assert.AreEqual(Constants.CartonStatusAllocated, CartonHdrCanceled.StatusCode);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeCancel()
        {
            Assert.AreEqual(PickLcnExtCase4BeforeApi.ActiveOrmtCount - 1, PickLcnExtCase4.ActiveOrmtCount);
        }

    }
}
