using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using System;
using System.Configuration;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class OrstMessageFixture : DataBaseFixtureForOrst
    {
        protected Int64 currentMsgKey;
        protected string CostUrl = @ConfigurationManager.AppSettings["EmsToWmsUrl"];
        protected IRestResponse Response;

        protected void InitializeTestData()
        {
            GetDataBeforeTriggerOrst();
        }

        protected void TestDataForActionCodeComplete()
        {
            GetDataBeforeCallingApiForActionCodeComplete();
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

        protected void MsgKeyForCase1()
        {
            currentMsgKey = msgKeyForCase1.MsgKey;
        }

        protected void MsgKeyForCase2()
        {
            currentMsgKey = msgKeyForCase2.MsgKey;
        }

        protected void MsgKeyForCase3()
        {
            currentMsgKey = msgKeyForCase3.MsgKey;
        }

        protected void MsgKeyForCase4()
        {
            currentMsgKey = msgKeyForCase4.MsgKey;
        }

        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(CostUrl);
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
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
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
            Assert.AreEqual(emsToWmsCase1.Process, swmFromMheCase1.SourceMessageProcess);
            Assert.AreEqual(emsToWmsCase1.MessageKey, swmFromMheCase1.SourceMessageKey);
            Assert.AreEqual(emsToWmsCase1.Status, swmFromMheCase1.SourceMessageStatus);
            Assert.AreEqual(emsToWmsCase1.Transaction, swmFromMheCase1.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsCase1.ResponseCode, swmFromMheCase1.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsCase1.MessageText, swmFromMheCase1.SourceMessageText);            
            Assert.AreEqual("ORST", orstCase1.TransactionCode);
            Assert.AreEqual("00255", orstCase1.MessageLength);
            Assert.AreEqual("Allocated", orstCase1.ActionCode);        
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete()
        {
            Assert.AreEqual(emsToWmsCase2.Process, swmFromMheCase2.SourceMessageProcess);
            Assert.AreEqual(emsToWmsCase2.MessageKey, swmFromMheCase2.SourceMessageKey);
            Assert.AreEqual(emsToWmsCase2.Status, swmFromMheCase2.SourceMessageStatus);
            Assert.AreEqual(emsToWmsCase2.Transaction, swmFromMheCase2.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsCase2.ResponseCode, swmFromMheCase2.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsCase2.MessageText, swmFromMheCase2.SourceMessageText);
            Assert.AreEqual("ORST", orstCase1.TransactionCode);
            Assert.AreEqual("00370", orstCase1.MessageLength);
            Assert.AreEqual("Complete", orstCase1.ActionCode);
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeDeAllocate()
        {
            Assert.AreEqual(emsToWmsCase4.Process, swmFromMheCase4.SourceMessageProcess);
            Assert.AreEqual(emsToWmsCase4.MessageKey, swmFromMheCase4.SourceMessageKey);
            Assert.AreEqual(emsToWmsCase4.Status, swmFromMheCase4.SourceMessageStatus);
            Assert.AreEqual(emsToWmsCase4.Transaction, swmFromMheCase4.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsCase4.ResponseCode, swmFromMheCase4.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsCase4.MessageText, swmFromMheCase4.SourceMessageText);
            Assert.AreEqual(DefaultValues.ContainerType, swmFromMheCase4.ContainerType);
            Assert.AreEqual(TransactionCode.Orst, orstCase4.TransactionCode);
            Assert.AreEqual("00370", orstCase4.MessageLength);
            Assert.AreEqual("Deallocate", orstCase4.ActionCode);
        }

        protected void VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeCancel()
        {
            Assert.AreEqual(emsToWmsCase3.Process, swmFromMheCase3.SourceMessageProcess);
            Assert.AreEqual(emsToWmsCase3.MessageKey, swmFromMheCase3.SourceMessageKey);
            Assert.AreEqual(emsToWmsCase3.Status, swmFromMheCase3.SourceMessageStatus);
            Assert.AreEqual(emsToWmsCase3.Transaction, swmFromMheCase3.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsCase3.ResponseCode, swmFromMheCase3.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsCase3.MessageText, swmFromMheCase3.SourceMessageText);
            Assert.AreEqual(DefaultValues.ContainerType, swmFromMheCase3.ContainerType);
            Assert.AreEqual(TransactionCode.Orst, orstCase3.TransactionCode);
            Assert.AreEqual("00370", orstCase3.MessageLength);
            Assert.AreEqual("Canceled", orstCase3.ActionCode);

        }

        protected void VerifyPickTicketStatusHasChangedToInPickingForActionCodeAllocated()
        {
            Assert.AreEqual(35, pickTktHdrCase1.PickTicketStatusCode);
        }

        protected void VerifyCartonStatusHasChangedToInPackingForActionCodeAllocated()
        {
            Assert.AreEqual(15, cartonHdrCase1.StatusCode);
            Assert.AreEqual(case1.CurrentLocationId ,orstCase1.CurrentLocationId);
            Assert.AreEqual(case1.DestLocnId + "-" +case1.ShipWCtrlNbr, orstCase1.DestinationLocationId);
        }


        protected void VerifyCartonStatusHasChangedToPickedForActionCodeComplete()
        {
            Assert.AreEqual("30",cartonHdrCase2.StatusCode);
            
        }

        protected  void ValidateForQuantitiesInTocartonDetailTableForActionCodeComplete()
        {
            Assert.AreEqual(cartonDtlCase2BeforeApi.UnitsPacked + Convert.ToDecimal(orstCase2.QuantityDelivered), cartonDtlCase2AfterApi.UnitsPacked);
            Assert.AreEqual(cartonDtlCase2BeforeApi.ToBePackedUnits - Convert.ToDecimal(orstCase2.QuantityDelivered), cartonDtlCase2AfterApi.ToBePackedUnits);
        }

        protected void ValidateForQuantitiesInToPickTicketDetailTableForActionCodeComplete()
        {
            Assert.AreEqual(pickTktDtlCase2BeforeApi.UnitsPacked + orstCase2.QuantityDelivered, pickTktDtlCase2AfterApi.UnitsPacked);
            Assert.AreEqual(pickTktDtlCase2BeforeApi.VerifiedAsPacked + orstCase2.QuantityDelivered, pickTktDtlCase2AfterApi.VerifiedAsPacked);
        }

        protected void VerifyPickTicketStatusHasChangedToWeighedForStatusCodeComplete()
        {
            Assert.AreEqual("50", pickTktHdrCase2.PickTicketStatusCode);
        }


        protected void VerifyAllocationStatusHasChangedToCompleteForActionCodeComplete()
        {
            Assert.AreEqual(allocInvnDtlCase2BeforeApi.QtyPulled + orstCase2.QuantityDelivered, allocInvnDtlCase2AfterApi.QtyPulled);
            Assert.AreEqual("90", allocInvnDtlCase2AfterApi.StatCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeComplete()
        {
            Assert.AreEqual(pickLcnCase2BeforeApi.ActualInventoryQuantity - Convert.ToDecimal(orstCase2.QuantityDelivered),pickLcnCase2.ActualInventoryQuantity);
            Assert.AreEqual(pickLcnCase2BeforeApi.ToBePickedQty - Convert.ToDecimal(orstCase2.QuantityDelivered), pickLcnCase2.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeComplete()
        {
            Assert.AreEqual(pickLcnExtCase2BeforeApi.ActiveOrmtCount - 1, pickLcnExtCase2.ActiveOrmtCount);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeDeAllocate()
        {
            Assert.AreEqual("5", cartonHdrCase3.StatusCode);
        }

        protected void VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeCancel()
        {
            Assert.AreEqual("5", cartonHdrCase4.StatusCode);
        }

        protected void ValidateForQuantitiesInToPickLocationTableForActionCodeCancel()
        {
            Assert.AreEqual(pickLcnCase4BeforeApi.ToBePickedQty - Convert.ToDecimal(orstCase4.QuantityDelivered), pickLcnCase4.ToBePickedQty);
        }

        protected void ValidateForOrmtCountHasReducedForActionCodeCancel()
        {
            Assert.AreEqual(pickLcnExtCase4BeforeApi.ActiveOrmtCount - 1, pickLcnExtCase4.ActiveOrmtCount);
        }


    }
}
