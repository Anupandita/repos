using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using RestSharp;
using Newtonsoft.Json;
using DefaultPossibleValue = Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using System.Configuration;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using ValidationMessage = Sfc.Wms.Api.Asrs.Test.Integrated.TestData.ValidationMessage;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
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
        protected BaseResult negativecase1;
        protected BaseResult negativecase2;
        protected BaseResult negativeCase3;
        protected BaseResult negativeCase4;

        protected void TestInitialize()
        {
            GetValidData();
        }

        protected void TestInitializeForValidMessage()
        {
            GetDataBeforeTrigger();        
        }

        protected void  TestInitializeForInvalidCase()
        {
            InsertCostMessageForInValidCase();
        }

        protected void TestInitializeForTransInvnDoesNotExist()
        {
            InsertCostMessageForTransInvnDoesNotExist();
        }

        protected void TestInitializeForPickLocnDoesNotExist()
        {
            InsertCostMessageForPickLocnDoesNotExist();
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
        protected BaseResult CostResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            return result;
        }
        protected void CostApiIsCalledWithValidMsgKey()
        {
            var result = CostResult();
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
        }
        protected void CostApiIsCalledForInvalidMessageKey()
        {
            negativecase1 = CostResult();       
        }  

        protected void CostApiIsCalledForInvalidCaseNumber()
        {
            negativecase2 = CostResult();    
        }

        protected void CostApiIsCalledForTransInvnNotFound()
        {
            negativeCase3 = CostResult();
        }
       
        protected void CostApiIsCalledForPickLocnNotFound()
        {
            negativeCase4 = CostResult();         
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
            Assert.AreEqual(DefaultValues.ContainerType, swmFromMhe.ContainerType);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Cost, cost.TransactionCode);
            Assert.AreEqual(DefaultValues.MessageLengthCost, cost.MessageLength);
            Assert.AreEqual(DefaultValues.ActionCodeCost, cost.ActionCode);
            Assert.AreEqual(caseHeaderDto.PoNumber, swmFromMhe.PoNumber);
        }
        
        protected void VerifyTheQuantityWasDecreasedInToTransInventory()
        {
            Assert.AreEqual(trnInvBeforeApi.ActualInventoryUnits - (Convert.ToDecimal(cost.StorageClassAttribute2)/100), trnInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(trnInvBeforeApi.ActualWeight - (unitweight1 * (Convert.ToDecimal(cost.StorageClassAttribute2)/100)), trnInvAfterApi.ActualWeight);
        }

        protected void VerifyTheQuantityWasIncreasedIntoPickLocationTable()
        {
            Assert.AreEqual(pickLcnDtlBeforeApi.ActualInventoryQuantity + (Convert.ToDecimal(cost.StorageClassAttribute2) / 100), pickLocnDtlAfterApi.ActualInventoryQuantity);
            Assert.AreEqual(pickLcnDtlBeforeApi.ToBeFilledQty - (Convert.ToDecimal(cost.StorageClassAttribute2) / 100), pickLocnDtlAfterApi.ToBeFilledQty);
        }
        protected void ValidateResultForInvalidMessageKey()
        {
            Assert.AreEqual(ResultType.NotFound, negativecase1.ResultType.ToString());
            Assert.AreEqual(1, negativecase1.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.EmsToWms, negativecase1.ValidationMessages[0].FieldName);
            Assert.AreEqual(ValidationMessage.InvalidMessageKey, negativecase1.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidCaseNumber()
        {
            Assert.AreEqual(2, negativecase2.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.ContainerId, negativecase2.ValidationMessages[0].FieldName);
            Assert.AreEqual(ValidationMessage.Invalid, negativecase2.ValidationMessages[0].Message);
            Assert.AreEqual(ValidationMessage.Sku, negativecase2.ValidationMessages[1].FieldName);
            Assert.AreEqual(ValidationMessage.Invalid, negativecase2.ValidationMessages[1].Message);
        }

        protected void ValidateResultForTransInventoryNotExist()
        {
            //* not implemented    
        }
        protected void ValidateResultForPickLocnNotFound()
        {
            Assert.AreEqual(1, negativeCase4.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.PickLocationDtl, negativeCase4.ValidationMessages[0].FieldName);
            Assert.AreEqual(ValidationMessage.NotFound, negativeCase4.ValidationMessages[0].Message);
        }
    }
}
