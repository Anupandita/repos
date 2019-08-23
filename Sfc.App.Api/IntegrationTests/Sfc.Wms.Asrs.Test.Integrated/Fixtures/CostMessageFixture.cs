using System;
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
        protected void AValidMsgKey()
        {
            currentMsgKey = costData.MsgKey;
        }
        protected void InvalidMsgKey()
        {
            currentMsgKey = 5;
        }
       
        protected void InvalidCaseMsgKey()
        {
            currentMsgKey = costData.InvalidKey;
        }
        
        protected void TransInvnNotExistsMsgKey()
        {
            currentMsgKey = costDataForTransInvnNotExist.MsgKey;
        }

        protected void PickLocationNotExistKey()
        {
            currentMsgKey = costDataForPickLocnNotExist.MsgKey;
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

        protected BaseResult CostResult()
        {
            var response = CostApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            return result;
        }

        protected void CostApiIsCalledWithValidMsgKey()
        {
            var result = CostResult();
            Assert.AreEqual("Created", result.ResultType.ToString());
        }

        protected void ValidateResultForInvalidMessageKey()
        {
            var result = CostResult();
            Assert.AreEqual("NotFound",result.ResultType.ToString());
            Assert.AreEqual(0,result.ValidationMessages.Count);
        }

        protected void ValidateResultForInvalidCaseNumber()
        {
            var result = CostResult();
            Assert.AreEqual("NotFound", result.ResultType.ToString());
        }

        protected void ValidateResultForTransInventoryNotExist()
        {
            var result = CostResult();
        }

        protected void ValidateResultForPickLocationNotExist()
        {
            var result = CostResult();
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
            Assert.AreEqual(costData.MsgKey, swmFromMhe.SourceMessageKey);
            Assert.AreEqual(emsToWmsParameters.Status,swmFromMhe.SourceMessageStatus);
            Assert.AreEqual(emsToWmsParameters.Transaction, swmFromMhe.SourceMessageTransactionCode);
            Assert.AreEqual(emsToWmsParameters.ResponseCode,swmFromMhe.SourceMessageResponseCode);
            Assert.AreEqual(emsToWmsParameters.MessageText, swmFromMhe.SourceMessageText);            
            Assert.AreEqual("Case", swmFromMhe.ContainerType);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Cost, cost.TransactionCode);
            Assert.AreEqual("268", cost.MessageLength);
            Assert.AreEqual("Arrival", cost.ActionCode);
            Assert.AreEqual(caseHeaderDto.PoNumber, swmFromMhe.PoNumber);
        }
        
        protected void VerifyTheQuantityWasDecreasedInToTransInventory()
        {
            Assert.AreEqual(trnInvBeforeApi.ActualInventoryUnits - Convert.ToDecimal(cost.StorageClassAttribute2), trnInvAfterApi.ActualInventoryUnits);
            //* the bellow line of code is tested after new build release.
            //Assert.AreEqual(trn3.ActualWeight - (unitweight1 * Convert.ToDecimal(cost.StorageClassAttribute2)), transInvn.ActualWeight);
        }

        protected void VerifyTheQuantityWasIncreasedIntoPickLocationTable()
        {
            Assert.AreEqual(pickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtlAfterApi.ActualInventoryQuantity);
            Assert.AreEqual(pickLocnDtlAfterApi.ActualInventoryQuantity - Convert.ToDecimal(cost.StorageClassAttribute2), pickLocnDtlAfterApi.ToBeFilledQty);
        }
    }
}
