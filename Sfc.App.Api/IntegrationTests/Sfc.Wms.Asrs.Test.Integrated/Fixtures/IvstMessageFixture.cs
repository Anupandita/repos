using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using System.Configuration;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class IvstMessageFixture: DataBaseFixtureForIvst
    {
        protected string BaseUrl = @ConfigurationManager.AppSettings["EmsToWmsUrl"];
        protected Int64 CurrentMsgKeys;
        protected Ivst Parameters;
        protected IRestResponse Response;
        protected long CurrentMsgKey;
        protected string CurrentMsgProcessor;
        protected string IvstUrl;
        protected OracleConnection Db;
        protected void TestInitialize()
        {
            GetDataBeforeApiTrigger();
        }
        protected void ValidIvstUrl()
        {
            IvstUrl = $"{BaseUrl}?{"msgKey"}={CurrentMsgKey}&{"msgProcessor"}={CurrentMsgProcessor}";
        }

        protected void ValidUrl(string url)
        {

        }
        protected void MsgKeyCycleCount()
        {
            CurrentMsgKey = IvstData.InvalidKey;
            CurrentMsgProcessor = DefaultPossibleValue.MessageProcessor;
        }
        protected void MsgKeyForUnexpectedOverageException()
        {
            CurrentMsgKey = IvstData.Key;
            CurrentMsgProcessor = EmsToWmsParameters.Process;
        }
        
        protected void MsgKeyForInventoryShortageForInboundPalletIsN()
        {
            CurrentMsgKey = InvShortageOutbound.Key;
            CurrentMsgProcessor = EmsToWmsParametersInventoryShortage.Process;
        }

        protected void MsgKeyForInventoryShortageException()
        {
            CurrentMsgKey = InvShortageInbound.Key;
            CurrentMsgProcessor = EmsToWmsParametersInventoryShortage.Process;
        }
        protected void MsgKeyForDamageException()
        {
            CurrentMsgKey = DamageInbound.Key;
            CurrentMsgProcessor = EmsToWmsParametersDamage.Process;
        }
        protected void MsgKeyForDamageOutbound()
        {
            CurrentMsgKey = DamageOutbound.Key;
            CurrentMsgProcessor = EmsToWmsParametersDamage.Process;
        }
        protected void MsgKeyForWrongSkuException()
        {
            CurrentMsgKey = WrongSku.Key;
            CurrentMsgProcessor = EmsToWmsParametersWrongSku.Process;
        }
        protected void MsgKeyForWrongSkuInboundIsN()
        {
            CurrentMsgKey = WrongSkuOutbound.Key;
            CurrentMsgProcessor = EmsToWmsParametersWrongSku.Process;
        }
        protected void MsgKeyForCycleCountAdjustmentPlus()
        {
            CurrentMsgKey = CycleCountAdjustmentPlus.Key;
            CurrentMsgProcessor = EmsToWmsParametersCycleCount.Process;
        }
        protected void MsgKeyForCycleCountAdjustmentMinus()
        {
            CurrentMsgKey = CycleCountAdjustmentMinus.Key;
            CurrentMsgProcessor = EmsToWmsParametersCycleCount.Process;
        }
        protected void TestDataForUnexpectedOverageException()
        {
            InsertIvstMessagetUnexpectedFunction();
        }
        protected void TestDataForInventoryException()
        {
            InsertIvstMessageForInventoryShortageInboundPalletIsY();
        }
        protected void TestDataForInventoryShortageInboundPalletIsN()
        {
            InsertIvstMessageForInventoryShortageInboundPalletIsN();
        }

        protected void TestDataForDamageException()
        {
            InsertIvstMessageDamageFunction();
        }

        protected void TestDataForDamageInboundPalletIsN()
        {
            InsertIvstMessageDamageForInboundPalletIsN();
        }


        protected void TestDataForWrongSkuException()
        {
            InsertIvstMessageWrongSkuFunction();
        }

        protected void TestDataForWrongSkuInboundPalletIsN()
        {
            InsertIvstMessageForWrongSkuInboundPalletIsN();
        }

        protected void TestDataForCycleCountAdjustmentPlus()
        {
            InsertIvstMessageForCycleCountWithAdjustmentPlus();
        }
        protected void TestDataForCycleCountAdjustmentMinus()
        {
            InsertIvstMessageForCycleCountWithAdjustmentMinus();
        }

        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger(CurrentMsgKey);
        }
        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(IvstUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Contents.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected void IvstApiIsCalledCreatedIsReturned()
        {
            var result = IvstResult();
            Assert.AreEqual(ResultTypes.Created.ToString(), result.ResultType.ToString());
        }
        protected BaseResult IvstResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheUnExceptedOverage()
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(IvstData.Key, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(IvstActionCode.AdjustmentPlus, Ivst.ActionCode);
            Assert.AreEqual(IvstException.UnexpectedOverage, Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortage(long messageKey)
        {
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(IvstActionCode.AdjustmentMinus, Ivst.ActionCode);
            Assert.AreEqual(IvstException.InventoryShortage, Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }
      
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForDamage(long messageKey)
        {
            Assert.AreEqual(EmsToWmsParametersDamage.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersDamage.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersDamage.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersDamage.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersDamage.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(IvstActionCode.AdjustmentMinus, Ivst.ActionCode);
            Assert.AreEqual(IvstException.Damage, Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheWrongSku(long messageKey)
        {
            Assert.AreEqual(EmsToWmsParametersWrongSku.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersWrongSku.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersWrongSku.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersWrongSku.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersWrongSku.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(IvstActionCode.AdjustmentMinus, Ivst.ActionCode);
            Assert.AreEqual(IvstException.WrongSku, Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCount(long messageKey, string actionCode)
        {
            Assert.AreEqual(EmsToWmsParametersCycleCount.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersCycleCount.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersCycleCount.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersCycleCount.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersCycleCount.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(actionCode,Ivst.ActionCode);
            Assert.AreEqual(IvstException.CycleCount,Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId,Ivst.Sku);
        }

        protected void VerifyCycleCountMessage()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Ivst.Quantity), PickLocnDtlAfterApi.ActualInventoryQuantity);
        }

        protected void VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus()
        {
            Assert.AreEqual("CC", Pixtran.ReasonCode);
        }
        protected void VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits + Convert.ToDecimal(Ivst.Quantity) , TrnsInvAfterApi.ActualInventoryUnits);
        }
        protected void VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForUnexpectedOverageException()
        {
            Assert.AreEqual("CO", Pixtran.ReasonCode);
        }

        protected void VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToDecimal(TrnsInvBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity)), Convert.ToDecimal(TrnsInvAfterApi.ActualWeight));
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity,PickLocnDtlAfterApi.ActualInventoryQuantity);
        }

        protected void VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity -Convert.ToDecimal(Ivst.Quantity),PickLocnDtlAfterApi.ActualInventoryQuantity);
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits ,TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(TrnsInvBeforeApi.ActualWeight,TrnsInvAfterApi.ActualWeight);
        }

        protected void VerifyTheQuantityHasIncreasedInPickLocationForCycleCountAdjustmentPlus()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Ivst.Quantity), PickLocnDtlAfterApi.ActualInventoryQuantity);
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits, TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(TrnsInvBeforeApi.ActualWeight, TrnsInvAfterApi.ActualWeight);
        }


        protected void VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForInventoryShortageException()
        {
            Assert.AreEqual("CS", Pixtran.ReasonCode);
        }

       
        protected void VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForDamageException()
        {
            Assert.AreEqual("DG", Pixtran.ReasonCode);
        }
       
        protected void VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForWrongSkuException()
        {
            Assert.AreEqual("CC", Pixtran.ReasonCode);
        }
    }
}
