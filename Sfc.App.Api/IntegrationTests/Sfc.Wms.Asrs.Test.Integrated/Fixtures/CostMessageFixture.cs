﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using RestSharp;
using Newtonsoft.Json;
using DefaultPossibleValue = Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Result;
using System.Configuration;
using Sfc.Wms.InboundLpn.Contracts.Dtos;

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
            GetDataForNegativeCases();
        }
        protected void SetCurrentMsgKey()
        {
            currentMsgKey = emsToWmsParameters.MessageKey;
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

        protected void SetForPickLocationNotExistKey()
        {
            currentMsgKey = costData.PickLocationNotExistKey;
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
            Assert.AreEqual("NotFound",result.ResultType.ToString());
            Assert.AreEqual(0,result.ValidationMessages.Count);
        }

        protected void ValidateResultForInvalidCaseNumber()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("NotFound", result.ResultType.ToString());
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
        }

        protected void ValidateResultForPickLocationNotExist()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());         
            Assert.AreEqual("PickLocationDtl", result.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not Found", result.ValidationMessages[0].Message);
        }


        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        
        protected void VerifyCostMessageWasInsertedIntoSwmFromMhe()
        {
            Assert.AreEqual(emsToWmsParameters.Process, swmFromMhe.SourceMessageProcess);
            Assert.AreEqual(emsToWmsParameters.MessageKey, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWmsParameters.Status,swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWmsParameters.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsParameters.ResponseCode,swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsParameters.MessageText, swmFromMhe.SourceMessageText);            
            Assert.AreEqual("Case", swmFromMhe.ContainerType);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Cost, cost.TransactionCode);
            Assert.AreEqual("268", cost.MessageLength);
            Assert.AreEqual("Arrival", cost.ActionCode);           
            Assert.AreEqual(caseHdr1.PoNumber, swmFromMhe.PoNumber);
        }

        protected void VerifyTheQuantityWasDecreasedInToTransInventory()
        {
            Assert.AreEqual(trnInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(cost.StorageClassAttribute2), trnInvAfterApi.ActualInventoryUnits);
            //Assert.AreEqual(trn3.ActualWeight - (unitweight1 * Convert.ToDecimal(cost.StorageClassAttribute2)), transInvn.ActualWeight);
        }

        protected void VerifyTheQuantityWasIncreasedIntoPickLocationTable()
        {
            Assert.AreEqual(pickLcnDtl.ActualInventoryQuantity + Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtl.ActualInventoryQuantity);
            Assert.AreEqual(pickLocnDtl.ActualInventoryQuantity - Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtl.ToBeFilledQty);
        }
    }
}
