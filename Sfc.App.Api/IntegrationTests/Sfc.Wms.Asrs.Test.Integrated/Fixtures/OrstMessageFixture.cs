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
        protected Int64 currentMsgKey;
        protected string OrstUrl = @ConfigurationManager.AppSettings["EmsToWmsUrl"];
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
            currentMsgKey = msgKeyForAllocated.MsgKey;
        }

        protected void MsgKeyForCase2()
        {
            currentMsgKey = msgKeyForCompleted.MsgKey;
        }

        protected void MsgKeyForCase3()
        {
            currentMsgKey = msgKeyForDeallocated.MsgKey;
        }

        protected void MsgKeyForCase4()
        {
            currentMsgKey = msgKeyForCanceled.MsgKey;
        }

        protected void MsgKeyForCase5()
        {
            currentMsgKey = msgKeyForCase5.MsgKey;
        }

        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(OrstUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(currentMsgKey);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);          
            return Response;
        }

        protected BaseResult OrstResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
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
            Assert.AreEqual("2", result.ValidationMessages.Count.ToString());
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

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeAllocated()
        {
            Assert.AreEqual(emsToWmsAllocated.Process, swmFromMheAllocated.SourceMessageProcess);
            Assert.AreEqual(emsToWmsAllocated.MessageKey, swmFromMheAllocated.SourceMessageKey);
            Assert.AreEqual(emsToWmsAllocated.Status, swmFromMheAllocated.SourceMessageStatus);
            Assert.AreEqual(emsToWmsAllocated.Transaction, swmFromMheAllocated.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsAllocated.ResponseCode, swmFromMheAllocated.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsAllocated.MessageText, swmFromMheAllocated.SourceMessageText);            
            Assert.AreEqual("ORST", orstAllocated.TransactionCode);
            Assert.AreEqual("00255", orstAllocated.MessageLength);
            Assert.AreEqual("Allocated", orstAllocated.ActionCode);        
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete()
        {
            Assert.AreEqual(emsToWmsCompleted.Process, swmFromMheComplete.SourceMessageProcess);
            Assert.AreEqual(emsToWmsCompleted.MessageKey, swmFromMheComplete.SourceMessageKey);
            Assert.AreEqual(emsToWmsCompleted.Status, swmFromMheComplete.SourceMessageStatus);
            Assert.AreEqual(emsToWmsCompleted.Transaction, swmFromMheComplete.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsCompleted.ResponseCode, swmFromMheComplete.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsCompleted.MessageText, swmFromMheComplete.SourceMessageText);
            Assert.AreEqual("ORST", orstCompleted.TransactionCode);
            Assert.AreEqual("00255", orstCompleted.MessageLength);
            //Assert.AreEqual("Complete", orstCase2.ActionCode);
        }
    
        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeCancel()
        {
            Assert.AreEqual(emsToWmsCanceled.Process, swmFromMheCancel.SourceMessageProcess);
            Assert.AreEqual(emsToWmsCanceled.MessageKey, swmFromMheCancel.SourceMessageKey);
            Assert.AreEqual(emsToWmsCanceled.Status, swmFromMheCancel.SourceMessageStatus);
            Assert.AreEqual(emsToWmsCanceled.Transaction, swmFromMheCancel.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsCanceled.ResponseCode, swmFromMheCancel.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsCanceled.MessageText, swmFromMheCancel.SourceMessageText);         
            Assert.AreEqual(TransactionCode.Orst, orstCanceled.TransactionCode);
            Assert.AreEqual("00255", orstCanceled.MessageLength);
            Assert.AreEqual("Canceled", orstCanceled.ActionCode);
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeDeAllocate()
        {
            Assert.AreEqual(emsToWmsDeallocated.Process, swmFromMheDeallocate.SourceMessageProcess);
            Assert.AreEqual(emsToWmsDeallocated.MessageKey, swmFromMheDeallocate.SourceMessageKey);
            Assert.AreEqual(emsToWmsDeallocated.Status, swmFromMheDeallocate.SourceMessageStatus);
            Assert.AreEqual(emsToWmsDeallocated.Transaction, swmFromMheDeallocate.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsDeallocated.ResponseCode, swmFromMheDeallocate.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsDeallocated.MessageText, swmFromMheDeallocate.SourceMessageText);   
            Assert.AreEqual(TransactionCode.Orst, orstDeallocate.TransactionCode);
            Assert.AreEqual("00255", orstDeallocate.MessageLength);
            Assert.AreEqual("Deallocate", orstDeallocate.ActionCode);
        }

        protected void VerifyPickTicketStatusHasChangedToInPickingForActionCodeAllocated()
        {
            Assert.AreEqual(35, pickTktHdrAllocated.PickTicketStatusCode);
        }

        protected void VerifyCartonStatusHasChangedToInPackingForActionCodeAllocated()
        {
            Assert.AreEqual(15, cartonHdrAllocated.StatusCode);
            Assert.AreEqual(allocated.CurrentLocationId ,orstAllocated.CurrentLocationId);
            Assert.AreEqual(allocated.DestLocnId , orstAllocated.DestinationLocationId);
        }


        protected void VerifyCartonStatusHasChangedToPickedForActionCodeComplete()
        {
            Assert.AreEqual(30,cartonHdrCompleted.StatusCode);         
        }

        protected void VerifyCartonStatusHasChangedTo5ForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(5, cartonHdrCompleted.StatusCode);
        }
        
        protected void VerifyForQuantitiesInToPickLocationTableForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(pickLcnCase2BeforeApi.ToBePickedQty - Convert.ToDecimal(orstCompleted.QuantityDelivered), pickLcnCase2.ToBePickedQty);
        }

        protected void VerifyForOrmtCountForActionCodeCompleteWithBitsEnabled()
        {
            Assert.AreEqual(pickLcnExtCase2BeforeApi.ActiveOrmtCount - 1, pickLcnExtCase2.ActiveOrmtCount);
        }

        protected  void ValidateForQuantitiesInTocartonDetailTableForActionCodeComplete()
        {
            Assert.AreEqual(cartonDtlCase2BeforeApi.UnitsPacked + Convert.ToDecimal(orstCompleted.QuantityDelivered), cartonDtlCase2AfterApi.UnitsPacked);
            Assert.AreEqual(cartonDtlCase2BeforeApi.ToBePackedUnits - Convert.ToDecimal(orstCompleted.QuantityDelivered), cartonDtlCase2AfterApi.ToBePackedUnits);
        }

        protected void ValidateForQuantitiesInToPickTicketDetailTableForActionCodeComplete()
        {
            Assert.AreEqual(pickTktDtlCase2BeforeApi.UnitsPacked + Convert.ToDecimal(orstCompleted.QuantityDelivered), pickTktDtlCase2AfterApi.UnitsPacked);
            Assert.AreEqual(pickTktDtlCase2BeforeApi.VerifiedAsPacked + Convert.ToDecimal(orstCompleted.QuantityDelivered), pickTktDtlCase2AfterApi.VerifiedAsPacked);
        }

        protected void VerifyPickTicketStatusHasChangedToWeighedForStatusCodeComplete()
        {
            Assert.AreEqual(50, pickTktHdrCompleted.PickTicketStatusCode);
        }

        protected void VerifyAllocationStatusHasChangedToCompleteForActionCodeComplete()
        {
            Assert.AreEqual(Convert.ToDecimal(allocInvnDtlCompletedBeforeApi.QtyPulled) + Convert.ToDecimal(orstCompleted.QuantityDelivered), Convert.ToDecimal(allocInvnDtlCompletedAfterApi.QtyPulled));
            Assert.AreEqual("90", allocInvnDtlCompletedAfterApi.StatCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeComplete()
        {
            Assert.AreEqual(pickLcnCase2BeforeApi.ActualInventoryQuantity - Convert.ToDecimal(orstCompleted.QuantityDelivered), pickLcnCase2.ActualInventoryQuantity);
            Assert.AreEqual(pickLcnCase2BeforeApi.ToBePickedQty - Convert.ToDecimal(orstCompleted.QuantityDelivered), pickLcnCase2.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeComplete()
        {
           // Assert.AreEqual(pickLcnExtCase2BeforeApi.ActiveOrmtCount - 1, pickLcnExtCase2.ActiveOrmtCount);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeDeAllocate()
        {
            Assert.AreEqual("5", cartonHdrDeallocated.StatusCode);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeCancel()
        {
            Assert.AreEqual(5, cartonHdrCanceled.StatusCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeCancel()
        {
            Assert.AreEqual(pickLcnCase4BeforeApi.ToBePickedQty - Convert.ToDecimal(orstCanceled.QuantityDelivered), pickLcnCase4.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeCancel()
        {
            Assert.AreEqual(pickLcnExtCase4BeforeApi.ActiveOrmtCount - 1, pickLcnExtCase4.ActiveOrmtCount);
        }


    }
}
