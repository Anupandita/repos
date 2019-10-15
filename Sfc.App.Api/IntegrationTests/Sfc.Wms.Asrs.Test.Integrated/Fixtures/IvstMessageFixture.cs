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
        protected string IvstUrl = @ConfigurationManager.AppSettings["IvstUrl"];
        protected Int64 currentMsgKey;
        protected Ivst Parameters;
        protected IRestResponse Response;
        protected long CurrentMsgKey;
        protected OracleConnection db;


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
            GetDataAfterTrigger();
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
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            return result;
        }
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheUnExceptedOverage()
        {
            Assert.AreEqual(emsToWmsParameters.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(IvstData.Key, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWmsParameters.Status, swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWmsParameters.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsParameters.ResponseCode, swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsParameters.MessageText, swmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, swmFromMhe.ContainerId);
        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheInventoryShortage()
        {
            Assert.AreEqual(emsToWmsParametersInventoryShortage.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(Invshort.Key, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWmsParametersInventoryShortage.Status, swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWmsParametersInventoryShortage.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsParametersInventoryShortage.ResponseCode, swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsParametersInventoryShortage.MessageText, swmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, swmFromMhe.ContainerId);

        }
        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheDamage()
        {
            Assert.AreEqual(emsToWmsParametersDamage.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(Damage.Key, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWmsParametersDamage.Status, swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWmsParametersDamage.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsParametersDamage.ResponseCode, swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsParametersDamage.MessageText, swmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, swmFromMhe.ContainerId);

        }

        protected void VerifyIvstMessageWasInsertedIntoSwmFromMheWrongSku()
        {
            Assert.AreEqual(emsToWmsParametersWrongSku.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(WrongSku.Key, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWmsParametersWrongSku.Status, swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWmsParametersWrongSku.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsParametersWrongSku.ResponseCode, swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsParametersWrongSku.MessageText, swmFromMhe.SourceMessageText);
            Assert.AreEqual(Ivst.ContainerId, swmFromMhe.ContainerId);

        }
        protected void VerifyCycleCountMessage()
        {
            Assert.AreEqual(pickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Ivst.Quantity), pickLocnDtlAfterApi.ActualInventoryQuantity);

        }

        protected void PixTransactionValidationForCycleCountAdjustmentPlus()
        {
            Assert.AreEqual("CC", pixtran.ReasonCode);
        }
        protected void VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(trnsInvBeforeApi.ActualInventoryUnits + Convert.ToDecimal(Ivst.Quantity), trnsInvAfterApi.ActualInventoryUnits);

        }
        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForUnexpectedOverageException()
        {
            Assert.AreEqual("CO", pixtran.ReasonCode);
        }

        protected void VerifyTheQuantityForInventoryShortageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(trnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), trnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToInt16(trnsInvBeforeApi.ActualWeight) - (unitWeight * Convert.ToDecimal(Ivst.Quantity)), Convert.ToDecimal(trnsInvAfterApi.ActualWeight));
        }

        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForInventoryShortageException()
        {
            Assert.AreEqual("CS", pixtran.ReasonCode);
        }

        protected void VerifyTheQuantityForDamageExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(trnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), trnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToInt16(trnsInvBeforeApi.ActualWeight) - (unitWeight * Convert.ToDecimal(Ivst.Quantity)), Math.Round(Convert.ToDecimal(trnsInvAfterApi.ActualWeight)));
        }
        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForDamageException()
        {
            Assert.AreEqual("DG", pixtran.ReasonCode);
        }
        protected void VerifyTheQuantityForWrongSkuExceptionIntoTransInventoryTable()
        {
            Assert.AreEqual(trnsInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(Ivst.Quantity), trnsInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(Convert.ToInt16(trnsInvBeforeApi.ActualWeight) - (unitWeight * Convert.ToDecimal(Ivst.Quantity)), Convert.ToDecimal(trnsInvAfterApi.ActualWeight));
        }
        protected void VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForWrongSkuException()
        {
            Assert.AreEqual("CC", pixtran.ReasonCode);
        }

    }
}
