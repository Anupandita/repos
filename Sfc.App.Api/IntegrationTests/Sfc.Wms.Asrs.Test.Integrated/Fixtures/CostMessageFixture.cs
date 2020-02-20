using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using RestSharp;
using Newtonsoft.Json;
using System.Configuration;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using ValidationMessage = Sfc.Wms.Api.Asrs.Test.Integrated.TestData.ValidationMessage;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class CostMessageFixture : DataBaseFixtureForCost
    {
        protected string CurrentCaseNbr;
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
        protected string CostUrl;
        protected CaseDetailDto CaseDetailDto;
        protected Cost Parameters;
        protected IRestResponse Response;
        protected BaseResult Negativecase1;
        protected BaseResult Negativecase2;
        protected BaseResult NegativeCase3;
        protected BaseResult NegativeCase4;

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

        protected void ValidCostUrlMsgKeyAndProcessorIs(string url,Int64 currentMsgKey,string currentMsgProcessor)
        {
            CostUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={currentMsgKey}&{TestData.Parameter.MsgProcessor}={currentMsgProcessor}";
        }
        protected IRestResponse ApiIsCalled()
        {
            var client = new RestClient(CostUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);           
            return Response;
        }
        protected BaseResult CostResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void CostApiIsCalledWithValidMsgKey()
        {
            var result = CostResult();
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
        }
        protected void CostApiIsCalledForInvalidMessageKey()
        {
            Negativecase1 = CostResult();       
        }  

        protected void CostApiIsCalledForInvalidCaseNumber()
        {
            Negativecase2 = CostResult();    
        }

        protected void CostApiIsCalledForTransInvnNotFound()
        {
            NegativeCase3 = CostResult();
        }
       
        protected void CostApiIsCalledForPickLocnNotFound()
        {
            NegativeCase4 = CostResult();         
        }
      
        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        
        protected void VerifyCostMessageWasInsertedIntoSwmFromMhe()
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMheDto.SourceMessageProcess);
            Assert.AreEqual(CostData.MsgKey, SwmFromMheDto.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMheDto.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMheDto.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.ResponseCode, SwmFromMheDto.SourceMessageResponseCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMheDto.SourceMessageText);            
            Assert.AreEqual(DefaultValues.ContainerType, SwmFromMheDto.ContainerType);
            Assert.AreEqual(TransactionCode.Cost, Cost.TransactionCode);
            Assert.AreEqual(DefaultValues.MessageLengthCost, Cost.MessageLength);
            Assert.AreEqual(DefaultValues.ActionCodeCost, Cost.ActionCode);
            Assert.AreEqual(CaseHeaderDto.PoNumber, SwmFromMheDto.PoNumber);
        }
        
        protected void VerifyTheQuantityWasDecreasedInToTransInventory()
        {
            Assert.AreEqual(TrnInvBeforeApi.ActualInventoryUnits - (Convert.ToDecimal(Cost.StorageClassAttribute2)/100), TrnInvAfterApi.ActualInventoryUnits);
            Assert.AreEqual(String.Format("{0:0.00}",TrnInvBeforeApi.ActualWeight - (UnitWeight1 * (Convert.ToDecimal(Cost.StorageClassAttribute2)/100))), String.Format("{0:0.00}", TrnInvAfterApi.ActualWeight));
        }

        protected void VerifyTheQuantityWasIncreasedIntoPickLocationTable()
        {
            Assert.AreEqual(PickLcnDtlBeforeApi.ActualInventoryQuantity + (Convert.ToDecimal(Cost.StorageClassAttribute2) / 100), PickLocnDtlAfterApi.ActualInventoryQuantity);
            Assert.AreEqual(PickLcnDtlBeforeApi.ToBeFilledQty - (Convert.ToDecimal(Cost.StorageClassAttribute2) / 100), PickLocnDtlAfterApi.ToBeFilledQty);
        }
        protected void ValidateResultForInvalidMessageKey()
        {
            Assert.AreEqual(ResultType.NotFound, Negativecase1.ResultType.ToString());
            Assert.AreEqual(Constants.ValidationCount, Negativecase1.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.EmsToWms, Negativecase1.ValidationMessages[0].FieldName);
            Assert.AreEqual(ValidationMessage.InvalidMessageKey, Negativecase1.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidCaseNumber()
        {
            Assert.AreEqual(Constants.ValidationCount, Negativecase2.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.EmsToWms, Negativecase2.ValidationMessages[0].FieldName);            
        }

        protected void ValidateResultForTransInventoryNotExist()
        {
            //* not implemented    
        }
        protected void ValidateResultForPickLocnNotFound()
        {
            Assert.AreEqual(Constants.ValidationCount, NegativeCase4.ValidationMessages.Count);
        }
    }
}
