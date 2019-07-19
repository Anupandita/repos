﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using RestSharp;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Newtonsoft.Json;
using DefaultPossibleValue = Sfc.Wms.DematicMessage.Contracts.Constants;
using Sfc.Wms.Result;
using Sfc.Wms.Asrs.Test.Integrated.Fixtures;
using System.Diagnostics;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class CostMessageFixture : DataBaseFixtureForCost
    {
        protected string currentCaseNbr;
        protected string CostUrl = "http://localhost:59351/api/cost";
        protected CaseDetailDto caseDetailDto;
        protected CostParams CostParameters;
        protected IRestResponse Response;
        private dynamic testResult;
        protected Int64 currentMsgKey;

        protected void GetValidDataBeforeTrigger()
        {
            GetDataBeforeTrigger();
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
            currentMsgKey = invalidMsgTextKey;
        }
        protected void SetForInvalidCaseMsgKey()
        {
            currentMsgKey = invalidCaseNumberKey;
        }
        protected void SetForInvalidCaseStatusMsgKey()
        {
            currentMsgKey = invalidStsKey;
        }
        protected void SetForTransInvnNotExistsMsgKey()
        {
            currentMsgKey = transInvnNotExistKey;
        }

        protected void AValidNewCostMessageRecord()
        {
            CostParameters = new CostParams
            {
                MsgKey = currentMsgKey
            };
        }

        protected void VerifyMessageKeyExistsInEmsToWms()
        {
            Assert.AreEqual(swmToMhe.SourceMessageKey, emsToWms.MessageKey);
        }

        protected void VerifyStatusIsReadyInEmsToWms()
        {
            Assert.AreEqual("Ready",emsToWms.Status);
        }

        protected IRestResponse CostApiIsCalled()
        {
            var client = new RestClient(CostUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(CostParameters);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);           
            return Response;
        }

        protected void ResultCreatedIsReturned()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Created", result.ResultType);
        }

        protected void ResultForInvalidMessageKey()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Invalid msgKey",result.ValidationMessages.ToString());
        }

        protected void ResultForInvalidMessageText()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Invalid Message Format", result.ValidationMessages.ToString());
        }

        protected void ResultForInvalidCaseNumber()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("No Case Found", result.ValidationMessages.ToString());
        }

        protected void ResultForInvalidCaseStatus()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Invalid Case Status", result.ValidationMessages.ToString());
        }

        protected void ResultForTransInventoryNotExist()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Not Enough Inv", result.ValidationMessages.ToString());
        }

        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        
        protected void VerifyCostMessageWasInsertedIntoSwmFromMhe()
        {
            Assert.AreEqual(emsToWms.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(emsToWms.MessageKey,swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWms.Status,swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWms.Transaction,swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWms.ResponseCode,swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWms.AddWho,swmFromMhe.SourceMessageUpdatedBy);
            Assert.AreEqual(emsToWms.AddDate,swmFromMhe.SourceMessageCreatedDateTime);
            Assert.AreEqual(emsToWms.ProcessedDate, swmFromMhe.SourceMessageUpdatedDateTime);
            Assert.AreEqual(emsToWms.MessageText,swmFromMhe.SourceMessageText);
            Assert.AreEqual(swmToMhe.ContainerId, swmFromMhe.ContainerId);
            Assert.AreEqual("Case", swmFromMhe.ContainerType);          
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Comt, cost.TransactionCode);
            Assert.AreEqual(DefaultPossibleValue.MessageLength.Comt, cost.MessageLength);
            Assert.AreEqual("Arrival", cost.ActionCode);
            Assert.AreEqual(swmToMhe.ContainerId, swmFromMhe.ContainerId);
            Assert.AreEqual(caseHdr.PoNumber,swmFromMhe.PoNumber);
        }

        protected void VerifyTheQuantityIsDecreasedToTransInventory()
        {
            Assert.AreEqual(trn.ActualInventoryUnits - Convert.ToDecimal(cost.StorageClassAttribute2), transInvn.ActualInventoryUnits);
            Assert.AreEqual(trn.ActualInventoryUnits - (unitWeight * Convert.ToDecimal(cost.StorageClassAttribute2)), transInvn.ActualWeight);
        }

        protected void VerifyTheQuantityIsIncreasedIntoPickLocationTable()
        {
            Assert.AreEqual(pickLcnDtl.ActualQty + Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtl.ActualQty);
            Assert.AreEqual(pickLcnDtl.ToBeFilledQty - Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtl.ToBeFilledQty);
            Assert.AreEqual("COSTProcessor", pickLocnDtl.UserId);     
        }

    }
}
