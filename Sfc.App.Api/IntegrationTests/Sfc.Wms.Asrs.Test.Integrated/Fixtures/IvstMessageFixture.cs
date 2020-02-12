using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using System.Configuration;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class IvstMessageFixture: DataBaseFixtureForIvst
    {
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
        protected Int64 CurrentMsgKeys;
        protected EmsToWmsDto ems = new EmsToWmsDto();
        protected Ivst Parameters;
        protected IRestResponse Response;
        protected long CurrentMsgKey;
        protected string CurrentMsgProcessor;
        public static string IvstUrl;
        protected OracleConnection Db;
      
        protected void ValidIvstUrlMsgKeyAndMsgProcessorIs(string url,Int64 currentMsgKey,string currentMsgProcessor)
        {
            IvstUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={currentMsgKey}&{TestData.Parameter.MsgProcessor}={currentMsgProcessor}";
        }
       
        protected void TestDataForUnexpectedOverageExceptionScenario1()
        {
            InsertIvstMessageUnexpectedOverageWhenCaseHeaderStatusCodeIsLessthan90AndCaseDtlActlQtyIsGreaterThanZero();
        }

        protected void TestDataForUnexpectedOverageExceptionScenario2()
        {
            IvstMessageUnexpectedOverageWhenCaseHeaderStatusCodeIsLessThanOrEqualTo90AndCaseDtlActlQtyIsEqualToZero();
        }

        protected void TestDataForUnexpectedOverageExceptionScenario3()
        {
            InsertIvstMessagetUnexpectedFunctionForstatus96AndNegativeTiAlreadyExists();
        }

        protected void TestDataForUnexpectedOverageExceptionScenario4()
        {
            InsertIvstMessagetUnexpectedFunctionForstatus96AndNegativeTiNotExists();
        }

        protected void TestDataForInventoryException()
        {
            InsertIvstMessageForInventoryShortageInboundPalletIsY();
        }
        protected void TestDataForInventoryShortageInboundPalletIsN()
        {
            InsertIvstMessageForInventoryShortageInboundPalletIsN();
        }

        protected void TestDataForInventoryShortageScenario3()
        {
            InsertIvstMessageForInventoryShortageOutBoundAndPickLocnDtlQtyIsGreaterThanZeroButLesserThanIvstQty();
        }
        protected void TestDataForInventoryShortageScenario4()
        {
            InsertIvstMessageForInventoryShortageOutBoundAndPickLocnDtlQtyIsGreaterThanZeroButLesserThanIvstQtyForNegativePickDoesNotExists();
        }

        protected void TestDataForInventoryShortageScenario5()
        {
            InsertIvstMessageForInventoryShortageOutBoundAndPickLocnQtyLesserThanOrEqualToZero();
        }
        protected void TestDataForDamageException()
        {
            InsertIvstMessageDamageForInboundPalletIsY();
        }

        protected void TestDataForDamageInboundPalletIsN()
        {
            InsertIvstMessageDamageForInboundPalletIsN();
        }

        protected void TestDataForDamageScenario3()
        {
            InsertIvstMessageDamageForInboundPalletIsYScenario3();
        }

        protected void TestDataForWrongSkuException()
        {
            InsertIvstMessageWrongSkuFunction();
        }     

        protected void TestDataForCycleCountAdjustmentPlus()
        {
            InsertIvstMessageForCycleCountWithAdjustmentPlus();
        }

        protected void TestDataForCcAdjustmentMinusScenario3()
        {
            IvstMessageForCcAdjustmentMinusWherePickLocnDtlActlQtyIsGreaterThanZeroButLessThanIvstQty();
        }

        protected void TestDataForAdjustmentMinusScenario4()
        {
            IvstMessageForCcAdjustMentMinusWherePickLocnDtlActlQtyIsGreaterThanZeroButLessThanIvstQtyAndNegativeTiDoesNotExists();
        }
        protected void TestDataForAdjustmentMinusScenario5()
        {
            IvstMessageForCcAdjustmentMinusWherePickLocnQtyIsLessThanOrEqualToZeroAndLessThanIvstQuantity();
        }

        protected void TestDataForCycleCountAdjustmentMinus()
        {
            IvstMessageForCcAdjustmentMinusWherePickLocnDtlActlQtyIsGreaterThanIvstQty();
        }

        protected void TestDataForMixedInventory()
        {
            InsertIvstMessageForMixedOrIncorrectInventory();
        }

        protected void TestDataForNoException()
        {
            InsertIvstMessageForNoExceptionScenario();
        }
      
        protected void GetValidDataAfterTriggerForKey(long key)
        {
            GetDataAfterTrigger(key);
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
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForUnexceptedOverage(long messageKey)
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(IvstActionCode.AdjustmentPlus, Ivst.ActionCode);
            Assert.AreEqual(IvstException.UnexpectedOverage, Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(long messageKey)
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
      
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForDamageAndMsgKeyShouldBe(long messageKey)
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

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForWrongSkuAndMsgKeyShouldBe(long messageKey)
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
       
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(long messageKey, string actionCode)
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
       
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForMixedInventoryAndMsgKeyShouldBe(long messageKey, string actionCode)
        {
            Assert.AreEqual(EmsToWmsParametersMixedInventory.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersMixedInventory.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersMixedInventory.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersMixedInventory.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersMixedInventory.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(actionCode, Ivst.ActionCode);
            Assert.AreEqual("0009", Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheForNoExceptionAndMsgKeyShouldBe(long messageKey, string actionCode)
        {
            Assert.AreEqual(EmsToWmsParametersNoException.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(messageKey, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersNoException.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersNoException.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersNoException.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersNoException.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(IvstData.CaseNumber, SwmFromMhe.ContainerId);
            Assert.AreEqual(actionCode, Ivst.ActionCode);
            Assert.AreEqual("0000", Ivst.AdjustmentReasonCode);
            Assert.AreEqual(IvstData.SkuId, Ivst.Sku);
        }

        protected void VerifyCycleCountMessage()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Ivst.Quantity), PickLocnDtlAfterApi.ActualInventoryQuantity);
        }

        protected void VerifyForCycleCountNegativePickRecordAlreadyExists()
        {
            Assert.AreEqual(TransInvnNegativePickBeforeApi.ActualInventoryUnits -(Convert.ToDecimal(Ivst.Quantity) - PickLcnDtlBeforeApi.ActualInventoryQuantity), TransInvnNegativePick.ActualInventoryUnits);
            Assert.AreEqual(TransInvnNegativePickBeforeApi.ActualWeight -((Convert.ToDecimal(Ivst.Quantity)- PickLcnDtlBeforeApi.ActualInventoryQuantity)*UnitWeight), TransInvnNegativePick.ActualWeight);
        }

        protected void ValidateForInventoryShortageScenario3()
        {
            Assert.AreEqual(TransInvnNegativePickBeforeApi.ActualInventoryUnits - (Convert.ToDecimal(Ivst.Quantity) - PickLcnDtlBeforeApi.ActualInventoryQuantity), TransInvnNegativePick.ActualInventoryUnits);
        }

        protected void ValidateForInventoryShortageScenario4()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity - Convert.ToDecimal(Ivst.Quantity), TransInvnNegativePick.ActualInventoryUnits);
        }

        protected void ValidateForInventoryShortageScenario5()
        {
            Assert.AreEqual(TransInvnNegativePickBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity),TransInvnNegativePick.ActualInventoryUnits);
        }      

        protected void VerifyQtyReducedToZeroInPickLocnDtlTable()
        {
            Assert.AreEqual("0", PickLocnDtlAfterApi.ActualInventoryQuantity.ToString());
        }

        protected void VerifyForCycleCountWherePickLocnQtyIsLessThanIvstQtyandLessthanOrEqualToZero()
        {
            Assert.AreEqual(TransInvnNegativePickBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity),TransInvnNegativePick.ActualInventoryUnits);
            Assert.AreEqual(String.Format("{0:0.00}", Convert.ToDecimal(TransInvnNegativePickBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity))), string.Format("{0:0.00}", Convert.ToDecimal(TransInvnNegativePick.ActualWeight)));

                 }
        // negative pick already exists.
        protected void ValidationForUnExpectedOverageWhereCaseHdrStatCode96AndNegativePickAlreadyExists()
        {
            Assert.AreEqual(TransInvnNegativePickBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), TransInvnNegativePick.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToDecimal(TransInvnNegativePickBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity)), Convert.ToDecimal(TransInvnNegativePick.ActualWeight));
        }

        // negative pick does not exists.
        protected void ValidationForUnExpectedOverageWhereCaseHdrStatCode96AndNegativePickDoesNotExists()
        {
            Assert.AreEqual(-Convert.ToDecimal(Ivst.Quantity) , TransInvnNegativePick.ActualInventoryUnits);
            Assert.AreEqual(-(UnitWeight * Convert.ToDecimal(Ivst.Quantity)), Convert.ToDecimal(TransInvnNegativePick.ActualWeight));
        }

        protected void VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus(string adjustmentType)
        {
            Assert.AreEqual(Constants.PixRsnCodeForCycleCount, Pixtran.ReasonCode);
            Assert.AreEqual(adjustmentType, Pixtran.InventoryAdjustmentType);
            Assert.AreEqual(Convert.ToDecimal(Ivst.Quantity), Pixtran.InventoryAdjustmentQuantity);
        }

        protected void VerifyTheRecordInsertedInToPixTransactionForCycleCountNegativePickScenario(string adjustmentType)
        {
            Assert.AreEqual(Constants.PixRsnCodeForCycleCount, Pixtran.ReasonCode);
            Assert.AreEqual(adjustmentType, Pixtran.InventoryAdjustmentType);
            Assert.AreEqual(Convert.ToDecimal(PickLcnDtlBeforeApi.ActualInventoryQuantity), Pixtran.InventoryAdjustmentQuantity);
        }

        //common validation for all unexpected overage scenarios.
        protected void VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTableAndPickLocnTable()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits + Convert.ToDecimal(Ivst.Quantity) , TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(String.Format("{0:0.00}", Convert.ToDecimal(TrnsInvBeforeApi.ActualWeight) + (UnitWeight * Convert.ToDecimal(Ivst.Quantity))), String.Format("{0:0.00}", Convert.ToDecimal(TrnsInvAfterApi.ActualWeight)));
            Assert.AreEqual(PickLcnDtlBeforeApi.ToBeFilledQty + Convert.ToDecimal(Ivst.Quantity), PickLocnDtlAfterApi.ToBeFilledQty);
        }

        protected void VerifyQtyForNegativePickWhenInsertedForUnExpectedOverageScenario3()
        {
            Assert.AreEqual(Convert.ToDecimal(Ivst.Quantity) - PickLcnDtlBeforeApi.ActualInventoryQuantity,TransInvnNegativePick.ActualInventoryUnits);
        }
        protected void ValidationForQuuantityInCaseDtlTableForUnExpectedOverageScenario1()
        {        
            Assert.AreEqual(Convert.ToDecimal(IvstData.CaseDtlQty) - Convert.ToDecimal(Ivst.Quantity), caseDtlQty.ActualQuantity);
        }

        protected void ValidationForStatCodeShouldBeUpdatedTo96ForScenario2()
        {
            Assert.AreEqual(96,caseHdrStatCode.StatusCode);
        }

        protected void VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryAndPicklocnForToBeFilledAndrInboundPalletIsY()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), TrnsInvAfterApi.ActualInventoryUnits);            
            Assert.AreEqual(String.Format("{0:0.00}", Convert.ToDecimal(TrnsInvBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity))), string.Format("{0:0.00}",Convert.ToDecimal(TrnsInvAfterApi.ActualWeight)));
            Assert.AreEqual(PickLcnDtlBeforeApi.ToBeFilledQty - Convert.ToDecimal(Ivst.Quantity), PickLocnDtlAfterApi.ToBeFilledQty);
        }

        protected void VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity - Convert.ToDecimal(Ivst.Quantity),PickLocnDtlAfterApi.ActualInventoryQuantity);
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
            Assert.AreEqual(Constants.PixRsnCodeForInventoryShortage, Pixtran.ReasonCode);
            Assert.AreEqual("S", Pixtran.InventoryAdjustmentType);
            Assert.AreEqual(Convert.ToDecimal(Ivst.Quantity), Pixtran.InventoryAdjustmentQuantity);
        }


        protected void VerifyForPixTransactionAndValidateReasonCodeForInventoryShortageExceptionWherePickLocnQtyIsSmallerThanIvstQty()
        {
            Assert.AreEqual(Constants.PixRsnCodeForInventoryShortage, Pixtran.ReasonCode);
            Assert.AreEqual("S", Pixtran.InventoryAdjustmentType);
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity, Pixtran.InventoryAdjustmentQuantity);
        }
        protected void VerifyTheQuantityShouldNotBeChanged()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity , PickLocnDtlAfterApi.ActualInventoryQuantity);
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits, TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(TrnsInvBeforeApi.ActualWeight, TrnsInvAfterApi.ActualWeight);
        }

        protected void VerifyForCaseHdrAndCaseDtlRecordInsertedOrNotForDamageAndWrongSkuExceptions()
        {         
           Assert.AreEqual(Convert.ToDecimal(Ivst.Quantity),caseDtlAfterApi.ActualQuantity);
           Assert.AreEqual(UnitWeight*Convert.ToDecimal(Ivst.Quantity), caseHdrAfterApi.EstimatedWeight);
           Assert.AreEqual(UnitWeight * Convert.ToDecimal(Ivst.Quantity), caseHdrAfterApi.ActualWeight);
           //Assert.AreEqual(Ivst.ContainerId, caseHdrAfterApi.CaseNumber);
           Assert.AreEqual(10, caseHdrAfterApi.StatusCode);
           Assert.AreEqual("Y", caseHdrAfterApi.SingleSkuId);
           Assert.AreEqual("Y", caseHdrAfterApi.SpecialInstructionCode1);
           Assert.AreEqual(Convert.ToDecimal(Ivst.Quantity), caseDtlAfterApi.OriginalQuantity);
           Assert.AreEqual(Convert.ToDecimal(Ivst.Quantity), caseDtlAfterApi.ShippedAsnQuantity);
           Assert.AreEqual(1,caseDtlAfterApi.CaseSequenceNumber);
        }

        protected void VerifyForCorbaHdrDtlRecords()
        {
            for (var j = 0; j < pb.Count; j++)
            {
                Assert.AreEqual("PROCESSED", pb[j].Status);
                Assert.AreEqual("PSI", pb[j].ChgUser);
                Assert.AreEqual("Crtn Hospital", pb[j].WorkStationId);       
            }
        }

        protected void VerifyForParmname()
        {
            Assert.AreEqual("caseNbr",pb[0].ParmName);
            Assert.AreEqual("error", pb[1].ParmName);
            Assert.AreEqual("return", pb[2].ParmName);
        }

        protected void VerifyParamType()
        {
            Assert.AreEqual("in",pb[0].ParmType);
            Assert.AreEqual("out", pb[1].ParmType);
            Assert.AreEqual("out", pb[2].ParmType);
        }

        protected void VerifyParamValue()
        {
           // Assert.AreEqual("", pb[1].ParmValue);
            Assert.AreEqual("", pb[1].ParmValue);
            Assert.AreEqual("PkValid", pb[2].ParmValue);
        }


    }
}
