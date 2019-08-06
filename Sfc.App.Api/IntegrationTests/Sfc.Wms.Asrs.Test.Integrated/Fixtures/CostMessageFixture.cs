using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using RestSharp;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Newtonsoft.Json;
using DefaultPossibleValue = Sfc.Wms.DematicMessage.Contracts.Constants;
using Sfc.Wms.Result;
using System.Configuration;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class CostMessageFixture : DataBaseFixtureForCost
    {
        protected string currentCaseNbr;
        protected string CostUrl = @ConfigurationManager.AppSettings["CostUrl"];
        protected CaseDetailDto caseDetailDto;
        protected Cost Parameters;
        protected IRestResponse Response;
        protected Int64 currentMsgKey;

        protected void GetValidDataBeforeTrigger()
        {
            GetDataBeforeTrigger();
            NegativeCases();
        }
        protected void SetCurrentMsgKey()
        {
            currentMsgKey = emsToWms.MessageKey;
        }
        protected void SetInvalidMsgKey()
        {
            currentMsgKey = 5;
        }
        protected void SetForInvalidMessageTextMsgKey()
        {
            currentMsgKey = costData.InvalidMsgTextKey;
        }
        protected void SetForInvalidCaseMsgKey()
        {
            currentMsgKey = costData.InvalidCaseNumberKey;
        }
        protected void SetForInvalidCaseStatusMsgKey()
        {
            currentMsgKey = costData.InvalidStsKey;
        }
        protected void SetForTransInvnNotExistsMsgKey()
        {
            currentMsgKey = costData.TransInvnNotExistKey;
        }
        
        protected IRestResponse CostApiIsCalled()
        {
            var client = new RestClient(CostUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(currentMsgKey);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);           
            return Response;
        }

        protected void CostApiIsCalledWithValidMsgKey()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Created", result.ResultType.ToString());
        }

        protected void ValidateResultForInvalidMessageKey()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("!0",result.ValidationMessages.Count);
        }

        protected void ValidateResultForInvalidCaseNumber()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("!0", result.ValidationMessages.Count);
        }

        protected void ValidateResultForInvalidCaseStatus()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("!0", result.ValidationMessages.ToString());
        }

        protected void ValidateResultForTransInventoryNotExist()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("!0", result.ValidationMessages.ToString());
        }
        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        
        protected void VerifyCostMessageWasInsertedIntoSwmFromMhe()
        {
            Assert.AreEqual(emsToWms.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(emsToWms.MessageKey, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWms.Status,swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWms.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWms.ResponseCode,swmFromMhe.SourceMessageResponseCode);
           // Assert.AreEqual(emsToWms.AddWho, swmFromMhe.SourceMessageUpdatedBy);
           // Assert.AreEqual(emsToWms.AddDate, swmFromMhe.SourceMessageCreatedDateTime);
           // Assert.AreEqual(emsToWms.ProcessedDate, swmFromMhe.SourceMessageUpdatedDateTime);
            Assert.AreEqual(emsToWms.MessageText, swmFromMhe.SourceMessageText);
            
            Assert.AreEqual("Case", swmFromMhe.ContainerType);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Cost, cost.TransactionCode);
            Assert.AreEqual("268", cost.MessageLength);
            Assert.AreEqual("Arrival", cost.ActionCode);
           
            Assert.AreEqual(caseHdr.PoNumber, swmFromMhe.PoNumber);
        }

        protected void VerifyTheQuantityWasDecreasedInToTransInventory()
        {
            Assert.AreEqual(trn.ActualInventoryUnits - Convert.ToDecimal(cost.StorageClassAttribute2), transInvn.ActualInventoryUnits);
            //Assert.AreEqual(trn.ActualInventoryUnits - (unitWeight * Convert.ToDecimal(cost.StorageClassAttribute2)), transInvn.ActualWeight);
        }

        protected void VerifyTheQuantityWasIncreasedIntoPickLocationTable()
        {
            Assert.AreEqual(pickLcnDtl.ActualQty + Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtl.ActualQty);
            Assert.AreEqual(pickLcnDtl.ActualQty - Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtl.ToBeFilledQty);
           // Assert.AreEqual("COSTProcessor", pickLocnDtl.UserId);     
        }
    }
}
