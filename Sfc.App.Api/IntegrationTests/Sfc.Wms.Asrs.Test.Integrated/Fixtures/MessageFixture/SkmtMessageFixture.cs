using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Newtonsoft.Json;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using ValidationMessage = Sfc.Wms.Api.Asrs.Test.Integrated.TestData.ValidationMessage;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class SkmtMessageFixture :DataBaseFixtureForSkmt
    {
        protected string CurrentSkuId;
        protected string CurrentActionCode;
        protected string InvalidSkuId;
        protected SkmtParams SkmtParameters;
        protected BaseResult Result;
        protected IRestResponse Response;
        protected BaseResult Negativecase;
        protected string SkmtUrl;
        protected string BaseUrl =ConfigurationManager.AppSettings["BaseUrl"]; 

        protected void InitializeTestData()
        {
            GetDataBeforeTriggerSkmt();
        }
        protected void InitializeTestDataParent()
        {
            GetDataBeforeTriggerSkmtParent();
        }
        protected void InitializeTestDataChild()
        {
            GetDataBeforeTriggerSkmtChildSku();
        }
        
        protected IRestResponse ApiIsCalled(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            Response = client.Execute(request);
            return Response;
        }
        protected void ValidSkuActioncodeAndSkmtUrlIs(string currentSkuId,string currentActionCode,string url)
        {
            SkmtUrl = $"{BaseUrl}{TestData.Parameter.SkuMaintenance}?{TestData.Parameter.ActionCode}={currentActionCode}&{TestData.Parameter.SkuId}={currentSkuId}";
        }

        protected BaseResult SkmtResult()
        {
            var response = ApiIsCalled(SkmtUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void SkmtApiIsCalledCreatedIsReturned()
        {
            Result = SkmtResult();
            Assert.AreEqual(ResultTypes.Created, Result.ResultType.ToString());
        }

        protected void SkmtApiIsCalledCreatedForNegativeCases()
        {
            Result = SkmtResult();
            Assert.AreEqual(ResultTypes.NotFounds, Result.ResultType.ToString());
        }
        protected void SkmtApiIsCalledForInvalidSkuId()
        {
            Negativecase = SkmtResult();
        }
        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        protected void VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode(string action, string act)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheSkmt.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Skmt, Skmt.TransactionCode);
            Assert.AreEqual(MessageLength.Skmt, Skmt.MessageLength);
            Assert.AreEqual(action, act);
            Assert.AreEqual(ItemMaster.SkuId, Skmt.Sku);
            //Assert.AreEqual(Uom, Skmt.UnitOfMeasure);
        }
        protected void VerifySkmtMessageWasInsertedIntoWmsToEms()
        {
            VerifySkmtMessageWasInsertedIntoWmsToEms(WmsToEmsSkmt);
        }
        protected void VerifySkmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte)
        {  
            Assert.AreEqual(SwmToMheSkmt.SourceMessageKey, wte.MessageKey);
            Assert.AreEqual(SwmToMheSkmt.SourceMessageText, wte.MessageText);
            Assert.AreEqual(SwmToMheSkmt.SourceMessageStatus, wte.Status);
            Assert.AreEqual(SwmToMheSkmt.SourceMessageResponseCode, wte.ResponseCode);
            Assert.AreEqual(TransactionCode.Skmt, wte.Transaction);
        }
        protected void VerifyForSkmtMessageSentTheSkuidWasNormalSku()
        {
            Assert.AreEqual(null, Skmt.ParentSku);
        }

        protected void VerifyForSkmtMessageSentTheSkuidWasParentSku()
        {
            Assert.AreEqual(ParentSku.Colordescription, Skmt.ParentSku);
        }

        protected void VerifyForSkmtMessageSentTheSkuidWasChildSku()
        {
            Assert.AreEqual(ParentSku.SkuId, ChildSku.SkuId);
        }

        protected void ValidateResultForInvalidSkuId()
        {
            Assert.AreEqual(ResultTypes.NotFounds, Negativecase.ResultType.ToString());
            Assert.AreEqual(1, Negativecase.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.ItemMasters, Negativecase.ValidationMessages[0].FieldName);
        }

    } 
}
