using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using System.Configuration;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class IvstMessageFixture: DataBaseFixtureForIvst
    {
        protected string IvstUrl = @ConfigurationManager.AppSettings["EmsToWmsUrl"];
        protected Int64 CurrentMsgKeys;
        protected Ivst Parameters;
        protected IRestResponse Response;
        protected long CurrentMsgKey;
        protected OracleConnection Db;
        protected void TestInitialize()
        {
            GetDataBeforeApiTrigger();
        }
        protected void MsgKeyCycleCount()
        {
            CurrentMsgKey = IvstData.InvalidKey;
        }
        protected void MsgKeyForUnexpectedOverageException()
        {
            CurrentMsgKey = IvstData.Key;
        }

        protected void MsgKeyForInventoryShortageException()
        {
            CurrentMsgKey = Invshort.Key;
        }

        protected void MsgKeyForDamageException()
        {
            CurrentMsgKey = Damage.Key;
        }
        protected void MsgKeyForWrongSkuException()
        {
            CurrentMsgKey = WrongSku.Key;
        }

        protected void TestDataForUnexpectedOverageException()
        {
            InsertIvstMessagetUnexpectedFunction();
        }
        protected void TestDataForInventoryException()
        {
            InsertIvstMessageInventoryFunction();
        }
        protected void TestDataForDamageException()
        {
            InsertIvstMessageDamageFunction();
        }

        protected void TestDataForWrongSkuException()
        {
            InsertIvstMessageWrongSkuFunction();
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
            request.AddJsonBody(CurrentMsgKey);
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
            Assert.AreEqual(Ivst.ContainerId, SwmFromMhe.ContainerId);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheInventoryShortage()
        {
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(Invshort.Key, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersInventoryShortage.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, SwmFromMhe.ContainerId);

        }
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheDamage()
        {
            Assert.AreEqual(EmsToWmsParametersDamage.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(Damage.Key, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersDamage.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersDamage.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersDamage.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersDamage.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, SwmFromMhe.ContainerId);

        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheWrongSku()
        {
            Assert.AreEqual(EmsToWmsParametersWrongSku.Process, SwmFromMhe.SourceMessageProcess);
            Assert.AreEqual(WrongSku.Key, SwmFromMhe.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParametersWrongSku.Status, SwmFromMhe.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParametersWrongSku.Transaction, SwmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParametersWrongSku.ResponseCode, SwmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParametersWrongSku.MessageText, SwmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, SwmFromMhe.ContainerId);

        }
        protected void VerifyCycleCountMessage()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Ivst.Quantity), PickLocnDtlAfterApi.ActualInventoryQuantity);

        }

        protected void PixTransactionValidationForCycleCountAdjustmentPlus()
        {
            Assert.AreEqual("CC", Pixtran.ReasonCode);
        }
        protected void VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits + Convert.ToDecimal(Ivst.Quantity) , TrnsInvAfterApi.ActualInventoryUnits);

        }
        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForUnexpectedOverageException()
        {
            Assert.AreEqual("CO", Pixtran.ReasonCode);
        }

        protected void VerifyTheQuantityForInventoryShortageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToDecimal(TrnsInvBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity)), Convert.ToDecimal(TrnsInvAfterApi.ActualWeight));
        }

        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForInventoryShortageException()
        {
            Assert.AreEqual("CS", Pixtran.ReasonCode);
        }

        protected void VerifyTheQuantityForDamageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToDecimal(TrnsInvBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity) ), Convert.ToDecimal(TrnsInvAfterApi.ActualWeight));
        }
        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForDamageException()
        {
            Assert.AreEqual("DG", Pixtran.ReasonCode);
        }
        protected void VerifyTheQuantityForWrongSkuExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(TrnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), TrnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToDecimal(TrnsInvBeforeApi.ActualWeight) - (UnitWeight * Convert.ToDecimal(Ivst.Quantity) ), Convert.ToDecimal(TrnsInvAfterApi.ActualWeight));
        }
        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForWrongSkuException()
        {
            Assert.AreEqual("CC", Pixtran.ReasonCode);
        }

    }
}
